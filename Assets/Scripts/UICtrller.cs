using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RenderHeads.Media.AVProVideo;

public class UICtrller : MonoBehaviour
{
    public GameObject imgtargetUI, videoTargetUI;
    public GameObject videoPlayer;
    public Sprite sprite;
    public string msg;
    public string imgPath, msgBgPath, videoPath;
    //edit by kuai----------------------
    //public bool isMouseDown = true;
    public bool isImageMouseDown = true;
    public bool isMessageMouseDown = true;
    public bool isVideoMouseDown = true;
    //-----------------------------------
    public int ajustMode;
    public FontStyle style;
    public int fontSize;
    public float spacing;
    public Font font;
    public bool clickOpen;
    public Vector3 imgPos, msgPos, videoPos;
    public bool autoPlay;
    public bool loop;
    public bool IsBG;
    public List<MessageAttribute> MessageAttributes = new List<MessageAttribute>();
    /// <summary>
    /// 背景图
    /// </summary>
    private GameObject imageBG;
    public MediaPlayer mediaPlayer;
    public DisplayUGUI displayUGUI;
    //public MovieTexture movieTex;
    void Awake()
    {
        imageBG = GameObject.Find("Canvas").transform.FindChild("ImageBG").gameObject;
    }

    // Use this for initialization
    void Start()
    {
        if (imgtargetUI != null)
            imgtargetUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //if (isMessageMouseDown)
        //{
        //    if (targetUI != null)
        //    {
        //        targetUI.SetActive(!targetUI.activeSelf);
        //    }
        //    isMessageMouseDown = false;
        //}
        if (isImageMouseDown)
        {
            if (imgtargetUI != null)
            {
                //imgtargetUI.SetActive(!imgtargetUI.activeSelf);
                imgtargetUI.SetActive(!imgtargetUI.activeSelf);
            }
            isImageMouseDown = false;
        }
        if (isVideoMouseDown)
        {
            if (videoTargetUI != null)
            { videoTargetUI.SetActive(true); }
            if (videoPlayer != null)
            {
                videoPlayer.SetActive(true);
            }
            isVideoMouseDown = false;
        }


        ////if (!clickOpen)
        ////{
        ////    //targetUI.SetActive (true);
        ////}
        SetImg();
        //SetMsg();
    }

    public void OpenUIPanel(int type)
    {
        switch (type)
        {
            case 0:
                //Debug.LogError("555");
                //if (targetUI != null)
                //{
                //    targetUI.SetActive(true);
                //    Debug.LogError("666");
                //}
                //SetMsg();
                break;
            case 1:
                if (imgtargetUI != null)
                {
                    //imgtargetUI.SetActive(!imgtargetUI.activeSelf);
                    imgtargetUI.SetActive(true);
                }
                SetImg();
                break;
                default:
                    break;
        }
    }



    public void OnMouseDown()
    {
        //isMouseDown = clickOpen;
        Debug.Log("不知道这里要做什么");
    }

    public void SetImg()
    {
        try
        {
            if (imgtargetUI.transform.parent.name.Equals("ImgContainer"))
            {
                if (!string.IsNullOrEmpty(imgPath))
                {
                    imgtargetUI.transform.localPosition = imgPos;
                    StartCoroutine(GetImgRes(imgPath, imgtargetUI.GetComponent<Image>()));
                }
            }
        }
        catch
        {
        }
    }
    //public void SetVideo()
    //{
    //    try
    //    {
    //        if (videoTargetUI.transform.parent.name.Equals("VideoContainer"))
    //        {
    //            if (!string.IsNullOrEmpty(videoPath))
    //            {
    //                videoTargetUI.transform.localPosition = videoPos;
    //                StartCoroutine(GetVideoRes(videoPath, videoTargetUI.GetComponent<RawImage>()));
    //            }
    //        }
    //    }
    //    catch
    //    {
    //    }
    //}
    public void SetVideo()
    {
        try
        {
            if (videoPlayer.transform.parent.name.Equals("VideoPlayer"))
            {
                if (!string.IsNullOrEmpty(videoPath))
                {
                    videoPlayer.transform.localPosition = videoPos;
                    SetVideoAjust();
                    StartCoroutine(GetVideoRes(videoPath));
                }
            }
        }
        catch
        {
        }
    }
    //public void SetMsg()
    //{
    //    try
    //    {
    //        targetUI.transform.localPosition = msgPos;
    //        targetUI.transform.GetChild(0).GetComponent<Text>().text = msg;
    //        targetUI.transform.GetChild(0).GetComponent<Text>().font = font;
    //        targetUI.transform.GetChild(0).GetComponent<Text>().fontSize = fontSize;
    //        targetUI.transform.GetChild(0).GetComponent<Text>().fontStyle = style;
    //        targetUI.transform.GetChild(0).GetComponent<TextSpacing>()._textSpacing = spacing;
    //        ajustMode = 0;
    //        if (!string.IsNullOrEmpty(msgBgPath))
    //        {
    //            targetUI.transform.localPosition = msgPos;
    //            StartCoroutine(GetImgRes(msgBgPath, targetUI.GetComponent<Image>()));
    //        }
    //    }
    //    catch
    //    {
    //    }
    //}

