using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishAction : Action<Main>
{

    public override void DoAction(Main m)
    {
        Debug.LogError(even.name);
        even.Do();

    }
}
