using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowMsg : Action<Main> {
	public string msg;

	public override void DoAction(Main m)
	{
		ObjMsg objMsg = m.gameObject.GetComponent<ObjMsg> ();
		if (objMsg == null) {
			objMsg = m.gameObject.AddComponent<ObjMsg> ();
		}
		objMsg.msg = msg;
	}
}
