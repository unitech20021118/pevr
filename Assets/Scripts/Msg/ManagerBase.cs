using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerBase :MonoBase {

    private Dictionary<int, List<MonoBase>> dict = new Dictionary<int, List<MonoBase>>();

    public override void Execute(int eventCode, object message)
    {
        if (!dict.ContainsKey(eventCode))
        {
            Debug.Log("没有注册");
            return;
        }

        List<MonoBase> list = dict[eventCode];
        for (int i = 0; i < list.Count; i++)
        {
            list[i].Execute(eventCode, message);
        }
    }

    public void Add(int eventCode, MonoBase mono)
    {
        List<MonoBase> list = null;
        if (!dict.ContainsKey(eventCode))
        {
            list = new List<MonoBase>();
            list.Add(mono);
            dict.Add(eventCode, list);
            return;
        }
        list = dict[eventCode];
        list.Add(mono);
    }

    public void Add(int[] eventCodes, MonoBase mono)
    {
        for (int i = 0; i < eventCodes.Length; i++)
        {
            Add(eventCodes[i], mono);
        }
    }

    public void Remove(int eventCode, MonoBase mono)
    {
        if (!dict.ContainsKey(eventCode))
        {
            Debug.Log("are you kidding");
            return;
        }
        List<MonoBase> list = dict[eventCode];
        if (list.Count == 1)
        {
            dict.Remove(eventCode);
        }
        else
        {
            list.Remove(mono);
        }
    }

    public void Remove(int[] eventCode, MonoBase mono)
    {
        for (int i = 0; i < eventCode.Length; i++)
        {
            Remove(eventCode[i], mono);
        }
    }
}
