using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<Main> {

    private Main mOwner;
    private State<Main> m_CurrentState;
    private State<Main> m_PreviousState;
    private State<Main> m_GlobalState;


    public StateMachine(Main owner)
    {
        mOwner = owner;
        m_CurrentState = null;
        m_PreviousState = null;
        m_GlobalState = null;
    }

    public void GlobalStateEnter()
    {
        m_GlobalState.Enter(mOwner);
    }

    public void SetGlobalState(State<Main> GlobalState)
    {
        m_GlobalState = GlobalState;
        m_GlobalState.target = mOwner;
        m_GlobalState.Enter(mOwner);
    }

    public void SetCurrentState(State<Main> CurrentState)
    {
        m_CurrentState = CurrentState;
        Debug.Log(mOwner);
        m_CurrentState.target = mOwner;        
        m_CurrentState.Enter(mOwner);
    }

    public void SMUpdate()
    {
        if (m_GlobalState != null)
        {
            m_GlobalState.Execute(mOwner);
        }
        if (m_CurrentState != null)
        {
            m_CurrentState.Execute(mOwner);
        }
    }

    public void ChangeState(State<Main> newState)
    {
        if (newState == null)
        {
            Debug.LogError("没有发现状态");
        }
       
        m_CurrentState.Exit(mOwner);
        m_PreviousState = m_CurrentState;
        m_CurrentState = newState;
        m_CurrentState.target = mOwner;
        m_CurrentState.Enter(mOwner);
    }

    public void RevertToPreviousState()
    {
        ChangeState(m_PreviousState);
    }

    public State<Main> CurrentState()
    {
        return m_CurrentState;
    }

    public State<Main> GlobalState()
    {
        return m_GlobalState;
    }

    public State<Main> PreviousState()
    {
        return m_PreviousState;
    }
}
