using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BtcColor : MonoBehaviour, IPointerEnterHandler
{
    public static BtcColor lastbtcColor;
    public EventsType eventType;
    public void ShowColor()
    {
        GetComponent<Image>().color = new Color(1, 1, 1, 1);
        lastbtcColor = this;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowColor();
    }
	
    /// <summary>
    /// 添加事件
    /// </summary>
    /// <param name="btnName"></param>
    public void  AddEvent(string btnName,EventsType eveTyp)
    {
        Manager.Instace.Menu.GetComponent<Menu>().AddEvent(btnName,eveTyp);
    }
}
