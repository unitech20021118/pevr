using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EventEditorBtn : MonoBehaviour {
    public Events currentEvent;
    public Text text;
    public Toggle toggle;
    public Button deleteBtn;
    List<GameObject> objs;
	// Use this for initialization
	void Start () {
        //currentEvent = new Events("", EventsType.CustomEvents);
        //currentEvent.name = text.text;
        //Manager.Instace.eventlist.Add(currentEvent);
        ////将自定义事件添加到Manager中
        //Manager.Instace.listTree.Root.data.customEvent.Add(text.text);
	}

    public void InitialEvent()
    {
        currentEvent = new Events("", EventsType.CustomEvents);
        currentEvent.name = text.text;
        Manager.Instace.eventlist.Add(currentEvent);
        //将自定义事件添加到Manager中
        Manager.Instace.allDataInformation.listTree.Root.data.customEvent.Add(text.text);
    }

    public void InitialEvent(string str)
    {
        currentEvent = new Events(str, EventsType.CustomEvents);
        Manager.Instace.eventlist.Add(currentEvent);
    }

    public void  Setobjs(GameObject obj)
    {
        if (objs == null)
        {
            objs = new List<GameObject>();
        }  
        objs.Add(obj);
    }
    public void DeleteEvent()
    {
        Manager.Instace.eventlist.Remove(currentEvent);
        Manager.Instace.allDataInformation.listTree.Root.data.customEvent.Remove(currentEvent.name);
        Destroy(gameObject);
        foreach (GameObject obj in objs)
        {
            Destroy(obj);
        }
    }
}
