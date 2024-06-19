using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DeliveryInforma : ActionInforma
{
    /// <summary>
    /// 传送的目标物体
    /// </summary>
    public string targetName;
    /// <summary>
    /// 传送坐标X
    /// </summary>
    public string X;
    /// <summary>
    /// 传送坐标Y
    /// </summary>
    public string Y;
    /// <summary>
    /// 传送坐标Z
    /// </summary>
    public string Z;

    public DeliveryInforma(bool isOnce)
    {
        this.isOnce = isOnce;
    }
}
