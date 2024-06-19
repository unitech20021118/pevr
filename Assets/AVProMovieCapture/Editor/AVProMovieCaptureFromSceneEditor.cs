using UnityEngine;
using UnityEditor;
using System.Collections;

//-----------------------------------------------------------------------------
// Copyright 2012-2013 RenderHeads Ltd.  All rights reserved.
//-----------------------------------------------------------------------------

[CustomEditor(typeof(AVProMovieCaptureFromScene))]
public class AVProMovieCaptureFromSceneEditor : Editor
{
	private AVProMovieCaptureFromScene _capture;
	
	public override void OnInspectorGUI()
	{
		_capture = (this.target) as AVProMovieCaptureFromScene;
		
		DrawDefaultInspector();
		//ConfigGUI();
				
		GUILayout.Space(8.0f);
		
		if (Application.isPlaying)
		{		
			if (!_capture.IsCapturing())
			{
				GUI.backgroundColor = Color.green;
		   		if (GUILayout.Button("Start Recording"))
				{
					_capture.SelectCodec(false);
					_capture.SelectAudioDevice(false);
					// We have to queue the start capture otherwise Screen.width and height aren't correct
					_capture.QueueStartCapture();
				}
				GUI.backgroundColor = Color.white;
			}
			else
			{				
				GUILayout.BeginHorizontal();
				if (_capture._frameTotal > (int)_capture._frameRate * 2)
				{
					Color originalColor = GUI.color;
					float fpsDelta = Mathf.Abs(_capture._fps - (int)_capture._frameRate);
					GUI.color = Color.red;
					if (fpsDelta < 10)
						GUI.color = Color.yellow;
					if (fpsDelta < 2)
						GUI.color = Color.green;
					GUILayout.Label("Recording at " + _capture._fps.ToString("F1") + " fps");
					
					GUI.color = originalColor;
				}
				else
				{
					GUILayout.Label("Recording at ... fps");	
				}
					
				if (!_capture.IsPaused())
				{
					GUI.backgroundColor = Color.yellow;
					if (GUILayout.Button("Pause Capture"))
					{
						_capture.PauseCapture();
					}
				}
				else
				{
					GUI.backgroundColor = Color.green;
					if (GUILayout.Button("Resume Capture"))
					{
						_capture.ResumeCapture();
					}					
				}
				GUI.backgroundColor = Color.red;
		   		if (GUILayout.Button("Stop Recording"))
				{
					_capture.StopCapture();
				}				
				GUILayout.EndHorizontal();
				
				GUI.backgroundColor = Color.white;
				
				GUILayout.Space(8.0f);
				GUILayout.Label("Recording at: " + _capture.GetRecordingWidth() + "x" + _capture.GetRecordingHeight() + " @ " + ((int)_capture._frameRate).ToString() + "fps");
				GUILayout.Space(8.0f);
				GUILayout.Label("Using video codec: '" + _capture._codecName + "'");
				GUILayout.Label("Using audio device: '" + _capture._audioDeviceName + "'");
			}	
		}
	}
	
