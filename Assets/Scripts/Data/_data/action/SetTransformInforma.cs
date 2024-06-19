using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SetTransformInforma : ActionInforma
{
    public string xp;
    public string yp;
    public string zp;
    public string xr;
    public string yr;
    public string zr;
    public string xs;
    public string ys;
    public string zs;
    /// <summary>
    /// 目标物体的名字
    /// </summary>
    public string targetName;
    /// <summary>
    /// 目标物体的路径
    /// </summary>
    public string rootName;

    public SetTransformInforma(bool isOnce)
    {
        this.isOnce = isOnce;
    }
}
