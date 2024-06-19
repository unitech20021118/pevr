using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class buttonShine : MonoBehaviour,IPointerDownHandler
{
    public static buttonShine lastbutton;
    Image _image;
	// Use this for initialization
	void Start () {
        _image = this.GetComponentInChildren<Image>();
        _image.color = Color.white;
	}
    public void OnPointerDown(PointerEventData eventData)
    {
        if (lastbutton == null)
        {
            _image.color = new Color(1f, 0.7f, 0, 1);
        }
        else
        {
            lastbutton._image.color = Color.white;
            _image.color = new Color(1f, 0.7f, 0, 1);
        }
        lastbutton = this;
    }
}
