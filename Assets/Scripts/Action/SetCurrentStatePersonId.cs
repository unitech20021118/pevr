using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCurrentStatePersonId : Action<Main> {
    public string personId ;
    public List<string> personIds;
    public override void DoAction(Main m)
    {
        try
        {
            foreach (string item in personIds)
            {
                m.m_StateMachine.CurrentState().personId.Add(item);
                Debug.Log(m.gameObject.name + "**********" + item + "**********" + personIds.Count);
            }
        }
        catch
        {
            Debug.Log("add personid error");
        }
        //m.m_StateMachine.CurrentState().personId.Add( personId);
        
    }

    public SetCurrentStatePersonId(string id)
    {
        personId = id;
    }
}
