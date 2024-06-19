using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MouseDownEditor : MonoBehaviour {
    private Button btn;
    public EventsType eventType;
	// Use this for initialization
	void Start () {
        btn = transform.GetComponent<Button>();
        btn.onClick.AddListener(delegate() { AddEvent(GetComponentInChildren<Text>().text, eventType); });
	}

    void AddEvent(string btnName, EventsType eventType)
    {
        foreach (Events e in Manager.Instace.eventlist)
        {
            if (e.name == btnName)
            {
                if (Manager.Instace.ChooseEventPanel.GetComponent<CurrentEditorActon>().editorAction.name=="ShowInterface(Clone)")
                {
                    ShowInterfaceUI showInterfaceUi = Manager.Instace.ChooseEventPanel.GetComponent<CurrentEditorActon>().editorAction.GetComponent<ShowInterfaceUI>();
                    showInterfaceUi.NowEditInterfaceQualityInforma.ButtonQualityInforma.EventName = btnName;
                    showInterfaceUi.NowEditInterfaceQuality.ButtonQuality.Events = e;
                    showInterfaceUi.NowEditInterfaceQuality.ButtonQuality.EventName = btnName;
                    showInterfaceUi.gameObject.transform.FindChild("ButtonEdit/SendEventButton")
                        .GetComponentInChildren<Text>().text = btnName;

                }
                else
                {
                    GameObject currentActionPanel = Manager.Instace.ChooseEventPanel.GetComponent<CurrentEditorActon>().editorAction;
                    //currentActionPanel.GetComponent<ActionUI>().action.even = e;
                    Action<Main> a = currentActionPanel.GetComponent<ActionUI>().action;
                    ActionInforma actionInfoma = currentActionPanel.GetComponent<ActionUI>().actionInforma;
                    a.even = e;
                    actionInfoma.eventName = btnName;
                    currentActionPanel.transform.FindChild("ChooseEvent").GetComponentInChildren<Text>().text = btnName;
                }
            }
        }
        Manager.Instace.ChooseEventPanel.SetActive(false);
        Manager.Instace.CustomEventlistUI2.SetActive(false);
    }
}
