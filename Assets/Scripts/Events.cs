using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Events{
	public static bool isNetMode=false;//区分是否是网络模式
    public bool golobal = false;
    public event EventHandle a;
    public event EventHandle b;
    public string name;
    public EventsType type;
    public Main target;
    public State<Main> currentState;
    public State<Main> nextState;
	public int keyid;
	static int id = 10000;
	public bool localHappened = false;
	public bool remoteHappened = false;
    
    /// <summary>
    /// 注册事件
    /// </summary>
    public void Register()
    {
        a += ChangeState;
    }

    public void Cancel()
    {
        if (a != null)
        {
            a -= ChangeState;
        }
    }

    public void ChangeState()
    {
        target.GetFSM().ChangeState(nextState);
    }
    /// <summary>
    /// 触发事件
    /// </summary>
    public void Do()
    {

        if (a != null)
        {

            //如果事件的状态和物体的当前状态相符，则实行事件触发
			if (target.GetFSM ().CurrentState () == currentState && currentState.personId.Contains (Manager.Instace.playerMng.GetCurrentRoleId ()) && !remoteHappened) {
				a();
                Debug.Log("wwwwwwww" + Manager.Instace.playerMng.GetCurrentRoleId());
                localHappened = true;
			}
			//来自网络的事件
			else if (target.GetFSM ().CurrentState () == currentState && !currentState.personId.Contains (Manager.Instace.playerMng.GetCurrentRoleId ()) && remoteHappened) {
                Debug.Log("shawanyia" + Manager.Instace.playerMng.GetCurrentRoleId());
				a();
			} else if (target.GetFSM ().CurrentState () == currentState&&!isNetMode) {
				a();
			}
            //a();
        }
    }

    public Events(string t,EventsType eventType)
    {
        id++;
        name = t;
        type = eventType;
    }

    public Events(string name)
    {
        this.name = name;
    }

	public Events(string t,EventsType eventType,int keyID)
	{
		id++;
		name = t;
		type = eventType;
		keyid = keyID;
	}

    //执行B事件
    public void DoRelateToEvents()
    {
        if (b != null)
        {
            b();
        }
    }
    //使该事件关注另一个事件
    public void RegisterTwo(Events events)
    {
        events.b += Do;
    }

}
public enum EventsType
{
    CustomEvents,
    SystemEvents,
    NetworkEvents,
	HTCEvents,
}
