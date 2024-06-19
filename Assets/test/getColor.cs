using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpringGUI;
public class getColor : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Color a = transform.GetComponent<ColorPickerExample>().ColorPicker.Color;
	}
	
	// Update is called once per frame
	void Update () {
        Color a = transform.GetComponent<ColorPicker>().Color;
        Debug.Log(a);
	}
}
