using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Elastic2nd : MonoBehaviour
{
    public  Dictionary<string, string> AccNam = new Dictionary<string, string>();
    public GameObject uiLead;
    public GameObject ContentLead;
    public GameObject UIGG; 

    RectTransform ContentRT;
    Vector2 contentSizedata;
    Vector2 FirstPos;

    public  List<GameObject> childsUI = new List<GameObject>();
    List<GameObject> _RealUIs = new List<GameObject>();
    List<float> UIsPos = new List<float>();

    private GameObject CurrentStateBtn;
    public  List<GameObject> USBs = new List<GameObject>();

    bool _isUICG;

    int lastCount;
    int nowCount;

    // Use this for initialization
    void Awake()
    {
        AccDic();
    }
    public void AccDic()
    {
        AccNam.Clear();
        AccNam.Add("ChangeColor", "改变材质颜色");
        AccNam.Add("OtherActionWithEvent", "发送消息");
        AccNam.Add("TriggerEvent", "触发器事件");
        AccNam.Add("ColliderEvent", "碰撞器事件");
        AccNam.Add("SetFirstPerson", "设置第一人称");
        AccNam.Add("PlayAnimation", "播放动画");
        AccNam.Add("ShowButton", "显示按钮");
        AccNam.Add("SetThirdPerson", "设置第三人称");
        AccNam.Add("PhysicalSetting", "物理设置");

        AccNam.Add("AddLightReources", "添加灯光");
        AccNam.Add("ParticleControl", "粒子控制");

        AccNam.Add("ShowMsg", "激光提示信息");
        AccNam.Add("PlayAudio", "音效设置");
        //AccNam.Add("Translate", "物体转换");
        AccNam.Add("TransColor", "物体变色");
        AccNam.Add("TransMove", "物体移动");
        AccNam.Add("TransRotate", "物体旋转");
        AccNam.Add("TransScale", "物体缩放");
        AccNam.Add("ShowBtn", "VR按钮");
        AccNam.Add("PCShowMsg", "显示文本信息");
        AccNam.Add("ShowImg", "显示图片");
        AccNam.Add("SetVisibility", "设置可见性");

        AccNam.Add("MoveToward", "向目标移动");
        AccNam.Add("CameraAction", "特写镜头");
        AccNam.Add("GetPosition", "获取坐标");

        AccNam.Add("WaitingSenconds", "设置延迟");
        AccNam.Add("DragObj", "设为可移动物体");
        AccNam.Add("MotionTrigger", "滑动触发器");
        AccNam.Add("LoadScene", "加载场景");
        AccNam.Add("FollowPlayer", "跟随主角");
        AccNam.Add("DeleteObj", "销毁物体");
        AccNam.Add("LightObj", "环境光调整");
        AccNam.Add("PIShow", "三维展示");
        AccNam.Add("ShowVideo", "播放视频");
        AccNam.Add("WorldText", "3D文本");
        AccNam.Add("CloseInterface","关闭界面");
        AccNam.Add("StopAnimation","动画停止");
        AccNam.Add("CreateRoom", "创建或加入房间");
        AccNam.Add("ChoosePlayer", "选择角色界面");
        AccNam.Add("SetCurrentStatePersonId", "设置子任务");
        AccNam.Add("SetTask", "设置任务");
        AccNam.Add("SetPlayer", "设置角色");
        AccNam.Add("SetHighLight","高光描边");
        AccNam.Add("MoveTarget","移动到目标位置");

        AccNam.Add("ShootingMode","射击模式");
        AccNam.Add("SetTransform","设置Transform");
        AccNam.Add("ShowWorldVideo","vr视频");
        AccNam.Add("FollowMouseMovement","跟随鼠标移动（有限制）");
        AccNam.Add("ShowPowerPoint","PPT");
        AccNam.Add("BlurAction","模糊效果");
        AccNam.Add("ShowInterface", "添加UI");
        AccNam.Add("Delivery","传送");
    }
    void Start()
    {
        FirstPos = UIGG.GetComponent<RectTransform>().anchoredPosition;
        ContentRT = ContentLead.GetComponent<RectTransform>();
        ContentRT.sizeDelta = new Vector2(0, 92);
        _isUICG = false;
    }

    // Update is called once per frame
    void Update()
    {
        SearchUICount();
        if (Manager.Instace._SSST)
        {
            UIListUpdate();

            UIMove();
            Manager.Instace._SSST = false;
        }
        if (_isUICG)
        {
            UIMove();
        }
    }
    /// <summary>
    /// 初始化列表
    /// </summary>
    public void InitialUIList()
    {
        UIListUpdate();
        for (int i = 0; i < childsUI.Count; i++)
        {
            childsUI[i].SetActive(false);
        }
    }
    /// <summary>
    /// 更新列表
    /// </summary>
    void UIListUpdate()
    {
        childsUI.Clear();
        USBs.Clear();
        for (int i = 0; i < ContentLead.transform.childCount; i++)
        {
            USBs.Add(ContentLead.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < USBs.Count; i++)
        {

            if (USBs[i].name != "TestUIFIEFIE")
            {
                GameObject Uitip = Instantiate(uiLead);
                Uitip.name = "TestUIFIEFIE";
                GameObject childUI = USBs[i];
                Uitip.transform.SetParent(ContentLead.transform);
                Uitip.transform.localScale = Vector3.one;
                Uitip.transform.localPosition = ContentLead.transform.position;
                if (childUI.name.Length > 7)
                {
                    string _accNam = childUI.name.Substring(0, childUI.name.Length - 7);
                    if (AccNam.ContainsKey(_accNam))
                    {
                        Uitip.transform.FindChild("Text").GetComponent<Text>().text = AccNam[_accNam];
                    }

                }
                childUI.transform.SetParent(Uitip.transform);
                childUI.gameObject.SetActive(false);
                childsUI.Add(Uitip);
            }
            if (ContentLead.transform.GetChild(i).name == "TestUIFIEFIE")
            {
                childsUI.Add(USBs[i]);
            }

        }
        ShowNum();
    }
    /// <summary>
    /// 更新高度
    /// </summary>
    void ContenSizeUP()
    {
        _RealUIs.Clear();
        UIsPos.Clear();
        contentSizedata = new Vector2(0, 0);
        float AllUIheight = 0;
        //float childUIPos=0;
        if (childsUI.Count > 0)
        {
            for (int i = 0; i < childsUI.Count; i++)
            {
                if (childsUI[i].activeInHierarchy)
                {
                    _RealUIs.Add(childsUI[i]);
                    UIsPos.Add(AllUIheight);
                   
                    //childUIPos += childsUI[i].GetComponent<RectTransform>().sizeDelta.y;
                    AllUIheight += childsUI[i].GetComponent<RectTransform>().sizeDelta.y;
                    if (childsUI[i].transform.childCount >= 5)
                    {
                        if (childsUI[i].transform.GetChild(4).gameObject.activeInHierarchy)
                        {
                            //childUIPos += childsUI[i].GetComponent<RectTransform>().sizeDelta.y;
                            _RealUIs.Add(childsUI[i].transform.GetChild(4).gameObject);
                            UIsPos.Add(childsUI[i].GetComponent<RectTransform>().sizeDelta.y);
                            AllUIheight += childsUI[i].transform.GetChild(4).GetComponent<RectTransform>().sizeDelta.y;
                        }
                    }               
                }
            }

            contentSizedata.y = AllUIheight + 20;

            ContentRT.sizeDelta = contentSizedata;
        }
        else
        {
            ContentRT.sizeDelta = new Vector2(0, 92);
        }

    }
    /// <summary>
    /// 位移RT
    /// </summary>
    void PosDate()
    {
        for (int i = 0; i < _RealUIs.Count; i++)
        {
            UIPosUP(_RealUIs[i], UIsPos[i]);
        }
    }
    /// <summary>
    /// 定制位移方式
    /// </summary>
    /// <param name="thisUI"></param>UI
    /// <param name="targetPos"></param>UI位置
    void UIPosUP(GameObject thisUI, float targetPos)
    {
        RectTransform rt = thisUI.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(FirstPos.x, FirstPos.y-targetPos);
    }
    /// <summary>
    /// 进行位移
    /// </summary>
    void UIMove()
    {
        UIListUpdate();
        ContenSizeUP();
        PosDate();
        lastCount = _RealUIs.Count;
        //print("last:=" + lastCount);
    }
    /// <summary>
    /// 平滑滑动
    /// </summary>
    void SmoothMove()
    {

    }
    /// <summary>
    /// 判断UI发生变化
    /// </summary>
    /// <returns></returns>
    bool SearchUICount()
    {
        nowCount = 0;
        for (int i = 0; i < ContentLead.transform.childCount; i++)
        {
            if (ContentLead.transform.GetChild(i).gameObject.activeInHierarchy)
            {
                nowCount += 1;
                if (ContentLead.transform.GetChild(i).childCount >= 5)
                {
                    if (ContentLead.transform.GetChild(i).GetChild(4).gameObject.activeInHierarchy)
                    {
                        nowCount += 1;
                    }
                }
            }
        }
        //print("now:=" + nowCount);

        _judgeCC(lastCount, nowCount);

        return _isUICG;
    }
    /// <summary>
    /// 比较返回是否发生变化
    /// </summary>
    /// <param name="last"></param>
    /// <param name="now"></param>
    void _judgeCC(int last, int now)
    {
        if (last == now)
        {
            _isUICG = false;
        }
        if (last != now)
        {
            _isUICG = true;
        }
    }
    /// <summary>
    /// 排序显示下标
    /// </summary>
    void ShowNum()
    {
        int count = 0;
        for (int i = 0; i < ContentLead.transform.childCount; i++)
        {
            if (ContentLead.transform.GetChild(i).gameObject.activeInHierarchy==true)
            {
                count++;
                ContentLead.transform.GetChild(i).GetChild(3).GetComponent<InputField>().text = count.ToString();
                ContentLead.transform.GetChild(i).GetComponent<UIClick>().lastNum = count;
            }
            
        }
    }
    /// <summary>
    /// 通过改动标号排列顺序
    /// </summary>
    /// <param name="tra">改动的对象</param>
    /// <param name="a">改动对象原本的下标</param>
    /// <param name="b">改动对象的目标下标</param>
    public void SortByNum(Transform tra,int a,int b)
    {
        
        Debug.Log(a +" "+ b);
        int activeChildCount=0;
        for (int i = 0; i < ContentLead.transform.childCount; i++)
        {
            if (ContentLead.transform.GetChild(i).gameObject.activeInHierarchy == true)
            {
                activeChildCount++;
            }
        }
        //输入的数字不合法
        if (b>activeChildCount||b<1||a==b)
        {
            //忽略这次改动
            tra.GetComponent<UIClick>().TransPositionError();
            return;
        }
        //改变content下子物体的顺序
        int count = 0;
        for (int i = 0; i < ContentLead.transform.childCount; i++)
        {
            if (ContentLead.transform.GetChild(i).gameObject.activeInHierarchy==true)
            {
                count++;
                if (count == b)
                {
                    tra.SetSiblingIndex(i);
                    
                    
                    ShowNum();
                    UIMove();
                    break;
                }
            }
        }
        tra.GetComponent<UIClick>().ChangeLastNum(b);
        StateInfo stateInfo = (StateInfo)Manager.Instace.dictFromObjectToInforma[CurrentStateBtn.transform.parent.gameObject];
        
        State<Main> state = Manager.Instace.ActionList.GetComponent<ActionList>().currentState;
        //state.onceActionList
        //List<ActionInforma> acList = new List<ActionInforma>();
        //foreach (var item in Manager.Instace.actionInfomaToGameobject)
        //{
        //    acList.Add(item.Key);
            
        //}

        
        ActionUI AULChange = state.actionUIlist[a - 1];
        Action<Main> OALChange = state.onceActionList[a - 1];
        //ActionInforma ALChange = acList[a - 1];
        ActionInforma AIFChange = stateInfo.actionList[a-1];
        
        if (b < a)
        {
            for (int i = a-1; i > b-1; i--)
            {
                //state.actionUIlist
                state.actionUIlist[i] = state.actionUIlist[i-1];
                //state.onceActionList
                state.onceActionList[i] = state.onceActionList[i - 1];

                //acList[i] = acList[i - 1];

                stateInfo.actionList[i] = stateInfo.actionList[i - 1];
                
            }
            state.actionUIlist[b - 1] = AULChange;

            state.onceActionList[b - 1] = OALChange;

            //acList[b - 1] = ALChange;
            stateInfo.actionList[b - 1] = AIFChange;
            //Dictionary<ActionInforma, GameObject> NewAcToObj = new Dictionary<ActionInforma, GameObject>();
            //for (int i = 0; i < acList.Count; i++)
            //{
            //    NewAcToObj.Add(acList[i], Manager.Instace.actionInfomaToGameobject[acList[i]]);
            //}
            
            //Manager.Instace.actionInfomaToGameobject = NewAcToObj;
            
        }
        else if (b>a)
        {
            
            for (int i = a; i < b; i++)
            {
                //state.actionUIlist
                state.actionUIlist[i-1] = state.actionUIlist[i];
                //state.onceActionList
                state.onceActionList[i - 1] = state.onceActionList[i];

                //acList[i - 1] = acList[i];

                stateInfo.actionList[i - 1] = stateInfo.actionList[i];


            }
            state.actionUIlist[b - 1] = AULChange;

            state.onceActionList[b - 1] = OALChange;

            //acList[b - 1] = ALChange;

            stateInfo.actionList[b - 1] = AIFChange;

            //Dictionary<ActionInforma, GameObject> NewAcToObj = new Dictionary<ActionInforma, GameObject>();
            //for (int i = 0; i < acList.Count; i++)
            //{
            //    NewAcToObj.Add(acList[i], Manager.Instace.actionInfomaToGameobject[acList[i]]);
            //}
            //Manager.Instace.actionInfomaToGameobject = NewAcToObj;
        }
       
        
    }
    
    //绑定当前状态按钮
    public void SetCurrentStateBtn(GameObject obj)
    {
        CurrentStateBtn = obj;
    }
}
