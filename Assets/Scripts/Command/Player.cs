using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    /// <summary>
    /// 角色移动事件
    /// </summary>
    /// <param name="pos"></param>
    public delegate void MoveDelegate(Vector3 pos);
    public event MoveDelegate MoveEvent;

    public void Move(Vector3 pos)
    {
        if (MoveEvent!=null)
        {
            MoveEvent(pos);
        }
    }
    public void MovePlayer(Vector3 pos)
    {
        this.transform.position = pos;
    }
    
}
