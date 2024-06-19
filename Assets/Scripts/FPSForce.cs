using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSForce : MonoBehaviour {

	public float forceNum;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnControllerColliderHit(ControllerColliderHit c){
		Rigidbody rig = c.transform.GetComponent<Rigidbody> ();
		if (rig) {
			rig.AddForceAtPosition (transform.forward * forceNum, transform.position);
		}
	}
}
