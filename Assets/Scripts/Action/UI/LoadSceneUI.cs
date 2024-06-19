using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadSceneUI : ActionUI {
	public Toggle ISVR;
	public InputField pathField;
	LoadScene loadScene;
	LoadSceneInforma lsInforma;

	public override Action<Main> CreateAction()
	{
		action = new LoadScene();
		action.isOnce = true;
		actionInforma = new LoadSceneInforma(true);
        lsInforma =(LoadSceneInforma)actionInforma;
		GetStateInfo().actionList.Add(actionInforma);
		actionInforma.name = "LoadScene";
		return base.CreateAction();
	}

	public override Action<Main> LoadAction(ActionInforma actionInforma)
	{
		lsInforma = (LoadSceneInforma)actionInforma;
		//actionInforma = a;
		//this.actionInforma = a;
		this.actionInforma = actionInforma;
		action = new LoadScene();
		loadScene = (LoadScene)action;
//		timeInputField.text = loadScene.duringTime.ToString();
		pathField.text = lsInforma.filePath;

		ISVR.isOn = lsInforma.ISVR;
		loadScene.filePath = lsInforma.filePath;
		loadScene.ISVR = lsInforma.ISVR;
		//		targetText.text = setVInforma.isVisible.ToString ();
		//		UpdateInput ();
		//		this.actionInforma = actionInforma;
		//return base.LoadAction(a);
		return action;
	}

//	void Start () {
//		timeInputField.onValueChanged.AddListener(delegate(string a) { ActionTimeChanged(); });
//	}
//	void ActionTimeChanged()
//	{
//		if (loadScene != null)
//		{
//			loadScene.duringTime = float.Parse(timeInputField.text);
//			lsInforma.durtime = loadScene.duringTime;
//		}
//	}

	public void UpdateInput(){
		loadScene = (LoadScene)action;
		try{
            if (lsInforma==null)
            { 
                lsInforma = (LoadSceneInforma)actionInforma;
            }
			loadScene.filePath=pathField.text;
			lsInforma.filePath=pathField.text;
		}
        catch(System.Exception e)
        {
            Debug.LogError(e);
		}
	}
    public void OnValueChanged()
    {
        loadScene = (LoadScene)action;
		try{
            if (lsInforma==null)
            {
                lsInforma = (LoadSceneInforma)actionInforma;
            }
			loadScene.ISVR=ISVR.isOn;
			lsInforma.ISVR=ISVR.isOn;
		}
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }
    }
}
