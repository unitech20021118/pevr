using UnityEngine;
using System.Collections;

public class WebcamCaptureDemo : MonoBehaviour 
{
	class Instance
	{
		public string name;
		public WebCamTexture texture;
		public AVProMovieCaptureFromTexture capture;
		public AVProMovieCaptureGUI gui;
	}

	public GUISkin _skin;
	public GameObject _prefab;
	private Instance[] _instances;
	private int _selectedWebcamIndex;
	
	void Start() 
	{	
		// Create instance data per webcam
		int numCams = WebCamTexture.devices.Length;
		_instances = new Instance[numCams];
		for (int i = 0 ; i < numCams; i++)
		{
			GameObject go = (GameObject)GameObject.Instantiate(_prefab);
			Instance instance = new Instance();
			instance.name = WebCamTexture.devices[i].name;
			instance.capture = go.GetComponent<AVProMovieCaptureFromTexture>();
			instance.capture._autoFilenamePrefix = "Demo4Webcam-" + i;
			instance.gui = go.GetComponent<AVProMovieCaptureGUI>();
			instance.gui._showUI = false;
			_instances[i] = instance;
		}

        if (numCams > 0)
        {
            Change(0);
        }
	}
	
	private void StartWebcam(Instance instance)
	{
		instance.texture = new WebCamTexture(instance.name, 640, 480, 30);
		instance.texture.Play();
		if (instance.texture.isPlaying)
		{
			bool requiresPOT = (SystemInfo.npotSupport == NPOTSupport.None);
			if (requiresPOT)
			{
				// WebCamTexture actually uses a power of 2 texture so we need to only grab a region of it
				float p2Width = Mathf.NextPowerOfTwo(instance.texture.width);
				float p2Height = Mathf.NextPowerOfTwo(instance.texture.height);			
				instance.capture.SetSourceTextureRegion(instance.texture, new Rect(0, 0, instance.texture.width / p2Width, instance.texture.height / p2Height));
			}
			else
			{
				instance.capture.SetSourceTexture(instance.texture);
			}
		}
		else
		{
			StopWebcam(instance);
		}
	}

	private void StopWebcam(Instance instance)
	{
		if (instance.texture != null)
		{
            if (instance.capture != null && instance.capture.IsCapturing())
			{
				instance.capture.StopCapture();
			}

			instance.texture.Stop();
			Destroy(instance.texture);
			instance.texture = null;
		}
	}
	
	void OnDestroy()
	{
		for (int i = 0; i < _instances.Length; i++)
		{
			StopWebcam(_instances[i]);
		}
	}

	private void Change(int index)
	{
		_selectedWebcamIndex = index;
		for (int j = 0; j < _instances.Length; j++)
		{
			_instances[j].gui._showUI = (j == _selectedWebcamIndex);
		}
	}
	
	void OnGUI()
	{
		GUI.skin = _skin;
		GUILayout.BeginArea(new Rect(Screen.width - 512, 0, 512, Screen.height));
		GUILayout.BeginVertical();

		GUILayout.Label("Select webcam:");

		for (int i = 0; i < _instances.Length; i++)
		{
			Instance webcam = _instances[i];

            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));

            if (_selectedWebcamIndex == i)
            {
                GUILayout.Label("->", GUILayout.Width(32f));
            }
            else
            {
                GUILayout.Label(" ", GUILayout.Width(32f));
            }

            if (webcam.capture.IsCapturing())
            {
                float t = Mathf.PingPong(Time.timeSinceLevelLoad, 0.25f) * 4f;
                GUI.backgroundColor = Color.Lerp(GUI.backgroundColor, Color.white, t);
                GUI.color = Color.Lerp(Color.red, Color.white, t);

            }

			if (GUILayout.Button(webcam.name, GUILayout.Width(200), GUILayout.ExpandWidth(true)))
			{
				Change(i);
			}
            GUI.backgroundColor = Color.white;
            GUI.color = Color.white;

			if (webcam.texture == null)
			{
				if (GUILayout.Button("Play", GUILayout.Width(64f)))
				{
					StartWebcam(webcam);
					Change(i);
				}
			}
			else
			{
				if (GUILayout.Button("Stop", GUILayout.Width(64f)))
				{
					StopWebcam(webcam);
					Change(i);
				}
			}

			if (webcam.texture != null)
			{
				Rect camRect = GUILayoutUtility.GetRect(256, 256.0f / (webcam.texture.width / (float)webcam.texture.height));
				GUI.DrawTexture(camRect, webcam.texture);
			}
			else
			{
				GUILayout.Label("No signal...", GUILayout.MinWidth(256.0f), GUILayout.MaxWidth(256.0f), GUILayout.ExpandWidth(false));
			}

			GUILayout.EndHorizontal();
		}

		GUILayout.EndVertical();
		GUILayout.EndArea();
	}
}