using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragableObjUI : ActionUI {
	public InputField sensitivity;

	public override Action<Main> CreateAction()
	{
		action = new Dragaction();
		//		action.isOnce = true;
		actionInforma = new DragactionInforma(true);
		GetStateInfo().actionList.Add(actionInforma);
		actionInforma.name = "DragObj";
		return base.CreateAction();
	}

	public override Action<Main> LoadAction(ActionInforma actionInforma)
	{
		DragactionInforma setVInforma = (DragactionInforma)actionInforma;
		//actionInforma = a;
		//this.actionInforma = a;
		this.actionInforma = actionInforma;
		action = new Dragaction();
		Dragaction setVisibility = (Dragaction)action;
		setVisibility.rotateSensitivity = setVInforma.rotateS;
		sensitivity.text = setVInforma.rotateS.ToString();
		return action;
	}

	public void UpdateInput(){
		Dragaction dragaction = (Dragaction)action;
		try{
			dragaction.rotateSensitivity=float.Parse(sensitivity.text);
			DragactionInforma setVInforma = (DragactionInforma)actionInforma;
			setVInforma.rotateS=float.Parse(sensitivity.text);
		}catch{

		}
	}
}
