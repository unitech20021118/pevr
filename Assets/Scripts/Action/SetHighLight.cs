using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetHighLight : Action<Main>
{
    /// <summary>
    /// 开关
    /// </summary>
    public bool swich;
    /// <summary>
    /// 持续时间
    /// </summary>
    public float duration;
    /// <summary>
    /// 是否闪烁
    /// </summary>
    public bool isTwinkle;
    /// <summary>
    /// 目标物体
    /// </summary>
    public GameObject target;
    public string targetPath;
    /// <summary>
    /// 高光描边的颜色
    /// </summary>
    public Color color;
    public override void DoAction(Main m)
    {
        if (target == null)
        {
            if (string.IsNullOrEmpty(targetPath))
            {
                target = m.gameObject;
            }
            else
            {
                target = GameObject.Find("Parent/" + targetPath);
            }
        }
        //开关开启
        if (swich)
        {
            if (target.gameObject.GetComponent<HighLightStart>() == null)
            {   //给物体添加脚本
                target.gameObject.AddComponent<HighLightStart>();
            }
            HighLightStart hls = target.gameObject.GetComponent<HighLightStart>();
            //开始执行高光描边效果
            hls.StartHighLight(duration, isTwinkle, color);
        }//开关关闭
        else
        {
            if (target.gameObject.GetComponent<HighLightStart>() != null)
            {
                HighLightStart hls = target.gameObject.GetComponent<HighLightStart>();
                hls.StopHighLight();
            }
        }
        
    }
}
