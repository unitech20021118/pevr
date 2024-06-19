using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PIShow : Action<Main>
{

    public override void DoAction(Main m)
    {
        if (VRSwitch.isVR)
        {
            PICamera.Instance.vrMode = true;
            PICamera.Instance.SetVrModeObjects();
            Debug.LogError("vrmoshi");
        }

        PICamera.Instance.StartPIShow(m.gameObject);
    }
}