	private void ConfigGUI()
	{
		GUILayout.Label("General", EditorStyles.boldLabel);
		EditorGUILayout.BeginVertical("box");			
		EditorGUI.indentLevel++;
		
		// Capture Mode
		{
			string[] captureModes = { "Realtime", "Offline" };
			int captureModeIndex = 0;
			if (!_capture._isRealTime)
				captureModeIndex = 1;
			captureModeIndex = EditorGUILayout.Popup("Mode", captureModeIndex, captureModes);
			_capture._isRealTime = (captureModeIndex == 0);
		}
		
		// File Name
		{
			EditorGUILayout.LabelField("File Name", EditorStyles.boldLabel);
			_capture._autoFilenamePrefix = EditorGUILayout.TextField("Prefix", _capture._autoFilenamePrefix);
			_capture._autoFilenameExtension = EditorGUILayout.TextField("Extension", _capture._autoFilenameExtension);
			_capture._autoGenerateFilename = EditorGUILayout.Toggle("Append Timestamp", _capture._autoGenerateFilename);
		}
		
		// File Path
		{
			EditorGUILayout.LabelField("File Path", EditorStyles.boldLabel);
			string[] outputFolders = { "Project Folder", "Absolute Folder" };
			int outputFolderIndex = 0;
			if (_capture._outputFolderType == AVProMovieCaptureBase.OutputPath.Absolute)
				outputFolderIndex = 1;
			int newOutputFolderIndex = EditorGUILayout.Popup("Relative to", outputFolderIndex, outputFolders);
			if (newOutputFolderIndex != outputFolderIndex)
			{
				_capture._outputFolderPath = string.Empty;
				outputFolderIndex = newOutputFolderIndex;
			}
			if (outputFolderIndex == 0)
			{
				_capture._outputFolderType = AVProMovieCaptureBase.OutputPath.RelativeToProject;
				_capture._outputFolderPath = EditorGUILayout.TextField("SubFolder(s)", _capture._outputFolderPath);
			}
			else
			{
				EditorGUILayout.BeginHorizontal();
				_capture._outputFolderType = AVProMovieCaptureBase.OutputPath.Absolute;
				_capture._outputFolderPath = EditorGUILayout.TextField("Path", _capture._outputFolderPath);
				if (GUILayout.Button(">", GUILayout.Width(22)))
				{
					_capture._outputFolderPath = EditorUtility.SaveFolderPanel("Select Folder To Store Video Captures", System.IO.Path.GetFullPath(System.IO.Path.Combine(Application.dataPath, "../")), "");
				}
				EditorGUILayout.EndHorizontal();
			}
		}
		
		EditorGUI.indentLevel--;
		EditorGUILayout.EndVertical();
	
		GUILayout.Label("Video", EditorStyles.boldLabel);
		EditorGUILayout.BeginVertical("box");
		EditorGUI.indentLevel++;
		
		// Downscale
		{
			string[] downScales = { "Original", "Half", "Quarter", "Eighth", "Sixteenth", "Specific" };
			AVProMovieCaptureBase.DownScale[] downScaleEnums = 
			{ 
				AVProMovieCaptureBase.DownScale.Original, AVProMovieCaptureBase.DownScale.Half, AVProMovieCaptureBase.DownScale.Quarter,
				AVProMovieCaptureBase.DownScale.Eighth, AVProMovieCaptureBase.DownScale.Sixteenth, AVProMovieCaptureBase.DownScale.Specific 
			};
			int downScaleIndex = 0;
			for (int i = 0; i < downScaleEnums.Length; i++)
			{
				if (downScaleEnums[i] ==_capture._downScale)
				{
					downScaleIndex = i;
					break;
				}
			}
			downScaleIndex = EditorGUILayout.Popup("Down Scale", downScaleIndex, downScales);
			if (downScaleIndex == 5)
			{
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("");
				_capture._maxVideoSize.x = EditorGUILayout.IntField((int)_capture._maxVideoSize.x);
				_capture._maxVideoSize.x = Mathf.Clamp((int)_capture._maxVideoSize.x, 1, 4096);
				EditorGUILayout.LabelField("x", GUILayout.ExpandWidth(false), GUILayout.Width(32));
				_capture._maxVideoSize.y = EditorGUILayout.IntField((int)_capture._maxVideoSize.y);
				_capture._maxVideoSize.y = Mathf.Clamp((int)_capture._maxVideoSize.y, 1, 4096);
				EditorGUILayout.EndHorizontal();
			}
			
			_capture._downScale = downScaleEnums[downScaleIndex];
		}
		
		
		// Frame Rate
		{
			_capture._frameRate = (AVProMovieCaptureBase.FrameRate)EditorGUILayout.EnumPopup("Frame Rate", _capture._frameRate);
		}

		EditorGUI.indentLevel--;
		EditorGUILayout.EndVertical();
		
		

		EditorGUI.BeginDisabledGroup(!_capture._isRealTime);
		GUILayout.Label("Audio", EditorStyles.boldLabel);
		EditorGUILayout.BeginVertical("box");
		EditorGUI.indentLevel++;
		
		// Capture Audio
		{
			_capture._noAudio = !EditorGUILayout.Toggle("Capture Audio", !_capture._noAudio);
		}
		
		// audio device
		// 1) from microphone, create list or force index
		// 2) from unity listener, easy
		
		// Capture Audio from Unity
		{
			EditorGUI.BeginDisabledGroup(_capture._forceAudioDeviceIndex == 1);// TODO: fix this
			EditorGUILayout.ObjectField("Audio Capture", _capture._audioCapture, typeof(AVProUnityAudioCapture), true);
			EditorGUI.EndDisabledGroup();
		}
		
		EditorGUI.indentLevel--;
		EditorGUILayout.EndVertical();
		EditorGUI.EndDisabledGroup();		
		
		if (GUI.changed)
		{
			EditorUtility.SetDirty(_capture);
		}
	}	
}