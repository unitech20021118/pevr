using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ColliderEventInforma : ActionInforma {
 public int colliderNameID;


 public ColliderEventInforma(bool isTrue)
    {
        this.isOnce = isTrue;
    }
}
