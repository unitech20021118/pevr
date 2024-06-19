using HighlightingSystem;
using System.Collections.Generic;
using Assets.Scripts.Undo;
using UnityEngine;
using UnityEngine.UI;

public class G_EditorTarget : MonoBehaviour {
    public bool isChose;
    public GameObject itemtemp;
    public Color selectColor=Color.white;
    public enum EditState
    {
        Invalid,
        Scale,
        Rotate,
        Note
    }

    public Material highLight;
    //edit by kuai
    //旧版本无用之物
    //public GameObject EditPanel;
    //public GameObject ScalePanel;
    //public GameObject RotatePanel;
    public Slider X;
    public Slider Y;
    public Slider Z;
    public Slider scale;
    public GameObject rotationCneter;
    public GameObject positionCenter;
    public GameObject scaleCenter;
    G_MoveObject moveObject;
    G_AdjustEditPanelPos adjustEditPanelPos; 
    G_Interface _interface;
    public GameObject moveTarget;
    public GameObject moveTargetPI;
    Vector3 rightMouseDownPos;
    Vector3 originalRotate;
    Vector3 originalScale;
    Vector3 eulerAngle;
    EditState currentState;
    Dictionary<Renderer, Material[]> originMaterial = new Dictionary<Renderer, Material[]>();
    private int num;
    public event DeleteHandle deleteHandle;





	// Use this for initialization
	void Start () {
	    rotationCneter.transform.localScale = new Vector3(1, 1, 1);
	    positionCenter.transform.localScale = new Vector3(1, 1, 1);
	    scaleCenter.transform.localScale = new Vector3(1, 1, 1);
	    G_MouseListener.GetInstance().AddLMouseDown(CheckGameObject);
	    G_MouseListener.GetInstance().AddRMouseDown(RightMouseDown);
	    //G_MouseListener.GetInstance().AddRMouseUp(ShowEditPanel);
	    moveObject = gameObject.GetComponent<G_MoveObject>();
	    _interface = gameObject.GetComponent<G_Interface>();
	    num = 2;

	    isChose = false;
	}
	
	// Update is called once per frame
    void Update()
    {
        //if (isChose == true)
        //{
        //    if (moveTarget!=null)
        //    {
        //        HighlighterController hc = moveTarget.GetComponentInParent<HighlighterController>();
        //        hc.MouseOver();
        //        //itemtemp = Manager.Instace.objectTopic[moveTarget];
        //        //itemtemp.transform.GetChild(1).GetComponent<itemShine>()._islight = true;
        //    }
        //    if (moveTargetPI!=null)
        //    {

        //        HighlighterController hc = moveTargetPI.GetComponentInParent<HighlighterController>();
        //        hc.MouseOver();
        //    }
            
        //}

        if ((moveTarget != null) && Input.GetKeyDown(KeyCode.Delete)&&Manager.Instace.saveObject==null)
        {

            DeleteObject();
        }
        //if (RotatePanel.activeInHierarchy && Input.GetMouseButton(0))
        //{
        //    RotateByX(X.value);
        //    RotateByY(Y.value);
        //    RotateByZ(Z.value);
        //}

    }

    public void UnDoDelete(GameObject targetObject)
    {
        GameObject thisOBJ = targetObject;
        int count = 0;
        while (thisOBJ.tag != "Editor")
        {

            thisOBJ = thisOBJ.transform.parent.gameObject;
            count++;
            if (count >= 10)
            {
                break;
            }
        }

        SetMoveTarget(thisOBJ);
        if (moveObject != null)
        {
            moveObject.SetTarget(thisOBJ);
            Debug.Log(rotationCneter.transform.localEulerAngles);
            G_TranformUI.Instance.SetObj(thisOBJ);
            Manager.Instace.gameObject.GetComponent<G_MouseListener>().DoOnCLickObject(thisOBJ);
        }

        if ((moveTarget != null) && Manager.Instace.saveObject == null)
        {
            DeleteObject(null,false);
        }
    }

