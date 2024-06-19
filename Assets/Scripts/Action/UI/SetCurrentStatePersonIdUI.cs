using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetCurrentStatePersonIdUI : ActionUI{
    //public Dropdown dropdown;
	public InputField taskNameField;
    public InputField inputField;
	public Dropdown roleList,taskList;
    public Button saveBtn;
    SetCurrentStatePersonId setCurrentStatePersonId;
    SetCurrentStatePersonIdInforma setCurrentStatePersonIdInforma;

    public override Action<Main> CreateAction()
    {
        setCurrentStatePersonId = new SetCurrentStatePersonId("");
        action = setCurrentStatePersonId;
        actionInforma = new SetCurrentStatePersonIdInforma(true);
        setCurrentStatePersonIdInforma = (SetCurrentStatePersonIdInforma)actionInforma;
        actionInforma.name = "SetCurrentStatePersonId";
        GetStateInfo().actionList.Add(actionInforma);
        setCurrentStatePersonId.isOnce = true;
        inputField.onEndEdit.AddListener(delegate(string a) { Listener(); });
        //
        return base.CreateAction();
    }

    public override Action<Main> LoadAction(ActionInforma actionInforma)
    {
        this.actionInforma = (SetCurrentStatePersonIdInforma)actionInforma;
        setCurrentStatePersonIdInforma = (SetCurrentStatePersonIdInforma)this.actionInforma;
        setCurrentStatePersonId = new SetCurrentStatePersonId(setCurrentStatePersonIdInforma.personId);
        action = setCurrentStatePersonId;

        if (setCurrentStatePersonId.personIds == null)
            setCurrentStatePersonId.personIds = new List<string>();
        foreach (string item in setCurrentStatePersonIdInforma.personIds)
        {
            print(item);
            setCurrentStatePersonId.personIds.Add(item);
        }
        
        //setCurrentStatePersonId.personId = setCurrentStatePersonIdInforma.personId;
        //inputField.text = setCurrentStatePersonIdInforma.personId;
		taskNameField.text = setCurrentStatePersonIdInforma.taskName;
        //2019-11-29-wzy
        RegisterAction();        
        //setCurrentStatePersonId.dictask = setCurrentStatePersonIdInforma.dictask;
        //Manager.Instace.cStatePersonIdDic = setCurrentStatePersonIdInforma.dictask;
        //RegisterAction ();
        //inputField.onEndEdit.AddListener(delegate(string a) { Listener(); });
        return base.LoadAction(actionInforma);

    }

    public void Listener()
    {
		setCurrentStatePersonId.personId = roleList.captionText.text;
        setCurrentStatePersonIdInforma.personId = setCurrentStatePersonId.personId;
    }

	public void SetPersonId(string personName){
        //print("设置任务执行者" + setCurrentStatePersonId.personIds.Count);
        //setCurrentStatePersonId.personId = personName;
        //setCurrentStatePersonIdInforma.personId = personName;
        if (setCurrentStatePersonId.personIds == null)
            setCurrentStatePersonId.personIds = new List<string>();
        //2019-12-6-wzy
        //setCurrentStatePersonId.personIds.Clear();
        //2019-12-12-wzy
        if (!setCurrentStatePersonId.personIds.Contains(personName))
        {
            setCurrentStatePersonId.personIds.Add(personName);
        }
        setCurrentStatePersonIdInforma.personIds = setCurrentStatePersonId.personIds.ToArray();
        //2019-12-6-wzy
        setCurrentStatePersonIdInforma.taskName = taskNameField.text;
    }

    public void RemovePersonId(string personName)
    {
        if (setCurrentStatePersonId.personIds.Contains(personName))
        {
            setCurrentStatePersonId.personIds.Remove(personName);
            setCurrentStatePersonIdInforma.personIds = setCurrentStatePersonId.personIds.ToArray();
        }
        
    }

    public void RegisterAction(){
		if (Manager.Instace.cStatePersonIdDic == null) {
			Manager.Instace.cStatePersonIdDic = new Dictionary<string, SetCurrentStatePersonIdUI> ();
		}
		if (!Manager.Instace.cStatePersonIdDic.ContainsKey (taskNameField.text)) {
			Manager.Instace.cStatePersonIdDic.Add (taskNameField.text, this);
            //setCurrentStatePersonId.dictask = Manager.Instace.cStatePersonIdDic;
            //setCurrentStatePersonIdInforma.dictask = Manager.Instace.cStatePersonIdDic;
            saveBtn.GetComponent<Image>().color = Color.gray;
		}
	}

    public void ChangeSubtask()
    {
        saveBtn.GetComponent<Image>().color = Color.white;
    }
}
