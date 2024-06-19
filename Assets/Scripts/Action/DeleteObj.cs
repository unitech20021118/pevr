using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteObj : Action<Main> {
    public GameObject target;
    public string targetName;
    
	public override void DoAction (Main m)
    {
        if (target==null)
        {
            if (string.IsNullOrEmpty(targetName))
            {
                target = m.gameObject;
            }
            else 
            {
                target = GameObject.Find("Parent/" + targetName);
            }
        }
        GameObject.Destroy(target);
	}
}
