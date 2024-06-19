using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SendEventUI :ActionUI {

    public Text evenText;
    SendEvent te;
    ActionInforma sendEventInforma;

    public override Action<Main> CreateAction()
    {
        action = new SendEvent();
        actionInforma = new ActionInforma(true);
        sendEventInforma = actionInforma;
        GetStateInfo().actionList.Add(actionInforma);
        actionInforma.name = "OtherActionWithEvent";
        te = (SendEvent)action;
        return base.CreateAction();
    }

    public override Action<Main> LoadAction(ActionInforma actionInforma)
    {
        sendEventInforma = actionInforma;
        this.actionInforma = actionInforma;
        action = new SendEvent();
        te = (SendEvent)action;
        foreach (Events e in Manager.Instace.eventlist)
        {
            if (e.name == sendEventInforma.eventName)
            {
                te.even = e;
                evenText.text = e.name;
            }
        }
        return base.LoadAction(sendEventInforma);
    }
}
