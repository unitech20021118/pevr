using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using LitJson;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.SceneManagement;
using VRTK;
using System;
using System.Net.Sockets;
using System.Text;
using Assets.Scripts.Action.System;
using Assets.Scripts.Login;
using Assets.Scripts.Undo;
using Common;
using HighlightingSystem;
using RecordOfSaveFile;

//using System.Drawing;

public class Manager : MonoBehaviour
{
    private static Manager _instance;

    public static Dictionary<string, string> itemNameList = new Dictionary<string, string>();

    public static Manager Instace
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("Canvas").GetComponent<Manager>();
            }
            return _instance;
        }
    }



    //public bool isChooseEvent = false;
    public Dictionary<string, GameObject> gameobjectList = new Dictionary<string, GameObject>();//管理外部加载的资源
    public Dictionary<string, bool> isloadedPic = new Dictionary<string, bool>();
    public Dictionary<StateInfo, GameObject> statenodeDict = new Dictionary<StateInfo, GameObject>(); //索引状态信息所实例化的物体
    /// <summary>
    /// 当前展示的资源
    /// </summary>
    public List<GameObject> nowLoadGameObject = new List<GameObject>();
    public GameObject Menu;
    private GameObject Event;
    public GameObject StatePanel;
    //===============================SetLight.hyx
    //public GameObject LightSetting;
    public GameObject LineArrow;
    public bool _Playing = false;
    public bool _SSST = false;
    public bool _Open = false;
    public bool _IsAlt = true;
    //edit by kuai
    //public bool _IsPublish = false;
    public bool ISOpen = false;
    public Elastic2nd _E2ND;
    //public GameObject Editor;
    private GameObject StateStart;
    private GameObject temp;//确保编辑物体不被改变
    public GameObject topUI;
    public GameObject gonggong;
    public GameObject lastEditorObject;
    public Main currentMain;
    public GameObject currentStateStart;//当前初始状态
    public GameObject ActionList;

    public GameObject actionlist;
    public GameObject EventListUI;
    public GameObject SystemEventListUI;
    public GameObject ColorPicker;
    //public GameObject EditorPanel;
    public GameObject person;
    public GameObject prefab;//资源图标Item预设
    public GameObject content;
    public GameObject item_GameObject;
    public GameObject objlist;
    bool canMove = true;
    public GameObject EditorCanvas;
    public Camera EditorCamera;
    public GameObject StateMachineCanvas;
    public Camera StateMachineCamera;
    public GameObject backGround1;
    public GameObject backGround2;
    public GameObject backGround3;
    public GameObject AddFSM;
    public Transform parent;

    public Transform mobanparent;
    public Transform imgmobanparent;

    public UniFBXImport ufbxi;
    public GameObject DaoruFbx;

    public GameObject CustomEventlistUI;
    public GameObject CustomEventlistUI2;
    public GameObject NetWorkEventListUI;
    public GameObject SystemEventListUI2;
    public GameObject ChooseEventPanel;
    public List<Events> eventlist = new List<Events>();
    public Dictionary<Events, List<Events>> eventRegisterList = new Dictionary<Events, List<Events>>();
    GameObject OtherActionWithEvent;
    GameObject TriggerEvent;
    GameObject SetFirstPerson;
    /// <summary>
    /// 第一人称控制器预设
    /// </summary>
    public GameObject FirstPerson;
    /// <summary>
    /// 第三人称控制预设
    /// </summary>
    public GameObject ThirdPerson;
    GameObject PlayAnimation;
    GameObject ChangeColor;
    public GameObject Substance;//被替代物
    public FirstPersonController FPC;
    public GameObject ShowImage;
    public GameObject ShowButton;
    public GameObject ChooseImagePanel;
    public ActionList actionList;
    GameObject tempActionEditorPanel;
    public Camera mainCamera;

    public Camera FirstPersonCamera;

    public Camera ActionCamera;
    //public Camera stateMachineCamera;

    Dictionary<string, GameObject> actiondict = new Dictionary<string, GameObject>();
    /// <summary>
    /// 临时保存的鼠标左键选中的物体，主要用于鼠标选中物体或UI里的状态和事件按delete进行删除
    /// </summary>
    public GameObject saveObject;
    public Transform pannel;
    public AllDataInformation allDataInformation;
    JsonData projectJd;//项目数据
    public GameObject prefabState;//状态按钮
    public GameObject prefab11;
    public GameObject prefab22;
    public GameObject prefab33;
    public Transform eventlist2;
    public Dictionary<GameObject, GameObject> objectTopic = new Dictionary<GameObject, GameObject>();
    public GameObject property;
    private bool isProperty;
    public GameObject propertyList;
    public GameObject cube;
    public Dictionary<GameObject, List<GameObject>> objectPropertyDic = new Dictionary<GameObject, List<GameObject>>();
    public Dictionary<GameObject, GameObject> objectToFsm = new Dictionary<GameObject, GameObject>();//物体对Fsm的索引
    public Dictionary<GameObject, Base> dictFromObjectToInforma = new Dictionary<GameObject, Base>();//物体对存储数据的索引
    public Dictionary<ActionInforma, GameObject> actionInfomaToGameobject = new Dictionary<ActionInforma, GameObject>();
    public Dictionary<Variable, GameObject> variableToGameObject = new Dictionary<Variable, GameObject>();
    /// <summary>
    /// 物体与其父物体名字的列表
    /// </summary>
    public List<GoAndParentName> parentNameList = new List<GoAndParentName>();
    /// <summary>
    /// 属性面板物体与子物体的列表
    /// </summary>
    public List<ItemAndChildrenName> childrenNnameList = new List<ItemAndChildrenName>();

    List<AssetBundle> assetList = new List<AssetBundle>();
    public List<Variable> variables = new List<Variable>();
    public GameObject AddAvariable;
    public GameObject typeList;
    public AddProperty addPreperty;
    public VariablePanelUI variablePanelUI;
    public GameObject addStatePanel;

    public GameObject LoginPanel;
    //VR
    //---------------------------------------------------
    /// <summary>
    /// vr相机
    /// </summary>
    public GameObject VRCamera;
    /// <summary>
    /// vr相机的摄像机
    /// </summary>
    public GameObject cameraEye;
    public VRTK_ControllerEvents ctrllerEventsL, ctrllerEventsR;
    public VRTK_Pointer VRPointerL, VRPointerR;
    public GameObject HTCEventListUI;
    /// <summary>
    /// 左控制器
    /// </summary>
    public GameObject LeftControllerGameObject;
    /// <summary>
    /// 右控制器
    /// </summary>
    public GameObject RightControllerGameObject;
    //---------------------------------------------------
    // Use this for initialization
    public GameObject TEST;
    public RolerChoose roleChoose;
    public delegate void Listener();
    public Listener a;

    //For Internet Connect
    //--------------------------------------------------------
    public Socket clientSocket;
    public RequestManager requestMng;
    public ClientManager clientMng;
    public PlayerManager playerMng;

    //edit by 吕存全
    public Dictionary<string, Task> taskDictionary;
    public Dictionary<string, SetCurrentStatePersonIdUI> cStatePersonIdDic;
    public List<Task> tasks;
    public List<string> playerNames;
    public Dictionary<string, string> playerNoteDic;
    //public List<string> playerNotes;

    //edit by 王梓亦
    public string datapath;

    public string dangqianstr;
    public GameObject dangqiangobj;

    //2019-11-25-wzy
    public bool isnet;
    public RuntimeAnimatorController Runimatorcontroller;
    public SteamVR_TrackedObject VRtoukuitongbu;
    public GameObject leftgo;
    public GameObject rightgo;
    /// <summary>
    /// 资源菜单
    /// </summary>
    private GameObject resourceMuneGameObject;
    public bool CanBuild;

    private List<string> resourceUnDeleteMenu;

    public DragUIMoveObjOnGround dragUIMoveObjOnGround;

    /// <summary>
    /// 存档的格式
    /// </summary>
    private string saveFileFormat = ".pevrsf";

    public GameObject SaveType;
    /// <summary>
    /// 定时保存间隔时间
    /// </summary>
    public int IntervalSavingTime = 300;

    public string SaveFileFormat { get { return saveFileFormat; } }

    public GameObject ResourceMuneGameObject
    {
        get
        {
            if (resourceMuneGameObject == null)
            {
                resourceMuneGameObject = Instantiate(Resources.Load<GameObject>("ResourceButtonMenu"));
            }
            return resourceMuneGameObject;
        }
        set { resourceMuneGameObject = value; }
    }
    void Awake()
    {
        datapath = Application.streamingAssetsPath;
        string projectname = "project";
        //string projectname = Camera.main.gameObject.name;
        int num = 1;
        Base project = new Base(num, projectname);
        //Informa<Base>.allData.Add(project);
        dictFromObjectToInforma.Add(cube, project);
        allDataInformation = new AllDataInformation();
        allDataInformation.brushedObjInformation = new Brush.BrushedObjInformation();
        ListTreeNode<Base> root = new ListTreeNode<Base>(project);
        resourceUnDeleteMenu = new List<string>();
        resourceUnDeleteMenu.Add("geometry");
        resourceUnDeleteMenu.Add("animation");
        resourceUnDeleteMenu.Add("scene");
        resourceUnDeleteMenu.Add("terrain");
        resourceUnDeleteMenu.Add("signal");
        resourceUnDeleteMenu.Add("texiao");
        resourceUnDeleteMenu.Add("sky");
        resourceUnDeleteMenu.Add("xiyuanguanglanjishu");
        allDataInformation.listTree = new ListTree<Base>(root);

        Init();
        requestMng = new RequestManager();
        playerMng = new PlayerManager();
        clientMng = new ClientManager();

    }

    public void HandleResponse(ActionCode actionCode, string data)
    {
        requestMng.HandleResponse(actionCode, data);
    }

    public void AddRequest(ActionCode actionCode, BaseRequest request)
    {
        //Debug.LogError(actionCode);
        //Debug.LogError(request);
        if (requestMng == null)
        {
            requestMng = new RequestManager();
        }
        requestMng.AddrequestCode(actionCode, request);

    }

    public void RemoveRequest(ActionCode actionCode)
    {
        requestMng.RemoveactionCode(actionCode);
    }

    public void SendRequest(RequestCode requestCode, ActionCode actionCode, string data)
    {
        clientMng.SendRequest(requestCode, actionCode, data);
    }

    public void SetUserData(UserData ud)
    {
        playerMng.UserData = ud;
    }

    public void SetCurrentRoleType(string id)
    {
        playerMng.SetCurrentRoleId(id);
    }

    public string GetCurrentRoleType()
    {
        return playerMng.GetCurrentRoleId();
    }

    public void SetCurrentRole(GameObject go)
    {
        playerMng.SetCurrentRoleGameObject(go);
    }



    public void SetRemoteRole(GameObject go)
    {
        playerMng.SetRemoteRoleGameObject(go);
    }

    public void SetRemoteRole(string id)
    {
        playerMng.SetRemoteRoleGameObject(id);
    }

    void Start()
    {

        _IsAlt = true;
        // GameObject obj = (GameObject)Resources.Load("Prefabs/Menu");
        //GameObject obj = (GameObject)Resources.Load("Prefabs/Menu");
        StateStart = (GameObject)Resources.Load("Prefabs/Fsm");
        Event = (GameObject)Resources.Load("Prefabs/Event");

        prefab = (GameObject)Resources.Load("Text");
        item_GameObject = (GameObject)Resources.Load("Prefabs/item");
        prefabState = (GameObject)Resources.Load("Prefabs/One");
        prefab11 = (GameObject)Resources.Load("Prefabs/eventEditorBtn");
        prefab22 = (GameObject)Resources.Load("Prefabs/EventBtn");
        prefab33 = (GameObject)Resources.Load("Prefabs/EventBtnEditor");
        Win32Help.SetImeEnable(false);



        StartCoroutine(DoTimeSaving());

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            saveObject = OnePointColliderObject();
            if (saveObject != null)
            {
                Debug.Log(saveObject.name);
            }
            else
            {
                // Debug.Log("为空物体");
            }


        }

        if (Input.GetMouseButtonDown(1))
        {
            AddAvariable.SetActive(true);
            typeList.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Delete) && saveObject != null && !saveObject.name.Equals("State"))
        {
            Debug.Log("DeleteGameobject!");

            GameObject line = null;
            switch (saveObject.name)
            {

                case "Event(Clone)":
                    line = saveObject.GetComponent<EventNode>().line;
                    saveObject.GetComponent<EventNode>().state.eventList
                        .Remove(saveObject.GetComponent<EventNode>().even);
                    Debug.LogError(saveObject.GetComponent<EventNode>().state.eventList.Count);
                    if (saveObject.GetComponentInChildren<Text>().text == "结束事件")
                    {
                        saveObject.GetComponent<EventNode>().state.EndAction = null;
                    }
                    //Debug.LogError("删除事件");
                    break;
                case "One(Clone)":
                    line = saveObject.transform.GetChild(0).GetComponent<StateNode>().line;

                    break;
                //edit by kuai
                //如果选中的是开始状态则不能被删除。
                case "Start":
                    return;
                default:
                    break;
            }
            if (line != null)
            {
                //Base data = Base.FindData(line.GetComponent<GameObjectIndex>().index);
                Base data = dictFromObjectToInforma[line];
                ListTreeNode<Base> parent = allDataInformation.listTree.GetNode(data).parent;
                allDataInformation.listTree.DeleteNode(parent, data);
                line.GetComponent<Line>().events.Cancel();
                Destroy(line);

            }
            //GameObjectIndex gameObjectIndex = saveObject.GetComponent<GameObjectIndex>();
            if (dictFromObjectToInforma.ContainsKey(saveObject))
            {
                ListTreeNode<Base> node = allDataInformation.listTree.GetNode(dictFromObjectToInforma[saveObject]);
                DestroyData(node);
                allDataInformation.listTree.DeleteNode(node.parent, node.data);
            }
            else
            {
                //GameObject obj=saveObject.GetComponent<item>().target;
                GetComponent<G_EditorTarget>().DeleteObject();
                //Debug.Log(obj.name);

            }

            Destroy(saveObject);
            //DestroyData(listTree.GetNode(obj));
            foreach (var obj in dictFromObjectToInforma)
            {
                Debug.LogError("dictFromObjectToInforma:" + obj.Key.name);
            }
        }
    }

    public class jsonmoban
    {
        public string name;
        public string imgpath;
        public string chaniesename;
    }

    //edit by 王梓亦
    public Texture2D ReadByPath(string path)
    {
        try
        {
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            int byteLength = (int)fs.Length;
            byte[] imgBytes = new byte[byteLength];
            fs.Read(imgBytes, 0, byteLength);
            fs.Close();
            fs.Dispose();

            System.Drawing.Image img = System.Drawing.Image.FromStream(new MemoryStream(imgBytes), true);
            Texture2D t2 = new Texture2D(img.Width, img.Height);
            img.Dispose();
            t2.LoadImage(imgBytes);
            t2.Apply();
            return t2;
        }
        catch (Exception e)
        {
            print(path);
            Debug.LogError(e);
            throw;
        }


    }

    Texture2D t2d;

    public IEnumerator ReadByWWW(string path)
    {
        WWW www = new WWW(path);
        yield return www;
        t2d = www.texture;

    }

    public void DES()
    {
        isloadedPic.Clear();
        print("删除");
        for (int i = 0; i < mobanparent.childCount; i++)
        {
            Destroy(mobanparent.GetChild(i).gameObject);
        }
        //for (int i = 0; i < imgmobanparent.childCount; i++)
        //{
        //    Destroy(imgmobanparent.GetChild(i).gameObject);
        //}        
    }

    public void Refresh()
    {
        Init();
        Transform t = imgmobanparent.FindChild("Scroll View/Viewport/Content");
        for (int i = 0; i < t.childCount; i++)
        {
            Destroy(t.GetChild(i).gameObject);
        }
        imgmobanparent.gameObject.SetActive(true);
        StartLoadJson(dangqianstr, dangqiangobj);
    }
    public void Init()
    {
        for (int i = 0; i < mobanparent.childCount; i++)
        {
            Destroy(mobanparent.GetChild(i).gameObject);
        }
        //for (int i = 0; i < imgmobanparent.childCount; i++)
        //{
        //    Destroy(imgmobanparent.GetChild(i).gameObject);
        //} 
        string path = Application.streamingAssetsPath + "/json.txt";
        string neirong = File.ReadAllText(path);
        List<jsonmoban> moban = JsonMapper.ToObject<List<jsonmoban>>(neirong);
        for (int i = 0; i < moban.Count; i++)
        {
            GameObject newmoban = Resources.Load("Btn_moban") as GameObject;//图标
            newmoban.name = "Btn_" + moban[i].name;
            //GameObject newimg = Resources.Load("Img_moban") as GameObject;//面板
            //newimg.name = "Img_" + moban[i].name;
            //GameObject img = Instantiate(newimg);
            //img.transform.GetChild(2).GetComponent<Text>().text = moban[i].chaniesename;
            //img.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(GetComponent<LogWeb>().GoToURLInput);
            //img.transform.SetParent(imgmobanparent);
            //img.transform.GetComponent<RectTransform>().localPosition = new Vector3(-340f, 90f, 0f);
            //img.GetComponent<RectTransform>().localScale = Vector3.one * 0.325f;
            newmoban.GetComponent<ResourcesBtn>().InitResBtn(moban[i].name, moban[i].chaniesename, imgmobanparent.gameObject);
            //newmoban.GetComponent<ResourcesBtn>().ButtonName = moban[i].name;
            //newmoban.GetComponent<ResourcesBtn>().panel = img;
            //newmoban.GetComponent<ResourcesBtn>().panel = imgmobanparent.gameObject;
            //img.SetActive(false);
            //图片
            //Debug.LogError(moban[i].imgpath + ".png");
            Texture2D t2d = ReadByPath(Application.streamingAssetsPath + "/" + moban[i].imgpath + ".png");
            //StartCoroutine(ReadByWWW(Application.streamingAssetsPath + "/" + moban[i].imgpath + ".png"));
            //Texture2D t2d = (Texture2D)Resources.Load(moban[i].imgpath, typeof(Texture2D));
            //print(moban[i].imgpath + "~~~~~~~~~~~~" + t2d.name);
            Sprite s = Sprite.Create(t2d, new Rect(0, 0, t2d.width, t2d.height), new Vector2(0.5f, 0.5f));
            newmoban.transform.GetChild(1).GetComponent<Image>().sprite = s;
            //中文名
            //newmoban.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>().text = moban[i].chaniesename;

            GameObject mb = Instantiate(newmoban);

            mb.transform.SetParent(mobanparent);
            mb.GetComponent<RectTransform>().localScale = Vector3.one;
        }
    }

    public GameObject AddActionToActionList(string actionName)
    {
        if (!actiondict.ContainsKey(actionName))
        {
            GameObject obj = (GameObject)Resources.Load("Prefabs/" + actionName);
            actiondict.Add(actionName, obj);
        }
        tempActionEditorPanel = Instantiate(actiondict[actionName]);
        tempActionEditorPanel.transform.SetParent(actionlist.transform);
        tempActionEditorPanel.transform.localScale = Vector3.one;
        tempActionEditorPanel.transform.localPosition = actionlist.transform.position;
        return tempActionEditorPanel;
    }

    /// <summary>
    /// 显示菜单
    /// </summary>
    /// <param name="currentState"></param>
    public void ShowMenu(Transform currentState, Vector3 temp, GameObject obj)
    {
        Menu.SetActive(true);

        Menu.transform.position = new Vector3(currentState.position.x + 2, currentState.position.y, currentState.position.z);

        //Menu.transform.position = new Vector3(currentState.position.x+2, currentState.position.y, currentState.position.z);
        //Menu.transform.SetParent(obj.transform);
        Menu.GetComponent<Menu>().SetCurrentState(currentState);
    }

    /// <summary>
    /// 创建事件按钮
    /// </summary>
    /// <returns></returns>
    public GameObject CreateEventButton()
    {
        if (Event == null)
        {
            Event = (GameObject)Resources.Load("Prefabs/Event");
        }
        GameObject obj = Instantiate(Event);
        return obj;
    }

    /// <summary>
    /// 将UIGUI从屏幕坐标转成世界坐标
    /// </summary>
    /// <param name="rect"></param>
    /// <param name="obj"></param>
    /// <returns></returns>
    public Vector3 UIScreenToWorldPoint(RectTransform rect, GameObject obj)
    {
        Vector2 pos = UIToScreenPoint(obj);
        Vector3 worldPoint;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rect, pos, Camera.main, out worldPoint))
        {
            return worldPoint;
        }
        return Vector3.zero;
    }

    /// <summary>
    /// 将UIGUI从屏幕坐标转成世界坐标
    /// </summary>
    /// <param name="rect"></param>
    /// <returns></returns>
    public Vector3 UIScreenToWorldPoint(RectTransform rect)
    {
        Vector2 pos = Input.mousePosition;
        Vector3 worldPoint;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rect, pos, Camera.main, out worldPoint))
        {
            return worldPoint;
        }
        return Vector3.zero;
    }

    /// <summary>
    /// 将UGUI转成屏幕坐标
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public Vector2 UIToScreenPoint(GameObject obj)
    {
        float a = obj.transform.position.x;
        float b = obj.transform.position.y;
        return new Vector2(a, b);


    }

    /// <summary>
    /// 显示移动物体的UI
    /// </summary>
    /// <param name="target">移动的目标</param>
    public void SetDragObjUI(Transform target)
    {
        dragUIMoveObjOnGround.Init(target);
        dragUIMoveObjOnGround.gameObject.SetActive(true);
    }
    /// <summary>
    /// 关闭移动物体的UI
    /// </summary>
    public void SetDragObjUI()
    {
        dragUIMoveObjOnGround.gameObject.SetActive(false);
    }

    ///// <summary>
    ///// 获得UI物体
    ///// </summary>
    ///// <returns></returns>
    //public GameObject OnePointColliderObject()
    //{
    //    PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
    //    eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    //    List<RaycastResult> results = new List<RaycastResult>();
    //    EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

    //    if (results.Count > 0)
    //    {
    //        return results[1].gameObject;
    //    }
    //    else
    //    {
    //        return null;
    //    }
    //}
    //public GameObject OnePointColliderObject()
    //{
    //    PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
    //    eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    //    List<RaycastResult> results = new List<RaycastResult>();
    //    EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

    //    if (results.Count > 0)
    //    {
    //        //foreach (GameObject i in results)
    //        //{
    //        //    i.tag = "zsgc";
    //        //    return results[1].gameObject;
    //        //}
    //        //o
    //        for (int i = 0; i < results.Count; i++)
    //        {
    //            if (results[i].gameObject.tag == "zsgc")
    //                return results[i].gameObject;
    //        }
    //        return null;
    //    }
    //    else
    //    {
    //        return null;
    //    }
    //}


    public GameObject OnePointColliderObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        if (results.Count > 0)
        {
            //foreach (GameObject i in results)
            //{
            //    i.tag = "zsgc";
            //    return results[1].gameObject;
            //}
            //o
            for (int i = 0; i < results.Count; i++)
            {
                if (results[i].gameObject.tag == "zsgc")
                    return results[i].gameObject;
            }
            return null;
        }
        else
        {
            return null;
        }
    }
    /// <summary>
    /// 发送状态
    /// </summary>
    /// <param name="s"></param>
    public void SendState(State<Main> s, StateNode sn)
    {
        StatePanel.GetComponent<StatePanelUI>().SetName(s, sn);
    }

    public void AddDelegate(Listener listener)
    {
        a += listener;
    }

    /// <summary>
    /// 开始编辑
    /// </summary>
    public void BeginEditor()
    {
        //gonggong = temp;
        //Editor.SetActive(false);
        if (StateStart == null)
        {
            StateStart = (GameObject)Resources.Load("Prefabs/Fsm");
        }

        currentStateStart = Instantiate(StateStart);

        currentStateStart.transform.SetParent(StateMachineCanvas.transform);
        currentStateStart.transform.localPosition = Vector3.zero;
        currentStateStart.transform.localScale = Vector3.one;
        AddMainToObject();
        currentMain.Fsm = currentStateStart;
        currentMain.Fsm.name = "FSM:" + gonggong.name;
        EditorCanvas.SetActive(true);

        //将该状态所依附的物体信息写到statenode里
        StateNode stateNode = currentStateStart.GetComponentInChildren<StateNode>();
        stateNode.TargetTransform = gonggong.transform;

        objectToFsm.Add(gonggong, currentStateStart);
        //记录FSM
        FSMInfo FsmData = new FSMInfo(currentMain.Fsm.name);
        //currentStateStart.AddComponent<GameObjectIndex>().index = FsmData.index;
        //Informa<Base>.allData.Add(FsmData);
        dictFromObjectToInforma.Add(currentStateStart, FsmData);
        ListTreeNode<Base> parent = allDataInformation.listTree.GetNode(dictFromObjectToInforma[gonggong]);
        allDataInformation.listTree.AddLeave(parent, FsmData);

        //记录StartState
        StateInfo stateData = new StateInfo(true);
        //stateData.StateTargetGameObjectPath = GetGongGongPath(gonggong.transform);
        //stateData.StateTargetGameObjectPath = gonggong.name;
        //currentStateStart.transform.GetChild(0).gameObject.AddComponent<GameObjectIndex>().index = stateData.index;
        currentStateStart.transform.GetChild(0).transform.position = ScreenPointToWorldPos(StateMachineCanvas.GetComponent<RectTransform>(), StateMachineCamera);
        //Informa<Base>.allData.Add(stateData);
        dictFromObjectToInforma.Add(currentStateStart.transform.GetChild(0).gameObject, stateData);
        ListTreeNode<Base> stateParent = allDataInformation.listTree.GetNode(FsmData);
        allDataInformation.listTree.AddLeave(stateParent, stateData);


        backGround3.SetActive(true);
        backGround3.transform.GetChild(0).GetComponent<Text>().text = gonggong.name + ":FSM";
        backGround2.SetActive(false);
        AddFSM.SetActive(false);
    }

    /// <summary>
    /// 将Main脚本添加到Object上
    /// </summary>
    public void AddMainToObject()
    {
        currentMain = gonggong.AddComponent<Main>();
        currentStateStart.GetComponentInChildren<StateNode>().Initial();
        currentMain.SetStart(currentStateStart.GetComponentInChildren<StateNode>().state);
        currentMain.enabled = false;
    }

    /// <summary>
    /// 设置运行按钮
    /// </summary>
    public void SetMainActive()
    {
        //在运行前清除选中物体遗留的高光
        transform.GetComponent<G_EditorTarget>().GetOriginColor();
        transform.GetComponent<G_EditorTarget>().GetOriginColorPI();
        //Debug.LogError ("111@@@@@@@");
        ChangePlaying(true);
        //this.transform.GetComponent<G_MouseListener>().ChangePlaying = true;

        //edit by kuai
        //this.transform.GetComponent<G_EditorTarget>().EditPanel.transform.position = new Vector3(2000, 2000, 2000);
        //this.transform.GetComponent<G_EditorTarget>().ScalePanel.transform.position = new Vector3(2000, 2000, 2000);
        //this.transform.GetComponent<G_EditorTarget>().RotatePanel.transform.position = new Vector3(2000, 2000, 2000);
        if (LoadManager.Instance.IsPublish == true)
        {
            topUI.SetActive(false);
        }
        foreach (KeyValuePair<Events, List<Events>> kvp in eventRegisterList)
        {
            for (int i = kvp.Value.Count - 1; i >= 0; i--)
            {
                kvp.Value[i].RegisterTwo(kvp.Key);
            }
        }
        Main[] mains = parent.GetComponentsInChildren<Main>();
        for (int i = 0; i < mains.Length; i++)
        {
            mains[i].enabled = true;
        }
        //foreach (Transform i in parent)
        //{
        //    if (i.GetComponent<Main>() != null)
        //    {
        //        i.GetComponent<Main>().enabled = true;
        //    }
        //}
        //是图标隐藏
        foreach (Transform t in transform)
        {
            if (t.gameObject.activeSelf && t.tag != "run")
            {
                t.gameObject.SetActive(false);
            }
        }

        //edit by 吕存全
        Dictionary<GameObject, GameObject>.ValueCollection valueColl = objectToFsm.Values;
        Debug.Log(objectToFsm.Count);
        foreach (GameObject go in valueColl)
        {
            go.SetActive(true);
        }
        //-------------

        SetCameraConfiguration();
        SetUnActiveEditorFunction();

    }

    //void SetCameraConfiguration()
    //{
    //    mainCamera.rect = new Rect(0, 0, 1, 1);
    //    stateMachineCamera.depth = -2f;
    //    editorCamera.depth = -2f;
    //}

    /// <summary>
    /// 修改运行状态
    /// </summary>
    public void ChangePlaying(bool value)
    {
        _Playing = value;

        transform.GetComponent<G_MouseListener>().ChangePlaying = value;
    }
    void SetCameraConfiguration()
    {
        mainCamera.rect = new Rect(0, 0, 1, 1);
        //if (FirstPerson.activeSelf)
        //{
        //    mainCamera.gameObject.SetActive(false);
        //}

        StateMachineCamera.gameObject.SetActive(false);
        //StateMachineCanvas.gameObject.SetActive(false);
        EditorCamera.gameObject.SetActive(false);
        EditorCanvas.gameObject.SetActive(false);
    }

    void SetUnActiveEditorFunction()
    {
        gameObject.GetComponent<G_MoveObject>().enabled = false;
        gameObject.GetComponent<G_EditorTarget>().rotationCneter.SetActive(false);
        gameObject.GetComponent<G_EditorTarget>().positionCenter.SetActive(false);
        gameObject.GetComponent<G_EditorTarget>().scaleCenter.SetActive(false);
        gameObject.GetComponent<G_EditorTarget>().enabled = false;
        //gameObject.GetComponent<G_MouseListener>().enabled = false;

    }

    public void DaoruFBX()
    {
        DaoruFbx.SetActive(true);
    }

    class js
    {
        string name;
        string modelpath;
        string name2;
    }
    Coroutine loadJsonCoroutine;
    public void StartLoadJson(string name, GameObject panel)
    {
        if (loadJsonCoroutine != null)
        {
            StopCoroutine(loadJsonCoroutine);
        }
        loadJsonCoroutine = StartCoroutine(DoLoadJson(name, panel));
    }
    private IEnumerator DoLoadJson(string name, GameObject panel)
    {
        nowLoadGameObject.Clear();
        //edit by 王梓亦
        string paths = datapath + "/" + name + ".txt";
        string pngpath = Application.streamingAssetsPath + "/ink2/";
        string data = File.ReadAllText(paths);
        JsonData charactersJd = JsonMapper.ToObject(data);
        //TextAsset ta = Resources.Load<TextAsset>(name);
        //JsonData charactersJd = JsonMapper.ToObject(ta.text);
        JsonData character = charactersJd[name];
        //if (Directory.Exists(Application.streamingAssetsPath + "/ink2/" + name))
        //{
        //    pngpath = Application.streamingAssetsPath + "/ink2/" + name + "/";
        //}
        foreach (JsonData characterJd in character)
        {
            //Debug.Log("OOOOOOOOOOOOO");
            //Debug.Log(characterJd["name"]);
            //Debug.Log(characterJd["modelpath"]);
            //Debug.Log(characterJd["isLoaded"]);
            //Debug.Log(characterJd["name2"]);
            ////``````````````````````````````````````````````````````````/
            pngpath = "/ink2/" + name + "/";
            itemNameList[characterJd["name"].ToString()] = characterJd["name2"].ToString();
            if (prefab == null)
            {
                prefab = (GameObject)Resources.Load("Text");
            }
            GameObject item = Instantiate(prefab);
            Transform t = panel.GetComponentInChildren<GridLayoutGroup>().transform;
            item.transform.SetParent(t);
            item.transform.localScale = Vector3.one;
            item.transform.localPosition = new Vector3(item.transform.position.x, item.transform.position.y, 0f);

            //Texture2D t2d = (Texture2D)Resources.Load("ink2/" + characterJd["name"], typeof(Texture2D));

            if (!File.Exists(Application.streamingAssetsPath + pngpath + characterJd["name"] + ".png"))
            {
                pngpath = "/ink2/";
            }
            Texture2D t2d = ReadByPath(Application.streamingAssetsPath + pngpath + characterJd["name"] + ".png");

            string imagePath = pngpath + characterJd["name"] + ".png";//图片路径

            //Texture2D t2d = ReadByPath(Application.streamingAssetsPath + "/ink2/" + characterJd["name"] + ".png");

            //StartCoroutine(ReadByWWW(Application.streamingAssetsPath + "/ink2/" + characterJd["name"] + ".png"));

            Sprite s = Sprite.Create(t2d, new Rect(0, 0, t2d.width, t2d.height), new Vector2(0.5f, 0.5f));
            item.transform.GetChild(1).GetComponent<Image>().sprite = s;
            item.transform.GetChild(2).GetComponent<Text>().text = characterJd["name2"].ToString();
            Color color = item.GetComponentInChildren<Image>().color;
            item.GetComponentInChildren<Image>().color = new Color(color.r, color.g, color.b, 255f);
            //print(characterJd["modelpath"].ToString());
            item.GetComponent<Button>().onClick.AddListener(delegate () { StartCoroutine(CreateGameObj(name, characterJd["modelpath"].ToString(), characterJd["name"].ToString(), imagePath)); });

            item.GetComponent<EventTrigger>().triggers[0].callback.AddListener(delegate
            {
                if (Input.GetMouseButtonUp(1))
                {
                    Debug.Log("鼠标右键点击了");
                    //打开右键菜单
                    if (!resourceUnDeleteMenu.Contains(name))
                    {
                        OpenResourcesMenu(name, characterJd["name"].ToString());
                    }

                }
            });
            //item.GetComponent<Button>().onClick.AddListener(delegate() { CreateGameObj(name, characterJd["modelpath"].ToString(), characterJd["name"].ToString(), imagePath); });
            nowLoadGameObject.Add(item);
            yield return null;
        }
        //isloadedPic.Add(name, true);
    }
    public void LoadJson(string name, GameObject panel)
    {

        nowLoadGameObject.Clear();
        //edit by 王梓亦
        string paths = datapath + "/" + name + ".txt";
        string pngpath = Application.streamingAssetsPath + "/ink2/";
        string data = File.ReadAllText(paths);
        JsonData charactersJd = JsonMapper.ToObject(data);
        //TextAsset ta = Resources.Load<TextAsset>(name);
        //JsonData charactersJd = JsonMapper.ToObject(ta.text);
        JsonData character = charactersJd[name];
        //if (Directory.Exists(Application.streamingAssetsPath + "/ink2/" + name))
        //{
        //    pngpath = Application.streamingAssetsPath + "/ink2/" + name + "/";
        //}
        foreach (JsonData characterJd in character)
        {
            //Debug.Log("OOOOOOOOOOOOO");
            //Debug.Log(characterJd["name"]);
            //Debug.Log(characterJd["modelpath"]);
            //Debug.Log(characterJd["isLoaded"]);
            //Debug.Log(characterJd["name2"]);
            ////``````````````````````````````````````````````````````````/
            pngpath = "/ink2/" + name + "/";
            itemNameList[characterJd["name"].ToString()] = characterJd["name2"].ToString();
            if (prefab == null)
            {
                prefab = (GameObject)Resources.Load("Text");
            }
            GameObject item = Instantiate(prefab);
            Transform t = panel.GetComponentInChildren<GridLayoutGroup>().transform;
            item.transform.SetParent(t);
            item.transform.localScale = Vector3.one;
            item.transform.localPosition = new Vector3(item.transform.position.x, item.transform.position.y, 0f);

            //Texture2D t2d = (Texture2D)Resources.Load("ink2/" + characterJd["name"], typeof(Texture2D));

            if (!File.Exists(Application.streamingAssetsPath + pngpath + characterJd["name"] + ".png"))
            {
                pngpath = "/ink2/";
            }
            Texture2D t2d = ReadByPath(Application.streamingAssetsPath + pngpath + characterJd["name"] + ".png");

            string imagePath = pngpath + characterJd["name"] + ".png";//图片路径

            //Texture2D t2d = ReadByPath(Application.streamingAssetsPath + "/ink2/" + characterJd["name"] + ".png");

            //StartCoroutine(ReadByWWW(Application.streamingAssetsPath + "/ink2/" + characterJd["name"] + ".png"));

            Sprite s = Sprite.Create(t2d, new Rect(0, 0, t2d.width, t2d.height), new Vector2(0.5f, 0.5f));
            item.transform.GetChild(1).GetComponent<Image>().sprite = s;
            item.transform.GetChild(2).GetComponent<Text>().text = characterJd["name2"].ToString();
            Color color = item.GetComponentInChildren<Image>().color;
            item.GetComponentInChildren<Image>().color = new Color(color.r, color.g, color.b, 255f);
            //print(characterJd["modelpath"].ToString());
            item.GetComponent<Button>().onClick.AddListener(delegate () { StartCoroutine(CreateGameObj(name, characterJd["modelpath"].ToString(), characterJd["name"].ToString(), imagePath)); });

            item.GetComponent<EventTrigger>().triggers[0].callback.AddListener(delegate
            {
                if (Input.GetMouseButtonUp(1))
                {
                    Debug.Log("鼠标右键点击了");
                    //打开右键菜单
                    if (!resourceUnDeleteMenu.Contains(name))
                    {
                        OpenResourcesMenu(name, characterJd["name"].ToString());
                    }

                }
            });
            //item.GetComponent<Button>().onClick.AddListener(delegate() { CreateGameObj(name, characterJd["modelpath"].ToString(), characterJd["name"].ToString(), imagePath); });
            nowLoadGameObject.Add(item);
        }
        //isloadedPic.Add(name, true);

    }
    private Coroutine cor;
    /// <summary>
    /// 打开资源菜单
    /// </summary>
    private void OpenResourcesMenu(string type, string name)
    {
        ResourceMuneGameObject.transform.SetParent(transform);
        ResourceMuneGameObject.SetActive(true);
        Image progreImage = ResourceMuneGameObject.transform.Find("Delete/Progress").GetComponent<Image>();
        progreImage.fillAmount = 0f;
        ResourceMuneGameObject.GetComponent<RectTransform>().position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        var deleteEventDown = ResourceMuneGameObject.transform.Find("Delete").GetComponent<EventTrigger>().triggers[0];
        var deleteEventUp = ResourceMuneGameObject.transform.Find("Delete").GetComponent<EventTrigger>().triggers[1];
        Button cancelButton = ResourceMuneGameObject.transform.Find("Cancel").GetComponent<Button>();
        deleteEventDown.callback.RemoveAllListeners();
        deleteEventUp.callback.RemoveAllListeners();
        cancelButton.onClick.RemoveAllListeners();
        deleteEventDown.callback.AddListener(delegate
        {
            Debug.Log("鼠标按下");
            cor = StartCoroutine(LongPress(progreImage, type, name));
        });
        deleteEventUp.callback.AddListener(delegate
        {
            Debug.Log("鼠标松开");
            StopCoroutine(cor);
            progreImage.fillAmount = 0f;
        });
        cancelButton.onClick.AddListener(delegate
        {
            ResourceMuneGameObject.SetActive(false);
        });
    }

    private IEnumerator LongPress(Image progressImage, string type, string name)
    {
        float longPressTime = 2f;
        float pressTime = 0f;
        while (pressTime < longPressTime)
        {
            pressTime += Time.deltaTime;
            progressImage.fillAmount = pressTime / longPressTime;
            yield return null;
        }
        Debug.Log("删除");
        string jsonStr =
            File.ReadAllText(Application.streamingAssetsPath + "/" + type + ".txt",
                Encoding.Unicode);
        JsonData jsonData = JsonMapper.ToObject(jsonStr);
        JsonData character = jsonData[type];
        foreach (JsonData data in character)
        {
            if (data["name"].ToString() == name)
            {
                //从文本文件中移除该模型的地址
                character.ValueAsArray().Remove(data);

                break;
            }
        }

        //character.ValueAsArray().Remove(xxx);
        jsonData[type] = character;
        jsonStr = JsonMapper.ToJson(jsonData);
        jsonStr = Encoding.Unicode.GetString(Encoding.Unicode.GetBytes(jsonStr));
        File.WriteAllText(Application.streamingAssetsPath + "/" + type + ".txt", jsonStr, Encoding.GetEncoding("Unicode"));

        //删除该模型的模型和预览图
        if (File.Exists(TheTools.Tools.Instance.GetAssteBundlesPath() + "/" + type + "/" + name + ".3dpro"))
        {
            File.Delete(TheTools.Tools.Instance.GetAssteBundlesPath() + "/" + type + "/" + name + ".3dpro");
        }
        if (File.Exists(Application.streamingAssetsPath + "/ink2/" + name + ".png"))
        {
            File.Delete(Application.streamingAssetsPath + "/ink2/" + name + ".png");
        }
        else if (File.Exists(Application.streamingAssetsPath + "/ink2/" + name + ".jpg"))
        {
            File.Delete(Application.streamingAssetsPath + "/ink2/" + name + ".jpg");
        }


        //刷新商店数据
        ShopManager.Instance.DeleteIdName(name, type);
        //刷新资源面板
        RefershResourcePanel();
        ResourceMuneGameObject.SetActive(false);



    }

    /// <summary>
    /// 刷新资源面板
    /// </summary>
    public void RefershResourcePanel()
    {
        ResourcesBtn.lastBtn.Close();
        ResourcesBtn.lastBtn.OnClick(ResourcesBtn.lastBtn.ButtonName);
    }

    public IEnumerator CreateGameObj(string nam, string modelpath, string name, string imagepath)
    {
        //Debug.LogError("666");
        while (EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0))
            {
                yield break;
            }
            yield return null;
        }
        //Debug.Log(nam);
        //Debug.Log(modelpath);
        //Debug.Log(name);
        //Debug.Log(imagepath);
        //void  CreateGameObj(string nam,string modelpath,string name,string imagepath){
        //=======================================================To Set Sky
        //todo skybox
        if (nam.Equals("sky"))
        {
            //AssetBundle assets = AssetBundle.LoadFromFile(modelpath);
            //GameObject obj = assets.LoadAsset<GameObject>(name);
            Material skystyle;
            skystyle = Resources.Load<Material>(modelpath);
            RenderSettings.skybox = skystyle;
        }
        //=======================================================To Set Sky
        else
        {
            if (!gameobjectList.ContainsKey(name))
            {
                //Debug.LogError("create");
                string s = string.Empty;
                string[] sarr = modelpath.Split('/');
                for (int i = 0; i < sarr.Length - 1; i++)
                {
                    s += sarr[i] + "/";
                }
                string houzhui = sarr[sarr.Length - 1].Split('.')[1];
                string assetsname = s + sarr[sarr.Length - 1].Split('.')[0];
                if (houzhui == "3dpro")
                {
                    print(modelpath);

                    AssetBundle assets = AssetBundle.LoadFromFile(modelpath);
                    //print(assets.name);
                    string[] ssr = assets.name.Split('.');
                    //print(assets.LoadAsset<GameObject>(ssr[0]));
                    GameObject obj = assets.LoadAsset<GameObject>(ssr[0]);//资源加载到内存
                    //print(obj.transform.position);
                    // obj.transform.GetChild(0).gameObject.AddComponent<CapsuleCollider>();//在物体的自物体上添加碰撞体
                    // obj.AddComponent<BoxCollider>();
                    assetList.Add(assets);
                    obj.name = name;
                    //改变Json文件中的数值
                    gameobjectList.Add(name, obj);//对加载的资源进行管理
                }
                else if (houzhui == "FBX" || houzhui == "fbx")
                {

                    string ssname = sarr[sarr.Length - 1].Split('.')[0];
                    ufbxi.setting.paths.SetModelPath(s, ssname);
                    print(modelpath);
                    yield return ufbxi.Load();
                    //ufbxi.GetObject().name = name;
                    //gameobjectList.Add(name, ufbxi.GetObject());
                    GameObject obj = ufbxi.GetObject();
                    print(obj);
                    obj.name = name;
                    obj.transform.localScale = new Vector3(100, 100, 100);
                    //改变Json文件中的数值
                    gameobjectList.Add(name, obj);
                    Destroy(ufbxi.GetObject());
                }

            }
            GameObject temp;
            ObjectInfo objectData;
            if (nam.Equals("scene") || nam.Equals("terrain"))
            {
                temp = GetComponent<G_CreateObject>().CreateGameObject(gameobjectList[name], false, name, imagepath);
                objectData = new ObjectInfo(modelpath, imagepath, temp.transform, temp.name, true);
            }
            else
            {
                temp = GetComponent<G_CreateObject>().CreateGameObject(gameobjectList[name], true, name, imagepath);
                objectData = new ObjectInfo(modelpath, imagepath, temp.transform, temp.name, false);
            }
            dictFromObjectToInforma.Add(temp, objectData);
            //temp.AddComponent<GameObjectIndex>().index = objectData.index;
            //Informa<Base>.allData.Add(objectData);
            //ListTreeNode<Base> parent=listTree.GetNode(Base.FindData(1));
            ListTreeNode<Base> parent = allDataInformation.listTree.Root;
            allDataInformation.listTree.AddLeave(parent, objectData);

            //添加Undo功能 EditBy 潘晓峰
            var objInfo = new CreateUndoObjInfo()
            {
                Tag = nam,
                ModelPath = modelpath,
                Name = name,
                ImagePath = imagepath,
            };
            if (temp.GetComponent<CreateObjUndoComponent>() == null)
            {
                var undoComponent = temp.AddComponent<CreateObjUndoComponent>();
                undoComponent.Init(objInfo);
                Debug.Log("CreteGamObject ：" + temp);
            }
        }
    }

    public Vector3 ScreenPointToWorldPos(RectTransform rect, Camera camera)
    {
        Vector3 worldPoint;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rect, Input.mousePosition, camera, out worldPoint))
        {
            return worldPoint;
        }
        return Vector3.zero;
    }

    //添加状态
    public void AddState()
    {
        if (prefabState == null)
        {
            prefabState = (GameObject)Resources.Load("Prefabs/One");
        }
        GameObject obj = Instantiate(prefabState);
        obj.transform.SetParent(currentStateStart.transform);
        obj.transform.localScale = Vector3.one;
        obj.transform.localPosition = Vector3.zero;
        transform.parent.gameObject.SetActive(false);
    }

    //显示当前对象物体的状态栏
    public void IsCurrentEditorObjContainFSm()
    {
        if (gonggong.GetComponent<Main>() == null)
        {
            backGround2.SetActive(true);
            backGround3.SetActive(false);
            backGround1.SetActive(false);
        }
        else
        {
            backGround3.SetActive(true);
            backGround1.SetActive(false);
            backGround2.SetActive(false);
            gonggong.GetComponent<Main>().Fsm.SetActive(true);
            backGround3.transform.GetChild(0).GetComponent<Text>().text = gonggong.name + ":FSM";
            //lastEditorObject.GetComponent<Main>().Fsm.SetActive(false);
        }
    }

    public void SetEditorObjectTarget(GameObject target)
    {
        if (gonggong == null)
        {
            gonggong = target;
        }
        else
        {
            if (gonggong != target)
            {
                lastEditorObject = gonggong;

                if (lastEditorObject.GetComponent<Main>() != null)
                {
                    gonggong.GetComponent<Main>().Fsm.SetActive(false);
                }
                gonggong = target;
            }
        }
        IsCurrentEditorObjContainFSm();
    }

    public void SetEditorObjectProperty(GameObject target)
    {
        //if (gonggong == null)
        //{
        //    //影藏
        //}
        //else
        //{
        if (gonggong != target)
        {
            //切换第二个物体
            gonggong = target;
            if (objectPropertyDic.ContainsKey(gonggong))
            {
                if (objectPropertyDic[gonggong].Count > 0)
                {
                    List<GameObject> list = objectPropertyDic[gonggong];
                    foreach (GameObject g in list)
                    {
                        g.SetActive(true);
                    }
                }
            }
            if (objectPropertyDic.ContainsKey(target))
            {
                if (objectPropertyDic[target].Count > 0)
                {
                    List<GameObject> list = objectPropertyDic[target];
                    foreach (GameObject g in list)
                    {
                        g.SetActive(false);
                    }
                }
            }

        }

    }

    public void OpenNewScene()
    {
        //Application.LoadLevel(Application.loadedLevel);
        //显示窗口，判断是否需要保存当前项目
        foreach (AssetBundle a in assetList)
        {
            a.Unload(false);
        }
        StateNode.dict.Clear();
        //GameObjectIndex.GameObjectList.Clear();
        Product.dict.Clear();
        Product.removeList.Clear();
        ProductType.productTypeList.Clear();
        Base.id = 0;
        SceneCtrl.CreateNewScene = true;
        RecordSaveFile.record = null;
        SceneManager.LoadScene("BBT");
    }

    IEnumerator DoTimeSaving()
    {
        while (true)
        {
            yield return new WaitForSeconds(IntervalSavingTime);
            if (RecordSaveFile.SaveJudgementPrompt())
            {
                SaveToFile(Application.dataPath + "/SaveFile/automaticSF.pevrsf");
            }
        }
    }

    /// <summary>
    /// 存储
    /// </summary>
    /// <param name="automatic">是否自动存储</param>
    public void Save(bool automatic, bool loadScene = false)
    {
        //如果是发布后的不需要存储
        if (LoadManager.Instance.IsPublish == true)
        {
            return;
        }
        //记录物体的scale,pos,rotate
        allDataInformation.listTree.Root.data.GetCameraTransform();
        List<ListTreeNode<Base>> objects = allDataInformation.listTree.Root.children;
        foreach (ListTreeNode<Base> obj in objects)
        {
            obj.data.GetTransform();
            ObjectInfo objectInfo = (ObjectInfo)obj.data;
            if (!objectInfo.IsScene)
            {
                if (obj.children.Count > 0)
                {
                    ListTreeNode<Base> fsm = obj.children[0];
                    fsm.data.GetTransform();
                    foreach (ListTreeNode<Base> stateInfo in fsm.children)
                    {
                        //获得状态的当前位置，name,description
                        stateInfo.data.GetTransform();
                        foreach (ListTreeNode<Base> eventInfo in stateInfo.children)
                        {
                            eventInfo.data.GetTransform();
                            foreach (ListTreeNode<Base> lineInfo in eventInfo.children)
                            {
                                lineInfo.data.GetTransform();
                            }
                        }
                    }
                }
            }
        }
        BrushManager.Instance.SaveAllBrushObjInformation();
        BinaryFormatter bf = new BinaryFormatter();
        if (!Directory.Exists("Assets/SaveFile"))
        {
            Directory.CreateDirectory("Assets/SaveFile");
        }
        string path;
        if (automatic == true)
        {
            //先清除原有的存档
            string[] files = Directory.GetFiles(Application.streamingAssetsPath + "/EXE/SaveFile");
            string[] files2 = Directory.GetFiles(Application.streamingAssetsPath + "/EXE2/SaveFile");
            if (files != null)
            {
                foreach (string f in files)
                {
                    File.Delete(f);
                }
            }
            if (files2 != null)
            {
                foreach (string f in files2)
                {
                    File.Delete(f);
                }
            }

            path = Application.streamingAssetsPath + "/EXE/SaveFile/automatic" + saveFileFormat;
            FileStream file = File.Create(path);
            bf.Serialize(file, allDataInformation);
            file.Close();
            File.Copy(path, Application.streamingAssetsPath + "/EXE2/SaveFile/automatic" + saveFileFormat);
            if (loadScene)
            {
                if (!Directory.Exists(Application.dataPath + "/SaveFile"))
                {
                    Directory.CreateDirectory(Application.dataPath + "/SaveFile");
                }

                File.Copy(path, Application.dataPath + "/SaveFile/automatic" + saveFileFormat, true);
            }
        }
        else
        {
            path = IOHelper.OpenFileDlgToSave();
            //FileStream file = File.Create(Application.dataPath + "/SaveFile/save.txt");
            if (!string.IsNullOrEmpty(path))
            {
                FileStream file = File.Create(path);
                bf.Serialize(file, allDataInformation);
                file.Close();
                //存档保存后将存档记录下来
                //if (RecordSaveFile.record!=null)
                //{

                //}

                string[] files = Directory.GetFiles(Application.streamingAssetsPath + "/EXE/SaveFile");
                string[] files2 = Directory.GetFiles(Application.streamingAssetsPath + "/EXE2/SaveFile");

                if (files != null)
                {
                    foreach (string f in files)
                    {
                        File.Delete(f);
                    }
                }
                if (files2 != null)
                {
                    foreach (string f in files2)
                    {
                        File.Delete(f);
                    }
                }
                File.Copy(path, Application.streamingAssetsPath + "/EXE/SaveFile/" + Path.GetFileName(path));
                File.Copy(path, Application.streamingAssetsPath + "/EXE2/SaveFile/" + Path.GetFileName(path));
            }
            //将images文件夹复制到存档所在目录
            //string targetDirectoty = Path.GetDirectoryName (path);
            //Debug.LogError(targetDirectoty);
            //if (!Directory.Exists (targetDirectoty + @"\images")) {
            //    Directory.CreateDirectory (targetDirectoty + @"\images");
            //}
            //string[] targetFiles = Directory.GetFiles (Application.dataPath+@"\images");
            //if (targetFiles != null) {
            //    foreach (string item in targetFiles) {
            //        File.Copy (Application.dataPath + @"\images\"+Path.GetFileName(item), targetDirectoty + @"\images\"+Path.GetFileName(item),true);
            //    }
            //}
        }


    }
    /// <summary>
    /// 存储按钮点击
    /// </summary>
    public void SaveButtonClick()
    {

        if (SaveType.activeSelf)
        {
            //SaveType.SetActive(false);
        }
        else
        {
            SaveType.SetActive(true);
            WaitingClose.Enqueue(SaveType);
        }
    }
    /// <summary>
    /// 保存到文件
    /// </summary>
    public void SaveToFile(string path)
    {
        //记录物体的scale,pos,rotate
        allDataInformation.listTree.Root.data.GetCameraTransform();
        List<ListTreeNode<Base>> objects = allDataInformation.listTree.Root.children;
        foreach (ListTreeNode<Base> obj in objects)
        {
            obj.data.GetTransform();
            ObjectInfo objectInfo = (ObjectInfo)obj.data;
            if (!objectInfo.IsScene)
            {
                if (obj.children.Count > 0)
                {
                    ListTreeNode<Base> fsm = obj.children[0];
                    fsm.data.GetTransform();
                    foreach (ListTreeNode<Base> stateInfo in fsm.children)
                    {
                        //获得状态的当前位置，name,description
                        stateInfo.data.GetTransform();
                        foreach (ListTreeNode<Base> eventInfo in stateInfo.children)
                        {
                            eventInfo.data.GetTransform();
                            foreach (ListTreeNode<Base> lineInfo in eventInfo.children)
                            {
                                lineInfo.data.GetTransform();
                            }
                        }
                    }
                }
            }
        }

        BrushManager.Instance.SaveAllBrushObjInformation();

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file;

        file = File.Create(path);
        bf.Serialize(file, allDataInformation);
        file.Close();
        SaveType.SetActive(false);
    }
    /// <summary>
    /// 保存按钮点击
    /// </summary>
    public void SaveClick()
    {
        if (RecordSaveFile.record != null)
        {
            RecordSaveFile.SaveRecordSF(RecordSaveFile.record.PathOfRecord, RecordSaveFile.record.ID);
            SaveToFile(RecordSaveFile.record.PathOfRecord);
        }
        else
        {
            SaveAsClick();
        }
    }
    /// <summary>
    /// 另存为按钮点击
    /// </summary>
    public void SaveAsClick()
    {
        string path = IOHelper.OpenFileDlgToSave();
        if (string.IsNullOrEmpty(path))
        {
            return;
        }
        RecordSaveFile.SaveRecordSF(path, 0);
        SaveToFile(path);
    }
    /// <summary>
    /// 加载存档
    /// </summary>
    public void Open()
    {
        //edit by kuai
        SaveFileControl.Instance.OpenSaveFileList();

        /*edit by kuai
        string path = IOHelper.OpenFileDlgToLoad();
        if (!string.IsNullOrEmpty(path))
        {
            SceneCtrl.OpenStatePath = path;
            OpenNewScene();
        }
        else
        {
            Debug.LogError("未选择存档");
        }
        */

        ////if (File.Exists(Application.dataPath + "/SaveFile/save.txt"))
        ////{
        ////    BinaryFormatter bf = new BinaryFormatter();
        ////    FileStream file = File.Open(Application.dataPath + "/SaveFile/save.txt", FileMode.Open);
        ////    listTree = (ListTree<Base>)bf.Deserialize(file);
        ////    file.Close();
        ////    InitialAll();
        ////}
        //if (listTree != null)
        //{
        //    // DestroyData(listTree.Root);
        //    //listTree = null;
        //    //string projectname = "project";
        //    //int num = 1;
        //    //Base project = new Base(num, projectname);
        //    //dictFromObjectToInforma.Add(cube, project);
        //    //ListTreeNode<Base> root = new ListTreeNode<Base>(project);
        //    //listTree = new ListTree<Base>(root);
        //    foreach (ListTreeNode<Base> node in listTree.Root.children)
        //    {
        //        DestroyData(node);
        //    }

        //}

        //BinaryFormatter bf = new BinaryFormatter();
        //string path = IOHelper.OpenFileDlgToLoad();
        ////edited by kuai   取消了将寻找预览图的路径从软件目录改为桌面的操作
        ////ResLoader.targetPath = path.Substring (0, path.LastIndexOf ("\\"));
        //if (!string.IsNullOrEmpty(path))
        //{
        //    FileStream file = File.Open(path, FileMode.Open);
        //    listTree = (ListTree<Base>)bf.Deserialize(file);
        //    file.Close();
        //    print("cuo");

        //    StartCoroutine(InitialAll());
        //}
    }

    /// <summary>
    /// 加载
    /// </summary>
    public void ReWrite()
    {
        //if (File.Exists(Application.dataPath + "/SaveFile/save.txt"))
        //{
        //    BinaryFormatter bf = new BinaryFormatter();
        //    FileStream file = File.Open(Application.dataPath + "/SaveFile/save.txt", FileMode.Open);
        //    listTree = (ListTree<Base>)bf.Deserialize(file);
        //    file.Close();
        //    InitialAll();
        //}
        if (allDataInformation.listTree != null)
        {
            // DestroyData(listTree.Root);
            //listTree = null;
            //string projectname = "project";
            //int num = 1;
            //Base project = new Base(num, projectname);
            //dictFromObjectToInforma.Add(cube, project);
            //ListTreeNode<Base> root = new ListTreeNode<Base>(project);
            //listTree = new ListTree<Base>(root);
            foreach (ListTreeNode<Base> node in allDataInformation.listTree.Root.children)
            {
                DestroyData(node);
            }

        }

        BinaryFormatter bf = new BinaryFormatter();
        string path = IOHelper.OpenFileDlgToLoad();
        //edited by kuai   取消了将寻找预览图的路径从软件目录改为桌面的操作
        //ResLoader.targetPath = path.Substring (0, path.LastIndexOf ("\\"));
        if (!string.IsNullOrEmpty(path))
        {
            FileStream file = new FileStream(path, FileMode.Open);
            allDataInformation = (AllDataInformation)bf.Deserialize(file);
            file.Close();
            foreach (ListTreeNode<Base> childen in allDataInformation.listTree.Root.children)
            {
                ObjectInfo objectInfo = (ObjectInfo)childen.data;
                string str = objectInfo.imgPath;
                string[] f = { "StreamingAssets" };
                string[] ss = str.Split(f, StringSplitOptions.None);
                print(ss[1]);
                objectInfo.imgPath = ss[1];
                FileStream file2 = File.Create(path);
                bf.Serialize(file2, allDataInformation);
                file2.Close();
            }
        }
    }

    public void DestroyData(ListTreeNode<Base> treeNode)
    {
        if (treeNode.children.Count != 0)
        {
            foreach (ListTreeNode<Base> node in treeNode.children)
            {
                DestroyData(node);
            }
        }

        //如果是状态，则删除状态下的动作
        StateInfo stateInfo = treeNode.data as StateInfo;
        if (stateInfo != null && stateInfo.actionList.Count != 0)
        {
            StatePanel.GetComponent<StatePanelUI>().currentState = null;

            foreach (ActionInforma i in stateInfo.actionList)
            {
                Destroy(actionInfomaToGameobject[i]);
            }
        }
        GameObject obj = dictFromObjectToInforma.FirstOrDefault(q => q.Value == treeNode.data).Key;
        if (obj != cube)
        {
            //Manager.Instace.listTree.DeleteNode(treeNode.parent, treeNode.data);
            Destroy(obj);

        }

        if (obj != null)
        {
            if (objectTopic.ContainsKey(obj))
            {
                Destroy(objectTopic[obj]);
                gonggong = null;
                backGround2.SetActive(false);
                backGround3.SetActive(false);
                backGround1.SetActive(true);
            }
        }

    }

    public IEnumerator InitialAll()
    {
        InitialCustomEvent(allDataInformation.listTree.Root.data.customEvent);
        InitialVariable(allDataInformation.listTree.Root.data.customVariable);
        //Base project = listTree.Root.data;
        Base.id = allDataInformation.listTree.nodeNum;
        //Base.allData.Add(project);
        Camera.main.transform.position = GetVector3(allDataInformation.listTree.Root.data.pos);
        //Camera.main.transform.position = GetVector3( listTree.Root.data.pos);
        dictFromObjectToInforma[cube].pos = allDataInformation.listTree.Root.data.pos;
        dictFromObjectToInforma[cube].rotate = allDataInformation.listTree.Root.data.rotate;
        dictFromObjectToInforma[cube].scale = allDataInformation.listTree.Root.data.scale;
        foreach (ListTreeNode<Base> childen in allDataInformation.listTree.Root.children)
        {
            ObjectInfo objectInfo = (ObjectInfo)childen.data;
            yield return InitialObject(objectInfo);
            GameObject targetobj = gonggong;

            objectInfo.name = targetobj.name;
            //targetobj.AddComponent<GameObjectIndex>().index = objectInfo.index;
            //Base.id = objectInfo.index;
            //Informa<Base>.allData.Add(objectInfo);
            //dictFromObjectToInforma.Add(targetobj, objectInfo);
            if (!objectInfo.IsScene && childen.children.Count > 0)
            {
                ListTreeNode<Base> fsmNode = childen.children[0];
                FSMInfo fsmInfo = (FSMInfo)fsmNode.data;
                //GameObject fsm=InitialUIFsm(fsmInfo,targetobj);
                currentStateStart = InitialUIFsm(fsmInfo, targetobj);
                objectToFsm.Add(targetobj, currentStateStart);
                //fsm.AddComponent<GameObjectIndex>().index = fsmInfo.index;
                //Base.id = listTree.nodeNum;
                //Informa<Base>.allData.Add(fsmInfo);
                dictFromObjectToInforma.Add(currentStateStart, fsmInfo);
                foreach (ListTreeNode<Base> stateNode in fsmNode.children)
                {
                    StateInfo statenode = (StateInfo)stateNode.data;

                    State<Main> s = null;
                    Transform stateui = InitialUIState(statenode, currentStateStart, out s);
                    //stateui.gameObject.AddComponent<GameObjectIndex>().index = statenode.index;
                    //Base.id = statenode.index;
                    //Informa<Base>.allData.Add(statenode);
                    dictFromObjectToInforma.Add(stateui.gameObject, statenode);
                    foreach (ListTreeNode<Base> eventNode in stateNode.children)
                    {
                        EventInfo eventnode = (EventInfo)eventNode.data;
                        Transform eventui = InitialUIEvent(eventnode, stateui, s);
                        //eventui.gameObject.AddComponent<GameObjectIndex>().index = eventnode.index;
                        //Base.id = eventnode.index;
                        //Informa<Base>.allData.Add(eventnode);
                        dictFromObjectToInforma.Add(eventui.gameObject, eventnode);
                        if (eventNode.children.Count > 0)
                        {
                            ListTreeNode<Base> line = eventNode.children[0];//初始化线段
                            if (line != null)
                            {
                                LineInfo linenode = (LineInfo)line.data;
                                GameObject l = InitialUILine(linenode, currentStateStart.transform, eventui, targetobj);
                                //l.AddComponent<GameObjectIndex>().index = linenode.index;
                                //Base.id = linenode.index;
                                //Informa<Base>.allData.Add(linenode);
                                dictFromObjectToInforma.Add(l, linenode);
                            }
                        }
                    }
                }
            }
            else if (objectInfo.IsScene)
            {
                dictFromObjectToInforma.Add(targetobj, objectInfo);
            }

        }
        //所有物体创建完成后设置其父子层级关系
        for (int i = 0; i < parentNameList.Count; i++)
        {
            if (parentNameList[i].parentName != "Parent")
            {
                GameObject[] parent = GameObject.FindGameObjectsWithTag("Editor");
                for (int j = 0; j < parent.Length; j++)
                {
                    if (parentNameList[i].parentName == parent[j].name)
                    {
                        Vector3 scale = parentNameList[i].Go.transform.localScale;
                        parentNameList[i].Go.transform.SetParent(parent[j].transform);
                        parentNameList[i].Go.transform.localScale = scale;
                        break;
                    }
                }

            }

            objectTopic[parentNameList[i].Go].GetComponent<item>().parentName = parentNameList[i].parentName;


        }
        for (int i = 0; i < parentNameList.Count; i++)
        {
            List<string> childNameList = new List<string>();
            GameObject[] Gos = GameObject.FindGameObjectsWithTag("Editor");
            for (int j = 0; j < Gos.Length; j++)
            {
                if (Gos[j].transform.parent.name == parentNameList[i].Go.name)
                {
                    childNameList.Add(Gos[j].name);
                }
            }
            if (childNameList.Count > 0)
            {
                //Debug.Log("6666666666666");
                item _item = objectTopic[parentNameList[i].Go].GetComponent<item>();
                _item.ChildrenName = childNameList;
                //在重置下子物体列表
                _item.InitChildList();
            }
        }
        if (allDataInformation.brushedObjInformation != null)
        {
            BrushManager.Instance.AutoCreatBrushedObjsByInformation();
        }
        ISOpen = true;
    }

    public void AddStateInfoByUndo(ListTreeNode<Base> data, GameObject targetObject)
    {
        if (data.children.Count > 0)
        {
            ListTreeNode<Base> fsmNode = data.children[0];

            FSMInfo fsmInfo = (FSMInfo)fsmNode.data;
            currentStateStart = InitialUIFsm(fsmInfo, targetObject);
            objectToFsm.Add(targetObject, currentStateStart);
            dictFromObjectToInforma.Add(currentStateStart, fsmInfo);
            foreach (ListTreeNode<Base> stateNode in fsmNode.children)
            {
                StateInfo statenode = (StateInfo)stateNode.data;

                State<Main> s = null;
                Transform stateui = InitialUIState(statenode, currentStateStart, out s);
                dictFromObjectToInforma.Add(stateui.gameObject, statenode);
                foreach (ListTreeNode<Base> eventNode in stateNode.children)
                {
                    EventInfo eventnode = (EventInfo)eventNode.data;
                    Transform eventui = InitialUIEvent(eventnode, stateui, s);
                    dictFromObjectToInforma.Add(eventui.gameObject, eventnode);
                    if (eventNode.children.Count > 0)
                    {
                        ListTreeNode<Base> line = eventNode.children[0];//初始化线段
                        if (line != null)
                        {
                            LineInfo linenode = (LineInfo)line.data;
                            GameObject l = InitialUILine(linenode, currentStateStart.transform, eventui, targetObject);
                            dictFromObjectToInforma.Add(l, linenode);
                        }
                    }
                }
            }
        }
    }


    public Coroutine InitialObject(ObjectInfo obj)
    {
        //加载模型物体数据
        return StartCoroutine(LoadGameObject(obj));
    }

    //初始化自定义的事件
    public void InitialCustomEvent(List<string> str)
    {
        if (eventlist.Count == 0)
        {
            foreach (string s in str)
            {
                if (prefab11 == null)
                {
                    prefab11 = (GameObject)Resources.Load("Prefabs/eventEditorBtn");
                }
                GameObject obj = Instantiate(prefab11);
                obj.transform.SetParent(eventlist2.transform.Find(@"Scroll View/Viewport/Content"));
                obj.transform.localScale = Vector3.one;
                obj.transform.localPosition = new Vector3(obj.transform.position.x, obj.transform.position.y, 0f);
                obj.GetComponentInChildren<Text>().text = s;
                obj.GetComponent<EventEditorBtn>().InitialEvent(s);

                //在自定义事件中也添加
                if (prefab22 == null)
                {
                    prefab22 = (GameObject)Resources.Load("Prefabs/EventBtn");
                }
                GameObject obj2 = Instantiate(prefab22);
                obj2.GetComponentInChildren<Text>().text = s;
                obj2.transform.SetParent(CustomEventlistUI.transform.Find(@"Scroll View/Viewport/Content"));
                obj2.transform.localScale = Vector3.one;
                obj2.transform.localPosition = new Vector3(obj.transform.position.x, obj.transform.position.y, 0f);
                obj.GetComponent<EventEditorBtn>().Setobjs(obj2);
                if (prefab33 == null)
                {
                    prefab33 = (GameObject)Resources.Load("Prefabs/EventBtnEditor");
                }
                GameObject obj3 = Instantiate(prefab33);
                obj3.GetComponentInChildren<Text>().text = s;
                obj3.transform.SetParent(CustomEventlistUI2.transform.Find(@"Scroll View/Viewport/Content"));
                obj3.transform.localScale = Vector3.one;
                obj3.transform.localPosition = new Vector3(obj.transform.position.x, obj.transform.position.y, 0f);
                obj.GetComponent<EventEditorBtn>().Setobjs(obj3);
            }
        }
    }

    public void InitialVariable(List<variableData> list)
    {
        foreach (variableData v in list)
        {
            GameObject obj = variablePanelUI.LoadItem(v.number);
            obj.GetComponent<VariableControl>().variable.data = v;
            obj.GetComponent<VariableControl>().variable.name = v.name;
            obj.GetComponent<VariableControl>().variable.type = v.number;

        }
    }

    //加载FsmUI数据
    public GameObject InitialUIFsm(FSMInfo fsmInfo, GameObject obj)
    {
        if (StateStart == null)
        {
            StateStart = (GameObject)Resources.Load("Prefabs/Fsm");
        }
        currentStateStart = Instantiate(StateStart);
        currentStateStart.transform.SetParent(StateMachineCanvas.transform);
        currentStateStart.transform.localPosition = GetVector3(fsmInfo.pos);
        currentStateStart.transform.eulerAngles = GetVector3(fsmInfo.rotate);
        currentStateStart.transform.localScale = GetVector3(fsmInfo.scale);
        currentStateStart.name = fsmInfo.name;
        currentStateStart.SetActive(false);
        return currentStateStart;

    }


    //加载状态
    public Transform InitialUIState(StateInfo stateInfo, GameObject obj, out State<Main> s)
    {

        Transform statenode = null;
        //State<Main> s = null;
        //开始状态只加载数据，不用创建
        if (stateInfo.IsStart)
        {
            statenode = obj.transform.GetChild(0);
            //给其赋值Main和初始化开始状态
            //currentMain = gonggong.AddComponent<Main>();
            currentMain = gonggong.AddComponent<Main>();
            currentStateStart.GetComponentInChildren<StateNode>().Initial();
            currentStateStart.GetComponentInChildren<StateNode>().TargetTransform = gonggong.transform;
            s = currentMain.SetStart(currentStateStart.GetComponentInChildren<StateNode>().state);
            currentMain.Fsm = obj;
            currentMain.enabled = false;
            //初始化stateInfo中的值，给状态中actionList赋值,
            //s.onceActionList = stateInfo.onceActionList;

        }
        else
        {
            if (prefabState == null)
            {
                prefabState = (GameObject)Resources.Load("Prefabs/One");
            }
            statenode = Instantiate(prefabState).transform;
            statenode.SetParent(obj.transform); print(obj.name);
            s = statenode.GetComponentInChildren<StateNode>().Initial();
            statenode.GetComponentInChildren<StateNode>().TargetTransform = gonggong.transform;
        }
        //Debug.Log(2);
        if (statenodeDict.ContainsKey(stateInfo))
        {
            statenodeDict[stateInfo] = statenode.gameObject;
        }
        else
        {
            statenodeDict.Add(stateInfo, statenode.gameObject);
        }

        InitialActionList(s, stateInfo);
        //s.EndAction=
        statenode.localPosition = GetVector3(stateInfo.pos);
        statenode.localScale = GetVector3(stateInfo.scale);
        statenode.eulerAngles = GetVector3(stateInfo.rotate);
        statenode.GetChild(0).GetComponent<StateNode>().name = stateInfo.name;
        statenode.GetChild(0).GetComponent<StateNode>().description = stateInfo.Description;
        statenode.GetChild(0).GetComponent<StateNode>().text.text = stateInfo.name;
        statenode.GetChild(0).GetComponentInChildren<Text>().text = stateInfo.name;
        s.name = stateInfo.name;
        statenode.gameObject.SetActive(stateInfo.isActive);
        return statenode;
    }

    public Transform InitialUIEvent(EventInfo eventInfo, Transform transform, State<Main> s)
    {
        Transform eventnode = Instantiate(Event).transform;
        eventnode.SetParent(transform);
        eventnode.localPosition = GetVector3(eventInfo.pos);
        eventnode.localScale = GetVector3(eventInfo.scale);
        eventnode.eulerAngles = GetVector3(eventInfo.rotate);
        eventnode.GetComponent<EventNode>().name = eventInfo.name;
        eventnode.GetComponentInChildren<Text>().text = eventInfo.name;
        //初始化事件
        switch (eventInfo.eventType)
        {
            case EventsType.CustomEvents:
                //则查找自定义事件列表
                foreach (Events e in eventlist)
                {
                    if (eventInfo.name == e.name)
                    {
                        Events events = eventnode.GetComponent<EventNode>().InitialEvent(e);

                        events.currentState = s;//设置事件的当前状态
                        s.eventList.Add(events);
                        if (eventRegisterList.ContainsKey(e))
                        {
                            eventRegisterList[e].Add(events);
                        }
                        else
                        {
                            List<Events> list = new List<Events>();
                            list.Add(events);
                            eventRegisterList.Add(e, list);
                        }

                    }
                }
                break;
            case EventsType.NetworkEvents:
                break;
            case EventsType.SystemEvents:
                //则查找系统事件列表
                Events a = eventnode.GetComponent<EventNode>().InitialEvent(eventInfo.name, eventInfo.eventType, s);
                a.currentState = s;//设置事件的当前状态
                s.eventList.Add(a);
                if (a.name == "鼠标左键按下")
                {
                    MouseDown m = new MouseDown();
                    m.even = a;

                    m.Ini(ctrllerEventsR, VRPointerR);

                    s.updateActionList.Add(m);

                    currentMain.SetStart(s);

                }
                if (a.name == "结束事件")
                {
                    FinishAction f = new FinishAction();
                    Debug.LogError("Manager");
                    f.even = a;
                    //s.updateActionList.Add(f);
                    s.EndAction = f;
                    currentMain.SetStart(s);
                }
                if (a.name == "按钮按下")
                {
                    BtnToNext m = new BtnToNext();
                    ShowBtn showBtn = null;
                    m.even = a;
                    s.updateActionList.Add(m);
                    foreach (Action<Main> item in s.updateActionList)
                    {
                        try
                        {
                            showBtn = (ShowBtn)item;

                            if (showBtn.tarid == s.eventList.Count - 1)
                            {
                                break;
                            }
                        }
                        catch
                        {

                        }
                    }
                    if (showBtn != null)
                    {
                        m.vrBtn = showBtn.currentUI.GetComponent<VRBtn>();
                    }
                    currentMain.SetStart(s);

                }

                if (a.name.Contains("按键按下"))
                {
                    KeyDown keyDown = new KeyDown();
                    keyDown.even = a;
                    s.updateActionList.Add(keyDown);
                    keyDown.keyCode = (KeyCode)eventInfo.keyid;
                    currentMain.SetStart(s);
                }
                if (a.name.Contains("鼠标左键长按"))
                {
                    MouseDownLong m = new MouseDownLong();
                    m.even = a;

                    //m.Ini(Manager.Instace.ctrllerEventsR, Manager.Instace.VRPointerR);

                    s.updateActionList.Add(m);

                    currentMain.SetStart(s);
                }
                if (a.name.Contains("鼠标左键松开"))
                {
                    MouseUp m = new MouseUp();
                    m.even = a;
                    s.updateActionList.Add(m);

                    currentMain.SetStart(s);
                }
                if (a.name.Contains("鼠标进入"))
                {
                    MouseEnter m = new MouseEnter();
                    m.even = a;
                    s.updateActionList.Add(m);

                    currentMain.SetStart(s);
                }
                if (a.name.Contains("鼠标离开"))
                {
                    MouseExit m = new MouseExit();
                    m.even = a;
                    s.updateActionList.Add(m);

                    currentMain.SetStart(s);
                }
                if (a.name.Contains("鼠标右键按下"))
                {
                    RMouseDown m = new RMouseDown();
                    m.even = a;
                    s.updateActionList.Add(m);
                    currentMain.SetStart(s);
                }
                break;
            case EventsType.HTCEvents:
                Events htce = eventnode.GetComponent<EventNode>().InitialEvent(eventInfo.name, eventInfo.eventType, s);
                htce.currentState = s;//设置事件的当前状态
                s.eventList.Add(htce);
                if (htce.name == "手柄激光点击")
                {
                    PointerSelected ps = new PointerSelected();
                    ps.even = htce;
                    ps.Ini(ctrllerEventsR, VRPointerR);
                    s.updateActionList.Add(ps);
                    currentMain.SetStart(s);
                }
                if (htce.name == "手柄接触")
                {
                    ControllerInteract ci = new ControllerInteract();
                    ci.Ini();
                    ci.SetType(ControllerInteract.InteractType.touch);
                    ci.even = htce;
                    s.updateActionList.Add(ci);
                    currentMain.SetStart(s);
                }
                if (htce.name == "手柄停止接触")
                {
                    ControllerInteract ci = new ControllerInteract();
                    ci.Ini();
                    ci.SetType(ControllerInteract.InteractType.untouch);
                    ci.even = htce;
                    s.updateActionList.Add(ci);
                    currentMain.SetStart(s);
                }
                if (htce.name == "手柄抓取")
                {
                    ControllerInteract ci = new ControllerInteract();
                    ci.Ini();
                    ci.SetType(ControllerInteract.InteractType.grab);
                    ci.even = htce;
                    s.updateActionList.Add(ci);
                    currentMain.SetStart(s);
                }
                if (htce.name == "手柄停止抓取")
                {
                    ControllerInteract ci = new ControllerInteract();
                    ci.Ini();
                    ci.SetType(ControllerInteract.InteractType.ungrab);
                    ci.even = htce;
                    s.updateActionList.Add(ci);
                    currentMain.SetStart(s);
                }
                if (htce.name == "物体划动")
                {
                    ControllerInteract ci = new ControllerInteract();
                    ci.Ini();
                    ci.SetType(ControllerInteract.InteractType.moveThrough);
                    ci.even = htce;
                    s.updateActionList.Add(ci);
                    currentMain.SetStart(s);
                }
                break;
        }
        return eventnode;
    }

    public GameObject InitialUILine(LineInfo lineInfo, Transform transform, Transform eventNode, GameObject obj)
    {
        GameObject line = new GameObject();
        line.transform.SetParent(transform);
        LineRenderer lr = line.AddComponent<LineRenderer>();
        Line InitialLine = line.AddComponent<Line>();
        //给线段中的eventnode和statenode赋值
        InitialLine.eventnode = eventNode.GetComponent<EventNode>();
        //使用协同
        StartCoroutine(SetLineEnd(lineInfo, InitialLine, eventNode, obj, line));

        //l.statenode=lineInfo.stateInfo
        lr.SetPosition(0, GetVector3(lineInfo.beginPos));
        lr.SetPosition(1, GetVector3(lineInfo.endPos));
        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;
        lr.material.color = Color.black;
        return line;
    }

    IEnumerator SetLineEnd(LineInfo lineinfo, Line l, Transform even, GameObject obj, GameObject line)
    {
        if (!statenodeDict.ContainsKey(lineinfo.stateInfo))
        {
            statenodeDict.Add(lineinfo.stateInfo, null);
        }

        while (statenodeDict[lineinfo.stateInfo] == null)
        {
            yield return null;
        }
        //Debug.Log(1);
        l.statenode = statenodeDict[lineinfo.stateInfo].GetComponentInChildren<StateNode>();
        l.statenode.line = line;
        lineinfo.stateInfo.lineInfo = lineinfo;
        even.GetComponent<EventNode>().even.nextState = l.statenode.state;
        even.GetComponent<EventNode>().line = line;
        even.GetComponent<EventNode>().even.target = obj.GetComponent<Main>();
        even.GetComponent<EventNode>().even.Register();
        l.events = even.GetComponent<EventNode>().even;


    }

    public void InitialActionList(State<Main> state, StateInfo stateInfo)
    {
        foreach (ActionInforma actionInfo in stateInfo.actionList)
        {
            //还原动作，将其加到状态中去
            GameObject action = AddActionToActionList(actionInfo.name);
            action.GetComponent<ActionUI>().ActionTargetTransform =
                StateNode.dict[state].GetComponent<StateNode>().TargetTransform;
            //Debug.LogError(StateNode.dict[state].GetComponent<StateNode>().TargetTransform);
            Action<Main> currentAction = action.GetComponent<ActionUI>().LoadAction(actionInfo);
            if (!actionInfomaToGameobject.ContainsKey(actionInfo))
            {
                actionInfomaToGameobject.Add(actionInfo, action);
            }


            if (actionInfo.isOnce)
            {
                state.onceActionList.Add(currentAction);
            }
            else
            {
                state.updateActionList.Add(currentAction);
            }

            action.GetComponent<ActionUI>().SetObject(action);
            state.actionUIlist.Add(action.GetComponent<ActionUI>());
            action.SetActive(false);
        }
        //初始化列表
        _E2ND.AccDic();
        _E2ND.InitialUIList();
    }
    //加载模型
    #region
    /// <summary>
    /// 加载场景窗口的物体信息
    /// </summary>
    /// <param name="data"></param>
    public IEnumerator LoadGameObject(ObjectInfo obj)
    {
        if (obj.IsScene)
        {
            string[] array = obj.name.Split('(');
            yield return LoadSceneOrStuff(array[0], obj);
            //GameObject temp = gonggong;
            print(gonggong + "!!!!!!!!!!!!!");
            gonggong.layer = G_PubDef.quiescentObject;
            gonggong.tag = "Editor";
            Transform[] all = gonggong.transform.GetComponentsInChildren<Transform>();
            foreach (Transform i in all)
            {
                i.gameObject.layer = G_PubDef.quiescentObject;
                //i.gameObject.tag = "Editor";
            }
            GetComponent<G_CreateObject>().AddInObjectList(gonggong, array[0], obj.imgPath);
            //return temp;
        }
        else
        {
            string[] array = obj.name.Split('*');
            yield return LoadSceneOrStuff(array[0], obj);
            //gonggong = gonggong;
            Product.ManagerGameobject(array[0], gonggong);
            gonggong.tag = "Editor";
            gonggong.layer = G_PubDef.dynamicObject;
            foreach (Transform i in gonggong.transform)
            {
                i.gameObject.layer = G_PubDef.dynamicObject;
            }

            GetComponent<G_CreateObject>().AddInObjectList(gonggong, array[0], obj.imgPath);

            dictFromObjectToInforma.Add(gonggong, obj);
            InitialProperty(obj);
            //gonggong = gonggong;
            //return gonggong;
        }
    }

    /// <summary>
    /// 初始化属性面板
    /// </summary>
    /// <param name="obj"></param>
    public void InitialProperty(ObjectInfo obj)
    {
        if (obj.hasRigid)
        {
            addPreperty.AddComponnent(2);
        }
    }

    //void InitialRigid(ObjectInfo obj)
    //{
    //    GameObject  prefab = (GameObject)Resources.Load("Prefabs/AddRigid");
    //    GameObject ob = Instantiate(prefab);
    //    ob.transform.SetParent(addPreperty.transform);
    //    ob.transform.localScale = Vector3.one;
    //    ob.GetComponent<AddRigid>().b = obj;
    //    List<GameObject> temp = new List<GameObject>();
    //    temp.Add(ob);
    //    Manager.Instace.objectPropertyDic.Add(Manager.Instace.gonggong, temp);
    //    Rigidbody rb = Manager.Instace.gonggong.AddComponent<Rigidbody>();
    //    rb.useGravity = false;
    //    rb.isKinematic = true;
    //}
    private IEnumerator loadfbx(GameObject one, string name, ObjectInfo objectInfo)
    {
        string s = string.Empty;
        string[] sarr = objectInfo.modelPath.Split('/');
        for (int i = 0; i < sarr.Length - 1; i++)
        {
            s += sarr[i] + "/";
        }
        string houzhui = sarr[sarr.Length - 1].Split('.')[1];
        string ssname = sarr[sarr.Length - 1].Split('.')[0];
        ufbxi.setting.paths.SetModelPath(s, ssname);
        print(objectInfo.name);
        yield return ufbxi.Load();
        //ufbxi.GetObject().name = name;
        //gameobjectList.Add(name, ufbxi.GetObject());
        GameObject obj = Instantiate<GameObject>(ufbxi.GetObject());
        Destroy(ufbxi.GetObject());
        obj.name = objectInfo.name;
        obj.transform.localScale = new Vector3(100, 100, 100);
        //改变Json文件中的数值
        print(name + "*****");
        gameobjectList.Add(name, obj);
        one = Instantiate(gameobjectList[name]);
        one.transform.position = GetVector3(objectInfo.pos);
        one.transform.localScale = GetVector3(objectInfo.scale);
        one.transform.eulerAngles = GetVector3(objectInfo.rotate);
        one.name = objectInfo.name;
        one.transform.SetParent(parent);
        GoAndParentName GP = new GoAndParentName(one, objectInfo.parentName);
        parentNameList.Add(GP);
        if (objectInfo.ChildIntroduce != null && objectInfo.ChildIntroduce.Count > 0)
        {
            Introduce introduce;
            for (int i = 0; i < objectInfo.ChildIntroduce.Count; i++)
            {
                introduce = one.transform.FindChild(objectInfo.ChildIntroduce[i].childName).gameObject
                    .AddComponent<Introduce>();
                introduce.SetIntroduce(objectInfo.ChildIntroduce[i].introduce);
                introduce.SetImagePath(objectInfo.ChildIntroduce[i].imagePath);
                introduce.SetVideoPath(objectInfo.ChildIntroduce[i].videoPath);
            }
        }
        //gonggong = one;
        gonggong = one;
        Debug.LogError(gonggong.name);
    }
    private IEnumerator loadthreepro(GameObject one, string name, ObjectInfo objectInfo)
    {
        print(objectInfo.modelPath);
        AssetBundle assets = AssetBundle.LoadFromFile(objectInfo.modelPath);
        #region 如果要读取的三维模型不存在，读取一个问号的模型到场景中示意
        if (assets == null)
        {
            assets = AssetBundle.LoadFromFile("AssetBundles/error_missing/error_missing.3dpro");
        }
        #endregion
        string[] ssr = assets.name.Split('.');
        GameObject temp = assets.LoadAsset<GameObject>(ssr[0]);//资源加载到内存
        //GameObject temp = assets.LoadAsset<GameObject>(name);//资源加载到内存
        // obj.transform.GetChild(0).gameObject.AddComponent<CapsuleCollider>();//在物体的自物体上添加碰撞体
        //temp.AddComponent<BoxCollider>();
        print(assets.name + "?????");
        temp.name = objectInfo.name;
        assetList.Add(assets);
        //改变Json文件中的数值
        gameobjectList.Add(name, temp);//对加载的资源进行管理
        one = Instantiate(gameobjectList[name]);
        one.transform.position = GetVector3(objectInfo.pos);
        one.transform.localScale = GetVector3(objectInfo.scale);
        one.transform.eulerAngles = GetVector3(objectInfo.rotate);
        one.name = objectInfo.name;

        one.transform.SetParent(parent);
        GoAndParentName GP = new GoAndParentName(one, objectInfo.parentName);
        parentNameList.Add(GP);
        if (objectInfo.ChildIntroduce != null && objectInfo.ChildIntroduce.Count > 0)
        {
            Introduce introduce;
            for (int i = 0; i < objectInfo.ChildIntroduce.Count; i++)
            {
                introduce = one.transform.FindChild(objectInfo.ChildIntroduce[i].childName).gameObject
                    .AddComponent<Introduce>();
                introduce.SetIntroduce(objectInfo.ChildIntroduce[i].introduce);
                introduce.SetImagePath(objectInfo.ChildIntroduce[i].imagePath);
                introduce.SetVideoPath(objectInfo.ChildIntroduce[i].videoPath);
            }
        }
        gonggong = one;
        //Debug.LogError("chuangjianlewuti");
        //edit by kuai
        //todo add undoScript
        var objInfo = new CreateUndoObjInfo()
        {
            Tag = objectInfo.modelPath.Split('/')[1],
            ModelPath = objectInfo.modelPath,
            Name = name,
            ImagePath = "/ink2/" + name + ".PNG",
        };
        if (one.GetComponent<CreateObjUndoComponent>() == null)
        {
            var undoComponent = one.AddComponent<CreateObjUndoComponent>();
            undoComponent.Init(objInfo);
            Debug.Log("CreteGamObject ：" + one);
        }
        yield return null;

    }
    public Coroutine LoadSceneOrStuff(string name, ObjectInfo objectInfo)
    {
        print(name);
        //Debug.LogError()

        //string _realname = itemNameList.FirstOrDefault(q => q.Value.Contains(name)).Key;
        //var _realname = itemNameList.Where(q => q.Value == name).Select(q => q.Key);
        //string sceneName = name;
        GameObject one = null;
        if (!gameobjectList.ContainsKey(name))
        {


            string s = string.Empty;
            string[] sarr = objectInfo.modelPath.Split('/');
            for (int i = 0; i < sarr.Length - 1; i++)
            {
                s += sarr[i] + "/";
            }
            string houzhui = sarr[sarr.Length - 1].Split('.')[1];

            if (houzhui == "3dpro")
            {
                //    AssetBundle assets = AssetBundle.LoadFromFile(objectInfo.modelPath);
                //    GameObject temp = assets.LoadAsset<GameObject>(sceneName);//资源加载到内存
                //    // obj.transform.GetChild(0).gameObject.AddComponent<CapsuleCollider>();//在物体的自物体上添加碰撞体
                //    //temp.AddComponent<BoxCollider>();
                //    temp.name = objectInfo.name;
                //    assetList.Add(assets);
                //    //改变Json文件中的数值
                //    gameobjectList.Add(sceneName, temp);//对加载的资源进行管理
                //    one = Instantiate(gameobjectList[sceneName]);
                //    one.transform.position = GetVector3(objectInfo.pos);
                //    one.transform.localScale = GetVector3(objectInfo.scale);
                //    one.transform.eulerAngles = GetVector3(objectInfo.rotate);
                //    one.name = objectInfo.name;
                //    one.transform.SetParent(parent);
                //    GoAndParentName GP = new GoAndParentName(one, objectInfo.parentName);
                //    parentNameList.Add(GP);
                //    if (objectInfo.ChildIntroduce != null && objectInfo.ChildIntroduce.Count > 0)
                //    {
                //        for (int i = 0; i < objectInfo.ChildIntroduce.Count; i++)
                //        {
                //            one.transform.FindChild(objectInfo.ChildIntroduce[i].childName).gameObject.AddComponent<Introduce>().SetIntroduce(objectInfo.ChildIntroduce[i].introduce);
                //        }
                //    }
                //    string[] array = objectInfo.name.Split('*');
                //    //gonggong = one;
                //    Product.ManagerGameobject(array[0], one);
                //    one.tag = "Editor";
                //    one.layer = G_PubDef.dynamicObject;
                //    foreach (Transform i in one.transform)
                //    {
                //        i.gameObject.layer = G_PubDef.dynamicObject;
                //    }
                //    GetComponent<G_CreateObject>().AddInObjectList(one, array[0]);

                //    dictFromObjectToInforma.Add(one, objectInfo);
                //    InitialProperty(objectInfo);
                return StartCoroutine(loadthreepro(one, name, objectInfo));
            }
            else if (houzhui == "FBX" || houzhui == "fbx")
            {
                print("我咋回事啊");
                return StartCoroutine(loadfbx(one, name, objectInfo));

            }
            else return null;
        }
        else
        {
            one = Instantiate(gameobjectList[name]);
            one.transform.position = GetVector3(objectInfo.pos);
            one.transform.localScale = GetVector3(objectInfo.scale);
            one.transform.eulerAngles = GetVector3(objectInfo.rotate);
            one.name = objectInfo.name;

            one.transform.SetParent(parent);
            GoAndParentName GP = new GoAndParentName(one, objectInfo.parentName);
            parentNameList.Add(GP);
            if (objectInfo.ChildIntroduce != null && objectInfo.ChildIntroduce.Count > 0)
            {
                Introduce introduce;
                for (int i = 0; i < objectInfo.ChildIntroduce.Count; i++)
                {
                    introduce = one.transform.FindChild(objectInfo.ChildIntroduce[i].childName).gameObject
                        .AddComponent<Introduce>();

                    introduce.SetIntroduce(objectInfo.ChildIntroduce[i].introduce);
                    introduce.SetImagePath(objectInfo.ChildIntroduce[i].imagePath);
                    introduce.SetVideoPath(objectInfo.ChildIntroduce[i].videoPath);
                }
            }
            gonggong = one;

        }
        return null;


        //GameObject[] par = GameObject.FindGameObjectsWithTag("Editor");
        //Transform t = null;
        //for (int i = 0; i < par.Length; i++)
        //{
        //    if (par[i].name == objectInfo.parentName)
        //    {
        //        t = par[i].transform;
        //    }
        //}
        //if (t != null)
        //{
        //    one.transform.SetParent(t);

        //}
        //return one;

    }
    #endregion
    #region
    //public void StringToJson(string data)
    //{
    //    JsonData projectJd = JsonMapper.ToObject(data);
    //    JsonData objects = projectJd["data"];
    //    foreach (JsonData obj in objects)
    //    {
    //        LoadGameObject(obj);
    //        //foreach (JsonData fsm in obj)
    //        //{
    //        //    LoadFsmInfo(fsm);
    //        //}

    //    }
    //    JsonData name = projectJd["name"];
    //    Debug.Log(name.ToString());
    //}

    public void LoadFsmInfo(JsonData data)
    {
        currentStateStart = Instantiate(StateStart);

        currentStateStart.transform.SetParent(StateMachineCanvas.transform);
        currentStateStart.transform.localPosition = GetVector3(data["pos"].ToString());
        currentStateStart.transform.localScale = GetVector3(data["scale"].ToString());
        currentStateStart.transform.eulerAngles = GetVector3(data["rotate"].ToString());
        AddMainToObject();
        currentMain.Fsm = currentStateStart;
        currentMain.Fsm.name = "FSM:" + gonggong.name;
        EditorCanvas.SetActive(true);
    }

    /// <summary>
    /// 加载场景窗口的物体信息
    /// </summary>
    /// <param name="data"></param>

    //public void  LoadGameObject(JsonData data){
    //    if ((bool)data["IsScene"])
    //    {
    //        string[] array = data["name"].ToString().Split('(');
    //     GameObject temp = LoadSceneOrStuff(array[0], data["modelPath"].ToString(), data["name"].ToString(), data["pos"].ToString(), data["rotate"].ToString(), data["scale"].ToString());
    //     temp.layer = G_PubDef.quiescentObject;
    //     Transform[] all = temp.transform.GetComponentsInChildren<Transform>();
    //     foreach (Transform i in all)
    //     {
    //         i.gameObject.layer = G_PubDef.quiescentObject;
    //         i.gameObject.tag = "Editor";
    //     }
    //    }
    //    else
    //    {
    //        string[] array = data["name"].ToString().Split('*');
    //     GameObject temp= LoadSceneOrStuff(array[0], data["modelPath"].ToString(), data["name"].ToString(), data["pos"].ToString(), data["rotate"].ToString(), data["scale"].ToString());
    //     temp.layer = G_PubDef.dynamicObject;
    //     foreach (Transform i in temp.transform)
    //     {
    //         i.gameObject.layer = G_PubDef.dynamicObject;
    //     }
    //     GetComponent<G_CreateObject>().AddInObjectList(temp, array[0]);
    //    }
    //}

    public GameObject LoadSceneOrStuff(string name, string modelPath, string names, string pos, string rotate, string scale)
    {
        string sceneName = name;
        if (!gameobjectList.ContainsKey(sceneName))
        {
            AssetBundle assets = AssetBundle.LoadFromFile(modelPath);
            GameObject temp = assets.LoadAsset<GameObject>(sceneName);//资源加载到内存
            // obj.transform.GetChild(0).gameObject.AddComponent<CapsuleCollider>();//在物体的自物体上添加碰撞体
            temp.AddComponent<BoxCollider>();
            temp.name = names;
            //改变Json文件中的数值
            gameobjectList.Add(sceneName, temp);//对加载的资源进行管理
        }
        GameObject one = Instantiate(gameobjectList[sceneName]);
        one.transform.position = GetVector3(pos);
        one.transform.localScale = GetVector3(scale);
        one.transform.eulerAngles = GetVector3(rotate);
        one.name = names;
        one.transform.SetParent(parent);
        return one;

    }

    public Vector3 GetVector3(string str)
    {
        string[] temp = str.Substring(1, str.Length - 2).Split(',');
        float x = float.Parse(temp[0]);
        float y = float.Parse(temp[1]);
        float z = float.Parse(temp[2]);
        Vector3 v = new Vector3(x, y, z);
        return v;
    }

    public float GetVector3_X(string str)
    {
        string[] temp = str.Substring(1, str.Length - 2).Split(',');
        return float.Parse(temp[0]);
    }

    public float GetVector3_Y(string str)
    {
        string[] temp = str.Substring(1, str.Length - 2).Split(',');
        return float.Parse(temp[1]);
    }

    public float GetVector3_Z(string str)
    {
        string[] temp = str.Substring(1, str.Length - 2).Split(',');
        return float.Parse(temp[2]);
    }

    public Vector4 GetColor(string str)
    {
        string[] temp = str.Substring(5, str.Length - 6).Split(',');
        float r = float.Parse(temp[0]);
        float g = float.Parse(temp[1]);
        float b = float.Parse(temp[2]);
        float a = float.Parse(temp[3]);
        Color c = new Color(r, g, b, a);
        return c;
    }

    public void ListToJson(ListTree<Base> listTree)
    {
        if (listTree.Root != null)
        {
            projectJd = new JsonData();
            JsonData objectList = new JsonData();
            objectList.SetJsonType(JsonType.Array);
            foreach (ListTreeNode<Base> objectnode in listTree.Root.children)
            {
                JsonData objectJd = new JsonData();
                ObjectInfo objectinfo = (ObjectInfo)objectnode.data;
                objectJd["name"] = objectinfo.name;
                //objectJd["pos"] = objectinfo.transform.position.ToString();
                //objectJd["scale"] = objectinfo.transform.localScale.ToString();
                //objectJd["rotate"] = objectinfo.transform.eulerAngles.ToString();
                objectJd["modelPath"] = objectinfo.modelPath;
                //objectJd["imagePath"] = objectinfo.imagePath;
                objectJd["IsScene"] = objectinfo.IsScene;
                objectList.Add(objectJd);
            }
            projectJd["name"] = listTree.Root.data.name;
            projectJd["projectId"] = listTree.Root.data.projectId;
            projectJd["data"] = objectList;

        }
    }
    #endregion

    //public void ShowGameObjectProperty()
    //{
    //    if (!isProperty&&Manager.Instace.gonggong!=null)
    //    {

    //        property.transform.GetComponent<RectTransform>().localPosition = new Vector3(176.5f, 186.7f, 0);

    //    }
    //    else
    //    {
    //        property.transform.position = new Vector3(10000, 0, 0);

    //    }
    //    isProperty = !isProperty;
    //}


    public void ShowGameObjectProperty()
    {
        if (gonggong != null)
        {

            property.transform.GetComponent<RectTransform>().localPosition = new Vector3(176.5f, 186.7f, 0);

        }
        else
        {
            property.transform.position = new Vector3(10000, 0, 0);

        }

    }

    public void SetPropertyListShow()
    {
        propertyList.SetActive(true);
    }

    public void UninstallAllAssets()
    {
        foreach (AssetBundle item in assetList)
        {
            item.Unload(true);
        }
    }
    /// <summary>
    /// 判断发布的权限
    /// </summary>
    public void BuildJudge()
    {
        //发布类型的按钮
        GameObject buildType = transform.FindChild("topui/buildType").gameObject;
        //发布权限的提示
        GameObject buildJurisdictionTips = transform.FindChild("topui/buildJurisdictionTips").gameObject;
        if (CanBuild)
        {
            if (buildType != null && !buildType.activeSelf)
            {
                buildType.SetActive(true);
                WaitingClose.Enqueue(buildType);
            }
            //else
            //{
            //    buildType.SetActive(false);
            //}
        }
        else
        {
            if (buildJurisdictionTips != null && !buildJurisdictionTips.activeSelf)
            {
                buildJurisdictionTips.SetActive(true);
            }
            else
            {
                buildJurisdictionTips.SetActive(false);
            }
        }
    }
    /// <summary>
    /// 发布信息
    /// </summary>
	public void Release()
    {
        if (!PEVRRelease.ReleaseManager.Check())
            return;
        //Debug.LogError(VRSwitch.isVR);
        Save(true);
        string path = IOHelper.OpenFileDlgToSave();
        //Debug.LogError(path);
        if (!string.IsNullOrEmpty(path))
        {
            string file = Path.GetDirectoryName(path);
            if (!Directory.Exists(file + "/AssetBundles"))
            {
                Directory.CreateDirectory(file + "/AssetBundles");
            }
            PEVRRelease.ReleaseManager.Release(path,VRSwitch.isVR);
           
            Move(file, Path.GetFileNameWithoutExtension(path));
        }
    }
    

    public void Move(string destPath, string name)
    {
        Debug.Log(destPath+"   "+ name);
        List<string> folders = new List<string>(Directory.GetDirectories("AssetBundles"));
        folders.ForEach(c =>
            {
                string destDir = null;
                destDir = Path.Combine(destPath + "/AssetBundles", Path.GetFileName(c));
                //MoveFolder(c, destDir, name);
                if (!File.Exists(destDir))
                {
                    Directory.CreateDirectory(destDir);
                }
            }
        );
        List<string> files = new List<string>();
        List<string> imgFiles = new List<string>();
        files.Add("AssetBundles/AssetBundles");
        //files.Add("AssetBundles/AssetBundles.manifest");
        foreach (ListTreeNode<Base> a in allDataInformation.listTree.Root.children)
        {
            ObjectInfo temp = (ObjectInfo)a.data;
            files.Add(temp.modelPath);
            imgFiles.Add(temp.imgPath);
            Debug.Log(temp.imgPath+"    "+temp.modelPath);
            //string s = temp.modelPath+".manifest";
            //files.Add(s);
        }
        files.ForEach(c =>
        {
            string destFile = null;
                //destFile = Path.Combine(destPath, Path.GetFileName(c));
                destFile = Path.Combine(destPath, c);
            if (File.Exists(destFile))
            {
                File.Delete(destFile);
            }
            Debug.Log(c + "    " + destFile);
            File.Copy(c, destFile,true);


        }
        );

        imgFiles.ForEach(c =>
        {
            string imgDestFile = null;
            imgDestFile = Path.Combine(destPath, name + "_Data/StreamingAssets" +c);
            if (File.Exists(imgDestFile))
            {
                File.Delete(imgDestFile);
            }
            Debug.Log(c + "    " + imgDestFile);
            File.Copy(Application.streamingAssetsPath+c, imgDestFile,true);
           
        }
        );



    }

    void OnDestroy()
    {
        if (clientSocket != null)
        {
            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
        }

    }
    /// <summary>
    /// 让物体在根目录下显示
    /// </summary>
    public void SetRootParent()
    {
        if (item.isDragging)
        {
            //如果该物体原来有其他的父物体
            if (item.dragedItem.parentName != "Parent")
            {
                GameObject[] Gos = GameObject.FindGameObjectsWithTag("zsgc");
                for (int i = 0; i < Gos.Length; i++)
                {
                    if (item.dragedItem.parentName == Gos[i].name)
                    {
                        for (int j = 0; j < Gos[i].GetComponent<item>().ChildrenName.Count; j++)
                        {
                            if (Gos[i].GetComponent<item>().ChildrenName[j] == item.dragedItem.name)
                            {
                                item _item = Gos[i].GetComponent<item>();
                                _item.ChildrenName.Remove(Gos[i].GetComponent<item>().ChildrenName[j]);
                                _item.ChildList.Remove(Gos[i].GetComponent<item>().ChildList[j]);
                                _item.UpdateChildren();
                                _item.InitChildPosition();
                            }
                        }
                    }
                }
            }
            item.dragedItem.parentName = parent.name;
            item.dragedItem.target.transform.SetParent(parent);
        }
    }

    /// <summary>
    /// 开始协成
    /// </summary>
    public void StartAddComoent()
    {
        StartCoroutine(DoAddCompent());
    }

    /// <summary>
    /// 等待3s添加脚本
    /// </summary>
    /// <returns></returns>
    private IEnumerator DoAddCompent()
    {
        yield return new WaitForSeconds(3);
        cameraEye.AddComponent<HighlightingRenderer>();
    }

    public List<AssetBundle> GetassetList()
    {
        return assetList;
    }

    public void LoadShootingMode(Events actionEvents, string gunName = null)
    {
        //添加枪械模型
        var guntrans = GameObject.Find("FPSController/FirstPersonCharacter").transform;
        //edit by kuai
        GameObject gunPrefab;
        if (gunName != null)
        {
            gunPrefab = Resources.Load<GameObject>("ModelPrefabs/" + gunName);
        }
        else
        {
            gunPrefab = Resources.Load<GameObject>("ModelPrefabs/M16");
        }

        var gunObj = Instantiate(gunPrefab);
        gunObj.transform.SetParent(guntrans, false);
        //edit by kuai
        gunObj.transform.localPosition = new Vector3(0.797f, -0.465f, 1.058f);
        gunObj.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
        //添加射击组件
        var actionHelp = gunObj.AddComponent<ShootingModeHelp>();
        actionHelp.SetShootingMode(actionEvents);
    }

    public void LoadPcShowMsgUIModel(GameObject modelObj, GameObject uiObj, float lineDistance)
    {
        //todo 判断是否是第一人称
        if (FirstPerson.activeSelf)
        {
            if (FirstPersonCamera.GetComponent<CameraDrawLine>() == null)
            {
                FirstPersonCamera.gameObject.AddComponent<CameraDrawLine>();
            }
            var fPcompoent = FirstPersonCamera.gameObject.GetComponent<CameraDrawLine>();
            var helpCompoennt = uiObj.gameObject.AddComponent<PCShowMsgHelpCompoennt>();
            helpCompoennt.SetLineRender(fPcompoent);
            fPcompoent.ImagePos = uiObj;
            fPcompoent.ModelPos = modelObj;
            fPcompoent.SetDistance(lineDistance);
            fPcompoent.SetDrawLine(true);
        }
        else
        {
            //如果不是第一人称
            if (Camera.main.GetComponent<CameraDrawLine>() == null)
            {
                Camera.main.gameObject.AddComponent<CameraDrawLine>();
            }
            var component = Camera.main.gameObject.GetComponent<CameraDrawLine>();
            var helpCompoennt = uiObj.gameObject.AddComponent<PCShowMsgHelpCompoennt>();
            helpCompoennt.SetLineRender(component);
            component.ImagePos = uiObj;
            component.ModelPos = modelObj;
            component.SetDistance(lineDistance);
            component.SetDrawLine(true);
        }
    }


    public GameObject ShopGameObject;
    public void OpenShopPanel(Button btn)
    {
        //判断是否已经登录
        if (string.IsNullOrEmpty(LoginProxy.Proxy.AccountId))
        {
            //未登录提示登录
            LoginPanel.SetActive(true);
            LoginPanel.transform.Find("BackGround").gameObject.SetActive(true);
            LoginPanel.transform.Find("Login").gameObject.SetActive(true);
            LoginPanel.GetComponent<NewLoginPanelComponent>().MessageBox.ShowMassage("请先登录");
        }
        else
        {
            ShopGameObject.SetActive(true);
            ShopManager.Instance.InitShopData(btn.name);
        }
    }



    #region 标题栏打开的界面隐藏

    public Queue<GameObject> WaitingClose = new Queue<GameObject>();
    private bool ClickDown;

    public void LateUpdate()
    {
        if (WaitingClose.Count > 0 && Input.GetMouseButtonDown(0))
        {
            ClickDown = true;
        }
        if (WaitingClose.Count > 0 && Input.GetMouseButtonUp(0) && ClickDown)
        {
            ClickDown = false;
            StartCoroutine(DoWaitClose(WaitingClose));
        }
    }
    IEnumerator DoWaitClose(Queue<GameObject> objs)
    {
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < objs.Count; i++)
        {
            objs.Dequeue().SetActive(false);
        }
    }


    #endregion
}



/// <summary>
/// 记录实际物体与其父物体名字的类
/// </summary>
public class GoAndParentName
{
    /// <summary>
    /// 物体
    /// </summary>
    public GameObject Go;
    /// <summary>
    /// 父物体名
    /// </summary>
    public string parentName;
    public GoAndParentName() { }
    public GoAndParentName(GameObject go, string pName)
    {
        Go = go;
        parentName = pName;
    }
}
/// <summary>
/// 记录属性面板中物体的子物体名字的类
/// </summary>
public class ItemAndChildrenName
{
    public GameObject Item;
    public List<string> childrenNameList;
    public ItemAndChildrenName() { }
    public ItemAndChildrenName(GameObject item, List<string> list)
    {
        Item = item;
        childrenNameList = list;
    }
}