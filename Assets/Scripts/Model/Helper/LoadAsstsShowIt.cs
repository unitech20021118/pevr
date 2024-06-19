using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LoadAsstsShowIt : MonoBehaviour {

    G_CreateObject gc;
    bool canmove = true;
    GameObject[] staticobj;
    public GameObject content;
    public GameObject staticContent;
    public static Dictionary<string, GameObject> gameobjectList = new Dictionary<string, GameObject>();
    GameObject parent;
    Vector3 originPos;
    GameObject lastObj;
	// Use this for initialization
	void Start () {
        print(gameObject.name + "   000000000");
        gc=transform.GetComponent<G_CreateObject>();
        parent = GameObject.Find("Parent");
        //AssetBundle assets = AssetBundle.LoadFromFile("AssetBudles/light.3dpro");
        AssetBundle assets = AssetBundle.LoadFromFile("AssetBundles/person.3dpro");
        GameObject[] objs = assets.LoadAllAssets<GameObject>();
        GameObject[] gameobj = new GameObject[objs.Length];
        for (int i = 0; i < objs.Length; i++)
        {
            gameobj[i] = Instantiate(objs[i]);
            gameobj[i].AddComponent<MeshRenderer>();
            gameobj[i].AddComponent<CapsuleCollider>();
            gameobj[i].transform.position = new Vector3(2000, 1000, 1000);
        }
        GameObject obj = (GameObject)Resources.Load("Text");

        for (int i = 0; i < objs.Length; i++)
        {
            AddItemToScroll(obj,objs[i].name,content);
            gameobjectList[objs[i].name] = gameobj[i];
        }

        //AssetBundle staticAssets = AssetBundle.LoadFromFile("AssetBudles/house.3dpro");
        AssetBundle staticAssets = AssetBundle.LoadFromFile("AssetBundles/scene.3dpro");
        GameObject[] staticObj = staticAssets.LoadAllAssets<GameObject>();
        staticobj = new GameObject[staticObj.Length];
        for (int i = 0; i < staticObj.Length; i++)
        {
            staticobj[i] = Instantiate(staticObj[i]);
            foreach (Transform l in staticobj[i].transform)
            {
                l.gameObject.layer = G_PubDef.quiescentObject;
                foreach (Transform j in l)
                {
                    j.gameObject.layer = G_PubDef.quiescentObject;
                }
            }
            originPos = staticobj[i].transform.position;
            staticobj[i].transform.position = new Vector3(2000, 1000, 1000);
            //staticobj[i].SetActive(false);
        }
        //加载item
        GameObject sobj = (GameObject)Resources.Load("Text");
        for (int j = 0; j < staticObj.Length; j++)
        {
            AddItemToScrollTwo(sobj, staticObj[j].name,staticContent);
            gameobjectList[staticObj[j].name] = staticobj[j];
        }
        
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void AddItemToScroll(GameObject obj,string name,GameObject content)
    {
        GameObject item = Instantiate(obj);
        item.transform.parent = content.transform;
        item.transform.localScale = Vector3.one;
        item.GetComponent<Text>().text = name;
        Sprite sprite = (Sprite)Resources.Load("ink2/" + name,typeof(Sprite));
        item.GetComponentInChildren<Image>().sprite = sprite;
        Color color = item.GetComponentInChildren<Image>().color;
        item.GetComponentInChildren<Image>().color = new Color(color.r, color.g, color.b, 255f);
        item.GetComponent<Button>().onClick.AddListener(delegate() { CreateGame(name); });
        //item.GetComponent<Button>
        
    }

    public void CreateGame(string name)
    {
        //if (can)
        //{
            GameObject obj = gameobjectList[name];
            
            //gc.CreateGameObject(obj, canmove,name);
            
        //}
        //else
        //{
        //    staticobj[0].SetActive(true);
        //}
        //GameObject newobject = Instantiate(obj);
    }


    void AddItemToScrollTwo(GameObject obj, string name, GameObject content)
    {
        GameObject item = Instantiate(obj);
        item.transform.parent = content.transform;
        item.transform.localScale = Vector3.one;
        item.GetComponent<Text>().text = name;
        Sprite sprite = (Sprite)Resources.Load("ink2/" + name, typeof(Sprite));
        item.GetComponentInChildren<Image>().sprite = sprite;
        Color color = item.GetComponentInChildren<Image>().color;
        item.GetComponentInChildren<Image>().color = new Color(color.r, color.g, color.b, 255f);
        item.GetComponent<Button>().onClick.AddListener(delegate() { CreateGame2(name); });
        //item.GetComponent<Button>

    }

    public void CreateGame2(string name)
    {
       //staticobj[0].transform.position = originPos;
       //staticobj[0].transform.parent = parent.transform;]
        if (lastObj != null)
        {
            lastObj.transform.parent = null;
            lastObj.transform.position = new Vector3(1000, 1000, 1000);
        }
       GameObject obj = GameObject.Find(name + "(Clone)");
       //obj.name = name;
       obj.transform.position = originPos;
       obj.transform.parent = parent.transform;
       lastObj = obj;
       
    }
}
