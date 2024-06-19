using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransColorInforma :  ActionInforma
{
    /// <summary>
    /// 目标名字
    /// </summary>
    public string targetName;
   
    /// <summary>
    /// 颜色
    /// </summary>
    public string color;
    public TransColorInforma(bool isOnce)
    {
        this.isOnce = isOnce;
    }

}
