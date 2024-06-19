using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class WaitingSecondsInforma:ActionInforma{

    public WaitingSecondsInforma(bool isOnce)
    {
        this.isOnce = isOnce;
    }
}
