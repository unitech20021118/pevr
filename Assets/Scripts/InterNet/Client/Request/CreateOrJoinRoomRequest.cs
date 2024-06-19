using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using System.Net.Sockets;
public class CreateOrJoinRoomRequest :BaseRequest{

    public override void Awake()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.CreateOrJoinRoom;
        base.Awake();
    }

    public void SendRequest()
    {
        base.SendRequest("r");
        
    }

    public override void OnResponse(string data)
    {
        string[] strs = data.Split(',');
        ReturnCode returnCode = (ReturnCode)int.Parse(strs[0]);
        //RoleType roleType = (RoleType)int.Parse(strs[1]);
        //manager.SetCurrentRoleType(roleType);
        //int num = int.Parse(strs[1]);
        //manager.SetCurrentRoleType(num);
    }

}
