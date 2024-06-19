using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDown : Action<Main> {
	public KeyCode keyCode;
	public static int currentKeyID;

	public override void DoAction(Main m)
	{
		if (Input.GetKeyDown(keyCode))
		{
			even.Do();
		}
	}
}
