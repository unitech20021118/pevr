using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PIShowUI : ActionUI {
    private PIShow piShow;
    private PIShowInforma piShowInforma;
    public override Action<Main> CreateAction()
    {
        action = new PIShow();
        actionInforma = new PIShowInforma(true);
        GetStateInfo().actionList.Add(actionInforma);
        actionInforma.name = "PIShow";
        return base.CreateAction();
    }
    public override Action<Main> LoadAction(ActionInforma actionInforma)
    {
        piShowInforma = (PIShowInforma)actionInforma;
        this.actionInforma = actionInforma;
        action = new PIShow();
        piShow = (PIShow)action;
        

        return base.LoadAction(actionInforma);
    }
}
