using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ShowImgUI : ActionUI
{
    public Image image;
    public InputField rotate, x, y, z, w, h;
    public Toggle clickOpen;
    public string imgpath, imgName;
    public Sprite targetSprite;
    public Transform preShow;
    public int ajustMode;
    public Toggle AjustMode0Toggle;
    public Toggle AjustMode1Toggle;
    public Toggle AjustMode2Toggle;
    /// <summary>
    /// 设置为背景的toggle
    /// </summary>
    public Toggle SetBG;
    //public InputField timeInputField;
    ShowImg _showImg;
    ShowImgInforma _sImgInforma;
    RectTransform imgRect;

    public override Action<Main> CreateAction()
    {
        action = new ShowImg();
        action.isOnce = true;
        actionInforma = new ShowImgInforma(true);
        _sImgInforma = (ShowImgInforma)actionInforma;
        _showImg = (ShowImg)action;
        //		showImg.canvas = Instantiate<GameObject> (Resources.Load<GameObject> ("Prefabs/ImgCanvas")).GetComponent<RectTransform>();
        //		showImg.canvas=preShow.GetChild(0) as RectTransform;
        //初始化变量
        ajustMode = 0;
        _sImgInforma.ajustMode = 0;
        _showImg.ajustMode = 0;
        AjustMode0Toggle.isOn = true;
        AjustMode1Toggle.isOn = false;
        AjustMode2Toggle.isOn = false;
        if (!imgRect)
        {
            imgRect = Instantiate<GameObject>(preShow.GetChild(0).gameObject, preShow).GetComponent<RectTransform>();
        }
        _showImg.canvas = imgRect;
        GetStateInfo().actionList.Add(actionInforma);
        actionInforma.name = "ShowImg";
        return base.CreateAction();
    }

    public override Action<Main> LoadAction(ActionInforma actionInforma)
    {
        _sImgInforma = (ShowImgInforma)actionInforma;
        this.actionInforma = actionInforma;
        action = new ShowImg();
        _showImg = (ShowImg)action;
        //读取数据
        rotate.text = _sImgInforma.rotate.ToString();
        x.text = _sImgInforma.x.ToString();
        y.text = _sImgInforma.y.ToString();
        z.text = _sImgInforma.z.ToString();
        w.text = _sImgInforma.w.ToString();
        h.text = _sImgInforma.h.ToString();

        //		clickOpen.isOn = sImgInforma.clickOpen;
        
        SetBG.isOn = _sImgInforma.ISBG;
        timeInputField.text = _showImg.duringTime.ToString();
        //		showImg.canvas = Instantiate<GameObject> (Resources.Load<GameObject> ("Prefabs/ImgCanvas")).GetComponent<RectTransform>();
        //		targetImg = showImg.canvas.GetComponent<Image> ();
        //		targetImg.sprite=ResLoader.resLoader.GetSprite (sImgInforma.imgpath, ref targetSprite);
        //		showImg.canvas=preShow.GetChild(0) as RectTransform;
        if (!imgRect)
        {
            imgRect = Instantiate<GameObject>(preShow.GetChild(0).gameObject, preShow).GetComponent<RectTransform>();
        }
        _showImg.canvas = imgRect;
        if (!string.IsNullOrEmpty(_sImgInforma.imgName))
        {
            //showImg.sprite = ResLoader.resLoader.GetSprite (sImgInforma.imgpath, ref targetSprite);
        }
        //		image.sprite = targetSprite;
        clickOpen.isOn = _sImgInforma.clickOpen;
        if (string.IsNullOrEmpty(_sImgInforma.imgName))
        {
            _showImg.imgPath = "";
        }
        else
        {
            _showImg.imgPath = ResLoader.targetPath + @"\images\" + _sImgInforma.imgName;
        }
        _showImg.ajustMode = _sImgInforma.ajustMode;
        _showImg.showPos = new Vector3(_sImgInforma.px, _sImgInforma.py, _sImgInforma.pz);
        imgRect.localPosition = _showImg.showPos;
        if (!string.IsNullOrEmpty(_sImgInforma.imgName))
        {
            ResLoader.resLoader.StartCoroutine(ResLoader.resLoader.GetImgRes(ResLoader.targetPath + @"\images\" + _sImgInforma.imgName, image));
        }
        imgpath = ResLoader.targetPath + @"\images\" + _sImgInforma.imgName;
        _showImg.rotate = _sImgInforma.rotate;
        _showImg.x = _sImgInforma.x;
        _showImg.y = _sImgInforma.y;
        _showImg.z = _sImgInforma.z;
        _showImg.w = _sImgInforma.w;
        _showImg.h = _sImgInforma.h;
        _showImg.clickOpen = _sImgInforma.clickOpen;
        _showImg.IsBG = _sImgInforma.ISBG;

        ajustMode = _sImgInforma.ajustMode;
        _showImg.ajustMode = _sImgInforma.ajustMode;
        switch (ajustMode)
        {
            case 0:
                AjustMode0Toggle.isOn = true;
                AjustMode1Toggle.isOn = false;
                AjustMode2Toggle.isOn = false;
                break;
            case 1:
                AjustMode0Toggle.isOn = false;
                AjustMode1Toggle.isOn = true;
                AjustMode2Toggle.isOn = false;
                break;
            case 2:
                AjustMode0Toggle.isOn = false;
                AjustMode1Toggle.isOn = false;
                AjustMode2Toggle.isOn = true;
                break;
                default:
                AjustMode0Toggle.isOn = true;
                AjustMode1Toggle.isOn = false;
                AjustMode2Toggle.isOn = false;
                break;
        }
        UIController.Instance.SetImage(_showImg.imgPath, imgRect.GetComponent<Image>());

        return action;
    }

    void OnEnable()
    {
        if (preShow)
        {
            if (!imgRect)
            {
                imgRect = Instantiate<GameObject>(preShow.GetChild(0).gameObject, preShow).GetComponent<RectTransform>();
            }
            PreShowVisibility(false);
            SetPreShowSize();
        }
    }

    void Awake()
    {
        preShow = Manager.Instace.transform.Find("ImgContainer");
    }

    void Start()
    {
        timeInputField.onValueChanged.AddListener(delegate (string a) { ActionTimeChanged(); });
    }
    void ActionTimeChanged()
    {
        if (_showImg != null)
        {
            _showImg.duringTime = float.Parse(timeInputField.text);
            _sImgInforma.durtime = _showImg.duringTime;
        }
    }
    /// <summary>
    /// 背景图设置
    /// </summary>
    public void SetBg(Toggle toggle)
    {
        if (toggle.isOn == true)
        {
            _sImgInforma.ISBG = true;
            _showImg.IsBG = true;
        }
        else
        {
            _sImgInforma.ISBG = false;
            _showImg.IsBG = false;
        }
    }

    public void SetAjustMode(int mode)
    {
        ajustMode = mode;
        SetImgAjust();
        _showImg.ajustMode = ajustMode;
        _sImgInforma.ajustMode = ajustMode;
    }

    public void GetImage()
    {
        try
        {
            FileInfo fileInfo = new FileInfo(IOHelper.GetImageName());
            imgpath = fileInfo.FullName;
            imgName = fileInfo.Name;
            CopyImage();
            _showImg = (ShowImg)action;
            _showImg.imgPath = imgpath;
            _showImg.ajustMode = ajustMode;
            if (_sImgInforma == null)
            {
                _sImgInforma = (ShowImgInforma)actionInforma;
            }
            _sImgInforma.ajustMode = ajustMode;
            _sImgInforma.imgName = imgName;
            //Debug.LogError(imgpath);
            StartCoroutine(GetImage(imgpath));
            UpdateInput();
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }
    }

    IEnumerator GetImage(string path)
    {
        WWW www = new WWW(@"file://" + path);
        yield return www;
        Texture2D tex2d = www.texture;
        targetSprite = Sprite.Create(tex2d, new Rect(0, 0, tex2d.width, tex2d.height), Vector3.zero);
        image.sprite = targetSprite;
        imgRect.GetComponent<Image>().sprite = targetSprite;
        SetImgAjust();
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

    public void UpdateInput()
    {
        ShowImg showImg = (ShowImg)action;
        try
        {
            showImg.rotate = float.Parse(rotate.text);
            showImg.x = float.Parse(x.text);
            showImg.y = float.Parse(y.text);
            showImg.z = float.Parse(z.text);
            showImg.w = float.Parse(w.text);
            showImg.h = float.Parse(h.text);
            showImg.clickOpen = clickOpen.isOn;
            showImg.sprite = targetSprite;
            showImg.showPos = imgRect.localPosition;
            ShowImgInforma showImgInforma = (ShowImgInforma)actionInforma;
            //将属性值保存
            showImgInforma.rotate = float.Parse(rotate.text);
            showImgInforma.x = float.Parse(x.text);
            showImgInforma.y = float.Parse(y.text);
            showImgInforma.z = float.Parse(z.text);
            showImgInforma.w = float.Parse(w.text);
            showImgInforma.h = float.Parse(h.text);
            showImgInforma.clickOpen = clickOpen.isOn;
            showImgInforma.imgName = imgName;
            showImgInforma.px = imgRect.localPosition.x;
            showImgInforma.py = imgRect.localPosition.y;
            showImgInforma.pz = imgRect.localPosition.z;
        }
        catch(System.Exception e)
        {
            Debug.LogError(e);
        }
    }

    public void PreShowVisibility(bool isShow)
    {
        imgRect.gameObject.SetActive(isShow);
    }

    public void SetPreShowSize()
    {
        try
        {
            RectTransform rt = imgRect;
            rt.sizeDelta = new Vector2(float.Parse(w.text), float.Parse(h.text));
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }
    }

    public void SetPreShowImg()
    {
        imgRect.GetComponent<Image>().sprite = image.sprite;
        SetImgAjust();
        //SetPreShowSize();
    }

    public void SetImgAjust()
    {
        //Vector2 containerSize = preShow.GetComponent<RectTransform>().sizeDelta;
        float imgSizeW = imgRect.GetComponent<Image>().preferredWidth;
        float imgSizeH = imgRect.GetComponent<Image>().preferredHeight;
        switch (ajustMode)
        {
            case 0:
                imgRect.sizeDelta = new Vector2(float.Parse(w.text),float.Parse(h.text));
                break;
            case 1:
                imgRect.sizeDelta = new Vector2(float.Parse(w.text), float.Parse(w.text) * imgSizeH / imgSizeW);
                break;
            case 2:
                imgRect.sizeDelta = new Vector2(float.Parse(h.text) * imgSizeW / imgSizeH, float.Parse(h.text));
                break;
            default:
                imgRect.sizeDelta = new Vector2(float.Parse(w.text), float.Parse(h.text));
                break;
        }
    }
    /// <summary>
    /// 当宽高变化时
    /// </summary>
    public void OnSizeChanged(int a)
    {
        switch (a)
        {
            case 0:
                //当宽度修改了
                _sImgInforma.w = float.Parse(w.text);
                _showImg.w = _sImgInforma.w;
                break;
            case 1:
                //当高度修改了
                _sImgInforma.h = float.Parse(h.text);
                _showImg.h = float.Parse(h.text);
                break;
        }
        SetImgAjust();
    }
    /// <summary>
    /// 锁定位置
    /// </summary>
    public void SetPosition()
    {
        _sImgInforma.px = imgRect.localPosition.x;
        _sImgInforma.py = imgRect.localPosition.y;
        _sImgInforma.pz = imgRect.localPosition.z;
        _showImg.showPos = imgRect.localPosition;
    }
}
