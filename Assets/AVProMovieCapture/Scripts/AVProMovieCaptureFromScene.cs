#if UNITY_3_5 || UNITY_4_1 || UNITY_4_0_1 || UNITY_4_0 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_5
#define AVPRO_MOVIECAPTURE_GLISSUEEVENT
#endif

using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Text;
using System.Runtime.InteropServices;

//-----------------------------------------------------------------------------
// Copyright 2012-2015 RenderHeads Ltd.  All rights reserved.
//-----------------------------------------------------------------------------

[AddComponentMenu("AVPro Movie Capture/From Scene")]
public class AVProMovieCaptureFromScene : AVProMovieCaptureBase
{	
	public Shader _shaderSwapRedBlue;
	private bool _useNativeGrabber;
	private Texture2D _screenTexture;
	private Material _materialSwapRedBlue;
	private Material _materialConversion;
	
	public override void Start()
	{
		_materialSwapRedBlue = new Material(_shaderSwapRedBlue);
		_materialSwapRedBlue.name = "AVProMovieCapture-Material";

		base.Start();
	}
	
	public override void OnDestroy()
	{
		_materialConversion = null;
		if (_materialSwapRedBlue != null)
		{
			Material.Destroy(_materialSwapRedBlue);
			_materialSwapRedBlue = null;
		}
		
		if (_screenTexture != null)
		{
			Texture2D.Destroy(_screenTexture);
			_screenTexture = null;
		}
		
		base.OnDestroy();
	}
	
	public override bool PrepareCapture()
	{
		if (_capturing)
			return false;
		
		SelectRecordingResolution(Screen.width, Screen.height);
		
		_materialConversion = null;
		_useNativeGrabber = false;
#if AVPRO_MOVIECAPTURE_GLISSUEEVENT		
		if (_isDirectX11)
		{
			//_materialConversion = _materialSwapRedBlue;
			_useNativeGrabber = true;
		}
		else
		{
			_useNativeGrabber = true;
		}
#endif
				
		if (!_useNativeGrabber)
		{
			_texture = new Texture2D(_targetWidth, _targetHeight, TextureFormat.ARGB32, false);
			_texture.name = "AVProMovieCapture-Texture";
			if (_screenTexture != null)
			{
				if (_screenTexture.width != Screen.width || 
					_screenTexture.height != Screen.height)
				{
					Texture2D.Destroy(_screenTexture);
					_screenTexture = null;
				}
			}
			
			if (_screenTexture == null)
			{
				_screenTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, false);
				_screenTexture.name = "AVProMovieCapture-ScreenTexture";
				_screenTexture.filterMode = FilterMode.Bilinear;
				_screenTexture.Apply(false, false);
			}
			
			_materialConversion = _materialSwapRedBlue;
		}
		
		_pixelFormat = AVProMovieCapturePlugin.PixelFormat.RGBA32;
		if (SystemInfo.graphicsDeviceVersion.StartsWith("OpenGL"))
		{
			_pixelFormat = AVProMovieCapturePlugin.PixelFormat.BGRA32;
			_isTopDown = true;
		}
		else
		{
			_isTopDown = false;
			
			if (!_useNativeGrabber)
				_isTopDown = true;

			if (_isDirectX11)
			{
				_isTopDown = false;
			}
		}
		
		GenerateFilename();

		return base.PrepareCapture();
	}
	
	// This is a conversion path used for non-native screen capturing.  This path is slower than native.
	// Used for DX11 support as we don't have native access to the swap chain
	// Used for older versions of Unity that lack native GPU plugin support (GL.IssuePluginEvent)
	private void ConvertAndEncode()
	{
		// Use the min dimensions incase screen size has changed during recording
		int w = Mathf.Min(Screen.width, _screenTexture.width);
		int h = Mathf.Min(Screen.height, _screenTexture.height);
		_screenTexture.ReadPixels(new Rect(0, 0, w, h), 0, 0, false);
		_screenTexture.Apply(false, false);
		
		RenderTexture old = RenderTexture.active;
		RenderTexture buffer = RenderTexture.GetTemporary(_texture.width, _texture.height, 0);						

		// Resize and convert pixel format
		if (_materialConversion == null)
			Graphics.Blit(_screenTexture, buffer);
		else
			Graphics.Blit(_screenTexture, buffer, _materialConversion);
			
		RenderTexture.active = buffer;

		// Read RenderTexture back to Texture2D
		_texture.ReadPixels(new Rect(0, 0, buffer.width, buffer.height), 0, 0, false);
		
		EncodeTexture(_texture);
		RenderTexture.active = old;
		
		 
		RenderTexture.ReleaseTemporary(buffer);		
	}
	
	private int _lastFrame;
	
	private IEnumerator FinalRenderCapture()
	{
		yield return new WaitForEndOfFrame();
		
		while (_handle >= 0 && !AVProMovieCapturePlugin.IsNewFrameDue(_handle))
		{
			System.Threading.Thread.Sleep(8);
		}
		
		if (_handle >= 0)
		{

			// Grab final RenderTexture into texture and encode
#if AVPRO_MOVIECAPTURE_GLISSUEEVENT
			if (_useNativeGrabber)
			{
				if (_audioCapture && _audioDeviceIndex < 0 && !_noAudio)
				{
					AVProMovieCapturePlugin.EncodeAudio(_handle, _audioCapture.BufferPtr, (uint)_audioCapture.BufferLength);
					_audioCapture.FlushBuffer();
				}
				GL.IssuePluginEvent(AVProMovieCapturePlugin.PluginID | (int)AVProMovieCapturePlugin.PluginEvent.CaptureFrameBuffer | _handle);
                GL.InvalidateState();
			}
#endif
            if (!_useNativeGrabber)
			{
				ConvertAndEncode();
				//_texture.ReadPixels(new Rect(0, 0, _texture.width, _texture.height), 0, 0, false);
				//EncodeTexture(_texture);
			}
			
			UpdateFPS();
		}
	
		yield return null;
	}
	
	public override void UpdateFrame()
	{
		if (_capturing && !_paused)
		{
			StartCoroutine("FinalRenderCapture");
		}
		base.UpdateFrame();
	}
}