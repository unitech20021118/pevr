using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnType : MonoBehaviour {
    public static BtnType lastBtn;
    public GameObject panel;
	// Use this for initialization

    public void OnClick()
    {
        if (lastBtn != null)
        {
            lastBtn.panel.SetActive(false);
        }
        panel.SetActive(true);
        lastBtn = this;
    }
}
