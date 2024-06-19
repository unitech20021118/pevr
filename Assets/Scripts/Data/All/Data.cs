using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : ControlData{
    public static List<GameObjectInfo> gameObjectList = new List<GameObjectInfo>();
    public Data()
    {

    }
    public virtual void Save(object a)
    {
        throw new System.NotImplementedException();
    }

    public virtual void Update(object a)
    {
        throw new System.NotImplementedException();
    }

    public virtual void Delete(object a)
    {
        throw new System.NotImplementedException();
    }

    public virtual void Add(object a)
    {
        throw new System.NotImplementedException();
    }
}
