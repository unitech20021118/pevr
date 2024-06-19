using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskInfo : MonoBehaviour {
	public Text roleNameText;
	public Text taskName;
	public static string roleName;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//选择任务
	public void SelectTask(bool isSelected){
		if (!string.IsNullOrEmpty (roleName)) {
            if (isSelected)
            {
                //遍历所有任务状态，绑定到指定角色
                foreach (State<Main> item in Manager.Instace.taskDictionary[taskName.text].states)
                {
                    item.personId.Add(roleName);
                    print (item.personId[0] + "^^^^^^^^^" + roleName);
                }
            }
            else
            {
                foreach (State<Main> item in Manager.Instace.taskDictionary[taskName.text].states)
                {
                    item.personId.Remove(roleName);
                }
            }
        }
    }

	//选择角色
	public void SelectRole(bool isSelected){
        if(isSelected)
		    roleName = roleNameText.text;
	}

    public void SelectRole()
    {
        roleName = roleNameText.text;
    }
}
