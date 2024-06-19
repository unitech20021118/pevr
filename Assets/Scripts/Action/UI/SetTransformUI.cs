using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 设置Transform的UI脚本  created by kuai
/// </summary>
public class SetTransformUI : ActionUI
{
    /// <summary>
    /// 位置的X轴
    /// </summary>
    public InputField XpInputField;
    /// <summary>
    /// 位置的Y轴
    /// </summary>
    public InputField YpInputField;
    /// <summary>
    /// 位置的Z轴
    /// </summary>
    public InputField ZpInputField;
    
    /// <summary>
    /// 旋转的X轴
    /// </summary>
    public InputField XrInputField;
    /// <summary>
    /// 旋转的Y轴
    /// </summary>
    public InputField YrInputField;
    /// <summary>
    /// 旋转的Z轴
    /// </summary>
    public InputField ZrInputField;

    /// <summary>
    /// 缩放的X轴
    /// </summary>
    public InputField XsInputField;
    /// <summary>
    /// 缩放的Y轴
    /// </summary>
    public InputField YsInputField;
    /// <summary>
    /// 缩放的Z轴
    /// </summary>
    public InputField ZsInputField;

    /// <summary>
    /// 目标物体
    /// </summary>
    public GameObject TargetGameObject;
    /// <summary>
    /// 显示目标物体名字的文本框
    /// </summary>
    public Text TargetText;
    private string targetPath;
    public string targetName;

    private SetTransform _setTransform;
    private SetTransformInforma _setTransformInforma;

    public override Action<Main> CreateAction()
    {

        action = new SetTransform();
        //action.isOnce = true;
        actionInforma = new SetTransformInforma(true);
        _setTransform = (SetTransform)action;
        _setTransformInforma = (SetTransformInforma)actionInforma;
        //初始化变量
        _setTransformInforma.xp = "";
        _setTransformInforma.yp = "";
        _setTransformInforma.zp = "";
        _setTransformInforma.xr = "";
        _setTransformInforma.yr = "";
        _setTransformInforma.zr = "";
        _setTransformInforma.xs = "";
        _setTransformInforma.ys = "";
        _setTransformInforma.zs = "";

        _setTransform.xp = "";
        _setTransform.yp = "";
        _setTransform.zp = "";
        _setTransform.xr = "";
        _setTransform.yr = "";
        _setTransform.zr = "";
        _setTransform.xs = "";
        _setTransform.ys = "";
        _setTransform.zs = "";



        GetStateInfo().actionList.Add(actionInforma);
        actionInforma.name = "SetTransform";
        return base.CreateAction();
    }


    public override Action<Main> LoadAction(ActionInforma actionInforma)
    {
        _setTransformInforma = (SetTransformInforma)actionInforma;
        this.actionInforma = actionInforma;
        action = new SetTransform();
        _setTransform = (SetTransform)action;
        //读取数据
        _setTransform.xp = _setTransformInforma.xp;
        XpInputField.text = _setTransformInforma.xp;

        _setTransform.yp = _setTransformInforma.yp;
        YpInputField.text = _setTransformInforma.yp;

        _setTransform.zp = _setTransformInforma.zp;
        ZpInputField.text = _setTransformInforma.zp;

        _setTransform.xr = _setTransformInforma.xr;
        XrInputField.text = _setTransformInforma.xr;

        _setTransform.yr = _setTransformInforma.yr;
        YrInputField.text = _setTransformInforma.yr;

        _setTransform.zr = _setTransformInforma.zr;
        ZrInputField.text = _setTransformInforma.zr;

        _setTransform.xs = _setTransformInforma.xs;
        XsInputField.text = _setTransformInforma.xs;

        _setTransform.ys = _setTransformInforma.ys;
        YsInputField.text = _setTransformInforma.ys;

        _setTransform.zs = _setTransformInforma.zs;
        ZsInputField.text = _setTransformInforma.zs;

        targetName = _setTransformInforma.targetName;
        targetPath = _setTransformInforma.rootName;
        _setTransform.targetPath = _setTransformInforma.rootName;

        if (!string.IsNullOrEmpty(_setTransformInforma.targetName))
        {
            TargetText.text = _setTransformInforma.targetName;
        }
        return base.LoadAction(actionInforma);
    }


    /// <summary>
    /// 设置要修改的值
    /// </summary>
    /// <param name="str"></param>
    public void SetTrans(string str)
    {
        switch (str)
        {
            case "xp":
                _setTransformInforma.xp = XpInputField.text;
                _setTransform.xp = XpInputField.text;
                break;
            case "yp":
                _setTransformInforma.yp = YpInputField.text;
                _setTransform.yp = YpInputField.text;
                break;
            case "zp":
                _setTransformInforma.zp = ZpInputField.text;
                _setTransform.zp = ZpInputField.text;
                break;
            case "xr":
                _setTransformInforma.xr = XrInputField.text;
                _setTransform.xr = XrInputField.text;
                break;
            case "yr":
                _setTransformInforma.yr = YrInputField.text;
                _setTransform.yr = YrInputField.text;
                break;
            case "zr":
                _setTransformInforma.zr = ZrInputField.text;
                _setTransform.zr = ZrInputField.text;
                break;
            case "xs":
                _setTransformInforma.xs = XsInputField.text;
                _setTransform.xs = XsInputField.text;
                break;
            case "ys":
                _setTransformInforma.ys = YsInputField.text;
                _setTransform.ys = YsInputField.text;
                break;
            case "zs":
                _setTransformInforma.zs = ZsInputField.text;
                _setTransform.zs = ZsInputField.text;
                break;
        }
    }

    public void DropGameObject()
    {
        if (item.isDragging)
        {
            TargetGameObject = item.dragedItem.GetTarget();
            targetPath = item.dragedItem.GetTargetPath();
            targetName = TargetGameObject.name;
            GetTarget();
        }
    }
    public void GetTarget()
    {

        _setTransform.target = TargetGameObject;
        //给拖拽的目标物体上一个蓝色的边缘发光效果
        if (TargetGameObject && !TargetGameObject.GetComponent<ConstantHighlighting>())
        {
            TargetGameObject.AddComponent<ConstantHighlighting>();
        }

        if (!string.IsNullOrEmpty(targetName))
        {
            TargetText.text = targetName;
            _setTransformInforma.targetName = targetName;
        }
        if (!string.IsNullOrEmpty(targetPath))
        {
            _setTransformInforma.rootName = targetPath;
        }
    }
}