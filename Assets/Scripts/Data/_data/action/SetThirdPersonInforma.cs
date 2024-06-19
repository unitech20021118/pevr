using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class SetThirdPersonInforma:ActionInforma
{
    //edit by 王梓亦
    public string ChooseNum;

    public string task;
    public bool isNet;
    public SetThirdPersonInforma(bool isTrue)
    {
        this.isOnce = isTrue;
    }
}
