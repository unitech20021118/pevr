using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShowMoreExplain : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Text exText;
    RectTransform A;
    GameObject TextObj;
    Camera cam;
    RectTransform P;
    // Use this for initialization
	void Start () {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();


        TextObj = exText.transform.parent.gameObject;
        A = TextObj.transform.GetComponent<RectTransform>();
        P = transform.GetComponent<RectTransform>();
        TextObj.SetActive(false);
        ExplainText(exText.text);
	}
    void ExplainText(string text)
    {
        Font font = Font.CreateDynamicFontFromOSFont("Arial", 19);
        font.RequestCharactersInTexture(text, 19, 0);
        CharacterInfo characterInfo;
        float width = 0;
        for (int i = 0; i < text.Length; i++)
        {
            font.GetCharacterInfo(text[i], out characterInfo, 19);
            width += characterInfo.advance;
        }
        int count = ((int)width / 245)+1;
        Vector2 Adate=new Vector2(A.sizeDelta.x,A.sizeDelta.y);
        A.sizeDelta = new Vector2(Adate.x, A.sizeDelta.y * count);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        //TextObj.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        //TextObj.SetActive(false);
    }
}
