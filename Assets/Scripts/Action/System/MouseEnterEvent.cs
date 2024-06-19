using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 鼠标进入事件
/// </summary>
public class MouseEnterEvent : MonoBehaviour
{
    private Button btn;
    public EventsType eventType;
    void Start()
    {
        btn = transform.GetComponent<Button>();
        btn.onClick.AddListener(delegate () { AddEvent(GetComponentInChildren<Text>().text, eventType); });
    }

    void AddEvent(string btnName, EventsType eventType)
    {
        Manager.Instace.Menu.GetComponent<Menu>().AddEvent(GetComponentInChildren<Text>().text, eventType);
    }
}
