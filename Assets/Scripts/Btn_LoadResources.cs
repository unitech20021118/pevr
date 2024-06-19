using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Btn_LoadResources : MonoBehaviour {
    public GameObject img_Rescources;

    public void OpenResources()
    {
        img_Rescources.SetActive(true);
        gameObject.SetActive(false);
    }
}
