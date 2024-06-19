using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : Action<Main> {
	public static string targetPath;
	public string filePath;
	public bool ISVR;

	public override void DoAction(Main m)
	{
        Manager.Instace.Save(true,true);
		targetPath = filePath;
		SceneCtrl.isAction = true;
        //edit by kuai
		//VRSwitch.SetVR (ISVR);
        //VRSwitch.Instance.SetVRState(ISVR);
        LoadManager.Instance.SetIsVR(ISVR);
        SceneCtrl.instance.OpenScene(true);
	}
}
