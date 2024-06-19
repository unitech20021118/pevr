using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
public class ListRoomRequest : BaseRequest{
    List<UserData> udList;
    bool Roomhas = false;
    Loader loader;
    public override void Awake()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.ListRoom;
        base.Awake();
    }

    void Update()
    {
        if (Roomhas)
        {
            Roomhas = false;
            loader=GetComponent<Loader>();
            foreach (UserData s in udList)
            {
                loader.CreateRoomBtn(s.Task, s.Username);
            }
        }
    }

    public override void SendRequest()
    {
        base.SendRequest("r");
    }

    public override void OnResponse(string data)
    {
        
        udList = new List<UserData>();
        if (data != "0")
        {
            string[] udArray = data.Split('|');
            foreach (string ud in udArray)
            {
                string[] strs = ud.Split(',');
                udList.Add(new UserData(int.Parse(strs[0]), strs[1], int.Parse(strs[2]), int.Parse(strs[3]),strs[4]));
            }
            Roomhas = true;
        }


    }
}
