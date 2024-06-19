using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class MoveTargetInforma : ActionInforma
{
    /// <summary>
    /// 开关是否打开
    /// </summary>
    public bool ONOFF;
    /// <summary>
    /// 是否以第一人称为目标
    /// </summary>
    public bool FPS;
    /// <summary>
    /// 速度
    /// </summary>
    public float speed;
    /// <summary>
    /// 无视重力
    /// </summary>
    public bool noGravity;
    /// <summary>
    /// 目标物体的名字
    /// </summary>
    public string targetName;
    /// <summary>
    /// 目标物体的路径
    /// </summary>
    public string rootName;
    public MoveTargetInforma(bool isOnce)
    {
        this.isOnce = isOnce;
    }
}
