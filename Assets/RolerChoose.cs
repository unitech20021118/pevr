using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common;
public class RolerChoose :BaseRequest {
	public RoleSelectClient rsclient;
	List<Button> roleBtns;
    GameObject prefab;
    bool rolerChoosed = false;
    bool rolerExited = false;
    public Transform content;
    public Button ChosseRolerBtn;
	public Toggle taskBtn;
    public Toggle playerBtn;
    const string ROLEBUTTONPATH = "Prefabs/RoleBtn";
	Dictionary<SetFirstPerson, GameObject> dict = new Dictionary<SetFirstPerson, GameObject>();
    //edit by 王梓亦
    Dictionary<SetFirstPerson, GameObject> dictfirst = new Dictionary<SetFirstPerson, GameObject>();

    Dictionary<SetThirdPerson, GameObject> dictthird = new Dictionary<SetThirdPerson, GameObject>();

    public override void Awake()
    {
		roleBtns = new List<Button> ();
        requestCode = RequestCode.Game;
        actionCode = ActionCode.ChooseRoler;
        base.Awake();
    }
    void Start()
    {
        ChosseRolerBtn.onClick.AddListener(ChooseRolerBtnListener);
    }

	void Update(){
		if ( Manager.Instace.tasks!=null&&taskBtn.transform.parent.childCount-1 < Manager.Instace.tasks.Count )
        {
			Transform[] taskObjs = taskBtn.transform.parent.GetComponentsInChildren<Transform> ();
			foreach (Transform item in taskObjs) {
				if(item!=taskBtn.transform.parent)
					Destroy (item.gameObject);
			}
            
            foreach (Task item in Manager.Instace.tasks)
            {
                GameObject go = Instantiate(taskBtn.gameObject, taskBtn.transform.parent) as GameObject;
                go.SetActive(true);
                go.GetComponentInChildren<Text>().text = item.taskName;
				go.transform.Find ("describe").GetComponent<Text> ().text = item.taskDescribe;
            }
		}

		HideRole ();
		foreach (Transform item in taskBtn.transform.parent) {
			if (item.GetComponent<Toggle> ().isOn) {
				string currentTask = item.GetComponentInChildren<Text> ().text;
				string[] roles = GetTaskByName (currentTask).roles;
				foreach (string role in roles) {
					foreach (Transform roleBtn in content) {
						if (roleBtn.Find ("Text").GetComponent<Text> ().text == role) {
							roleBtn.gameObject.SetActive (true);
							roleBtn.Find ("Task").GetComponent<Text> ().text = Manager.Instace.playerNoteDic [role];
						}
					}
				}
				break;
			}
		}

		string[] roleSelection = rsclient.receivedMsg.Split (':');
		//print (Manager.Instace.tasks.Count);
		if (roleSelection.Length == 2) {
			if (roleSelection [0] == "selectedrole") {
				foreach (Transform item in content) {
					//print (item.GetComponentInChildren<Text> ().text);
					if (item.GetComponentInChildren<Text> ().text == roleSelection [1]) {
						if (!item.GetComponent<RoleBtnInfo> ().isSelect) {
							item.GetComponent<RoleBtnInfo> ().remoteSelected = true;
							item.GetComponent<RoleBtnInfo> ().SelectBtn (true);
						}
					}
				}
			} else if (roleSelection [0] == "disselectedrole") {
				foreach (Transform item in content) {
					if (item.GetComponentInChildren<Text> ().text == roleSelection [1]) {
						if (!item.GetComponent<RoleBtnInfo> ().isSelect) {
							item.GetComponent<RoleBtnInfo> ().remoteSelected = false;
							item.GetComponent<RoleBtnInfo> ().SelectBtn (false);
						}
					}
				}
			}
		}
        //if(Manager.Instace.playerNames != null && playerBtn.transform.parent.childCount - 1 < Manager.Instace.playerNames.Count)
        //{
        //    Transform[] playerObjs = playerBtn.transform.parent.GetComponentsInChildren<Transform>();
        //    foreach (Transform item in playerObjs)
        //    {
        //        if (item != playerBtn.transform.parent)
        //            Destroy(item.gameObject);
        //    }

        //    foreach (string item in Manager.Instace.playerNames)
        //    {
        //        GameObject go = Instantiate(playerBtn.gameObject, playerBtn.transform.parent) as GameObject;
        //        go.SetActive(true);
        //        go.GetComponentInChildren<Text>().text = item;
        //    }
        //}
	}

    void ChooseRolerBtnListener()
    {
        if (rolerChoosed)
        {
            SendRequest(manager.playerMng.GetCurrentRoleId().ToString());
        }
        StartCoroutine(DoSomething());
        //if (rolerExited) 
        //{ 
        //    gameObject.SetActive(false);
            
        //    //让动作进行下去
        //    //Manager.Instace.gonggong.GetComponent<Main>().m_StateMachine.CurrentState().SetTimerPause();
        //    manager.a();
        //    Debug.Log(Manager.Instace.gonggong);
            
        //}
    }

