using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class SetTaskInforma : ActionInforma {
	public string taskName;
	public string taskDescribe;
	public string[] roles;

	public SetTaskInforma(bool isOnce){
		this.isOnce = isOnce;
	}
}
