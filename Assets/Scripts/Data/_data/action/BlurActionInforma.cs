using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BlurActionInforma : ActionInforma
{
    /// <summary>
    /// 蜡烛名
    /// </summary>
    public string CandleName;
    /// <summary>
    /// 凸透镜名
    /// </summary>
    public string ConvexLensName;
    /// <summary>
    /// 光屏名
    /// </summary>
    public string OpticalScreenName;
    /// <summary>
    /// 凸透镜的焦距
    /// </summary>
    public float FocalLength;
    /// <summary>
    /// 模糊变化程度
    /// </summary>
    public float BlurLevel;
    /// <summary>
    /// 大小比例
    /// </summary>
    public float Proportion;


    public BlurActionInforma(bool isOnce)
    {
        this.isOnce = isOnce;
    }
}
