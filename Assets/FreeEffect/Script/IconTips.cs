using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class IconTips : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    
	// Use this for initialization
	void Start () {
        transform.GetChild(0).gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }
   
    public void OnPointerExit(PointerEventData eventData)
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
