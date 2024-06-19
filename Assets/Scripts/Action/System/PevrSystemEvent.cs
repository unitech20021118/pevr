using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PevrSystemEvent : MonoBehaviour {
    private Button btn;
    private EventsType eventType;
    void Start()
    {
        eventType = EventsType.SystemEvents;
        btn = transform.GetComponent<Button>();
        btn.onClick.AddListener(delegate () { AddEvent(GetComponentInChildren<Text>().text, eventType); });
    }

    void AddEvent(string btnName, EventsType eventType)
    {
        Manager.Instace.Menu.GetComponent<Menu>().AddEvent(GetComponentInChildren<Text>().text, eventType);
    }
}
