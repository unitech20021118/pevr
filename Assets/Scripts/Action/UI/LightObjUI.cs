using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightObjUI : ActionUI
{
    public Slider IndensitysSlider;//光强度
    public Toggle PreviewToggle;
    public InputField IndensitysInputField;
    private LightObjInforma _lightObjInforma;
    private LightObj _lightObj;
    private Light thelight;
    private GameObject lightObj;

    private float indensity;

    private float originalIndensity;
    // Use this for initialization
    void Start()
    {
        //if (lightObj == null)
        //{
        //    lightObj = new GameObject("light");
        //    thelight = lightObj.AddComponent<Light>();
        //    lightObj.transform.parent = Manager.Instace.gonggong.transform;
        //    lightObj.transform.localEulerAngles = Vector3.zero;
        //    lightObj.transform.localPosition = Vector3.zero;
        //    thelight.type = LightType.Directional;
        //    SceneCtrl.instance.PlayScene += HideLightObj;
        //}
    }

    // Update is called once per frame
    void Update()
    {

    }

    //void HideLightObj()
    //{
    //    lightObj.SetActive(false);
    //}

    public override Action<Main> CreateAction()
    {
        action = new LightObj();
        actionInforma = new LightObjInforma(true);
        _lightObj = (LightObj) action;
        _lightObjInforma = (LightObjInforma) actionInforma;


        GetLight();
        //初始化slider的值
        IndensitysSlider.value = thelight.intensity;
        PreviewToggle.isOn = true;
        GetStateInfo().actionList.Add(actionInforma);
        actionInforma.name = "LightObj";
        return base.CreateAction();
    }

    public override Action<Main> LoadAction(ActionInforma actionInforma)
    {
        _lightObjInforma = (LightObjInforma)actionInforma;
        this.actionInforma = actionInforma;
        action = new LightObj();
        _lightObj = (LightObj)action;

        //读取数据
        GetLight();
        IndensitysSlider.value = _lightObjInforma.indensity;

        _lightObj.intensity = _lightObjInforma.indensity;

        //if (lightObj == null)
        //{
        //    lightObj = new GameObject("light");
        //}
        //thelight = lightObj.AddComponent<Light>();
        //lightObj.transform.parent = Manager.Instace.gonggong.transform;
        //lightObj.transform.localEulerAngles = Vector3.zero;
        //lightObj.transform.localPosition = Vector3.zero;
        //thelight.type = LightType.Directional;
        //thelight.intensity = _lightObjInforma.indensity;
        //_lightObj.thelight = thelight;
        //_lightObj.intensity = _lightObjInforma.indensity;
        //lightObj.SetActive(preShow.isOn);

        //indensity.value = _lightObjInforma.indensity;


        return action;
    }


    public void GetLight()
    {
        //获取环境光
        thelight = GameObject.Find("Directional Light").GetComponent<Light>();
        originalIndensity = thelight.intensity;
    }

    public void OnIndensitysSliderChanged()
    {
        _lightObjInforma.indensity = IndensitysSlider.value;
        _lightObj.intensity = IndensitysSlider.value;
        IndensitysInputField.text = IndensitysSlider.value.ToString();
        if (PreviewToggle.isOn)
        {
            thelight.intensity = IndensitysSlider.value;
        }
        
    }

    public void OnPreviewValueChanged()
    {
        if (PreviewToggle.isOn)
        {
           thelight.intensity = IndensitysSlider.value;
        }
        else
        {
            thelight.intensity = originalIndensity;
        }
    }

    public void OnIndensitysIFChanged()
    {
        indensity = float.Parse(IndensitysInputField.text);
        indensity = Mathf.Clamp(indensity, 0f, 8f);
        IndensitysInputField.text = indensity.ToString();
        IndensitysSlider.value = indensity;
    }
    //public void UpdateInput()
    //{
    //    _lightObj = (LightObj)action;
    //    _lightObj.thelight = thelight;
    //    _lightObj.intensity = IndensitysSlider.value;
    //    try
    //    {
    //        thelight.intensity = IndensitysSlider.value;
    //        _lightObjInforma = (LightObjInforma)actionInforma;
    //        _lightObjInforma.indensity = IndensitysSlider.value;
    //        _lightObjInforma.preshow = PreviewToggle.isOn;
    //        lightObj.SetActive(PreviewToggle.isOn);
    //    }
    //    catch
    //    {

    //    }
    //}

    //void OnDestroy()
    //{
    //    Destroy(lightObj);
    //    SceneCtrl.instance.PlayScene -= HideLightObj;
    //}
}
