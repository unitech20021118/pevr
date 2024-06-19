using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoleBtnInfo : MonoBehaviour {
	public bool isSelect = false;
	public bool remoteSelected = false;
	public RoleSelectClient rsClient;
	public string roleName;
	private Button btn;

	void Awake(){
		btn = GetComponent<Button> ();
		if (btn) {
			btn.onClick.AddListener (ClickBtn);
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SelectBtn(bool isOn){
		if (isOn)
			btn.GetComponent<Image>().color = Color.blue;
		else
			btn.GetComponent<Image>().color = Color.white;
	}

	void ClickBtn(){
		if (!remoteSelected) {
			isSelect = !isSelect;
			if (isSelect) {
				RoleSelectClient.roleInfo = roleName;
				rsClient.SendRoleInfo ();
				SelectBtn (true);
			} else {
				RoleSelectClient.roleInfo = roleName;
				rsClient.SendRoleInfo2 ();
				SelectBtn (false);
			}
		}
	}
}
