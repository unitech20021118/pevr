using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleControl : Action<Main>
{
    public GameObject target;
    /// <summary>
    /// 选择的操作
    /// </summary>
    public int _Etype;
    //public float _Dtime=0;
    public string targetPath;

    private FreeEffectMainC freeEffectMainC;
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
        freeEffectMainC = target.GetComponent<FreeEffectMainC>();
        if (freeEffectMainC == null)
        {
            freeEffectMainC = target.AddComponent<FreeEffectMainC>();
        }

        if (freeEffectMainC != null)
        {
            freeEffectMainC.GetParticleSystem();
            switch (_Etype)
            {
                case 0:
                    freeEffectMainC.OpenEffect();//开始播放
                    break;
                case 1:
                    freeEffectMainC.PauseEffect();//暂停播放
                    break;
                case 2:
                    freeEffectMainC.StopEffect();//停止播放
                    break;
                case 3:
                    freeEffectMainC.ContinueEffect();//继续播放
                    break;
                case 4:
                    freeEffectMainC.gogoEffect();//循环播放
                    //isOder = true;
                    break;
                case 5:
                    freeEffectMainC.DeleteWffect();//删除效果
                    break;
                case 6:
                    freeEffectMainC.ShowEffect();//显示效果
                    break;
            }
            //target.GetComponent<FreeEffectMainC>().StopEffect();
            //target.GetComponent<FreeEffectMainC>().Etype = _Etype;
        }

        
    }
    public ParticleControl()
    {
        SetSituation();
    }


    public ParticleControl(int type)
    {
        _Etype = type;
    }
}

    
