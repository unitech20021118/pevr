using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryUI : ActionUI
{
    public GameObject KeyObjPrefab;
    public GameObject keyObj;
    /// <summary>
    /// 传送的目标位置
    /// </summary>
    public Vector3 DeliveryPosition;
    /// <summary>
    /// 目标物体
    /// </summary>
    public GameObject target;
    /// <summary>
    /// 目标物体显示的文本框
    /// </summary>
    public Text targetText;
    public string targetName;

    public GameObject SelectPositionButton;

    public GameObject SavePositionButton;
    // Use this for initialization

    private Delivery _delivery;
    private DeliveryInforma _deliveryInforma;

    public override Action<Main> CreateAction()
    {
        action = new Delivery();
        action.isOnce = true;
        actionInforma = new DeliveryInforma(true);
        _delivery = (Delivery)action;
        _deliveryInforma = (DeliveryInforma)actionInforma;
        GetStateInfo().actionList.Add(actionInforma);
        //初始化属性
        
        actionInforma.name = "Delivery";
        return base.CreateAction();
    }

    public override Action<Main> LoadAction(ActionInforma actionInforma)
    {
        _deliveryInforma = (DeliveryInforma)actionInforma;
        //		this.actionInforma = actionInforma;
        action = new Delivery();
        _delivery = (Delivery)action;
        //读取属性
        targetName = _deliveryInforma.targetName;
        _delivery.targetName = _deliveryInforma.targetName;
        DeliveryPosition = new Vector3(float.Parse(_deliveryInforma.X), float.Parse(_deliveryInforma.Y), float.Parse(_deliveryInforma.Z));
        _delivery.TargetPosition = new Vector3(float.Parse(_deliveryInforma.X), float.Parse(_deliveryInforma.Y), float.Parse(_deliveryInforma.Z));

        if (keyObj == null)
        {
            keyObj = Instantiate<GameObject>(KeyObjPrefab);
        }
        keyObj.transform.position = DeliveryPosition;

        this.actionInforma = actionInforma;
        return base.LoadAction(actionInforma);
    }



    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// 检测拖动其他物体到目标文本框
    /// </summary>
    public void DropGameObject()
    {
        if (item.isDragging)
        {
            target = item.dragedItem.GetTarget();

            targetText.text = target.name;


            _delivery.target = target;
            _delivery.targetName = target.name;
            _deliveryInforma.targetName = target.name;
        }
    }
    /// <summary>
    /// 显示目标球
    /// </summary>
    public void ShowKeyObject()
    {
        if (keyObj==null)
        {
            keyObj = Instantiate<GameObject>(KeyObjPrefab, target.transform.position, Quaternion.identity);
        }
        keyObj.SetActive(true);
        keyObj.GetComponent<VTest>().owner = gameObject;
        keyObj.GetComponent<VTest>().SetTarget();
        _delivery.TargetPosition = keyObj.transform.position;
        _deliveryInforma.X = keyObj.transform.position.x.ToString();
        _deliveryInforma.Y = keyObj.transform.position.y.ToString();
        _deliveryInforma.Z = keyObj.transform.position.z.ToString();
        SelectPositionButton.SetActive(false);
        SavePositionButton.SetActive(true);
    }
    /// <summary>
    /// 保存目标球的位置信息
    /// </summary>
    public void SaveKeyObjectPosition()
    {
        if (keyObj==null)
        {
            Debug.LogError("没有创建目标球");
            return;
        }
        keyObj.SetActive(false);
        _delivery.TargetPosition = keyObj.transform.position;
        _deliveryInforma.X = keyObj.transform.position.x.ToString();
        _deliveryInforma.Y = keyObj.transform.position.y.ToString();
        _deliveryInforma.Z = keyObj.transform.position.z.ToString();
        SelectPositionButton.SetActive(true);
        SavePositionButton.SetActive(false);
    }
}
