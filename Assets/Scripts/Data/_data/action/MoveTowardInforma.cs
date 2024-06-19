using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class MoveTowardInforma : ActionInforma {
	public string targetName;
	public bool RunToggle;
	public float t=0;
	public List<float> xs;
	public List<float> ys;
	public List<float> zs;

	//public List<Vector3> points;


	public MoveTowardInforma(bool isOnce)
	{
		this.isOnce = isOnce;
	}

	public void Addxs(float x){
		if (xs == null) {
			xs = new List<float> ();
		}
		xs.Add (x);
	}

	public void Addys(float x){
		if (ys == null) {
			ys = new List<float> ();
		}
		ys.Add (x);
	}

	public void Addzs(float x){
		if (zs == null) {
			zs = new List<float> ();
		}
		zs.Add (x);
	}

	public void Removexs(){
		if (xs == null) {
			return;
		}
		xs.RemoveAt (xs.Count - 1);
	}

	public void Removeys(){
		if (ys == null) {
			return;
		}
		ys.RemoveAt (ys.Count - 1);
	}

	public void Removezs(){
		if (zs == null) {
			return;
		}
		zs.RemoveAt (zs.Count - 1);
	}
}
