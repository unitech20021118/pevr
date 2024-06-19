using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PlayAnimationInforma : ActionInforma {
    //public string targetName;
    public string animationName;
    public bool isloop;
    public float speed;
    public bool isAnimation;
    //edited by kuai
    //public int numbers;
    public PlayAnimationInforma(bool isTrue)
    {
        this.isOnce = isTrue;
    }
	
}
