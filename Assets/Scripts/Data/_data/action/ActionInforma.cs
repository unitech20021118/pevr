using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ActionInforma  {

    public bool isOnce;
    public string name;
    public string eventName;
    public string ip;
    public float durtime;
    public ActionInforma()
    {

    }

    public ActionInforma(bool isOnce)
    {
        this.isOnce = isOnce;
    }

}
