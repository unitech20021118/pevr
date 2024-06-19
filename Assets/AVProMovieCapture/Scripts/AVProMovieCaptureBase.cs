using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Text;
using System.Runtime.InteropServices;

//-----------------------------------------------------------------------------
// Copyright 2012-2015 RenderHeads Ltd.  All rights reserved.
//-----------------------------------------------------------------------------

public class AVProMovieCaptureBase : MonoBehaviour 
{
	public enum FrameRate
	{
		Fifteen = 15,
		TwentyFour = 24,
		TwentyFive = 25,
		Thirty = 30,
		Fifty = 50,
		Sixty = 60,
	}
	
	public enum DownScale
	{
		Original = 1,
		Half = 2,
		Quarter = 4,
		Eighth = 8,
		Sixteenth = 16,
		Specific = 100,
	}
	
	public enum OutputPath
	{
		RelativeToProject,
		Absolute,
	}

	public enum OutputType
	{
		VideoFile,
		NamedPipe,
	}

	public KeyCode _captureKey = KeyCode.None;
	public bool _captureOnStart = false;
	public bool _listVideoCodecsOnStart = false;
	public string[] _videoCodecPriority = { "Lagarith Lossless Codec",
											"x264vfw - H.264/MPEG-4 AVC codec",
											"Xvid MPEG-4 Codec",
											"ffdshow video encoder",
											"Cinepak Codec by Radius",
											};
	public string[] _audioCodecPriority = { };
	public int _forceVideoCodecIndex = -1;
	public int _forceAudioCodecIndex = -1;
	public int _forceAudioDeviceIndex = -1;
	public FrameRate _frameRate = FrameRate.Thirty;
	public DownScale _downScale = DownScale.Original;
	public Vector2 _maxVideoSize = Vector2.zero;
	public bool _isRealTime = true;
	public bool _autoGenerateFilename = true;
	public OutputPath _outputFolderType = OutputPath.RelativeToProject;
	public string _outputFolderPath;
	public string _autoFilenamePrefix = "MovieCapture";
	public string _autoFilenameExtension = "avi";
	public string _forceFilename = "movie.avi";
	public OutputType _outputType = OutputType.VideoFile;

	[System.NonSerialized]
	public string _codecName = "uncompressed";
	[System.NonSerialized]
	public int _codecIndex = -1;

	[System.NonSerialized]
	public string _audioCodecName = "uncompressed";
	[System.NonSerialized]
	public int _audioCodecIndex = -1;

	[System.NonSerialized]
	public string _audioDeviceName = "Unity";
	[System.NonSerialized]
	public int _audioDeviceIndex = -1;
	
	public bool _noAudio = true;
	[System.NonSerialized]
	public int _unityAudioSampleRate = -1;
	[System.NonSerialized]
	public int _unityAudioChannelCount = -1;

	public AVProUnityAudioCapture _audioCapture;

	protected Texture2D _texture;
	protected int _handle = -1;
	protected int _targetWidth, _targetHeight;
	protected bool _capturing = false;
	protected bool _paused = false;
	protected string _filePath;
	protected FileInfo _fileInfo;
	protected AVProMovieCapturePlugin.PixelFormat _pixelFormat = AVProMovieCapturePlugin.PixelFormat.YCbCr422_YUY2;
	private int _oldVSyncCount = 0;
	protected bool _isTopDown = true;
	protected bool _isDirectX11 = false;
	private bool _queuedStartCapture = false;
	private bool _queuedStopCapture = false;
	
	public string LastFilePath  
	{
		get { return _filePath; }
	}
	
	// Stats
	private uint _numDroppedFrames;
	private uint _numDroppedEncoderFrames;
	private uint _numEncodedFrames;
	private uint _totalEncodedSeconds;
	
	public uint NumDroppedFrames
	{
		get { return _numDroppedFrames; }
	}
	
	public uint NumDroppedEncoderFrames
	{
		get { return _numDroppedEncoderFrames; }
	}

	public uint NumEncodedFrames
	{
		get { return _numEncodedFrames; }
	}

