using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseExitEvent : MonoBehaviour {

    private Button btn;
    public EventsType eventType;
    // Use this for initialization
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
