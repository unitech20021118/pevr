using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delivery : Action<Main>
{

    public GameObject target;
    public string targetName;
    public Vector3 TargetPosition;

    public override void DoAction(Main m)
    {
        if (target == null)
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
        target.transform.position = TargetPosition;
    }


}
