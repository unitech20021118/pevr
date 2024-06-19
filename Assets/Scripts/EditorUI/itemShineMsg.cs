using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class itemShineMsg : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,IPointerDownHandler
{
    public static itemShineMsg lastitemShineMsg;
    public GameObject _UIShine;
    public GameObject _UIMsg;
    bool isReady;
	// Use this for initialization
	void Start () {
        _UIShine.SetActive(false);
        _UIMsg.SetActive(false);
        _UIMsg.GetComponentInChildren<Text>().text = this.transform.GetChild(2).GetComponent<Text>().text;
        isReady = false;
	}
	
    public void OnPointerEnter(PointerEventData eventData)
    {
        _UIMsg.SetActive(true);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (lastitemShineMsg == null)
        {
            _UIShine.SetActive(true);
        }
        else
        {
            lastitemShineMsg._UIShine.SetActive(false);
            _UIShine.SetActive(true);
        }
        lastitemShineMsg = this;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        _UIMsg.SetActive(false);
    }
}
