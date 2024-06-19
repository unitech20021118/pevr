using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerGameobject : MonoBehaviour {
    /// <summary>
    /// 需要跟随第一人称的物体的队列
    /// </summary>
    public Queue<GameObject> followGameobject = new Queue<GameObject>();
    /// <summary>
    /// 第一人称
    /// </summary>
    public GameObject firstPerson;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (firstPerson.activeSelf==true)
        {
            if (followGameobject.Count>0)
            {
                followGameobject.Dequeue().transform.SetParent(firstPerson.transform);
                
            }
        }
	}

    /// <summary>
    /// 添加跟随物体
    /// </summary>
    public void AddFollowGameObject(GameObject go)
    {
        followGameobject.Enqueue(go);
    }
}
