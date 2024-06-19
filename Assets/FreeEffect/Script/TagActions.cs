using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TagActions : MonoBehaviour {
    public Transform totalAC;
    public Text typeName;
    public int lastNum=0;
    List<Image> acImgs=new List<Image>();

    List<string> As = new List<string>();
    List<string> Bs = new List<string>(); 
    List<string> Cs = new List<string>();
    List<string> Ds = new List<string>(); 
    List<string> Es = new List<string>();

    List<GameObject> Ag = new List<GameObject>();
    List<GameObject> Bg = new List<GameObject>();
    List<GameObject> Cg = new List<GameObject>();
    List<GameObject> Dg = new List<GameObject>();
    List<GameObject> Eg = new List<GameObject>();
	// Use this for initialization
    void Start()
    {
        initialStr();//初始化标签值
        initialACgam();//初始化标签对应物体

        initialACTag();//初始化标签UI图标 
        Actype(lastNum);
    }

    void Update()
    {
    }
    public void Actype(int num)
    {
        switch (num)
        {
            case 0:
                ACtORf(Ag, true);
                ACtORf(Bg, false);
                ACtORf(Cg, false);
                ACtORf(Dg, false);
                ACtORf(Eg, false);
                typeName.text = "角色动作";
                ACImg(acImgs, 0);
                break;
            case 1:
                ACtORf(Ag, false);
                ACtORf(Bg, true);
                ACtORf(Cg, false);
                ACtORf(Dg, false);
                ACtORf(Eg, false);
                typeName.text = "逻辑动作";
                ACImg(acImgs, 1);
                break;
            case 2:
                ACtORf(Ag, false);
                ACtORf(Bg, false);
                ACtORf(Cg, true);
                ACtORf(Dg, false);
                ACtORf(Eg, false);
                typeName.text = "VR动作";
                ACImg(acImgs, 2);
                break;
            case 3:
                ACtORf(Ag, false);
                ACtORf(Bg, false);
                ACtORf(Cg, false);
                ACtORf(Dg, true);
                ACtORf(Eg, false);
                typeName.text = "平面动作";
                ACImg(acImgs, 3);
                break;
            case 4:
                ACtORf(Ag, false);
                ACtORf(Bg, false);
                ACtORf(Cg, false);
                ACtORf(Dg, false);
                ACtORf(Eg, true);
                typeName.text = "三维动作";
                ACImg(acImgs, 4);
                break;
            case 9:
                ACtORf(Ag, true);
                ACtORf(Bg, true);
                ACtORf(Cg, true);
                ACtORf(Dg, true);
                ACtORf(Eg, true);
                typeName.text = "混合动作";
                ACImg(acImgs, 9);
                break;
        }
    }
    public void initialStr()
    {
        As.Clear();
        foreach (var n in ActionMsgInit.As)
        {
            As.Add(n.Key);
        }
     
        Bs.Clear();
        foreach (var n in ActionMsgInit.Bs)
        {
            Bs.Add(n.Key);
        }
       
        Cs.Clear();
        foreach (var n in ActionMsgInit.Cs)
        {
            Cs.Add(n.Key);
        }
      
        Ds.Clear();
        foreach (var n in ActionMsgInit.Ds)
        {
            Ds.Add(n.Key);
        }
       

        Es.Clear();
        foreach (var n in ActionMsgInit.Es)
        {
            Es.Add(n.Key);
        }

       
    }
    public void initialACgam()
    {
        Ag.Clear();
        Bg.Clear();
        Cg.Clear();
        Dg.Clear();
        Eg.Clear();

        for (int i = 0; i < totalAC.childCount; i++)
        {
            GameObject cc = totalAC.GetChild(i).gameObject;
            if (As.Contains(cc.name))
            {
                Ag.Add(cc);
            }
            if (Bs.Contains(cc.name))
            {
                Bg.Add(cc);
            }
            if (Cs.Contains(cc.name))
            {
                Cg.Add(cc);
            }
            if (Ds.Contains(cc.name))
            {
                Dg.Add(cc);
            }
            if (Es.Contains(cc.name))
            {
                Eg.Add(cc);
            }
        }
    }

    public void initialACTag()
    {
        acImgs.Clear();
        for (int i = 0; i < transform.childCount - 1; i++)
        {
            acImgs.Add(transform.GetChild(i).GetComponent<Image>());
        }
    }
  
    void ACImg(List<Image> imgs,int num)
    {
        for (int i = 0; i < imgs.Count; i++)
        {
            if (i != num)
            {
                imgs[i].enabled = false;
            }
            if (i == num)
            {
                imgs[i].enabled = true;
            }
        }

        lastNum = num;
    }
    void ACtORf(List<GameObject> list,bool tf)
    {
            for (int i = 0; i < list.Count; i++)
            {
                list[i].SetActive(tf);
            }   
    }

	// Update is called once per frame

}
