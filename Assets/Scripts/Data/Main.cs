using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {

    public StateMachine<Main> m_StateMachine;
    public GameObject Fsm;
    private State<Main> start;
    public bool IsEnter = false;
    public bool IsStay = false;
    public bool isExit = false;

	void OnEnable(){
		if (ResLoader.isBack) {
			Start ();
		}
	}
	// Use this for initialization
	void Start () {
        m_StateMachine = new StateMachine<Main>(this);
        //m_StateMachine.SetGlobalState(GlobalState.Instance());
        //设置初始状态
        //State<Main> s = Manager.Instace.currentStateStart.GetComponentInChildren<StateNode>().state;
        m_StateMachine.SetCurrentState(start);
        
	}

    void LateUpdate()
    {
        IsEnter = false;
        IsStay = false;
        isExit = false;
    }
	// Update is called once per frame
	void Update () {
        if (IsEnter)
        {
            //Debug.Log(111);
        }
    m_StateMachine.SMUpdate();
        
	}

    public StateMachine<Main> GetFSM()
    {
        return m_StateMachine;

    }
    public State<Main> SetStart(State<Main> s)
    {
        if (start == null)
        {
            start = s;
        }
        return start;
    }

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("进入");
        IsEnter = true;
        //IsStay = false;
        //isExit = false;
    }

    void OnTriggerExit(Collider other)
    {
        //Debug.Log("离开");
        //IsEnter = false;
        //IsStay = false;
        isExit = true;
    }

    void OnTriggerStay(Collider other)
    {
       // Debug.Log("持续");
//        Debug.Log("出去");
        //IsEnter = false;
        IsStay = true;
        //isExit = false;
    }
    void OnCollisionEnter(Collision other)
    {
        //Debug.Log("Enter");
        IsEnter = true;
    }
    void OnCollisionStay(Collision other)
    {
        //Debug.Log("Stay");
        IsStay = true;
    }
    void OnCollisionExit(Collision other)
    {
        //Debug.Log("Exit");
        isExit = true;
    }
}