	public uint TotalEncodedSeconds
	{
		get { return _totalEncodedSeconds; }
	}


	public void Awake()
	{
		try
		{
			AVProMovieCapturePlugin.Init();
			Debug.Log("[AVProMovieCapture] Init plugin version: " + AVProMovieCapturePlugin.GetPluginVersion().ToString("F2") + " with GPU " + SystemInfo.graphicsDeviceVersion);
		}
		catch (DllNotFoundException e)
		{
			Debug.LogError("[AVProMovieCapture] Unity couldn't find the DLL, did you move the 'Plugins' folder to the root of your project?");
			throw e;
		}

		_isDirectX11 = SystemInfo.graphicsDeviceVersion.StartsWith("Direct3D 11");
		
		SelectCodec(_listVideoCodecsOnStart);
		SelectAudioCodec(_listVideoCodecsOnStart);
		SelectAudioDevice(_listVideoCodecsOnStart);		
	}
	
	public virtual void Start() 
	{
		Application.runInBackground = true;
		
		if (_captureOnStart)
		{
			StartCapture();
		}
	}
	
	public void SelectCodec(bool listCodecs)
	{
		// Enumerate video codecs
		int numVideoCodecs = AVProMovieCapturePlugin.GetNumAVIVideoCodecs();
		if (listCodecs)
		{
			for (int i = 0; i < numVideoCodecs; i++)
			{
				Debug.Log("VideoCodec " + i + ": " + AVProMovieCapturePlugin.GetAVIVideoCodecName(i));
			}
		}
		
		// The user has specified their own codec index
		if (_forceVideoCodecIndex >= 0)
		{
			if (_forceVideoCodecIndex < numVideoCodecs)
			{
				_codecName = AVProMovieCapturePlugin.GetAVIVideoCodecName(_forceVideoCodecIndex);
				_codecIndex = _forceVideoCodecIndex;
			}
		}
		else
		{
			// Try to find the codec based on the priority list
			if (_videoCodecPriority != null)
			{
				foreach (string codec in _videoCodecPriority)
				{
					string codecName = codec.Trim();
					// Empty string means uncompressed
					if (string.IsNullOrEmpty(codecName))
						break;
					
					for (int i = 0; i < numVideoCodecs; i++)
					{
						if (codecName == AVProMovieCapturePlugin.GetAVIVideoCodecName(i))
						{
							_codecName = codecName;
							_codecIndex = i;
							break;
						}
					}
					
					if (_codecIndex >= 0)
						break;
				}
			}
		}
		
		if (_codecIndex < 0)
		{
			_codecName = "Uncompressed";
			Debug.LogWarning("[AVProMovieCapture] Codec not found.  Video will be uncompressed.");
		}
	}
	

	public void SelectAudioCodec(bool listCodecs)
	{
		// Enumerate audio codecs
		int numAudioCodecs = AVProMovieCapturePlugin.GetNumAVIAudioCodecs();
		if (listCodecs)
		{
			for (int i = 0; i < numAudioCodecs; i++)
			{
				Debug.Log("AudioCodec " + i + ": " + AVProMovieCapturePlugin.GetAVIAudioCodecName(i));
			}
		}
		
		// The user has specified their own codec index
		if (_forceAudioCodecIndex >= 0)
		{
			if (_forceAudioCodecIndex < numAudioCodecs)
			{
				_audioCodecName = AVProMovieCapturePlugin.GetAVIAudioCodecName(_forceAudioCodecIndex);
				_audioCodecIndex = _forceAudioCodecIndex;
			}
		}
		else
		{
			// Try to find the codec based on the priority list
			if (_audioCodecPriority != null)
			{
				foreach (string codec in _audioCodecPriority)
				{
					string codecName = codec.Trim();
					// Empty string means uncompressed
					if (string.IsNullOrEmpty(codecName))
						break;
					
					for (int i = 0; i < numAudioCodecs; i++)
					{
						if (codecName == AVProMovieCapturePlugin.GetAVIAudioCodecName(i))
						{
							_audioCodecName = codecName;
							_audioCodecIndex = i;
							break;
						}
					}
					
					if (_audioCodecIndex >= 0)
						break;
				}
			}
		}
		
		if (_audioCodecIndex < 0)
		{
			_audioCodecName = "Uncompressed";
			Debug.LogWarning("[AVProMovieCapture] Codec not found.  Audio will be uncompressed.");
		}
	}	

