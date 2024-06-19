using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShowWorldVideoInforma : ActionInforma
{
    /// <summary>
    /// 视频地址
    /// </summary>
    public string videoPath;
    /// <summary>
    /// 缩放模式
    /// </summary>
    public int ajustMode;
    /// <summary>
    /// 自动播放
    /// </summary>
    public bool autoPlay;
    /// <summary>
    /// 循环播放
    /// </summary>
    public bool loop;

    public ShowWorldVideoInforma(bool isOnce)
    {
        this.isOnce = isOnce;
    }
}
