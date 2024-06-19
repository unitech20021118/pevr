using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;
using VRTK.Examples;

public class ShowInterfaceUI : ActionUI
{
    /// <summary>
    /// 选择添加UI的下拉列表
    /// </summary>
    public Dropdown Dropdown;
    /// <summary>
    /// 可以用鼠标拖动修改UI大小的组件
    /// </summary>
    private GameObject frameImagePrefab;
    /// <summary>
    /// 添加后的UIitem所在的父物体
    /// </summary>
    private Transform contentTransform;
    /// <summary>
    /// UI本体所在的父物体
    /// </summary>
    private Transform pevrUIParentTransform;
    /// <summary>
    /// 添加后的UI的item的预设物
    /// </summary>
    private  GameObject itemUIPrefab;

    /// <summary>
    /// 文本预设
    /// </summary>
    private GameObject textPrefab;
    /// <summary>
    /// 图片预设
    /// </summary>
    private GameObject imagePrefab;
    /// <summary>
    /// 按钮预设
    /// </summary>
    private GameObject buttonPrefab;
    /// <summary>
    /// 选项框预设
    /// </summary>
    private GameObject togglePrefab;


    /// <summary>
    /// 当前编辑的属性
    /// </summary>
    private InterfaceQualityInforma _nowEditInterfaceQualityInforma;

    private InterfaceQuality _nowEditInterfaceQuality;
    /// <summary>
    /// 当前编辑的UI物体
    /// </summary>
    private GameObject _nowEditUiGameObject;
    /// <summary>
    /// 当前打开的编辑界面
    /// </summary>
    private GameObject _nowEditPanelGameObject;
    /// <summary>
    /// 当前创建出来的UI
    /// </summary>
    private GameObject UI;
    /// <summary>
    /// 当前创建出来的UI的item
    /// </summary>
    private GameObject PEVRUIItem;
    /// <summary>
    /// 当前编辑的itemui
    /// </summary>
    private ItemUI itemUi;

    private ShowInterface _showInterface;
    private ShowInterfaceInforma _showInterfaceInforma;

    public InterfaceQualityInforma NowEditInterfaceQualityInforma
    {
        get { return _nowEditInterfaceQualityInforma; }
    }

    public InterfaceQuality NowEditInterfaceQuality
    {
        get { return _nowEditInterfaceQuality; }
    }
    public GameObject ItemUIPrefab
    {
        get
        {
            if (itemUIPrefab==null)
            {
                itemUIPrefab = Resources.Load<GameObject>("PEVRUI/ItemUI");
            }
            return itemUIPrefab;
        }
    }

    public GameObject TextPrefab
    {
        get
        {
            if (textPrefab == null)
            {
                textPrefab = Resources.Load<GameObject>("PEVRUI/PEVRText");
            }
            return textPrefab;
        }
    }

    public GameObject ImagePrefab
    {
        get
        {
            if (imagePrefab == null)
            {
                imagePrefab = Resources.Load<GameObject>("PEVRUI/PEVRImage");
            }
            return imagePrefab;
        }
    }

    public GameObject ButtonPrefab
    {
        get
        {
            if (buttonPrefab == null)
            {
                buttonPrefab = Resources.Load<GameObject>("PEVRUI/PEVRButton");
            }
            return buttonPrefab;
        }
    }

    public GameObject TogglePrefab
    {
        get
        {
            if (togglePrefab == null)
            {
                togglePrefab = Resources.Load<GameObject>("PEVRUI/PEVRToggle");
            }
            return togglePrefab;
        }
    }

    public GameObject FrameImagePrefab
    {
        get
        {
            if (frameImagePrefab==null)
            {
                frameImagePrefab = Resources.Load<GameObject>("PEVRUI/FrameImage");
            }
            return frameImagePrefab;
        }
    }

    public Transform ContentTransform
    {
        get
        {
            if (contentTransform==null)
            {
                contentTransform = transform.FindChild("Scroll View/Viewport/Content");
            }
            return contentTransform;
        }
    }

    public Transform PevrUIParentTransform
    {
        get
        {
            if (pevrUIParentTransform==null)
            {
                pevrUIParentTransform = Manager.Instace.transform.FindChild("PEVRUIParent");
            }
            return pevrUIParentTransform;
        }
    }

   

    void Update()
    {
        //todo  更新字体颜色

        CommitUiSize();
    }

    public void CommitUiSize()
    {
        if (_nowEditPanelGameObject == TextEditPanelGameObject && _nowEditUiGameObject != null)
        {
            WidthInputField.text = _nowEditUiGameObject.GetComponent<RectTransform>().sizeDelta.x.ToString();
            HeightInputField.text = _nowEditUiGameObject.GetComponent<RectTransform>().sizeDelta.y.ToString();
        }
        else if (_nowEditPanelGameObject == ImageEditPanelGameObject && _nowEditUiGameObject != null)
        {
            ImageWidthInputField.text = _nowEditUiGameObject.GetComponent<RectTransform>().sizeDelta.x.ToString();
            ImageHeightInputField.text = _nowEditUiGameObject.GetComponent<RectTransform>().sizeDelta.y.ToString();
        }
    }
    
    public override Action<Main> CreateAction()
    {
        action = new ShowInterface();
        actionInforma = new ShowInterfaceInforma(true);
        _showInterface = (ShowInterface) action;
        _showInterfaceInforma = (ShowInterfaceInforma) actionInforma;
        


        GetStateInfo().actionList.Add(actionInforma);
        actionInforma.name = "ShowInterface";
        return base.CreateAction();
    }


