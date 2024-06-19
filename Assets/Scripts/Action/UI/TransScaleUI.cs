using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransScaleUI : ActionUI
{
    /// <summary>
    /// x轴输入框
    /// </summary>
    public InputField trans_X;
    /// <summary>
    /// y轴输入框
    /// </summary>
    public InputField trans_Y;
    /// <summary>
    /// z轴输入框
    /// </summary>
    public InputField trans_Z;
    /// <summary>
    /// 持续时间文本输入框
    /// </summary>
    public InputField durationTime;
    /// <summary>
    /// 目标物体
    /// </summary>
    public GameObject target;
    /// <summary>
    /// 目标物体显示的文本框
    /// </summary>
    public Text targetText;
    private TransScale _transScale;
    private TransScaleInforma _transScaleInforma;

    //void Start()
    //{
    //    if (string.IsNullOrEmpty(targetName))
    //    {
    //        targetText.text = "拖拽物体至此框内";
    //    }
    //}
    public override Action<Main> CreateAction()
    {
        action = new TransScale();
        action.isOnce = true;
        _transScale = (TransScale)action;
        actionInforma = new TransScaleInforma(true);
        _transScaleInforma = (TransScaleInforma)actionInforma;

        //初始化相关变量
        _transScale.Target_V3 = Vector3.one;

        GetStateInfo().actionList.Add(actionInforma);
        actionInforma.name = "TransScale";
        return base.CreateAction();
    }

    public override Action<Main> LoadAction(ActionInforma actionInforma)
    {
        _transScaleInforma = (TransScaleInforma)actionInforma;
        this.actionInforma = actionInforma;
        action = new TransScale();
        _transScale = (TransScale)action;

        //读取数据
        _transScale.Target_V3.x = _transScaleInforma.trans_X;
        _transScale.Target_V3.y = _transScaleInforma.trans_Y;
        _transScale.Target_V3.z = _transScaleInforma.trans_Z;

        _transScale.targetName = _transScaleInforma.targetName;
        _transScale.durationTime = _transScaleInforma.durationTime;
        _transScale.target = target;

        if (string.IsNullOrEmpty(_transScaleInforma.targetName))
        {
            targetText.text = "拖拽物体至此框内";
        }
        else
        {
            targetText.text = _transScaleInforma.targetName;
        }

        //设置UI界面属性
        trans_X.text = _transScaleInforma.trans_X.ToString();
        trans_Y.text = _transScaleInforma.trans_Y.ToString();
        trans_Z.text = _transScaleInforma.trans_Z.ToString();
        durationTime.text = _transScaleInforma.durationTime.ToString();
        this.actionInforma = actionInforma;
        return action;
    }
    /// <summary>
    /// x输入框的值改变时
    /// </summary>
    public void Changed_X()
    {
        _transScale.Target_V3.x = float.Parse(trans_X.text);
        _transScaleInforma.trans_X = float.Parse(trans_X.text);

    }
    /// <summary>
    /// y输入框的值改变时
    /// </summary>
    public void Changed_Y()
    {
        _transScale.Target_V3.y = float.Parse(trans_Y.text);
        _transScaleInforma.trans_Y = float.Parse(trans_Y.text);
    }
    /// <summary>
    /// z输入框的值改变时
    /// </summary>
    public void Changed_Z()
    {
        _transScale.Target_V3.z = float.Parse(trans_Z.text);
        _transScaleInforma.trans_Z = float.Parse(trans_Z.text);
    }
    /// <summary>
    /// 时间输入框的值改变时
    /// </summary>
    public void Changed_T()
    {
        _transScale.durationTime = float.Parse(durationTime.text);
        _transScaleInforma.durationTime = float.Parse(durationTime.text);
    }

    public void DropGameObject()
    {
        if (item.isDragging)
        {
            target = item.dragedItem.GetTarget();
            targetText.text = target.name;

            _transScale.target = target;
            _transScale.targetName = target.name;
            _transScaleInforma.targetName = target.name;
        }
    }
}
