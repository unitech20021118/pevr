using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EventsPanelUI : MonoBehaviour {
    public InputField eventName;
    public GameObject text;
    public GameObject eventList;

	// Use this for initialization
	void Start () {
        eventName.onValueChanged.AddListener(delegate{ ValueChangeCheck(); });

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddNewEvent()
    {
        if (!text.activeInHierarchy&&!string.IsNullOrEmpty(eventName.text))
        {
            
            GameObject obj = Instantiate(Manager.Instace.prefab11);
			obj.transform.SetParent(eventList.transform.Find(@"Scroll View/Viewport/Content"));
            obj.transform.localScale = Vector3.one;
            obj.transform.localPosition = new Vector3(obj.transform.position.x, obj.transform.position.y,0f);
            obj.GetComponentInChildren<Text>().text = eventName.text;
            obj.GetComponent<EventEditorBtn>().InitialEvent();
            
            //在自定义事件中也添加
            GameObject obj2 = Instantiate(Manager.Instace.prefab22);
            obj2.GetComponentInChildren<Text>().text = eventName.text;
			obj2.transform.SetParent(Manager.Instace.CustomEventlistUI.transform.Find(@"Scroll View/Viewport/Content"));
            obj2.transform.localScale = Vector3.one;
            obj2.transform.localPosition = new Vector3(obj.transform.position.x, obj.transform.position.y, 0f);
            obj.GetComponent<EventEditorBtn>().Setobjs(obj2);

            GameObject obj3 = Instantiate(Manager.Instace.prefab33);
            obj3.GetComponentInChildren<Text>().text = eventName.text;
			obj3.transform.SetParent(Manager.Instace.CustomEventlistUI2.transform.Find(@"Scroll View/Viewport/Content"));
            obj3.transform.localScale = Vector3.one;
            obj3.transform.localPosition = new Vector3(obj.transform.position.x, obj.transform.position.y, 0f);
            eventName.text = null;
            obj.GetComponent<EventEditorBtn>().Setobjs(obj3);
        }
        
    }

    /// <summary>
    /// 检测有没有存在过列表内
    /// </summary>
    public void ValueChangeCheck()
    {
        foreach (Events s in Manager.Instace.eventlist)
        {
            if (s.name == eventName.text)
            {
                text.SetActive(true);
            }
            else
            {
                text.SetActive(false);
            }
        }
        
    }
}
