using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightObj : Action<Main> {
	//public Light thelight;
	public float intensity;

	public override void DoAction(Main m)
	{
		//thelight.gameObject.SetActive (true);
		//thelight.intensity = intensity;
	    GameObject.Find("Directional Light").GetComponent<Light>().intensity = intensity;
        Debug.LogError("555555555555");
	}
}
