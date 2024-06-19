using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TransScale : Action<Main>
{
    /// <summary>
    /// 目标
    /// </summary>
    public Vector3 Target_V3;
    /// <summary>
    /// 缩放时间
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
        //平滑缩放
        Vector3 t = new Vector3(target.transform.localScale.x * Target_V3.x, target.transform.localScale.y * Target_V3.y, target.transform.localScale.z * Target_V3.z);
        target.transform.DOScale(t, durationTime);
    }
	
}
