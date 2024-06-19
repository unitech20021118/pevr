using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
/// <summary>
/// created by kuai
/// </summary>
public class TransRotate : Action<Main>
{
    /// <summary>
    /// 目标
    /// </summary>
    public Vector3 Target_V3;
    /// <summary>
    /// 移动时间
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
        //平滑旋转
        target.transform.DOLocalRotate(Target_V3, durationTime, RotateMode.WorldAxisAdd);
    }
}
