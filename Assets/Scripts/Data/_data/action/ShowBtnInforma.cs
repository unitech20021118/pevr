using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ShowBtnInforma : ActionInforma
{
    public float w, h, mx, my, mz;
	public int targetID;
    public ShowBtnInforma()
    {
       
    }
    public ShowBtnInforma(bool isOnce)
	{
		this.isOnce = isOnce;
	}
}
