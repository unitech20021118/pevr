using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class FollowPlayerInforma : ActionInforma {

	public FollowPlayerInforma(bool isOnce)
	{
		this.isOnce = isOnce;
	}
}
