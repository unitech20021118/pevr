using UnityEngine;
using UnityEditor;

//-----------------------------------------------------------------------------
// Copyright 2012-2014 RenderHeads Ltd.  All rights reserved.
//-----------------------------------------------------------------------------

public class AVProMovieCaptureEditorWindow : EditorWindow 
{
	private const string TempGameObjectName = "Temp_MovieCapture";
	private const string SettingsPrefix = "AVProMovieCapture.EditorWindow.";
		
	private AVProMovieCaptureBase _capture;
	private AVProMovieCaptureFromScene _captureScene;
	private AVProMovieCaptureFromCamera _captureCamera;
	private AVProUnityAudioCapture _audioCapture;
	private static bool _isCreated = false;
	private static bool _isInit = false;
	private static bool _isFailedInit = false;
	private static string _lastCapturePath;
	
	private static string[] _videoCodecNames;
	private static string[] _audioCodecNames;
	private static string[] _audioDeviceNames;
	private static bool[] _videoCodecConfigurable;
	private static bool[] _audioCodecConfigurable;
	private readonly string[] _downScales = { "Original", "Half", "Quarter", "Eighth", "Sixteenth", "Specific" };	
	//private readonly string[] _frameRates = { "15", "24", "25", "30", "50", "60" };
	private readonly string[] _captureModes = { "Realtime", "Offline" };
	private readonly string[] _outputFolders = { "Project Folder", "Absolute Folder" };
	
	private enum SourceType
	{
		Scene,
		Camera,
	}
	
	private bool _expandConfig = true;
	private SourceType _sourceType = SourceType.Scene;
	private Camera _cameraNode;
	private string _cameraName;	
	private bool _cameraFastPixel;	
	private int _captureModeIndex;
	private int _outputFolderIndex;
	private bool _autoFilenamePrefixFromSceneName = true;
	private string _autoFilenamePrefix = "capture";
	private string _autoFilenameExtension = "avi";
	private string _outputFolderRelative = string.Empty;
	private string _outputFolderAbsolute = string.Empty;
	private bool _appendTimestamp = true;
	private int _downScaleIndex;
	private int _downscaleX;
	private int _downscaleY;
	private AVProMovieCaptureBase.FrameRate _frameRate = AVProMovieCaptureBase.FrameRate.Thirty;
	private int _videoCodecIndex;
	private bool _captureAudio;
	private int _audioCodecIndex;
	private int _audioDeviceIndex;
	private Vector2 _scroll = Vector2.zero;
	private bool _queueStart;
    private int _queueConfigureVideoCodec = -1;
    private int _queueConfigureAudioCodec = -1;

    private long _lastFileSize;
    private uint _lastEncodedMinutes;
    private uint _lastEncodedSeconds;

	[MenuItem ("Window/AVPro Movie Capture")]
	private static void Init()
	{
		if (_isInit)
			return;
		
		if (_isCreated)
			return;
				
		_isCreated = true;
		
        // Get existing open window or if none, make a new one:
        AVProMovieCaptureEditorWindow window = (AVProMovieCaptureEditorWindow)EditorWindow.GetWindow(typeof(AVProMovieCaptureEditorWindow));

		if (window != null)
		{
			window.SetupWindow();
		}
	}

	public void SetupWindow()
	{
		_isCreated = true;
		if (Application.platform == RuntimePlatform.WindowsEditor)
		{
			this.minSize = new Vector2(200f, 48f);
			this.maxSize = new Vector2(340f, 620f);
#if UNITY_5_1
			this.titleContent = new GUIContent("Movie Capture");
#else
			this.title = "Movie Capture";
#endif
			this.CreateGUI();
			this.LoadSettings();
			this.Repaint();		
		}
	}

