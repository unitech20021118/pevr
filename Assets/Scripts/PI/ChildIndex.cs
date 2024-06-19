using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChildIndex : MonoBehaviour
{
    /// <summary>
    /// 选中的
    /// </summary>
    public static ChildIndex dragedChildIndex;
    /// <summary>
    /// 目标物体
    /// </summary>
    public GameObject Target;
    /// <summary>
    /// 名字文本
    /// </summary>
    public Text textName;
    /// <summary>
    /// 是否展示子物体的文本
    /// </summary>
    public Text IsShowChildText;
    /// <summary>
    /// 父物体索引
    /// </summary>
    public Transform parentIndex;
    /// <summary>
    /// 子物体列表
    /// </summary>
    public List<GameObject> childrenItemList = new List<GameObject>();

    public List<Transform> ChildrenTransformList = new List<Transform>();
    /// <summary>
    /// 子物体信息列表
    /// </summary>
    public List<ChildrenAttribute> ChildrenAttributes = new List<ChildrenAttribute>();
    /// <summary>
    /// 子物体是否显示
    /// </summary>
    private bool IsShowChild = true;
    /// <summary>
    /// 自身是否显示
    /// </summary>
    public bool IsShow = true;
    /// <summary>
    /// 是否获取过子物体
    /// </summary>
    bool IsGetChildList = false;
    /// <summary>
    /// 是否是编辑模式
    /// </summary>
    public bool IsEdit = true;
    /// <summary>
    /// 查找根节点
    /// </summary>
    public Transform FindTransform;
    /// <summary>
    /// 是否是点击过展开折叠按钮
    /// </summary>
    bool IsFirstClick = false;
    /// <summary>
    /// 显示自己是否被选中的image
    /// </summary>
    public Image imageColor;

    public bool openChildrenInScene;
    private bool Opened;
    private PIHighlightController PHC;
    Material material;
    // Use this for initialization
    void Start()
    {
        if (parentIndex != null)
        {
            textName.rectTransform.anchoredPosition = parentIndex.GetComponent<ChildIndex>().textName.rectTransform.anchoredPosition + new Vector2(20, 0);
            IsShowChildText.rectTransform.anchoredPosition = parentIndex.GetComponent<ChildIndex>().IsShowChildText.rectTransform.anchoredPosition + new Vector2(20, 0);

        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetInformation(GameObject go, Transform par)
    {
        Target = go;
        parentIndex = par;
        textName.text = go.name;
    }

    /// <summary>
    /// 点击展开与折叠
    /// </summary>
    public void ButtonClick()
    {
        if (IsShowChild)
        {
            IsShowChild = false;
            IsShowChildText.text = "+";
        }
        else
        {
            IsShowChild = true;
            IsShowChildText.text = "-";
        }
        SetChildActive();
        if (IsFirstClick == false && parentIndex == null)
        {
            IsFirstClick = true;
            ButtonClick();
        }
        //Debug.LogError("11111");
    }


    /// <summary>
    /// 控制子物体的显示与隐藏
    /// </summary>
    public void SetChildActive()
    {
        GetChildList();
        //Debug.LogError(childrenList.Count);
        if (childrenItemList != null && childrenItemList.Count > 0)
        {
            for (int i = 0; i < childrenItemList.Count; i++)
            {
                childrenItemList[i].GetComponent<ChildIndex>().IsShow = IsShowChild;
                if (!IsShowChild)
                {
                    childrenItemList[i].GetComponent<ChildIndex>().IsShowChild = IsShowChild;
                    childrenItemList[i].GetComponent<ChildIndex>().SetChildActive();
                }

                childrenItemList[i].SetActive(IsShowChild);
            }
        }
    }
    /// <summary>
    /// 获取子物体列表
    /// </summary>
    public void GetChildList()
    {
        if (IsGetChildList == false)
        {
            IsGetChildList = true;
            //Debug.LogError("555555555000");
            //GameObject[] children = GameObject.FindGameObjectsWithTag("SCI");

            //for (int i = 0; i < children.Length; i++)
            //{
            //    if (children[i].GetComponent<ChildIndex>().parentIndex.name == transform.name)
            //    {
            //        childrenList.Add(children[i]);
            //    }
            //}
            for (int i = 1; i < FindTransform.childCount; i++)
            {
                if (FindTransform.GetChild(i).gameObject.GetComponent<ChildIndex>().parentIndex.name == transform.name)
                {
                    childrenItemList.Add(FindTransform.GetChild(i).gameObject);
                }
            }
            if (childrenItemList == null || childrenItemList.Count == 0)
            {
                IsShowChildText.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 设置查找根节点
    /// </summary>
    public void SetFindTransform(Transform tra)
    {
        FindTransform = tra;
    }
    /// <summary>
    /// 点击显示目标
    /// </summary>
    public void ShowTarget()
    {
        if (dragedChildIndex == null)
        {

            dragedChildIndex = this;
        }
        else if (dragedChildIndex != null)
        {
            //切换选中时将上一个选中的还原
            if (dragedChildIndex==this)
            {
                return;
            }
            PHC = dragedChildIndex.Target.GetComponent<PIHighlightController>();
            if (PHC == null)
            {
                PHC = dragedChildIndex.Target.AddComponent<PIHighlightController>();
            }
            PHC.StopHighLight(true);
        }
        dragedChildIndex.imageColor.color = new Color32(255, 255, 255, 0);
        dragedChildIndex = this;
        this.imageColor.color = new Color32(45, 168, 255, 100);
        int a = transform.GetSiblingIndex();
        if (IsEdit)
        {
            if (a == 0)
            {   //如果是列表下第一个物体点击时等同于item的点击
                Manager.Instace.gameObject.GetComponent<G_EditorTarget>().SetMoveTarget(Target);
                if (Manager.Instace.gameObject.GetComponent<G_MoveObject>() != null)
                {
                    Manager.Instace.gameObject.GetComponent<G_MoveObject>().SetTarget(Target);
                }
            }//如果不是第一个物体
            else
            {
                Manager.Instace.gameObject.GetComponent<G_EditorTarget>().SetMoveTargetPI(Target);

                if (Manager.Instace.gameObject.GetComponent<G_MoveObject>() != null)
                {
                    Manager.Instace.gameObject.GetComponent<G_MoveObject>().SetPITarget(Target);
                }
            }
            PICamera.Instance.ShowOriginalIntroduce();
        }
        else
        {
            PHC = Target.GetComponent<PIHighlightController>();
            if (PHC == null)
            {
                PHC = Target.AddComponent<PIHighlightController>();
            }
            PHC.StartHighLight(true);
        }
    }
    /// <summary>
    /// 取消选中
    /// </summary>
    public void Reduce()
    {
        PHC = dragedChildIndex.Target.GetComponent<PIHighlightController>();
        if (PHC == null)
        {
            PHC = dragedChildIndex.Target.AddComponent<PIHighlightController>();
        }
        PHC.StopHighLight(true);
        dragedChildIndex.imageColor.color = new Color32(255, 255, 255, 0);
        dragedChildIndex = null;
    }

    public void ShowSuspensionTarget()
    {
        PHC = Target.GetComponent<PIHighlightController>();
        if (PHC == null)
        {
            PHC = Target.AddComponent<PIHighlightController>();
        }
        PHC.StartHighLight(false);
    }

    public void StopShowSuspensionTarget()
    {
        PHC = Target.GetComponent<PIHighlightController>();
        if (PHC != null)
        {
            PHC.StopHighLight(false);
        }
        else
        {
            Debug.LogError("PHC");
        }
    }
    public void PointerDown()
    {
        ShowTarget();
    }

    public void PointerUp()
    {
        if (Input.GetMouseButtonUp(1))
        {
            if (childrenItemList != null && childrenItemList.Count > 0)
            {
                PICamera.Instance.ShowOption(true);
            }
            else
            {
                PICamera.Instance.ShowOption(false);
            }
        }
    }
    /// <summary>
    /// 在场景中展开子物体
    /// </summary>
    public void OpenChildInScene()
    {
        ChildrenAttribute childrenAttribute;
        Transform c;
        if (!Opened)
        {
            for (int i = 0; i < Target.transform.childCount; i++)
            {
                c = Target.transform.GetChild(i);
                ChildrenTransformList.Add(c);
                //记录子物体信息
                childrenAttribute = new ChildrenAttribute(c.localPosition, c.localEulerAngles);
                ChildrenAttributes.Add(childrenAttribute);
            }
        }
        if (!openChildrenInScene)
        {
            StartCoroutine(DoOpenChildInScene());
            openChildrenInScene = true;
            Opened = true;
        }
    }

    /// <summary>
    /// 将被展开的子物体还原
    /// </summary>
    public void Reduction()
    {
        if (openChildrenInScene)
        {
            StartCoroutine(DoReduction());
            openChildrenInScene = false;
        }
        //将自己子物体中被展开的一同还原
        for (int i = 0; i < childrenItemList.Count; i++)
        {
            if (childrenItemList[i].GetComponent<ChildIndex>().openChildrenInScene)
            {
                childrenItemList[i].GetComponent<ChildIndex>().Reduction();
            }
        }
    }
    IEnumerator DoOpenChildInScene()
    {
        for (int i = 0; i < ChildrenTransformList.Count; i++)
        {
            //展开子物体 （将pos放大一定倍数）
            ChildrenTransformList[i].localPosition = ChildrenTransformList[i].localPosition * 5f;
            yield return null;
        }
    }
    /// <summary>
    /// 将展开的子物体还原
    /// </summary>
    IEnumerator DoReduction()
    {
        for (int i = 0; i < ChildrenTransformList.Count; i++)
        {
            //还原子物体相对位置
            ChildrenTransformList[i].localPosition = ChildrenAttributes[i].localPosition;
            ChildrenTransformList[i].localEulerAngles = ChildrenAttributes[i].localEulerAngles;
            yield return null;
        }
    }
}


public class ChildrenAttribute
{
    public Vector3 localPosition;
    public Vector3 localEulerAngles;

    public ChildrenAttribute() { }

    public ChildrenAttribute(Vector3 pos, Vector3 rot)
    {
        localPosition = pos;
        localEulerAngles = rot;
    }
}
