using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetThirdPersonUI :ActionUI{
    //edit by 王梓亦
    SetThirdPerson setThirdPerson;
    SetThirdPersonInforma setThirdPersonInforma;

    public InputField inputField;
    public Dropdown playerList;
    public Toggle isNet;
    public Dropdown actionDropdown;
    public Transform actionlist;
    public GameObject actionItem;
    List<string> actionNames;
    GameObject btn;
    //edit by 王梓亦
    void Do()
    {
        inputField.onEndEdit.AddListener(delegate(string s) { ChangeListener(); });
        print(Manager.Instace.roleChoose);
        btn = Manager.Instace.roleChoose.AddRole(setThirdPerson, Manager.Instace.objectTopic[Manager.Instace.gonggong]);
    }
    //edit by 王梓亦
    void Awake()
    {
        Events.isNetMode = isNet.isOn;
        actionNames = new List<string>();
    }

    //edit by 王梓亦
    public void ChangeListener()
    {
        //setFirstPerson.num = drop.value;
        //setFirstPerson.num = inputField.text;
        setThirdPerson.num = playerList.captionText.text;
        setThirdPersonInforma.ChooseNum = setThirdPerson.num;
        btn.transform.Find("Text").GetComponent<Text>().text = setThirdPerson.num;
    }
    //edit by 王梓亦
    void ChangeListener2()
    {
        //setFirstPerson.task = inputTask.text;
        setThirdPersonInforma.task = setThirdPerson.task;
        btn.transform.Find("Task").GetComponent<Text>().text = setThirdPerson.task;
    }
    public override Action<Main> CreateAction()
    {
        action = new SetThirdPerson();
        actionInforma = new SetThirdPersonInforma(true);
        
        //edit by 王梓亦
        setThirdPersonInforma = (SetThirdPersonInforma)actionInforma;
        setThirdPerson = (SetThirdPerson)action;
        GetStateInfo().actionList.Add(actionInforma);

        GetStateInfo().actionList.Add(actionInforma);
        actionInforma.name = "SetThirdPerson";
        //edit by 王梓亦
        Do();

        return base.CreateAction();
    }

    public override Action<Main> LoadAction(ActionInforma actionInforma)
    {
        SetThirdPersonInforma setThirdPersonInforma = (SetThirdPersonInforma)actionInforma;
        this.actionInforma = actionInforma;
        action = new SetThirdPerson();

        setThirdPerson = (SetThirdPerson)action;
        //this.actionInforma = actionInforma;
        if (setThirdPersonInforma != null)
        {           
            //edit by 王梓亦

            setThirdPerson.isNet = setThirdPersonInforma.isNet;
            isNet.isOn = setThirdPersonInforma.isNet;
            //-----------
        }

        //edit by 王梓亦
        setThirdPerson.num = setThirdPersonInforma.ChooseNum;
        setThirdPerson.task = setThirdPersonInforma.task;
        //inputField.text = setFirstPersonInforma.ChooseNum.ToString();
        //inputTask.text = setFirstPersonInforma.task;
        //edit by 王梓亦
        Do();
        //--------------      

        return action;
    }

    //edit by 王梓亦
    public void SetIsNet(bool isOn)
    {
        setThirdPerson.isNet = isOn;
        setThirdPersonInforma.isNet = isOn;
        Events.isNetMode = isOn;
    }

    public override void Close()
    {
        Manager.Instace.roleChoose.DeleteRole(setThirdPerson);
        base.Close();
    }

    public void GetPlayers()
    {
        if (Manager.Instace.playerNames == null)
        {
            Manager.Instace.playerNames = new List<string>();
        }
        playerList.ClearOptions();
        playerList.AddOptions(Manager.Instace.playerNames);
    }

    public void AddAction(Toggle item)
    {
        if (!actionNames.Contains(item.GetComponentInChildren<Text>().text))
        {
            actionNames.Add(item.GetComponentInChildren<Text>().text);
            GameObject temp = Instantiate(actionItem, actionlist);
            temp.SetActive(true);
            temp.GetComponent<Text>().text = item.GetComponentInChildren<Text>().text;
        }
    }

    public void GetActionList()
    {
        actionDropdown.ClearOptions();
        List<string> actionNames = new List<string>();
        foreach (KeyValuePair<string, SetCurrentStatePersonIdUI> item in Manager.Instace.cStatePersonIdDic)
        {
            actionNames.Add(item.Value.taskNameField.text);
        }
        actionDropdown.AddOptions(actionNames);
    }
}
