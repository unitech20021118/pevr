using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
public class QuitRoomRequest : BaseRequest {

    public override void Awake()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.QuitRoom;
        base.Awake();
    }

    public override void SendRequest()
    {
        base.SendRequest("r");
    }

    public override void OnResponse(string data)
    {
        base.OnResponse(data);
    }
}
