using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowWorldVideo : Action<Main>
{
    /// <summary>
    /// 视频地址
    /// 因为将要播放的视频复制到软件目录下，所以这里记录的只是视频名（带后缀名）
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
    /// 循环
    /// </summary>
    public bool loop;

    private WorldVideoController worldVideoController;
    public override void DoAction(Main m)
    {
        //将预设物做成模型使用
        worldVideoController = m.gameObject.GetComponent<WorldVideoController>();
        if (worldVideoController == null)
        {
            worldVideoController = m.gameObject.AddComponent<WorldVideoController>();
        }

        worldVideoController.SetValue(videoPath, autoPlay, loop, ajustMode);
    }
}
