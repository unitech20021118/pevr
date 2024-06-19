using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class LoadSceneInforma : ActionInforma {
	public string filePath;
	public bool ISVR;

	public LoadSceneInforma(bool isOnce)
	{
		this.isOnce = isOnce;
	}
}
