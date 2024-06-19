using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State<T>{
    
    public string name;
    public string description;
    public T target;
	public List<string> personId=new List<string>();
    public List<Action<T>> onceActionList = new List<Action<T>>();
    public List<Action<T>> updateActionList = new List<Action<T>>();
    public List<Events> eventList = new List<Events>();
    public List<ActionUI> actionUIlist = new List<ActionUI>();
    public int indexnum;//对状态按钮的索引
    public Action<T> EndAction;
	vp_Timer.Handle Timer;
    public State()
    {
        name = "State";
        description = null;
		Manager.Instace.AddDelegate(SetTimerPause);
        //Debug.LogError("11111");

    }

    public void  GetIndex(int num){
        indexnum = num;
    }
    /// <summary>
    /// 进入状态
    /// </summary>
    public virtual void Enter(T entity)
    {
        if (onceActionList != null)
        {
            //foreach (Action<T> a in onceActionList)
            //{
            //    vp_Timer.Handle Timer = new vp_Timer.Handle();
            //    vp_Timer.In(5, new vp_Timer.Callback(() => { Timer.Cancel(); a.DoAction(entity); }), Timer);
            //       //a.DoAction(entity);               
            //}
            First(0, entity);

                    


        }
    }

    void First(int i, T entity)
    {
        //vp_Timer.Handle Timer = new vp_Timer.Handle();
		Timer = new vp_Timer.Handle();
        int tem = i;
        if (i == 0)
        {
            vp_Timer.In(0.0001f, new vp_Timer.Callback(() => { Timer.Cancel(); Next(i, entity); }), Timer);
        }
        else
        {
            vp_Timer.In(onceActionList[i-1].duringTime, new vp_Timer.Callback(() => { Timer.Cancel(); Next(i, entity); }), Timer);
			Timer.Paused = onceActionList[i - 1].pause;
        }
        
    }

    void Next(int i,T entity)
    {    
        if (i < onceActionList.Count)
        {
            onceActionList[i].DoAction(entity);
            First(++i, entity);
        }

        
    }

	public void SetTimerPause()
	{
		if (Timer != null)
		{
			Timer.Paused = false;
		}

	}

    /// <summary>
    /// 执行状态
    /// </summary>
    //public virtual void Execute(T entity)
    public void Execute(T entity)
    {
        if (updateActionList != null)
        {
            //Debug.LogError("updateActionList:"+updateActionList.Count);
            foreach (Action<T> a in updateActionList)
            {
                a.DoAction(entity);
            }
        }
        if (EndAction != null)
        {
            EndAction.DoAction(entity);
        }
    }

    /// <summary>
    /// 退出状态
    /// </summary>
    public virtual void Exit(T entity)
    {

    }
}
