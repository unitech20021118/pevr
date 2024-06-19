using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


public class ShowWorldVideoUI : ActionUI
{
    /// <summary>
    /// 显示视频地址的文本框
    /// </summary>
    public Text VideoPathText;
    /// <summary>
    /// 视频地址
    /// </summary>
    private string videoPath;
    /// <summary>
    /// 视频名
    /// </summary>
    private string videoName;
    /// <summary>
    /// 播放视频的预设物
    /// </summary>
    private GameObject worldVideoPrefab;
    /// <summary>
    /// 拉伸模式
    /// </summary>
    private int ajustMode;
    /// <summary>
    /// 是否自动播放的选项
    /// </summary>
    public Toggle AutoPlayToggle;
    /// <summary>
    /// 是否循环播放的选项
    /// </summary>
    public Toggle LoopToggle;

    /// <summary>
    /// 拉伸模式1
    /// </summary>
    public Toggle ScaleMode1Toggle;
    /// <summary>
    /// 拉伸模式2
    /// </summary>
    public Toggle ScaleMode2Toggle;
    /// <summary>
    /// 拉伸模式3
    /// </summary>
    public Toggle ScaleMode3Toggle;

    private ShowWorldVideo _showWorldVideo;
    private ShowWorldVideoInforma _showWorldVideoInforma;

    void Awake()
    {
        //worldVideoPrefab = Resources.Load<GameObject>("WorldVideo");
    }

    public override Action<Main> CreateAction()
    {

        action = new ShowWorldVideo();
        actionInforma = new ShowWorldVideoInforma(true);
        _showWorldVideo = (ShowWorldVideo)action;
        _showWorldVideoInforma = (ShowWorldVideoInforma)actionInforma;
        //初始化相关变量
        _showWorldVideoInforma.ajustMode = 1;
        _showWorldVideo.ajustMode = 1;
        _showWorldVideoInforma.autoPlay = true;
        _showWorldVideo.autoPlay = true;
        _showWorldVideoInforma.loop = false;
        _showWorldVideo.loop = false;

        GetStateInfo().actionList.Add(actionInforma);
        actionInforma.name = "ShowWorldVideo";
        return base.CreateAction();
    }



    public override Action<Main> LoadAction(ActionInforma actionInforma)
    {
        _showWorldVideoInforma = (ShowWorldVideoInforma)actionInforma;
        this.actionInforma = actionInforma;
        action = new ShowWorldVideo();
        _showWorldVideo = (ShowWorldVideo)action;
        //读取数据

        if (string.IsNullOrEmpty(_showWorldVideoInforma.videoPath))
        {
            _showWorldVideo.videoPath = "";
        }
        else
        {
            _showWorldVideo.videoPath = Application.streamingAssetsPath + "/videos/" + _showWorldVideoInforma.videoPath;
            VideoPathText.text = _showWorldVideo.videoPath;
        }

        _showWorldVideo.ajustMode = _showWorldVideoInforma.ajustMode;
        _showWorldVideo.autoPlay = _showWorldVideoInforma.autoPlay;
        _showWorldVideo.loop = _showWorldVideoInforma.loop;
        ajustMode = _showWorldVideoInforma.ajustMode;
        AutoPlayToggle.isOn = _showWorldVideoInforma.autoPlay;
        LoopToggle.isOn = _showWorldVideoInforma.loop;
        switch (_showWorldVideoInforma.ajustMode)
        {
            case 1:
                ScaleMode1Toggle.isOn = true;
                ScaleMode2Toggle.isOn = false;
                ScaleMode3Toggle.isOn = false;
                break;
            case 2:
                ScaleMode1Toggle.isOn = false;
                ScaleMode2Toggle.isOn = true;
                ScaleMode3Toggle.isOn = false;
                break;
            case 3:
                ScaleMode1Toggle.isOn = false;
                ScaleMode2Toggle.isOn = false;
                ScaleMode3Toggle.isOn = true;
                break;
            default:
                break;
        }


        return base.LoadAction(actionInforma);
    }


    /// <summary>
    /// 获取视频地址
    /// </summary>
    public void GetVideo()
    {
        try
        {
            FileInfo fileInfo = new FileInfo(IOHelper.GetVideoName());
            videoPath = fileInfo.FullName;
            videoName = fileInfo.Name;
            CopyVideo();
            _showWorldVideo.videoPath = videoPath;
            VideoPathText.text = videoPath;

            _showWorldVideoInforma.videoPath = videoName;
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
    /// <summary>
    /// 将视频复制到软件目录下
    /// </summary>
    private void CopyVideo()
    {
        if (Directory.Exists(Application.streamingAssetsPath+"/videos"))
        {
            File.Copy(videoPath,Application.streamingAssetsPath + "/videos/"+videoName,true);
        }
        else
        {
            Directory.CreateDirectory(Application.streamingAssetsPath + "/videos");
            File.Copy(videoPath,Application.dataPath + "/videos"+videoName,true);
        }
    }

    /// <summary>
    /// 设置缩放模式
    /// </summary>
    public void SetScaleMode()
    {
        if (ScaleMode1Toggle.isOn)
        {
            _showWorldVideoInforma.ajustMode = 1;
            _showWorldVideo.ajustMode = 1;
        }
        else if(ScaleMode2Toggle.isOn)
        {
            _showWorldVideoInforma.ajustMode = 2;
            _showWorldVideo.ajustMode = 2;
        }
        else if(ScaleMode3Toggle.isOn)
        {
            _showWorldVideoInforma.ajustMode = 3;
            _showWorldVideo.ajustMode = 3;
        }
    }
    /// <summary>
    /// 设置是否自动播放
    /// </summary>
    public void SetAutoPlay()
    {
        _showWorldVideoInforma.autoPlay = AutoPlayToggle.isOn;
        _showWorldVideo.autoPlay = AutoPlayToggle.isOn;
    }
    /// <summary>
    /// 设置是否循环播放
    /// </summary>
    public void SetLoop()
    {
        _showWorldVideoInforma.loop = LoopToggle.isOn;
        _showWorldVideo.loop = LoopToggle.isOn;
    }
}
