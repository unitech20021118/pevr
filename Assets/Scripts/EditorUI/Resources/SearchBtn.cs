using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public class SearchBtn : MonoBehaviour
{
    public GameObject Panel;
    public static SearchBtn Instance;
    public List<string> names = new List<string>();
    /// <summary>
    /// 所有的资源列表
    /// </summary>
    public List<JsonData> allJsonData = new List<JsonData>();
    public Dictionary<JsonData, string> allData = new Dictionary<JsonData, string>();
    //edit by 王梓亦
    private string path;
    void Awake()
    {
        path = Application.streamingAssetsPath;
        Instance = this;
    }
    // Use this for initialization
    void Start()
    {

        names.Add("animal");
        //names.Add("animation");
        names.Add("character");
        names.Add("equip");
        names.Add("food");
        names.Add("estate");
        names.Add("plant");
        names.Add("scene");
        //names.Add("signal");
        //names.Add("sky");
        names.Add("terrain");
        names.Add("geometry");
        //names.Add("test2");
        names.Add("texiao");
        names.Add("vehicle");
        names.Add("weapon");
        //names.Add("zhangpeng");

        StartCoroutine(DoLoadAllAsset());
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnClick()
    {
        //if (ResourcesBtn.lastBtn != null)
        //{
        //    ResourcesBtn.lastBtn.panel.SetActive(false);
        //}
        if (ResourcesBtn.lastBtn!=null)
        {
            ResourcesBtn.lastBtn.panel.SetActive(false);
        }
        Panel.SetActive(true);
    }
    /// <summary>
    /// 读取所有资源列表
    /// </summary>
    IEnumerator DoLoadAllAsset()
    {
        for (int i = 0; i < names.Count; i++)
        {
            //edit by 王梓亦
            string paths = path + "/" + names[i] + ".txt";
            string data = File.ReadAllText(paths);
            //TextAsset ta = Resources.Load<TextAsset>(names[i]);
            //JsonData charactersJd = JsonMapper.ToObject(ta.text);
            JsonData charactersJd = JsonMapper.ToObject(data);
            JsonData character = charactersJd[names[i]];
            foreach (JsonData CJD in character)
            {
                allJsonData.Add(CJD);
                allData.Add(CJD, names[i]);
                //Debug.Log(CJD["name2"].ToString()+"    "+CJD["name2"].ToString().Length);
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
}
