using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Undo;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class item : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler{
	public static item dragedItem;
	public static bool isDragging;
    public Image pic;
    public Text text;
    /// <summary>
    /// 目标物体
    /// </summary>
    public GameObject target;
    public Sprite hightLightSprite;
    public Button btn;
    /// <summary>
    /// 父物体名称
    /// </summary>
    public string parentName;
    /// <summary>
    /// 子物体是否显示
    /// </summary>
    public bool ChildIsShow = false;
    /// <summary>
    /// 自身是否显示
    /// </summary>
    public bool IsShow = true;
    /// <summary>
    /// 子物体名字列表
    /// </summary>
    public List<string> ChildrenName = new List<string>();
    /// <summary>
    /// 子物体对象列表
    /// </summary>
    public List<GameObject> ChildList = new List<GameObject>();
    /// <summary>
    /// 展示物体所在位置
    /// </summary>
    public Transform PIParent;

    bool pressed;
    /// <summary>
    /// 拖动时跟随鼠标移动的动画物体
    /// </summary>
    public GameObject dragImage;
    public GameObject clone;
    public Text _Text;
    public GameObject TopImageGameObject;

    private string str;
    // Use this for initialization
    void Start () {
        if (string.IsNullOrEmpty(parentName))
        {
            parentName = "Parent";
        }
        
        Register();
        //Debug.Log("********************");
        btn.onClick.AddListener( delegate(){ OnClick(btn);});        
        PIParent = GameObject.Find("PIParent").transform;
        dragImage = Resources.Load<GameObject>("Prefabs/DragImage");
       
	}
	
	// Update is called once per frame
	void Update () {

        //if (target==null)
        //{

        //    //ListTreeNode<Base> a = Manager.Instace.listTree.GetNode(Manager.Instace.dictFromObjectToInforma[moveTarget]);
        //       // Manager.Instace.DestroyData(a);
        //        //Manager.Instace.listTree.DeleteNode(a.parent, a.data);
        //}
        if (isDragging&&clone!=null)
        {
            clone.transform.position = Input.mousePosition+new Vector3(8,-8,0);

        }
        if(isDragging && !TopImageGameObject.activeSelf)
        {
            TopImageGameObject.SetActive(true);
        }
	    if (!isDragging&&TopImageGameObject.activeSelf)
	    {
	        TopImageGameObject.SetActive(false);
        }

        ////AutomaticSort();
        //ChangeIsFold();
        //SetActiveByIsFold();
        
        if (ChildIsShow)
        {
            _Text.text = "-";
        }
        else
        {
            _Text.text = "+";
        }
	}
    
    
    void OnGUI()
    {
        if (Event.current.isMouse && Event.current.type == EventType.MouseDown && Event.current.clickCount == 2)
        {
            if (pressed)
            {
                Camera.main.transform.position = target.transform.position + new Vector3(0, 3, 10);
                Camera.main.transform.LookAt(target.transform);
            }
        } 
    }
    /// <summary>
    /// 当被删除时清除父物体对自己的引用
    /// </summary>
    void OnDestroy()
    {
        if (parentName!="Parent")
        {
            GameObject[] GO = GameObject.FindGameObjectsWithTag("zsgc");
            for (int i = 0; i < GO.Length; i++)
            {
                if (GO[i].name==parentName)
                {
                    //Debug.LogError("1111");
                    GO[i].GetComponent<item>().ChildrenName.Remove(this.name);
                    GO[i].GetComponent<item>().InitChildList(true);
                }
            }
        }
    }
    
    //void OnMouseEnter()
    //{
    //    pressed = true;
    //}

    //void OnMouseExit()
    //{
    //    pressed = false;
    //}

    public void OnPointerEnter(PointerEventData eventData)
    {
        pressed = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        pressed = false;
    }

    public void SetTarget(GameObject obj)
    {
        target = obj;
    }

    public void OnClick(Button btn)
    {
       // Manager.Instace.SetEditorObjectTarget(target);
       Manager.Instace.gameObject.GetComponent<G_EditorTarget>(). SetMoveTarget(target);
       if (Manager.Instace.gameObject.GetComponent<G_MoveObject>() != null)
        {
            Manager.Instace.gameObject.GetComponent<G_MoveObject>().SetTarget(target);
        }

    }
    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="obj"></param>
    public void Delete(GameObject obj)
    {
        //Debug.Log(target);
        //Debug.LogError("5555555555");
        
        if (obj == target)
        {
            Destroy(gameObject);
            if (target.GetComponent<Main>() != null)
            {
                Destroy(target.GetComponent<Main>().Fsm);
                Manager.Instace.gonggong = null;
                Manager.Instace.backGround2.SetActive(false);
                Manager.Instace.backGround3.SetActive(false);
                Manager.Instace.backGround1.SetActive(true);
            }
            
        }

    }
    
    //注册事件
    void Register()
    {
        Manager.Instace.gameObject.GetComponent<G_EditorTarget>().deleteHandle += Delete;
    }

    void Cancell()
    {
        Manager.Instace.gameObject.GetComponent<G_EditorTarget>().deleteHandle -= Delete;
    }

	public void SetIsDragging(bool isDrag){
		isDragging = isDrag;
		if (isDrag) {
			dragedItem = this;
            
            GameObject a = Instantiate(dragImage);
            clone = a;
            clone.transform.SetParent(GameObject.Find("Canvas").transform);
            ChildIsShow = false;
            SetChildActive();
		} else {
			dragedItem = null;
            Destroy(clone);
		}
	}

	public GameObject GetTarget(){
		return target;
	}
    
    /// <summary>
    /// 拖动其他物体于此处松开
    /// </summary>
    public void DropGameObject()
    {
        if (item.isDragging&&item.dragedItem!=this)
        {
            //如果该物体原本有其他的父物体,则清除原本父物体关于该物体的索引
            if (item.dragedItem.parentName != "Parent")
            {
                GameObject[] Gos = GameObject.FindGameObjectsWithTag("zsgc");
                for (int i = 0; i < Gos.Length; i++)
                {
                    if (item.dragedItem.parentName == Gos[i].name)
                    {
                        for (int j = 0; j < Gos[i].GetComponent<item>().ChildrenName.Count; j++)
                        {
                            if (Gos[i].GetComponent<item>().ChildrenName[j] == item.dragedItem.name)
                            {
                                Gos[i].GetComponent<item>().ChildrenName
                                    .Remove(Gos[i].GetComponent<item>().ChildrenName[j]);
                                Gos[i].GetComponent<item>().ChildList.Remove(Gos[i].GetComponent<item>().ChildList[j]);
                                Gos[i].GetComponent<item>().UpdateChildren();
                                Gos[i].GetComponent<item>().InitChildPosition();

                            }
                        }
                    }
                }
            }
            item.dragedItem.transform.Find("Image_LeaveParent").gameObject.SetActive(true);
            item.dragedItem.parentName = this.target.name;
            //item.dragedItem.target.transform.SetParent(GameObject.Find("Parent").transform);
            item.dragedItem.target.transform.SetParent(this.target.transform);
            Debug.Log("target:"+this.target.name);
            this.ChildrenName.Add(item.dragedItem.name);
            int num = transform.GetSiblingIndex();
            item.dragedItem.transform.SetSiblingIndex(num + 1);
            item.dragedItem.InitChildPosition();
            this.ChildList.Add(item.dragedItem.gameObject);
            UpdateChildren();
            InitChildPosition();
            if (!ChildIsShow)
            {
                GameObject Go=item.dragedItem.gameObject;
                item.dragedItem.SetIsDragging(false);
                Go.GetComponent<item>().ChildIsShow = false;
                Go.GetComponent<item>().SetChildActive();
                Go.SetActive(false);
            }
        }
       
    }

    public void LeaveParent()
    {
        
        item.dragedItem = gameObject.GetComponent<item>();
        item.isDragging = true;
        GameObject[] Gos = GameObject.FindGameObjectsWithTag("zsgc");
        for (int i = 0; i < Gos.Length; i++)
        {
            if (item.dragedItem.parentName == Gos[i].name)
            {
                Gos[i].GetComponent<item>().DropTop();
            }
            break;
        }
        transform.Find("Image_LeaveParent").gameObject.SetActive(false);
        item.isDragging = false;
    }
    /// <summary>
    /// 顶部检测
    /// </summary>
    public void DropTop()
    {
        if (item.isDragging)
        {
            if (item.dragedItem.parentName == this.transform.name)
            {
                //将该子物体移除
                for (int i = 0; i < ChildrenName.Count; i++)
                {
                    if (item.dragedItem.transform.name==ChildrenName[i])
                    {
                        ChildrenName.Remove(ChildrenName[i]);
                        break;
                    }
                }
                InitChildList();
            }
            item.dragedItem.target.transform.SetParent(this.target.transform.parent);
            item.dragedItem.parentName = this.parentName;
            item.dragedItem.transform.Find("Image_LeaveParent").gameObject.SetActive(false);
            //int num = transform.GetSiblingIndex();
            //item.dragedItem.transform.SetSiblingIndex(num);
            item.dragedItem.InitChildPosition();
        }
       
    }
    /// <summary>
    /// 重置子物体的位置（在自己后面）
    /// </summary>
    public void InitChildPosition()
    {
        int num = transform.GetSiblingIndex();
        if (ChildList!=null&&ChildList.Count>0)
        {
            for (int i = 0; i < ChildList.Count; i++)
            {
                ChildList[i].transform.SetSiblingIndex(num + 1);
                if (ChildList[i].GetComponent<item>().ChildList!=null&&ChildList[i].GetComponent<item>().ChildList.Count>0)
                {
                    ChildList[i].GetComponent<item>().InitChildPosition();
                }
            }
        }
    }
    
    /// <summary>
    /// 控制子物体的显示与隐藏
    /// </summary>
    public void SetChildActive()
    {
        if (ChildList!=null&&ChildList.Count>0)
        {
            for (int i = 0; i < ChildList.Count; i++)
            {
                ChildList[i].GetComponent<item>().IsShow = ChildIsShow;
                if (ChildIsShow)
                {
                    ChildList[i].GetComponent<item>().ChildIsShow = ChildIsShow;
                    ChildList[i].GetComponent<item>().SetChildActive();
                    ChildList[i].transform.Find("Image_LeaveParent").gameObject.SetActive(true);
                }
                
                ChildList[i].SetActive(ChildIsShow);
            }
        }
        
    }
    /// <summary>
    /// 子物体是否显示的控制
    /// </summary>
    public void ChangeChildIsShow()
    {
        ChildIsShow = !ChildIsShow;
        SetChildActive();
    }
    /// <summary>
    /// 判断子物体数量是否大于0以确定是否需要显示+号
    /// </summary>
    public void UpdateChildren()
    {
        for (int i = 0; i < ChildList.Count; i++)
        {
            if (ChildList[i]==null)
            {
                ChildList.Remove(ChildList[i]);
            }
        }
        if (ChildList == null || ChildList.Count == 0)
        {
            _Text.transform.parent.gameObject.SetActive(false);
            ChildIsShow = false;
        }
        else if (ChildList != null && ChildList.Count > 0)
        {
            _Text.transform.parent.gameObject.SetActive(true);
        }
    }
    /// <summary>
    /// 在读取时刷新子物体链表
    /// </summary>
    public void InitChildList(bool IsFirst=false)
    {
        if (ChildrenName.Count != ChildList.Count)
        {
            ChildList.Clear();
            GameObject[] Gos = GameObject.FindGameObjectsWithTag("zsgc");
            for (int i = 0; i < ChildrenName.Count; i++)
            {
                for (int j = 0; j < Gos.Length; j++)
                {
                    if (ChildrenName[i] == Gos[j].name)
                    {
                        if (IsFirst==false)
                        {
                            Gos[j].SetActive(false);
                        }
                        ChildList.Add(Gos[j]);
                        break;
                    }
                }
            }

        }
        UpdateChildren();
        InitChildPosition();
    }


    /// <summary>
    /// 当物体的父物体不是parent时获取该物体在parent下的路径
    /// </summary>
    /// <returns></returns>
    public string GetTargetPath()
    {
        str = "";
        GetPath(target.transform);
        return str+target.name;
    }

    private void GetPath(Transform transform)
    {
        if (transform.parent.name!="Parent")
        {
            GetPath(transform.parent);
            str += transform.parent.name+"/";
        }
    }
}
