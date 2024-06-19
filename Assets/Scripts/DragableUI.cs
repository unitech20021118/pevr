using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragableUI : MonoBehaviour
{
    float offsetX, offsetY;
    /// <summary>
    /// 是否允许被覆盖
    /// </summary>
    //public bool IsCover = true;
    
    
    public bool CanDrag = true;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GetOffset()
    {
        if (CanDrag)
        {
            offsetX = Input.mousePosition.x - transform.position.x;
            offsetY = Input.mousePosition.y - transform.position.y;
        }
    }

    public void FollowMouse()
    {
        if (CanDrag)
        {
            transform.position = new Vector3(Input.mousePosition.x - offsetX, Input.mousePosition.y - offsetY, transform.position.z);
        }
    }
}
