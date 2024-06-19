using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ShowImgInforma : ActionInforma {
	public float rotate,x,y,z,w=260,h=180;
	public string imgName;
	public int ajustMode;
	public bool clickOpen;
	public float px, py, pz;
    public bool ISBG;
	//public float durtime;

	public ShowImgInforma(bool isOnce)
	{
		this.isOnce = isOnce;
	}
}
