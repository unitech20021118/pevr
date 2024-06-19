using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectId : MonoBehaviour {
    public static int id = 0;
    public static List<int> list=new List<int>();
    public int TransformId;
    public string TransformName;
    public static int GetTransformId()
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] == -1)
            {
                list[i] = list[i - 1] + 1;
                return list[i];
            }
        }
        id = id + 1;
        list.Add(id);
        return id;
    }


    void OnDestroy()
    {
        for (int i = 0; i < list.Count;i++ )
        {
            if (list[i]== TransformId)
            {
                list[i] = -1;
            }
        }
    }
}
