using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
public class VRMoveRequest :BaseRequest {
    //edit by 王梓亦
    private Transform localPlayerTransform;
    //public PlayerMove locakPlayerMove;
    private int synRate = 20;
    private Vector3 pos;
    private Vector3 rote;
    private string id;
    private Transform remotePlayerTransform;
    private bool isSyncRemotePlayer;
    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.Move;
        base.Awake();
    }


    private void Start()
    {
        InvokeRepeating("SyncLocalPlayer", 0, 1f / synRate);
    }

    private void Update()
    {
        if (isSyncRemotePlayer == true)
        {
            SyncRemotePlayer();
            isSyncRemotePlayer = false;
        }

    }

    public void SetLocalPlayer(Transform localPlayerTransform)
    {
        this.localPlayerTransform = localPlayerTransform;
    }

    public void SetRemotePlayer(Transform remotePlayerTransform)
    {
        this.remotePlayerTransform = remotePlayerTransform;
    }

    private void SyncLocalPlayer()
    {
        //同步VR相机的位置，模型的ID
        Transform trans = Manager.Instace.VRCamera.transform;
        SendRequest(trans.position.x, trans.position.y, trans.position.z,
                    trans.eulerAngles.x, trans.eulerAngles.y, trans.eulerAngles.z,
                    localPlayerTransform.GetComponent<GameObjectId>().TransformName);
    }

    private void SyncRemotePlayer()
    {
        //if (remotePlayerTransform != null)
        //{
        //    remotePlayerTransform.position = pos;
        //    remotePlayerTransform.eulerAngles = rote;
        //}
       //模型跟随vr相机
       Transform obj= manager.playerMng.transformDict[id.ToString()];
       obj.position = pos;
       obj.eulerAngles = rote;               
    }

    public void SendRequest(float x,float y,float z,float rotationX,float rotationY,float rotationZ,string id)
    {
        string data = string.Format("{0},{1},{2}|{3},{4},{5}|{6}", x, y, z, rotationX, rotationY, rotationZ,id);
        base.SendRequest(data);
    }

    public override void OnResponse(string data)
    {
        string[] strs = data.Split('|');
        pos = Format(strs[0]);
        rote = Format(strs[1]);
        //通过ID找到remotePlayer
        //manager.SetRemoteRole(int.Parse(strs[2]));
        //id = FormatToInt(strs[2]);
        id = strs[2];
        isSyncRemotePlayer = true;
        base.OnResponse(data);
    }

    public Vector3 Format(string data)
    {
        string[] strs = data.Split(',');
        float x =float.Parse(strs[0]);
        float y = float.Parse(strs[1]);
        float z = float.Parse(strs[2]);
        return new Vector3(x, y, z);
    }

    public int FormatToInt(string data)
    {
        return int.Parse(data);
    }
}
