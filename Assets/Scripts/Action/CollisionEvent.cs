using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionEvent : Action<Main> {

    public CollisionType colliderType;
    public override void DoAction(Main m)
    {
        if (m.gameObject.GetComponent<Rigidbody>() == null)
        {
            Rigidbody r = m.gameObject.AddComponent<Rigidbody>();
            r.useGravity = false;
            r.isKinematic = true;
        }
        if (m.gameObject.GetComponent<BoxCollider>() == null)
        {
            BoxCollider b = m.gameObject.AddComponent<BoxCollider>();
            b.isTrigger = false;
        }
        else
        {
            m.gameObject.GetComponent<BoxCollider>().isTrigger = false;
        }
        switch (colliderType)
        {
            case CollisionType.OnCollisionEnter:
                if (m.IsEnter)
                {
                    //even.Do();
                    even.DoRelateToEvents();
                }
                break;
            case CollisionType.OnCollisionStay:
                if (m.IsStay)
                {
                    //even.Do();
                    even.DoRelateToEvents();
                }
                break;
            case CollisionType.OnCollisionExit:
                if (m.isExit)
                {
                    //even.Do();
                    even.DoRelateToEvents();
                }
                break;
        }
    }

    public CollisionEvent()
    {
        isOnce = false;
    }
}

public enum CollisionType
{
    OnCollisionEnter,
    OnCollisionStay,
    OnCollisionExit
}