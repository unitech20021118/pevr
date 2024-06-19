using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Introduce : MonoBehaviour {
    /// <summary>
    /// 介绍文字
    /// </summary>
    public string introduce;
    /// <summary>
    /// 图片地址
    /// </summary>
    public string imagePath;
    /// <summary>
    /// 视频地址
    /// </summary>
    public string videoPath;
    /// <summary>
    /// 设置介绍
    /// </summary>
    public void SetIntroduce(string str)
    {
        introduce = str;
    }

    public void SetImagePath(string str)
    {
        imagePath = str;
    }
    public void SetVideoPath(string str)
    {
        videoPath = str;
    }
}
