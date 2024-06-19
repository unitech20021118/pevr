using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 用于关闭所有显示出来的按钮的动作的UI
/// created by kuai
/// </summary>
public class CloseInterfaceUI : ActionUI
{
    private CloseInterface closeInterface;
    private CloseInterfaceInforma closeInterfaceInforma;
    /// <summary>
    /// 按钮
    /// </summary>
    public Toggle BtnToggle;
    /// <summary>
    /// 图片
    /// </summary>
    public Toggle ImgToggle;
    /// <summary>
    /// 文字
    /// </summary>
    public Toggle MsgToggle;

    public override Action<Main> CreateAction()
    {
        action = new CloseInterface();
        actionInforma = new CloseInterfaceInforma(true);
        closeInterface = (CloseInterface) action;
        GetStateInfo().actionList.Add(actionInforma);
        actionInforma.name = "CloseInterface";
        closeInterface.BtnParentTransform  = GameObject.Find("Canvas/PCBtnParent").transform;
        closeInterface.ImgParentTransform = GameObject.Find("Canvas/ImgContainer").transform;
        closeInterface.MsgParentTransform = GameObject.Find("Canvas/MsgContainer").transform;
        return base.CreateAction();
    }
    public override Action<Main> LoadAction(ActionInforma actionInforma)
    {
        closeInterfaceInforma = (CloseInterfaceInforma)actionInforma;
        this.actionInforma = actionInforma;
        action = new CloseInterface();
        closeInterface = (CloseInterface)action;
        closeInterface.BtnParentTransform = GameObject.Find("Canvas/PCBtnParent").transform;
        closeInterface.ImgParentTransform = GameObject.Find("Canvas/ImgContainer").transform;
        closeInterface.MsgParentTransform = GameObject.Find("Canvas/MsgContainer").transform;
        BtnToggle.isOn = closeInterfaceInforma.Btn;
        closeInterface.Btn = closeInterfaceInforma.Btn;
        ImgToggle.isOn = closeInterfaceInforma.Img;
        closeInterface.Img = closeInterfaceInforma.Img;
        MsgToggle.isOn = closeInterfaceInforma.Msg;
        closeInterface.Msg = closeInterfaceInforma.Msg;
        return base.LoadAction(actionInforma);
    }

    /// <summary>
    /// 值的修改
    /// </summary>
    /// <param name="a"></param>
    public void ChangeToggle(int a)
    {
        if (closeInterfaceInforma == null)
        {
            closeInterfaceInforma = (CloseInterfaceInforma) actionInforma;
        }
        if (closeInterface==null)
        {
            closeInterface = (CloseInterface) action;
        }
        switch (a)
        {
            case 1:
                closeInterface.Btn = BtnToggle.isOn;
                closeInterfaceInforma.Btn = BtnToggle.isOn;
                break;
            case 2:
                closeInterface.Img = ImgToggle.isOn;
                closeInterfaceInforma.Img = ImgToggle.isOn;
                break;
            case 3:
                closeInterface.Msg = MsgToggle.isOn;
                closeInterfaceInforma.Msg = MsgToggle.isOn;
                break;
                default:
                    break;
        }
    }
}