    /// <summary>
    /// 点击选择 “移动/旋转/缩放”
    /// </summary>
    /// <param name="a"></param>
    public void ChoosePRS(int a){
        num=a;
        if (moveTarget != null)
        {
            SetTargetPRSTrue(moveTarget);
        }
    }

    void RightMouseDown()
    {
        rightMouseDownPos = Input.mousePosition;
    }
    //edit by kuai
    //旧版本无用之物
    //void ShowEditPanel()
    //{
    //    if (rightMouseDownPos != Input.mousePosition)
    //    {
    //        return;
    //    }
    //    Vector3 hitPos;
    //    GameObject target;
    //    if (!G_RayCastCheck.MouseCheckGameObejctReturnPos(out target, out hitPos, G_PubDef.DynamicObject))
    //    {
    //        EditPanel.SetActive(false);
    //        return;
    //    }
    //    EditPanel.SetActive(true);
    //    adjustEditPanelPos = EditPanel.GetComponent<G_AdjustEditPanelPos>();
    //    adjustEditPanelPos.SetEditorPanelPos();
    //    SetMoveTarget(target);
    //    _interface.SendMouseTouchEvent(moveTarget, hitPos);
    //}

    /// <summary>
    /// 删除高光组件
    /// </summary>
    /// <param name="obj"></param>
    void RemoveHighlight(GameObject obj)
    {
        if (obj.transform.GetComponent<Highlighter>() != null)
        {
            Destroy(obj.GetComponent<FlashingController>());
            Destroy(obj.GetComponent<Highlighter>());
        }
        Manager.Instace.SetDragObjUI();
    }
    /// <summary>
    /// 按下鼠标左键，通过射线检测物体
    /// </summary>
    void CheckGameObject()
    {
        if (!Input.GetKey(KeyCode.LeftAlt))
        {
            GameObject gameObjectChecked = null;
            //修改了射线检测的次序，先测轴，后测物体
            if (!G_RayCastCheck.MouseCheckGameObject(out gameObjectChecked, G_PubDef.layerMask))
            {
                SetTargetPRSFalse();

                //检测到物体
                if (G_RayCastCheck.MouseCheckGameObject(out gameObjectChecked, G_PubDef.DynamicObject))
                {
                    //如果检测到的是子物体，则建立目标的时候自动转换为父物体
                    //如果检测到的不是标签为Editor的物体去寻找他的父物体直到是Editor的标签
                    GameObject thisOBJ = gameObjectChecked;
                    Debug.Log("$$$$$$$$$" + thisOBJ.name);
                    int count = 0;
                    while (thisOBJ.tag != "Editor")
                    {
                        
                        thisOBJ = thisOBJ.transform.parent.gameObject;
                        count++;
                        if (count>=10)
                        {
                            Debug.LogError("选中了非场景建造的物体");
                            break;
                        }
                        
                    }

                    SetMoveTarget(thisOBJ);
                    //G_TranformUI.Instance.SetObj();
                    if (moveObject != null)
                    {
                        moveObject.SetTarget(thisOBJ);
                        //Debug.Log(rotationCneter.transform.localEulerAngles);
                        G_TranformUI.Instance.SetObj(thisOBJ);
                        Manager.Instace.gameObject.GetComponent<G_MouseListener>().DoOnCLickObject(thisOBJ);
                        //SpriteState aa = Manager.Instace.objectTopic[gameObjectChecked].GetComponentInChildren<Button>().spriteState;
                        //ss.highlightedSprite=Manager.Instace.objectTopic[gameObjectChecked].GetComponentInChildren<Button>().spriteState.highlightedSprite;
                        //Manager.Instace.objectTopic[gameObjectChecked].GetComponentInChildren<Button>().spriteState = ss;
                    }
                }
                else//没检测到物体
                {
                    rotationCneter.GetComponent<G_RotateBall>().Reset();
                    positionCenter.GetComponent<G_PositionAxis>().Reset();
                    if (moveTarget != null)
                    {
                        GetOriginColor();
                        moveTarget = null;
                    }
                    if (moveTargetPI!=null)
                    {
                        GetOriginColorPI();
                        moveTargetPI = null;
                    }
                    if (Manager.Instace.gonggong != null)
                    {
                        if (Manager.Instace.gonggong.GetComponent<Main>() != null)
                        {
                            Manager.Instace.gonggong.GetComponent<Main>().Fsm.SetActive(false);
                        }
                    }


                    Manager.Instace.gonggong = null;
                    Manager.Instace.backGround2.SetActive(false);
                    Manager.Instace.backGround3.SetActive(false);
                    Manager.Instace.backGround1.SetActive(true);
                    //foreach (Transform t in Manager.Instace.pannel)
                    //{
                    //    t.gameObject.SetActive(false);
                    //    Debug.LogError("555");
                    //}
                    Manager.Instace.AddFSM.SetActive(false);
                    moveTarget = null;
                    //EditPanel.SetActive(false);

                }
                //edit by kuai
                //旧版本无用之物
                //EditPanel.SetActive(false);
                //RotatePanel.SetActive(false);

            }

        }
       
    }



