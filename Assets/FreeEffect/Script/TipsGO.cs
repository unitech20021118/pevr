using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipsGO : MonoBehaviour {
    public GameObject tips;
    public GameObject child;
    List<string> _explain = new List<string>();
    List<string> tipsExplain=new List<string>();
	// Use this for initialization
	void Awake () {
        Getmessage();
        GetTipsmessage();
        TipsOK();
	}
    void Start()
    {
        child.SetActive(false);
    }
    public void ToHelp()
    {
        //bool ONoff = child.activeInHierarchy;
        child.SetActive(true);
        Manager.Instace.WaitingClose.Enqueue(child);
    }
    void TipsOK()
    {
        int num = tipsExplain.Count/3;
        for (int i = 0; i < num; i++)
        {
            SetTips(tipsExplain[3 * i], tipsExplain[3 * i + 1], tipsExplain[3 * i + 2]);
        }
    }
    void SetTips(string A,string B,string C)
    {
        GameObject _tips = Instantiate(tips,child.transform);
        _tips.transform.localScale = Vector3.one;
        _tips.transform.GetChild(0).GetComponent<Text>().text = A;
        _tips.transform.GetChild(1).GetComponent<Text>().text = B;
        _tips.transform.GetChild(2).GetComponentInChildren<Text>().text = C;
    }
    void Getmessage()
    {
        _explain.Clear();
        _explain.Add("Z*复位*状态机操作面板复位");
        _explain.Add("F*复位*选中物体时视口模型复位");
        _explain.Add("Tab*唤出鼠标*在运行状态下，唤出鼠标，用于点击操作");
        _explain.Add("Delete*删除*模型|事件|状态 的删除");

        _explain.Add("滚轮+-*视口缩放*各操作视口的视野缩放");
        _explain.Add("中键拖拉*视口平移*对视口进行视口平移");
        _explain.Add("中键*关闭*对部分右键激活的菜单面板进行关闭");

        _explain.Add("Alt+左键*视野旋转*用鼠标拖拉进行主视口的视野旋转");
        _explain.Add("选中+Alt+左键*视野旋转*用鼠标拖拉进行主视口以所选物体为中心进行视野旋转");

        _explain.Add("左键*点击*选中| 激活按钮对应面板");
        _explain.Add("右键*菜单栏*部分操作视口可以通过右键激活对应菜单面板");
        _explain.Add("右键*视野旋转*主视口右键拖拉进行视野旋转");
        _explain.Add("左键*变量赋值*变量操作视口，对具体变量进行赋值");
        _explain.Add("右键*变量退出赋值*变量操作视口，对具体变量进行赋值的退出");



    }
    void GetTipsmessage()
    {
        tipsExplain.Clear();
        for (int i = 0; i < _explain.Count; i++)
        {
           string[]str=_explain[i].Split('*');
           tipsExplain.Add(str[0]);
           tipsExplain.Add(str[1]);
           if (str.Length == 3)
               tipsExplain.Add(str[2]);
           if (str.Length != 3)
               tipsExplain.Add("暂无描述");
        }
    }
	// Update is called once per frame
	void Update () {
		
	}
}
