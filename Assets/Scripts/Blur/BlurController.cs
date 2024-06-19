using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlurController : MonoBehaviour
{
    /// <summary>
    /// 模糊渲染的相机
    /// </summary>
    private Camera blurCamera;

    private RapidBlurEffect rapidBlurEffect;
    /// <summary>
    /// 蜡烛
    /// </summary>
    public Transform _candleTransform;
    /// <summary>
    /// 凸透镜
    /// </summary>
    public Transform _convexLensTransform;
    /// <summary>
    /// 光屏
    /// </summary>
    public Transform _opticalScreenTransform;
    /// <summary>
    /// 凸透镜的焦距
    /// </summary>
    public float focalLength;
    /// <summary>
    /// 按大小比例计算的焦距
    /// </summary>
    public float proportionFocalLength;
    private Coroutine coroutine;

    /// <summary>
    /// 物距
    /// </summary>
    public float objectDistance;
    /// <summary>
    /// 像距
    /// </summary>
    public float imageDistance;
    /// <summary>
    /// 清晰的相距
    /// </summary>
    public float clearImageDistance;
    /// <summary>
    /// 模糊度程度分级（0-20）
    /// </summary>
    public float blur;
    /// <summary>
    /// 模糊变化程度
    /// </summary>
    public float blurLevel;
    /// <summary>
    /// 大小比例
    /// </summary>
    public float proportion;
    private RenderTexture renderTexture;

    private Material material;
    // Use this for initialization
    void Start()
    {
        //StartRendering();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 开始渲染的入口
    /// </summary>
    public void StartRendering(Transform _candleTransform,Transform _convexLensTransform,Transform _opticalScreenTransform,float focalLength,float blurLevel,float proportion)
    {
        //初始化相关数据
        this._candleTransform = _candleTransform;
        this._convexLensTransform = _convexLensTransform;
        this._opticalScreenTransform = _opticalScreenTransform;
        this.focalLength = focalLength;
        this.blurLevel = blurLevel;
        this.proportion = proportion;

        proportionFocalLength = this.focalLength * this.proportion;

        //绑定必要的组件
        GameObject cam = Instantiate(Resources.Load<GameObject>("Blur/Camera"));
        cam.transform.SetParent(transform);
        cam.transform.localPosition=new Vector3(0,0,-3);
        cam.transform.localEulerAngles=Vector3.zero;
        blurCamera = cam.GetComponent<Camera>();
        blurCamera.enabled = true;
        if (material == null)
        {
            material = Resources.Load<Material>("Blur/FireMaterial");
        }
        if (renderTexture == null)
        {
            renderTexture = Resources.Load<RenderTexture>("Blur/FireTexure");
        }
        blurCamera.targetTexture = renderTexture;
        _opticalScreenTransform.GetChild(1).GetChild(0).gameObject.SetActive(true);
        _opticalScreenTransform.GetChild(1).GetChild(0).GetComponent<MeshRenderer>().material.mainTexture=renderTexture;
        rapidBlurEffect = blurCamera.gameObject.GetComponent<RapidBlurEffect>();
        if (rapidBlurEffect == null)
        {
            rapidBlurEffect = blurCamera.gameObject.AddComponent<RapidBlurEffect>();
        }



        //开始执行渲染
        coroutine = StartCoroutine(DoRendering());
    }

    /// <summary>
    /// 结束渲染
    /// </summary>
    public void EndRendering()
    {

    }

    /// <summary>
    /// 执行渲染
    /// </summary>
    /// <returns></returns>
    IEnumerator DoRendering()
    {
        while (true)
        {
            //计算物距
            objectDistance = _convexLensTransform.position.x - _candleTransform.position.x;
            //计算像距
            imageDistance = _opticalScreenTransform.position.x - _convexLensTransform.position.x;
            
            //如果物距大于两倍焦距，像距在1到2倍焦距之间成倒立缩小的实相
            if (objectDistance >= 2 * proportionFocalLength)
            {
                //计算当前物距下清晰的像距（仅限可以成实相的情况下）
                clearImageDistance = proportionFocalLength * objectDistance / (objectDistance - proportionFocalLength);

                //相机倒立缩小
                blurCamera.orthographicSize = -0.1f + (2f * proportionFocalLength / objectDistance - 1) * 0.05f;
                //if (objectDistance > 29.5f && objectDistance < 30.5f)
                //{
                if (imageDistance >= clearImageDistance)
                {
                    blur = (imageDistance - clearImageDistance) * blurLevel;
                    //rapidBlurEffect.BlurSpreadSize = (imageDistance - 15f) * 0.8f;
                    //if (imageDistance - clearImageDistance >= 15f)
                    //{
                    //    blur = 20f;
                    //    //rapidBlurEffect.BlurSpreadSize = (40f - 15f) * 0.8f;
                    //}
                }
                else if (imageDistance < clearImageDistance)
                {
                    blur = (clearImageDistance - imageDistance) * blurLevel;
                    //if (clearImageDistance-imageDistance <= 0f)
                    //{
                    //    blur = 20f;
                    //}
                }
                //}
            }//如果物距大于一倍焦距小于二倍焦距，像距大于二倍焦距成倒立放大的实像
            else if (objectDistance < 2* proportionFocalLength && objectDistance > proportionFocalLength)
            {
                //计算当前物距下清晰的像距（仅限可以成实相的情况下）
                clearImageDistance = proportionFocalLength * objectDistance / (objectDistance - proportionFocalLength);

                //相机倒立放大
                blurCamera.orthographicSize = -0.1f + (2f * proportionFocalLength / objectDistance - 1) * 0.05f;
                //if (objectDistance > 14.5f && objectDistance < 15.5f)
                //{
                if (imageDistance >= clearImageDistance)
                {
                    blur = (imageDistance - clearImageDistance) * blurLevel;
                    //if (imageDistance-clearImageDistance >= 10f)
                    //{
                    //    blur = 20f;
                    //}
                }
                else if (imageDistance < clearImageDistance)
                {
                    blur = (clearImageDistance - imageDistance) * blurLevel;
                    //if (clearImageDistance-imageDistance>=10)
                    //{
                    //    blur = 20f;
                    //}
                }
                //}
            }//如果物距小于焦距，成正立放大的虚像（就是没有实像）
            else
            {
                //相机正立放大
                blurCamera.orthographicSize = 0.05f;
                //模糊
                blur = 20f;
            }
            

            SetValue();
            yield return null;
        }

    }

    /// <summary>
    /// 根据像距与实像的距离控制模糊程度
    /// </summary>
    private void SetValue()
    {
        rapidBlurEffect.DownSampleNum = 0;
        if (blur <= 1f)
        {
            rapidBlurEffect.BlurSpreadSize = 0;
            rapidBlurEffect.BlurIterations = 0;
        }
        else if (blur > 1 && blur <= 4)
        {
            rapidBlurEffect.BlurSpreadSize = blur - 1;
            rapidBlurEffect.BlurIterations = 1;
        }
        else if (blur > 4 && blur <= 8)
        {
            rapidBlurEffect.BlurSpreadSize = blur - 3;
            rapidBlurEffect.BlurIterations = 2;
        }
        else if (blur > 8 && blur <= 12)
        {
            rapidBlurEffect.BlurSpreadSize = blur - 5;
            rapidBlurEffect.BlurIterations = 3;
        }
        else if (blur > 12 && blur <= 16)
        {
            rapidBlurEffect.BlurSpreadSize = blur - 8;
            rapidBlurEffect.BlurIterations = 4;
        }
        else if (blur > 16 && blur <= 20)
        {
            rapidBlurEffect.BlurSpreadSize = blur - 11;
            rapidBlurEffect.BlurIterations = 5;
        }
        else if (blur > 20f)
        {
            rapidBlurEffect.BlurSpreadSize = 10;
            rapidBlurEffect.BlurIterations = 5;
        }
    }
}
