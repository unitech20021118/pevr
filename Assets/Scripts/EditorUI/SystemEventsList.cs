using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class SystemEventsList : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public GameObject systemEventList;
    public void OnPointerEnter(PointerEventData eventData)
    {

        systemEventList.SetActive(true);
        RectTransform rect = transform.GetComponent<RectTransform>();

        systemEventList.transform.position = transform.position;
        transform.GetComponent<Image>().color = Color.blue;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.GetComponent<Image>().color = Color.white;
    }

}
