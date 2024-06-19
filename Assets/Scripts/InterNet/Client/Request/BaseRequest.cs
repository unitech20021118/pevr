using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
public class BaseRequest : MonoBehaviour {
    protected RequestCode requestCode = RequestCode.None;
    protected ActionCode actionCode = ActionCode.None;
    protected Manager manager
    {
        get { return Manager.Instace; }
    }
    public virtual void Awake() {
        Manager.Instace.AddRequest(actionCode, this);

    }

    protected  void SendRequest(string data) {
        manager.SendRequest(requestCode, actionCode, data);
    }

    public virtual void SendRequest() { }

    public virtual void OnResponse(string data) { }

    public virtual void OnDestroy()
    {
        Manager.Instace.RemoveRequest(actionCode);
    }
	
}
