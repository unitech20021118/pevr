using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using UnityEngine.UI;

public class PCShowMsg : Action<Main>
{
    public string msg;
    //public float rotateAngle,offsetX,offsetY=0.2f,offsetZ=0.5f;
    public GameObject tipObj;
    public FontStyle style;
    public int fontSize;
    public Font font;
    public bool clickOpen;
    public float spacing;
    public string imagePath;
    public Vector3 showPos;
    public float w, h;
    UIController uiController;
    public bool IsCover;
    public bool CanDrag;
    /// <summary>
    /// 字体颜色
    /// </summary>
    public Color color;
    /// <summary>
    /// 隐藏按钮
    /// </summary>
    public bool hideButton;
    /// <summary>
    /// 在列表中的索引
    /// </summary>
    public int IndexInMessageAttributes = -1;
    public bool IsShowLine; //edit by pxf
    public float LineDistance; //edit by pxf
    public override void DoAction(Main m)
    {
        if (tipObj != null)
        {
            uiController = UIController.Instance;
            //先将所有可被覆盖的文本关闭
            for (int i = 0; i < uiController.MessageAttributes.Count; i++)
            {
                if (uiController.MessageAttributes[i].IsCover)
                {
                    uiController.MessageAttributes[i].MessageGameObject.SetActive(false);
                }
            }
            tipObj.transform.parent.gameObject.SetActive(true);
            tipObj.SetActive(true);
            if (hideButton)
            {
                tipObj.transform.FindChild("Button").gameObject.SetActive(false);
            }
            tipObj.GetComponent<RectTransform>().sizeDelta = new Vector2(w, h);
            tipObj.GetComponent<DragableUI>().CanDrag = CanDrag;

            //uiCtrller = m.gameObject.GetComponent<UICtrller>();
            //if (uiCtrller == null)
            //{
            //    uiCtrller = m.gameObject.AddComponent<UICtrller>();
            //}
            //uiCtrller.targetUI = tipObj;
            //if (!string.IsNullOrEmpty(msg))
            //{
            //    Debug.LogError(msg);
            //    uiCtrller.msg = msg;
            //    uiCtrller.font = font;
            //    uiCtrller.style = style;
            //    uiCtrller.fontSize = fontSize;
            //    uiCtrller.clickOpen = clickOpen;
            //    uiCtrller.OpenUIPanel(0);
            //    uiCtrller.spacing = spacing;
            //    uiCtrller.msgBgPath = imagePath;
            //    uiCtrller.msgPos = showPos;
            //}
            //			tipObj.transform.eulerAngles = new Vector3 (0, rotateAngle, 0);
            //			tipObj.transform.localPosition = new Vector3 (offsetX, offsetY, offsetZ);
            var targetUi = tipObj;
            if (IsShowLine)
            {
                //显示模型和UI的连线
                Manager.Instace.LoadPcShowMsgUIModel(m.gameObject, targetUi, LineDistance);
            }
            if (IndexInMessageAttributes>=0)
            {
                if (tipObj == uiController.MessageAttributes[IndexInMessageAttributes].MessageGameObject)
                {
                    return;
                }
            }
            MessageAttribute ma = new MessageAttribute(tipObj,msg,font,style,fontSize,imagePath,showPos,IsCover);
            uiController.MessageAttributes.Add(ma);
            IndexInMessageAttributes = uiController.MessageAttributes.Count - 1;
           
        }
    }
}
