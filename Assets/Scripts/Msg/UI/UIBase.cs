using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBase : MonoBase {

    List<int> list = new List<int>();

    protected void Bind(params int[] eventCodes)
    {
        list.AddRange(eventCodes);
        UIManager.Instance.Add(list.ToArray(), this);
    }

    protected void UnBind()
    {
        UIManager.Instance.Remove(list.ToArray(), this);
    }

    public virtual void OnDestroy()
    {
        if (list != null)
        {
            UnBind();
        }
    }

    public void Dispatch(int areaCode, int eventCode, object message)
    {
        MsgCenter.Instance.Dispatch(areaCode, eventCode, message);
    }
}
