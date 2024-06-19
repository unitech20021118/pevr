using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemToClose : MonoBehaviour
{
    private ShowInterfaceUI showInterfaceUi;
    /// <summary>
    /// item对应的UI物体
    /// </summary>
    private GameObject uiGameObject;
    /// <summary>
    /// 所在位置索引
    /// </summary>
    private int index;
    /// <summary>
    /// 是否正在显示UI物体
    /// </summary>
    private bool showing;
    /// <summary>
    /// 用于显示选中效果的image
    /// </summary>
    private Image itemImage;
    /// <summary>
    /// 是否隐藏的选框
    /// </summary>
    private Toggle closeToggle;

    public Toggle CloseToggle
    {
        get
        {
            if (closeToggle==null)
            {
                closeToggle = transform.FindChild("Toggle").GetComponent<Toggle>();
            }
            return closeToggle;
        }
    }

    public Image ItemImage
    {
        get
        {
            if (itemImage==null)
            {
                itemImage = transform.GetComponent<Image>();
            }
            return itemImage;
        }
    }

    public void SetAttribbute(GameObject go,int index,ShowInterfaceUI showInterfaceUi)
    {
        uiGameObject = go;
        this.showInterfaceUi = showInterfaceUi;
        this.index = index;
    }


    public void OnToggleValueChanged()
    {
        if (CloseToggle.isOn)
        {
            //添加到 showinterfaceUI 中
            showInterfaceUi.AddOrDetach(index, true);
        }
        else
        {
            //从 showinterfaceUI 中移除
            showInterfaceUi.AddOrDetach(index, false);
        }
    }

    public void SetToggleValue(bool bl)
    {
        CloseToggle.isOn = bl;
    }

    /// <summary>
    /// 当item被点击时显示或隐藏关联的UI物体
    /// </summary>
    public void OnItemClick()
    {
        //显示或隐藏物体
        if (showing)
        {
            ItemImage.color = new Color32(255, 255, 255, 255);
            uiGameObject.SetActive(false);
            uiGameObject.transform.FindChild("FrameImage(Clone)").gameObject.SetActive(false);
            showing = false;
        }
        else
        {
            ItemImage.color = new Color32(0,180,255,255);
            uiGameObject.SetActive(true);
            uiGameObject.transform.FindChild("FrameImage(Clone)").gameObject.SetActive(true);
            showing = true;
        }
    }
}
