using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public delegate void EventHandle();

public class StateNode : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler,IPointerDownHandler
{

    public static Dictionary<State<Main>, GameObject> dict = new Dictionary<State<Main>, GameObject>();
    public int id;
    public string name;
    public string description;
    public Text text;
    private bool isDrag = false;
    private Vector3 offset = Vector3.zero;
    public State<Main> state;
    public GameObject line;
    /// <summary>
    /// 这个状态机附着的物体
    /// </summary>
    public Transform TargetTransform;
   
    /// <summary>
    /// 属性面板的状态按钮
    /// </summary>
    private GameObject statePanelBtn;

	// Use this for initialization
	void Start () {
        if (state == null)
        {
            state = new State<Main>();
        }        
        transform.GetComponent<Button>().onClick.AddListener(SendStateTOEditorPanel);
        state.name = GetComponentInChildren<Text>().text;
	    if (!dict.ContainsKey(state))
	    {
	        dict.Add(state, gameObject);
        }
        
        //获取属性面板的状态按钮
        if (statePanelBtn==null)
        {
            statePanelBtn = GameObject.Find("EditorCanvas/EditorPanel/Panel/State");
            
        }
	}

    public State<Main> Initial()
    {
        state = new State<Main>();
        if (!dict.ContainsKey(state))
        {
            dict.Add(state, gameObject);
        }
        return state;
    }

 

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 temp = Manager.Instace.ScreenPointToWorldPos(Manager.Instace.StateMachineCanvas.GetComponent<RectTransform>(), Manager.Instace.StateMachineCamera);
            Manager.Instace.ShowMenu(transform, temp, Manager.Instace.StateMachineCanvas);
        }

    }

    //拖拉功能
    public void OnBeginDrag(PointerEventData eventData)
    {
        isDrag = false;
        transform.parent.position = transform.position;
        SetDragObjPosition(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {

            isDrag = true;

            transform.parent.position = transform.position;
        SetDragObjPosition(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.parent.position = transform.position;
        SetDragObjPosition(eventData);
    }

    void SetDragObjPosition(PointerEventData eventData)
    {
        RectTransform rect = this.GetComponent<RectTransform>();
        Vector3 mouseWorldPosition;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rect, eventData.position, eventData.pressEventCamera, out mouseWorldPosition))
        {
            if (isDrag)
            {
                rect.position = mouseWorldPosition + offset;
            }
            else
            {
                offset = rect.position - mouseWorldPosition;
            }
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

    /// <summary>
    /// 按下按钮将State传到StatePanel上
    /// </summary>
    public void SendStateTOEditorPanel()
    {
        //将该状态按钮发送给elastic2nd
        Manager.Instace._E2ND.SetCurrentStateBtn(this.gameObject);
        if (Manager.Instace.StatePanel.GetComponent<StatePanelUI>().currentState != null)
        {
            //如果不是一个状态，则更新StatePanel
            if (Manager.Instace.StatePanel.GetComponent<StatePanelUI>().currentState != state)
            {
                Manager.Instace.StatePanel.GetComponent<StatePanelUI>().UnableCurrentActionlist();

                Manager.Instace.SendState(state,this);
            }
            if (Manager.Instace.StatePanel.GetComponent<StatePanelUI>().currentState == state)
            {
                Manager.Instace.SendState(state, this);
               

            }
        }
        else
        {
            Manager.Instace.SendState(state,this);
        }
        Manager.Instace.ActionList.GetComponent<ActionList>().currentState = state;
        //顺便打开statepanel
        statePanelBtn.GetComponent<BtnType>().OnClick();
    }

    public void OnBecameInvisible()
    {
        Debug.Log(111);
    }

    //void OnDestroy()
    //{
    //  Base a=Manager.Instace.dictFromObjectToInforma[transform.parent.gameObject];
    //  ListTreeNode<Base> node = Manager.Instace.listTree.GetNode(a);
    //  StateInfo stateInfo = (StateInfo)node.data;
    //  foreach (ActionInforma i in stateInfo.actionList)
    //  {
    //      Destroy(Manager.Instace.actionInfomaToGameobject[i]);
    //  }
    //  Manager.Instace.DestroyData(node);
    //}
}
