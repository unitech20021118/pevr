using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class AddTransition : MonoBehaviour,IPointerEnterHandler{
    public GameObject eventList;
    public void OnPointerEnter(PointerEventData eventData)
    {
        
        eventList.SetActive(true);
        RectTransform  rect = transform.GetComponent<RectTransform>();
        float offset = rect.sizeDelta.x-10;
        eventList.transform.position = transform.position;
    }
}
