using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class TranslateInforma : ActionInforma {
	public string targetName;
	public float tx=0, ty=0, tz=0, tt = 0, rx=0, ry=0, rz=0, rt=0, sx=1, sy=1, sz=1, st=0;
	public bool isKeepGo;

	public TranslateInforma(bool isOnce){
		this.isOnce = isOnce;
	}
}
