using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
public class RegisterRequest :BaseRequest {
    private LoginPanel loginPanel;
    public override void Awake()
    {
        requestCode = RequestCode.User;
        actionCode = ActionCode.Register;
        loginPanel=GetComponent<LoginPanel>();
        base.Awake();
    }

    public void SendRequest(string username, string password)
    {
        string data = username + "," + password;
        base.SendRequest(data);
    }

    public override void OnResponse(string data)
    {
        ReturnCode returnCode = (ReturnCode)int.Parse(data);
        loginPanel.OnLoginResponse(returnCode);
    }
}
