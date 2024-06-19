using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
[System.Serializable]
public class EventInfo:Base {
    public EventsType eventType;
	public int keyid;
    //public Events events;

    public EventInfo(string name, EventsType even)
    {
        id++;
        index = id;
        this.name = name;
        eventType = even;
    }

	public EventInfo(string name, EventsType even,int keyID)
	{
		id++;
		index = id;
		this.name = name;
		eventType = even;
		keyid = keyID;
	}

    public override void GetTransform()
    {
        //存在问题
        //Transform t = GameObjectIndex.GameObjectList[index].transform;
        Transform t = Manager.Instace.dictFromObjectToInforma.FirstOrDefault(q => q.Value == this).Key.transform;
        pos = t.localPosition.ToString();
        scale = t.localScale.ToString();
        rotate = t.eulerAngles.ToString();
        t.GetComponent<EventNode>().name = name;       
        isActive = t.gameObject.activeSelf;
    }
}
