using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class PCShowMsgUI : ActionUI
{
    /// <summary>
    /// 文本输入框
    /// </summary>
    public InputField msgField;
    /// <summary>
    /// 显示的文本所在的父物体
    /// </summary>
    public Transform preShow;
    /// <summary>
    /// 粗体，斜体
    /// </summary>
    public Toggle bold, italic;
    /// <summary>
    /// 字体大小输入框
    /// </summary>
    public InputField fontSize;
    /// <summary>
    /// 字间距输入框（未启用）
    /// </summary>
    public InputField spacing;
    /// <summary>
    /// 文本框的宽高输入框
    /// </summary>
    public InputField w, h;
    public Toggle clickOpen;
    /// <summary>
    /// 选择字体的下拉列表
    /// </summary>
    public Dropdown fontId;
    /// <summary>
    /// 字体加粗斜体等的模式
    /// </summary>
    public FontStyle style;
    /// <summary>
    /// 字体的数组
    /// </summary>
    public Font[] fonts;
    /// <summary>
    /// 文本底图的路径和名字
    /// </summary>
    public string imgpath, imgName;
    /// <summary>
    /// 底图
    /// </summary>
    public Sprite targetSprite;
    RectTransform msgRect;
    /// <summary>
    /// 是否允许被覆盖
    /// </summary>
    public Toggle IsCover;
    /// <summary>
    /// 是否允许被拖动
    /// </summary>
    public Toggle CanDragToggle; 
    /// <summary>
    /// 用于显示已选颜色的图片
    /// </summary>
    public Image ColorImage;
    /// <summary>
    /// 显示出的文字
    /// </summary>
    private Text text;
    /// <summary>
    /// 隐藏按钮
    /// </summary>
    public Toggle HideButtonToggle;

    public InputField LineDisdence; //折线长度 by pxf
    public Toggle IsShowLine;       //是否显示连线 by pxf

    private PCShowMsg _pcShowMsg;
    private PCShowMsgInforma _pcShowMsgInforma;
    public override Action<Main> CreateAction()
    {
        action = new PCShowMsg();
        //action.isOnce = true;
        actionInforma = new PCShowMsgInforma(true);
        _pcShowMsg = (PCShowMsg)action;
        _pcShowMsgInforma = (PCShowMsgInforma) actionInforma;

        if (!msgRect)
        {
            msgRect = Instantiate<GameObject>(preShow.GetChild(0).gameObject, preShow).GetComponent<RectTransform>();
        }
        //初始化相关变量
        _pcShowMsg.color=new Color32(255,255,255,255);
        _pcShowMsgInforma.color = _pcShowMsg.color.ToString();
        ColorImage.color = _pcShowMsg.color;

        _pcShowMsgInforma.msg = "";
        _pcShowMsgInforma.fontStyle = 0;
        _pcShowMsgInforma.fontSize = 14;
        _pcShowMsgInforma.font = 0;
        _pcShowMsgInforma.spacing = 1;
        _pcShowMsgInforma.clickOpen = false;
        _pcShowMsgInforma.imagePath = "";
        _pcShowMsgInforma.px = msgRect.localPosition.x;
        _pcShowMsgInforma.py = msgRect.localPosition.y;
        _pcShowMsgInforma.pz = msgRect.localPosition.z;
        _pcShowMsgInforma.w = 260f;
        _pcShowMsgInforma.h = 180f;
        _pcShowMsgInforma.IsCover = true;
        _pcShowMsgInforma.CanDrag = true;
        _pcShowMsgInforma.hideButton = false;
        _pcShowMsgInforma.IsShowLine = false;
        _pcShowMsgInforma.LineDistance = 0.8f;

        _pcShowMsg.msg = "";
        _pcShowMsg.style = FontStyle.Normal;
        _pcShowMsg.fontSize = 14;
        _pcShowMsg.font = fonts[0];
        _pcShowMsg.spacing = 1;
        _pcShowMsg.clickOpen = false;
        _pcShowMsg.imagePath = "";
        _pcShowMsg.showPos = new Vector3(msgRect.localPosition.x, msgRect.localPosition.y, msgRect.localPosition.z);
        _pcShowMsg.w = 260f;
        _pcShowMsg.h = 180f;
        _pcShowMsg.IsCover = true;
        _pcShowMsg.CanDrag = true;
        _pcShowMsg.hideButton = false;
        _pcShowMsg.IsShowLine = false;
        _pcShowMsg.LineDistance = 0.8f;

        _pcShowMsg.tipObj = msgRect.gameObject;
        GetStateInfo().actionList.Add(actionInforma);
        actionInforma.name = "PCShowMsg";
        return base.CreateAction();
    }

    public override Action<Main> LoadAction(ActionInforma actionInforma)
    {
        _pcShowMsgInforma = (PCShowMsgInforma)actionInforma;
        this.actionInforma = actionInforma;
        action = new PCShowMsg();
        _pcShowMsg = (PCShowMsg) action;

        if (!msgRect)
        {
            msgRect = Instantiate<GameObject>(preShow.GetChild(0).gameObject, preShow).GetComponent<RectTransform>();
        }
        //读取数据

        if (_pcShowMsgInforma.color != null)
        {
            ColorImage.color = Manager.Instace.GetColor(_pcShowMsgInforma.color);
            _pcShowMsg.color = ColorImage.color;
        }
        msgField.text = _pcShowMsgInforma.msg;
        _pcShowMsg.msg = _pcShowMsgInforma.msg;

        fontId.value = _pcShowMsgInforma.font;
        _pcShowMsg.font = fonts[fontId.value];

        fontSize.text = _pcShowMsgInforma.fontSize.ToString();
        _pcShowMsg.fontSize = _pcShowMsgInforma.fontSize;
        //未启用
        //clickOpen.isOn = _pcShowMsgInforma.clickOpen;
        //_pcShowMsg.clickOpen = _pcShowMsgInforma.clickOpen;
        //spacing.text = _pcShowMsgInforma.spacing.ToString();
        //_pcShowMsg.spacing = _pcShowMsgInforma.spacing;

        style = (FontStyle)_pcShowMsgInforma.fontStyle;
        _pcShowMsg.style = style;
        SetStyle(_pcShowMsgInforma.fontStyle);

        w.text = _pcShowMsgInforma.w.ToString();
        h.text = _pcShowMsgInforma.h.ToString();

        LineDisdence.text = _pcShowMsgInforma.LineDistance.ToString(); //Edit by pxf
        IsShowLine.isOn = _pcShowMsgInforma.IsShowLine;                //Edit by pxf
        IsCover.isOn = _pcShowMsgInforma.IsCover;

        _pcShowMsg.IsCover = _pcShowMsgInforma.IsCover;

        CanDragToggle.isOn = _pcShowMsgInforma.CanDrag;
        _pcShowMsg.CanDrag = _pcShowMsgInforma.CanDrag;

        HideButtonToggle.isOn = _pcShowMsgInforma.hideButton;
        _pcShowMsg.hideButton = _pcShowMsgInforma.hideButton;
        //pcshowMsg.tipObj = Instantiate<GameObject> (Resources.Load<GameObject> ("Prefabs/TipContainer"), Manager.Instace.gonggong.transform);
        //pcshowMsg.tipObj=preShow.gameObject;
        
        _pcShowMsg.tipObj = msgRect.gameObject;
        print(_pcShowMsgInforma.imagePath);
        if (string.IsNullOrEmpty(_pcShowMsgInforma.imagePath))
        {
            _pcShowMsg.imagePath = "";
        }
        else
        {
            _pcShowMsg.imagePath = ResLoader.targetPath + @"\images\" + _pcShowMsgInforma.imagePath;
        }
        print(_pcShowMsg.imagePath);
        if (!string.IsNullOrEmpty(_pcShowMsgInforma.imagePath))
        {
            ResLoader.resLoader.StartCoroutine(ResLoader.resLoader.GetImgRes(ResLoader.targetPath + @"\images\" + _pcShowMsgInforma.imagePath, msgRect.GetComponent<Image>()));
        }
        _pcShowMsg.showPos = new Vector3(_pcShowMsgInforma.px, _pcShowMsgInforma.py, _pcShowMsgInforma.pz);
        msgRect.localPosition = _pcShowMsg.showPos;
        msgRect.sizeDelta = new Vector2(_pcShowMsgInforma.w, _pcShowMsgInforma.h);
        _pcShowMsg.w = _pcShowMsgInforma.w;
        _pcShowMsg.h = _pcShowMsgInforma.h;
        _pcShowMsg.IsShowLine = _pcShowMsgInforma.IsShowLine;
        _pcShowMsg.LineDistance = _pcShowMsgInforma.LineDistance;
        return action;
    }

    void OnEnable()
    {
        if (preShow)
        {
            if (!msgRect)
            {
                msgRect = Instantiate<GameObject>(preShow.GetChild(0).gameObject, preShow).GetComponent<RectTransform>();
            }
            PreShowVisibility(false);
            SetPreShowSize();
        }
    }

    void Awake()
    {
        preShow = Manager.Instace.transform.Find("MsgContainer");
        if (!msgRect)
        {
            msgRect = Instantiate<GameObject>(preShow.GetChild(0).gameObject, preShow).GetComponent<RectTransform>();
        }
        //fonts = new Font[2];
        fonts[0] = msgRect.GetChild(0).GetComponent<Text>().font;
    }

    // Use this for initialization
    void Start()
    {
        //preShow = Manager.Instace.transform.Find ("MsgContainer");
        text = msgRect.GetChild(0).GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (text!=null)
        {
            text.color = ColorImage.color;
        }
        else
        {
            text= msgRect.GetChild(0).GetComponent<Text>();
        }
    }

    IEnumerator GetImage(string path)
    {
        WWW www = new WWW(@"file://" + path);
        yield return www;
        Texture2D tex2d = www.texture;
        targetSprite = Sprite.Create(tex2d, new Rect(0, 0, tex2d.width, tex2d.height), Vector3.zero);
        msgRect.GetComponent<Image>().sprite = targetSprite;
    }

    public void GetImage()
    {
        try
        {
            FileInfo fileInfo = new FileInfo(IOHelper.GetImageName());
            imgpath = fileInfo.FullName;
            imgName = fileInfo.Name;
            CopyImage();
            _pcShowMsg.imagePath = imgpath;
            _pcShowMsgInforma.imagePath = imgName;
            StartCoroutine(GetImage(imgpath));
        }
        catch
        {

        }
    }

    void CopyImage()
    {
        if (Directory.Exists(Application.dataPath + @"\images"))
        {
            File.Copy(imgpath, Application.dataPath + @"\images\" + imgName, true);
        }
        else
        {
            Directory.CreateDirectory(Application.dataPath + @"\images");
            File.Copy(imgpath, Application.dataPath + @"\images\" + imgName, true);
        }
    }

    //public void UpdateInput()
    //{
    //    UpdateStyle();
    //    PCShowMsg pcshowMsg = (PCShowMsg)action;
    //    try
    //    {
    //        pcshowMsg.msg = msgField.text;
    //        pcshowMsg.fontSize = int.Parse(fontSize.text);
    //        pcshowMsg.style = style;
    //        pcshowMsg.font = fonts[fontId.value];
    //        pcshowMsg.clickOpen = clickOpen.isOn;
    //        pcshowMsg.spacing = float.Parse(spacing.text);
    //        pcshowMsg.showPos = msgRect.localPosition;
    //        pcshowMsg.w = float.Parse(w.text);
    //        pcshowMsg.h = float.Parse(h.text);
    //        pcshowMsg.IsCover = IsCover.isOn;
    //        pcshowMsg.CanDrag = CanDragToggle.isOn;
    //        PCShowMsgInforma psInforma = (PCShowMsgInforma)actionInforma;
    //        //将属性值保存
    //        psInforma.msg = msgField.text;
    //        psInforma.fontSize = int.Parse(fontSize.text);
    //        psInforma.fontStyle = (int)style;
    //        psInforma.font = fontId.value;
    //        psInforma.clickOpen = clickOpen.isOn;
    //        psInforma.spacing = float.Parse(spacing.text);
    //        psInforma.px = msgRect.localPosition.x;
    //        psInforma.py = msgRect.localPosition.y;
    //        psInforma.pz = msgRect.localPosition.z;
    //        psInforma.w = float.Parse(w.text);
    //        psInforma.h = float.Parse(h.text);
    //        psInforma.IsCover = IsCover.isOn;
    //        psInforma.CanDrag = CanDragToggle.isOn;
    //    }
    //    catch
    //    {
    //    }
    //}

    public void UpdatePreShowText()
    {
        if (preShow)
        {
            msgRect.GetChild(0).GetComponent<TextSpacing>()._textSpacing = float.Parse(spacing.text);
            msgRect.GetChild(0).GetComponent<Text>().text = "";
            msgRect.GetChild(0).GetComponent<Text>().text = msgField.text;
            msgRect.GetChild(0).GetComponent<Text>().fontStyle = style;
            msgRect.GetChild(0).GetComponent<Text>().fontSize = int.Parse(fontSize.text);
            msgRect.GetChild(0).GetComponent<Text>().font = fonts[fontId.value];
            msgRect.GetChild(0).GetComponent<Text>().color = _pcShowMsg.color;
            //Debug.Log("IsCover="+IsCover.isOn);
            try
            {
                msgRect.sizeDelta = new Vector2(float.Parse(w.text), float.Parse(h.text));
            }
            catch
            {

            }
        }
    }

    public void PreShowVisibility(bool isShow)
    {
        msgRect.gameObject.SetActive(isShow);
    }

    public void UpdateStyle()
    {
        if (bold.isOn && italic.isOn)
        {
            style = FontStyle.BoldAndItalic;
        }
        else if (bold.isOn)
        {
            style = FontStyle.Bold;
        }
        else if (italic.isOn)
        {
            style = FontStyle.Italic;
        }
        else
        {
            style = FontStyle.Normal;
        }
        _pcShowMsgInforma.fontStyle = (int)style;
        _pcShowMsg.style = style;
        UpdatePreShowText();
    }

    public void SetPreShowSize()
    {
        try
        {
            RectTransform rt = msgRect;
            rt.sizeDelta = new Vector2(float.Parse(w.text), float.Parse(h.text));
        }
        catch
        {

        }
    }

    void SetStyle(int styleId)
    {
        switch (styleId)
        {
            case 0:
                bold.isOn = false;
                italic.isOn = false;
                break;
            case 1:
                bold.isOn = true;
                italic.isOn = false;
                break;
            case 2:
                bold.isOn = false;
                italic.isOn = true;
                break;
            case 3:
                bold.isOn = true;
                italic.isOn = true;
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 当允许覆盖的值改变时
    /// </summary>
    public void OnIsCoverChanged()
    {
        _pcShowMsgInforma.IsCover = IsCover.isOn;
        _pcShowMsg.IsCover = IsCover.isOn;

        //msgRect.GetComponent<DragableUI>().IsCover = IsCover.isOn;
    }
    /// <summary>
    /// 当允许拖动的值改变时
    /// </summary>
    public void OnCanDragChanged()
    {
        _pcShowMsgInforma.CanDrag = CanDragToggle.isOn;
        _pcShowMsg.CanDrag = CanDragToggle.isOn;
    }
    /// <summary>
    /// 当隐藏按钮的值改变时
    /// </summary>
    public void OnHideButtonChanged()
    {
        _pcShowMsgInforma.hideButton = HideButtonToggle.isOn;
        _pcShowMsg.hideButton = HideButtonToggle.isOn;
    }

    /// <summary>
    /// 当字体大小改变时
    /// </summary>
    public void OnFontSizeChanged()
    {
        _pcShowMsgInforma.fontSize = int.Parse(fontSize.text);
        _pcShowMsg.fontSize = _pcShowMsgInforma.fontSize;

        UpdatePreShowText();
    }
    /// <summary>
    /// 当字体被改变时
    /// </summary>
    public void OnFontChanged()
    {
        _pcShowMsgInforma.font = fontId.value;
        _pcShowMsg.font = fonts[fontId.value];

        UpdatePreShowText();
    }

    /// <summary>
    /// 当宽的值改变时
    /// </summary>
    public void OnWideChanged()
    {
        _pcShowMsgInforma.w = float.Parse(w.text);
        _pcShowMsg.w = _pcShowMsgInforma.w;

        UpdatePreShowText();

    }
    /// <summary>
    /// 当高的值改变时
    /// </summary>
    public void OnHighChanged()
    {
        _pcShowMsgInforma.h = float.Parse(h.text);
        _pcShowMsg.h = _pcShowMsgInforma.h;

        UpdatePreShowText();
    }

    /// <summary>
    /// 确认位置
    /// </summary>
    public void ConfirmPosition()
    {
        _pcShowMsgInforma.px = msgRect.localPosition.x;
        _pcShowMsgInforma.py = msgRect.localPosition.y;
        _pcShowMsgInforma.pz = msgRect.localPosition.z;
        _pcShowMsg.showPos = new Vector3(msgRect.localPosition.x, msgRect.localPosition.y, msgRect.localPosition.z);
    }
    /// <summary>
    /// 当文本框中的内容发生改变时
    /// </summary>
    public void OnTextChanged()
    {
        _pcShowMsgInforma.msg = msgField.text;
        _pcShowMsg.msg = msgField.text;

        UpdatePreShowText();
    }

    /// <summary>
    /// 设置颜色
    /// </summary>
    public void SetColor()
    {
        Manager.Instace.ColorPicker.SetActive(true);
        Manager.Instace.ColorPicker.GetComponent<ColorPickerUI>().image = ColorImage;
        Manager.Instace.ColorPicker.GetComponent<ColorPickerUI>().SetPcShowMessageFontColor(_pcShowMsg, _pcShowMsgInforma);
        //UpdatePreShowText();
    }

    /// <summary>
    /// 修改显示连线
    /// </summary>
    public void UpdateShowLine()
    {
        _pcShowMsg.IsShowLine = IsShowLine.isOn;
        _pcShowMsgInforma.IsShowLine = IsShowLine.isOn; //Edit by pxf
     

    }

    /// <summary>
    /// 修改显示连线属性
    /// </summary>
    public void UpdateShowLineDistanceText()
    {
        _pcShowMsg.LineDistance = float.Parse(LineDisdence.text);
        _pcShowMsgInforma.LineDistance = float.Parse(LineDisdence.text); //Edit by pxf
    }
}
