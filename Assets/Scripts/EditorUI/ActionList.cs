using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ActionList : MonoBehaviour {
    //private static ActionList _instance;
    //public static ActionList Instance
    //{
    //    get
    //    {
    //        if (_instance == null)
    //        {
    //            _instance=GameObject.Find("ActtionList").GetComponent<ActionList>();
                
    //        }
    //        return _instance;
    //    }
    //}
    private Action<Main> currentAction;
    public State<Main> currentState;
    string currentChooseActionName;
    //List<string> actionNames = new List<string>();
    Dictionary<string, string> actionNames= new Dictionary<string, string>();
    GameObject prefab;
    public Transform parent;

    void Init()
    {
        ActionMsgInit.Init();
  //      //actionNames.Add("TestUIzf", "测试用");
  //      //对动作分类的脚本是 TagAction
  //      actionNames.Add("ChangeColor","改变材质颜色");//三维动作

  //      actionNames.Add("OtherActionWithEvent", "发送消息");//逻辑动作
  //      actionNames.Add("TriggerEvent", "触发器事件");//逻辑动作
  //      actionNames.Add("ColliderEvent", "碰撞器事件");//逻辑动作
  //      actionNames.Add("SetFirstPerson", "设置第一人称");//角色动作
  //      actionNames.Add("PlayAnimation", "播放动画");//三维动作
  //      //actionNames.Add("ShowImage","显示信息");//*********
  //      actionNames.Add("ShowButton", "显示按钮");//平面动作
  //      //actionNames.Add("SetThirdPerson", "设置第三人称");//角色动作
  //      actionNames.Add("PhysicalSetting", "物理设置");//三维动作

  //      actionNames.Add("AddLightReources", "添加灯光");//三维动作
  //      actionNames.Add("ParticleControl", "粒子控制");//三维动作

		//actionNames.Add ("ShowMsg", "激光提示信息");//VR动作
  //      actionNames.Add("PlayAudio", "音效设置");//三维动作
  //      //actionNames.Add("Translate", "物体转换");//三维动作
  //      actionNames.Add("TransColor", "物体变色");//三维动作
  //      actionNames.Add("TransMove", "物体移动");//三维动作
  //      actionNames.Add("TransRotate", "物体旋转");//三维动作
  //      actionNames.Add("TransScale", "物体缩放");//三维动作
  //      actionNames.Add("ShowBtn", "VR按钮");//VR动作
		//actionNames.Add("PCShowMsg", "显示文本信息");//平面动作
  //      actionNames.Add("ShowImg", "显示图片");//平面动作
  //      actionNames.Add("SetVisibility", "设置可见性");//三维动作
  //      actionNames.Add("WaitingSenconds", "设置延迟");//逻辑动作
  //      actionNames.Add("MoveToward", "向目标移动");//三维动作
  //      actionNames.Add("CameraAction", "特写镜头");//角色动作
  //      //actionNames.Add("GetPosition", "获取坐标");//三维动作
		//actionNames.Add ("DragObj", "设为可移动物体");//三维动作
  //      actionNames.Add("MotionTrigger", "划动触发器");//逻辑动作
  //      actionNames.Add("LoadScene", "加载场景");//逻辑动作
		//actionNames.Add ("FollowPlayer", "跟随主角");//角色动作
  //      actionNames.Add("DeleteObj", "销毁物体");//逻辑动作
  //      actionNames.Add("LightObj", "环境光调整");//三维动作

  //      actionNames.Add("CreateRoom", "创建或加入房间");//逻辑动作
  //      //actionNames.Add("JoinRoom", "加入房间");//逻辑动作
  //      actionNames.Add("PIShow", "三维展示");//三维动作
  //      actionNames.Add("ShowVideo", "播放视频");//平面动作
        

  //      actionNames.Add("ChoosePlayer", "选择角色界面");
  //      actionNames.Add("SetCurrentStatePersonId", "设置子任务");
  //      actionNames.Add("SetTask", "设置任务");
  //      actionNames.Add("SetPlayer", "设置角色");
  //      actionNames.Add("WorldText", "3D文本");//三维动作
  //      actionNames.Add("CloseInterface","关闭界面");//平面动作
  //      actionNames.Add("StopAnimation","动画停止");//三维动作
  //      actionNames.Add("SetHighLight","高光描边");//三维动作
  //      actionNames.Add("MoveTarget","移动到目标的位置");//三维动作

  //      actionNames.Add("ShootingMode","射击模式"); //模拟枪械射击
  //      actionNames.Add("SetTransform","设置Transform");
  //      actionNames.Add("ShowWorldVideo","vr视频");
  //      actionNames.Add("FollowMouseMovement","跟随鼠标移动（有限制）");
  //      actionNames.Add("ShowPowerPoint","PPT");
  //      //actionNames.Add("BlurAction","模糊效果");
  //      actionNames.Add("ShowInterface", "添加UI");
        //foreach (string i in actionNames)
        //{
        //    if (prefab == null)
        //    {
        //        prefab = (GameObject)Resources.Load("Prefabs/ActionBtn");
        //    }
        //    GameObject obj = Instantiate(prefab);

        //    obj.transform.SetParent(parent);
        //    obj.transform.localScale = Vector3.one;
        //    obj.name=i;
        //    obj.GetComponent<Button>().onClick.AddListener(delegate() { OnClick(i);});
        //    obj.transform.GetComponentInChildren<Text>().text = i;
        //}

        /// <summary>
        /// modified
        ///  by
        ///  jiangning


        foreach (KeyValuePair<string,string> i in ActionMsgInit.actionNames)        {
            if (prefab == null)
            {
                prefab = (GameObject)Resources.Load("Prefabs/ActionBtn");
            }
            GameObject obj = Instantiate(prefab);

            obj.transform.SetParent(parent);
            obj.transform.localScale = Vector3.one;
            obj.name = i.Key;
            obj.GetComponent<Button>().onClick.AddListener(delegate() { OnClick(i.Key); });
            obj.transform.GetComponentInChildren<Text>().text = i.Value;
        }


        /// </summary>
      







    }

    void Awake()
    {
        Init();
    }
    /// <summary>
    /// 关闭ActionList面板
    /// </summary>
    public void CloseActionList()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 点击按钮一
    /// </summary>
    //public void OnClick()
    //{
    //    currentAction = new ChangeColor();
    //}

    //public void OnClickTwo()
    //{
    //    currentAction = new OtherActionWithEvent();
    //}
    //public void OnClickThree()
    //{
    //    currentAction = new TriggerEvent();
    //    //修改Collider的值
    //    Manager.Instace.gonggong.GetComponent<Collider>().isTrigger = true;
    //}

    //public void OnClickFour()
    //{
    //    currentAction = new SetFirstPerson();
    //}

    //public void OnClickFive()
    //{
    //    currentAction = new PlayAnimation();
    //}

    public  void OnClick(string name)
    {
        currentChooseActionName = name;
    }
    /// <summary>
    /// 将当前动作传递到当前状态中去
    /// </summary>
    /// <param name="s"></param>
    //public void AddActionToState2()
    //{
    //   // currentState.onceActionList.Add(currentAction);
    //   //GameObject changeColor=Manager.Instace.AddChangeColorToActionList();
    //   // //将当前动作传递到ActionUI中去
    //   //changeColor.GetComponent<ActionUI>().SetAction(currentAction);
    //   //changeColor.GetComponent<ActionUI>().SetObject(changeColor);
    //   //currentState.actionUIlist.Add(changeColor.GetComponent<ActionUI>()); 
    //    switch(currentAction.id){
    //        case 1:
    //        currentState.onceActionList.Add(currentAction);
    //        GameObject changeColor = Manager.Instace.AddChangeColorToActionList();
    //        //将当前动作传递到ActionUI中去
    //        changeColor.GetComponent<ActionUI>().SetAction(currentAction);
    //        changeColor.GetComponent<ActionUI>().SetObject(changeColor);
    //        currentState.actionUIlist.Add(changeColor.GetComponent<ActionUI>());
    //            break;
    //        case 2:
    //        currentState.onceActionList.Add(currentAction);
    //        GameObject OtherActionWithEvent = Manager.Instace.AddOtherActionWithEventToActionList();
    //        OtherActionWithEvent.GetComponent<ActionUI>().SetAction(currentAction);
    //        OtherActionWithEvent.GetComponent<ActionUI>().SetObject(OtherActionWithEvent);
    //        currentState.actionUIlist.Add(OtherActionWithEvent.GetComponent<ActionUI>());
    //            break;
    //        case 3:
    //            currentState.updateActionList.Add(currentAction);
    //            GameObject TriggerEvent = Manager.Instace.AddTriggerEventToActionList();
    //            TriggerEvent.GetComponent<TriggerEventUI>().SetAction(currentAction);
    //            TriggerEvent.GetComponent<TriggerEventUI>().SetObject(TriggerEvent);
    //            currentState.actionUIlist.Add(TriggerEvent.GetComponent<TriggerEventUI>());
    //            break;
    //        case 4:
    //                        currentState.onceActionList.Add(currentAction);
    //                        GameObject SetFirstP = Manager.Instace.AddSetFirstPersonToActionList();
    //        SetFirstP.GetComponent<ActionUI>().SetAction(currentAction);
    //        SetFirstP.GetComponent<ActionUI>().SetObject(SetFirstP);
    //        currentState.actionUIlist.Add(SetFirstP.GetComponent<ActionUI>());
    //        break;
    //        case 5:
    //        currentState.onceActionList.Add(currentAction);
    //        GameObject Playanimation = Manager.Instace.AddPlayAnimationToActionList();
    //        Playanimation.GetComponent<PlayAnimationUI>().SetAction(currentAction);
    //        Playanimation.GetComponent<PlayAnimationUI>().SetObject(Playanimation);
    //        currentState.actionUIlist.Add(Playanimation.GetComponent<PlayAnimationUI>());
    //        break;
    //    }

    //}

    public void AddActionToState()
    {
        //获取动作
        GameObject action = Manager.Instace.AddActionToActionList(currentChooseActionName);
        //Debug.LogError(currentState.target);
        //action.GetComponent<ActionUI>().TargetGameObject = currentState.target.gameObject;
        ActionUI actionUi = action.GetComponent<ActionUI>();
        //actionUi.TargetTransform =
        //    Manager.Instace.GetTransformByGongGongPath(actionUi.GetStateInfo().StateTargetGameObjectPath); 
        //Debug.LogError(actionUi.TargetTransform);
        
        actionUi.ActionTargetTransform = StateNode.dict[currentState].GetComponent<StateNode>().TargetTransform;
        //Debug.LogError(actionUi.TargetTransform.name);
        currentAction =action.GetComponent<ActionUI>().CreateAction();
        if (currentAction.isOnce)
        {
            currentState.onceActionList.Add(currentAction);
            //actionInfo = new ActionInforma(true);
        }
        else
        {
            currentState.updateActionList.Add(currentAction);
            //actionInfo = new ActionInforma(false);
        }
        //添加动作到State中去
        
        //stateInfo.actionList.Add(actionInfo);
        //actionInfo.name = currentChooseActionName;

        

        action.GetComponent<ActionUI>().SetObject(action);
        currentState.actionUIlist.Add(action.GetComponent<ActionUI>());
    }
}
