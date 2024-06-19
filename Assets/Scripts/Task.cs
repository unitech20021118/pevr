using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task {
	public string taskName;//任务名
	public string taskDescribe;
	public string[] roles;
	public List<State<Main>> states;//所有注册该任务状态

	public Task(string taskName,string describe,string[] roles){
		this.taskName = taskName;
		taskDescribe = describe;
		this.roles = roles;
		states = new List<State<Main>> ();
	}

	public void AddState(State<Main> state){
		if (!states.Contains (state)) {
			states.Add (state);
		}
	}

	public void RemoveState(State<Main> state){
		if (states.Contains (state)) {
			states.Remove (state);
		}
	}
}
