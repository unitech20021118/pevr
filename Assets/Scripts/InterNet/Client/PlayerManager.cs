using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.Characters.ThirdPerson;
using VRTK;

public class PlayerManager : BaseManager {

    private UserData userData;

    private Dictionary<RoleType, RoleData> roleDataDict = new Dictionary<RoleType, RoleData>();
    public Dictionary<string, Transform> transformDict = new Dictionary<string, Transform>();
    //private RoleType currentRoleType;
    private string currentRoleId;
    private GameObject currentRoleGameObject;
    private GameObject remoteRoleGameObject;
    public UserData UserData
    {
        set { userData = value; }
        get { return userData;  }
    }

    //public void SetCurrentRoleType(RoleType rt)
    //{
    //    currentRoleType = rt;
    //}



    //public RoleType GetCurrentRoleType()
    //{
    //    return currentRoleType;
    //}

    public void SetCurrentRoleId(string id)
    {
        currentRoleId = id;
    }

    public string GetCurrentRoleId()
    {
        return currentRoleId;
    }


    //public void SetCurrentRoleGameObject(GameObject go)
    //{
    //    currentRoleGameObject = go;
    //    currentRoleGameObject.AddComponent<PlayerMove>();
    //    FollowTarget ft = Manager.Instace.mainCamera.GetComponent<FollowTarget>();
    //    ft.enabled = true;
    //    ft.target = go.transform;
    //    ft.GetComponent<G_ObserveScene>().enabled = false;

    //}

    public void SetCurrentRoleGameObject(GameObject go)
    {
        currentRoleGameObject = go;
        if (VRSwitch.isVR)
        {
            //2019-11-26-wzy
            if (Manager.Instace.isnet)
            {
                Debug.Log(789798);
                VRSwitch.SetVR(true);
                go.GetComponent<Rigidbody>().isKinematic = false;
                go.GetComponent<Rigidbody>().mass = 0.4f;
                go.AddComponent<ThirdPersonCharacter>();
                go.AddComponent<FpersonVR>();
                
                Manager.Instace.cameraEye.GetComponent<VRTK_TrackedHeadset>().enabled = false;
                Manager.Instace.VRtoukuitongbu.GetComponent<SteamVR_TrackedObject>().enabled = false;
                Manager.Instace.leftgo.SetActive(false);
                Manager.Instace.rightgo.SetActive(false);
                Manager.Instace.VRCamera.SetActive(true);
                Manager.Instace.VRCamera.transform.SetParent(go.transform);
                Manager.Instace.VRCamera.transform.localPosition = Vector3.zero;
                
                FollowTarget ft = Manager.Instace.mainCamera.GetComponent<FollowTarget>();
            }
            else
            {
                VRSwitch.SetVR(true);
                Manager.Instace.VRCamera.SetActive(true);
                go.transform.SetParent(Manager.Instace.VRCamera.transform);
                go.transform.position = Vector3.zero;

                go.AddComponent<Fperson>();
                go.AddComponent<FirstPersonController>();
                go.GetComponent<BoxCollider>().enabled = false;
                go.GetComponent<CharacterController>().enabled = true;

                FollowTarget ft = Manager.Instace.mainCamera.GetComponent<FollowTarget>();
            }
        }
        else
        {
            if (Manager.Instace.isnet)
            {
                Manager.Instace.FirstPerson.SetActive(true);
                //go.transform.SetParent(Manager.Instace.FirstPerson.transform);
                //go.transform.position = Vector3.zero;
                Manager.Instace.FirstPerson.transform.SetParent(go.transform);
                Manager.Instace.FirstPerson.transform.localPosition = Vector3.zero;
                //2019-11-25-wzy

                go.GetComponent<Rigidbody>().isKinematic = false;
                go.GetComponent<Rigidbody>().mass = 0.4f;
                go.AddComponent<ThirdPersonCharacter>();
                go.AddComponent<Fperson>();
                Manager.Instace.FirstPerson.GetComponent<CharacterController>().enabled = false;
                Manager.Instace.FirstPerson.GetComponent<CapsuleCollider>().enabled = false;
                Manager.Instace.FirstPerson.GetComponent<FPSForce>().enabled = false;
                Manager.Instace.FirstPerson.GetComponent<FirstPersonController>().enabled = false;
                FollowTarget ft = Manager.Instace.mainCamera.GetComponent<FollowTarget>();
            }
        }
    }


    public void SetRemoteRoleGameObject(GameObject go)
    {
        remoteRoleGameObject = go;
    }

    public void SetRemoteRoleGameObject(string id)
    {
        remoteRoleGameObject= transformDict.TryGet(id).gameObject;
    }


    public void InitRoleDataDict()
    {
        roleDataDict.Add(RoleType.Blue, new RoleData(RoleType.Blue));
        roleDataDict.Add(RoleType.Red, new RoleData(RoleType.Red));
    }

    //public void SpawnRoles()
    //{
    //    foreach (RoleData roleData in roleDataDict.Values)
    //    {
    //        GameObject go =GameObject.Instantiate(roleData.RolePrefab);
    //        if (roleData.roleType == currentRoleType)
    //        {
    //            currentRoleGameObject =go;//获得当前客户端的角色
    //        }
    //    }
    //}

    
}
