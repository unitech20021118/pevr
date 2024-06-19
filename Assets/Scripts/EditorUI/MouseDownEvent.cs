using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MouseDownEvent : MonoBehaviour {
    private Button btn;
    public EventsType eventType;
	// Use this for initialization
	void Start () {
        btn = transform.GetComponent<Button>();
        btn.onClick.AddListener(delegate() { AddEvent(GetComponentInChildren<Text>().text,eventType); });
	}

    void AddEvent(string btnName, EventsType eventType)
    {
        //if (!Manager.Instace.isChooseEvent)
        //{
            Manager.Instace.Menu.GetComponent<Menu>().AddEvent(GetComponentInChildren<Text>().text, eventType);
        //}
        //else
        //{//在选择状态时
        //    foreach (Events e in Manager.Instace.eventlist)
        //    {
        //        if (e.name == btnName)
        //        {
        //            GameObject currentActionPanel = Manager.Instace.ChooseEventPanel.GetComponent<CurrentEditorActon>().editorAction;
        //            //currentActionPanel.GetComponent<ActionUI>().action.even = e;
        //            Action<Main> a = currentActionPanel.GetComponent<ActionUI>().action;
        //            a.even = e;
        //            currentActionPanel.transform.FindChild("ChooseEvent").GetComponentInChildren<Text>().text = btnName;

        //        }

        //    }
        //    Manager.Instace.ChooseEventPanel.SetActive(false);
        //    Manager.Instace.CustomEventlistUI2.SetActive(false);
        //}
        //Destroy(gameObject);
    }
}
