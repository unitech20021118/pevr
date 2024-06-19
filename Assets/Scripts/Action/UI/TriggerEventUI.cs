using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TriggerEventUI :ActionUI {
    public Dropdown dropdown;
    public Text evenText;
    TriggerEvent te;
    TriggerEventInforma triggerEventInforma;
    //void Start()
    //{
    //    te = (TriggerEvent)action;
    //    te.triggerType = TriggerType.OnTriggerEnter;
    //    Manager.Instace.gonggong.GetComponent<Collider>().isTrigger = true;
    //    dropdown.onValueChanged.AddListener(delegate(int a) { Change(dropdown.value); });
    //}

    void Change(int index)
    {
        triggerEventInforma.triggerNameID = index;
        switch (index)
        {
            case 0:
                te.triggerType = TriggerType.OnTriggerEnter;
                
                break;
            case 1:
                te.triggerType = TriggerType.OnTriggerStay;
                break;
            case 2:
                te.triggerType = TriggerType.OnTriggerExit;
                break;
                
        }
    }

    public override Action<Main> CreateAction()
    {
        action = new TriggerEvent();
        action.isOnce = false;
        actionInforma = new TriggerEventInforma(false);
        triggerEventInforma = (TriggerEventInforma)actionInforma;
        GetStateInfo().actionList.Add(actionInforma);
        actionInforma.name = "TriggerEvent";
        te = (TriggerEvent)action;
        te.triggerType = TriggerType.OnTriggerEnter;
        Manager.Instace.gonggong.GetComponent<Collider>().isTrigger = true;
        dropdown.onValueChanged.AddListener(delegate(int a) { Change(dropdown.value); });
        return base.CreateAction();
    }

    public override Action<Main> LoadAction(ActionInforma actionInforma)
    {
        triggerEventInforma = (TriggerEventInforma)actionInforma;
        this.actionInforma = actionInforma;
        action = new TriggerEvent();
        
        te = (TriggerEvent)action;
        
        te.triggerType = (TriggerType)triggerEventInforma.triggerNameID;
        foreach (Events e in Manager.Instace.eventlist)
        {
            if (e.name == triggerEventInforma.eventName)
            {
                te.even = e;
                evenText.text = e.name;
            }
        }
        dropdown.value = triggerEventInforma.triggerNameID;
        Manager.Instace.gonggong.GetComponent<Collider>().isTrigger = true;
        dropdown.onValueChanged.AddListener(delegate(int a) { Change(dropdown.value); });
        return action;
    }
}
