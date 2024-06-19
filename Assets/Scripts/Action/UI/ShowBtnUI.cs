using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowBtnUI : ActionUI
{
    public GameObject uiPre;
    public GameObject currentUI;
    public InputField w, h, mx, my, mz;
    public string w1 = "50", h1 = "20", mx1 = "0", my1 = "0", mz1 = "0";


    //edit by 王梓亦
    ShowBtn sb;
    ShowBtnInforma sbi;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// 设置宽度
    /// </summary>
    /// <param name="W"></param>
    public void SetW(InputField W)
    {
        if (float.Parse(W.text) <= 0)
        {
            W.text = w1;
        }
        else
        {
            sb.w = float.Parse(W.text);
            sbi.w = float.Parse(W.text);
            w1 = W.text;
        }
    }
    /// <summary>
    /// 设置长度
    /// </summary>
    /// <param name="H"></param>
    public void SetH(InputField H)
    {
        if (float.Parse(H.text) <= 0)
        {
            H.text = w1;
        }
        else
        {
            sb.h = float.Parse(H.text);
            sbi.h = float.Parse(H.text);
            h1 = H.text;
        }
    }
    
    /// <summary>
    /// 设置位置x
    /// </summary>
    /// <param name="x"></param>
    public void Setx(InputField x)
    {
        if (float.Parse(x.text) <= 0)
        {
            x.text = w1;
        }
        else
        {
            sb.x = float.Parse(x.text);
            sbi.mx = float.Parse(x.text);
            mx1 = x.text;
        }
    }
    /// <summary>
    /// 设置位置y
    /// </summary>
    /// <param name="y"></param>
    public void Sety(InputField y)
    {
        if (float.Parse(y.text) <= 0)
        {
            y.text = w1;
        }
        else
        {
            sb.y = float.Parse(y.text);
            sbi.my = float.Parse(y.text);
            my1 = y.text;
        }
    }
    /// <summary>
    /// 设置位置z
    /// </summary>
    /// <param name="z"></param>
    public void Setz(InputField z)
    {
        if (float.Parse(z.text) <= 0)
        {
            z.text = w1;
        }
        else
        {
            sb.z = float.Parse(z.text);
            sbi.mz = float.Parse(z.text);
            mz1 = z.text;
        }
    }

    public override Action<Main> CreateAction()
    {
        action = new ShowBtn();
		actionInforma = new ShowBtnInforma(false);
        //edit by 王梓亦
        sb = (ShowBtn)action;
        sbi = (ShowBtnInforma)actionInforma;
        print(sbi);
        if (sbi != null)
        {
            sb.w = float.Parse(w.text);
            sb.h = float.Parse(h.text);
            sb.x = float.Parse(mx.text);
            sb.y = float.Parse(my.text);
            sb.z = float.Parse(mz.text);
            sbi.w = float.Parse(w.text);
            sbi.h = float.Parse(h.text);
            sbi.mx = float.Parse(mx.text);
            sbi.my = float.Parse(my.text);
            sbi.mz = float.Parse(mz.text);
            print("yes" + w.text);
        }

        print("no" + w.text);
        GetStateInfo().actionList.Add(actionInforma);
        actionInforma.name = "ShowBtn";

        ShowBtn showbtn = (ShowBtn)action;
		ShowBtn.globalId += 1;
		showbtn.id=ShowBtn.globalId;
		GameObject go= Instantiate<GameObject> (BtnInfo.template, BtnInfo.template.transform.parent);
        print(go.name + "123456789" + BtnInfo.template.transform.parent.name);
		go.GetComponent<BtnInfo> ().SetInfo (ShowBtn.globalId, ShowBtn.globalId.ToString ());
		go.GetComponent<BtnInfo> ().showBtnUI = this;
		go.SetActive (true);
        currentUI = Instantiate<GameObject>(uiPre);
        showbtn.currentUI = currentUI;
        return base.CreateAction();
    }

    public override Action<Main> LoadAction(ActionInforma actionInforma)
    {
        ShowBtnInforma showbtnInforma = (ShowBtnInforma)actionInforma;
        //edit by 王梓亦
        this.actionInforma = actionInforma;
        action = new ShowBtn();
        ShowBtn showbtn = (ShowBtn)action;
//        this.actionInforma = actionInforma;
		GameObject go= Instantiate<GameObject> (BtnInfo.template, BtnInfo.template.transform.parent);
        print(go.name + "~~~~~~~~~" + BtnInfo.template.transform.parent.name);
		go.GetComponent<BtnInfo> ().SetInfo (showbtnInforma.targetID, showbtnInforma.targetID.ToString ());
		go.GetComponent<BtnInfo> ().showBtnUI = this;
		go.SetActive (true);

        currentUI = Instantiate<GameObject>(uiPre);
        showbtn.currentUI = currentUI;
        //设置UI界面属性
        print(w.text + "~~~" + h.text);
        w.text = showbtnInforma.w.ToString();
        h.text = showbtnInforma.h.ToString();
        mx.text = showbtnInforma.mx.ToString();
        my.text = showbtnInforma.my.ToString();
        mz.text = showbtnInforma.mz.ToString();
        print(w.text + ":::" + h.text);
        //UpdateInput();
		showbtn.tarid = showbtnInforma.targetID;

        //edit by 王梓亦
        showbtn.w = showbtnInforma.w;
        showbtn.h = showbtnInforma.h;
        showbtn.x = showbtnInforma.mx;
        showbtn.y = showbtnInforma.my;
        showbtn.z = showbtnInforma.mz;
        return action;
    }

	public void SaveTargetID(int targetID){
		ShowBtnInforma showbtnInforma = (ShowBtnInforma)actionInforma;
		showbtnInforma.targetID = targetID;
	}

    public void UpdateInput()
    {
        ShowBtn showbtn = (ShowBtn)action;
        showbtn.w = float.Parse(w.text);
        showbtn.h = float.Parse(h.text);
        showbtn.x = float.Parse(mx.text);
        showbtn.y = float.Parse(my.text);
        showbtn.z = float.Parse(mz.text);
        //存储属性代码
        try
        {
            ShowBtnInforma showbtnInforma = (ShowBtnInforma)actionInforma;
            showbtnInforma.w = float.Parse(w.text);
            showbtnInforma.h = float.Parse(h.text);
            showbtnInforma.mx = float.Parse(mx.text);
            showbtnInforma.my = float.Parse(my.text);
            showbtnInforma.mz = float.Parse(mz.text);
        }
        catch { }
    }
}
