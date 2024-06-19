using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowImg : Action<Main> {
	public RectTransform canvas;
	public Sprite sprite;
	public float rotate=0,x=0,y=0,z=0,w=260,h=180;
	public string imgPath;
	public int ajustMode;
	public bool clickOpen;
	public Vector3 showPos;
	public float duringTime;
    public bool IsBG;
	//UICtrller uiCtrller;
    private UIController uiController;

    private int IndexInMessageAttributes = -1;
    private bool opened;

    public override void DoAction(Main m)
	{
		if (canvas != null) {
			canvas.parent.gameObject.SetActive(true);
		    uiController = UIController.Instance;
            
            //canvas.sizeDelta = new Vector2 (w, h);
            //uiCtrller.imgtargetUI = canvas.gameObject;
            //uiCtrller.imgPath = imgPath;
            //uiCtrller.ajustMode = ajustMode;
            //uiCtrller.clickOpen = clickOpen;
            //uiCtrller.OpenUIPanel(1);
            //uiCtrller.imgPos = showPos;
		    //         uiCtrller.IsBG = IsBG;
		    if (IndexInMessageAttributes >= 0)
		    {
		        if (canvas.gameObject == uiController.ImageAttributes[IndexInMessageAttributes].ImageGameObject)
		        {
		            opened = true;
		        }
		    }
		    if (!opened)
		    {
		        ImageAttribute ia = new ImageAttribute(canvas.gameObject, imgPath, ajustMode, showPos, IsBG);
		        uiController.ImageAttributes.Add(ia);
		        IndexInMessageAttributes = uiController.ImageAttributes.Count - 1;
            }
		    float imgSizeW = canvas.GetComponent<Image>().preferredWidth;
		    float imgSizeH = canvas.GetComponent<Image>().preferredHeight;
		    RectTransform ImageRectTransform = canvas.GetComponent<RectTransform>();
		    switch (ajustMode)
		    {
		        case 0:
		            ImageRectTransform.sizeDelta = new Vector2(w,h);
		            break;
		        case 1:
		            ImageRectTransform.sizeDelta = new Vector2(w, w * imgSizeH / imgSizeW);
		            break;
		        case 2:
		            ImageRectTransform.sizeDelta = new Vector2(h * imgSizeW / imgSizeH, h);
		            break;
		        default:
		            ImageRectTransform.sizeDelta = new Vector2(w, h);
		            break;
		    }
		    if (IsBG)
		    {
		        canvas.sizeDelta=new Vector2(800,450);
                canvas.localPosition=Vector3.zero;
		    }
            //显示图片
            canvas.gameObject.SetActive(true);
        }
	}
}
