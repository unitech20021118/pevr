using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureChartSizeControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
			transform.localScale = new Vector3(transform.localScale.x * 0.9f, transform.localScale.y * 0.9f, 1);
        }
        else if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
			transform.localScale = new Vector3(transform.localScale.x * 1.1f, transform.localScale.y * 1.1f, 1);
        }
	}
}
