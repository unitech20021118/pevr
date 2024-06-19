using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class DragactionInforma : ActionInforma {
	public float rotateS;

	public DragactionInforma(bool isOnce)
	{
		this.isOnce = isOnce;
	}
}
