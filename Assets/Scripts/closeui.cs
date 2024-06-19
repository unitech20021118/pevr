using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class closeui : MonoBehaviour {
    GameObject thisUI;
	// Use this for initialization
	void Start () {
        thisUI = this.transform.parent.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void CloseimgUi()
    {
        thisUI.SetActive(false);
    }
}
