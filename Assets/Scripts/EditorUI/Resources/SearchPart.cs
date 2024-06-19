using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
/// <summary>
/// 区域搜索脚本
/// </summary>
public class SearchPart : MonoBehaviour {

    /// <summary>
    /// 文本输入框
    /// </summary>
    public InputField search;
    /// <summary>
    /// 展示列表父物体
    /// </summary>
    public Transform content;
    /// <summary>
    /// 对比字符串
    /// </summary>
    string strFunction;
    
    // Use this for initialization
    void Start()
    {
        content = transform.parent.GetChild(0).GetChild(0).GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 当搜索框内容改变时
    /// </summary>
    public void OnChanged()
    {
        if (search.text != "")
        {
            CleanSearched();
            //Debug.LogError(Manager.Instace.nowLoadGameObject.Count);
            for (int i = 0; i < Manager.Instace.nowLoadGameObject.Count; i++)
            {
                strFunction = Manager.Instace.nowLoadGameObject[i].transform.FindChild("Text").GetComponent<Text>().text;
                if (strFunction == search.text)
                {
                    Manager.Instace.nowLoadGameObject[i].SetActive(true);
                }
                else if (strFunction.Length > search.text.Length)
                {
                    for (int j = 0; j < strFunction.Length - search.text.Length+1; j++)
                    {
                        if (strFunction.Substring(j, search.text.Length) == search.text)
                        {
                            Manager.Instace.nowLoadGameObject[i].SetActive(true);
                            break;
                        }
                    }
                }
            }
        }
        else 
        {
            ShowAll();
        }
        
        

    }
    /// <summary>
    /// 清空面板
    /// </summary>
    public void CleanSearched()
    {
        for (int i = 0; i < content.childCount; i++)
        {
            content.GetChild(i).gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// 显示所有
    /// </summary>
    public void ShowAll()
    {
        for (int i = 0; i < content.childCount; i++)
        {
            content.GetChild(i).gameObject.SetActive(true);
        }
    }
    
   
}
