using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

/// <summary>
/// 全局搜索脚本
/// </summary>
public class SearchAll : MonoBehaviour
{
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
    /// <summary>
    /// 预设物
    /// </summary>
    GameObject prefab;
    /// <summary>
    /// 上个协程
    /// </summary>
    Coroutine lastCoroutine;
    /// <summary>
    /// 所有搜索到的数据
    /// </summary>
    List<JsonData> allSearchedJSData = new List<JsonData>();
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void OnChanged()
    {
        allSearchedJSData.Clear();
        string searchName = search.text;
        if (searchName!="")
        {
            for (int i = 0; i < SearchBtn.Instance.allJsonData.Count; i++)
            {
                strFunction = SearchBtn.Instance.allJsonData[i]["name2"].ToString();
                if (strFunction==searchName)
                {
                    allSearchedJSData.Add(SearchBtn.Instance.allJsonData[i]);
                }
                else if (strFunction.Length > searchName.Length)
                {
                    for (int j = 0; j < strFunction.Length - searchName.Length+1; j++)
                    {
                        if (strFunction.Substring(j, searchName.Length) == searchName)
                        {
                            allSearchedJSData.Add(SearchBtn.Instance.allJsonData[i]);
                            break;
                        }
                    }
                }
            }
            CleanSearched();
            
            lastCoroutine = StartCoroutine(DoShowSearched());
        }
        else
        {
            CleanSearched();
        }
        
    }
    /// <summary>
    /// 清空面板上搜索出的内容并结束上一次的搜索
    /// </summary>
    public void CleanSearched()
    {
        for (int i = 0; i < content.childCount; i++)
        {
            Destroy(content.GetChild(i).gameObject);
        }
        if (lastCoroutine != null)
        {
            StopCoroutine(lastCoroutine);
        }
    }
    /// <summary>
    /// 展示搜索出的结果
    /// </summary>
    public IEnumerator DoShowSearched()
    {
        foreach (JsonData jsonData in allSearchedJSData)
	{


        Manager.itemNameList[jsonData["name"].ToString()] = jsonData["name2"].ToString();
            if (prefab == null)
            {
                prefab = (GameObject)Resources.Load("Text");
            }
            GameObject item = Instantiate(prefab);

            item.transform.SetParent(content);
            item.transform.localScale = Vector3.one;
            item.transform.localPosition = new Vector3(item.transform.position.x, item.transform.position.y, 0f);
            string imagePath = "ink2/" + jsonData["name"];
            Sprite sprite = (Sprite)Resources.Load("ink2/" + jsonData["name"], typeof(Sprite));
            if (sprite == null)
            {
                //TEST.SetActive(false);
            }
            item.transform.GetChild(1).GetComponent<Image>().sprite = sprite;
            item.transform.GetChild(2).GetComponent<Text>().text = jsonData["name2"].ToString();
            Color color = item.GetComponentInChildren<Image>().color;
            item.GetComponentInChildren<Image>().color = new Color(color.r, color.g, color.b, 255f);
            string name = "";
            foreach (var a in SearchBtn.Instance.allData)
            {
                if (a.Key == jsonData)
                {
                    name = a.Value;
                    break;
                }
            }
            Debug.Log(jsonData["modelpath"].ToString());
            item.GetComponent<Button>().onClick.AddListener(delegate() { StartCoroutine(Manager.Instace.CreateGameObj(name, jsonData["modelpath"].ToString(), jsonData["name"].ToString(), imagePath)); });
            //item.GetComponent<Button>().onClick.AddListener(delegate() { Debug.Log(jsonData["name2"]); });
            
            //Manager.Instace.isloadedPic.Add(name, true);
            yield return null;
            
        }
        
    }
}
