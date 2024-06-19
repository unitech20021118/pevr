using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

//-----------------------------------------------------------------------------
// Copyright 2012-2015 RenderHeads Ltd.  All rights reserved.
//-----------------------------------------------------------------------------

[RequireComponent(typeof(AudioListener))]
[AddComponentMenu("AVPro Movie Capture/Audio Capture (requires AudioListener)")]
public class AVProUnityAudioCapture : MonoBehaviour 
{
	private float[] _buffer;
	private int _bufferIndex;
	private GCHandle _bufferHandle;
	private int _numChannels;
	
	public float[] Buffer  { get { return _buffer; } }
	public int BufferLength  { get { return _bufferIndex; } }
	public int NumChannels { get { return _numChannels; } }
	public System.IntPtr BufferPtr { get { return _bufferHandle.AddrOfPinnedObject(); } }
	
	void OnEnable()
	{
		int bufferLength = 0;
		int numBuffers = 0;
		AudioSettings.GetDSPBufferSize(out bufferLength, out numBuffers);

#if UNITY_5
        _numChannels = GetNumChannels(AudioSettings.driverCapabilities);
		if (AudioSettings.speakerMode != AudioSpeakerMode.Raw &&
            AudioSettings.speakerMode < AudioSettings.driverCapabilities)
		{
			_numChannels = GetNumChannels(AudioSettings.speakerMode);
		}
        Debug.Log(string.Format("[AVProUnityAudiocapture] SampleRate: {0}hz SpeakerMode: {1} BestDriverMode: {2} (DSP using {3} buffers of {4} bytes using {5} channels)", AudioSettings.outputSampleRate, AudioSettings.speakerMode.ToString(), AudioSettings.driverCapabilities.ToString(), numBuffers, bufferLength, _numChannels));
#else
        _numChannels = GetNumChannels(AudioSettings.driverCaps);
		if (AudioSettings.speakerMode != AudioSpeakerMode.Raw &&
            AudioSettings.speakerMode < AudioSettings.driverCaps)
		{
			_numChannels = GetNumChannels(AudioSettings.speakerMode);
		}

        Debug.Log(string.Format("[AVProUnityAudiocapture] SampleRate: {0}hz SpeakerMode: {1} BestDriverMode: {2} (DSP using {3} buffers of {4} bytes using {5} channels)", AudioSettings.outputSampleRate, AudioSettings.speakerMode.ToString(), AudioSettings.driverCaps.ToString(), numBuffers, bufferLength, _numChannels));
#endif

		_buffer = new float[bufferLength * 256];
		_bufferIndex = 0;	
		_bufferHandle = GCHandle.Alloc(_buffer, GCHandleType.Pinned);
	}
	
	void OnDisable()
	{
		FlushBuffer();
		
		if (_bufferHandle.IsAllocated)
			_bufferHandle.Free();
		_buffer = null;

		_numChannels = 0;
	}
	
	public void FlushBuffer()
	{
		_bufferIndex = 0;
	}

	void OnAudioFilterRead(float[] data, int channels)
	{
		if (_buffer != null)
		{
			int length = Mathf.Min(data.Length, _buffer.Length - _bufferIndex);

			//System.Array.Copy(data, 0, _buffer, _bufferIndex, length);
			for (int i = 0; i < length; i++)
			{
				_buffer[i + _bufferIndex] = data[i];
			}
			_bufferIndex += length;
		}
	}


	static public int GetNumChannels(AudioSpeakerMode mode)
	{
		int result = 0;
		switch (mode)
		{
			case AudioSpeakerMode.Raw:
				break;
			case AudioSpeakerMode.Mono:
				result = 1;
				break;
			case AudioSpeakerMode.Stereo:
				result = 2;
				break;
			case AudioSpeakerMode.Quad:
				result = 4;
				break;
			case AudioSpeakerMode.Surround:
				result = 5;
				break;
			case AudioSpeakerMode.Mode5point1:
				result = 6;
				break;
			case AudioSpeakerMode.Mode7point1:
				result = 8;
				break;
			case AudioSpeakerMode.Prologic:
				result = 2;
				break;
		}
		return result;
	}
}