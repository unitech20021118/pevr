using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
public class ChooseSceneRequest :BaseRequest{

    public override void Awake()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.CreateRoom;
        base.Awake();
    }

    public void SendRequest(string data)
    {
        base.SendRequest(data);
    }

    public override void OnResponse(string data)
    {
        string[] strs = data.Split(',');
        ReturnCode returnCode = (ReturnCode)int.Parse(strs[0]);
        //RoleType roleType = (RoleType)int.Parse(strs[1]);
        string s = strs[1];
        if (returnCode == ReturnCode.Success)
        {
            Debug.Log("房间创建成功"+s);
        }
    }
}
