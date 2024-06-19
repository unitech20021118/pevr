using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 控制物体移动的ui脚本     created by kuai
/// </summary>
public class TransMoveUI : ActionUI
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
    /// 移动时间文本输入框
    /// </summary>
    public InputField moveTime;
    /// <summary>
    /// 目标物体
    /// </summary>
    public GameObject target;
    /// <summary>
    /// 目标物体显示的文本框
    /// </summary>
    public Text targetText;

    private TransMove _transMove;
    private TransMoveInforma _transMoveInforma;


    //void Start()
    //{Debug.LogError("111");
    //    if (string.IsNullOrEmpty(targetName))
    //    {
    //        targetText.text = "拖拽物体至此框内";
    //    }
    //}
    public override Action<Main> CreateAction()
    {
        action = new TransMove();
        action.isOnce = true;
        _transMove = (TransMove)action;
        actionInforma = new TransMoveInforma(true);
        _transMoveInforma = (TransMoveInforma)actionInforma;

        //初始化相关变量
        _transMove.Target_V3 = Vector3.zero;

        GetStateInfo().actionList.Add(actionInforma);
        actionInforma.name = "TransMove";
        return base.CreateAction();
    }

    public override Action<Main> LoadAction(ActionInforma actionInforma)
    {
        _transMoveInforma = (TransMoveInforma)actionInforma;
        this.actionInforma = actionInforma;
        action = new TransMove();
        _transMove = (TransMove)action;

        //读取数据

        _transMove.Target_V3.x = _transMoveInforma.trans_X;
        _transMove.Target_V3.y = _transMoveInforma.trans_Y;
        _transMove.Target_V3.z = _transMoveInforma.trans_Z;

        _transMove.targetName = _transMoveInforma.targetName;
        _transMove.durationTime = _transMoveInforma.durationTime;
        _transMove.target = target;
        if (string.IsNullOrEmpty(_transMoveInforma.targetName))
        {
            targetText.text = "拖拽物体至此框内";
        }
        else
        {
            targetText.text = _transMoveInforma.targetName;
        }


        //设置UI界面属性
        trans_X.text = _transMoveInforma.trans_X.ToString();
        trans_Y.text = _transMoveInforma.trans_Y.ToString();
        trans_Z.text = _transMoveInforma.trans_Z.ToString();
        moveTime.text = _transMoveInforma.durationTime.ToString();
        this.actionInforma = actionInforma;
        return action;
    }
    /// <summary>
    /// x输入框的值改变时
    /// </summary>
    public void Changed_X()
    {
        _transMove.Target_V3.x = float.Parse(trans_X.text);
        _transMoveInforma.trans_X = float.Parse(trans_X.text);
    }
    /// <summary>
    /// y输入框的值改变时
    /// </summary>
    public void Changed_Y()
    {

        _transMove.Target_V3.y = float.Parse(trans_Y.text);
        _transMoveInforma.trans_Y = float.Parse(trans_Y.text);
    }
    /// <summary>
    /// z输入框的值改变时
    /// </summary>
    public void Changed_Z()
    {
        _transMove.Target_V3.z = float.Parse(trans_Z.text);
        _transMoveInforma.trans_Z = float.Parse(trans_Z.text);
    }
    /// <summary>
    /// 时间输入框的值改变时
    /// </summary>
    public void Changed_T()
    {
        float t = float.Parse(moveTime.text);
        //判断如果输入的时间的值小于0则修改为0
        if (t < 0)
        {
            t = 0;
            moveTime.text = "0";
        }
        _transMove.durationTime = t;
        _transMoveInforma.durationTime = t;

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


            _transMove.target = target;
            _transMove.targetName = target.name;
            _transMoveInforma.targetName = target.name;
        }
    }
}
