using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public class G_SaveAndLoad : MonoBehaviour
{

    GameObject parent;
    JsonData product = new JsonData();
    JsonData pro = new JsonData();
    // Use this for initialization
    void Start()
    {
        parent = GameObject.Find("Parent");
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 保存场景物体的数据信息
    /// </summary>
    public void Save()
    {
        foreach (Transform i in parent.transform)
        {
            JsonData temp = new JsonData();
            string[] temparray = i.name.Split('(');
            temp["name"] = temparray[0];
            temp["position"] = i.localPosition.ToString();
            temp["euler"] = i.eulerAngles.ToString();
            temp["scale"] = i.localScale.ToString();
            temp["layer"] = i.gameObject.layer;
            pro.Add(temp);

            product["products"] = pro;
        }

        string a = product.ToJson();
        string path = IOHelper.OpenFileDlgToSave();
        StreamWriter writer = new StreamWriter(path);
        writer.WriteLine(a);
        writer.Close();
    }


    /// <summary>
    /// 加载保存的场景信息
    /// </summary>
    public void Load()
    {
        string path = IOHelper.OpenFileDlgToLoad();
        StreamReader reader = new StreamReader(path);
        string objectInfo = reader.ReadLine();
        reader.Close();

        JsonData products = JsonMapper.ToObject(objectInfo);

        JsonData product = products["products"];
        foreach (JsonData pro in product)
        {
            string temp = pro["name"].ToString();
            string pos = pro["position"].ToString();
            string euler = pro["euler"].ToString();
            string scale = pro["scale"].ToString();
            int layer = (int)pro["layer"];
            GameObject tempobj = Instantiate(LoadAsstsShowIt.gameobjectList[temp]);
            tempobj.transform.parent = parent.transform;
            tempobj.transform.localPosition = SaveAndLoad.Parse(pos);
            tempobj.transform.eulerAngles = SaveAndLoad.Parse(euler);
            tempobj.transform.localScale = SaveAndLoad.Parse(scale);
            tempobj.layer = layer;
            //Debug.Log(temp + pos + euler + scale + "");
        }
    }

    public static Vector3 Parse(string name)
    {
        name = name.Replace("(", "").Replace(")", "");
        string[] s = name.Split(',');
        return new Vector3(float.Parse(s[0]), float.Parse(s[1]), float.Parse(s[2]));
    }
}
