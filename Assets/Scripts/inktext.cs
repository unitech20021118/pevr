using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class inktext : MonoBehaviour {
    Text inkText;
    Image inkImage;
	// Use this for initialization
	void Start () {
        inkText = this.GetComponent<Text>();
        inkImage=this.transform.parent.FindChild("Image").GetComponent<Image>();
        inkText.text = inkImage.sprite.name;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
