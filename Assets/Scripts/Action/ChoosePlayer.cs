using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoosePlayer :Action<Main>{

    public override void DoAction(Main m)
    {
        Manager.Instace.roleChoose.gameObject.SetActive(true);        
        duringTime = 1f;
    }
    public ChoosePlayer()
    {
        pause = true;
    }
}
