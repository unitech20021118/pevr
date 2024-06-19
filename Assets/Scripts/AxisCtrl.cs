using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisCtrl : MonoBehaviour {
	public CamRayCtrl camRayCtrl;
	public int axisID;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnMouseDown(){
		
	}

	public void SetAxis(){
		switch (axisID) {
		case 0:
			camRayCtrl.isX = true;
			break;
		case 1:
			camRayCtrl.isY = true;
			break;
		case 2:
			camRayCtrl.isZ = true;
			break;
		}
	}
}
