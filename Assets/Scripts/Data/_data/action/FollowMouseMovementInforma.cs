using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FollowMouseMovementInforma : ActionInforma
{
    public float X;
    public float Y;
    public float Z;
    public string XMin;
    public string XMax;
    public string YMin;
    public string YMax;
    public string ZMin;
    public string ZMax;
    public bool Place;
    /// <summary>
    /// 是否是面模式
    /// </summary>
    public bool isFace;

    public bool faceX;
    public bool faceY;
    public bool faceZ;

    public FollowMouseMovementInforma(bool isOnce)
    {
        this.isOnce = isOnce;
    }
}
