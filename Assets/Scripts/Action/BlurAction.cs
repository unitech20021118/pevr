using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlurAction : Action<Main>
{
    /// <summary>
    /// 蜡烛名
    /// </summary>
    public string CandleName;
    /// <summary>
    /// 蜡烛
    /// </summary>
    private Transform _candleTransform;
    /// <summary>
    /// 凸透镜名
    /// </summary>
    public string ConvexLensName;
    /// <summary>
    /// 凸透镜
    /// </summary>
    private Transform _convexLensTransform;
    /// <summary>
    /// 光屏名
    /// </summary>
    public string OpticalScreenName;
    /// <summary>
    /// 光屏
    /// </summary>
    private Transform _opticalScreenTransform;
    /// <summary>
    /// 凸透镜的焦距
    /// </summary>
    public float FocalLength;
    /// <summary>
    /// 模糊变化程度
    /// </summary>
    public float BlurLevel;
    /// <summary>
    /// 大小比例
    /// </summary>
    public float Proportion;

    public override void DoAction(Main m)
    {
        if (_candleTransform == null && !string.IsNullOrEmpty(CandleName))
        {
            _candleTransform = Manager.Instace.parent.Find(CandleName);
            //Debug.LogError(CandleTransform.position);
        }
        if (_convexLensTransform == null && !string.IsNullOrEmpty(ConvexLensName))
        {
            _convexLensTransform = Manager.Instace.parent.Find(ConvexLensName);
            //Debug.LogError(ConvexLensTransform.position);
        }
        if (_opticalScreenTransform == null && !string.IsNullOrEmpty(OpticalScreenName))
        {
            _opticalScreenTransform = Manager.Instace.parent.Find(OpticalScreenName);
            //Debug.LogError(OpticalScreenTransform.position);
        }

        BlurController bc = m.gameObject.GetComponent<BlurController>();
        if (bc==null)
        {
            bc = m.gameObject.AddComponent<BlurController>();
        }
        bc.StartRendering(_candleTransform,_convexLensTransform,_opticalScreenTransform,FocalLength, BlurLevel,Proportion);
    }
}
