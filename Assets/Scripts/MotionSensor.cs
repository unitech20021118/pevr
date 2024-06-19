using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionSensor : MonoBehaviour {
	public MotionArea area1, area2,midArea;
	public GameObject model;
	public Motion motion=Motion.noMotion;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Manager.Instace._Playing) {
			model.SetActive (false);
		} else {
			model.SetActive (true);
		}
		if (area1.isValidTime && area2.isValidTime && midArea.isValidTime) {
			motion = Motion.moveThrough;
		} else {
			motion = Motion.noMotion;
		}
	}
}

public enum Motion{
	moveThrough,noMotion
}
