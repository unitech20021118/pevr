using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.eulerAngles = new Vector3(0, 0, Camera.main.transform.eulerAngles.y);
	}
}