	public void SelectAudioDevice(bool display)
	{
		// Enumerate
		int num = AVProMovieCapturePlugin.GetNumAVIAudioInputDevices();
		if (display)
		{
			for (int i = 0; i < num; i++)
			{
				Debug.Log("AudioDevice " + i + ": " + AVProMovieCapturePlugin.GetAVIAudioInputDeviceName(i));
			}
		}

		// The user has specified their own device index
		if (_forceAudioDeviceIndex >= 0)
		{
			if (_forceAudioDeviceIndex < num)
			{
				_audioDeviceName = AVProMovieCapturePlugin.GetAVIAudioInputDeviceName(_forceAudioDeviceIndex);
				_audioDeviceIndex = _forceAudioDeviceIndex;
			}
		}
		else
		{
			/*_audioDeviceIndex = -1;
			// Try to find one of the loopback devices
			for (int i = 0; i < num; i++)
			{
				StringBuilder sbName = new StringBuilder(512);
				if (AVProMovieCapturePlugin.GetAVIAudioInputDeviceName(i, sbName))
				{
					string[] loopbackNames = { "Stereo Mix", "What U Hear", "What You Hear", "Waveout Mix", "Mixed Output" };
					for (int j = 0; j < loopbackNames.Length; j++)
					{
						if (sbName.ToString().Contains(loopbackNames[j]))
						{
							_audioDeviceIndex = i;
							_audioDeviceName = sbName.ToString();
						}
					}
				}
				if (_audioDeviceIndex >= 0)
					break;
			}
			
			if (_audioDeviceIndex < 0)
			{
				// Resort to the no recording device
				_audioDeviceName = "Unity";
				_audioDeviceIndex = -1;
			}*/

			_audioDeviceName = "Unity";
			_audioDeviceIndex = -1;
		}
	}

	public static Vector2 GetRecordingResolution(int width, int height, DownScale downscale, Vector2 maxVideoSize)
	{
		int targetWidth = width;
		int targetHeight = height;
		if (downscale != DownScale.Specific)
		{
			targetWidth /= (int)downscale;
			targetHeight /= (int)downscale;
		}
		else
		{
			if (maxVideoSize.x >= 1.0f && maxVideoSize.y >= 1.0f)
			{
				targetWidth = Mathf.FloorToInt(maxVideoSize.x);
				targetHeight = Mathf.FloorToInt(maxVideoSize.y);
			}
		}
		
		// Some codecs like Lagarith in YUY2 mode need size to be multiple of 4
		targetWidth = NextMultipleOf4(targetWidth);
		targetHeight = NextMultipleOf4(targetHeight);

		return new Vector2(targetWidth, targetHeight);
	}

	public void SelectRecordingResolution(int width, int height)
	{
		_targetWidth = width;
		_targetHeight = height;
		if (_downScale != DownScale.Specific)
		{
			_targetWidth /= (int)_downScale;
			_targetHeight /= (int)_downScale;
		}
		else
		{
			if (_maxVideoSize.x >= 1.0f && _maxVideoSize.y >= 1.0f)
			{
				_targetWidth = Mathf.FloorToInt(_maxVideoSize.x);
				_targetHeight = Mathf.FloorToInt(_maxVideoSize.y);
			}
		}
		
		// Some codecs like Lagarith in YUY2 mode need size to be multiple of 4
		_targetWidth = NextMultipleOf4(_targetWidth);
		_targetHeight = NextMultipleOf4(_targetHeight);
	}

	public virtual void OnDestroy()
	{
		StopCapture();
		AVProMovieCapturePlugin.Deinit();
	}
	
	public void OnApplicationQuit()
	{
		StopCapture();
		AVProMovieCapturePlugin.Deinit();
	}
		