	private void LoadSettings()
	{
		_expandConfig = EditorPrefs.GetBool(SettingsPrefix + "ExpandConfig", true);
		_sourceType = (SourceType)EditorPrefs.GetInt(SettingsPrefix + "SourceType", (int)_sourceType);
		_cameraName = EditorPrefs.GetString(SettingsPrefix + "CameraName", string.Empty);
		_cameraFastPixel = EditorPrefs.GetBool(SettingsPrefix + "CameraFastPixel", true);
		_captureModeIndex = EditorPrefs.GetInt(SettingsPrefix + "CaptureModeIndex", 0);
		
		_autoFilenamePrefixFromSceneName = EditorPrefs.GetBool(SettingsPrefix + "AutoFilenamePrefixFromScenename", _autoFilenamePrefixFromSceneName);
		_autoFilenamePrefix = EditorPrefs.GetString(SettingsPrefix + "AutoFilenamePrefix", "capture");
		_autoFilenameExtension = EditorPrefs.GetString(SettingsPrefix + "AutoFilenameExtension", "avi");
		_appendTimestamp = EditorPrefs.GetBool(SettingsPrefix + "AppendTimestamp", true);
		
		_outputFolderIndex = EditorPrefs.GetInt(SettingsPrefix + "OutputFolderIndex", 0);
		_outputFolderRelative = EditorPrefs.GetString(SettingsPrefix + "OutputFolderRelative", string.Empty);
		_outputFolderAbsolute = EditorPrefs.GetString(SettingsPrefix + "OutputFolderAbsolute", string.Empty);
		
		_downScaleIndex = EditorPrefs.GetInt(SettingsPrefix + "DownScaleIndex", 0);
		_downscaleX = EditorPrefs.GetInt(SettingsPrefix + "DownScaleX", 1);
		_downscaleY = EditorPrefs.GetInt(SettingsPrefix + "DownScaleY", 1);
		_frameRate = (AVProMovieCaptureBase.FrameRate)System.Enum.Parse(typeof(AVProMovieCaptureBase.FrameRate), EditorPrefs.GetString(SettingsPrefix + "FrameRate", "Thirty"));
		_videoCodecIndex = EditorPrefs.GetInt(SettingsPrefix + "VideoCodecIndex", 0);

		_captureAudio = EditorPrefs.GetBool(SettingsPrefix + "CaptureAudio", false);
		_audioCodecIndex = EditorPrefs.GetInt(SettingsPrefix + "AudioCodecIndex", 0);
		_audioDeviceIndex = EditorPrefs.GetInt(SettingsPrefix + "AudioDeviceIndex", 0);
		
		if (!string.IsNullOrEmpty(_cameraName))
		{
			Camera[] cameras = (Camera[])GameObject.FindObjectsOfType(typeof(Camera));
			foreach (Camera cam in cameras)
			{
				if (cam.name == _cameraName)
				{
					_cameraNode = cam;
					break;
				}
			}
		}

		// Check ranges
		if (_videoCodecIndex >= _videoCodecNames.Length)
		{
			_videoCodecIndex = 0;
		}
		if (_audioDeviceIndex >= _audioDeviceNames.Length)
		{
			_audioDeviceIndex = 0;
			_captureAudio = false;
		}
		if (_audioCodecIndex >= _audioCodecNames.Length)
		{
			_audioCodecIndex = 0;
			_captureAudio = false;
		}
	}
	
	private void SaveSettings()
	{	
		EditorPrefs.SetBool(SettingsPrefix + "ExpandConfig", _expandConfig);
		EditorPrefs.SetInt(SettingsPrefix + "SourceType", (int)_sourceType);
		EditorPrefs.SetString(SettingsPrefix + "CameraName", _cameraName);
		EditorPrefs.SetBool(SettingsPrefix + "CameraFastPixel", _cameraFastPixel);
		EditorPrefs.SetInt(SettingsPrefix + "CaptureModeIndex", _captureModeIndex);	
		
		EditorPrefs.SetBool(SettingsPrefix + "AutoFilenamePrefixFromScenename", _autoFilenamePrefixFromSceneName);
		EditorPrefs.SetString(SettingsPrefix + "AutoFilenamePrefix", _autoFilenamePrefix);
		EditorPrefs.SetString(SettingsPrefix + "AutoFilenameExtension", _autoFilenameExtension);
		EditorPrefs.SetBool(SettingsPrefix + "AppendTimestamp", _appendTimestamp);
		
		EditorPrefs.SetInt(SettingsPrefix + "OutputFolderIndex", _outputFolderIndex);
		EditorPrefs.SetString(SettingsPrefix + "OutputFolderRelative", _outputFolderRelative);
		EditorPrefs.SetString(SettingsPrefix + "OutputFolderAbsolute", _outputFolderAbsolute);
		
		EditorPrefs.SetInt(SettingsPrefix + "DownScaleIndex", _downScaleIndex);
		EditorPrefs.SetInt(SettingsPrefix + "DownScaleX", _downscaleX);
		EditorPrefs.SetInt(SettingsPrefix + "DownScaleY", _downscaleY);
		EditorPrefs.SetString(SettingsPrefix + "FrameRate", _frameRate.ToString());
		EditorPrefs.SetInt(SettingsPrefix + "VideoCodecIndex", _videoCodecIndex);

		EditorPrefs.SetBool(SettingsPrefix + "CaptureAudio", _captureAudio);
		EditorPrefs.SetInt(SettingsPrefix + "AudioCodecIndex", _audioCodecIndex);
		EditorPrefs.SetInt(SettingsPrefix + "AudioDeviceIndex", _audioDeviceIndex);
	}
	
