using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 挂载在UI的item上的脚本
/// </summary>
public class ItemUI : MonoBehaviour
{
    /// <summary>
    /// UI名
    /// </summary>
    public Text UiNameText;
    /// <summary>
    /// UI本体
    /// </summary>
    public GameObject uiBodyGameObject;
    /// <summary>
    /// 选中的ui
    /// </summary>
    public static ItemUI ChoiceItemUi;

    private Image buttonImage;
    /// <summary>
    /// 拖动UI移动位置的脚本
    /// </summary>
    private DragableUI dragableUi;

    public InterfaceQualityInforma InterfaceQualityInforma;
    public InterfaceQuality InterfaceQuality;

    public Image ButtonImage
    {
        get
        {
            if (buttonImage==null)
            {
                buttonImage = gameObject.GetComponent<Image>();
            }
            return buttonImage;
        }
    }

    // Use this for initialization
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 创建时设置物体同时选中该物体
    /// </summary>
    public void SetUIBody(GameObject go,string uiName)
    {
        uiBodyGameObject = go;
        UiNameText.text = uiName;
        dragableUi = uiBodyGameObject.AddComponent<DragableUI>();
        ItemClick();
    }

    /// <summary>
    /// 点击同时索引到实际UI物体
    /// </summary>
    public void ItemClick()
    {
        if (ChoiceItemUi != null && ChoiceItemUi != this)
        {
            dragableUi.enabled = false;
            ChoiceItemUi.uiBodyGameObject.transform.FindChild("FrameImage(Clone)").gameObject.SetActive(false);
            ChoiceItemUi.ButtonImage.color=new Color32(255,255,255,255);
        }
        ChoiceItemUi = this;
        uiBodyGameObject.transform.FindChild("FrameImage(Clone)").gameObject.SetActive(true);
        ButtonImage.color = new Color32(0, 155, 255, 255);
        dragableUi.enabled = true;
    }
}