    public override Action<Main> LoadAction(ActionInforma actionInforma)
    {
        _showInterfaceInforma = (ShowInterfaceInforma) actionInforma;
        this.actionInforma = actionInforma;
        action = new ShowInterface();
        _showInterface = (ShowInterface) action;

        // 读取相关数据
        if (_showInterfaceInforma.InterfaceQualityInformas.Count>0)
        {
            
            for (int i = 0; i < _showInterfaceInforma.InterfaceQualityInformas.Count; i++)
            {
                if (_showInterfaceInforma.InterfaceQualityInformas[i].Pevrui == PEVRUI.Text)
                {
                       CreatUI(PEVRUI.Text,true);
                    Text UiText = UI.GetComponent<Text>();
                    UiText.text = _showInterfaceInforma.InterfaceQualityInformas[i]
                        .TextQualityInforma.TextStr;
                    UiText.font = Fonts[_showInterfaceInforma.InterfaceQualityInformas[i]
                        .TextQualityInforma.Font];
                    UiText.fontSize = _showInterfaceInforma.InterfaceQualityInformas[i]
                        .TextQualityInforma.TextFontSize;
                    UiText.fontStyle = GetFontStyleByInt(_showInterfaceInforma.InterfaceQualityInformas[i]
                        .TextQualityInforma.TextFontStyle);
                    UiText.color = Manager.Instace.GetColor(_showInterfaceInforma.InterfaceQualityInformas[i]
                        .TextQualityInforma.TextColor);
                    UI.transform.position = new Vector3(_showInterfaceInforma.InterfaceQualityInformas[i]
                        .TextQualityInforma.TextPositionX, _showInterfaceInforma.InterfaceQualityInformas[i]
                        .TextQualityInforma.TextPositionY, _showInterfaceInforma.InterfaceQualityInformas[i]
                        .TextQualityInforma.TextPositionZ);
                    UI.GetComponent<RectTransform>().sizeDelta = new Vector2(_showInterfaceInforma.InterfaceQualityInformas[i]
                        .TextQualityInforma.TextWidth, _showInterfaceInforma.InterfaceQualityInformas[i]
                        .TextQualityInforma.TextHeight);

                    //   将数据同步到执行动作的列表中

                    TextQuality textQuality = new TextQuality();
                    textQuality.Font = UiText.font;
                    textQuality.TextFontSize = UiText.fontSize;
                    textQuality.TextFontStyle = UiText.fontStyle;
                    textQuality.TextColor = UiText.color;
                    textQuality.TextPositionVector3 = UI.transform.position;
                    textQuality.TextRectSizeVector2 = UI.GetComponent<RectTransform>().sizeDelta;
                    textQuality.TextStr = UiText.text;
                    textQuality.TextUiGameObject = UI;

                    InterfaceQuality interfaceQuality = new InterfaceQuality(textQuality);
                    _showInterface.InterfaceQualitys.Add(interfaceQuality);


                    itemUi.InterfaceQualityInforma = _showInterfaceInforma.InterfaceQualityInformas[i];
                    itemUi.InterfaceQuality = _showInterface.InterfaceQualitys[i];
                }
                else if (_showInterfaceInforma.InterfaceQualityInformas[i].Pevrui == PEVRUI.Image)
                {
                    CreatUI(PEVRUI.Image, true);
                   
                    UI.transform.position = new Vector3(
                        _showInterfaceInforma.InterfaceQualityInformas[i].ImageQualityInforma.ImagePositionX, 
                        _showInterfaceInforma.InterfaceQualityInformas[i].ImageQualityInforma.ImagePositionY, 
                        _showInterfaceInforma.InterfaceQualityInformas[i].ImageQualityInforma.ImagePositionZ);
                    UI.GetComponent<RectTransform>().sizeDelta = new Vector2(
                        _showInterfaceInforma.InterfaceQualityInformas[i].ImageQualityInforma.ImageWidth,
                        _showInterfaceInforma.InterfaceQualityInformas[i].ImageQualityInforma.ImageHeight);

                    // todo ajustMode

                    // 将数据同步到执行动作的列表中
                    ImageQuality imageQuality = new ImageQuality();
                    if (_showInterfaceInforma.InterfaceQualityInformas[i].ImageQualityInforma.ImageName!="")
                    {
                        imageQuality.ImagePath = Application.streamingAssetsPath + "/images/" + _showInterfaceInforma
                                          .InterfaceQualityInformas[i].ImageQualityInforma.ImageName;
                        //修改图片
                       ResLoader.resLoader.StartCoroutine(ResLoader.resLoader.GetImgRes(imageQuality.ImagePath, UI.GetComponent<Image>()));
                        //StartCoroutine(GetImage(imageQuality.ImagePath, UI.GetComponent<Image>()));
                    }
                    imageQuality.ImagePositionVector3 = UI.transform.position;
                    imageQuality.ImageRectSizeVector2 = UI.GetComponent<RectTransform>().sizeDelta;
                    imageQuality.IsBg = _showInterfaceInforma.InterfaceQualityInformas[i].ImageQualityInforma.IsBg;
                    imageQuality.AjustMode = _showInterfaceInforma.InterfaceQualityInformas[i].ImageQualityInforma.AjustMode;
                    imageQuality.ImageUiGameObject = UI;

                    InterfaceQuality interfaceQuality = new InterfaceQuality(imageQuality);
                    _showInterface.InterfaceQualitys.Add(interfaceQuality);


                    itemUi.InterfaceQualityInforma = _showInterfaceInforma.InterfaceQualityInformas[i];
                    itemUi.InterfaceQuality = _showInterface.InterfaceQualitys[i];
                }
                else if (_showInterfaceInforma.InterfaceQualityInformas[i].Pevrui == PEVRUI.Button)
                {
                    CreatUI(PEVRUI.Button, true);


                    Text buttonText = UI.GetComponentInChildren<Text>();
                    ButtonQuality buttonQuality = new ButtonQuality();


                    UI.transform.position = new Vector3(
                        _showInterfaceInforma.InterfaceQualityInformas[i].ButtonQualityInforma.ButtonPositionX, 
                        _showInterfaceInforma.InterfaceQualityInformas[i].ButtonQualityInforma.ButtonPositionY, 
                        _showInterfaceInforma.InterfaceQualityInformas[i].ButtonQualityInforma.ButtonPositionZ);
                    UI.GetComponent<RectTransform>().sizeDelta = new Vector2(
                        _showInterfaceInforma.InterfaceQualityInformas[i].ButtonQualityInforma.ButtonWidth,
                        _showInterfaceInforma.InterfaceQualityInformas[i].ButtonQualityInforma.ButtonHeight);

                    buttonText.text = _showInterfaceInforma.InterfaceQualityInformas[i]
                        .ButtonQualityInforma.ButtonText;
                    buttonText.fontSize = _showInterfaceInforma.InterfaceQualityInformas[i]
                        .ButtonQualityInforma.TextFontSize;
                    buttonText.fontStyle = GetFontStyleByInt(_showInterfaceInforma.InterfaceQualityInformas[i]
                        .ButtonQualityInforma.TextFontStyle);
                    buttonText.font = Fonts[_showInterfaceInforma.InterfaceQualityInformas[i]
                        .ButtonQualityInforma.Font];
                    buttonText.color = Manager.Instace.GetColor(_showInterfaceInforma.InterfaceQualityInformas[i]
                        .ButtonQualityInforma.TextColor);

                    //将数据同步到要执行的动作列表中

                    if (_showInterfaceInforma.InterfaceQualityInformas[i].ButtonQualityInforma.ImageName!="")
                    {
                        buttonQuality.ImagePath = Application.streamingAssetsPath + "/images/" + _showInterfaceInforma
                                                      .InterfaceQualityInformas[i].ButtonQualityInforma.ImageName;
                        //修改图片
                        ResLoader.resLoader.StartCoroutine(
                            ResLoader.resLoader.GetImgRes(buttonQuality.ImagePath, UI.GetComponent<Image>()));
                    }
                    
                    buttonQuality.Font = buttonText.font;
                    buttonQuality.TextFontSize = buttonText.fontSize;
                    buttonQuality.TextFontStyle = buttonText.fontStyle;
                    buttonQuality.TextColor = buttonText.color;

                    buttonQuality.ButtonPositionVector3 = UI.transform.position;
                    buttonQuality.ButtonRectSizeVector2 = UI.GetComponent<RectTransform>().sizeDelta;
                    buttonQuality.EventName =
                        _showInterfaceInforma.InterfaceQualityInformas[i].ButtonQualityInforma.EventName;
                    buttonQuality.ButtonUiGameObject = UI;
                    foreach (Events e in Manager.Instace.eventlist)
                    {
                        if (e.name == _showInterfaceInforma.InterfaceQualityInformas[i].ButtonQualityInforma.EventName)
                        {
                            buttonQuality.Events = e;
                        }
                    }
                    InterfaceQuality interfaceQuality = new InterfaceQuality(buttonQuality);
                    _showInterface.InterfaceQualitys.Add(interfaceQuality);
                    itemUi.InterfaceQualityInforma = _showInterfaceInforma.InterfaceQualityInformas[i];
                    itemUi.InterfaceQuality = _showInterface.InterfaceQualitys[i];

                }
            }
        }

        if (_showInterfaceInforma.CloseIndexList.Count>0)
        {
            CloseIndex = _showInterfaceInforma.CloseIndexList;
            _showInterface.CloseIndexList = _showInterfaceInforma.CloseIndexList;
        }

        return base.LoadAction(actionInforma);
    }


