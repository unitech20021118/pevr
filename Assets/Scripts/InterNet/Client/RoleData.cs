using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
public class RoleData {
    public RoleType roleType;
    public GameObject RolePrefab { get; private set; }
    public RoleData(RoleType roleType)
    {
        this.roleType = roleType;
    }
 
}
