using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskEditor : MonoBehaviour {
	public float taskUIOpenHight,taskUICloseHight;
	public RectTransform taskUI;
	public InputField taskNameField, taskDescribeField;
	public Transform rolesContent;
	public GameObject roleTogglePrefab;
	public Dropdown roleList;
	public List<string> roles;
    public string taskName, taskDescribe;

	// Use this for initialization
	void Start () {
		//taskUI.sizeDelta = new Vector2 (200, 60);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void FoldTaskUI(){
		if (Mathf.Abs(taskUI.sizeDelta.y - taskUIOpenHight) < 1) {
			taskUI.sizeDelta = new Vector2 (taskUI.sizeDelta.x, taskUICloseHight);
			return;
		}

		if (Mathf.Abs(taskUI.sizeDelta.y - taskUICloseHight) < 1) {
			taskUI.sizeDelta = new Vector2 (taskUI.sizeDelta.x, taskUIOpenHight);
			return;
		}

	}

	public void AddRoles(bool checkSelect){
		foreach (string item in Manager.Instace.playerNames) {
			GameObject temp = Instantiate (roleTogglePrefab, rolesContent);
            temp.GetComponentInChildren<Text>().text = item;
            if (checkSelect)
            {
                foreach (string roleName in roles)
                {
                    string tempText= temp.GetComponentInChildren<Text>().text;
                    if (roleName == tempText)
                    {
                        temp.GetComponent<Toggle>().isOn = true;
                        break;
                    }
                }
            }
			temp.SetActive (true);
		}
	}

	public void ChooseRole(Toggle roleToggle){
		if (roles == null) {
			roles = new List<string> ();
		}

        string roleName = roleToggle.GetComponentInChildren<Text>().text;
        if (roleToggle.isOn)
        {
            if (!roles.Contains(roleName))
            {
                roles.Add(roleName);
            }
        }
        else
        {
            if (roles.Contains(roleName))
            {
                roles.Remove(roleName);
            }
        }
	}

	public void GetPlayerList()
	{
		
	}

	public Task GetTask(){
		//Text[] roleTexts = rolesContent.GetComponentsInChildren<Text> ();
		//if (roles == null) {
		//	foreach (Text item in roleTexts) {
		//		roles.Add (item.text);
		//	}
		//}
        //return new Task (taskNameField.text, taskDescribeField.text, roles.ToArray ());
        return new Task(taskName, taskDescribe, roles.ToArray());
    }
}
