using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class G_ScaleSliderText : MonoBehaviour {

    public Text text;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        text.text = transform.GetComponent<Slider>().value.ToString();
	}
}
