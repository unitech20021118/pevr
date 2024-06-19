using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetFirstPersonUI : ActionUI
{
    public InputField inputF;
    /// <summary>
    /// 行走速度输入框
    /// </summary>
    public InputField IF_WalkSpeed;
    /// <summary>
    /// 跑步速度输入框
    /// </summary>
    public InputField IF_RunSpeed;
    /// <summary>
    /// 跳跃速度输入框
    /// </summary>
    public InputField IF_JumpSpeed;
    /// <summary>
    /// 第一人称高度输入框
    /// </summary>
    public InputField IF_FpsHight;
    private string wSpeed = "5";
    private string rSpeed = "10";
    private string jSpeed = "10";
    private string FpsHeight = "1";
    HandForceCtrl hfcL, hfcR;
    float currentForce;
    SetFirstPerson setFirstPerson;
    SetFirstPersonInforma setFirstPersonInforma;

    //edit by 吕存全
    //public InputField inputTask;
    public List<SetCurrentStatePersonIdUI> SelectedActions;
    public InputField inputField;
    public Dropdown playerList;
    public Toggle isNet;
    public Dropdown actionDropdown;
    public Transform actionlist;
    public GameObject actionItem;
    public GameObject NetPanelGameObject;
    List<string> actionNames;
    GameObject btn;

    void Do()
    {
        hfcL = Manager.Instace.ctrllerEventsL.GetComponent<HandForceCtrl>();
        hfcR = Manager.Instace.ctrllerEventsR.GetComponent<HandForceCtrl>();
        //drop.onValueChanged.AddListener(delegate(int a) { ChangeListener(); });
        //inputField.onEndEdit.AddListener(delegate (string s) { ChangeListener(); });
        //inputTask.onEndEdit.AddListener(delegate (string s) { ChangeListener2(); });
        print(Manager.Instace.roleChoose);
        btn = Manager.Instace.roleChoose.AddRole(setFirstPerson, Manager.Instace.objectTopic[Manager.Instace.gonggong]);
    }

    void Awake()
    {
        Events.isNetMode = isNet.isOn;
        actionNames = new List<string>();
    }

    //------------

    void Start()
    {
        hfcL = Manager.Instace.ctrllerEventsL.GetComponent<HandForceCtrl>();
        hfcR = Manager.Instace.ctrllerEventsR.GetComponent<HandForceCtrl>();
    }

    //edit by 吕存全
    public void ChangeListener()
    {
        //setFirstPerson.num = drop.value;
        //setFirstPerson.num = inputField.text;
        setFirstPerson.num = playerList.captionText.text;
        print(setFirstPerson.num);
        setFirstPersonInforma.ChooseNum = setFirstPerson.num;
        print(setFirstPersonInforma.ChooseNum);
        btn.transform.Find("Text").GetComponent<Text>().text = setFirstPerson.num;
    }

    void ChangeListener2()
    {
        //setFirstPerson.task = inputTask.text;
        setFirstPersonInforma.task = setFirstPerson.task;
        btn.transform.Find("Task").GetComponent<Text>().text = setFirstPerson.task;
    }
    //------------

    public override Action<Main> CreateAction()
    {
        action = new SetFirstPerson();
        actionInforma = new SetFirstPersonInforma(true);
        setFirstPersonInforma = (SetFirstPersonInforma)actionInforma;
        setFirstPerson = (SetFirstPerson)action;
        if (setFirstPersonInforma != null)
        {
            setFirstPersonInforma.walkSpeed = float.Parse(IF_WalkSpeed.text);
            setFirstPerson.walkSpeed = float.Parse(IF_WalkSpeed.text);

            setFirstPersonInforma.runSpeed = float.Parse(IF_RunSpeed.text);
            setFirstPerson.runSpeed = float.Parse(IF_RunSpeed.text);

            setFirstPersonInforma.jumpSpeed = float.Parse(IF_JumpSpeed.text);
            setFirstPerson.jumpSpeed = float.Parse(IF_JumpSpeed.text);

            setFirstPersonInforma.fpsHeight = float.Parse(IF_FpsHight.text);
            setFirstPerson.fpsHeight = float.Parse(IF_FpsHight.text);

            //setFirstPersonInforma.
        }
        GetStateInfo().actionList.Add(actionInforma);
        actionInforma.name = "SetFirstPerson";

        //edit by 吕存全
        Do();
        //-----------
        return base.CreateAction();
    }
    public override Action<Main> LoadAction(ActionInforma actionInforma)
    {
        setFirstPersonInforma = (SetFirstPersonInforma)actionInforma;
        this.actionInforma = actionInforma;
        action = new SetFirstPerson();
        setFirstPerson = (SetFirstPerson)action;
        //this.actionInforma = actionInforma;
        if (setFirstPersonInforma != null)
        {
            if (setFirstPersonInforma.walkSpeed > 0)
            {
                IF_WalkSpeed.text = setFirstPersonInforma.walkSpeed.ToString();
                setFirstPerson.walkSpeed = setFirstPersonInforma.walkSpeed;
            }
            else
            {
                setFirstPerson.walkSpeed = float.Parse(IF_WalkSpeed.text);
            }
            if (setFirstPersonInforma.runSpeed > 0)
            {
                IF_RunSpeed.text = setFirstPersonInforma.runSpeed.ToString();
                setFirstPerson.runSpeed = setFirstPersonInforma.runSpeed;
            }
            else
            {
                setFirstPerson.runSpeed = float.Parse(IF_RunSpeed.text);
            }
            if (setFirstPersonInforma.jumpSpeed > 0)
            {
                IF_JumpSpeed.text = setFirstPersonInforma.jumpSpeed.ToString();
                setFirstPerson.jumpSpeed = setFirstPersonInforma.jumpSpeed;
            }
            else
            {
                setFirstPerson.jumpSpeed = float.Parse(IF_JumpSpeed.text);
            }
            if (setFirstPersonInforma.fpsHeight>0)
            {
                IF_FpsHight.text = setFirstPersonInforma.fpsHeight.ToString();
                setFirstPerson.fpsHeight = setFirstPersonInforma.fpsHeight;
            }

            //edit by 吕存全

            setFirstPerson.isNet = setFirstPersonInforma.isNet;
            isNet.isOn = setFirstPersonInforma.isNet;
            //-----------
            //2019-12-9-wzy
            //edit by 吕存全
            setFirstPerson.num = setFirstPersonInforma.ChooseNum;
            setFirstPerson.task = setFirstPersonInforma.task;
            Do();
            //2019-12-9-wzy
            playerList.captionText.text = setFirstPersonInforma.ChooseNum;
            ChangeListener();
            //GetActionList();
        }

        
        //inputField.text = setFirstPersonInforma.ChooseNum.ToString();
        //inputTask.text = setFirstPersonInforma.task;

        
        //--------------
        SetHandForce(setFirstPersonInforma.forceNum);
        return action;
    }
    public void SetHandForce(InputField ipt)
    {
        hfcL.SetForce(float.Parse(ipt.text));
        hfcR.SetForce(float.Parse(ipt.text));
        //设置第一人称作用力
        Manager.Instace.FirstPerson.GetComponent<FPSForce>().forceNum = (float.Parse(ipt.text));
        try
        {
            SetFirstPersonInforma sfp = (SetFirstPersonInforma)actionInforma;
            sfp.forceNum = float.Parse(inputF.text);
        }
        catch
        {

        }
    }
    /// <summary>
    /// 设置第一人称移动速度
    /// </summary>
    public void SetWalkSpeed(InputField walkSpeed)
    {
        if (float.Parse(walkSpeed.text) <= 0)
        {
            walkSpeed.text = wSpeed;
        }
        else
        {
            setFirstPerson.walkSpeed = float.Parse(walkSpeed.text);
            setFirstPersonInforma.walkSpeed = float.Parse(walkSpeed.text);
            wSpeed = walkSpeed.text;
        }


    }
    /// <summary>
    /// 设置第一人称跑动速度
    /// </summary>
    /// <param name="runSpeed"></param>
    public void SetRunSpeed(InputField runSpeed)
    {
        if (float.Parse(runSpeed.text) <= 0)
        {
            runSpeed.text = rSpeed;
        }
        else
        {
            setFirstPerson.runSpeed = float.Parse(runSpeed.text);
            setFirstPersonInforma.runSpeed = float.Parse(runSpeed.text);
            rSpeed = runSpeed.text;
        }

    }
    /// <summary>
    /// 设置第一人称跳跃速度
    /// </summary>
    /// <param name="jumpSpeed"></param>
    public void SetJumpSpeed(InputField jumpSpeed)
    {
        if (float.Parse(jumpSpeed.text) <= 0)
        {
            jumpSpeed.text = jSpeed;
        }
        else
        {
            setFirstPerson.jumpSpeed = float.Parse(jumpSpeed.text);
            setFirstPersonInforma.jumpSpeed = float.Parse(jumpSpeed.text);
            jSpeed = jumpSpeed.text;
        }

    }
    /// <summary>
    /// 设置视角高度
    /// </summary>
    /// <param name="fpsHeight"></param>
    public void SetFpsHeight(InputField fpsHeight)
    {
        if (float.Parse(fpsHeight.text)<=0)
        {
            fpsHeight.text = FpsHeight;
        }
        else
        {
            setFirstPerson.fpsHeight = float.Parse(fpsHeight.text);
            setFirstPersonInforma.fpsHeight = float.Parse(fpsHeight.text);
            FpsHeight = fpsHeight.text;
        }
    }
    public void SetHandForce(float num)
    {
        hfcL = Manager.Instace.ctrllerEventsL.GetComponent<HandForceCtrl>();
        hfcR = Manager.Instace.ctrllerEventsR.GetComponent<HandForceCtrl>();
        hfcL.SetForce(num);
        hfcR.SetForce(num);
        inputF.text = num.ToString();
        //设置第一人称作用力
        Manager.Instace.FirstPerson.GetComponent<FPSForce>().forceNum = num;
    }

    //edit by 吕存全
    public void SetIsNet(bool isOn)
    {
        setFirstPerson.isNet = isOn;
        setFirstPersonInforma.isNet = isOn;
        Events.isNetMode = isOn;
        if (isOn)
        {
            NetPanelGameObject.SetActive(true);
        }
        else
        {
            NetPanelGameObject.SetActive(false);
        }
    }

    public override void Close()
    {
        Manager.Instace.roleChoose.DeleteRole(setFirstPerson);
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
        ChangeListener();
    }

    public void AddAction(Toggle item)
    {
        if (!actionNames.Contains(item.GetComponentInChildren<Text>().text))
        {
            actionNames.Add(item.GetComponentInChildren<Text>().text);
            GameObject temp = Instantiate(actionItem, actionlist);
            temp.SetActive(true);
            temp.GetComponent<Text>().text = item.GetComponentInChildren<Text>().text;
            print(playerList.captionText.text);
            print(Manager.Instace.cStatePersonIdDic[temp.GetComponent<Text>().text]);
            Manager.Instace.cStatePersonIdDic[temp.GetComponent<Text>().text].SetPersonId(playerList.captionText.text);//action注册相应角色
            print("666666666");
            if (SelectedActions == null)
            {
                SelectedActions = new List<SetCurrentStatePersonIdUI>();
            }
            SelectedActions.Add(Manager.Instace.cStatePersonIdDic[temp.GetComponent<Text>().text]);//保存已选子任务的脚本
            Manager.Instace.cStatePersonIdDic.Remove(temp.GetComponent<Text>().text);//删除manager字典中该子任务
        }
    }

    public void GetActionList()
    {
        foreach (Transform item in actionlist)
        {
            if (item.gameObject.activeSelf)
            {
                Destroy(item.gameObject);
            }
        }

        //actionDropdown.ClearOptions ();
        //List<string> actionNames = new List<string>();
        foreach (KeyValuePair<string, SetCurrentStatePersonIdUI> item in Manager.Instace.cStatePersonIdDic)
        {
            //actionNames.Add (item.Value.taskNameField.text);
            GameObject temp = Instantiate<GameObject>(actionItem, actionlist);
            temp.SetActive(true);
            temp.GetComponentInChildren<Text>().text = item.Value.taskNameField.text;
            //2019-12-12-wzy
            if (!setFirstPerson.persontaskDic.ContainsKey(temp.GetComponentInChildren<Text>().text))
            {
                setFirstPerson.persontaskDic.Add(temp.GetComponentInChildren<Text>().text, false);
            }
            if (!setFirstPersonInforma.persontaskDic.ContainsKey(temp.GetComponentInChildren<Text>().text))
            {
                setFirstPersonInforma.persontaskDic.Add(temp.GetComponentInChildren<Text>().text, false);
            }
            temp.GetComponent<Toggle>().isOn = setFirstPersonInforma.persontaskDic[temp.GetComponentInChildren<Text>().text];
        }
        //actionDropdown.AddOptions (actionNames);
    }

    public void AddSubtask(Toggle item)
    {
        if (item.isOn)
        {
            Manager.Instace.cStatePersonIdDic[item.GetComponentInChildren<Text>().text].SetPersonId(playerList.captionText.text);
            print(item.GetComponentInChildren<Text>().text + "@@@@@@@@@@" + playerList.captionText.text);
            setFirstPerson.persontaskDic[item.GetComponentInChildren<Text>().text] = true;
            setFirstPersonInforma.persontaskDic[item.GetComponentInChildren<Text>().text] = true;
        }
        else
        {
            Manager.Instace.cStatePersonIdDic[item.GetComponentInChildren<Text>().text].RemovePersonId(playerList.captionText.text);
            setFirstPerson.persontaskDic[item.GetComponentInChildren<Text>().text] = false;
            setFirstPersonInforma.persontaskDic[item.GetComponentInChildren<Text>().text] = false;
        }
    }
    //------------
}