	private void ResetSettings()
	{
		_expandConfig = true;
		_sourceType = SourceType.Scene;
		_cameraNode = null;
		_cameraName = string.Empty;
		_cameraFastPixel = true;
		_captureModeIndex = 0;
		_autoFilenamePrefixFromSceneName = true;
		_autoFilenamePrefix = "capture";
		_autoFilenameExtension = "avi";
		_outputFolderIndex = 0;
		_outputFolderRelative = string.Empty;
		_outputFolderAbsolute = string.Empty;
		_appendTimestamp = true;
		_downScaleIndex = 0;
		_downscaleX = 1;
		_downscaleY = 1;
		_frameRate = AVProMovieCaptureBase.FrameRate.Thirty;
		_videoCodecIndex = 0;
		_captureAudio = false;
		_audioCodecIndex = 0;
		_audioDeviceIndex = 0;
	}

	private static AVProMovieCaptureBase.DownScale GetDownScaleFromIndex(int index)
	{
		AVProMovieCaptureBase.DownScale result = AVProMovieCaptureBase.DownScale.Original;
		switch (index)
		{
		case 0:
			result = AVProMovieCaptureBase.DownScale.Original;
			break;
		case 1:
			result = AVProMovieCaptureBase.DownScale.Half;
			break;
		case 2:
			result = AVProMovieCaptureBase.DownScale.Quarter;
			break;
		case 3:
			result = AVProMovieCaptureBase.DownScale.Eighth;
			break;
		case 4:
			result = AVProMovieCaptureBase.DownScale.Sixteenth;
			break;
		case 5:
			result = AVProMovieCaptureBase.DownScale.Specific;
			break;
		}

		return result;
	}
		
	private void Configure(AVProMovieCaptureBase capture)
	{
		capture._videoCodecPriority = null;
		capture._audioCodecPriority = null;

		capture._captureOnStart = false;
		capture._listVideoCodecsOnStart = false;
		capture._frameRate = _frameRate;
		capture._downScale = GetDownScaleFromIndex(_downScaleIndex);
		if (capture._downScale == AVProMovieCaptureBase.DownScale.Specific)
		{
			capture._maxVideoSize.x = _downscaleX;
			capture._maxVideoSize.y = _downscaleY;
		}
		
		capture._isRealTime = (_captureModeIndex == 0);
		capture._autoGenerateFilename = _appendTimestamp;
		capture._autoFilenamePrefix = _autoFilenamePrefix;
		capture._autoFilenameExtension = _autoFilenameExtension;
		if (!capture._autoGenerateFilename)
		{
			capture._forceFilename = _autoFilenamePrefix + "." + _autoFilenameExtension;
		}
		
		capture._outputFolderType = AVProMovieCaptureBase.OutputPath.RelativeToProject;
		capture._outputFolderPath = _outputFolderRelative;
		if (_outputFolderIndex == 1)
		{
			capture._outputFolderType = AVProMovieCaptureBase.OutputPath.Absolute;
			capture._outputFolderPath = _outputFolderAbsolute;
		}
		
		capture._forceVideoCodecIndex = capture._codecIndex = Mathf.Max(-1, (_videoCodecIndex - 2));
		capture._noAudio = !_captureAudio;
		capture._forceAudioCodecIndex = capture._audioCodecIndex = Mathf.Max(-1, (_audioCodecIndex - 2));
		capture._forceAudioDeviceIndex = capture._audioDeviceIndex = Mathf.Max(-1, (_audioDeviceIndex - 2));
	}
	
