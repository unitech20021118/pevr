using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PCShowMsgInforma : ActionInforma {
	public string msg;
	public int fontStyle;
	public int fontSize;
	public int font;
	public float spacing;
	public bool clickOpen;
	public string imagePath;
	public float px,py,pz,w,h;
    public bool IsCover;

    public bool IsShowLine;  //Edit by pxf
    public float LineDistance; //Edit by pxf

    public bool CanDrag;
    /// <summary>
    /// 字体颜色
    /// </summary>
    public string color;
    /// <summary>
    /// 隐藏按钮
    /// </summary>
    public bool hideButton;
//	public float rotateAngle, offsetX,offsetY,offsetZ;

	public PCShowMsgInforma(bool isOnce)
	{
		this.isOnce = isOnce;
	}
}
