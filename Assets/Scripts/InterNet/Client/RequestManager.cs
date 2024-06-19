using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
public class RequestManager : BaseManager {

    private Dictionary<ActionCode, BaseRequest> requestDict = new Dictionary<ActionCode, BaseRequest>();

    public void AddrequestCode(ActionCode actionCode, BaseRequest baseRequest)
    {
        requestDict.Add(actionCode, baseRequest);
    }

    public void RemoveactionCode(ActionCode actionCode)
    {
        requestDict.Remove(actionCode);
    }

    public void HandleResponse(ActionCode actionCode, string data)
    {
        BaseRequest request = requestDict.TryGet<ActionCode, BaseRequest>(actionCode);
        if (request == null)
        {
            Debug.Log("无法得到"); return;
        }
        request.OnResponse(data);
    }
}
