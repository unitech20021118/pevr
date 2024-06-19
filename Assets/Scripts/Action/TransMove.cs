using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 控制物体移动的动作的脚本   created by kuai
/// </summary>
public class TransMove : Action<Main> {
    /// <summary>
    /// 移动目标位置
    /// </summary>
    public Vector3 Target_V3;
    /// <summary>
    /// 持续时间
    /// </summary>
    public float durationTime;
    /// <summary>
    /// 目标名称
    /// </summary>
    public string targetName;
    /// <summary>
    /// 目标物体
    /// </summary>
    public GameObject target;
	// Use this for initialization
    public override void DoAction(Main m)
    {
        if (target == null)
        {
            if (string.IsNullOrEmpty(targetName))
            {
                target = m.gameObject;
            }
            else
            {
                target = GameObject.Find("Parent/" + targetName);
            }
        }
        Vector3 ta = Target_V3 + target.transform.position;
        //平滑移动
        target.transform.DOLocalMove(ta, durationTime);
    }
}
