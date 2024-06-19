using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireUI : ActionUI
{
    private Fire fire;
    private FireInforma fireInforma;
    public override Action<Main> CreateAction()
    {
        action = new Fire();
        actionInforma = new FireInforma(true);
        GetStateInfo().actionList.Add(actionInforma);
        actionInforma.name = "Fire";
        return base.CreateAction();
    }
    public override Action<Main> LoadAction(ActionInforma actionInforma)
    {
        fireInforma = (FireInforma)actionInforma;
        this.actionInforma = actionInforma;
        action = new Fire();
        fire = (Fire)action;
        return base.LoadAction(actionInforma);
    }
}
