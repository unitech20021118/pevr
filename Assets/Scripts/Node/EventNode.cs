using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class EventNode : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //edit by 吕存全
    //public int id;
    public string id;
    //---------
    public string name;

    Vector3 posFirst;
    Vector3 posSecond;
    public GameObject line;
    public Events even;

    public State<Main> state;
    
	// Use this for initialization
	void Start () {
        //edit by 吕存全
        id = (10000 + GameObjectId.id + Manager.Instace.playerMng.transformDict.Count).ToString();
        Manager.Instace.playerMng.transformDict.Add(id, transform);
        //------------
    }
	
	// Update is called once per frame
	void Update () {
        //edit by 吕存全
        //print(gameObject.name);
        if (even != null && even.localHappened)
        {        
            Debug.Log("Sending netevent func");
            Manager.Instace.GetComponent<SendNetEventRequest>().SendRequest(id);
            even.localHappened = false;
        }
        //------------
    }

    /// <summary>
    /// 初始化事件
    /// </summary>
    public Events InitialEvent(string name, EventsType eventType,State<Main> state)
    {
        print(name + "^^^^^" + eventType.ToString());
        even = new Events(name, eventType);
        this.state = state;
        even.target = Manager.Instace.currentMain;
        return even;
    }

    //public void InitialEvent(Events e)
    //{
    //    even = e;

    //}

    public Events InitialEvent(Events e)
    {
        print(name + "&&&&&&&");
        even = new Events(e.name);
        //新事件关注全局事件
        //even.RegisterTwo(e);
        return even;
    }
    /// <summary>
    /// 开始拖动
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (line != null)
        {
            //Base data = Base.FindData(line.GetComponent<GameObjectIndex>().index);
            Base data =Manager.Instace.dictFromObjectToInforma[line];
            ListTreeNode<Base> parent = Manager.Instace.allDataInformation.listTree.GetNode(data).parent;
            Manager.Instace.allDataInformation.listTree.DeleteNode(parent, data);
            line.GetComponent<Line>().events.Cancel();
            Destroy(line);
        }
        line = new GameObject();
        line.transform.SetParent(Manager.Instace.gonggong.GetComponent<Main>().Fsm.transform);
        LineRenderer lineRender = line.AddComponent<LineRenderer>();        
        Line l=line.AddComponent<Line>();
        l.lr = lineRender;
        //posFirst = Manager.Instace.UIScreenToWorldPoint(this.GetComponent<RectTransform>(), gameObject);
        //lineRender.SetPosition(0, posFirst);
        lineRender.SetPosition(0, transform.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        //posSecond = Manager.Instace.UIScreenToWorldPoint(this.GetComponent<RectTransform>());
        //line.GetComponent<LineRenderer>().SetPosition(1, posSecond);
        line.GetComponent<LineRenderer>().SetPosition(1, Manager.Instace.ScreenPointToWorldPos(Manager.Instace.StateMachineCanvas.GetComponent<RectTransform>(),Manager.Instace.StateMachineCamera));
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        if (results.Count > 1 && results[1].gameObject.tag=="State")
        {
            //posSecond = Manager.Instace.UIScreenToWorldPoint(results[1].gameObject.GetComponent<RectTransform>(), results[1].gameObject);
            //line.GetComponent<LineRenderer>().SetPosition(1, posSecond);
            line.GetComponent<LineRenderer>().SetPosition(1, results[1].gameObject.transform.position);
            line.GetComponent<Line>().eventnode = this;
            line.GetComponent<Line>().statenode=results[1].gameObject.GetComponent<StateNode>();
            line.GetComponent<Line>().SetEvents(even);
            even.nextState = results[1].gameObject.GetComponent<StateNode>().state;
            results[1].gameObject.GetComponent<StateNode>().line = line;
            even.target = Manager.Instace.currentMain;
            even.Register();

            //FSMInfo FsmData = new FSMInfo(currentMain.Fsm.name);
            //currentStateStart.AddComponent<GameObjectIndex>().index = FsmData.index;
            //Base.allData.Add(FsmData);
            //ListTreeNode<Base> parent = listTree.GetNode(Base.FindData(gonggong.GetComponent<GameObjectIndex>().index));
            //listTree.AddLeave(parent, FsmData);
           // StateInfo stateInfo=(StateInfo) Base.FindData(results[1].gameObject.transform.parent.GetComponent<GameObjectIndex>().index);
            StateInfo stateInfo = (StateInfo)Manager.Instace.dictFromObjectToInforma[results[1].gameObject.transform.parent.gameObject];
            LineInfo LineData = new LineInfo(stateInfo);
            //line.AddComponent<GameObjectIndex>().index = LineData.index;
            //line.GetComponent<Line>().id = LineData.index;
            //Informa<Base>.allData.Add(LineData);
            Manager.Instace.dictFromObjectToInforma.Add(line, LineData);
            ListTreeNode<Base> parent=Manager.Instace.allDataInformation.listTree.GetNode(Manager.Instace.dictFromObjectToInforma[this.gameObject]);
            Manager.Instace.allDataInformation.listTree.AddLeave(parent, LineData);
        }
        else
        {
            
            Destroy(line);
            
        }
    }

    /// <summary>
    /// 获得世界坐标
    /// </summary>
    /// <returns></returns>
    //public Vector3 FindWorldPos()
    //{
    //    return Manager.Instace.UIScreenToWorldPoint(this.GetComponent<RectTransform>(), gameObject);
    //}

}
