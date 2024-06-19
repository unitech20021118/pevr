using System;
using System.Collections;
using System.Collections.Generic;
using HighlightingSystem;
using UnityEngine;

/// <summary>
/// 开始高光效果的脚本  created by kuai
/// </summary>
public class HighLightStart : MonoBehaviour
{
    /// <summary>
    /// 持续时间
    /// </summary>
    private float duration;
    /// <summary>
    /// 闪烁
    /// </summary>
    private bool isTwinkle;
    /// <summary>
    /// 闪烁间隔时间
    /// </summary>
    private float interval=0.02f;
    /// <summary>
    /// 高光管理脚本
    /// </summary>
    private HighlighterController hc;
    /// <summary>
    /// 协成
    /// </summary>
    private Coroutine coroutine;
   
    /// <summary>
    /// 开始高光效果
    /// </summary>
    public void StartHighLight(float duration,bool isTwinkle,Color color)
    {
        //赋值
        this.duration = duration;
        this.isTwinkle = isTwinkle;
        //添加高光的控制脚本
        if (gameObject.GetComponent<HighlighterController>()==null)
        {
            gameObject.AddComponent<FlashingController>();
            //每次新添加高光描边效果脚本时将vr相机上的渲染脚本重新添加一下
            if (Manager.Instace.cameraEye.GetComponent<HighlightingRenderer>())
            {
                Destroy(Manager.Instace.cameraEye.GetComponent<HighlightingRenderer>());
            }
        }
        hc= gameObject.GetComponent<HighlighterController>();
        hc._selectColor = color;
        
        //开启协成
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(DoControl());
    }

    /// <summary>
    /// 关闭高光描边效果
    /// </summary>
    public void StopHighLight()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        if (gameObject.GetComponent<HighlighterController>())
        {
            Destroy(gameObject.GetComponent<FlashingController>());
            Destroy(gameObject.GetComponent<HighlighterController>());
        }
        
    }

    /// <summary>
    /// 控制高光描边持续显示或闪烁的效果
    /// </summary>
    /// <returns></returns>
    IEnumerator DoControl()
    {
        yield return null;
        //等待一帧给vr相机添加渲染高光描边的脚本
        if (Manager.Instace.cameraEye.GetComponent<HighlightingRenderer>()==null)
        {
            Manager.Instace.cameraEye.AddComponent<HighlightingRenderer>();
        }
        
        //当不需要闪烁效果时
        if (!isTwinkle)
        {
            //当有持续时间时
            if (duration > 0)
            {
                float t = 0;
                while (t <= duration)
                {
                    t += Time.deltaTime;
                    hc.MouseOver();
                    yield return null;
                }
            }//当持续时间为0时为永久
            else if(duration==0)
            {
                while (true)
                {
                    hc.MouseOver();
                    yield return null;
                }
            }
        }//要闪烁效果时
        else
        {
            //持续时间大于0
            if (duration>0)
            {
                float t = 0f;
                float t1 = 0f;
                bool show = true;
                while (t <= duration)
                {
                    t += Time.deltaTime;
                   
                    if (hc._selectColor.a>=1)
                    {
                        show = false;
                        
                    }
                    else if (hc._selectColor.a<=0)
                    {
                        show = true;
                        
                    }
                    if (show)
                    {
                        hc._selectColor.a += interval;
                    }
                    else
                    {
                        hc._selectColor.a -= interval;
                    }
                    hc.MouseOver();
                    //t1 += Time.deltaTime;
                    //if (t1 >= interval)
                    //{
                    //    if (show)
                    //    {
                    //        show = false;
                    //    }
                    //    else
                    //    {
                    //        show = true;
                    //    }
                    //    t1 = 0f;
                    //}
                    //if (show)
                    //{
                    //    hc.MouseOver();
                    //}
                    yield return null;
                }
            }//持续时间为0即永久
            else if(duration==0)
            {
                float t1 = 0f;
                bool show = true;
                while (true)
                {
                    if (hc._selectColor.a >= 1)
                    {
                        show = false;

                    }
                    else if (hc._selectColor.a <= 0)
                    {
                        show = true;

                    }
                    if (show)
                    {
                        hc._selectColor.a += interval;
                    }
                    else
                    {
                        hc._selectColor.a -= interval;
                    }
                    hc.MouseOver();
                    //t1 += Time.deltaTime;
                    //if (t1>=interval)
                    //{
                    //    if (show)
                    //    {
                    //        show = false;
                    //    }
                    //    else
                    //    {
                    //        show = true;
                    //    }
                    //    t1 = 0f;
                    //}
                    //if (show)
                    //{
                    //    hc.MouseOver();
                    //}
                    yield return null;
                }
            }
        }
    }

}
