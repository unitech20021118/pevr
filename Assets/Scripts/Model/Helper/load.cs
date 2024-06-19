using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using System.IO;
public class load : MonoBehaviour
{

    Create gc;
    bool canmove = true;
    public GameObject modelContent;
    public static Dictionary<string, GameObject> gameobjectList = new Dictionary<string, GameObject>();
    JsonData product = new JsonData();
    JsonData pro = new JsonData();
    string objectInfo;
    // Use this for initialization
    void Start()
    {
        pro.SetJsonType(JsonType.Array);
        gc = transform.GetComponent<Create>();
        AssetBundle assets = AssetBundle.LoadFromFile("AssetBudles/light.3dpro");
        GameObject[] objs = assets.LoadAllAssets<GameObject>();
        GameObject[] gameobj = new GameObject[objs.Length];
        for (int i = 0; i < objs.Length; i++)
        {
            gameobj[i] = Instantiate(objs[i]);
            gameobj[i].transform.position = new Vector3(1000, 1000, 1000);
        }
        GameObject obj = (GameObject)Resources.Load("Text");
        //foreach (GameObject i in objs)
        //{
        //    Debug.Log(i.name);
        //    AddItemToScroll(obj,i.name);

        //}
        for (int i = 0; i < objs.Length; i++)
        {
            AddItemToScroll(obj, objs[i].name,modelContent);
            gameobjectList[objs[i].name] = gameobj[i];
        }



    }

    // Update is called once per frame
    void Update()
    {

    }

    void AddItemToScroll(GameObject obj, string name,GameObject content)
    {
        GameObject item = Instantiate(obj);
        item.transform.parent = content.transform;
        item.GetComponent<Text>().text = name;
        item.GetComponent<Button>().onClick.AddListener(delegate() { CreateGame(name); });
        //item.GetComponent<Button>

    }

    public void CreateGame(string name)
    {
        GameObject obj = gameobjectList[name];
        //Fun2(name);
        gc.CreateGameObject(obj, canmove);
        //GameObject newobject = Instantiate(obj);
    }







    //--------------------------------------------通过点击创建按钮获得数据---------------------------------------------------
    /// <summary>
    /// 写到Json中去
    /// </summary>
    /// <param name="name"></param>
    void Fun2(string name)
    {
        JsonData cjd = new JsonData();        
        cjd["name"] = name;
        pro.Add(cjd);

        product["products"] = pro;
        Debug.Log(product.ToJson());
    }

    /// <summary>
    /// 写到本地
    /// </summary>
    /// <param name="info"></param>
    public void Save()
    {
        string a = product.ToJson();
        StreamWriter writer = new StreamWriter("text.txt");
        writer.WriteLine(a);
        writer.Close();
    }

    /// <summary>
    /// 从本地文件中读取,并加载到场景中去
    /// </summary>
    public void Load()
    {
        StreamReader reader = new StreamReader("text.txt");
        objectInfo = reader.ReadLine();
        Debug.Log(objectInfo);
        reader.Close();


        JsonData products = JsonMapper.ToObject(objectInfo);

        JsonData heros = products["products"];
        foreach (JsonData herod in heros)
        {           
            string temp = herod["name"].ToString();
            GameObject obj = gameobjectList[temp];
            
            gc.CreateGameObject(obj, canmove);

        }
        //JsonData objects = JsonMapper.ToObject(objectInfo);
        //string a = objects["name"].ToString();
        //Debug.Log(objects["name"]);
        
    }

    //-------------------------------------------------------------------------------------------------

}
