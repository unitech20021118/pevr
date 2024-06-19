using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ShowPCButtonInforma : ActionInforma{

    public float left;
    public float up;
    public float width;
    public float height;
    public string imagePath;
    /// <summary>
    /// 按钮的文本
    /// </summary>
    public string buttonText;

    public ShowPCButtonInforma(bool isOnce)
    {
        left = 0.1f;
        up = 0.1f;
        width = 0.1f;
        height = 0.1f;
        this.isOnce = isOnce;
    }
}
