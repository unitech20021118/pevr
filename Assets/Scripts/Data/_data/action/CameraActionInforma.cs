using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class CameraActionInforma : ActionInforma
{
    public bool close;
	public int type;
	public float px,py,pz,rx,ry,rz;
	public float t=0;
	public float speed=0;

	public CameraActionInforma(bool isOnce)
	{
		this.isOnce = isOnce;
	}
}
