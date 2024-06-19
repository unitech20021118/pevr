using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransColorUI: ActionUI{

    /// <summary>
    /// 颜色栏
    /// </summary>
    public Dropdown drop;
    /// <summary>
    /// 目标物体
    /// </summary>
    public GameObject target;
    /// <summary>
    /// 目标物体显示的文本框
    /// </summary>
    public Text targetText;

    private TransColor _transColor;
    private TransColorInforma _transColorInforma;

    public override Action<Main> CreateAction()
    {
        action = new TransColor();
        action.isOnce = true;
        _transColor = (TransColor)action;
        actionInforma = new TransColorInforma(true);
        _transColorInforma = (TransColorInforma)actionInforma;
        _transColorInforma.color = drop.options[drop.value].text;
        _transColor.color = drop.options[drop.value].text;


        GetStateInfo().actionList.Add(actionInforma);
        actionInforma.name = "TransColor";
        return base.CreateAction();
    }

    public override Action<Main> LoadAction(ActionInforma actionInforma)
    {
        _transColorInforma = (TransColorInforma)actionInforma;
        this.actionInforma = actionInforma;
        action = new TransRotate();
        _transColor = (TransColor)action;


        //读取数据



        _transColor.targetName = _transColorInforma.targetName;
        _transColor.target = target;


        if (string.IsNullOrEmpty(_transColorInforma.targetName))
        {
            targetText.text = "拖拽物体至此框内";
        }
        else
        {
            targetText.text = _transColorInforma.targetName;
        }

        //设置UI界面属性
       
        this.actionInforma = actionInforma;
        return action;
    }

    public void Value_Change()
    {
        _transColorInforma.color = drop.options[drop.value].text;
        _transColor.color= drop.options[drop.value].text;
    }
}
