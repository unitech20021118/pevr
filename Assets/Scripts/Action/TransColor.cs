using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransColor : Action<Main>
{

   
   
    /// <summary>
    /// 目标名称
    /// </summary>
    public string targetName;
    /// <summary>
    /// 目标物体
    /// </summary>
    public GameObject target;

    /// <summary>
    /// 改变颜色
    /// </summary>
    public string color;

    public override void DoAction(Main m)
    {
        if (target == null)
        {
            if (string.IsNullOrEmpty(targetName))
            {
                target = m.gameObject;
            }
            else
            {
                target = GameObject.Find("Parent/" + targetName);
            }
        }

        switch (color)
        {
            case "红色" : Change_Material(target,Color.red); break;
            case "蓝色" : Change_Material(target, Color.blue); break;
            case "绿色" : Change_Material(target, Color.green); break;
            case "黄色" : Change_Material(target, Color.yellow); break;
        }
       

    }

    private void Change_Material(GameObject moden, Color color)
    {

        if (moden.GetComponent<Renderer>())
        {
            moden.GetComponent<Renderer>().material.color = color;
        }
        for (int i = 0; i < moden.transform.childCount; i++)
        {
            if (moden.transform.GetChild(i).GetComponent<Renderer>())
            {
                moden.transform.GetChild(i).GetComponent<Renderer>().material.color = color;

            }
            if (moden.transform.GetChild(i).childCount > 0)
            {
                Change_Material(moden.transform.GetChild(i).gameObject, color);
            }

        }
    }

}
