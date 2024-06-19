using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerUI : ActionUI {

	public override Action<Main> CreateAction()
	{
		action = new FollowPlayer();
		actionInforma = new FollowPlayerInforma(true);
		GetStateInfo().actionList.Add(actionInforma);
		actionInforma.name = "FollowPlayer";
		return base.CreateAction();
	}

	public override Action<Main> LoadAction (ActionInforma actionInforma)
	{
		FollowPlayerInforma followPlayerInforma = (FollowPlayerInforma)actionInforma;
		this.actionInforma = actionInforma;
		action = new FollowPlayer();

		return action;
	}
}