    /// <summary>
    /// 当下拉列表选择改变时
    /// </summary>
    public void OnDropDownChanged()
    {
        switch (Dropdown.value)
        {
            case 0:
                break;
            case 1:
                CreatUI(PEVRUI.Text);
                Dropdown.value = 0;
                break;
            case 2:
                CreatUI(PEVRUI.Image);
                Dropdown.value = 0;
                break;
            case 3:
                CreatUI(PEVRUI.Button);
                Dropdown.value = 0;
                break;
            case 4:
                CreatUI(PEVRUI.Toggle);
                Dropdown.value = 0;
                break;
                default:
                    break;
        }
    }

    /// <summary>
    /// 创建UI
    /// </summary>
    public void CreatUI(PEVRUI pevrui,bool load=false)
    {
        
        switch (pevrui)
        {
            case PEVRUI.Text://Text
                //将预设的TEXT创建到界面中
                UI = Instantiate(TextPrefab);
                UI.transform.SetParent(PevrUIParentTransform);
                UI.transform.localScale=Vector3.one;
                
                //初始位置修改
                UI.transform.localPosition = new Vector3(100,100,0);
                AddResizeUI(UI);
                //创建TEXT的item
                PEVRUIItem = Instantiate(ItemUIPrefab);
                PEVRUIItem.transform.SetParent(ContentTransform);
                PEVRUIItem.transform.localScale=Vector3.one;
                //添加相关属性
                itemUi = PEVRUIItem.GetComponent<ItemUI>();
                itemUi.SetUIBody(UI, "Text");
                if (!load)
                {
                    TextQualityInforma textQualityInforma = new TextQualityInforma();
                    TextQuality textQuality = new TextQuality();
                    itemUi.InterfaceQualityInforma = new InterfaceQualityInforma(textQualityInforma);
                    itemUi.InterfaceQuality = new InterfaceQuality(textQuality);
                    itemUi.InterfaceQuality.TextQuality.TextUiGameObject = UI;
                    _showInterfaceInforma.InterfaceQualityInformas.Add(itemUi.InterfaceQualityInforma);
                    _showInterface.InterfaceQualitys.Add(itemUi.InterfaceQuality);
                }
                else
                {
                    UI.SetActive(false);
                }
                break;
            case PEVRUI.Image:
                //将预设的Image创建到界面中
                UI = Instantiate(ImagePrefab);
                UI.transform.SetParent(PevrUIParentTransform);
                UI.transform.localScale = Vector3.one;
                //初始位置修改
                UI.transform.localPosition = new Vector3(100, 100, 0);
                AddResizeUI(UI);
                //创建image的item
                PEVRUIItem = Instantiate(ItemUIPrefab);
                PEVRUIItem.transform.SetParent(ContentTransform);
                PEVRUIItem.transform.localScale = Vector3.one;
                //添加相关属性
                itemUi = PEVRUIItem.GetComponent<ItemUI>();
                itemUi.SetUIBody(UI, "Image");

                if (!load)
                {
                    ImageQualityInforma qualityInforma = new ImageQualityInforma();
                    ImageQuality imageQUality = new ImageQuality();
                    itemUi.InterfaceQualityInforma = new InterfaceQualityInforma(qualityInforma);
                    itemUi.InterfaceQuality = new InterfaceQuality(imageQUality);
                    itemUi.InterfaceQuality.ImageQuality.ImageUiGameObject = UI;
                    _showInterfaceInforma.InterfaceQualityInformas.Add(itemUi.InterfaceQualityInforma);
                    _showInterface.InterfaceQualitys.Add(itemUi.InterfaceQuality);
                }
                else
                {
                    UI.SetActive(false);
                }
                break;
            case PEVRUI.Button:
                //将预设的TEXT创建到界面中
                UI = Instantiate(ButtonPrefab);
                UI.transform.SetParent(PevrUIParentTransform);
                UI.transform.localScale = Vector3.one;
                //初始位置修改
                UI.transform.localPosition = new Vector3(100, 100, 0);
                AddResizeUI(UI);
                //创建Button的item
                PEVRUIItem = Instantiate(ItemUIPrefab);
                PEVRUIItem.transform.SetParent(ContentTransform);
                PEVRUIItem.transform.localScale = Vector3.one;
                //添加相关属性
                itemUi = PEVRUIItem.GetComponent<ItemUI>();
                itemUi.SetUIBody(UI,"Button");
                if (!load)
                {
                    ButtonQualityInforma buttonQualityInforma = new ButtonQualityInforma();
                    ButtonQuality buttonQuality = new ButtonQuality();
                    itemUi.InterfaceQualityInforma = new InterfaceQualityInforma(buttonQualityInforma);
                    itemUi.InterfaceQuality = new InterfaceQuality(buttonQuality);
                    itemUi.InterfaceQuality.ButtonQuality.ButtonUiGameObject = UI;
                    _showInterfaceInforma.InterfaceQualityInformas.Add(itemUi.InterfaceQualityInforma);
                    _showInterface.InterfaceQualitys.Add(itemUi.InterfaceQuality);
                }
                else
                {
                    UI.SetActive(false);
                }
                break;
            //case PEVRUI.Toggle:
            //    //将预设的toggle创建到界面中
            //    UI = Instantiate(TogglePrefab);
            //    UI.transform.SetParent(PevrUIParentTransform);
            //    UI.transform.localScale = Vector3.one;
            //    //初始位置修改
            //    UI.transform.localPosition = new Vector3(100, 100, 0);
            //    //创建TEXT的item
            //    PEVRUIItem = Instantiate(ItemUIPrefab);
            //    PEVRUIItem.transform.SetParent(ContentTransform);
            //    PEVRUIItem.transform.localScale = Vector3.one;
            //    //添加相关属性
            //    PEVRUIItem.GetComponent<ItemUI>().SetUIBody(UI,"Toggle");
            //    break;
            default:
                break;
        }
    }
    /// <summary>
    /// 添加拖动控制UI
    /// </summary>
    public void AddResizeUI(GameObject go)
    {
        GameObject fi = Instantiate(FrameImagePrefab);
        fi.transform.SetParent(go.transform);
        fi.transform.localPosition = Vector3.zero;
        fi.transform.localScale = Vector3.one;
        RectTransform rect = fi.transform.GetComponent<RectTransform>();
        rect.offsetMax = Vector2.zero;
        rect.offsetMin = Vector2.zero;
    }

