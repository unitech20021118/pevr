using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoosePlayerUI :ActionUI{

    public override Action<Main> CreateAction()
    {
        action = new ChoosePlayer();
        action.isOnce = true;
        actionInforma = new ActionInforma(true);
        GetStateInfo().actionList.Add(actionInforma);
        actionInforma.name = "ChoosePlayer";
        return base.CreateAction();
    }

    public override Action<Main> LoadAction(ActionInforma actionInforma)
    {
        action = new ChoosePlayer();
        action.isOnce = true;
        this.actionInforma = actionInforma;
        return base.LoadAction(actionInforma);
    }
}
