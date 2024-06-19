using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolControl : MonoBehaviour
{

    //private bool isOpen;

    public GameObject toolbox;
    public List<GameObject> tool_child_box;
    void Start ()
	{

	    //isOpen = false;
	}
	
	
	void Update () {
		
	}

    public void onClick()
    {
        //isOpen = !isOpen;
        //toolbox.SetActive(isOpen);
        //bg.SetActive(isOpen);
        //if (isOpen == false)
        //{
        //    foreach (var n in tool_child_box)
        //    {
        //        n.SetActive(false);   
        //    }
        //}
        toolbox.SetActive(true);
        Manager.Instace.WaitingClose.Enqueue(toolbox);
    }
}
