using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraCamera : MonoBehaviour {
	public static ExtraCamera extraCam;
	public Camera cam;


    void Awake()
    {
        extraCam = this;
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
//		if (Input.GetMouseButtonDown (1)) {
//			Manager.Instace.FirstPerson.SetActive (true);
//			gameObject.SetActive (false);
//		}
//		if (isKeepGo) {
//			Invoke ("HideSelf", 1f);
//		} else {
//			gameObject.SetActive (false);
//			isKeepGo = true;
//		}
	}
}
