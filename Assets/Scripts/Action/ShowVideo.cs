using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShowVideo : Action<Main> {
    public RectTransform canvas;
    //public MovieTexture movieTexure;


    //public float rotate = 0;

    //public float x = 0;
    //public float y = 0;
    //public float z = 0;


    /// <summary>
    /// 宽度 默认260
    /// </summary>
    public float w = 260;
    /// <summary>
    /// 高度 默认180
    /// </summary>
    public float h = 180;
    /// <summary>
    /// 视频地址
    /// </summary>
    public string videoPath;
    /// <summary>
    /// 拉伸模式
    /// </summary>
    public int ajustMode;
    //public bool clickOpen;
    /// <summary>
    /// 视频在界面上播放的位置
    /// </summary>
    public Vector3 showPos;

    //无用   kuai
    //public float duringTime;

    /// <summary>
    /// 自动播放默认为true
    /// </summary>
    public bool autoPlay = true;
    /// <summary>
    /// 循环播放默认为false
    /// </summary>
    public bool loop = false;
    /// <summary>
    /// UI界面控制类
    /// </summary>
    UICtrller uiCtrller;
    //public RawImage rawImage;
    public override void DoAction(Main m)
    {
        if (canvas != null)
        {
            //Transform img = canvas.GetComponentInChildren<Mask> ().transform;
            //img.GetComponent<Image> ().sprite = sprite;
            //if (sprite == null) {
            //	sprite = ResLoader.resLoader.currentSprite;
            //	img.GetComponent<Image> ().sprite = sprite;
            //}
            //canvas.position = m.gameObject.transform.position+new Vector3 (x, y, z);
            //canvas.eulerAngles = new Vector3 (0, rotate, 0);
            //canvas.sizeDelta = new Vector2 (w, h);
            canvas.parent.gameObject.SetActive(true);
            //canvas.gameObject.SetActive(true);
            uiCtrller = m.gameObject.GetComponent<UICtrller>();
            
            if (uiCtrller == null)
            {
                uiCtrller = m.gameObject.AddComponent<UICtrller>();
            }
            
            canvas.sizeDelta = new Vector2(w, h);
            //uiCtrller.videoTargetUI = canvas.gameObject;
            uiCtrller.videoPlayer = canvas.gameObject;
            uiCtrller.videoPath = videoPath;
            uiCtrller.ajustMode = ajustMode;
            uiCtrller.autoPlay = autoPlay;
            uiCtrller.loop = loop;
            //uiCtrller.clickOpen = clickOpen;
            uiCtrller.isVideoMouseDown = true;
            uiCtrller.videoPos = showPos;
            uiCtrller.SetVideo();
        }
    }
    
    
}
