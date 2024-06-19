using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
public class UpdateRoomRequest : BaseRequest {
    List<UserData> listUserData;
    bool isUpdate;
    public override void Awake()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.UpdateRoom;
        base.Awake();
    }

    void Update()
    {
        if (isUpdate)
        {
            isUpdate = false;
            Loader.Instance.UpdateRoom(listUserData);
        }
        
    }

    public override void OnResponse(string data)
    {
        Debug.Log("更新房间");
        string[] strs2 = data.Split('|');
        listUserData = new List<UserData>();

            //string[] strs3 = strs[1].Split(',');
            //ud1 = new UserData(strs3[0], strs3[1], strs3[2], strs3[3],strs3[4]);
            foreach (string s in strs2)
            {
                string[] temp = s.Split(',');
                UserData u = new UserData(int.Parse(temp[0]), temp[1], int.Parse(temp[2]), int.Parse(temp[3]), temp[4]);
                listUserData.Add(u);
            }
            isUpdate = true;
        
    }
}
