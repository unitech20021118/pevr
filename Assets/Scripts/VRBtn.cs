using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VRBtn : MonoBehaviour {
    public Button btn;
    public bool isClicked;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetClickState(bool isc)
    {
        isClicked = isc;
    }
}
