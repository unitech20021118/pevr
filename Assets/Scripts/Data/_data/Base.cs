using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[System.Serializable]
public class Base{

    public static int id;
    public int index;
    public int projectId;
    public string name;

    public string pos;
    public string rotate;
    public string scale;
    public bool isActive;
    public List<string> customEvent = new List<string>();//事件
    public List<variableData> customVariable = new List<variableData>();//变量
    /// <summary>
    /// 子物体名字及该物体上的介绍
    /// </summary>
    public List<ChildAndIntroduce> ChildIntroduce = new List<ChildAndIntroduce>();
    /// <summary>
    /// 父物体的名字
    /// </summary>
    public string parentName;
    
    //public static List<Base> allData = new List<Base>();
    public Base()
    {
        

    }

    public Base(int i,string name)
    {
        this.projectId = i;
        this.name = name;
        id++;
        index = id;
        
    }

    public static Base FindData(string name)
    {
        foreach (Base b in Informa<Base>.allData)
        {
            if (b.name.Equals(name))
            {
                return b;
            }
        }
        return null;
    }

    //public static virtual T FindData(string name)
    //{

    //    return default(T);
    //}

    public static Base FindData(int num)
    {
        foreach (Base i in Informa<Base>.allData)
        {
            if (i.index == num)
            {
                return i;
            }
        }
        return null;
    }

    //public static virtual T FindData(int num)
    //{
        
    //    return default(T);
    //}

    public virtual void GetTransform()
    {
        Transform t = Manager.Instace.parent.FindChild(name);
        if (t==null)
        {
            GameObject[] all = GameObject.FindGameObjectsWithTag("Editor");
            for (int i = 0; i < all.Length; i++)
            {
                if (all[i].name==name)
                {
                    t = all[i].transform;
                }
            }
        }
        
       
        pos = t.position.ToString("G");
        rotate = t.eulerAngles.ToString("G");
        scale = t.localScale.ToString("G");
        parentName = t.parent.name;
        Transform[] child = t.GetComponentsInChildren<Transform>();
        for (int i = 1; i < child.Length; i++)
        {
            if (child[i].GetComponent<Introduce>()!=null)
            {
                //ChildAndIntroduce cai = new ChildAndIntroduce(child[i].name, child[i].GetComponent<Introduce>().introduce,child[i].GetComponent<Introduce>().imagePath, child[i].GetComponent<Introduce>().videoPath);
                ChildAndIntroduce cai = new ChildAndIntroduce(PICamera.Instance.GetChildFullName(child[i]), child[i].GetComponent<Introduce>().introduce,child[i].GetComponent<Introduce>().imagePath, child[i].GetComponent<Introduce>().videoPath);
                ChildIntroduce.Add(cai);
            }
        }
    }
   

    public void GetCameraTransform()
    {
        Transform t = Camera.main.transform;
        pos = t.position.ToString();
        rotate = t.eulerAngles.ToString();
        scale = t.localScale.ToString("G25");
    }
}
