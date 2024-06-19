using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveTargetUI : ActionUI
{
    /// <summary>
    /// 开关
    /// </summary>
    public Toggle ONOFFToggle;
    /// <summary>
    /// 无视重力
    /// </summary>
    public Toggle NoGravityToggle;
    /// <summary>
    /// /移动速度
    /// </summary>
    public InputField SpeedInputField;
    /// <summary>
    /// 显示目标物体的文本框
    /// </summary>
    public Text TargetText;
    /// <summary>
    /// 选择第一个人称为目标
    /// </summary>
    public Toggle FpsToggle;
    /// <summary>
    /// 选择目标的物体
    /// </summary>
    public GameObject TargetImageGameObject;

    private GameObject targetGameObject;
    private string targetPath;
    private string targetName;
    private MoveTarget _moveTarget;
    private MoveTargetInforma _moveTargetInforma;
    public override Action<Main> CreateAction()
    {
        action = new MoveTarget();
        actionInforma = new MoveTargetInforma(true);
        _moveTarget = (MoveTarget) action;
        _moveTargetInforma = (MoveTargetInforma) actionInforma;
        //初始化数值
        Init();
        GetStateInfo().actionList.Add(actionInforma);
        actionInforma.name = "MoveTarget";
        return base.CreateAction();
    }
    public override Action<Main> LoadAction(ActionInforma actionInforma)
    {
        _moveTargetInforma = (MoveTargetInforma)actionInforma;
        this.actionInforma = actionInforma;
        action = new MoveTarget();
        _moveTarget = (MoveTarget)action;
        //读取数据
        LoadData();
        return base.LoadAction(actionInforma);
    }
    /// <summary>
    /// 设置开关
    /// </summary>
    public void SetONOFF()
    {
        _moveTarget.ONOFF = ONOFFToggle.isOn;
        _moveTargetInforma.ONOFF = ONOFFToggle.isOn;
    }
    /// <summary>
    /// 设置无视重力
    /// </summary>
    public void SetGravity()
    {
        _moveTarget.noGravity = NoGravityToggle.isOn;
        _moveTargetInforma.noGravity = NoGravityToggle.isOn;
    }
    /// <summary>
    /// 设置速度
    /// </summary>
    public void SetSpeed()
    {
        if (float.Parse(SpeedInputField.text)<0)
        {
            SpeedInputField.text = "0";
        }
        _moveTarget.speed = float.Parse(SpeedInputField.text);
        _moveTargetInforma.speed = float.Parse(SpeedInputField.text);
    }
    /// <summary>
    /// 设置第一人称
    /// </summary>
    public void SetFPS()
    {
        _moveTargetInforma.FPS = FpsToggle.isOn;
        _moveTarget.FPS = FpsToggle.isOn;
        if (FpsToggle.isOn)
        {
            TargetImageGameObject.SetActive(false);
        }
        else
        {
            TargetImageGameObject.SetActive(true);
        }
    }
    public void DropGameObject()
    {
        if (item.isDragging)
        {
            targetGameObject = item.dragedItem.GetTarget();
            targetPath = item.dragedItem.GetTargetPath();
            targetName = targetGameObject.name;
            GetTarget();
        }
    }
    public void GetTarget()
    {
        _moveTarget.target = targetGameObject;
        //给拖拽的目标物体上一个蓝色的边缘发光效果
        if (targetGameObject && !targetGameObject.GetComponent<ConstantHighlighting>())
        {
            targetGameObject.AddComponent<ConstantHighlighting>();
        }

        if (!string.IsNullOrEmpty(targetName))
        {
            string[] shortName = targetName.Split('/');
            if (shortName.Length > 0)
                TargetText.text = shortName[shortName.Length - 1];
        }
        if (!string.IsNullOrEmpty(targetPath))
        {
            _moveTargetInforma.targetName = targetName;
            _moveTargetInforma.rootName = targetPath;
        }
        else
        {
            _moveTargetInforma.targetName = targetGameObject.name;
        }
    }
    /// <summary>
    /// 初始化数值
    /// </summary>
    private void Init()
    {
        SpeedInputField.text = "1";
        _moveTargetInforma.speed = 1f;
        _moveTarget.speed = 1f;

        NoGravityToggle.isOn = false;
        _moveTargetInforma.noGravity = false;
        _moveTarget.noGravity = false;

        ONOFFToggle.isOn = true;
        _moveTargetInforma.ONOFF = true;
        _moveTarget.ONOFF = true;

        FpsToggle.isOn = true;
        _moveTargetInforma.FPS = true;
        _moveTarget.FPS = true;
    }
    /// <summary>
    /// 读取数据
    /// </summary>
    private void LoadData()
    {
        ONOFFToggle.isOn = _moveTargetInforma.ONOFF;
        _moveTarget.ONOFF = _moveTargetInforma.ONOFF;

        SpeedInputField.text = _moveTargetInforma.speed.ToString();
        _moveTarget.speed = _moveTargetInforma.speed;

        NoGravityToggle.isOn = _moveTargetInforma.noGravity;
        _moveTarget.noGravity = _moveTargetInforma.noGravity;

        targetName = _moveTargetInforma.targetName;
        targetPath = _moveTargetInforma.rootName;
        TargetText.text = targetName;
        _moveTarget.targetPath = targetPath;

        FpsToggle.isOn = _moveTargetInforma.FPS;
        _moveTarget.FPS = _moveTargetInforma.FPS;
    }
}
