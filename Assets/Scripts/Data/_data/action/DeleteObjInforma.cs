using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class DeleteObjInforma : ActionInforma {
    public string targetName, rootName;
	public DeleteObjInforma(bool isOnce){
		this.isOnce = isOnce;
	}
}
