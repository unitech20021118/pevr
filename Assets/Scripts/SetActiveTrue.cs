using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveTrue : MonoBehaviour {
    /// <summary>
    /// 返回第一个场景的按钮
    /// </summary>
    public GameObject ReturnFirstSceneButton;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (ReturnFirstSceneButton.activeSelf==false)
        {
            ReturnFirstSceneButton.SetActive(true);
        }
	}
}