    public void OpenEditPanel()
    {
        if (ItemUI.ChoiceItemUi!=null)
        {
            ItemUI.ChoiceItemUi.uiBodyGameObject.SetActive(true);
            _nowEditUiGameObject = ItemUI.ChoiceItemUi.uiBodyGameObject;
            _nowEditInterfaceQualityInforma = ItemUI.ChoiceItemUi.InterfaceQualityInforma;
            _nowEditInterfaceQuality = ItemUI.ChoiceItemUi.InterfaceQuality;
            
            
            if (_nowEditInterfaceQualityInforma.Pevrui==PEVRUI.Text)
            {
                //打开文本编辑界面
                TextEditPanelGameObject.SetActive(true);
                _nowEditPanelGameObject = TextEditPanelGameObject;
                //将属性配置到UI上
                TextInputField.text = _nowEditInterfaceQualityInforma.TextQualityInforma.TextStr;
                FontSizeInputField.text =
                    _nowEditInterfaceQualityInforma.TextQualityInforma.TextFontSize.ToString();
                FontDropdown.value = _nowEditInterfaceQualityInforma.TextQualityInforma.Font;
                WidthInputField.text = _nowEditInterfaceQualityInforma.TextQualityInforma.TextWidth.ToString();
                HeightInputField.text = _nowEditInterfaceQualityInforma.TextQualityInforma.TextHeight.ToString();
                ColorImage.color =
                    Manager.Instace.GetColor(_nowEditInterfaceQualityInforma.TextQualityInforma.TextColor);
                FontStyleDropdown.value = _nowEditInterfaceQualityInforma.TextQualityInforma.TextFontStyle;
            }else if (_nowEditInterfaceQualityInforma.Pevrui == PEVRUI.Image)
            {
                //打开显示图片编辑界面
                ImageEditPanelGameObject.SetActive(true);
                _nowEditPanelGameObject = ImageEditPanelGameObject;
                //将属性配置到UI上
                AjustModeDropdown.value = _nowEditInterfaceQualityInforma.ImageQualityInforma.AjustMode;
                ImageWidthInputField.text = _nowEditInterfaceQualityInforma.ImageQualityInforma.ImageWidth.ToString();
                ImageHeightInputField.text = _nowEditInterfaceQualityInforma.ImageQualityInforma.ImageHeight.ToString();
                ImageIsBgToggle.isOn = _nowEditInterfaceQualityInforma.ImageQualityInforma.IsBg;
                if (_nowEditInterfaceQualityInforma.ImageQualityInforma.ImageName!="")
                {
                    imagePath = Application.streamingAssetsPath + "/images/" +
                                _nowEditInterfaceQualityInforma.ImageQualityInforma.ImageName;
                    StartCoroutine(GetImage(imagePath,PreviewImage));
                }
                
            }else if (_nowEditInterfaceQualityInforma.Pevrui == PEVRUI.Button)
            {
                //打开显示按钮的编辑界面
                ButtonEditPanelGameObject.SetActive(true);
                _nowEditPanelGameObject = ButtonEditPanelGameObject;
                //将属性配置到UI上
                ButtonWidthInputField.text = _nowEditInterfaceQualityInforma.ButtonQualityInforma.ButtonWidth.ToString();
                ButtonHeightInputField.text = _nowEditInterfaceQualityInforma.ButtonQualityInforma.ButtonHeight.ToString();
                ButtonFontSizeInputField.text = _nowEditInterfaceQualityInforma.ButtonQualityInforma.TextFontSize.ToString();
                ButtonFontDropdown.value = _nowEditInterfaceQualityInforma.ButtonQualityInforma.Font;
                ButtonFontStyleDropdown.value = _nowEditInterfaceQualityInforma.ButtonQualityInforma.TextFontStyle;
                ButtonTextColorImage.color = Manager.Instace.GetColor(_nowEditInterfaceQualityInforma.ButtonQualityInforma.TextColor);
                ButtonEvent.GetComponentInChildren<Text>().text = _nowEditInterfaceQualityInforma.ButtonQualityInforma.EventName!="" ? _nowEditInterfaceQualityInforma.ButtonQualityInforma.EventName : "选择事件";
                
                ButtonTextInputField.text = _nowEditInterfaceQualityInforma.ButtonQualityInforma.ButtonText;
                
                if (_nowEditInterfaceQualityInforma.ButtonQualityInforma.ImageName != "")
                {
                    imagePath = Application.streamingAssetsPath + "/images/" +
                                _nowEditInterfaceQualityInforma.ButtonQualityInforma.ImageName;
                    StartCoroutine(GetImage(imagePath, ButtonPreviewImage));
                }
            }
        }
    }
    /// <summary>
    /// 当隐藏按钮点击时
    /// </summary>
    public void OnHideButtonClick()
    {
        if (ItemUI.ChoiceItemUi!=null)
        {
            ItemUI.ChoiceItemUi.uiBodyGameObject.SetActive(false);
        }
        
    }
    /// <summary>
    /// 删除按钮按下时
    /// </summary>
    public void OnDeleteButtonClick()
    {
        if (ItemUI.ChoiceItemUi != null)
        {
            _showInterface.InterfaceQualitys.Remove(ItemUI.ChoiceItemUi.InterfaceQuality);
            _showInterfaceInforma.InterfaceQualityInformas.Remove(ItemUI.ChoiceItemUi.InterfaceQualityInforma);
            Destroy(ItemUI.ChoiceItemUi.uiBodyGameObject);
            Destroy(ItemUI.ChoiceItemUi.gameObject);
        }
    }

