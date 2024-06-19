using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class MoveTarget : Action<Main>
{
    /// <summary>
    /// 开关
    /// </summary>
    public bool ONOFF;
    /// <summary>
    /// 第一人称
    /// </summary>
    public bool FPS;
    /// <summary>
    /// 目标
    /// </summary>
    public GameObject target;
    /// <summary>
    /// 目标路径
    /// </summary>
    public string targetPath;
    /// <summary>
    /// 移动速度
    /// </summary>
    public float speed;
    /// <summary>
    /// 无视重力
    /// </summary>
    public bool noGravity;
    public override void DoAction(Main m)
    {
        MoveTargetController mtc;
        mtc = m.gameObject.GetComponent<MoveTargetController>();
        if (ONOFF)
        {
            if (mtc == null)
            {
                mtc = m.gameObject.AddComponent<MoveTargetController>();
            }
            if (!mtc.enabled)
            {
                mtc.enabled = true;
            }
            if (FPS)
            {
                target = Manager.Instace.FirstPerson;
            }
            else
            {
                if (target == null)
                {
                    if (!string.IsNullOrEmpty(targetPath))
                    {
                        target = GameObject.Find("Parent/" + targetPath);
                    }
                }
            }
            if (target != null)
            {
                mtc.SetTargetGameObject(target);
                mtc.SetGravity(noGravity);
                mtc.SetSpeed(speed);
            }

        }
        else
        {
            
            if (mtc!=null)
            {
                mtc.enabled = false;
            }
        }
    }
}
