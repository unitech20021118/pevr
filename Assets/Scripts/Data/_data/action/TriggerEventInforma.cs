using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class TriggerEventInforma :ActionInforma {

    public int triggerNameID;
    

    public TriggerEventInforma(bool isTrue)
    {
        this.isOnce = isTrue;
    }
}
