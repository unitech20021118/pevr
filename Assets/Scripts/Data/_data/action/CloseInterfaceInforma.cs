using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// created by kuai
/// 关闭按钮动作的数据类
/// </summary>
[System.Serializable]
public class CloseInterfaceInforma : ActionInforma
{
    /// <summary>
    /// 是否关闭按钮
    /// </summary>
    public bool Btn;
    /// <summary>
    /// 是否关闭图片
    /// </summary>
    public bool Img;
    /// <summary>
    /// 是否关闭文字
    /// </summary>
    public bool Msg;

    public CloseInterfaceInforma(bool isOnce)
    {
        this.isOnce = isOnce;
    }
}