	protected void EncodeTexture(Texture2D texture)
	{
		Color32[] bytes = texture.GetPixels32();
		GCHandle _frameHandle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
		
		EncodePointer(_frameHandle.AddrOfPinnedObject());		
				
		if (_frameHandle.IsAllocated)
			_frameHandle.Free();
	}
	
	public virtual void EncodePointer(System.IntPtr ptr)
	{	
		if (_audioCapture == null || (_audioDeviceIndex >= 0 || _noAudio))
		{
			AVProMovieCapturePlugin.EncodeFrame(_handle, ptr);
		}
		else
		{
			AVProMovieCapturePlugin.EncodeFrameWithAudio(_handle, ptr, _audioCapture.BufferPtr, (uint)_audioCapture.BufferLength);
			_audioCapture.FlushBuffer();
		}
	}
	
	public bool IsCapturing()
	{
		return _capturing;
	}

	public bool IsPaused()
	{
		return _paused;
	}
	
	public int GetRecordingWidth()
	{
		return _targetWidth;
	}
	
	public int GetRecordingHeight()
	{
		return _targetHeight;
	}
	
	protected virtual string GenerateTimestampedFilename(string filenamePrefix, string filenameExtension)
	{
		TimeSpan span = (DateTime.Now - DateTime.Now.Date);
		return string.Format("{0}-{1}-{2}-{3}-{4}s-{5}x{6}.{7}", filenamePrefix, DateTime.Now.Year, DateTime.Now.Month.ToString("D2"), DateTime.Now.Day.ToString("D2"), ((int)(span.TotalSeconds)).ToString(), _targetWidth, _targetHeight, filenameExtension);		
	}
	
	private static string AutoGenerateFilename(OutputPath outputPathType, string path, string filename)
	{
		// Create folder
		string fileFolder = string.Empty;
		if (outputPathType == OutputPath.RelativeToProject)
		{
			string projectFolder = System.IO.Path.GetFullPath(System.IO.Path.Combine(Application.dataPath, ".."));
			fileFolder = System.IO.Path.Combine(projectFolder, path);
		}
		else if (outputPathType == OutputPath.Absolute)
		{
			fileFolder = path;
		}
		
		// Combine path and filename
		return System.IO.Path.Combine(fileFolder, filename);
	}
	
	private static string ManualGenerateFilename(OutputPath outputPathType, string path, string filename)
	{
		string result = filename;
		
		if (outputPathType == OutputPath.RelativeToProject)
		{
			if (!System.IO.Path.IsPathRooted(filename) && !System.IO.Path.IsPathRooted(path))
			{
				string projectFolder = System.IO.Path.GetFullPath(System.IO.Path.Combine(Application.dataPath, ".."));
				result = System.IO.Path.Combine(System.IO.Path.Combine(projectFolder, path), filename);
			}
		}
		else if (outputPathType == OutputPath.Absolute)
		{
			if (!System.IO.Path.IsPathRooted(filename))
			{
				result = System.IO.Path.Combine(path, filename);
			}
		}
		
		return result;
	}
	
	/*[ContextMenu("Debug GenerateFilename")]
	public void DebugGenereateFilename()
	{
		GenerateFilename();
		Debug.Log("PATH: " + _filePath);
	}*/
	
	protected void GenerateFilename()
	{	
		if (_autoGenerateFilename || string.IsNullOrEmpty(_forceFilename))
		{
			string filename = GenerateTimestampedFilename(_autoFilenamePrefix, _autoFilenameExtension);
			_filePath = AutoGenerateFilename(_outputFolderType, _outputFolderPath, filename);
		}
		else
		{
			_filePath = ManualGenerateFilename(_outputFolderType, _outputFolderPath, _forceFilename);
		}
		
		// Create target directory if doesn't exist
		String directory = Path.GetDirectoryName(_filePath);
		if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
			Directory.CreateDirectory(directory);
	}
	
