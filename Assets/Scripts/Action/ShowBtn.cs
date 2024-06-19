using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowBtn : Action<Main>
{
    public GameObject currentUI;
    public float w, h, x, y, z;
	public int id, tarid;
	public static int globalId=0;

    public override void DoAction(Main m)
    {
        Debug.Log(w + ":" + h);
        RectTransform uiTrans = currentUI.GetComponent<RectTransform>();
        uiTrans.GetComponent<BoxCollider>().size = new Vector3(w, h, 5);
        uiTrans.sizeDelta = new Vector2(w, h);
        Vector3 rootPos = m.gameObject.transform.position;
        currentUI.transform.position = rootPos + new Vector3(x, y, z);
    }
}
