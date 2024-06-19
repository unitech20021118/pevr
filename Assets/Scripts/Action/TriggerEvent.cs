using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEvent : Action<Main>{



    public TriggerType triggerType;
    public override void DoAction(Main m)
    {
        //eidt by kuai 
        //在触发器本身上添加刚体以满足触发条件
        if (m.gameObject.GetComponent<Rigidbody>()==null)
        {
           Rigidbody r = m.gameObject.AddComponent<Rigidbody>();
            r.useGravity = false;
            r.isKinematic = true;
        }
        if (m.gameObject.GetComponent<BoxCollider>()==null)
        {
            BoxCollider b = m.gameObject.AddComponent<BoxCollider>();
            b.isTrigger = true;
        }
        else
        {
            m.gameObject.GetComponent<BoxCollider>().isTrigger = true;
        }
        switch (triggerType)
        {
            case TriggerType.OnTriggerEnter:
                if (m.IsEnter)
                {
                    //even.Do();
                    even.DoRelateToEvents();
                }
                break;
            case TriggerType.OnTriggerStay:
                if (m.IsStay)
                {
                    //even.Do();
                    even.DoRelateToEvents();
                }
                break;
            case TriggerType.OnTriggerExit:
                if (m.isExit)
                {
                    //even.Do();
                    even.DoRelateToEvents();
                }
                break;
        }
    }

    public TriggerEvent()
    {
        isOnce = false;
    }
}
public enum TriggerType
{
    OnTriggerEnter,
    OnTriggerStay,
    OnTriggerExit,
}