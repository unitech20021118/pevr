using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SetFirstPersonInforma :ActionInforma {

	public float forceNum;
    public float walkSpeed;
    public float runSpeed;
    public float jumpSpeed;
    public float fpsHeight;


    //edit by 吕存全
    public string ChooseNum;

    public string task;
    public bool isNet;
    //----------
    //2019-12-12-wzy
   public Dictionary<string, bool> persontaskDic = new Dictionary<string, bool>();

    public SetFirstPersonInforma(bool isOnce)
    {
        this.isOnce = isOnce;
    }
}
