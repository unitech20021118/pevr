using HighlightingSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PIHighlightController : MonoBehaviour {
    /// <summary>
    /// 高光管理脚本
    /// </summary>
    private HighlighterController hc;
    /// <summary>
    /// 选中的携程
    /// </summary>
    private Coroutine selectCoroutine;
    /// <summary>
    /// 悬浮的携程
    /// </summary>
    private Coroutine SuspensionCoroutine;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartHighLight(bool a)
    {
        if (gameObject.GetComponent<HighlighterController>() == null)
        {
            gameObject.AddComponent<FlashingController>();
        }
        hc = gameObject.GetComponent<HighlighterController>();
        if (a)
        {
            //开启协成
            if (selectCoroutine != null)
            {
                StopCoroutine(selectCoroutine);
                selectCoroutine = null;
            }
            if (SuspensionCoroutine!=null)
            {
                StopCoroutine(SuspensionCoroutine);
                SuspensionCoroutine = null;
            }
            selectCoroutine = StartCoroutine(DoSelectHighLight());
        }
        else
        {
            if (selectCoroutine != null)
            {
                return;;
            }
            //开启协成
            if (SuspensionCoroutine != null)
            {
                StopCoroutine(SuspensionCoroutine);
                SuspensionCoroutine = null;
            }
            SuspensionCoroutine = StartCoroutine(DoSuspensionHighLight());
        }
        
    }

    public void StopHighLight(bool a)
    {
        if (a)
        {
            if (selectCoroutine != null)
            {
                StopCoroutine(selectCoroutine);
                selectCoroutine = null;
                ClearHighLight();

            }
        }
        else
        {
            if (SuspensionCoroutine != null)
            {
                StopCoroutine(SuspensionCoroutine);
                SuspensionCoroutine = null;
                ClearHighLight();
            }
        }
        
    }

    IEnumerator DoSelectHighLight()
    {
        hc._selectColor = Color.green;
        while (true)
        {
            hc.MouseOver(); ;
            yield return null;
        }
    }

    IEnumerator DoSuspensionHighLight()
    {
        hc._selectColor = Color.yellow;
        while (true)
        {
            hc.MouseOver(); ;
            yield return null;
        }
    }

    /// <summary>
    /// 清除高光组件
    /// </summary>
    public void ClearHighLight()
    {
        Destroy(gameObject.GetComponent<HighlighterController>());
        Destroy(gameObject.GetComponent<FlashingController>());
        Destroy(gameObject.GetComponent<Highlighter>());
        Destroy(this);
    }
}
