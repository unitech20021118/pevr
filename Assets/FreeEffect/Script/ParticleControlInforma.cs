using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ParticleControlInforma: ActionInforma
{
    /// <summary>
    /// 目标物体的名字
    /// </summary>
    public string targetName;
    /// <summary>
    /// 目标物体的路径
    /// </summary>
    public string rootName;
    /// <summary>
    /// 选择的操作
    /// </summary>
    public int _Etype;
    //public float d;

    public ParticleControlInforma(bool isOnce)
    {
        this.isOnce = isOnce;
    }
}