    public void OnReturnButtonClick()
    {
        if (_nowEditPanelGameObject!=null)
        {
            _nowEditPanelGameObject.SetActive(false);
        }
    }

    #region 文本部分
    
    // ================================================text=============================================
    /// <summary>
    /// 显示文本的编辑界面
    /// </summary>
    public GameObject TextEditPanelGameObject;
    /// <summary>
    /// 文本输入框
    /// </summary>
    public InputField TextInputField;
    /// <summary>
    /// 确认位置按钮
    /// </summary>
    public Button ComfirmPositionButton;
    /// <summary>
    /// 字体选择下拉列表
    /// </summary>
    public Dropdown FontStyleDropdown;
    /// <summary>
    /// 字体大小输入框
    /// </summary>
    public InputField FontSizeInputField;
    /// <summary>
    /// 字体下拉列表
    /// </summary>
    public Dropdown FontDropdown;
    /// <summary>
    /// 宽度输入框
    /// </summary>
    public InputField WidthInputField;
    /// <summary>
    /// 高度输入框
    /// </summary>
    public InputField HeightInputField;

    public Font[] Fonts;
    /// <summary>
    /// 显示当前颜色的图
    /// </summary>
    public Image ColorImage;
    /// <summary>
    /// 当文本框输入内容改变时
    /// </summary>
    public void OnTextInputFieldValueChanged()
    {
        _nowEditInterfaceQualityInforma.TextQualityInforma.TextStr = TextInputField.text;
        _nowEditUiGameObject.GetComponent<Text>().text = TextInputField.text;
        _nowEditInterfaceQuality.TextQuality.TextStr = TextInputField.text;
    }
    /// <summary>
    /// 当按下确定位置按钮时
    /// </summary>
    public void OnComfirmPositionButtonClick()
    {
        _nowEditInterfaceQualityInforma.TextQualityInforma.TextPositionX = _nowEditUiGameObject.transform.position.x;
        _nowEditInterfaceQualityInforma.TextQualityInforma.TextPositionY = _nowEditUiGameObject.transform.position.y;
        _nowEditInterfaceQualityInforma.TextQualityInforma.TextPositionZ = _nowEditUiGameObject.transform.position.z;
        _nowEditInterfaceQuality.TextQuality.TextPositionVector3 = _nowEditUiGameObject.transform.position;
    }
    /// <summary>
    /// 当字体大小改变时
    /// </summary>
    public void OnTextFontSizeInputFieldValueChanged()
    {
        _nowEditInterfaceQualityInforma.TextQualityInforma.TextFontSize = int.Parse(FontSizeInputField.text);
        _nowEditUiGameObject.GetComponent<Text>().fontSize = int.Parse(FontSizeInputField.text);
        _nowEditInterfaceQuality.TextQuality.TextFontSize = int.Parse(FontSizeInputField.text);
    }
    /// <summary>
    /// 当文本宽度改变时
    /// </summary>
    public void OnTextWidthInputFieldValueChanged()
    {
        _nowEditInterfaceQualityInforma.TextQualityInforma.TextWidth = float.Parse(WidthInputField.text);
        _nowEditUiGameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(float.Parse(WidthInputField.text), _nowEditUiGameObject.GetComponent<RectTransform>().sizeDelta.y);
        _nowEditInterfaceQuality.TextQuality.TextRectSizeVector2 = _nowEditUiGameObject.GetComponent<RectTransform>().sizeDelta;
    }
    /// <summary>
    /// 当文本高度改变时
    /// </summary>
    public void OnTextHeightInputFieldValueChanged()
    {
        _nowEditInterfaceQualityInforma.TextQualityInforma.TextHeight = float.Parse(HeightInputField.text);
        _nowEditUiGameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(_nowEditUiGameObject.GetComponent<RectTransform>().sizeDelta.x, float.Parse(HeightInputField.text));
        _nowEditInterfaceQuality.TextQuality.TextRectSizeVector2 = _nowEditUiGameObject.GetComponent<RectTransform>().sizeDelta;
    }
    /// <summary>
    /// 当字体模式改变时
    /// </summary>
    public void OnFontStyleDropDownValueChanged()
    {
        _nowEditInterfaceQualityInforma.TextQualityInforma.TextFontStyle = FontStyleDropdown.value;
        _nowEditUiGameObject.GetComponent<Text>().fontStyle = GetFontStyleByInt(FontStyleDropdown.value);
        _nowEditInterfaceQuality.TextQuality.TextFontStyle = GetFontStyleByInt(FontStyleDropdown.value);
    }

