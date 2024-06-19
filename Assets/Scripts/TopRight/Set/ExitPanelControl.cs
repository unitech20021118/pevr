using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitPanelControl : MonoBehaviour
{
    /// <summary>
    /// 退出按钮
    /// </summary>
    private Button exitButton;
	// Use this for initialization
	void Start ()
	{
	    exitButton = transform.Find("BtnConform").GetComponent<Button>();
        exitButton.onClick.AddListener(delegate {Application.Quit();});
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
