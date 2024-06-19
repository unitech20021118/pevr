using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectIndex : MonoBehaviour {
    public int index;

    public static Dictionary<int, GameObject> GameObjectList = new Dictionary<int, GameObject>();

    void Start()
    {
        GameObjectList.Add(index, gameObject);
    }
}
