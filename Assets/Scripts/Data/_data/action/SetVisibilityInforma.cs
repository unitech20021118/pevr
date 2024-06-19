using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class SetVisibilityInforma : ActionInforma {
	public string targetName,rootName;
	public int isVisible=1;

	public SetVisibilityInforma(bool isOnce)
	{
		this.isOnce = isOnce;
	}
}