    void SetImgAjust()
    {
        Vector2 containerSize = imgtargetUI.transform.parent.GetComponent<RectTransform>().sizeDelta;
        float imgSizeW = imgtargetUI.GetComponent<Image>().preferredWidth;
        float imgSizeH = imgtargetUI.GetComponent<Image>().preferredHeight;
        switch (ajustMode)
        {
            case 1:
                imgtargetUI.GetComponent<RectTransform>().sizeDelta = new Vector2(containerSize.x, (containerSize.x * imgSizeH / imgSizeW));
                break;
            case 2:
                imgtargetUI.GetComponent<RectTransform>().sizeDelta = new Vector2((containerSize.y * imgSizeW / imgSizeH), containerSize.y);
                break;
            default:
                break;
        }
    }

    IEnumerator GetImgRes(string path, Image targetImg)
    {
        WWW www = new WWW(@"file://" + path);
        yield return www;
        Texture2D tex2d = www.texture;
        //		if(targetUI!=null)
        //			targetUI.GetComponent<Image>().sprite = Sprite.Create (tex2d, new Rect (0, 0, tex2d.width, tex2d.height), Vector3.zero);
        //		if(imgtargetUI!=null)
        //			imgtargetUI.GetComponent<Image>().sprite = Sprite.Create (tex2d, new Rect (0, 0, tex2d.width, tex2d.height), Vector3.zero);
        targetImg.sprite = Sprite.Create(tex2d, new Rect(0, 0, tex2d.width, tex2d.height), Vector2.zero, 100, 1, SpriteMeshType.Tight);
        if (IsBG == true)
        {
            imageBG.GetComponent<Image>().sprite = targetImg.sprite;
            targetImg.gameObject.SetActive(false);
            imageBG.gameObject.SetActive(true);
        }
        //Debug.LogError("111");
        //SetImgAjust ();
        //targetUI.GetComponent<Image> ().SetNativeSize ();
    }
    //IEnumerator GetVideoRes(string path, RawImage targetVideo)
    //{
    //    WWW www = new WWW(@"file://" + path);
    //    while (www.isDone ==false)
    //    {
    //        yield return null;
    //    }
    //    movieTex = www.movie;
    //    targetVideo.texture = movieTex;
    //    movieTex.Play();
    //}


    /// <summary>
    /// 根据视频路径获取并播放视频
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    IEnumerator GetVideoRes(string path)
    {
        //displayUGUI.gameObject.SetActive(true);
        //mediaPlayer.gameObject.SetActive(true);
        yield return null;

        GetMediaAndDisplay();

        mediaPlayer.OpenVideoFromFile(MediaPlayer.FileLocation.RelativeToDataFolder, path, autoPlay);
        //mediaPlayer.m_VideoPath = path;
        //mediaPlayer.m_AutoOpen = true;
        mediaPlayer.Control.SetLooping(loop);

        //mediaPlayer.m_AutoStart = autoPlay;





    }
    /// <summary>
    /// 获取播放视频的两个重要组件
    /// </summary>
    public void GetMediaAndDisplay()
    {
        if (mediaPlayer == null)
        {
            mediaPlayer = videoPlayer.transform.GetChild(1).GetComponent<MediaPlayer>();
        }
        if (displayUGUI == null)
        {
            displayUGUI = videoPlayer.transform.GetChild(0).GetComponent<DisplayUGUI>();
        }

    }
    /// <summary>
    /// 设置视频拉伸模式
    /// </summary>
    public void SetVideoAjust()
    {
        GetMediaAndDisplay();
        switch (ajustMode)
        {
            case 1:
                displayUGUI._scaleMode = ScaleMode.ScaleToFit;
                break;
            case 2:
                displayUGUI._scaleMode = ScaleMode.StretchToFill;
                break;
            case 3:
                displayUGUI._scaleMode = ScaleMode.ScaleAndCrop;
                break;
            default:
                break;
        }
    }

    public void SetMessageBGImage(string imagepath,Image image)
    {
        StartCoroutine(GetImgRes(imagepath, image));
    }

}
