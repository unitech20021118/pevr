using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class TaskSetting  {
	public RoleInfo[] roles;
	public TaskSaveInfo[] tasks;

	public void SetRoles(string[] roleNames,string[] roleDescribes){
		roles = new RoleInfo[roleNames.Length];
		for (int i = 0; i < roles.Length; i++) {
			roles [i].roleName = roleNames [i];
			roles [i].roleDescribe = roleDescribes [i];
		}
	}
}
[System.Serializable]
public struct RoleInfo
{
	public string roleName;
	public string roleDescribe;
}
[System.Serializable]
public struct TaskSaveInfo
{
	public string taskName;
	public string taskDescribe;
	public string[] roleNames;
}
