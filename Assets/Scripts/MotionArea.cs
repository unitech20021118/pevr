using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionArea : MonoBehaviour {
	public bool isValidTime, isStay, isExit;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider collider){
		
		StartCoroutine (ValidMotionTime (1));
		//isStay = true;
		isExit = false;
	}

	void OnTriggerStay(){
		isStay = true;
	}

	void OnTriggerExit(){
		isExit = true;
	}

	IEnumerator ValidMotionTime(float time){
		isValidTime = true;
		yield return new WaitForSeconds (time);
		isValidTime = false;
	}
}
