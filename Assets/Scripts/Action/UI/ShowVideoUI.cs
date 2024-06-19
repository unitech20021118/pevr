using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ShowVideoUI : ActionUI
{
    /// <summary>
    /// 用于显示视频地址的文本框
    /// </summary>
    public Text videoPathText;

    //public InputField rotate, x, y, z;

    /// <summary>
    /// 视频的宽度
    /// </summary>
    public InputField w;
    /// <summary>
    /// 视频个的高度
    /// </summary>
    public InputField h;

    //public Toggle clickOpen;

    /// <summary>
    /// 视频地址
    /// </summary>
    public string videoPath;
    /// <summary>
    /// 视频名字
    /// </summary>
    public string videoName;

    //public Sprite targetSprite;
    /// <summary>
    /// 播放视频的预设物体
    /// </summary>
    public Transform preShow;
    /// <summary>
    /// 拉伸模式
    /// </summary>
    public int ajustMode;
    /// <summary>
    /// 管理播放视频位置的载体
    /// </summary>
    RectTransform videoRect;
    /// <summary>
    /// 是否自动播放选项框
    /// </summary>
    public Toggle autoPlay;
    /// <summary>
    /// 是否循环播放选项框
    /// </summary>
    public Toggle loop;
    /// <summary>
    /// 拉伸模式1
    /// </summary>
    public Toggle scaleMode1;
    /// <summary>
    /// 拉伸模式2
    /// </summary>
    public Toggle scaleMode2;
    /// <summary>
    /// 拉伸模式3
    /// </summary>
    public Toggle scaleMode3;
    ShowVideo _showVideo;
    ShowVideoInforma _sVideoInforma;
    void Awake()
    {
        preShow = Manager.Instace.transform.Find("VideoPlayer");
    }

    void OnEnable()
    {
        if (preShow)
        {
            if (!videoRect)
            {
                videoRect = Instantiate<GameObject>(preShow.GetChild(0).gameObject, preShow).GetComponent<RectTransform>();
            }
            PreShowVisibility(false);
            SetPreShowSize();
        }
    }
    /// <summary>
    /// 设置可见性
    /// </summary>
    /// <param name="isShow"></param>
    public void PreShowVisibility(bool isShow)
    {
        videoRect.gameObject.SetActive(isShow);
    }
    /// <summary>
    /// 设置宽高
    /// </summary>
    public void SetPreShowSize()
    {
        try
        {
            RectTransform rt = videoRect;
            rt.sizeDelta = new Vector2(float.Parse(w.text), float.Parse(h.text));
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }
    }
    public override Action<Main> CreateAction()
    {
        action = new ShowVideo();
        action.isOnce = true;
        actionInforma = new ShowVideoInforma(true);
        _showVideo = (ShowVideo)action;
        _sVideoInforma = (ShowVideoInforma) actionInforma;
        if (!videoRect)
        {
            videoRect = Instantiate<GameObject>(preShow.GetChild(0).gameObject, preShow).GetComponent<RectTransform>();
        }
        _showVideo.canvas = videoRect;
        GetStateInfo().actionList.Add(actionInforma);
        actionInforma.name = "ShowVideo";
        return base.CreateAction();
    }

    public override Action<Main> LoadAction(ActionInforma actionInforma)
    {
        _sVideoInforma = (ShowVideoInforma)actionInforma;
        this.actionInforma = actionInforma;

        action = new ShowVideo();
        _showVideo = (ShowVideo)action;
        //rotate.text = sVideoInforma.rotate.ToString();
        //x.text = sVideoInforma.x.ToString();
        //y.text = sVideoInforma.y.ToString();
        //z.text = sVideoInforma.z.ToString();

        w.text = _sVideoInforma.w.ToString();
        h.text = _sVideoInforma.h.ToString();

        
        //timeInputField.text = showVideo.duringTime.ToString();
        if (!videoRect)
        {
            videoRect = Instantiate<GameObject>(preShow.GetChild(0).gameObject, preShow).GetComponent<RectTransform>();
        }
        _showVideo.canvas = videoRect;
        //clickOpen.isOn = sVideoInforma.clickOpen;
        if (string.IsNullOrEmpty(_sVideoInforma.videopath))
        {
            _showVideo.videoPath = "";
        }
        else
        {
            //showVideo.videoPath = ResLoader.targetPath + @"\videos\" + sVideoInforma.videopath;
            _showVideo.videoPath = Application.streamingAssetsPath + "/videos/" + _sVideoInforma.videopath;
        }
        _showVideo.ajustMode = _sVideoInforma.ajustMode;
        _showVideo.showPos = new Vector3(_sVideoInforma.px, _sVideoInforma.py, _sVideoInforma.pz);
        videoRect.localPosition = _showVideo.showPos;
        //if (!string.IsNullOrEmpty(sVideoInforma.videopath))
        //{
        //    ResLoader.resLoader.StartCoroutine(ResLoader.resLoader.GetVideoRes(ResLoader.targetPath + @"\videos\" + sVideoInforma.videopath,videoPathText));
        //}
        videoPath = _showVideo.videoPath;
        //Debug.Log("6666666  "+videoPath);
        videoPathText.text = _showVideo.videoPath;
        //showVideo.rotate = sVideoInforma.rotate;
        //showVideo.x = sVideoInforma.x;
        //showVideo.y = sVideoInforma.y;
        //showVideo.z = sVideoInforma.z;
        _showVideo.w = _sVideoInforma.w;
        _showVideo.h = _sVideoInforma.h;
        //showVideo.clickOpen = sVideoInforma.clickOpen;
        autoPlay.isOn = _sVideoInforma.AutoPlay;
        loop.isOn = _sVideoInforma.Loop;
        switch (_showVideo.ajustMode)
        {
            case 1:
                scaleMode1.isOn = true;
                scaleMode2.isOn = false;
                scaleMode3.isOn = false;
                break;
            case 2:
                scaleMode2.isOn = true;
                scaleMode1.isOn = false;
                scaleMode3.isOn = false;
                break;
            case 3:
                scaleMode3.isOn = true;
                scaleMode1.isOn = false;
                scaleMode2.isOn = false;
                break;
            default:
                break;
        }
        return base.LoadAction(actionInforma);
    }

    /// <summary>
    /// 更新输入内容
    /// </summary>
    public void UpdateInput()
    {
        //ShowVideo showVideo = (ShowVideo)action;
        //if (_showVideo == null)
        //{
        //    _showVideo = (ShowVideo)action;
        //}
        try
        {
            //showVideo.rotate = float.Parse(rotate.text);
            //showVideo.x = float.Parse(x.text);
            //showVideo.y = float.Parse(y.text);
            //showVideo.z = float.Parse(z.text);

            _showVideo.w = float.Parse(w.text);
            _showVideo.h = float.Parse(h.text);
            //showVideo.clickOpen = clickOpen.isOn;
            ////showVideo.rawImage = targetSprite;
            _showVideo.showPos = videoRect.localPosition;
            //GetInforma();

            //将属性值保存
            //sVideoInforma.rotate = float.Parse(rotate.text);
            //sVideoInforma.x = float.Parse(x.text);
            //sVideoInforma.y = float.Parse(y.text);
            //sVideoInforma.z = float.Parse(z.text);

            _sVideoInforma.w = float.Parse(w.text);
            _sVideoInforma.h = float.Parse(h.text);
            //sVideoInforma.clickOpen = clickOpen.isOn;
            _sVideoInforma.videopath = videoName;
            _sVideoInforma.px = videoRect.localPosition.x;
            _sVideoInforma.py = videoRect.localPosition.y;
            _sVideoInforma.pz = videoRect.localPosition.z;
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }
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
            //_showVideo = (ShowVideo)action;
            _showVideo.videoPath = videoPath;
            videoPathText.text = videoPath;
            //sVideoInforma = (ShowVideoInforma)actionInforma;
            //GetInforma();
            _sVideoInforma.videopath = videoName;
            //Debug.LogError(imgpath);
            //StartCoroutine(GetVideo(videoPath));
            //UpdateInput();
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }
    }
    /// <summary>
    /// 将视频复制到软件目录下
    /// </summary>
    void CopyVideo()
    {
        if (Directory.Exists(Application.streamingAssetsPath + "/videos"))
        {
            File.Copy(videoPath, Application.streamingAssetsPath + "/videos/" + videoName, true);
        }
        else
        {
            Directory.CreateDirectory(Application.streamingAssetsPath + "/videos");
            File.Copy(videoPath, Application.streamingAssetsPath + "/videos/" + videoName, true);
        }
    }

    //IEnumerator GetVideo(string path)
    //{
    //    WWW www = new WWW(@"file://" + path);
    //    while (www.isDone==false)
    //    {
    //        yield return null;
    //    }

    //    //showVideo.movieTexure = www.movie;
    //    //videoRect.GetComponent<RawImage>().texture = showVideo.movieTexure;

    //    videoPathText.text = path;
    //    //SetVideoAjust();
    //}


    /// <summary>
    /// 缩放模式1
    /// </summary>
    /// <param name="a"></param>
    public void SetScaleModel1(Toggle toggle)
    {

        if (toggle.isOn)
        {
            //GetInforma();
            _sVideoInforma.ajustMode = 1;
            _showVideo.ajustMode = 1;
        }
    }
    /// <summary>
    /// 缩放模式2
    /// </summary>
    /// <param name="toggle"></param>
    public void SetScaleModel2(Toggle toggle)
    {
        if (toggle.isOn)
        {
            //GetInforma();
            _sVideoInforma.ajustMode = 2;
            _showVideo.ajustMode = 2;
        }
    }
    /// <summary>
    /// 缩放模式3
    /// </summary>
    /// <param name="toggle"></param>
    public void SetScaleModel3(Toggle toggle)
    {
        if (toggle.isOn)
        {
            //GetInforma();
            _sVideoInforma.ajustMode = 3;
            _showVideo.ajustMode = 3;
        }
    }
    /// <summary>
    /// 设置是否自动播放
    /// </summary>
    /// <param name="toggle"></param>
    public void SetAutoPlay(Toggle toggle)
    {
        //GetInforma();
        _sVideoInforma.AutoPlay = toggle.isOn;
        _showVideo.autoPlay = toggle.isOn;
    }
    /// <summary>
    /// 设置是否循环播放
    /// </summary>
    /// <param name="toggle"></param>
    public void SetLoop(Toggle toggle)
    {
        //GetInforma();
        _sVideoInforma.Loop = toggle.isOn;
        _showVideo.loop = toggle.isOn;
    }
    /// <summary>
    /// 获取储存数据的脚本
    /// </summary>
    public void GetInforma()
    {
        if (_sVideoInforma == null)
        {
            _sVideoInforma = (ShowVideoInforma)actionInforma;
        }
    }

}