    /// <summary>
    /// 当字体改变时
    /// </summary>
    public void OnFontChanged()
    {
        _nowEditInterfaceQualityInforma.TextQualityInforma.Font = FontDropdown.value;
        _nowEditUiGameObject.GetComponent<Text>().font = Fonts[FontDropdown.value];
        _nowEditInterfaceQuality.TextQuality.Font = Fonts[FontDropdown.value];
    }
    /// <summary>
    /// 设置字体颜色
    /// </summary>
    public void SetColor()
    {
        Manager.Instace.ColorPicker.SetActive(true);
        Manager.Instace.ColorPicker.GetComponent<ColorPickerUI>().image = ColorImage;
        Manager.Instace.ColorPicker.GetComponent<ColorPickerUI>().SetShowInterfaceFontColor(_nowEditInterfaceQuality.TextQuality,_nowEditInterfaceQualityInforma.TextQualityInforma);
    }
    
    public FontStyle GetFontStyleByInt(int num)
    {
        switch (num)
        {
            case 0:
                return FontStyle.Normal;
            case 1:
                return FontStyle.Bold;
            case 2:
                return FontStyle.Italic;
            case 3:
                return FontStyle.BoldAndItalic;
                default:
                    return FontStyle.Normal;
        }
    }
    #endregion

    #region 图片部分
    //==============================================  Image  ============================================
    /// <summary>
    /// 显示图片编辑界面
    /// </summary>
    public GameObject ImageEditPanelGameObject;

    public Dropdown AjustModeDropdown;
    public InputField ImageWidthInputField;
    public InputField ImageHeightInputField;
    public Toggle ImageIsBgToggle;
    public Image PreviewImage;
    private string imageName;
    private string imagePath;

    public void ImageAjustModeDropDownChanged()
    {
        _nowEditInterfaceQualityInforma.ImageQualityInforma.AjustMode = AjustModeDropdown.value;
        //todo  ajustMode
        //_nowEditUiGameObject.GetComponent<Text>().text = TextInputField.text;
        _nowEditInterfaceQuality.ImageQuality.AjustMode = AjustModeDropdown.value;

    }

