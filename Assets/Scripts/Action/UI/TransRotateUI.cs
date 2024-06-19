using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// created by kuai
/// </summary>
public class TransRotateUI : ActionUI
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

    private TransRotate _transRotate;
    private TransRotateInforma _transRotateInforma;


    //void Start()
    //{
    //    if (string.IsNullOrEmpty(targetName))
    //    {
    //        targetText.text = "拖拽物体至此框内";
    //    }
    //}
    public override Action<Main> CreateAction()
    {
        action = new TransRotate();
        action.isOnce = true;
        _transRotate = (TransRotate)action;
        actionInforma = new TransRotateInforma(true);
        _transRotateInforma = (TransRotateInforma)actionInforma;

        //初始化相关变量
        _transRotate.Target_V3 = Manager.Instace.gonggong.transform.localEulerAngles;

        GetStateInfo().actionList.Add(actionInforma);
        actionInforma.name = "TransRotate";
        return base.CreateAction();
    }

    public override Action<Main> LoadAction(ActionInforma actionInforma)
    {
        _transRotateInforma = (TransRotateInforma)actionInforma;
        this.actionInforma = actionInforma;
        action = new TransRotate();
        _transRotate = (TransRotate)action;
        
        
        //读取数据

        _transRotate.Target_V3.x = _transRotateInforma.trans_X;
        _transRotate.Target_V3.y = _transRotateInforma.trans_Y;
        _transRotate.Target_V3.z = _transRotateInforma.trans_Z;

        _transRotate.targetName = _transRotateInforma.targetName;
        _transRotate.durationTime = _transRotateInforma.durationTime;
        _transRotate.target = target;

        
        if (string.IsNullOrEmpty(_transRotateInforma.targetName))
        {
            targetText.text = "拖拽物体至此框内";
        }
        else
        {
            targetText.text = _transRotateInforma.targetName;
        }

        //设置UI界面属性
        trans_X.text = _transRotateInforma.trans_X.ToString();
        trans_Y.text = _transRotateInforma.trans_Y.ToString();
        trans_Z.text = _transRotateInforma.trans_Z.ToString();
        durationTime.text = _transRotateInforma.durationTime.ToString();
        this.actionInforma = actionInforma;
        return action;
    }
    /// <summary>
    /// x输入框的值改变时
    /// </summary>
    public void Changed_X()
    {
        _transRotate.Target_V3.x = float.Parse(trans_X.text);
        _transRotateInforma.trans_X = float.Parse(trans_X.text);

    }
    /// <summary>
    /// y输入框的值改变时
    /// </summary>
    public void Changed_Y()
    {
        _transRotate.Target_V3.y = float.Parse(trans_Y.text);
        _transRotateInforma.trans_Y = float.Parse(trans_Y.text);
    }
    /// <summary>
    /// z输入框的值改变时
    /// </summary>
    public void Changed_Z()
    {
        _transRotate.Target_V3.z = float.Parse(trans_Z.text);
        _transRotateInforma.trans_Z = float.Parse(trans_Z.text);
    }
    /// <summary>
    /// 时间输入框的值改变时
    /// </summary>
    public void Changed_T()
    {
        _transRotate.durationTime = float.Parse(durationTime.text);
        _transRotateInforma.durationTime = float.Parse(durationTime.text);

    }

    public void DropGameObject()
    {
        if (item.isDragging)
        {
            target = item.dragedItem.GetTarget();
            targetText.text = target.name;

            _transRotate.target = target;
            _transRotate.targetName = target.name;

            _transRotateInforma.targetName = target.name;

        }
    }
}
