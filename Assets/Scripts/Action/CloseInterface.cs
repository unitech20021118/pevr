using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseInterface : Action<Main>
{
    /// <summary>
    /// 按钮的父物体
    /// </summary>
    public Transform BtnParentTransform;
    /// <summary>
    /// 是否关闭按钮
    /// </summary>
    public bool Btn;
    /// <summary>
    /// 图片的父物体
    /// </summary>
    public Transform ImgParentTransform;
    /// <summary>
    /// 是否关闭图片
    /// </summary>
    public bool Img;
    /// <summary>
    /// 文字的父物体
    /// </summary>
    public Transform MsgParentTransform;
    /// <summary>
    /// 是否关闭文字
    /// </summary>
    public bool Msg;
    public override void DoAction(Main m)
    {
        if (Btn)
        {
            foreach (Transform chi in BtnParentTransform)
            {
                chi.gameObject.SetActive(false);
            }
        }
        if (Img)
        {
            foreach (Transform chi in ImgParentTransform)
            {
                chi.gameObject.SetActive(false);
            }
        }
        if (Msg)
        {
            foreach (Transform chi in MsgParentTransform)
            {
                chi.gameObject.SetActive(false);
            }
        }
        

    }
}
