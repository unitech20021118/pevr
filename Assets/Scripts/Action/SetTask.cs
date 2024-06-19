using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTask : Action<Main> {
	public string taskName;
	public string taskDescribe;
	public string[] roles;

	public override void DoAction(Main m)
	{
		if (Manager.Instace.tasks == null) {
			Manager.Instace.tasks = new List<Task> ();
		}

		Manager.Instace.tasks.Add (new Task (taskName, taskDescribe, roles));
	}
}
