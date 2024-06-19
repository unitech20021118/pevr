using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G_ObjectMouseListener : MonoBehaviour {
    public delegate void ObjectMouseEventDelegate(GameObject eventObject);
    
    public ObjectMouseEventDelegate onClickDown;
    public ObjectMouseEventDelegate onClickUp;
    public ObjectMouseEventDelegate onEnter;
    public ObjectMouseEventDelegate onExit;
    private bool downHit;
    private bool mouseOn;

	// Use this for initialization
	void Start () {
        
        G_MouseListener instance = G_MouseListener.GetInstance();
        instance.AddLMouseDown(onLButtonDown);
        instance.AddLMouseUp(onLButtonUp);
	}
    /// <summary>
    /// 对准旋转坐标轴按下鼠标，触发点击事件（轴变黑色）
    /// </summary>
    private void onLButtonDown()
    {
        GameObject x;
        if (G_RayCastCheck.MouseCheckGameObject(out x, G_PubDef.layerMask) && x == base.gameObject)
        {
            Debug.Log(gameObject.name);
            this.downHit = true;
            if (this.onClickDown != null)
            {
                this.onClickDown(base.gameObject);
            }
        }
    }

    /// <summary>
    /// 左键抬起，触发事件
    /// </summary>
    private void onLButtonUp()
    {
        if (this.downHit && this.onClickUp != null)
        {
            this.onClickUp(base.gameObject);
        }
        this.downHit = false;
    }


	/// <summary>
	/// 时刻检测鼠标有没有在坐标轴上
	/// </summary>
	void Update () {
        GameObject x;
        if (G_RayCastCheck.MouseCheckGameObject(out x,G_PubDef.layerMask) && x == gameObject)
        {
            if (!mouseOn && onEnter != null)
            {
                onEnter(gameObject);
            }
            mouseOn = true;
        }
        else
        {
            if (!downHit && mouseOn && onExit != null)
            {
                onExit(gameObject);
            }
            mouseOn = false;
        }
	}

    private void OnDestroy()
    {
        G_MouseListener instance = G_MouseListener.GetInstance();
        //instance.DeleteLMouseUp(onLButtonUp);
        //instance.DeleteMouseDown(onLButtonDown);
    }
}
