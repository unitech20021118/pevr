using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 设置高光描边动作的数据类
/// created by kuai
/// </summary>
[System.Serializable]
public class SetHighLightInforma : ActionInforma
{
    /// <summary>
    /// 开关是否打开
    /// </summary>
    public bool swich;
    /// <summary>
    /// 持续时间
    /// </summary>
    public float duration;
    /// <summary>
    /// 是否闪烁
    /// </summary>
    public bool isTwinkle;
    /// <summary>
    /// 目标物体的名字
    /// </summary>
    public string targetName;
    /// <summary>
    /// 目标物体的路径
    /// </summary>
    public string rootName;
    /// <summary>
    /// 高光颜色
    /// </summary>
    public string color;
    public SetHighLightInforma(bool isOnce)
    {
        this.isOnce = isOnce;
    }
}
