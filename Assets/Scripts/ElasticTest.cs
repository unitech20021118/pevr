using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ElasticTest : MonoBehaviour
{
    [System.Serializable]
    public class ChildList
    {
        public GameObject parent;//父物体
        public bool autoAddChild;//是否自动填加子物体
        public List<GameObject> child;//子物体列表
    }
    List<GameObject> allGroup = new List<GameObject>();//所有的父菜单
    Vector2 contentSizedata;//每次更改都需要计算content所容纳的数量
    RectTransform contentRt;
    GameObject lastObj;//最后一次点击的父菜单
    Vector2 oriPosition;

    GameObject _Content;
    int lastcount, thiscount;
    bool _IsChange;

    public List<ChildList> itemList = new List<ChildList>();
    void Awake()
    {
        _Content = GameObject.Find("EditorCanvas/EditorPanel/StatePanel/Scroll View/Viewport/Content");
        itemList.Clear();
    }
    
    /// <summary>
    /// 检测是否有变化
    /// </summary>
    void Update()
    {
        thiscount = _Content.transform.childCount;
        if (thiscount != lastcount)
        {
            _IsChange = true;
        }
        if (thiscount == lastcount)
        {
            _IsChange = false;
        }
        if (_IsChange)
        {
            StGo();
            lastcount = itemList.Count;
            _IsChange = false;
        }
    }

    void StGo()
    {
        itemList.Clear();//去掉误差
        AddItemList();
        //Material mat = GetComponent<Renderer>().material;
        //float f = mat.GetFloat("_SplitY");
        //StartCoroutine(WaitHide(f, mat));
        //return;
        List<GameObject> child = new List<GameObject>();//

        for (int i = 0; i < itemList.Count; i++)
        {
            AddItem(itemList[i]);
            if (i == 0)
                oriPosition = itemList[i].parent.GetComponent<RectTransform>().localPosition;
        }
        if (thiscount != 0)
        {
            Init();
        }   
    }

    IEnumerator WaitHide(float f, Material mat)
    {
        while (f > -10)
        {
            f -= Time.deltaTime * 3;
            Debug.Log(f);
            mat.SetFloat("_SplitY", f);
            yield return new WaitForFixedUpdate();
        }
    }
    //  AutoAddItemlist     *****************
    void AddItemList()
    {

        for (int i = 0; i < _Content.transform.childCount; i++)
        {
            ChildList _thisList=new ChildList();
            _thisList.parent =_Content.transform.GetChild(i).gameObject;
            _thisList.autoAddChild = true;
            itemList.Add(_thisList);
        }

    }


    /// <summary>
    /// 初始化
    /// </summary>
    void Init()
    {
        lastObj = null;
        contentRt = GameObject.Find("EditorCanvas/EditorPanel/StatePanel/Scroll View/Viewport/Content").GetComponent<RectTransform>();
        for (int i = 0; i < allGroup.Count; i++)
        {
            ChildListState(allGroup[i], 0);
        }
        CostContentSize();//确定尺寸
    }
    /// <summary>
    /// 根据父物体加入子物体信息
    /// </summary>
    /// <param name="item">一组父子物体的信息</param>
    private void AddItem(ChildList item)
    {
        if (item.parent.GetComponent<ItemGroup>() == null)
        {
            item.parent.AddComponent<ItemGroup>();
        }
        ItemGroup group = item.parent.GetComponent<ItemGroup>();
        item.parent.GetComponent<Button>().onClick.AddListener(delegate { OnClickParent(item.parent); });
        allGroup.Add(item.parent);
        if (!item.autoAddChild)
        {
            foreach (GameObject _item in item.child)
                group.childList.Add(_item);
        }
        else
        {
            Transform t = item.parent.transform.GetChild(0);
            for (int i = 0; i < t.childCount; i++)
            {
                group.childList.Add(t.GetChild(i).gameObject);
            }
            item.child = group.childList;
        }
    }
    /// <summary>
    /// 控制子菜单的显示和隐藏
    /// </summary>
    /// <param name="item">父菜单物体</param>
    /// <param name="isShow">是否显示，0隐藏1显示2列表的第一个物体的相反状态</param>
    void ChildListState(GameObject item, int isShow)
    {
        List<GameObject> childlist = item.GetComponent<ItemGroup>().childList;
        if (isShow == 2 && childlist.Count > 0)
        {
            isShow = childlist[0].activeSelf == false ? 1 : 0;
        }
        for (int i = 0; i < childlist.Count; i++)
        {
            childlist[i].SetActive(isShow == 1 ? true : false);
        }
    }
    List<GameObject> allObj = new List<GameObject>();
    List<float> allPos = new List<float>();
    float allChildHeight;
    bool isCanClick = true;
    /// <summary>
    /// 点击父菜单的时候弹出子菜单
    /// </summary>
    /// <param name="item">父菜单物体</param>
    void OnClickParent(GameObject item)
    {
        //Debug.Log(isCanClick);
        if (!isCanClick)
        {
           //  Debug.Log("点击过快");
            return;
        }
        else
        {
            isCanClick = false;
            //Debug.Log("开始");
        }
        if (lastObj != null)
        {
            if (lastObj != item)
            {
                //Debug.Log("点击的不是同一个，lastObj", lastObj);
               // Debug.Log("点击的不是同一个，item", item);
                MoveToTarget(lastObj, false, true);
                MoveToTarget(item, true);
            }
            else
            {
                if (item.transform.Find("ChildContent").GetChild(0).gameObject.activeSelf)
                {
                   // Debug.Log("有一级展开，是自己，收回");
                    MoveToTarget(item, false);
                }
                else
                {
                  //  Debug.Log("有一级展开，是自己，展开");
                    MoveToTarget(item, true);
                }
            }
        }
        else
        {
          //  Debug.Log("第一次点击");
            MoveToTarget(item, true);
        }
        lastObj = item;
    }
    /// <summary>
    /// 移动一个列表的物体
    /// </summary>
    /// <param name="item">父菜单对象</param>
    /// <param name="isDown">是否收回</param>
    /// <param name="isLast">是否收回子物体</param>
    private void MoveToTarget(GameObject item, bool isDown, bool isLast = false)
    {
        allObj.Clear();//初始化
        allPos.Clear();
        allChildHeight = 0;//展开的所有子物体的高度
        for (int i = 0; i < allGroup.Count; i++)
        {
            if (item == allGroup[i])//当前点击的对象
            {
                //新的子物体的Y=初始Y+子物体的索引*子物体的高度+前边所有的子物体的高度（由于是负数所以表达式里省略了+号）
                allPos.Add(oriPosition.y - i * allGroup[i].GetComponent<RectTransform>().sizeDelta.y - allChildHeight);
                allObj.Add(allGroup[i]);
                RectTransform rt = allGroup[i].GetComponent<RectTransform>();
                List<GameObject> list = allGroup[i].GetComponent<ItemGroup>().childList;
                if (list.Count > 0)//有些菜单可能没有子菜单
                {
                    if (isDown)
                    {
                        for (int j = 0; j < list.Count; j++)
                        {
                            ReversalGameObject(list[j]);
                            if (list[j].activeSelf)
                            {
                                allChildHeight += list[j].GetComponent<RectTransform>().sizeDelta.y;
                                allObj.Add(list[j]);
                                allPos.Add(-list[j].GetComponent<RectTransform>().sizeDelta.y - list[j].GetComponent<RectTransform>().sizeDelta.y * j);
                            }
                        }
                    }
                    else
                    {
                        for (int j = 0; j < list.Count; j++)
                        {
                            allChildHeight += 0;
                            allObj.Add(list[j]);
                            allPos.Add(-100);//子菜单的第一个Y值
                        }
                    }
                }
            }
            else
            {
                if (isLast)
                    continue;
                RectTransform rt = allGroup[i].GetComponent<RectTransform>();
                List<GameObject> list = allGroup[i].GetComponent<ItemGroup>().childList;
                if (lastObj == allGroup[i] && list.Count > 0)
                {
                    if (lastObj.GetComponent<ItemGroup>().childList[0].activeSelf)
                    {
                        Debug.Log("dd");
                        for (int j = 0; j < list.Count; j++)
                        {
                            ReversalGameObject(list[j], 2);//确保上一个菜单已经关闭
                        }
                    }
                }
                //新的父物体的Y=初始Y+父物体的索引*父物体的高度+前边所有的子物体的高度（由于是负数所以表达式里省略了+号）
                allObj.Add(allGroup[i]);
                allPos.Add(oriPosition.y - i * allGroup[i].GetComponent<RectTransform>().sizeDelta.y - allChildHeight);
            }
        }
        StartCoroutine(SetObjectsPosition(isDown));//多个协程嵌套，保证以最少的时间完成任务并加强体验
    }

    void ReversalGameObject(GameObject obj, int state = 0)//0反转1显示 2隐藏
    {
        if (state == 0)
            obj.SetActive(!obj.activeSelf);
        else
            obj.SetActive(state == 1 ? true : false);
    }
    private IEnumerator SetObjectsPosition(bool isDown)
    {
        GameObject[] game = allObj.ToArray();//由于是引用对象，加上协程，有时候协程没执行完却又执行到这里就会出现差错，所以在这里重新创建并赋值
        float[] ff = allPos.ToArray();
        yield return StartCoroutine(Wait(isDown));
        yield return null;
        for (int i = 0; i < game.Length; i++)
        {
            if (!isDown && game[i].transform.parent.name == "ChildContent")
            {
                ReversalGameObject(game[i], 2);
            }
        }
        CostContentSize();
    }
    private IEnumerator Wait(bool isDown)
    {
        GameObject[] game = allObj.ToArray();//由于是引用对象，加上协程，有时候协程没执行完却又执行到这里就会出现差错，所以在这里重新创建并赋值
        float[] ff = allPos.ToArray();
        for (int i = 0; i < game.Length; i++)
        {
            if (isDown)
                StartCoroutine(GoToPointDownItem(game[i], ff[i], isDown, game.Length - 1 == i ? true : false));
            else
                StartCoroutine(GoToPointUpItem(game[i], ff[i], isDown, game.Length - 1 == i ? true : false));
        }
        yield return null;
    }

    float shiftY = 20;//渐变的速度
    IEnumerator GoToPointDownItem(GameObject currentObj, float target, bool isDown, bool isFinish)//弹出
    {
        RectTransform rt = currentObj.GetComponent<RectTransform>();
        Vector2 pos = rt.localPosition;
        while (pos.y > target)
        {
            pos.y -= shiftY;
            rt.localPosition = pos;
            yield return null;
        }
        currentObj.GetComponent<RectTransform>().localPosition = new Vector2(currentObj.GetComponent<RectTransform>().localPosition.x, target);
        yield return null;
        if (isFinish)//子物体都收回完毕，可以进行下一次操作
        {
            isCanClick = true;
            //   Debug.Log("finish");
        }
    }
    IEnumerator GoToPointUpItem(GameObject currentObj, float target, bool isDown, bool isFinish)//收回
    {
        RectTransform rt = currentObj.GetComponent<RectTransform>();
        Vector2 pos = rt.localPosition;
        while (pos.y < target)
        {
            pos.y += shiftY;
            rt.localPosition = pos;
            yield return null;
        }
        currentObj.GetComponent<RectTransform>().localPosition = new Vector2(currentObj.GetComponent<RectTransform>().localPosition.x, target);
        yield return null;
        if (isFinish)//子物体都收回完毕，可以进行下一次操作
        {
            isCanClick = true;
            //Debug.Log("finish");
        }
    }
    void CostContentSize()//计算content的可视范围
    {
        float allItemHeight = 0;
        for (int i = 0; i < allGroup.Count; i++)
        {
            allItemHeight += allGroup[i].GetComponent<RectTransform>().sizeDelta.y;
            List<GameObject> list = allGroup[i].GetComponent<ItemGroup>().childList;
            if (list.Count > 0)
            {
                for (int j = 0; j < list.Count; j++)
                {
                    if (!list[j].activeSelf)
                        continue;
                    allItemHeight += list[j].GetComponent<RectTransform>().sizeDelta.y;
                }
            }
        }
        float paddingTop = 20;
        contentSizedata.y = allItemHeight;
        contentSizedata.y += paddingTop;
        contentRt.sizeDelta = contentSizedata;
    }
}