    /// <summary>
    /// 设置要操作的目标
    /// </summary>
    /// <param name="temp"></param>
    public  void SetMoveTarget(GameObject temp){
        
        if (temp != moveTarget)
        {
            if (moveTargetPI != null)
            {
                //Debug.LogError("11111111");
                GetOriginColorPI();
                moveTargetPI = null;
            }
            if (moveTarget != null)
            {
                
                //将上一个物体颜色还原

                GetOriginColor();
            }
            //Debug.LogError("222222");
            moveTarget = temp;
            
            itemtemp = Manager.Instace.objectTopic[moveTarget];
            itemtemp.transform.GetChild(1).GetComponent<itemShine>()._islight = true;
            HighLight(moveTarget);
            //Debug.LogError("3333333");
            SetTargetPRSTrue(moveTarget);
            //Debug.LogError("4444444444");
        }
        
    }
    /// <summary>
    /// 设置目标物体(PI)
    /// </summary>
    /// <param name="obj"></param>
    public void SetMoveTargetPI(GameObject obj)
    {
        
        if (obj!=moveTargetPI)
        {
            if (moveTarget != null)
            {
                GetOriginColor();
                moveTarget = null;
            }
            if (moveTargetPI!=null)
            {
                //将上一个物体颜色还原
                GetOriginColorPI();
            }
            moveTargetPI = obj;
            
            HighLight(moveTargetPI);
        }
    }
    /// <summary>
    /// 让物体变回原来的颜色(PI)
    /// </summary>
    public void GetOriginColorPI()
    {
        if (moveTargetPI==null)
        {
            return;
        }
        isChose = false;
        RemoveHighlight(moveTargetPI);
    }
    /// <summary>
    /// 让旋转坐标轴显示在目标物体上
    /// </summary>
    /// <param name="target"></param>
    public void SetTargetPRSTrue(GameObject target)
    {
        switch(num){
            case 1: rotationCneter.SetActive(true);
                    positionCenter.SetActive(false);
                    scaleCenter.SetActive(false);
                    //rotationCneter.transform.position = target.transform.position;
                    G_RotateBall rotateBall = rotationCneter.GetComponent<G_RotateBall>();
                    rotateBall.SetTargetTwo(target);
                    break;
            case 2: rotationCneter.SetActive(false);
                    positionCenter.SetActive(true);
                    scaleCenter.SetActive(false);
                    //positionCenter.transform.position = target.transform.position;
                    G_PositionAxis positionAxis=positionCenter.GetComponent<G_PositionAxis>();
                    positionAxis.SetTargetTwo(target);
                    break;
            case 3: rotationCneter.SetActive(false);
                    positionCenter.SetActive(false);
                    scaleCenter.SetActive(true);
                    //scaleCenter.transform.position = target.transform.position;
                    G_ScaleAxis scaleAxis=scaleCenter.GetComponent<G_ScaleAxis>();
                    scaleAxis.SetTarget(target);
                    break;
            default: break;
        }
        
    }

