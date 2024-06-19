using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TagType : MonoBehaviour
    //, IPointerEnterHandler, IPointerExitHandler
{
    public Sprite unclik, cliked;
    public bool isTage;
    public GameObject inClik; 
     Text childtext;
    /// <summary>
    /// 显示子菜单
    /// </summary>
    
    void Awake()
    {
        childtext = transform.FindChild("Text").GetComponent<Text>();
        //childtext.color = Color.black;
        //childtext.fontStyle = FontStyle.Bold;
        isTage = false;
        gameObject.GetComponent<Image>().sprite = unclik;
    }
    void Update()
    {
        IsClike();
    }
    public void IsClike()
    {
        if (inClik.activeInHierarchy)
        {
            isTage = true;
            gameObject.GetComponent<Image>().sprite = cliked;
            //childtext.fontStyle = FontStyle.Italic;
        }
        else
        {
            isTage = false;
            gameObject.GetComponent<Image>().sprite = unclik;
            //childtext.fontStyle = FontStyle.Normal;
        }
    }


    //public void OnPointerEnter(PointerEventData eventData)
    //{
    //}
    //public void OnPointerExit(PointerEventData eventData)
    //{
    //}
}
    

