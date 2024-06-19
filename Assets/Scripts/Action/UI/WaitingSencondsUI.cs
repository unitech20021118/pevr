using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WaitingSencondsUI :ActionUI {
    WaitingSenconds waitingSecond;
    WaitingSecondsInforma waitingSecondsInforma;
    public override Action<Main> CreateAction()
    {
        action = new WaitingSenconds();
        action.isOnce = true;
        waitingSecond = (WaitingSenconds)action;
        waitingSecondsInforma = new WaitingSecondsInforma(true);
        actionInforma = waitingSecondsInforma;
        GetStateInfo().actionList.Add(actionInforma);
        actionInforma.name = "WaitingSenconds";

        timeInputField.onValueChanged.AddListener(delegate(string a) { ChangedListener(); });
        return base.CreateAction();
    }

    void ChangedListener()
    {
        waitingSecond.duringTime =float.Parse(timeInputField.text);
        waitingSecondsInforma.durtime = waitingSecond.duringTime;
    }
    public override Action<Main> LoadAction(ActionInforma actionInforma)
    {
        waitingSecondsInforma = (WaitingSecondsInforma)actionInforma;
        this.actionInforma = actionInforma;
        action = new WaitingSenconds();
        action.isOnce = true;
        waitingSecond = (WaitingSenconds)action;
        waitingSecond.duringTime = waitingSecondsInforma.durtime;
        timeInputField.text = waitingSecondsInforma.durtime.ToString();
        timeInputField.onValueChanged.AddListener(delegate(string a) { ChangedListener(); });
        return base.LoadAction(actionInforma);
    }
}
