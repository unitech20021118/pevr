using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Undo;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class G_CreateObject : MonoBehaviour {

    GameObject mainCamera;
    public int DefaultZ = 10;
    public Text text;
    private GameObject createObject;
    GameObject parent;
    //List<string> scenes=new List<string>();//加载过的场景名称
    Dictionary<GameObject, GameObject> scenes = new Dictionary<GameObject, GameObject>();
    private GameObject lastScene;
    public  bool IsUndo = false;
    /// <summary>
    /// 物体嵌入地面的修正值
    /// </summary>
    private Vector3 upGroundVector3;
    void Awake()
    {
        G_MouseListener.GetInstance().AddLMouseUp(DropObject);
        //G_MouseListener.Instance.AddLMouseDown(DropObject);
        mainCamera = GameObject.Find("Main Camera");
        parent = GameObject.Find("Parent");
    }

    public void DropObject(){

        createObject = null;
    }

    public IEnumerator SetCreateObjectPosition()
    {
        while (IsUndo == false)
        {
            if (createObject == null)
            {
                yield break;
            }
            if (createObject.layer != G_PubDef.quiescentObject)
            {
                createObject.transform.position = GetWorldPos();
            }
            yield return 0;
        }
    }

    /// <summary>
    /// 点击创建物体
    /// </summary>
    /// <param name="creatObject"></param>
    /// <param name="canMove"></param>
    public GameObject CreateGameObject(GameObject creatObject, bool canMove, string name, string imgpath, bool isUndo = false)
    {

        if (canMove)//加载物品
        {
            createObject = Instantiate<GameObject>(creatObject);
            upGroundVector3 = UpGround(createObject);
            createObject.GetComponentInChildren<Renderer>().sortingLayerName = "bana";
            //物体被创建，则保存该物品的信息

            //Product temp = new Product(createObject.name, createObject);
            //Product.productList.Add(temp);
            Product.ManagerGameobject(name, createObject);
            //添加到对象列表中去
            //StartCoroutine()
            createObject.transform.parent = parent.transform;

            //Undo创建物体时不需要修改物体位置随鼠标移动
            if (isUndo == false)
            {
                StartCoroutine(SetCreateObjectPosition());
                createObject.transform.position = GetWorldPos();
            }
            else
            {
                IsUndo = true;
                StopCoroutine("SetCreateObjectPosition");
            }

            createObject.tag = "Editor";
            //Debug.LogError(createObject + "     " + name);
            AddInObjectList(createObject, name, imgpath);
            //text.text = createObject.name;
            text.text = name + Product.dict[name].Count.ToString();
            createObject.layer = G_PubDef.dynamicObject;
            foreach (Transform i in createObject.transform)
            {
                if (i.gameObject.layer.Equals(0))
                {
                    i.gameObject.layer = G_PubDef.dynamicObject;
                }
            }
        }
        else//加载场景
        {
            //if (lastScene == creatObject)
            //{

            //}
            //else
            //{
            //if (!scenes.ContainsKey(creatObject))
            //{

            //    createObject = Instantiate<GameObject>(creatObject);
            //    createObject.transform.parent = parent.transform;
            //    //createObject.transform.position = GetWorldPos();
            //    createObject.tag = "Editor";
            //    scenes.Add(creatObject, createObject);
            //}
            //else
            //{
            //    createObject = scenes[creatObject];
            //    createObject.SetActive(true);
            //}
            createObject = Instantiate<GameObject>(creatObject);
            createObject.transform.parent = parent.transform;
            createObject.tag = "Editor";
            //Product.ManagerGameobject(name, createObject);

            AddInObjectList(createObject, name, imgpath);
            createObject.layer = G_PubDef.quiescentObject;
            createObject.GetComponentInChildren<Renderer>().sortingLayerName = "app";
            Transform[] all = createObject.transform.GetComponentsInChildren<Transform>();
            foreach (Transform i in all)
            {
                i.gameObject.layer = G_PubDef.quiescentObject;
                //i.gameObject.tag = "Editor";
            }
            //if (lastScene != null)
            //{
            //    scenes[lastScene].SetActive(false);
            //}

            //    }
            mainCamera.transform.position = createObject.transform.position;
            //    lastScene = creatObject;

        }
        return createObject;
    }

    //edit by 王梓亦
    public Texture2D ReadByWWW(string path)
    {
        FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
        int byteLength = (int)fs.Length;
        byte[] imgBytes = new byte[byteLength];
        fs.Read(imgBytes, 0, byteLength);
        fs.Close();
        fs.Dispose();
        System.Drawing.Image img = System.Drawing.Image.FromStream(new MemoryStream(imgBytes));
        Texture2D t2 = new Texture2D(img.Width, img.Height);
        img.Dispose();
        t2.LoadImage(imgBytes);
        t2.Apply();
        return t2;

    }


    public Vector3 GetWorldPos()
    {
        Vector3 groundPos;
        if (G_RayCastCheck.QuiescentObjectCheckGround(out groundPos, G_PubDef.QuiescentObject))
        {
            if (createObject!=null)
            {
                return groundPos + upGroundVector3;
            }
            else
            {
                return groundPos;
            }
            
        }

        Vector3 tempPos = Input.mousePosition;
        tempPos.z = mainCamera.transform.position.z + DefaultZ;
        tempPos = G_RayCastCheck.ScreenPosToWorldPos(tempPos);
        return tempPos;
    }

    /// <summary>
    /// 在对象列表中添加属性图标
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="name"></param>
    public void AddInObjectList(GameObject obj, string name, string imgpath)
    {
        if (Manager.Instace.item_GameObject == null)
        {
            Manager.Instace.item_GameObject = (GameObject)Resources.Load("Prefabs/item");
        }

        GameObject items = Instantiate(Manager.Instace.item_GameObject);
        items.transform.SetParent(Manager.Instace.objlist.transform);
        items.transform.localPosition = new Vector3(items.transform.position.x, items.transform.position.y, 0f);
        items.transform.localScale = Vector3.one;
        items.GetComponent<item>().text.text = obj.name;
        //items.GetComponent<RectTransform>().sizeDelta = new Vector2(120, 100);
        //items.GetComponent<item>().parentName = obj.transform.parent.name;
        items.name = obj.name;

        //Texture2D t2d = ReadByWWW(Application.streamingAssetsPath + "/ink2/" + name + ".png");
        print(imgpath);

        #region 如果要读取的三维模型的预览图不存在，读取一个问号的模型预览图到场景中示意

        string wwwPath;
        if (!File.Exists(Application.streamingAssetsPath + imgpath))
        {
            wwwPath = Application.streamingAssetsPath + "/ink2/error_missing.png";
        }
        else
        {
            wwwPath = Application.streamingAssetsPath + imgpath;
        }
        #endregion
        Texture2D t2d = ReadByWWW(wwwPath);

        //Texture2D t2d = (Texture2D)Resources.Load("ink2/" + name, typeof(Texture2D));
        Sprite s = Sprite.Create(t2d, new Rect(0, 0, t2d.width, t2d.height), new Vector2(0.5f, 0.5f));
        items.GetComponent<item>().pic.sprite = s;

        //Sprite sprite = (Sprite)Resources.Load("ink2/" + name, typeof(Sprite));
        //items.GetComponent<item>().pic.sprite = sprite;
        items.GetComponent<item>().SetTarget(obj);//设置item中对对象物体的索引
        //Manager.Instace.childrenNnameList.Add(items,)

        //edit by kuai
        //Manager.Instace.objectTopic.Add(obj, items);
        if (!Manager.Instace.objectTopic.ContainsKey(obj))
        {
            Manager.Instace.objectTopic.Add(obj, items);
        }
        else
        {
            Debug.LogError("字典中已有名为:"+ obj.name +"的键值，无法添加！！！");
        }
    }
    /// <summary>
    /// 计算物体会嵌入地面中的距离并回复这个距离
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public Vector3 UpGround(GameObject obj)
    {
        if (obj.GetComponent<Collider>() != null)
        {
            return new Vector3(0, Vector3.Distance(obj.GetComponent<Collider>().ClosestPointOnBounds(obj.GetComponent<Collider>().transform.position + new Vector3(0, -1000f, 0)),
                obj.transform.position), 0);
        }
        else
        {
            return Vector3.zero;
        }
    }
}
