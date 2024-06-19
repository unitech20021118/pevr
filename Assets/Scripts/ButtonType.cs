using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonType : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static ButtonType lastBtn;
    public GameObject panel;
    public Sprite state, mouse;
    public bool isLast;
     Text childtext;
    /// <summary>
    /// 显示子菜单
    /// </summary>

    void Awake()
    {
        childtext = transform.FindChild("Text").GetComponent<Text>();
        childtext.color = Color.black;
        childtext.fontStyle = FontStyle.Bold;
        gameObject.GetComponent<Image>().sprite = state;
    }
    public void ShowPanel()
    {
        if (lastBtn == null)
        {
            if (panel != null)
            {
                panel.SetActive(true);
            }
        }
        else
        {
            if (lastBtn != this)
            {
                if (lastBtn.panel != null)
                {
                    lastBtn.panel.SetActive(false);
                }
            }
            gameObject.GetComponent<Image>().sprite = mouse;
            childtext.color = Color.white;
            if (panel != null)
            {
                panel.SetActive(true);
                panel.transform.position = transform.position;
            }
        }
        lastBtn = this;
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isLast) {
            ShowPanel();
        }
        else
        {
            gameObject.GetComponent<Image>().sprite = mouse;
            childtext.color = Color.white;
        }

    }
    public void OnPointerExit(PointerEventData eventData)
    {
        gameObject.GetComponent<Image>().sprite = state;
        childtext.color = Color.black;
    }
}
    

