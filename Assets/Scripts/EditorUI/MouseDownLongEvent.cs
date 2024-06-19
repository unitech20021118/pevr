using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseDownLongEvent : MonoBehaviour
{
    private Button btn;
    public EventsType eventType;
    // Use this for initialization
    void Start()
    {
        btn = transform.GetComponent<Button>();
        btn.onClick.AddListener(delegate () { AddEvent(eventType); });
    }

    void AddEvent(EventsType eventType)
    {
        Manager.Instace.Menu.GetComponent<Menu>().AddEvent(GetComponentInChildren<Text>().text, eventType);
    }
}
