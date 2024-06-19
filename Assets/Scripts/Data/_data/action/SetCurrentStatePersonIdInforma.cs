using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class SetCurrentStatePersonIdInforma :ActionInforma{
    public string personId;
    public string[] personIds;
	public string taskName;
    public SetCurrentStatePersonIdInforma(bool isOnce)
    {
        this.isOnce = isOnce;
    }
}
