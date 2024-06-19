using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class AddLightInforma : ActionInforma
{
	public string targetName;
    public string lightColor;
    public string z = "3";
    public string x = "1";
    //edit by kuai 
    //添加的灯光的相对位置
    public float LightLocalPositionX;
    public float LightLocalPositionY;
    public float LightLocalPositionZ;

    //edit by 王梓亦
    public string gstyle;

    public AddLightInforma(bool isOnce)
    {
        this.isOnce = isOnce;
    }
}
