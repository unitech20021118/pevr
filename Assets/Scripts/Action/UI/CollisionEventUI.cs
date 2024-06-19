using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollisionEventUI : ActionUI {

    public Dropdown dropdown;
    public Text evenText;
    CollisionEvent collisionEvent;
    ColliderEventInforma colliderEventInforma;
    //void Start()
    //{
    //    te = (TriggerEvent)action;
    //    te.triggerType = TriggerType.OnTriggerEnter;
    //    Manager.Instace.gonggong.GetComponent<Collider>().isTrigger = true;
    //    dropdown.onValueChanged.AddListener(delegate(int a) { Change(dropdown.value); });
    //}

    void Change(int index)
    {
        colliderEventInforma.colliderNameID = index;
        switch (index)
        {
            case 0:
                collisionEvent.colliderType = CollisionType.OnCollisionEnter;
                break;
            case 1:
                collisionEvent.colliderType = CollisionType.OnCollisionStay;
                break;
            case 2:
                collisionEvent.colliderType = CollisionType.OnCollisionExit;
                break;

        }
    }

    public override Action<Main> CreateAction()
    {
        action = new CollisionEvent();
        action.isOnce = false;
        actionInforma = new ColliderEventInforma(false);
        colliderEventInforma = (ColliderEventInforma)actionInforma;
        GetStateInfo().actionList.Add(actionInforma);
        actionInforma.name = "ColliderEvent";
        collisionEvent = (CollisionEvent)action;
        collisionEvent.colliderType = CollisionType.OnCollisionEnter;
        Manager.Instace.gonggong.GetComponent<Collider>().isTrigger = false;
        dropdown.onValueChanged.AddListener(delegate(int a) { Change(dropdown.value); });
        return base.CreateAction();
    }

    public override Action<Main> LoadAction(ActionInforma actionInforma)
    {
        colliderEventInforma = (ColliderEventInforma)actionInforma;
        this.actionInforma = actionInforma;
        action = new CollisionEvent();

        collisionEvent = (CollisionEvent)action;

        collisionEvent.colliderType = (CollisionType)colliderEventInforma.colliderNameID;
        foreach (Events e in Manager.Instace.eventlist)
        {
            if (e.name == colliderEventInforma.eventName)
            {
                collisionEvent.even = e;
                evenText.text = e.name;
            }
        }
        dropdown.value = colliderEventInforma.colliderNameID;
        Manager.Instace.gonggong.GetComponent<Collider>().isTrigger = false;
        dropdown.onValueChanged.AddListener(delegate(int a) { Change(dropdown.value); });
        return action;
    }
}
