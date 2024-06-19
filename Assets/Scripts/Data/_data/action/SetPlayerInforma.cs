using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class SetPlayerInforma : ActionInforma
{
    public string[] playerNames;

    public SetPlayerInforma(bool isOnce)
    {
        this.isOnce = isOnce;
    }
}
