using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class NetTaskSettingUI : MonoBehaviour {
	public GameObject UIObject;
	public List<string> playerNames,playerNotes;
	public InputField playerNameField,playerNoteField;
	public Dropdown roleList;
	public GameObject roleTextPrefab,taskRoleTextPrefab;
	public Transform roleListContent,taskListContent;
	public GameObject taskUITemplate;
	public GameObject[] parts;
	public TaskSetting taskSetting;
    
    public RectTransform addPlayerContent,addTaskContent;
    public GameObject editPlayerUI,editTaskUI;

    private int editRoleIndex;
    private Transform currentEditRole;
    private TaskEditor currentEditTask;

	void Awake(){
		if (UIObject == null)
			UIObject = gameObject;
		UIObject.SetActive (false);
		taskSetting = new TaskSetting ();
		OpenInfoFile ();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// 保存角色列表信息
    /// </summary>
	public void SaveRoleInfo(){
		taskSetting.SetRoles (playerNames.ToArray (), playerNotes.ToArray ());
	}

    /// <summary>
    /// 保存任务设置
    /// </summary>
	public void SaveTaskInfo(){
		TaskEditor[] taskEditors = taskListContent.GetComponentsInChildren<TaskEditor> ();//获取所有设置了任务信息的TaskEditor对象

        //将每个任务信息保存到TaskSaveInfo数组中
		TaskSaveInfo[] taskSaveInfos=new TaskSaveInfo[taskEditors.Length];
		for (int i = 0; i < taskSaveInfos.Length; i++) {
            //taskSaveInfos [i].taskName = taskEditors [i].taskNameField.text;
            //taskSaveInfos [i].taskDescribe = taskEditors [i].taskDescribeField.text;
            taskSaveInfos[i].taskName = taskEditors[i].taskName;
            taskSaveInfos[i].taskDescribe = taskEditors[i].taskDescribe;
            taskSaveInfos [i].roleNames = taskEditors [i].roles.ToArray();
		}

        //将任务设置保存到可序列化的TaskSetting对象中
        taskSetting.tasks = taskSaveInfos;
	}

    /// <summary>
    /// 将TaskSetting对象保存到固定的路径下面
    /// </summary>
	public void SaveInfoToFile(){
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.dataPath+@"/taskSettingInfo.txt");
		bf.Serialize(file, taskSetting);
        file.Close();

        File.Copy(Application.dataPath + @"/taskSettingInfo.txt", Application.streamingAssetsPath + @"/EXE/EXE_Data/taskSettingInfo.txt", true);
        File.Copy(Application.dataPath + @"/taskSettingInfo.txt", Application.streamingAssetsPath + @"/EXE2/EXE_Data/taskSettingInfo.txt", true);
    }

    /// <summary>
    /// 打开保存的任务设置文件，读取任务信息
    /// </summary>
	public void OpenInfoFile(){
		string path = Application.dataPath + @"/taskSettingInfo.txt";
		if (File.Exists (path)) {
			FileStream file=File.Open(path,FileMode.Open);
			BinaryFormatter bf=new BinaryFormatter();
			taskSetting=(TaskSetting)bf.Deserialize(file);
			file.Close();

			foreach (RoleInfo item in taskSetting.roles) {
				playerNames.Add (item.roleName);
				playerNotes.Add (item.roleDescribe);
				GameObject temp = Instantiate<GameObject>(roleTextPrefab, roleListContent);
                temp.transform.Find("name").GetComponent<Text>().text = item.roleName;
                temp.transform.Find("des").GetComponent<Text>().text = item.roleDescribe;
                temp.SetActive(true);
				if (Manager.Instace.playerNames == null)
				{
					Manager.Instace.playerNames = new List<string>();
				}
				Manager.Instace.playerNames.Add(item.roleName);
				if (Manager.Instace.playerNoteDic == null)
				{
					Manager.Instace.playerNoteDic = new Dictionary<string, string> ();
				}
				Manager.Instace.playerNoteDic.Add(item.roleName,item.roleDescribe);
			}

			for (int i = 0; i < taskSetting.tasks.Length; i++) {
				AddTask (taskSetting.tasks[i].taskName,taskSetting.tasks[i].taskDescribe);
			}
			TaskEditor[] taskEditors = taskListContent.GetComponentsInChildren<TaskEditor> ();
			for (int i = 0; i < taskSetting.tasks.Length; i++) {
				//taskEditors [i].taskNameField.text = taskSetting.tasks [i].taskName;
				//taskEditors [i].taskDescribeField.text = taskSetting.tasks [i].taskDescribe;
                taskEditors[i].taskName= taskSetting.tasks[i].taskName;
                taskEditors[i].taskDescribe= taskSetting.tasks[i].taskDescribe;
                foreach (string item in taskSetting.tasks [i].roleNames) {
					taskEditors [i].roles.Add (item);
				}
				//taskEditors [i].AddRoles ();
			}
			AddAllTask ();
		}
	}

	public void NextPart(){
		for (int i = 0; i < parts.Length; i++) {
			if (parts [i].activeSelf&&i<parts.Length-1) {
				parts [i].SetActive (false);
				parts [i + 1].SetActive (true);
				return;
			}
		}
	}

	public void PrePart(){
		for (int i = parts.Length-1; i < parts.Length; i--) {
			if (parts [i].activeSelf&&i>0) {
				parts [i].SetActive (false);
				parts [i - 1].SetActive (true);
				return;
			}
		}
	}

	public void OpenUI(){
		UIObject.SetActive (true);
	}

	public void CloseUI(){
		UIObject.SetActive (false);
	}
    
	public void AddTask(){
		GameObject temp = Instantiate (taskUITemplate, taskListContent);
		temp.transform.SetSiblingIndex (taskListContent.childCount - 2);
		temp.SetActive (true);
	}

    /// <summary>
    /// 添加任务模板，设置任务信息
    /// </summary>
    /// <param name="taskName"></param>
    /// <param name="taskDes"></param>
    public void AddTask(string taskName,string taskDes)
    {
        GameObject temp = Instantiate(taskUITemplate, taskListContent);
        //temp.GetComponent<Text>().text = taskName;
        temp.transform.Find("name").GetComponent<Text>().text = taskName;
        temp.transform.Find("des").GetComponent<Text>().text = taskDes;
        temp.SetActive(true);
    }

    /// <summary>
    /// 根据用户输入的任务信息，将任务添加到任务列表中
    /// </summary>
    /// <param name="taskEditor"></param>
    public void AddTaskToList(TaskEditor taskEditor)
    {
        //判断输入的任务是否为空或是否重复
        Text[] taskTexts = taskListContent.GetComponentsInChildren<Text>();
        foreach (Text item in taskTexts)
        {
            if (item.text == taskEditor.taskNameField.text || taskEditor.taskNameField.text == "")
            {
                editTaskUI.SetActive(false);
                return;
            }
        }

        //创建任务模板，输入任务信息
        GameObject temp = Instantiate(taskUITemplate, taskListContent);
        //temp.GetComponent<Text>().text = taskEditor.taskNameField.text;
        temp.transform.Find("name").GetComponent<Text>().text = taskEditor.taskNameField.text;
        temp.transform.Find("des").GetComponent<Text>().text = taskEditor.taskDescribeField.text;
        temp.SetActive(true);
        temp.GetComponent<TaskEditor>().taskName= taskEditor.taskNameField.text;
        temp.GetComponent<TaskEditor>().taskDescribe = taskEditor.taskDescribeField.text;
        temp.GetComponent<TaskEditor>().roles = taskEditor.roles;
        //删除任务添加界面的角色列表，防止下次打开重复
        foreach (Toggle item in taskEditor.rolesContent.GetComponentsInChildren<Toggle>())
        {
            Destroy(item.gameObject);
        }
        taskEditor.taskNameField.text = "";
        taskEditor.taskDescribeField.text = "";
        editTaskUI.SetActive(false);
    }

    /// <summary>
    /// 删除任务
    /// </summary>
    /// <param name="nameText">显示任务名称的文本组件</param>
    public void DelTask(Text nameText)
    {
        Destroy(nameText.transform.parent.gameObject);
    }

    /// <summary>
    /// 修改任务
    /// </summary>
    /// <param name="taskEditor">保存了任务信息的对象</param>
    public void EditTask(TaskEditor taskEditor)
    {
        TaskEditor newTask= editTaskUI.GetComponent<TaskEditor>();
        currentEditTask = taskEditor;
        newTask.taskNameField.text = taskEditor.taskName;
        newTask.taskDescribeField.text = taskEditor.taskDescribe;
        newTask.roles = taskEditor.roles;
        print(newTask.roles.Count);
        //添加角色列表，勾选已选角色
        newTask.AddRoles(true);
        
        editTaskUI.SetActive(true);
    }

    /// <summary>
    /// 保存任务的修改
    /// </summary>
    /// <param name="taskEditor"></param>
    public void SaveEditTask(TaskEditor taskEditor)
    {
        currentEditTask.taskName = taskEditor.taskNameField.text;
        currentEditTask.taskDescribe = taskEditor.taskDescribeField.text;
        currentEditTask.roles = taskEditor.roles;
        //currentEditTask.GetComponent<Text>().text = currentEditTask.taskName;
        currentEditTask.transform.Find("name").GetComponent<Text>().text = currentEditTask.taskName;
        currentEditTask.transform.Find("des").GetComponent<Text>().text = currentEditTask.taskDescribe;

        foreach (Toggle item in taskEditor.rolesContent.GetComponentsInChildren<Toggle>())
        {
            Destroy(item.gameObject);
        }
        taskEditor.taskNameField.text = "";
        taskEditor.taskDescribeField.text = "";
        editTaskUI.SetActive(false);
    }

	public void AddPlayer()
	{
        //string inputName = playerNameField.text;
        //string inputDes = playerNoteField.text;

        string inputName = editPlayerUI.transform.Find("newName").GetComponent<InputField>().text;
        string inputDes = editPlayerUI.transform.Find("newDes").GetComponent<InputField>().text;

        if (inputName != "")
		{
			if (!playerNames.Contains(inputName))
			{
				playerNames.Add(inputName);
				playerNotes.Add (inputDes);
				GameObject temp = Instantiate<GameObject>(roleTextPrefab, roleListContent);
				temp.transform.Find("name").GetComponent<Text>().text = inputName;
                temp.transform.Find("des").GetComponent<Text>().text = inputDes;
                temp.SetActive(true);
				if (Manager.Instace.playerNames == null)
				{
					Manager.Instace.playerNames = new List<string>();
				}
				Manager.Instace.playerNames.Add(inputName);
				if (Manager.Instace.playerNoteDic == null)
				{
					Manager.Instace.playerNoteDic = new Dictionary<string, string> ();
				}
				Manager.Instace.playerNoteDic.Add(inputName, inputDes);
			}
		}
        editPlayerUI.SetActive(false);
	}

    public void DelPlayer(Text nameText)
    {
        string name = nameText.text;
        playerNames.Remove(name);
        Manager.Instace.playerNames.Remove(name);
        string note = Manager.Instace.playerNoteDic[name];
        if (note != null)
        {
            Manager.Instace.playerNoteDic.Remove(name);
            playerNotes.Remove(note);
        }
        Destroy(nameText.transform.parent.gameObject);
    }

    public void OpenAddplayer()
    {
        //float w = addPlayerContent.sizeDelta.x;
        //float h = addPlayerContent.sizeDelta.y;
        //if (h - 183 < 1)
        //{
        //    addPlayerContent.sizeDelta = new Vector2(w, 304);
        //    addPlayerContent.localPosition = new Vector3(0, 121, 0);
        //}
        //else
        //{
        //    addPlayerContent.sizeDelta = new Vector2(w, 183);
        //}
        editPlayerUI.SetActive(true);
    }

    public void OpenAddTask()
    {
        editTaskUI.SetActive(true);
        TaskEditor newTask = editTaskUI.GetComponent<TaskEditor>();
        newTask.AddRoles(true);
    }

    public void EditRole(Text nameText)
    {
        for (int i = 0; i < playerNames.Count; i++)
        {
            if (nameText.text.Equals(playerNames[i]))
            {
                editRoleIndex = i;
                currentEditRole = nameText.transform.parent;
                editPlayerUI.SetActive(true);
                editPlayerUI.transform.Find("newName").GetComponent<InputField>().text = playerNames[i];
                editPlayerUI.transform.Find("newDes").GetComponent<InputField>().text = Manager.Instace.playerNoteDic[playerNames[i]];
                return;
            }
        }
    }

    public void SaveEditRole(Transform editUI)
    {
        Manager.Instace.playerNoteDic.Remove(playerNames[editRoleIndex]);
        Manager.Instace.playerNames[editRoleIndex] = editUI.Find("newName").GetComponent<InputField>().text;
        playerNames[editRoleIndex]= editUI.Find("newName").GetComponent<InputField>().text;
        playerNotes[editRoleIndex]= editUI.Find("newDes").GetComponent<InputField>().text;
        Manager.Instace.playerNoteDic.Add(playerNames[editRoleIndex], playerNotes[editRoleIndex]);
        currentEditRole.transform.Find("name").GetComponent<Text>().text = playerNames[editRoleIndex];
        currentEditRole.transform.Find("des").GetComponent<Text>().text = playerNotes[editRoleIndex];
        editUI.gameObject.SetActive(false);
    }

	public void GetPlayerList()
	{
		roleList.ClearOptions();
		roleList.AddOptions(Manager.Instace.playerNames);
	}

	public void AddAllTask(){
		TaskEditor[] taskEditors = taskListContent.GetComponentsInChildren<TaskEditor> ();
		if (Manager.Instace.tasks == null) {
			Manager.Instace.tasks = new List<Task> ();
		}
		Manager.Instace.tasks.Clear ();
		foreach (TaskEditor item in taskEditors) {
			if(item.gameObject.activeSelf)
				Manager.Instace.tasks.Add (item.GetTask ());
		}
	}
}