	private void CreateComponents()
	{
		switch (_sourceType)
		{
		case SourceType.Scene:
			_captureScene = (AVProMovieCaptureFromScene)GameObject.FindObjectOfType(typeof(AVProMovieCaptureFromScene));		
			if (_captureScene == null)
			{
				GameObject go = new GameObject(TempGameObjectName);
				_captureScene = go.AddComponent<AVProMovieCaptureFromScene>();
			}
			_capture = _captureScene;
			break;
		case SourceType.Camera:
			_captureCamera = _cameraNode.gameObject.GetComponent<AVProMovieCaptureFromCamera>();
			if (_captureCamera == null)
			{
				_captureCamera = _cameraNode.gameObject.AddComponent<AVProMovieCaptureFromCamera>();
			}
			_captureCamera._useFastPixelFormat = _cameraFastPixel;
			_capture = _captureCamera;			
			break;
		}
				
		_audioCapture = null;
		if (_captureAudio && _audioDeviceIndex == 0)
		{
			_audioCapture = (AVProUnityAudioCapture)GameObject.FindObjectOfType(typeof(AVProUnityAudioCapture));
			if (_audioCapture == null && Camera.main != null)
			{
				_audioCapture = Camera.main.gameObject.AddComponent<AVProUnityAudioCapture>();
			}
		}		
	}
	
	private void CreateGUI()
	{
		try
		{
			if (!AVProMovieCapturePlugin.Init())
			{
				Debug.LogError("[AVProMovieCapture] Failed to initialise");
				return;
			}
		}
		catch (System.DllNotFoundException e)
		{
			_isFailedInit = true;
			Debug.LogError("[AVProMovieCapture] Unity couldn't find the plugin DLL, please move the 'Plugins' folder to the root of your project.");
			throw e;
		}

		// Video codec enumeration
		int numVideoCodecs = Mathf.Max(0, AVProMovieCapturePlugin.GetNumAVIVideoCodecs());
		_videoCodecNames = new string[numVideoCodecs + 2];
		_videoCodecNames[0] = "Uncompressed";
		_videoCodecNames[1] = null;
		_videoCodecConfigurable = new bool[numVideoCodecs + 2];
		_videoCodecConfigurable[0] = false;
		_videoCodecConfigurable[1] = false;
		for (int i = 0; i < numVideoCodecs; i++)
		{
			_videoCodecNames[i+2] = i.ToString("D2") + ") " + AVProMovieCapturePlugin.GetAVIVideoCodecName(i).Replace("/", "_");
			_videoCodecConfigurable[i+2] = AVProMovieCapturePlugin.IsConfigureVideoCodecSupported(i);
		}

		// Audio device enumeration
		int numAudioDevices = Mathf.Max(0, AVProMovieCapturePlugin.GetNumAVIAudioInputDevices());
		_audioDeviceNames = new string[numAudioDevices+2];
		_audioDeviceNames[0] = "Unity";
		_audioDeviceNames[1] = null;
		for (int i = 0; i < numAudioDevices; i++)
		{
			_audioDeviceNames[i + 2] = i.ToString("D2") + ") " + AVProMovieCapturePlugin.GetAVIAudioInputDeviceName(i).Replace("/", "_");
		}

		// Audio codec enumeration
		int numAudioCodecs = Mathf.Max(0, AVProMovieCapturePlugin.GetNumAVIAudioCodecs());
		_audioCodecNames = new string[numAudioCodecs+2];
		_audioCodecNames[0] = "Uncompressed";
		_audioCodecNames[1] = null;
		_audioCodecConfigurable = new bool[numAudioCodecs+2];
		_audioCodecConfigurable[0] = false;
		_audioCodecConfigurable[1] = false;
		for (int i = 0; i < numAudioCodecs; i++)
		{
			_audioCodecNames[i + 2] = i.ToString("D2") + ") " + AVProMovieCapturePlugin.GetAVIAudioCodecName(i).Replace("/", "_");
			_audioCodecConfigurable[i + 2] = AVProMovieCapturePlugin.IsConfigureAudioCodecSupported(i);
		}

		_isInit = true;
	}

	void OnEnable()
	{
		//Debug.Log("********** enable" + _isCreated);
		if (!_isCreated)
		{
			SetupWindow();
		}		
	}
	
