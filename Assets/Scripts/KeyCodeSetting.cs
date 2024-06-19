using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class KeyCodeSetting : MonoBehaviour {
	public RectTransform TipUI;
	public KeyCode keyCode;
	public Transform parent;
	public EventsType eventType;
	private bool isGettingKey;
	private GameObject tempUI;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (isGettingKey&&Input.anyKeyDown)
        {
			foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
            {
				if (Input.GetKeyDown (key))
                {
					print (key);
					keyCode = key;
					KeyDown.currentKeyID = (int)keyCode;
					isGettingKey = false;
					Manager.Instace.Menu.GetComponent<Menu>().AddEvent(keyCode.ToString () + "按键按下", eventType);
					Destroy (tempUI);
				}
			}
		}
	}

	public void CreateTip(){
		tempUI = Instantiate<GameObject> (TipUI.gameObject, parent);
		tempUI.GetComponent<RectTransform> ().localScale = Vector3.one;
		tempUI.GetComponent<RectTransform> ().localPosition = Vector3.zero;
		isGettingKey = true;
	}


}
