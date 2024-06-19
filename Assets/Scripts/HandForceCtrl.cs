using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class HandForceCtrl : MonoBehaviour {
	public float forceValue{ get; set; }
	public GameObject handCollider;

	// Use this for initialization
	void Start () {
		

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
	}

	public void SetForce(float forceNum){
		forceValue =  forceNum;
	}

	public void SetCollider(bool isActive){
		handCollider.GetComponent<BoxCollider> ().enabled = isActive;
	}
}
