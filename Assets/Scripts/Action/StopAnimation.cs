using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopAnimation : Action<Main>
{
    private Animator animator;
    private Animation animation;
    public override void DoAction(Main m)
    {
        animator = m.gameObject.GetComponent<Animator>();
        animation = m.gameObject.GetComponent<Animation>();
        if (animator!=null)
        {
            animator.speed = 0;
        }
        if (animation!=null)
        {
            animation.Stop();
        }
    }

}
