using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetTaskUI : ActionUI {
	public InputField taskNameField;
	public InputField inputField;
	public InputField taskDescribe;
	public Text roleItem;
	public Transform roleContent;
	public Dropdown roleList;
	List<string> roleNames;
	SetTask setTask;
	SetTaskInforma setTaskInforma;

	public override Action<Main> CreateAction()
	{
		roleNames = new List<string> ();
		setTask = new SetTask();
		action = setTask;
		actionInforma = new SetTaskInforma(true);
		setTaskInforma = (SetTaskInforma)actionInforma;
		actionInforma.name = "SetTask";
		GetStateInfo().actionList.Add(actionInforma);
		setTask.isOnce = true;
		return base.CreateAction();
	}

	public override Action<Main> LoadAction(ActionInforma actionInforma)
	{
		roleNames = new List<string> ();
		this.actionInforma = (SetTaskInforma)actionInforma;
		setTaskInforma = (SetTaskInforma)this.actionInforma;
		setTask = new SetTask();
		setTask.taskName = setTaskInforma.taskName;
		setTask.taskDescribe = setTaskInforma.taskDescribe;
		foreach (string item in setTaskInforma.roles) {
			roleNames.Add (item);
		}
		setTask.roles = setTaskInforma.roles;
		action = setTask;
		string temp1, temp2;
		temp1=setTaskInforma.taskName;
		temp2=setTaskInforma.taskDescribe;
		string[] temp3 = setTaskInforma.roles;
		taskNameField.text = temp1;
		taskDescribe.text = temp2;
		foreach (string tName in temp3) {
			roleNames.Add (tName);
			GameObject temp = Instantiate<GameObject> (roleItem.gameObject, roleContent);
			temp.GetComponent<Text> ().text = tName;
			temp.SetActive (true);
		}
		return base.LoadAction(actionInforma);
	}

	public void Listener()
	{
		
		setTaskInforma.roles = roleNames.ToArray ();
	}

	public void AddRole(Toggle item){
		if (!roleNames.Contains (item.GetComponentInChildren<Text>().text)) {
			roleNames.Add (item.GetComponentInChildren<Text>().text);
			GameObject temp = Instantiate<GameObject> (roleItem.gameObject, roleContent);
			temp.GetComponent<Text> ().text = item.GetComponentInChildren<Text>().text;
			temp.SetActive (true);
			Listener ();
		}
	}

	public void EditTask(){
		setTask.taskName=taskNameField.text;
		setTask.taskDescribe = taskDescribe.text;
		setTask.roles = roleNames.ToArray ();
		setTaskInforma.taskName = taskNameField.text;
		setTaskInforma.taskDescribe = taskDescribe.text;
		setTaskInforma.roles = roleNames.ToArray ();
	}

	public void GetPlayerList()
	{
		roleList.ClearOptions();
		roleList.AddOptions(Manager.Instace.playerNames);
	}
}
