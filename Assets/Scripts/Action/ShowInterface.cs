using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowInterface : Action<Main>
{
    public List<InterfaceQuality> InterfaceQualitys = new List<InterfaceQuality>();
    public List<int> CloseIndexList = new List<int>();
    public override void DoAction(Main m)
    {
        if (InterfaceQualitys.Count>0)
        {
            for (int i = 0; i < InterfaceQualitys.Count; i++)
            {
                if (InterfaceQualitys[i].Pevrui == PEVRUI.Text && InterfaceQualitys[i].TextQuality.TextUiGameObject!=null)
                {
                    InterfaceQualitys[i].TextQuality.TextUiGameObject.GetComponent<Text>().color =
                        InterfaceQualitys[i].TextQuality.TextColor;
                    InterfaceQualitys[i].TextQuality.TextUiGameObject.transform.parent.gameObject.SetActive(true);
                    InterfaceQualitys[i].TextQuality.TextUiGameObject.SetActive(true);

                }else if (InterfaceQualitys[i].Pevrui == PEVRUI.Image && InterfaceQualitys[i].ImageQuality.ImageUiGameObject != null)
                {
                    
                    if (InterfaceQualitys[i].ImageQuality.IsBg == true)
                    {
                        InterfaceQualitys[i].ImageQuality.ImageUiGameObject.GetComponent<RectTransform>().sizeDelta=new Vector2(800f,450f);
                        InterfaceQualitys[i].ImageQuality.ImageUiGameObject.transform.position = Vector3.zero;
                    }
                    InterfaceQualitys[i].ImageQuality.ImageUiGameObject.transform.parent.gameObject.SetActive(true);
                    InterfaceQualitys[i].ImageQuality.ImageUiGameObject.SetActive(true);
                }else if (InterfaceQualitys[i].Pevrui == PEVRUI.Button && InterfaceQualitys[i].ButtonQuality.ButtonUiGameObject!= null)
                {
                    InterfaceQualitys[i].ButtonQuality.ButtonUiGameObject.GetComponentInChildren<Text>().color =
                        InterfaceQualitys[i].ButtonQuality.TextColor;
                    InterfaceQualitys[i].ButtonQuality.ButtonUiGameObject.transform.parent.gameObject.SetActive(true);
                    InterfaceQualitys[i].ButtonQuality.ButtonUiGameObject.SetActive(true);
                    if (InterfaceQualitys[i].ButtonQuality.EventName!="")
                    {
                        
                        Events events = InterfaceQualitys[i].ButtonQuality.Events;
                        InterfaceQualitys[i].ButtonQuality.ButtonUiGameObject.GetComponent<Button>().onClick.AddListener(
                            delegate
                            {
                                events.DoRelateToEvents();
                            });
                    }
                }
            }
        }

        if (CloseIndexList != null && CloseIndexList.Count>0)
        {
            Transform tra = Manager.Instace.transform.FindChild("PEVRUIParent");
            for (int i = 0; i < CloseIndexList.Count; i++)
            {
                tra.GetChild(CloseIndexList[i]).gameObject.SetActive(false);
            }
        }
        base.DoAction(m);
    }

    public void SendEvent(Events e)
    {
        e.DoRelateToEvents();
    }
}
