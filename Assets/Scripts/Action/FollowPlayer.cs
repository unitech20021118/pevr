using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : Action<Main> {

	public override void DoAction(Main m)
	{
		//Manager.Instace.Substance = m.gameObject;
		if (VRSwitch.isVR) 
        {
			//---------------------------------------------------
			if (Manager.Instace.VRCamera.activeSelf) {
				m.gameObject.transform.SetParent(Manager.Instace.VRCamera.transform.Find(@"Camera (eye)"));
			}
			//---------------------------------------------------
		} 
        else 
        {
            GameObject.Find("Canvas").GetComponent<FollowPlayerGameobject>().AddFollowGameObject(m.gameObject);
		}
		//Manager.Instace.Substance.SetActive(false);

	}
    
}
