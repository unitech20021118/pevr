using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using UnityStandardAssets.Characters.ThirdPerson;
public class AnimatorRequest : BaseRequest
{
    //edit by 王梓亦
    private Transform localPlayerTransform;
    //public PlayerMove locakPlayerMove;
    private int synRate = 20;
    private bool isloop;
    private string playname;
    private string trigger;
    private bool isplay;
    private string id;

    private Vector3 movev3;
    private bool iscrouch;
    private bool isjump;

    private Vector3 pos;
    private Vector3 rote;

    private Transform remotePlayerTransform;
    private bool isSyncRemotePlayer;

    public bool isnet;
    public bool isvr;

    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.Move;
        base.Awake();
    }


    private void Start()
    {
        //InvokeRepeating("SyncLocalPlayer", 0, 1f / synRate);
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
        //id = localPlayerTransform.GetComponent<GameObjectId>().TransformName;//获取id
    }

    public void SetRemotePlayer(Transform remotePlayerTransform)
    {
        this.remotePlayerTransform = remotePlayerTransform;
    }

    private void SyncLocalPlayer()
    {
        //if(isvr)
        //{
        //    SendRequest(pos, rote, movev3, iscrouch, isjump, localPlayerTransform.GetComponent<GameObjectId>().TransformName);
        //}
        //else
        //{
        //    Transform trans = Manager.Instace.VRCamera.transform;
        //    SendRequest(trans.position.x, trans.position.y, trans.position.z,
        //                trans.eulerAngles.x, trans.eulerAngles.y, trans.eulerAngles.z,
        //                localPlayerTransform.GetComponent<GameObjectId>().TransformName);
        //}
        
    }

    private void SyncRemotePlayer()
    {
        //if (remotePlayerTransform != null)
        //{
        //    remotePlayerTransform.position = pos;
        //    remotePlayerTransform.eulerAngles = rote;
        //}
        Transform obj = manager.playerMng.transformDict[id.ToString()];
        obj.position = pos;
        obj.eulerAngles = rote;
        Animator ani = obj.GetComponent<Animator>();
        ThirdPersonCharacter tpc = obj.GetComponent<ThirdPersonCharacter>();
        if (isnet)
        {
            //同步动作
            tpc.Move(movev3,iscrouch,isjump);
        }
        else
        {
            if (isloop)
            {
                ani.SetBool(playname, isplay);
            }
            else
            {
                ani.SetTrigger(trigger);
            }
        }
    }

    public void SendRequest(bool loop, string pname, bool play, string tri, string id)
    {
        string data = string.Format("{0};{1};{2};{3};{4}", loop, pname, play, tri, id);
        base.SendRequest(data);
    }

    public void SendRequest(Vector3 p, Vector3 r, Vector3 move ,bool crouch, bool jump, string id)
    {
        string data = string.Format("{0};{1};{2};{3};{4};{5}", p, r, move, crouch, jump, id);
        base.SendRequest(data);
    }

    public void SendRequest(float x, float y, float z, float rotationX, float rotationY, float rotationZ, string id)
    {
        string data = string.Format("{0},{1},{2}|{3},{4},{5}|{6}", x, y, z, rotationX, rotationY, rotationZ, id);
        base.SendRequest(data);
    }

    public override void OnResponse(string data)
    {
        if (isnet)
        {
            string[] strs = data.Split(';');
            pos = Str2Vector3(strs[0]);
            rote = Str2Vector3(strs[1]);
            movev3 = Str2Vector3(strs[2]);
            iscrouch = bool.Parse(strs[3]);
            isjump = bool.Parse(strs[4]);
            id = strs[5];
            isSyncRemotePlayer = true;
            base.OnResponse(data);
        }
        else
        {
            string[] strs = data.Split(';');
            isloop = bool.Parse(strs[0]);
            playname = strs[1];
            isplay = bool.Parse(strs[2]);
            trigger = strs[3];
            id = strs[4];
            
        }
    }

    public Vector3 Format(string data)
    {
        string[] strs = data.Split(',');
        float x = float.Parse(strs[0]);
        float y = float.Parse(strs[1]);
        float z = float.Parse(strs[2]);
        return new Vector3(x, y, z);
    }

    public Vector3 Str2Vector3(string data)
    {
        string[] strs = data.Replace("(", "").Replace(")", "").Split(',');
        float x = float.Parse(strs[0]);
        float y = float.Parse(strs[1]);
        float z = float.Parse(strs[2]);
        return new Vector3(x, y, z);
    }

    public int FormatToInt(string data)
    {
        return int.Parse(data);
    }
}
