using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class MotionTriggerInforma : ActionInforma {
	public float px, py, pz, rx, ry, rz, sx, sy, sz;

	public MotionTriggerInforma(bool isOnce)
	{
		this.isOnce = isOnce;
	}
}
