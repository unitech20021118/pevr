using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Drag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private bool isDrag = false;
    private Vector3 offset = Vector3.zero;
    
    //拖拉功能
    public void OnBeginDrag(PointerEventData eventData)
    {
        isDrag = false;
        transform.parent.position = transform.position;
        SetDragObjPosition(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {

        isDrag = true;
        transform.parent.position = transform.position;
        
        SetDragObjPosition(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.parent.position = transform.position;
        SetDragObjPosition(eventData);
    }

    void SetDragObjPosition(PointerEventData eventData)
    {
        RectTransform rect = this.GetComponent<RectTransform>();
        Vector3 mouseWorldPosition;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rect, eventData.position, eventData.pressEventCamera, out mouseWorldPosition))
        {
            if (isDrag)
            {
                rect.position = mouseWorldPosition + offset;
            }
            else
            {
                offset = rect.position - mouseWorldPosition;
            }
        }
    }
}
