using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
public class CreateRoom :Action<Main>{

    public override void DoAction(Main m)
    {
        Manager.Instace.clientMng.OnInit();
        CreateOrJoinRoomRequest createRoomRequest = m.gameObject.AddComponent<CreateOrJoinRoomRequest>();
        createRoomRequest.SendRequest();
    }

        //PhotonNetwork.ConnectToMaster("192.168.1.4", 5055, "84d6e57f-4ca3-4b43-b11a-293bb2b18184", "1.0");

        //PhotonNetwork.playerName = "app";
        //PhotonNetwork.CreateRoom("apple", new RoomOptions() { MaxPlayers = 10 }, null);
        //m.GetComponent<PhotonView>().viewID = PhotonNetwork.AllocateViewID();
}