    /// <summary>
    /// 让旋转坐标轴影藏
    /// </summary>
    /// <param name="target"></param>
    public void SetTargetPRSFalse()
    {
        rotationCneter.SetActive(false);
        positionCenter.SetActive(false);
        scaleCenter.SetActive(false);
        G_RotateBall rotateBall = rotationCneter.GetComponent<G_RotateBall>();
        rotateBall.DropTarget();
    }

    /// <summary>
    /// 对所选中的物体变换颜色
    /// </summary>
    public void HighLight(GameObject obj)
    {
        if (!obj.GetComponent<FlashingController>())
        {
            obj.AddComponent<FlashingController>();
        }
        Manager.Instace.SetDragObjUI(obj.transform);
        isChose = true;
        //Renderer[] allChildObject = moveTarget.GetComponentsInChildren<Renderer>();
        //foreach (Renderer i in allChildObject)
        //{
        //    Material[] materials = i.materials;
        //    originMaterial[i] = materials;
        //    Material[] tempArray = new Material[materials.Length];
        //    for (int j = 0; j < tempArray.Length; j++)
        //    {
        //        tempArray[j] = new Material(highLight);
        //        tempArray[j].mainTexture = materials[j].mainTexture;
        //        //--------------------------------
        //        isChose = true;
        //        tempArray[j].SetColor("_EmissionColor", new Color(0.7f, 1, 1, 1));
        //    }
        //    i.materials = tempArray;
        //}       
    }

    /// <summary>
    /// 物体获得原来的颜色
    /// </summary>
    public void GetOriginColor()
    {
        if (moveTarget==null)
        {
            return;
        }
        isChose = false;
        RemoveHighlight(moveTarget);
        itemtemp = Manager.Instace.objectTopic[moveTarget];
        itemtemp.transform.GetChild(1).GetComponent<itemShine>()._islight = false;
        //Renderer[] renders=moveTarget.GetComponentsInChildren<Renderer>();
        //foreach (Renderer i in renders)
        //{
        //    if (originMaterial.ContainsKey(i))
        //    {
        //        i.materials = originMaterial[i];
        //        //---------------------
        //        isChose = false;
        //    }
            
        //}
        //originMaterial.Clear();

        //rotationCneter.SetActive(false);
    }

    /// <summary>
    /// 删除物体
    /// </summary>
    public void DeleteObject(GameObject obj=null,bool isUndo =true)
    {
        Debug.Log("MoveTarget in G_EditorTarget :" + moveTarget.name);
        if (moveObject != null)
        {
            if (obj == null)
            {
                obj = moveTarget;
            }
            //Product.DeleteGameObject(moveTarget.gameObject);
            if (deleteHandle != null)
            {
                //使得该物体上的信息存储也删除
                Debug.Log(Informa<Base>.allData.Count);
                //int num = moveTarget.GetComponent<GameObjectIndex>().index;
                if (obj.layer.Equals(9))
                {
                    Product.DeleteGameObject(obj);
                }
                
                //Manager.Instace.listTree.DeleteNode(Manager.Instace.listTree.Root, Manager.Instace.dictFromObjectToInforma[moveTarget]);
                //Debug.Log(Informa<Base>.allData.Count);
                //deleteHandle(moveTarget);//事件发生
                
                try
                {
                    ListTreeNode<Base> a = Manager.Instace.allDataInformation.listTree.GetNode(Manager.Instace.dictFromObjectToInforma[obj]);
                    Manager.Instace.DestroyData(a);
                    Manager.Instace.allDataInformation.listTree.DeleteNode(a.parent, a.data);
                    
                    //删除物体的Undo操作 Editby 潘晓峰
                    if (isUndo == true)
                    {
                        var objInfo = new CreateUndoObjInfo()
                        {
                            LocalScale = obj.transform.localScale,
                            Rotation =  obj.transform.rotation,
                            Position = obj.transform.position,
                            Data = a,
                        };

                        obj.GetComponent<CreateObjUndoComponent>().AddDeleteUnDoCommand(objInfo);

                    }
                }
                catch (System.Exception e)
                {

                    if (e!=null)
                    {
                        Debug.LogError(e);
                        
                        return;
                    }
                }
                
                
            }
            for (int i = 0; i < obj.transform.childCount; i++)
            {
                if (obj.transform.GetChild(i).tag == "Editor")
                {
                    DeleteObject(obj.transform.GetChild(i).gameObject);
                }
            }
            //Debug.LogError("删除");
            moveObject.Drop();
            //add by kuai
            if (Manager.Instace.objectToFsm.ContainsKey(obj))
            {
                Manager.Instace.objectToFsm.Remove(obj);
            }
            Destroy(obj);
            moveTarget = null;

            

        }
        positionCenter.SetActive(false);
        //EditPanel.SetActive(false);
        _interface.SendMouseClickEvent(G_Interface.EditMenuDef.Delete);
    }
    //edit by kuai
    //旧版本无用之物
    /// <summary>
    /// 旋转物体
    /// </summary>
    //public void Rotate()
    //{
    //    RotatePanel.SetActive(true);
    //    ScalePanel.SetActive(false);
    //    EditPanel.SetActive(false);
    //    if (currentState != EditState.Rotate)
    //    {
    //        originalRotate = Vector3.zero;
    //        Slider[] XYZAxis = RotatePanel.GetComponentsInChildren<Slider>();
    //        for (int i = 0; i < XYZAxis.Length; i++)
    //        {
    //            XYZAxis[i].value = 0f;
    //        }

