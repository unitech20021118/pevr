using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ShowPowerPointInforma : ActionInforma
{
    /// <summary>
    /// 文件夹名（也是ppt名）
    /// </summary>
    public string FolderName;
    /// <summary>
    /// 是不是自动播放
    /// </summary>
    public bool AutoPlay;
    /// <summary>
    /// 是否要循环播放
    /// </summary>
    public bool Loop;
    /// <summary>
    /// 自动播放的间隔时间
    /// </summary>
    public float Interval;

    public ShowPowerPointInforma(bool isOnce)
    {
        this.isOnce = isOnce;
    }
}
