using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseHoverTip : MonoBehaviour ,IPointerEnterHandler,IPointerExitHandler
{
	public string TipString;
    private static GameObject mouseHoverTipObj;

    public static GameObject MouseHoverTipObj 
	{
        get
        {
            if (!mouseHoverTipObj)
            {
                mouseHoverTipObj = Instantiate(Resources.Load<GameObject>("TipUI/MouseHoverTip"));
            }
            return mouseHoverTipObj;
        }
	}

    // Use this for initialization
    void Start ()
	{
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void OnPointerEnter(PointerEventData eventdata)
    {
        MouseHoverTipObj.SetActive(true);
        mouseHoverTipObj.transform.SetParent(transform.parent);
        mouseHoverTipObj.GetComponent<RectTransform>().localPosition = new Vector3(transform.GetComponent<RectTransform>().localPosition.x, transform.GetComponent<RectTransform>().localPosition.y+ transform.GetComponent<RectTransform>().sizeDelta.y / 2, transform.GetComponent<RectTransform>().localPosition.z);
        mouseHoverTipObj.transform.Find("Text").GetComponent<Text>().text = TipString;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        MouseHoverTipObj.SetActive(false);
    }
}
