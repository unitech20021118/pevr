using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopUi : MonoBehaviour {
	public GameObject btnTemplate;

	// Use this for initialization
	void Start () {
        transform.SetAsLastSibling();
		BtnInfo.SetTemplate (btnTemplate);
	}
	
	// Update is called once per frame
	void Update () {
        transform.SetAsLastSibling();
	}
}
