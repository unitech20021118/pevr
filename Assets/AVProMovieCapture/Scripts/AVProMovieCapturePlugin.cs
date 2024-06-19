using UnityEngine;
using System.Text;
using System.Runtime.InteropServices;

//-----------------------------------------------------------------------------
// Copyright 2012-2015 RenderHeads Ltd.  All rights reserved.
//-----------------------------------------------------------------------------

public class AVProMovieCapturePlugin
{
	public enum PixelFormat
	{
		RGBA32,
		BGRA32,				// Note: This is the native format for Unity textures with red and blue swapped.
		YCbCr422_YUY2,
		YCbCr422_UYVY,
		YCbCr422_HDYC,
	}

	// Used by GL.IssuePluginEvent
	public const int PluginID = 0xFA30000;
	public enum PluginEvent
	{
		CaptureFrameBuffer = 0,
	}	

	//////////////////////////////////////////////////////////////////////////
	// Global Init/Deinit
	
	[DllImport("AVProMovieCapture")]
	public static extern bool Init();

	[DllImport("AVProMovieCapture")]
	public static extern void Deinit();

	[DllImport("AVProMovieCapture")]
	public static extern float GetPluginVersion();

	//////////////////////////////////////////////////////////////////////////
	// Video Codecs
	
	[DllImport("AVProMovieCapture")]
	public static extern int GetNumAVIVideoCodecs();

	[DllImport("AVProMovieCapture")]
	public static extern bool IsConfigureVideoCodecSupported(int index);
	
	[DllImport("AVProMovieCapture")]
	public static extern void ConfigureVideoCodec(int index);
	
	public static string GetAVIVideoCodecName(int index)
	{
		string result = "Invalid";
		StringBuilder nameBuffer = new StringBuilder(256);
		if (GetAVIVideoCodecName(index, nameBuffer, nameBuffer.Capacity))
		{
			result = nameBuffer.ToString();
		}
		return result;
	}
	

	//////////////////////////////////////////////////////////////////////////
	// Audio Codecs
	
	[DllImport("AVProMovieCapture")]
	public static extern int GetNumAVIAudioCodecs();
	
	[DllImport("AVProMovieCapture")]
	public static extern bool IsConfigureAudioCodecSupported(int index);
	
	[DllImport("AVProMovieCapture")]
	public static extern void ConfigureAudioCodec(int index);

	public static string GetAVIAudioCodecName(int index)
	{
		string result = "Invalid";
		StringBuilder nameBuffer = new StringBuilder(256);
		if (GetAVIAudioCodecName(index, nameBuffer, nameBuffer.Capacity))
		{
			result = nameBuffer.ToString();
		}
		return result;
	}

	//////////////////////////////////////////////////////////////////////////
	// Audio Devices

	[DllImport("AVProMovieCapture")]
	public static extern int GetNumAVIAudioInputDevices();

	public static string GetAVIAudioInputDeviceName(int index)
	{
		string result = "Invalid";
		StringBuilder nameBuffer = new StringBuilder(256);
		if (GetAVIAudioInputDeviceName(index, nameBuffer, nameBuffer.Capacity))
		{
			result = nameBuffer.ToString();
		}
		return result;
	}

	//////////////////////////////////////////////////////////////////////////
	// Create the recorder
	
	[DllImport("AVProMovieCapture")]
	public static extern int CreateRecorderAVI([MarshalAs(UnmanagedType.LPWStr)] string filename, uint width, uint height, int frameRate, int format, 
											bool isTopDown, int videoCodecIndex, bool hasAudio, int audioSampleRate, int audioChannelCount, int audioInputDeviceIndex, int audioCodecIndex, bool isRealTime);

	[DllImport("AVProMovieCapture")]
	public static extern int CreateRecorderPipe([MarshalAs(UnmanagedType.LPWStr)] string filename, uint width, uint height, int frameRate, int format, bool isTopDown);

	//////////////////////////////////////////////////////////////////////////
	// Update recorder

	[DllImport("AVProMovieCapture")]
	public static extern void Start(int handle);

	[DllImport("AVProMovieCapture")]
	public static extern bool IsNewFrameDue(int handle);

	[DllImport("AVProMovieCapture")]
	public static extern bool EncodeFrame(int handle, System.IntPtr data);

	[DllImport("AVProMovieCapture")]
	public static extern bool EncodeAudio(int handle, System.IntPtr data, uint length);
	
	[DllImport("AVProMovieCapture")]
	public static extern bool EncodeFrameWithAudio(int handle, System.IntPtr videoData, System.IntPtr audioData, uint audioLength);
	
	[DllImport("AVProMovieCapture")]
	public static extern void Pause(int handle);
	
	[DllImport("AVProMovieCapture")]
	public static extern void Stop(int handle);
	
	//////////////////////////////////////////////////////////////////////////
	// Destroy recorder
	
	[DllImport("AVProMovieCapture")]
	public static extern void FreeRecorder(int handle);
	
	//////////////////////////////////////////////////////////////////////////
	// Debugging
	
	[DllImport("AVProMovieCapture")]
	public static extern uint GetNumDroppedFrames(int handle);
	
	[DllImport("AVProMovieCapture")]
	public static extern uint GetNumDroppedEncoderFrames(int handle);

	[DllImport("AVProMovieCapture")]
	public static extern uint GetNumEncodedFrames(int handle);

	[DllImport("AVProMovieCapture")]
	public static extern uint GetEncodedSeconds(int handle);

	//////////////////////////////////////////////////////////////////////////
	// Private internal functions

	[DllImport("AVProMovieCapture")]
	private static extern bool GetAVIVideoCodecName(int index, StringBuilder name, int nameBufferLength);

	[DllImport("AVProMovieCapture")]
	private static extern bool GetAVIAudioCodecName(int index, StringBuilder name, int nameBufferLength);

	[DllImport("AVProMovieCapture")]
	private static extern bool GetAVIAudioInputDeviceName(int index, StringBuilder name, int nameBufferLength);
}