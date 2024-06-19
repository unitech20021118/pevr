using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetailListButtonDrag : MonoBehaviour {

    private float offsetX;

    private float offsetY;

    public Button button;
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
        offsetX = Input.mousePosition.x - transform.position.x;
        offsetY = Input.mousePosition.y - transform.position.y;
        
    }

    public void FollowMouse()
    {
        //transform.position = new Vector3(Input.mousePosition.x - offsetX, Input.mousePosition.y - offsetY, transform.position.z);
        transform.position = Input.mousePosition;
    }

    public void BeginFollowMouse()
    {
        button.enabled = false;
    }

    public void LeaveMouse()
    {
        transform.localPosition = new Vector3(-116.3f, Mathf.Clamp(transform.localPosition.y, -280f, 110f), transform.localPosition.z);
        button.enabled = true;
    }
}
