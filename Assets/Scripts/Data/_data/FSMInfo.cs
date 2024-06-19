using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class FSMInfo : Base {

    public FSMInfo()
    {

    }



    public FSMInfo(string na)
    {
        
        //pos= trans.position;
        //scale = trans.localScale;
        //rotate = trans.eulerAngles;
        //transform = trans;
        this.name = na;
        id++;
        index = id;
    }
    public override void GetTransform()
    {
        Transform t = Manager.Instace.StateMachineCanvas.transform.Find(name);
        pos = t.localPosition.ToString();
        rotate = t.eulerAngles.ToString();
        scale = t.localScale.ToString();
        isActive = t.gameObject.activeSelf;

    }
}