    IEnumerator DoSomething()
    {
        yield return new WaitForSeconds(0.1f);
        if (rolerExited)
        {
            gameObject.SetActive(false);

            //让动作进行下去
            //Manager.Instace.gonggong.GetComponent<Main>().m_StateMachine.CurrentState().SetTimerPause();
            manager.a();
            Debug.Log(Manager.Instace.gonggong);

        }
    }


	void HideRole(){
		foreach (Transform item in content) {
			item.gameObject.SetActive (false);
		}
	}

	Task GetTaskByName(string taskName){
		foreach (Task item in Manager.Instace.tasks) {
			if (taskName == item.taskName)
				return item;
		}
		return null;
	}

    public GameObject AddRole(SetFirstPerson setFirstPerson,GameObject obj)
    {
        if(prefab==null)
        {
            prefab = (GameObject)Resources.Load(ROLEBUTTONPATH);
        }
        GameObject btn = Instantiate<GameObject>(prefab);
        btn.transform.SetParent(content);
        btn.transform.localScale = Vector3.one;
		btn.AddComponent<RoleBtnInfo> ();
        btn.GetComponent<Button>().onClick.AddListener(delegate() { Listener(setFirstPerson); });
        btn.GetComponentInChildren<Image>().sprite = obj.GetComponent<item>().pic.sprite;
        btn.transform.Find("Task").GetComponent<Text>().text = setFirstPerson.task;
		btn.transform.Find ("Text").GetComponent<Text> ().text = setFirstPerson.num;
		btn.GetComponent<RoleBtnInfo> ().roleName = setFirstPerson.num;
		btn.GetComponent<RoleBtnInfo> ().rsClient = rsclient;
		dict.Add (setFirstPerson, btn);

        dictfirst.Add(setFirstPerson, btn);
        return btn;
    }
    //第三人称增加角色//edit by 王梓亦
    public GameObject AddRole(SetThirdPerson setThirdPerson, GameObject obj)
    {
        if (prefab == null)
        {
            prefab = (GameObject)Resources.Load(ROLEBUTTONPATH);
        }
        GameObject btn = Instantiate<GameObject>(prefab);
        btn.transform.SetParent(content);
        btn.transform.localScale = Vector3.one;
        btn.GetComponent<Button>().onClick.AddListener(delegate() { Listener(setThirdPerson); });
        btn.GetComponentInChildren<Image>().sprite = obj.GetComponent<item>().pic.sprite;
        btn.transform.Find("Task").GetComponent<Text>().text = setThirdPerson.task;
        btn.transform.Find("Text").GetComponent<Text>().text = setThirdPerson.num;

        dictthird.Add(setThirdPerson, btn);
        return btn;
    }

    void Listener(SetFirstPerson setFirstPerson)
    {
        //Manager.Instace.SetCurrentRoleType(setFirstPerson.num);
        Manager.Instace.SetCurrentRoleType(setFirstPerson.num);
        //SendRequest(setFirstPerson.num.ToString());
        rolerChoosed = true;
    }
    //第三人称监听角色//edit by 王梓亦
    void Listener(SetThirdPerson setThirdPerson)
    {
        //Manager.Instace.SetCurrentRoleType(setFirstPerson.num);
        Manager.Instace.SetCurrentRoleType(setThirdPerson.num);
        //SendRequest(setFirstPerson.num.ToString());
        rolerChoosed = true;
    }

    public void DeleteRole(SetFirstPerson setFirstPerson)
    {
        GameObject obj;
        bool isGet = dictfirst.TryGetValue(setFirstPerson, out obj);
        if (isGet)
        {
            Destroy(obj);
			dict.Remove(setFirstPerson);
            dictfirst.Remove(setFirstPerson);
        }
        else
        {
            Debug.Log("字典中不存在该键值对");
        }
    }
    //第三人称删除角色//edit by 王梓亦
    public void DeleteRole(SetThirdPerson setThirdPerson)
    {
        GameObject obj;
        bool isGet = dictthird.TryGetValue(setThirdPerson, out obj);
        if (isGet)
        {
            Destroy(obj);
            dictthird.Remove(setThirdPerson);
        }
        else
        {
            Debug.Log("字典中不存在该键值对");
        }
    }

    //public void SendRequest(string id)
    //{
        
    //    base.SendRequest(id);
    //}

    public override void OnResponse(string data)
    {
        ReturnCode returnCode = (ReturnCode)int.Parse(data);
        if (returnCode == ReturnCode.Success)
        {
            rolerExited = true;
        }
        else
        {
            rolerExited = false;
        }
    }
}
