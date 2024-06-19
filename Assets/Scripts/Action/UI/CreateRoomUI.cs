using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CreateRoomUI :ActionUI {
    CreateRoom createRoom;
    public InputField IpInputField;
    public override Action<Main> CreateAction()
    {
        action = new CreateRoom();
        createRoom = (CreateRoom)action;
        actionInforma = new ActionInforma(true);
        GetStateInfo().actionList.Add(actionInforma);
        
        actionInforma.name = "CreateRoom";
        createRoom.SetSituation();
        IpInputField.onValueChanged.AddListener(delegate(string s) { IpChange(); });
        return base.CreateAction();
    }

    void IpChange()
    {
        Manager.Instace.clientMng.IP = IpInputField.text;
        actionInforma.ip = IpInputField.text;
    }

    public override Action<Main> LoadAction(ActionInforma actionInforma)
    {
        this.actionInforma = actionInforma;
        action = new CreateRoom();
        createRoom = (CreateRoom)action;
        createRoom.SetSituation();
        IpInputField.text = actionInforma.ip;
        Manager.Instace.clientMng.IP = IpInputField.text;
        IpInputField.onValueChanged.AddListener(delegate(string s) { IpChange(); });
        return base.LoadAction(actionInforma);
    }

    //public override Action<Main> LoadAction(ActionInforma actionInforma)
    //{
        
    //}
	
}