	void OnDisable()
	{
		SaveSettings();
		StopCapture();
		_isInit = false;
		_isCreated = false;
		//Debug.Log("********** disable" + _isCreated);
		Repaint();
	}
		
	private void StartCapture()
	{	
		_lastFileSize = 0;
		CreateComponents();
		if (_capture != null)
		{
			_capture._audioCapture = _audioCapture;
			Configure(_capture);
			_capture.SelectCodec(false);
			if (!_capture._noAudio)
			{
				_capture.SelectAudioCodec(false);
				_capture.SelectAudioDevice(false);
			}
			_capture.QueueStartCapture();
		}		
	}
	
	private void StopCapture()
	{
		if (_capture != null)
		{
			if (_capture.IsCapturing())
			{
				_capture.StopCapture();
				_lastCapturePath = _capture.LastFilePath;
			}
			_capture = null;
			_captureScene = null;
			_captureCamera = null;
		}
		_audioCapture = null;
	}

	private static bool ShowInExplorer(string itemPath)
	{
		bool result = false;

	    itemPath = System.IO.Path.GetFullPath(itemPath.Replace(@"/", @"\"));   // explorer doesn't like front slashes
		if (System.IO.File.Exists(itemPath))
		{
			System.Diagnostics.Process.Start("explorer.exe", "/select," + itemPath);
			result = true;
		}

		return result;
	}

	// Updates 10 times/second
	void OnInspectorUpdate()
	{
		if (_capture != null)
		{
			if (Application.isPlaying)
			{
				_lastFileSize = _capture.GetCaptureFileSize();
				_lastEncodedSeconds = _capture.TotalEncodedSeconds;
				_lastEncodedMinutes = _lastEncodedSeconds / 60;
				_lastEncodedSeconds = _lastEncodedSeconds % 60;
			}
			else
			{
				StopCapture();
			}

		}
		else
		{
	        if (_queueConfigureVideoCodec >= 0)
	        {
	            int configureVideoCodec = _queueConfigureVideoCodec;
	            _queueConfigureVideoCodec = -1;
	            AVProMovieCapturePlugin.ConfigureVideoCodec(configureVideoCodec);
	        }

	        if (_queueConfigureAudioCodec >= 0)
	        {
	            int configureAudioCodec = _queueConfigureAudioCodec;
	            _queueConfigureAudioCodec = -1;
	            AVProMovieCapturePlugin.ConfigureAudioCodec(configureAudioCodec);
	        }				

			if (_queueStart && Application.isPlaying)
			{
				_queueStart = false;
				StartCapture();
			}
		}
		
		Repaint();
	}
	
	private static bool ShowConfigList(string title, string[] items, bool[] isConfigurable, ref int itemIndex, bool showConfig = true)
	{
		bool result = false;

		if (itemIndex < 0 || items == null)
			return result;
		
		EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
		EditorGUILayout.BeginHorizontal();
		itemIndex = EditorGUILayout.Popup(itemIndex, items);
		
		if (showConfig && isConfigurable != null && itemIndex < isConfigurable.Length)
		{
			EditorGUI.BeginDisabledGroup(itemIndex == 0 || !isConfigurable[itemIndex]);
			if (GUILayout.Button("Configure"))
			{
				result = true;
			}
			EditorGUI.EndDisabledGroup();
		}
		
		EditorGUILayout.EndHorizontal();		

		return result;
	}

    void OnGUI()
	{
		if (Application.platform != RuntimePlatform.WindowsEditor)
		{
			EditorGUILayout.LabelField("AVPro Movie Capture only works on the Windows platform.");
			return;
		}

		if (!_isInit)
		{
			if (_isFailedInit)
			{
				GUILayout.Label("Error", EditorStyles.boldLabel);
				GUI.enabled = false;
				GUILayout.TextArea("Unity couldn't find the AVPro Movie Capture plugin DLL.\n\nPlease move the 'Plugins' folder to the root of your project and try again.\n\nYou may then need to restart Unity for it to find the plugin DLLs.");
				GUI.enabled = true;
				return;
			}
			else
			{
				EditorGUILayout.LabelField("Initialising...");
				return;
			}
		}
		
		_scroll = EditorGUILayout.BeginScrollView(_scroll);
		
		DrawControlButtonsGUI();

		if (Application.isPlaying && _capture != null && _capture.IsCapturing())
		{
			DrawCapturingGUI();
		}
		if (_capture == null)
		{
			DrawConfigGUI();
		}

		EditorGUILayout.EndScrollView();
	}

	private void DrawControlButtonsGUI()
	{
		EditorGUILayout.BeginHorizontal();
		if (_capture == null)
		{
			GUI.backgroundColor = Color.green;
			if (GUILayout.Button("Start Capture"))
			{
				bool isReady = true;
				if (_sourceType == SourceType.Camera && _cameraNode == null)
				{
					Debug.LogError("[AVProMovieCapture] Please select a Camera to capture from, or select to capture from Scene.");
					isReady = false;
				}
				
				if (isReady)
				{
					if (!Application.isPlaying)
					{
						EditorApplication.isPlaying = true;
						_queueStart = true;
					}
					else
					{
						StartCapture();
						Repaint();
					}
				}
			}
		}
		else
		{
			GUI.backgroundColor = Color.red;
			if (GUILayout.Button("Stop Capture"))
			{
				StopCapture();
				Repaint();
			}
		}
		
		EditorGUI.BeginDisabledGroup(_capture == null);
		if (_capture != null && _capture.IsPaused())
		{
			GUI.backgroundColor = Color.green;
			if (GUILayout.Button("Resume Capture"))
			{
				_capture.ResumeCapture();
				Repaint();
			}
		}
		else
		{
			GUI.backgroundColor = Color.yellow;
			if (GUILayout.Button("Pause Capture"))
			{
				_capture.PauseCapture();
				Repaint();
			}
		}
		EditorGUI.EndDisabledGroup();
		
		EditorGUILayout.EndHorizontal();

		GUI.backgroundColor = Color.white;
	}


	private void DrawCapturingGUI()
	{
		GUILayout.Space(8.0f);
		GUILayout.Label("Output", EditorStyles.boldLabel);
		EditorGUILayout.BeginVertical("box");
		EditorGUI.indentLevel++;

		GUILayout.Label("Recording to: " + System.IO.Path.GetFileName(_capture.LastFilePath));
		GUILayout.Space(8.0f);

		GUILayout.Label("Video");
		EditorGUILayout.LabelField("Dimensions", _capture.GetRecordingWidth() + "x" + _capture.GetRecordingHeight() + " @ " + ((int)_capture._frameRate).ToString() + "hz");	
		EditorGUILayout.LabelField("Codec", _capture._codecName);

		if (!_capture._noAudio && _captureModeIndex == 0)
		{
			GUILayout.Label("Audio");
			EditorGUILayout.LabelField("Source", _capture._audioDeviceName);
			EditorGUILayout.LabelField("Codec", _capture._audioCodecName);
			if (_capture._audioDeviceName == "Unity")
			{
				EditorGUILayout.LabelField("Sample Rate", _capture._unityAudioSampleRate.ToString() + "hz");
				EditorGUILayout.LabelField("Channels", _capture._unityAudioChannelCount.ToString());
			}
		}

		EditorGUI.indentLevel--;
		EditorGUILayout.EndVertical();

		GUILayout.Space(8.0f);

		GUILayout.Label("Stats", EditorStyles.boldLabel);
		EditorGUILayout.BeginVertical("box");
		EditorGUI.indentLevel++;

		if (_capture._frameTotal >= (int)_capture._frameRate * 2)
		{
			Color originalColor = GUI.color;
			float fpsDelta = (_capture._fps - (int)_capture._frameRate);
			GUI.color = Color.red;
			if (fpsDelta > -10)
				GUI.color = Color.yellow;
			if (fpsDelta > -2)
				GUI.color = Color.green;

			EditorGUILayout.LabelField("Capture Rate", _capture._fps.ToString("F1") + " FPS");
			
			GUI.color = originalColor;
		}
		else
		{
			EditorGUILayout.LabelField("Capture Rate", ".. FPS");
		}

		EditorGUILayout.LabelField("File Size", (int)(_lastFileSize / (1024*1024)) + "MB");
		EditorGUILayout.LabelField("Video Length", _lastEncodedMinutes + ":" + _lastEncodedSeconds + "s");
		EditorGUILayout.LabelField("Encoded Frames", _capture.NumEncodedFrames.ToString());

		GUILayout.Label("Dropped Frames");
		EditorGUILayout.LabelField("In Unity", _capture.NumDroppedFrames.ToString());
		EditorGUILayout.LabelField("In Encoder", _capture.NumDroppedEncoderFrames.ToString());

		EditorGUI.indentLevel--;
		EditorGUILayout.EndVertical();
	}

	private void DrawConfigGUI()
	{
		EditorGUI.BeginDisabledGroup(string.IsNullOrEmpty(_lastCapturePath));
		if (GUILayout.Button("Open Last Video"))
		{
			if (!ShowInExplorer(_lastCapturePath))
			{
				_lastCapturePath = string.Empty;
			}
		}
		EditorGUI.EndDisabledGroup();
		_expandConfig = EditorGUILayout.Foldout(_expandConfig, "Configure");
		if (_expandConfig)
		{	
			GUILayout.Label("General", EditorStyles.boldLabel);
			EditorGUILayout.BeginVertical("box");			
			EditorGUI.indentLevel++;
			
			_captureModeIndex = EditorGUILayout.Popup("Mode", _captureModeIndex, _captureModes);
			
			// Source
			EditorGUILayout.LabelField("Source", EditorStyles.boldLabel);
			EditorGUI.indentLevel++;
			_sourceType = (SourceType)EditorGUILayout.EnumPopup("Type", _sourceType);
			if (_sourceType == SourceType.Camera)
			{
				if (_cameraNode == null && Camera.main != null)
				{
					_cameraNode = Camera.main;
				}
				_cameraNode = (Camera)EditorGUILayout.ObjectField("Camera", _cameraNode, typeof(Camera), true);
				
				_cameraFastPixel = EditorGUILayout.Toggle("Fast Pixel Format", _cameraFastPixel);
			}
			EditorGUI.indentLevel--;
			
		
			// File name
			EditorGUILayout.LabelField("File Name", EditorStyles.boldLabel);
			EditorGUI.indentLevel++;
			_autoFilenamePrefixFromSceneName = EditorGUILayout.Toggle("From Scene Name", _autoFilenamePrefixFromSceneName);
			if (_autoFilenamePrefixFromSceneName)
			{
				_autoFilenamePrefix = System.IO.Path.GetFileNameWithoutExtension(EditorApplication.currentScene);
				if (string.IsNullOrEmpty(_autoFilenamePrefix))
				{
					_autoFilenamePrefix = "capture";
				}
			}
			EditorGUI.BeginDisabledGroup(_autoFilenamePrefixFromSceneName);
			_autoFilenamePrefix = EditorGUILayout.TextField("Prefix", _autoFilenamePrefix);
			EditorGUI.EndDisabledGroup();
			_autoFilenameExtension = EditorGUILayout.TextField("Extension", _autoFilenameExtension);
			_appendTimestamp = EditorGUILayout.Toggle("Append Timestamp", _appendTimestamp);
			EditorGUI.indentLevel--;
			
			// File path
			EditorGUILayout.LabelField("File Path", EditorStyles.boldLabel);
			EditorGUI.indentLevel++;
			_outputFolderIndex = EditorGUILayout.Popup("Relative to", _outputFolderIndex, _outputFolders);
			if (_outputFolderIndex == 0)
			{
				_outputFolderRelative = EditorGUILayout.TextField("SubFolder(s)", _outputFolderRelative);
			}
			else
			{
				EditorGUILayout.BeginHorizontal();
				_outputFolderAbsolute = EditorGUILayout.TextField("Path", _outputFolderAbsolute);
				if (GUILayout.Button(">", GUILayout.Width(22)))
				{
					_outputFolderAbsolute = EditorUtility.SaveFolderPanel("Select Folder To Store Video Captures", System.IO.Path.GetFullPath(System.IO.Path.Combine(Application.dataPath, "../")), "");				
					EditorUtility.SetDirty(this);
				}
				EditorGUILayout.EndHorizontal();
			}
			EditorGUI.indentLevel--;
			
			
			EditorGUI.indentLevel--;
			EditorGUILayout.EndVertical();
			
			
			GUILayout.Label("Video", EditorStyles.boldLabel);
			EditorGUILayout.BeginVertical("box");
			EditorGUI.indentLevel++;

			if (_sourceType == 0)
			{
				// We can't just use Screen.width and Screen.height because Unity returns the size of this window
				// So instead we look for a camera with no texture target and a valid viewport
				int inWidth = 1;
				int inHeight = 1;
				foreach (Camera cam in Camera.allCameras)
				{
					if (cam.targetTexture == null)
					{
						float rectWidth = Mathf.Clamp01(cam.rect.width + cam.rect.x) - Mathf.Clamp01(cam.rect.x);
						float rectHeight = Mathf.Clamp01(cam.rect.height + cam.rect.y) - Mathf.Clamp01(cam.rect.y);
						if (rectWidth > 0.0f && rectHeight > 0.0f)
						{
							inWidth = Mathf.FloorToInt(cam.pixelWidth / rectWidth);
							inHeight = Mathf.FloorToInt(cam.pixelHeight / rectHeight);
							//Debug.Log (rectWidth + "    " + (cam.rect.height - cam.rect.y) + " " + cam.pixelHeight + " = " + inWidth + "x" + inHeight);
						}
						break;
					}
				}
				Vector2 outSize = AVProMovieCaptureBase.GetRecordingResolution(inWidth, inHeight, GetDownScaleFromIndex(_downScaleIndex), new Vector2(_downscaleX, _downscaleY));
				EditorGUILayout.LabelField("Output", (int)outSize.x + " x " + (int)outSize.y + " @ " + (int)_frameRate, EditorStyles.boldLabel);
			}
			else
			{
				if (_cameraNode)
				{
					int inWidth = Mathf.FloorToInt(_cameraNode.pixelRect.width);
					int inHeight = Mathf.FloorToInt(_cameraNode.pixelRect.height);
					Vector2 outSize = AVProMovieCaptureBase.GetRecordingResolution(inWidth, inHeight, GetDownScaleFromIndex(_downScaleIndex), new Vector2(_downscaleX, _downscaleY));
					EditorGUILayout.LabelField("Output", (int)outSize.x + " x " + (int)outSize.y + " @ " + (int)_frameRate, EditorStyles.boldLabel);
				}
			}

			_downScaleIndex = EditorGUILayout.Popup("Down Scale", _downScaleIndex, _downScales);
			if (_downScaleIndex == 5)
			{
				Vector2 maxVideoSize = new Vector2(_downscaleX, _downscaleY);
				maxVideoSize = EditorGUILayout.Vector2Field("Size", maxVideoSize);
				_downscaleX = Mathf.Clamp((int)maxVideoSize.x, 1, 4096);
				_downscaleY = Mathf.Clamp((int)maxVideoSize.y, 1, 4096);
			}
			
			_frameRate = (AVProMovieCaptureBase.FrameRate)EditorGUILayout.EnumPopup("Frame Rate", _frameRate);

			if (ShowConfigList("Codec", _videoCodecNames, _videoCodecConfigurable, ref _videoCodecIndex))
			{
				_queueConfigureVideoCodec = Mathf.Max(-1, _videoCodecIndex - 2);
			}

			EditorGUI.indentLevel--;
			EditorGUILayout.EndVertical();

			
			EditorGUI.BeginDisabledGroup(_captureModeIndex != 0);
			GUILayout.Label("Audio", EditorStyles.boldLabel);
			EditorGUILayout.BeginVertical("box");
			EditorGUI.indentLevel++;

			_captureAudio = EditorGUILayout.Toggle("Capture Audio", _captureAudio);
			EditorGUI.BeginDisabledGroup(!_captureAudio);
			if (ShowConfigList("Source", _audioDeviceNames, null, ref _audioDeviceIndex, false))
			{
			}
			if (ShowConfigList("Codec", _audioCodecNames, _audioCodecConfigurable, ref _audioCodecIndex))
			{
				_queueConfigureAudioCodec = Mathf.Max(-1, _audioCodecIndex - 2);
			}
			EditorGUI.EndDisabledGroup();

			EditorGUI.indentLevel--;
			EditorGUILayout.EndVertical();
			EditorGUI.EndDisabledGroup();

			GUILayout.Space(16f);
			if (GUILayout.Button("Reset Settings"))
			{
				ResetSettings();
			}
			GUILayout.Space(4f);
		}
	}
}