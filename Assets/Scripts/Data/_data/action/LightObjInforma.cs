using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class LightObjInforma : ActionInforma {
	public float indensity;

	public LightObjInforma(bool isOnce){
		this.isOnce = isOnce;
	}
}
