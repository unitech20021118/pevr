using UnityEngine;
using System.Text;
using System.Collections;

//-----------------------------------------------------------------------------
// Copyright 2012-2015 RenderHeads Ltd.  All rights reserved.
//-----------------------------------------------------------------------------

public class MovieCaptureDemo : MonoBehaviour 
{
	public AudioClip _audioBG;
	public AudioClip _audioHit;
	public float _speed = 1.0f;
	public AVProMovieCaptureBase _capture;
	public GUISkin _guiSkin;
	private float _timer;
		
	void Start()
	{	
		if (_audioBG != null)
			AudioSource.PlayClipAtPoint(_audioBG, Vector3.zero);
	}
	
	void Update()
	{	
		if (Input.GetKeyDown(KeyCode.S))
		{
			if (_audioHit)
				AudioSource.PlayClipAtPoint(_audioHit, Vector3.zero);
			//Camera.main.backgroundColor = new Color(Random.value, Random.value, Random.value, 0);
		}
		
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (_capture != null && _capture.IsCapturing())
			{
				_capture.StopCapture();
			}
			else
			{
				Application.Quit();
			}
		}
		
		// Spin the camera around
		//Camera.main.transform.RotateAround(Vector3.zero, Vector3.up, 20f * Time.deltaTime * _speed);

		// Make cubes jump
		_timer += Time.deltaTime * _speed;
		if (_timer >= 1f)
		{
			_timer = 0f;
			object[] objs = FindObjectsOfType(typeof(Rigidbody));
			foreach (object o in objs)
			{
				((Rigidbody)o).AddForce(Vector3.up * 200f);
			}
		}
	}
	
	void OnGUI()
	{
		GUI.skin = _guiSkin;
		Rect r = new Rect(Screen.width - 108, 64, 128, 28);
		GUI.Label(r, "Frame " + Time.frameCount);
	}
}