    //    }
    //    eulerAngle = moveTarget.transform.rotation.eulerAngles;
    //    currentState = EditState.Rotate;
    //    _interface.SendMouseClickEvent(G_Interface.EditMenuDef.Rotate);

    //}

    public void RotateByX(float value)
    {
        if (moveTarget != null)
        {
           moveTarget .transform.Rotate(Vector3.right, value - originalRotate.x, Space.World);
        }
        originalRotate.x = value;
    }

    public void RotateByY(float value)
    {
        if (moveTarget != null)
        {
            moveTarget.transform.Rotate(Vector3.up, value - originalRotate.y, Space.World);
        }
        originalRotate.y = value;
    }

    public void RotateByZ(float value)
    {
        if (moveTarget != null)
        {
            moveTarget.transform.Rotate(Vector3.forward, value - originalRotate.z, Space.World);
        }
        originalRotate.z = value;
    }

    public void Say(int a)
    {
        
    }
    //edit by kuai
    //旧版本无用之物
    //public void SetFalseRotatePanel()
    //{
    //    RotatePanel.SetActive(false);
    //    moveTarget.transform.eulerAngles = eulerAngle;
        
    //}

    //public void SetTrueRotatePanel()
    //{
    //    RotatePanel.SetActive(false);
        
    //}

    /// <summary>
    /// 调整大小
    /// </summary>
    //public void Scale()
    //{
    //    if (moveTarget != null)
    //    {
    //        EditPanel.SetActive(false);
    //        if (currentState != EditState.Scale)
    //        {
    //            ScalePanel.SetActive(true);
    //            originalScale = moveTarget.transform.localScale;
    //            Slider[] childs = ScalePanel.GetComponentsInChildren<Slider>();
    //            foreach (Slider i in childs)
    //            {
    //                i.value = 100;
    //            }
    //        }
    //        currentState = EditState.Scale;
    //        _interface.SendMouseClickEvent(G_Interface.EditMenuDef.Scale);
    //    }
    //}

    public void ScaleBySlider()

    {
        if (moveTarget != null)
        {
            moveTarget.transform.localScale = originalScale * scale.value / 100;
        }
        
    }
    //edit by kuai
    //旧版本无用之物
    //public void SetFalseScalePanel()
    //{
    //    ScalePanel.SetActive(false);
    //    currentState = EditState.Invalid;
    //}

    //public void AddNote()
    //{
    //    EditPanel.SetActive(false);
    //    if (moveTarget != null)
    //    {
    //        _interface.SendMouseClickEvent(G_Interface.EditMenuDef.AddNote);
    //    }
    //}
}
