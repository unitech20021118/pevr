using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testt : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		Vector3 vec = Camera.main.WorldToScreenPoint(transform.position);
		Debug.Log(vec);
	}
}
