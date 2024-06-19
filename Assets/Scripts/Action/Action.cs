using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Action<T> {

    public int id;
    public string name;
    public Events even;
    public bool isOnce;
    public float duringTime;
    public bool pause = false;

    public virtual IEnumerator DoActionOnce(T m)
    {
        yield return null;
    }

    public virtual void DoAction(T m)
    {

    }

    public virtual void SetSituation() {
        isOnce = true;
    }

    public Action()
    {
        isOnce = true;
    }

     

        


}
