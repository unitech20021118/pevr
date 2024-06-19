using System.Collections;
using System.Collections.Generic;
using System.Xml.Xsl;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// edit by kuai
/// </summary>
public class WorldText : Action<Main>
{

    public bool close;
    /// <summary>
    /// 世界画布
    /// </summary>
    public GameObject worldCanvas;
    /// <summary>
    /// 需要显示的文本
    /// </summary>
    public string message;
    /// <summary>
    /// 大小
    /// </summary>
    public float size = 1f;

    public override void DoAction(Main m)
    {
        //先检测目标物体下是否已经有世界画布
        if (m.transform.FindChild("WorldCanvas") != null && m.transform.FindChild("WorldCanvas").tag=="WorldCanvas")
	    {
            worldCanvas = m.transform.FindChild("WorldCanvas").gameObject;
	        if (close)
	        {
	            worldCanvas.SetActive(false);
	        }
	        else
	        {
	            worldCanvas.SetActive(true);
	        }
        }//如果没有就创建
        else
        {
            if (close)
            {
                return;
            }
            worldCanvas = GameObject.Instantiate(GameObject.Find("WorldCanvas"));
            worldCanvas.transform.SetParent(m.transform);
            worldCanvas.transform.GetChild(0).gameObject.SetActive(true);
            worldCanvas.transform.localPosition=Vector3.zero;
            
            worldCanvas.name = "WorldCanvas";
        }
       
        if (worldCanvas != null)
        {
            worldCanvas.transform.GetChild(0).localScale = new Vector3(0.01f * size, 0.01f * size, 1);
            worldCanvas.GetComponentInChildren<Text>().text = message;
        }

    }
}
