using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TransScaleInforma : ActionInforma
{
/// <summary>
    /// 目标名字
    /// </summary>
    public string targetName;
    /// <summary>
    /// 记录目标的x
    /// </summary>
    public float trans_X=1;
    /// <summary>
    /// 记录目标的y
    /// </summary>
    public float trans_Y=1;
    /// <summary>
    /// 记录目标的z
    /// </summary>
    public float trans_Z=1;
    /// <summary>
    /// 持续时间
    /// </summary>
    public float durationTime;
    public TransScaleInforma(bool isOnce)
    {
		this.isOnce = isOnce;
	}
}
