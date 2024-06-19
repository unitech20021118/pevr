using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControl : MonoBehaviour
{

    /// <summary>
    /// animation动画组件
    /// </summary>
    private Animation ani;
    /// <summary>
    /// 剪辑动画
    /// </summary>
    private AnimationClip cp;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// 开始循环播放动画
    /// </summary>
    public void StartLoopAnimation(Animation animation)
    {
        ani = animation;
        cp = ani.clip;
        StartCoroutine(DoLoop());
    }

    /// <summary>
    /// 循环播放动画的协成
    /// </summary>
    /// <param name="ani"></param>
    /// <returns></returns>
    IEnumerator DoLoop()
    {
        
        while (true)
        {
            if (!ani.isPlaying)
            {
                ani.Play(cp.name);
            }
            yield return null;

        }
    }
}