    public void ImageComfirmPositionClick()
    {
        _nowEditInterfaceQualityInforma.ImageQualityInforma.ImagePositionX = _nowEditUiGameObject.transform.position.x;
        _nowEditInterfaceQualityInforma.ImageQualityInforma.ImagePositionY = _nowEditUiGameObject.transform.position.y;
        _nowEditInterfaceQualityInforma.ImageQualityInforma.ImagePositionZ = _nowEditUiGameObject.transform.position.z;
        _nowEditInterfaceQuality.ImageQuality.ImagePositionVector3 = _nowEditUiGameObject.transform.position;
    }

    public void ImageSetBgToggleChanged()
    {
        _nowEditInterfaceQualityInforma.ImageQualityInforma.IsBg = ImageIsBgToggle.isOn;
        _nowEditInterfaceQuality.ImageQuality.IsBg = ImageIsBgToggle.isOn;
    }

    public void ImageChoicePicture()
    {
        FileInfo fileInfo = new FileInfo(IOHelper.GetImageName());
        imagePath = fileInfo.FullName;
        imageName = fileInfo.Name;
        CopyImage();
        _nowEditInterfaceQuality.ImageQuality.ImagePath = imagePath;
        _nowEditInterfaceQualityInforma.ImageQualityInforma.ImageName = imageName;
        StartCoroutine(GetImage(imagePath,PreviewImage));
        StartCoroutine(GetImage(imagePath, _nowEditUiGameObject.GetComponent<Image>()));
    }

    public void ImageWidthInputFieldValueChanged()
    {
        _nowEditInterfaceQualityInforma.ImageQualityInforma.ImageWidth = float.Parse(ImageWidthInputField.text);
        _nowEditUiGameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(float.Parse(ImageWidthInputField.text), _nowEditUiGameObject.GetComponent<RectTransform>().sizeDelta.y);
        _nowEditInterfaceQuality.ImageQuality.ImageRectSizeVector2 = _nowEditUiGameObject.GetComponent<RectTransform>().sizeDelta;
    }

