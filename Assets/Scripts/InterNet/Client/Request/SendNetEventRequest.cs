using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
public class SendNetEventRequest :BaseRequest {
    int id;
    bool IsFromNetEvent = false;
    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.SendNetEvent;
        base.Awake();
    }

    void Update()
    {
        if (IsFromNetEvent)
        {
            IsFromNetEvent = false;
            Transform t= manager.playerMng.transformDict[id.ToString()];
            print(t.gameObject.name);
            t.GetComponent<EventNode>().even.remoteHappened = true;
            t.GetComponent<EventNode>().even.Do();
        }
    }



    public void SendRequest(string id)
    {
        string data = id.ToString();
        base.SendRequest(data);
    }

    public override void OnResponse(string data)
    {
        Debug.Log("sendNetEvent");
        id = int.Parse(data);
        IsFromNetEvent = true;
        base.OnResponse(data);
    }
}
