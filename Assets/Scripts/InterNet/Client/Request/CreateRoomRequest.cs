using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
public class CreateRoomRequest : BaseRequest {

    public override void Awake()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.CreateRoom;
        base.Awake();
    }

    public override void SendRequest()
    {
        base.SendRequest("r");
    }

    public override void OnResponse(string data)
    {
        string[] strs = data.Split(',');
        ReturnCode returnCode = (ReturnCode)int.Parse(strs[0]);
        //RoleType roleType = (RoleType)int.Parse(strs[1]);
        if (returnCode == ReturnCode.Success)
        {
            Debug.Log("房间创建成功");
        }
    }
}
