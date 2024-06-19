using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
public class JoinRoomRequest : BaseRequest {
    List<UserData> listUserData;
    private bool isJoined;
    public override void Awake()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.JoinRoom;
        base.Awake();
    }


    public void SendRequest(string  roomowner)
    {
        base.SendRequest(roomowner);
    }

    void Update()
    {
        if (isJoined)
        {
            isJoined = false;
            Loader.Instance.UpdateRoom(listUserData);
            Loader.Instance.taskStartPanel.SetActive(true);
        }
    }

    public override void OnResponse(string data)
    {

        string[] strs = data.Split('-');
        string[] strs1 = strs[0].Split(',');
        string[] strs2 = strs[1].Split('|');
        ReturnCode returnCode = (ReturnCode)int.Parse(strs1[0]);
        RoleType roleType = (RoleType)int.Parse(strs1[1]);
        listUserData = new List<UserData>();
        if (returnCode == ReturnCode.Success)
        {
            Debug.Log("加入成功");
            //string[] strs3 = strs[1].Split(',');
            //ud1 = new UserData(strs3[0], strs3[1], strs3[2], strs3[3],strs3[4]);
            foreach (string s in strs2)
            {
                string[] temp = s.Split(',');
                UserData u = new UserData(int.Parse(temp[0]), temp[1], int.Parse(temp[2]), int.Parse(temp[3]), temp[4]);
                listUserData.Add(u);
            }
            isJoined = true;

        }
    }


    //public void SendRequest(int id)
    //{
    //    base.SendRequest(id.ToString());
    //}

    //public override void OnResponse(string data)
    //{

    //    string[] strs = data.Split('-');
    //    string[] strs2 = strs[1].Split(',');
    //    ReturnCode returnCode = (ReturnCode)int.Parse(strs[0]);
    //    RoleType roleType = (RoleType)int.Parse(strs2[0]);
    //    UserData ud1 = null;
    //    if (returnCode == ReturnCode.Success)
    //    {
    //        Debug.Log("加入成功");
    //    }
    //}
}
