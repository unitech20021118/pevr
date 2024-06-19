using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendEvent : Action<Main>
{

    public override void DoAction(Main m )
    {
        even.DoRelateToEvents();
    }

}
