using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ChangeColorInforma :ActionInforma {

    public string color;
	public string targetName;
	public string rootName;
	public float time;

    public ChangeColorInforma(bool isOnce)
    {
        this.isOnce = isOnce;
    }
}
