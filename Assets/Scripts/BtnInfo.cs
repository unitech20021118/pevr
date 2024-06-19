using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnInfo : MonoBehaviour {
	public int id;
	public Text btnText;
	public ShowBtnUI showBtnUI;
	public static GameObject template;

	void Awake(){
		
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public static void SetTemplate(GameObject go){
		template = go;
	}

	public void SetBtnText(string text){
		btnText.text = text;
	}

	public void SetInfo(int id,string t){
		this.id = id;
		SetBtnText (t);
	}
}
