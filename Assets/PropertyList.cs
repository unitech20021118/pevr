using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropertyList : MonoBehaviour ,WindowInterface{

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
