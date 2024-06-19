using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PhysicalSettingInforma : ActionInforma {
    /// <summary>
    /// 质量
    /// </summary>
	public float massNum = 1;
    /// <summary>
    /// 摩擦系数
    /// </summary>
	public float factor = 0;
    /// <summary>
    /// 是否受重力
    /// </summary>
	public bool isGravity;
    //public bool isBoxCollider=true;

	public PhysicalSettingInforma(bool isOnce)
	{
		this.isOnce = isOnce;
	}
}
