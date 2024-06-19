using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class ShowInterfaceInforma : ActionInforma
{

    public List<InterfaceQualityInforma> InterfaceQualityInformas = new List<InterfaceQualityInforma>();

    public List<int> CloseIndexList = new List<int>();

    public ShowInterfaceInforma(bool isOnce)
    {
        this.isOnce = isOnce;
    }
}