	public virtual bool PrepareCapture()
	{
		// Delete file if it already exists
		if (File.Exists(_filePath))
		{
			File.Delete(_filePath);
		}
        
		// Disable vsync
		if (!Screen.fullScreen && QualitySettings.vSyncCount > 0)
		{
			_oldVSyncCount = QualitySettings.vSyncCount;
			//Debug.LogWarning("For best results vsync should be disabled during video capture.  Disabling vsync.");
			QualitySettings.vSyncCount = 0;
		}
		
		if (_isRealTime)
		{
			Application.targetFrameRate = (int)_frameRate;
		}
		else
		{
			Time.captureFramerate = (int)_frameRate;
		}
		
		int audioDeviceIndex = _audioDeviceIndex;
		int audioCodecIndex = _audioCodecIndex;
		bool noAudio = _noAudio;
		if (_noAudio || (_audioCapture == null && _audioDeviceIndex < 0))
		{
			audioCodecIndex = audioDeviceIndex = -1;
			_audioDeviceName = "none";
			noAudio = true;
		}
		
		_unityAudioSampleRate = -1;
		_unityAudioChannelCount = -1;
		if (!noAudio && _audioDeviceIndex < 0 && _audioCapture != null)
		{
			if (!_audioCapture.enabled)
				_audioCapture.enabled = true;
			_unityAudioSampleRate = AudioSettings.outputSampleRate;
			_unityAudioChannelCount = _audioCapture.NumChannels;
		}
		
		string info = string.Format("{0}x{1} @ {2}fps [{3}]", _targetWidth, _targetHeight, ((int)_frameRate).ToString(), _pixelFormat.ToString());
		info += string.Format(" vcodec:'{0}'", _codecName);
		if (!noAudio)
		{
			if (_audioDeviceIndex >= 0)
			{
				info += string.Format(" audio device:'{0}'", _audioDeviceName);
			}
			else
			{
				info += string.Format(" audio device:'Unity' {0}hz {1} channels", _unityAudioSampleRate, _unityAudioChannelCount);
			}
			info += string.Format(" acodec:'{0}'", _audioCodecName);
		}
		info += string.Format(" to file: '{0}'", _filePath);

		if (_outputType == OutputType.VideoFile)
		{
			Debug.Log("[AVProMovieCapture] Start File Capture: " + info);
			_handle = AVProMovieCapturePlugin.CreateRecorderAVI(_filePath, (uint)_targetWidth, (uint)_targetHeight, (int)_frameRate,
			                                                    (int)(_pixelFormat), _isTopDown, _codecIndex, !noAudio, _unityAudioSampleRate, _unityAudioChannelCount, audioDeviceIndex, audioCodecIndex, _isRealTime);
		}
		else if (_outputType == OutputType.NamedPipe)
		{
			Debug.Log("[AVProMovieCapture] Start Pipe Capture: " + info);
			_handle = AVProMovieCapturePlugin.CreateRecorderPipe(_filePath, (uint)_targetWidth, (uint)_targetHeight, (int)_frameRate,
			                                                     (int)(_pixelFormat), _isTopDown);
		}

		if (_handle < 0)
		{
			Debug.LogWarning("[AVProMovieCapture] Failed to create recorder");
			StopCapture();
		}

		return (_handle >= 0);
	}
	
	public void QueueStartCapture()
	{
		_queuedStartCapture = true;
	}

	public bool StartCapture()
	{
        Debug.Log("strat");
        if (_capturing)
			return false;

		if (_handle < 0)
		{
			if (!PrepareCapture())
			{
				return false;
			}
		}

		if (_audioCapture && _audioDeviceIndex < 0 && !_noAudio)
		{
			_audioCapture.FlushBuffer();
		}
		
		if (_handle >= 0)
		{
			AVProMovieCapturePlugin.Start(_handle);
			ResetFPS();
			_capturing = true;
			_paused = false;
		}

		return _capturing;
	}
	
	public void PauseCapture()
	{
		if (_capturing && !_paused)
		{
			AVProMovieCapturePlugin.Pause(_handle);
			_paused = true;
			ResetFPS();
		}
	}
	
	public void ResumeCapture()
	{
		if (_capturing && _paused)
		{
			AVProMovieCapturePlugin.Start(_handle);
			_paused = false;
		}
	}
	
