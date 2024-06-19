using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 激光提示信息动作的UI
/// </summary>
public class ShowMsgUI : ActionUI {
	public Text msg;
	public InputField msgField;
    ShowMsgInforma psInforma;
    ShowMsg showMsg;
	public override Action<Main> CreateAction()
	{
		action = new ShowMsg ();
		actionInforma = new ShowMsgInforma(true);
		GetStateInfo().actionList.Add(actionInforma);
		actionInforma.name = "ShowMsg";
		return base.CreateAction();
	}

	public override Action<Main> LoadAction (ActionInforma actionInforma)
	{
		psInforma = (ShowMsgInforma)actionInforma;
		this.actionInforma = actionInforma;
		msg.text = psInforma.msg;
		msgField.text = psInforma.msg;
		action = new ShowMsg();
		showMsg = (ShowMsg)action;
		showMsg.msg = psInforma.msg;
        timeInputField.text = showMsg.duringTime.ToString();
		return action;
	}

    void Start()
    {
        timeInputField.onValueChanged.AddListener(delegate(string a) { ActionTimeChanged(); });
    }
    void ActionTimeChanged()
    {
        if (showMsg != null)
        {
            showMsg.duringTime = float.Parse(timeInputField.text);
            psInforma.durtime = showMsg.duringTime;
        }
    }

	public void UpdateInput(){
		showMsg = (ShowMsg)action;
		try{
			showMsg.msg = msg.text;
			psInforma = (ShowMsgInforma)actionInforma;
			//将属性值保存
			psInforma.msg=msg.text;
		}catch{
		}
	}
}
