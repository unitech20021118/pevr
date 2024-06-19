using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ShowVideoInforma : ActionInforma {

    //public float rotate, x, y, z;
    public float w = 260;
    public float h = 180;
    public string videopath;
    public int ajustMode = 1;
    //public bool clickOpen;
    public float px, py, pz;
    /// <summary>
    /// 默认自动播放
    /// </summary>
    public bool AutoPlay = true;
    /// <summary>
    /// 默认不循环播放
    /// </summary>
    public bool Loop = false;
    public ShowVideoInforma(bool isOnce)
	{
		this.isOnce = isOnce;
	}
}