	public void StopCapture()
	{
		if (_capturing)
		{
			Debug.Log("[AVProMovieCapture] Stopping capture");
			_capturing = false;
		}
		
		if (_handle >= 0)
		{
			AVProMovieCapturePlugin.Stop(_handle);
			System.Threading.Thread.Sleep(100);
			AVProMovieCapturePlugin.FreeRecorder(_handle);
			_handle = -1;
		}

		_fileInfo = null;

		if (_audioCapture)
			_audioCapture.enabled = false;
		
		Time.captureFramerate = 0;
		Application.targetFrameRate = -1;
		
		if (_oldVSyncCount > 0)
		{
			QualitySettings.vSyncCount = _oldVSyncCount;
			_oldVSyncCount = 0;
		}
		
		if (_texture != null)
		{
			Destroy(_texture);
			_texture = null;
		}
	}
	
	private void ToggleCapture()
	{
		if (_capturing)
		{
			//_queuedStopCapture = true;
			//_queuedStartCapture = false;
			StopCapture();
		}
		else
		{
			//_queuedStartCapture = true;
			//_queuedStopCapture = false;
			StartCapture();
		}
	}
	
	void Update()
	{
		UpdateFrame();
	}
	
	public virtual void UpdateFrame() 
	{
		if (Input.GetKeyDown(_captureKey))
		{
			ToggleCapture();
		}
		
		if (_handle >= 0 && !_paused)
		{
			_numDroppedFrames = AVProMovieCapturePlugin.GetNumDroppedFrames(_handle);
			_numDroppedEncoderFrames = AVProMovieCapturePlugin.GetNumDroppedEncoderFrames(_handle);
			_numEncodedFrames = AVProMovieCapturePlugin.GetNumEncodedFrames(_handle);
			_totalEncodedSeconds = AVProMovieCapturePlugin.GetEncodedSeconds(_handle);
		}
		
		if (_queuedStopCapture)
		{
			_queuedStopCapture = false;
			_queuedStartCapture = false;
			StopCapture();
		}
		if (_queuedStartCapture)
		{
			_queuedStartCapture = false;
			StartCapture();
		}
	}

	[NonSerializedAttribute]
	public float _fps;
	[NonSerializedAttribute]
	public int _frameTotal;
	
	private int _frameCount;
	private float _startFrameTime;
	
	protected void ResetFPS()
	{
		_frameCount = 0;
		_frameTotal = 0;
		_fps = 0.0f;
		_startFrameTime = 0.0f;
	}
	
	public void UpdateFPS()
	{
		_frameCount++;
		_frameTotal++;
		
		float timeNow = Time.realtimeSinceStartup;
		float timeDelta = timeNow - _startFrameTime;
		if (timeDelta >= 1.0f)
		{
			_fps = (float)_frameCount / timeDelta;
			_frameCount  = 0;
			_startFrameTime = timeNow;
		}
	}	
	
    private void ConfigureCodec() 
	{
		AVProMovieCapturePlugin.Init();
       	SelectCodec(false);
		if (_codecIndex >= 0)
		{
			AVProMovieCapturePlugin.ConfigureVideoCodec(_codecIndex);
		}
		//AVProMovieCapture.Deinit();
	}

	public long GetCaptureFileSize()
	{
		long result = 0;
#if !UNITY_WEBPLAYER
		if (_handle >= 0)
		{
			if (_fileInfo == null && File.Exists(_filePath))
			{
				_fileInfo = new System.IO.FileInfo(_filePath);
			}
			if (_fileInfo != null)
			{
				_fileInfo.Refresh();
				result = _fileInfo.Length;
			}
		}
#endif
		return result;
	}

	private static long GetFileSize(string filename)
	{
#if UNITY_WEBPLAYER
		return 0;
#else
		System.IO.FileInfo fi = new System.IO.FileInfo(filename);
		return fi.Length;
#endif
	}	
	
	// Returns the next multiple of 4 or the same value if it's already a multiple of 4
	protected static int NextMultipleOf4(int value)
	{
		return (value + 3) & ~0x03;
	}
}