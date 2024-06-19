using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonUpDown : MonoBehaviour {
    public GameObject Btns;
	// Use this for initialization
    public void BtnUp()
    {
        RectTransform bt = Btns.GetComponent<RectTransform>();
        if (bt.transform.position.y <680)
        {
            bt.transform.position = new Vector3(bt.transform.position.x, bt.transform.position.y + 27, 0);
        }
    }
    public void BtnDown()
    { 
        RectTransform bt = Btns.GetComponent<RectTransform>();
        if (bt.transform.position.y > 600)
        {
            bt.transform.position = new Vector3(bt.transform.position.x, bt.transform.position.y - 27, 0);
        }
    }
}
