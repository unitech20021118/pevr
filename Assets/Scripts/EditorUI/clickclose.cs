using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clickclose : MonoBehaviour
{
    public RectTransform ChooseEventPanel;
    public RectTransform CustomEventlist;
    public RectTransform NetworkEventList;
    public RectTransform SystemEventList;
    public Camera camera;
    private bool Close;
    void Update()
    {
        if (gameObject.activeInHierarchy)
        {
            if (Input.GetMouseButtonUp(0))
            {
                Close = true;
                if (ChooseEventPanel.gameObject.activeInHierarchy && RectTransformUtility.RectangleContainsScreenPoint(ChooseEventPanel,Input.mousePosition, camera))
                {
                    Close = false;
                }
                if (CustomEventlist.gameObject.activeInHierarchy && RectTransformUtility.RectangleContainsScreenPoint(CustomEventlist, Input.mousePosition, camera))
                {
                    Close = false;
                }
                if (NetworkEventList.gameObject.activeInHierarchy && RectTransformUtility.RectangleContainsScreenPoint(NetworkEventList, Input.mousePosition, camera))
                {
                    Close = false;
                }
                if (SystemEventList.gameObject.activeInHierarchy && RectTransformUtility.RectangleContainsScreenPoint(SystemEventList, Input.mousePosition, camera))
                {
                    Close = false;
                }
            }
            ClickCloseUI();
        }
        //if (Input.GetMouseButtonUp(0) && gameObject.activeInHierarchy == true)
        //{
           
        //    StartCoroutine(ClickCloseUI());
        //    Debug.LogError("555555555");
        //} 
    }
    public void ClickCloseUI()
    {
        if (Close)
        {
            Manager.Instace.ChooseEventPanel.SetActive(false);
            Manager.Instace.CustomEventlistUI2.SetActive(false);
            Manager.Instace.NetWorkEventListUI.SetActive(false);
            Manager.Instace.SystemEventListUI2.SetActive(false);
            Close = false;
        }
        
    } 
}
