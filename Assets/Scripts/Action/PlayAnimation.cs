using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PlayAnimation :Action<Main>{
    //public GameObject target;
    //public string targetName;
    public string animationName;
    /// <summary>
    /// 动画剪辑
    /// </summary>
    public AnimationClip clip;
    public float speed=1;
    /// <summary>
    /// 是否是animation模式的循环播放
    /// </summary>
    public bool isLoop;


    public override void  DoAction(Main m)
    {
        Animator animator = m.gameObject.GetComponent<Animator>();
        Animation animation = m.gameObject.GetComponent<Animation>();
        //edit by kuai
        //检测是否有animator动画组件
        if (animator)
        {
            animator.Play(animationName);
            //animator.SetTrigger(animationName);
            Debug.Log(animationName);
            animator.speed = speed;
            Debug.Log(speed);
        }//如果是animation动画组件
        else if (animation)
        {
            animation.enabled = true;
            AnimationClip cp = animation.clip;
            if (!isLoop)
            {
                animation.Stop();
                animation.Play(cp.name);
            }
            else
            {
                AnimationControl ac = m.gameObject.AddComponent<AnimationControl>();
                ac.StartLoopAnimation(animation);
            }
           
        }
    }

    public PlayAnimation(string name)
    {
        id = 5;
        animationName = name;
        isOnce = true;
    }

    public PlayAnimation()
    {
        id = 5;
        isOnce = true;
    }

   
}
