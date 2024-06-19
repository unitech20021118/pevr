using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using UnityEngine.SceneManagement;
public class StartGameRequest : BaseRequest{
    private bool BeginGame;
    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.StartGame;
        base.Awake();
    }

    void Update()
    {
        if (BeginGame)
        {
            BeginGame = false;
            manager.clientMng.OnDestroy();
            SceneManager.LoadScene(1);
            
        }
    }

    public void SendRequest()
    {
        base.SendRequest("r");
    }

    public override void OnResponse(string data)
    {
        int num = int.Parse(data);
        ReturnCode returnCode = (ReturnCode)num;
        if (returnCode == ReturnCode.Success)
        {
            BeginGame = true;
        }
        base.OnResponse(data);
    }
}