    public void ImageHeightInputFieldValueChanged()
    {
        _nowEditInterfaceQualityInforma.ImageQualityInforma.ImageHeight = float.Parse(ImageHeightInputField.text);
        _nowEditUiGameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(_nowEditUiGameObject.GetComponent<RectTransform>().sizeDelta.x, float.Parse(ImageHeightInputField.text));
        _nowEditInterfaceQuality.ImageQuality.ImageRectSizeVector2 = _nowEditUiGameObject.GetComponent<RectTransform>().sizeDelta;
    }
    void CopyImage()
    {
        if (Directory.Exists(Application.streamingAssetsPath + @"\images"))
        {
            File.Copy(imagePath, Application.streamingAssetsPath + @"\images\" + imageName, true);
        }
        else
        {
            Directory.CreateDirectory(Application.streamingAssetsPath + @"\images");
            File.Copy(imagePath, Application.streamingAssetsPath + @"\images\" + imageName, true);
        }
    }
    IEnumerator GetImage(string path ,Image image)
    {
        WWW www = new WWW(@"file://" + path);
        yield return www;
        Texture2D tex2d = www.texture;
        image.sprite = Sprite.Create(tex2d, new Rect(0, 0, tex2d.width, tex2d.height), Vector3.zero);
        //todo
        //SetImgAjust();
    }


    #endregion
    
    #region 按钮部分
    //======================================  button  ===========================================
    /// <summary>
    /// 按钮编辑界面
    /// </summary>
    public GameObject ButtonEditPanelGameObject;

    public Image ButtonPreviewImage;
    public InputField ButtonWidthInputField;
    public InputField ButtonHeightInputField;
    public InputField ButtonTextInputField;
    public Button ButtonEvent;
    /// <summary>
    /// 字体选择下拉列表
    /// </summary>
    public Dropdown ButtonFontStyleDropdown;
    /// <summary>
    /// 字体大小输入框
    /// </summary>
    public InputField ButtonFontSizeInputField;
    /// <summary>
    /// 字体下拉列表
    /// </summary>
    public Dropdown ButtonFontDropdown;
    /// <summary>
    /// 显示当前颜色的图
    /// </summary>
    public Image ButtonTextColorImage;

    public void ButtonWidthInputFidleValueChanged()
    {
        _nowEditInterfaceQualityInforma.ButtonQualityInforma.ButtonWidth = float.Parse(ButtonWidthInputField.text);
        _nowEditUiGameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(float.Parse(ButtonWidthInputField.text), _nowEditUiGameObject.GetComponent<RectTransform>().sizeDelta.y);
        _nowEditInterfaceQuality.ButtonQuality.ButtonRectSizeVector2 = _nowEditUiGameObject.GetComponent<RectTransform>().sizeDelta;
    }

    public void ButtonHeightInputFieldValueChanged()
    {
        _nowEditInterfaceQualityInforma.ButtonQualityInforma.ButtonHeight = float.Parse(ButtonHeightInputField.text);
        _nowEditUiGameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(_nowEditUiGameObject.GetComponent<RectTransform>().sizeDelta.x, float.Parse(ButtonHeightInputField.text));
        _nowEditInterfaceQuality.ButtonQuality.ButtonRectSizeVector2 = _nowEditUiGameObject.GetComponent<RectTransform>().sizeDelta;
    }

    public void ButtonChoicePicture()
    {
        FileInfo fileInfo = new FileInfo(IOHelper.GetImageName());
        imagePath = fileInfo.FullName;
        imageName = fileInfo.Name;
        CopyImage();
        _nowEditInterfaceQuality.ButtonQuality.ImagePath = imagePath;
        _nowEditInterfaceQualityInforma.ButtonQualityInforma.ImageName = imageName;
        StartCoroutine(GetImage(imagePath, ButtonPreviewImage));
        StartCoroutine(GetImage(imagePath, _nowEditUiGameObject.GetComponent<Image>()));
    }

    public void ButtonChoiceEvent()
    {
        Manager.Instace.ChooseEventPanel.SetActive(true);
    }

    public void ButtonTextInputFieldVaueChanged()
    {
        _nowEditInterfaceQualityInforma.ButtonQualityInforma.ButtonText = ButtonTextInputField.text;
        _nowEditUiGameObject.GetComponentInChildren<Text>().text = ButtonTextInputField.text;
        _nowEditInterfaceQuality.ButtonQuality.ButtonText = ButtonTextInputField.text;
    }

    public void ButtonComfirmPosition()
    {
        _nowEditInterfaceQualityInforma.ButtonQualityInforma.ButtonPositionX = _nowEditUiGameObject.transform.position.x;
        _nowEditInterfaceQualityInforma.ButtonQualityInforma.ButtonPositionY = _nowEditUiGameObject.transform.position.y;
        _nowEditInterfaceQualityInforma.ButtonQualityInforma.ButtonPositionZ = _nowEditUiGameObject.transform.position.z;
        _nowEditInterfaceQuality.ButtonQuality.ButtonPositionVector3 = _nowEditUiGameObject.transform.position;
    }

    public void ButtonFontSizeInputFieldValueChanged()
    {
        _nowEditInterfaceQualityInforma.ButtonQualityInforma.TextFontSize = int.Parse(ButtonFontSizeInputField.text);
        _nowEditUiGameObject.GetComponentInChildren<Text>().fontSize = int.Parse(ButtonFontSizeInputField.text);
        _nowEditInterfaceQuality.ButtonQuality.TextFontSize = int.Parse(ButtonFontSizeInputField.text);
    }
    /// <summary>
    /// 当字体模式改变时
    /// </summary>
    public void ButtonFontStyleDropDownValueChanged()
    {
        _nowEditInterfaceQualityInforma.ButtonQualityInforma.TextFontStyle = ButtonFontStyleDropdown.value;
        _nowEditUiGameObject.GetComponentInChildren<Text>().fontStyle = GetFontStyleByInt(ButtonFontStyleDropdown.value);
        _nowEditInterfaceQuality.ButtonQuality.TextFontStyle = GetFontStyleByInt(ButtonFontStyleDropdown.value);
    }
    /// <summary>
    /// 当字体改变时
    /// </summary>
    public void ButtonFontChanged()
    {
        _nowEditInterfaceQualityInforma.ButtonQualityInforma.Font = ButtonFontDropdown.value;
        _nowEditUiGameObject.GetComponentInChildren<Text>().font = Fonts[ButtonFontDropdown.value];
        _nowEditInterfaceQuality.ButtonQuality.Font = Fonts[ButtonFontDropdown.value];
    }
    /// <summary>
    /// 设置字体颜色
    /// </summary>
    public void ButtonTextSetColor()
    {
        Manager.Instace.ColorPicker.SetActive(true);
        Manager.Instace.ColorPicker.GetComponent<ColorPickerUI>().image = ButtonTextColorImage;
        Manager.Instace.ColorPicker.GetComponent<ColorPickerUI>().SetShowInterfaceFontColor(_nowEditInterfaceQuality.ButtonQuality, _nowEditInterfaceQualityInforma.ButtonQualityInforma);
    }
    #endregion

    #region 关闭UI的部分

    public List<int> CloseIndex = new List<int>();

    public GameObject CloseUiPanelGameObject;
    /// <summary>
    /// 所有ui所在位置的父物体
    /// </summary>
    private Transform uiParentTransform;

    public GameObject CloseItemPrefabGameObject;
    /// <summary>
    /// 待关闭Ui的Item所在的父物体
    /// </summary>
    public Transform CloseItemParentTransform;
    public Transform UiParentTransform
    {
        get
        {
            if (uiParentTransform==null)
            {
                uiParentTransform = Manager.Instace.transform.FindChild("PEVRUIParent");
            }
            return uiParentTransform;
        }
    }
    /// <summary>
    /// 当关闭UI的按钮按下时
    /// </summary>
    public void OnCloseUIButtonClick()
    {
        CloseUiPanelGameObject.SetActive(true);
        ItemToClose itemToClose;
        //将已有的UI添加到条目中供用户选择关闭
        for (int i = 0; i < UiParentTransform.childCount; i++)
        {
            GameObject CloseItem = Instantiate(CloseItemPrefabGameObject);
            CloseItem.transform.SetParent(CloseItemParentTransform);
            CloseItem.transform.localScale = Vector3.one;
            
            itemToClose = CloseItem.GetComponent<ItemToClose>();
            itemToClose.SetAttribbute(UiParentTransform.GetChild(i).gameObject,i,this);
            if (CloseIndex.Count>0)
            {
                for (int j = 0; j < CloseIndex.Count; j++)
                {
                    if (CloseIndex[j]==i)
                    {
                        itemToClose.SetToggleValue(true);
                        break;
                    }
                }
            }
        }
    }

    public void ReturnMainPanel()
    {
        CloseUiPanelGameObject.SetActive(false);
    }

    /// <summary>
    /// 添加或移除
    /// </summary>
    public void AddOrDetach(int index,bool add)
    {
        if (add)
        {
            if (CloseIndex.Contains(index))
            {
                return;
            }
            _showInterfaceInforma.CloseIndexList.Add(index);
        }
        else
        {
            if (!CloseIndex.Contains(index))
            {
                return;
            }
            _showInterfaceInforma.CloseIndexList.Remove(index);
        }
        _showInterface.CloseIndexList = _showInterfaceInforma.CloseIndexList;
        //Debug.LogError(_showInterfaceInforma.CloseIndexList.Count);
    }
    
    #endregion
}


public enum PEVRUI
{
    Text,Image,Button,Toggle
}

