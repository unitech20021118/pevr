using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIClick : MonoBehaviour {
    public RectTransform roro;
    /// <summary>
    /// 原本的标号
    /// </summary>
    public int lastNum;
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if (this.transform.childCount < 5)
            Destroy(this.transform.gameObject);
	}
    public void _ChildUIClick()
    {
        if ((this.transform.childCount >= 5))
        {
            bool ONOFF;
            ONOFF = this.transform.GetChild(4).gameObject.activeInHierarchy;
            RPRO(ONOFF);
            this.transform.GetChild(4).gameObject.SetActive(!ONOFF);
        }
    }
    public void DesTROY()
    {
        this.transform.GetChild(4).gameObject.SetActive(true);
        ActionUI AcUI = this.transform.GetChild(4).GetComponent<ActionUI>();
        AcUI.Close();
    }
    void RPRO(bool tt)
    {        
        if (!tt)
            roro.DORotate(new Vector3(0, 0, -90), 0.2f);
        if (tt)
            roro.DORotate(new Vector3(0, 0, 0), 0.2f);
    }
    /// <summary>
    /// 换位
    /// </summary>
    public void TransPosition()
    {
        Debug.Log("!!!!!!!!!!!");
        string num = transform.GetChild(3).GetComponent<InputField>().text;
        int b=1;
       
        b = int.Parse(num);
        Debug.Log(b+"@@@@@@@"); 
        transform.parent.parent.parent.GetComponent<Elastic2nd>().SortByNum(this.transform,lastNum, b);
        
    }
    /// <summary>
    /// 更换原本标号
    /// </summary>
    /// <param name="a"></param>
    public void ChangeLastNum(int a)
    {
        lastNum = a;
    }
    /// <summary>
    /// 换位错误
    /// </summary>
    public void TransPositionError()
    {
        transform.GetChild(3).GetComponent<InputField>().text = lastNum.ToString();
    }
}

