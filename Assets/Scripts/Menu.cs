using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Menu : MonoBehaviour {
    private Transform currentState;
   
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    /// <summary>
    /// 设置当前状态
    /// </summary>
    /// <param name="t"></param>
    public void SetCurrentState(Transform t)
    {
        currentState = t;
    }

    /// <summary>
    /// 添加事件
    /// </summary>
    public void AddEvent(string name, EventsType eventType)
    {
        GameObject even = Manager.Instace.CreateEventButton();
        even.GetComponentInChildren<Text>().text = name;
        even.transform.SetParent(currentState.transform.parent);
        even.transform.localScale = Vector3.one;
        even.transform.localPosition = new Vector3(even.transform.localPosition.x, even.transform.localPosition.y, 0f);
        State<Main> s = currentState.GetComponentInChildren<StateNode>().state;
        switch (eventType)
        {
            case EventsType.CustomEvents:
                //则查找自定义事件列表
                foreach (Events e in Manager.Instace.eventlist)
                {
                    if (name == e.name)
                    {

                        
                        //if(e.golobal){
                        //    even.GetComponent<EventNode>().InitialGlobalEvent(e);
                        //}
                        //else{
                        //    even.GetComponent<EventNode>().InitialEvent(e);
                        //}
                        Events events =even.GetComponent<EventNode>().InitialEvent(e);

                        events.currentState = s;//设置事件的当前状态
                        s.eventList.Add(events);
                        if (Manager.Instace.eventRegisterList.ContainsKey(e))
                        {
                            Manager.Instace.eventRegisterList[e].Add(events);
                        }
                        else
                        {
                            List<Events> list = new List<Events>();
                            list.Add(events);
                            Manager.Instace.eventRegisterList.Add(e, list);
                        }
                        
                    }
                }
                break;
            case EventsType.NetworkEvents:
                break;
		case EventsType.SystemEvents:
                //则查找系统事件列表
			Events a = even.GetComponent<EventNode> ().InitialEvent (name, eventType,s);
			a.currentState = s;//设置事件的当前状态
			s.eventList.Add (a);
			if (a.name == "鼠标左键按下") {
				MouseDown m = new MouseDown();
				m.even = a;

				m.Ini (Manager.Instace.ctrllerEventsR, Manager.Instace.VRPointerR);

				s.updateActionList.Add (m);
                    
				Manager.Instace.currentMain.SetStart(s);

			}
            if (a.name == "结束事件")
            {
                FinishAction f = new FinishAction();
                    Debug.LogError("Menu");
                f.even = a;
                //s.updateActionList.Add(f);
                s.EndAction = f;
                Manager.Instace.currentMain.SetStart(s);
            }

			if (a.name == "按钮按下") {
				BtnToNext m = new BtnToNext();
				ShowBtn showBtn = null;
				m.even = a;
				s.updateActionList.Add(m);
				foreach (Action<Main> item in s.updateActionList) {
					try {
						showBtn = (ShowBtn)item;
						BtnInfo btnInfo = Manager.Instace.OnePointColliderObject ().GetComponent<BtnInfo> ();
						if (btnInfo.id == showBtn.id) {
							btnInfo.showBtnUI.SaveTargetID (s.eventList.Count - 1);
							break;
						}
					} catch {
						
					}
				}
				if (showBtn != null) {
					showBtn.tarid = s.eventList.Count - 1;
					m.vrBtn = showBtn.currentUI.GetComponent<VRBtn>();
				}
				Manager.Instace.currentMain.SetStart(s);

			}

			if (a.name.Contains("按键按下")) {
				KeyDown keyDown = new KeyDown();
				keyDown.even = a;
				s.updateActionList.Add (keyDown);
				a.keyid = KeyDown.currentKeyID;
				keyDown.keyCode = (KeyCode)a.keyid;
				Manager.Instace.currentMain.SetStart(s);
			}
		    if (a.name.Contains("鼠标左键长按"))
		    {
		        MouseDownLong m = new MouseDownLong();
		        m.even = a;

		        //m.Ini(Manager.Instace.ctrllerEventsR, Manager.Instace.VRPointerR);

		        s.updateActionList.Add(m);

		        Manager.Instace.currentMain.SetStart(s);
            }
		    if (a.name.Contains("鼠标左键松开"))
		    {
		        MouseUp m = new MouseUp();
		        m.even = a;

		        s.updateActionList.Add(m);

		        Manager.Instace.currentMain.SetStart(s);
		    }
		    if (a.name.Contains("鼠标进入"))
		    {
		        MouseEnter m = new MouseEnter();
		        m.even = a;

		        s.updateActionList.Add(m);

		        Manager.Instace.currentMain.SetStart(s);
		    }
		    if (a.name.Contains("鼠标离开"))
		    {
		        MouseExit m = new MouseExit();
		        m.even = a;

		        s.updateActionList.Add(m);

		        Manager.Instace.currentMain.SetStart(s);
		    }
		    if (a.name.Contains("鼠标右键按下"))
		    {
		        RMouseDown m = new RMouseDown();
		        m.even = a;

		        s.updateActionList.Add(m);
                }
                break;
		//---------------------------------------------------
		case EventsType.HTCEvents:
			Events htce = even.GetComponent<EventNode> ().InitialEvent (name, eventType,s);
			htce.currentState = s;//设置事件的当前状态
			s.eventList.Add (htce);
			if (htce.name == "手柄激光点击") {
				PointerSelected ps = new PointerSelected ();
				ps.even = htce;
				ps.Ini (Manager.Instace.ctrllerEventsR, Manager.Instace.VRPointerR);
				s.updateActionList.Add (ps);
				Manager.Instace.currentMain.SetStart (s);
			}
			if (htce.name == "手柄接触") {
				ControllerInteract ci = new ControllerInteract ();
				ci.Ini ();
				ci.SetType (ControllerInteract.InteractType.touch);
				ci.even = htce;
				s.updateActionList.Add (ci);
				Manager.Instace.currentMain.SetStart (s);
			}
			if (htce.name == "手柄停止接触") {
				ControllerInteract ci = new ControllerInteract ();
				ci.Ini ();
				ci.SetType (ControllerInteract.InteractType.untouch);
				ci.even = htce;
				s.updateActionList.Add (ci);
				Manager.Instace.currentMain.SetStart (s);
			}
			if (htce.name == "手柄抓取") {
				ControllerInteract ci = new ControllerInteract ();
				ci.Ini ();
				ci.SetType (ControllerInteract.InteractType.grab);
				ci.even = htce;
				s.updateActionList.Add (ci);
				Manager.Instace.currentMain.SetStart (s);
			}
			if (htce.name == "手柄停止抓取") {
				ControllerInteract ci = new ControllerInteract ();
				ci.Ini ();
				ci.SetType (ControllerInteract.InteractType.ungrab);
				ci.even = htce;
				s.updateActionList.Add (ci);
				Manager.Instace.currentMain.SetStart (s);
			}
			if (htce.name == "物体划动") {
				ControllerInteract ci = new ControllerInteract ();
				ci.Ini ();
				ci.SetType (ControllerInteract.InteractType.moveThrough);
				ci.even = htce;
				s.updateActionList.Add (ci);
				Manager.Instace.currentMain.SetStart (s);
			}
			break;
		//------------------------------------------------------
        }
        //Debug.Log(currentState.name);
        //GameObject even = Manager.Instace.CreateEventButton();
        //even.transform.localPosition = currentState.transform.parent.position;
        //even.transform.SetParent(currentState.transform.parent);
        //even.transform.localScale = Vector3.one;
        
        //even.GetComponentInChildren<Text>().text = name;
        //Events e = even.GetComponent<EventNode>().InitialEvent(name);
        
        //State<Main> s = currentState.GetComponentInChildren<StateNode>().state;
        //s.eventList.Add(e);
        //if (e.type == "鼠标左键按下")
        //{
        //    MouseDown m = new MouseDown();
        //    m.even = e;
        //    s.updateActionList.Add(m);
        //    Manager.Instace.currentMain.SetStart(s);//更新
        //}

        //FSMInfo FsmData = new FSMInfo(currentMain.Fsm.name);
        //currentStateStart.AddComponent<GameObjectIndex>().index = FsmData.index;
        //Base.allData.Add(FsmData);
        //ListTreeNode<Base> parent = listTree.GetNode(Base.FindData(gonggong.GetComponent<GameObjectIndex>().index));
        //listTree.AddLeave(parent, FsmData);

		EventInfo EventData = new EventInfo(name,eventType,KeyDown.currentKeyID);
        //even.AddComponent<GameObjectIndex>().index = EventData.index;
        //Informa<Base>.allData.Add(EventData);
        Manager.Instace.dictFromObjectToInforma.Add(even, EventData);
        ListTreeNode<Base> parent = Manager.Instace.allDataInformation.listTree.GetNode(Manager.Instace.dictFromObjectToInforma[ currentState.transform.parent.gameObject]);
        Manager.Instace.allDataInformation.listTree.AddLeave(parent, EventData);

        gameObject.SetActive(false);
        Manager.Instace.EventListUI.SetActive(false);
        Manager.Instace.SystemEventListUI.SetActive(false);
        Manager.Instace.CustomEventlistUI.SetActive(false);
		//---------------------------------------------------
		Manager.Instace.HTCEventListUI.SetActive (false);
		//---------------------------------------------------
    }
}
