using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using UnityStandardAssets.Characters.ThirdPerson;

public class MoveRequest :BaseRequest {
    
    private Transform localPlayerTransform;
    //public PlayerMove locakPlayerMove;
    private int synRate = 20;
    private Vector3 pos;
    private Vector3 rote;
    private Vector3 m_move;
    private bool m_crouch;
    private bool m_jump;
    private string id;
    private bool walk;
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
        //SyncLocalPlayer();
        if (isSyncRemotePlayer == true)
        {
            SyncRemotePlayer();
            isSyncRemotePlayer = false;
        }

        //if (walk == true)
        //{
        //    Transform obj = manager.playerMng.transformDict[id.ToString()];
        //    obj.GetComponent<Animator>().SetFloat("Forward", 1f);
        //}
        //if (walk == false)
        //{
        //    Transform obj = manager.playerMng.transformDict[id.ToString()];
        //    obj.GetComponent<Animator>().SetFloat("Forward", -1f);
        //}
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
        if (!VRSwitch.isVR)
        {
            SendRequest(localPlayerTransform.position.x, localPlayerTransform.position.y, localPlayerTransform.position.z,
                        localPlayerTransform.eulerAngles.x, localPlayerTransform.eulerAngles.y, localPlayerTransform.eulerAngles.z,
                        localPlayerTransform.GetComponent<GameObjectId>().TransformName, localPlayerTransform.GetComponent<Fperson>().m_Move,
                        localPlayerTransform.GetComponent<Fperson>().m_Crouch, localPlayerTransform.GetComponent<Fperson>().m_Jump);
        }
        else
        {
            SendRequest(localPlayerTransform.position.x, localPlayerTransform.position.y, localPlayerTransform.position.z,
                        localPlayerTransform.eulerAngles.x, localPlayerTransform.eulerAngles.y, localPlayerTransform.eulerAngles.z,
                        localPlayerTransform.GetComponent<GameObjectId>().TransformName, localPlayerTransform.GetComponent<FpersonVR>().m_Move,
                        localPlayerTransform.GetComponent<FpersonVR>().m_Crouch, localPlayerTransform.GetComponent<FpersonVR>().m_Jump);
        }
    }
    /// <summary>
    /// 做出应对位置和动作
    /// </summary>
    private void SyncRemotePlayer()
    {
        //if (remotePlayerTransform != null)
        //{
        //    remotePlayerTransform.position = pos;
        //    remotePlayerTransform.eulerAngles = rote;
        //}
        Transform obj= manager.playerMng.transformDict[id.ToString()];
        if (!obj.GetComponent<ThirdPersonCharacter>())
        {
            obj.gameObject.AddComponent<ThirdPersonCharacter>();
        }
        if (obj.GetComponent<Animator>().runtimeAnimatorController != Manager.Instace.Runimatorcontroller)
        {
            obj.GetComponent<Animator>().runtimeAnimatorController = Manager.Instace.Runimatorcontroller;
        }
        obj.GetComponent<Rigidbody>().isKinematic = false;
        obj.GetComponent<Rigidbody>().mass = 0.4f;
        //print(obj.name);
        obj.position = pos;
        obj.eulerAngles = rote;
        obj.GetComponent<ThirdPersonCharacter>().Move(m_move, m_crouch, m_jump);//移动动画
    }

    public void SendRequest(float x,float y,float z,float rotationX,float rotationY,float rotationZ,string id,Vector3 move,bool crouch,bool jump)
    {
        string data = string.Format("{0},{1},{2}|{3},{4},{5}|{6}|{7},{8},{9}|{10}|{11}", x, y, z, rotationX, rotationY, rotationZ,id, move.x,move.y,move.z,crouch,jump);
        //print(data);
        base.SendRequest(data);
    }

    public override void OnResponse(string data)
    {
        print("@@@" + data);
        string[] strs = data.Split('|');
        pos = Format(strs[0]);
        rote = Format(strs[1]);
        //通过ID找到remotePlayer
        //manager.SetRemoteRole(int.Parse(strs[2]));
        //id = FormatToInt(strs[2]);
        id = strs[2];
        m_move = Format(strs[3]);
        m_crouch = bool.Parse(strs[4]);
        m_jump = bool.Parse(strs[5]);
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
