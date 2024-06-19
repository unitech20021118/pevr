using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnToNext : Action<Main>
{
	public VRBtn vrBtn;

    public override void DoAction(Main m)
    {
		if (vrBtn!=null&&vrBtn.isClicked)
        {
            even.Do();
			vrBtn.SetClickState (false);
        }
		if (vrBtn == null) {
			Debug.Log ("vrbtn is null");
		}
    }
}
