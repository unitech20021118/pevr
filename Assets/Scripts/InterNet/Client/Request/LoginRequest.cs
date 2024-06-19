using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
public class LoginRequest : BaseRequest {
    private LoginPanel loginPanel;
    private bool islogin;
    UserData userdata;
	// Use this for initialization
    public override void Awake()
    {
        requestCode = RequestCode.User;
        actionCode = ActionCode.Login;
        loginPanel = GetComponent<LoginPanel>();
        base.Awake();
    }

    void Update()
    {
        if (islogin)
        {
            islogin = false;
            manager.SetUserData(userdata);
        }
    }

    public void SendRequest(string username, string password)
    {
        string data = username + "," + password;
        base.SendRequest(data);
    }

    public override void OnResponse(string data)
    {
        string[] strs = data.Split(',');
        ReturnCode returnCode = (ReturnCode)int.Parse(strs[0]);
        loginPanel.OnLoginResponse(returnCode);
        if (returnCode == ReturnCode.Success)
        {
            string username = strs[1];
            int totalCount = int.Parse(strs[2]);
            int winCount = int.Parse(strs[3]);
            userdata = new UserData(username, totalCount, winCount);
            islogin = true;
        }
        
    }
}
