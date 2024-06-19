using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ShowMsgInforma : ActionInforma {
	public string msg;

	public ShowMsgInforma(bool isOnce)
	{
		this.isOnce = isOnce;
	}
}
