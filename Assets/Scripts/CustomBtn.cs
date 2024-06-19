using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CustomBtn : MonoBehaviour {
    
    public GameObject prefab;
    public Transform customEventlist;
    void Start()
    {
        prefab = (GameObject)Resources.Load("Prefabs/EventBtn");
        ShowCustomEvent();
    }

    public void ShowCustomEvent()
    {
        //foreach (Events e in Manager.Instace.eventlist)
        //{
        //    GameObject obj = Instantiate(prefab);
        //    obj.GetComponentInChildren<Text>().text = e.name;
        //    obj.transform.SetParent(customEventlist);
        //    obj.transform.localScale = Vector3.one;
        //    obj.transform.localPosition = new Vector3(obj.transform.position.x, obj.transform.position.y, 0f);
        //}
    }
}
