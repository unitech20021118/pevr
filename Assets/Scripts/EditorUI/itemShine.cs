using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class itemShine : MonoBehaviour,IPointerDownHandler
{
    public static itemShine lastitem;
    public GameObject _UIShine;
    Text _text;
    public bool _islight;
	// Use this for initialization
	void Start () {
        _UIShine.SetActive(false);
        _text = transform.parent.FindChild("TextName").GetComponentInChildren<Text>();
        _text.color = Color.white;
	}
    void Update()
    {
            if (_islight)
            {
                _UIShine.SetActive(true);
                _text.color = Color.black;
            }
            if (!_islight)
            {
                _UIShine.SetActive(false);
                _text.color = Color.white;
            }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (lastitem == null)
        {
            _UIShine.SetActive(true);
            _text.color = Color.black;
        }
        else
        {
            lastitem._UIShine.SetActive(false);
            lastitem._text.color = Color.white;
            _UIShine.SetActive(true);
            _text.color = Color.black;
        }
        lastitem = this;
    }
}
