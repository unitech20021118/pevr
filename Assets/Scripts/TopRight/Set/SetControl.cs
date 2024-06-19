using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.UI;

public class SetControl : MonoBehaviour
{
    /// <summary>
    /// 是否隐藏变换工具栏的单选框
    /// </summary>
    private Toggle changeToolToggle;
    /// <summary>
    /// 变换工具栏
    /// </summary>
    private GameObject changeToolGameObject;
    /// <summary>
    /// 变换工具栏原本的位置
    /// </summary>
    private Vector3 changeToolPositionVector3;
    /// <summary>
    /// 是否隐藏对象列表的单选框
    /// </summary>
    private Toggle objectToggle;
    /// <summary>
    /// 对象列表
    /// </summary>
    private GameObject objectListGameObject;
    /// <summary>
    /// 对象列表原本的位置
    /// </summary>
    private Vector3 objectListPosition;
    /// <summary>
    /// 是否是全屏的toggle
    /// </summary>
    private Toggle fullScreenToggle;
    /// <summary>
    /// 分辨率选择下拉框
    /// </summary>
    private Dropdown resolvingDropdown;
    /// <summary>
    /// 光照进度条
    /// </summary>
    private Slider lightslider;
    private InputField lightIntensityInputField;
    private Light light;

    /// <summary>
    /// 确认修改按钮
    /// </summary>
    private Button confirmChangeButton;
    /// <summary>
    /// 关闭/取消修改按钮
    /// </summary>
    private Button CloseButton;
    /// <summary>
    /// 设置的值
    /// </summary>
    private SetUpValue setUpValue;

    private string SetUpKey = "SetUpKey";
    // Use this for initialization
    void Start()
    {
        changeToolToggle = transform.Find("set_panel/set_box/change_tool_toggle").GetComponent<Toggle>();
        changeToolToggle.onValueChanged.AddListener(delegate { OnToggleValueChanged(changeToolToggle); });
        changeToolGameObject = GameObject.Find("Canvas").transform.Find("ChangeTool").gameObject;
        objectToggle = transform.Find("set_panel/set_box/object_toggle").GetComponent<Toggle>();
        objectToggle.onValueChanged.AddListener(delegate { OnToggleValueChanged(objectToggle); });
        objectListGameObject = GameObject.Find("Canvas").transform.Find("ObjectListPanel").gameObject;
        fullScreenToggle = transform.Find("set_panel/set_box/fullwindow_toggle").GetComponent<Toggle>();
        fullScreenToggle.onValueChanged.AddListener(delegate { OnToggleValueChanged(fullScreenToggle); });
        resolvingDropdown = transform.Find("set_panel/set_box/drop/Dropdown").GetComponent<Dropdown>();
        lightslider= transform.Find("set_panel/set_box/light_slider/slider").GetComponent<Slider>();
        lightIntensityInputField = transform.Find("set_panel/set_box/light_slider/LightIntensityInputField").GetComponent<InputField>();
        light =GameObject.Find("Directional Light").GetComponent<Light>();
        resolvingDropdown.onValueChanged.AddListener(delegate { OnDropDownValueChanged(resolvingDropdown); });
        confirmChangeButton = transform.Find("set_panel/BtnConform").GetComponent<Button>();
        confirmChangeButton.onClick.AddListener(ConfirmButtonCilck);
        CloseButton = transform.Find("set_panel/close").GetComponent<Button>();
        CloseButton.onClick.AddListener(CloseButtonClick);
        
        try
        {
            setUpValue = JsonMapper.ToObject<SetUpValue>(PlayerPrefs.GetString(SetUpKey));
            if (setUpValue == null)
            {
                setUpValue = new SetUpValue();
                ApplySetUpValue();
            }
            else
            {
                InitSetUpValue();
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }


    }
    /// <summary>
    /// 应用设置的值
    /// </summary>
    private void InitSetUpValue()
    {
        changeToolToggle.isOn = setUpValue.ToolToggleValue;
        objectToggle.isOn = setUpValue.ObjToggleValue;
        fullScreenToggle.isOn = setUpValue.FullScreenToggleValue;
        resolvingDropdown.value = setUpValue.ResolvingDropdownValue;
        lightIntensityInputField.text = setUpValue.LightValue;
        OnLigntIntensityInputFieldValueChanged();
    }
    /// <summary>
    /// 修改设置的值
    /// </summary>
    private void ApplySetUpValue()
    {
        setUpValue.ToolToggleValue = changeToolToggle.isOn;
        setUpValue.ObjToggleValue = objectToggle.isOn;
        setUpValue.FullScreenToggleValue = fullScreenToggle.isOn;
        setUpValue.ResolvingDropdownValue = resolvingDropdown.value;
        setUpValue.LightValue = lightIntensityInputField.text;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnToggleValueChanged(Toggle toggle)
    {
        if (toggle == changeToolToggle)
        {
            if (toggle.isOn)
            {
                changeToolGameObject.transform.localPosition = changeToolPositionVector3;
            }
            else
            {
                changeToolPositionVector3 = changeToolGameObject.transform.localPosition;
                changeToolGameObject.transform.localPosition = new Vector3(5000, 0, 0);
            }
        }
        else if (toggle == objectToggle)
        {
            if (toggle.isOn)
            {
                objectListGameObject.transform.localPosition = objectListPosition;
            }
            else
            {
                objectListPosition = objectListGameObject.transform.localPosition;
                objectListGameObject.transform.localPosition = new Vector3(5000, 0, 0);
            }
        }
        else if (toggle == fullScreenToggle)
        {
            Screen.fullScreen = toggle.isOn;
        }
    }

    private void OnDropDownValueChanged(Dropdown dropdown)
    {
        if (dropdown == resolvingDropdown)
        {
            switch (dropdown.value)
            {
                case 0:
                    Screen.SetResolution(1024, 576, fullScreenToggle.isOn);
                    break;
                case 1:
                    Screen.SetResolution(1280, 720, fullScreenToggle.isOn);
                    break;
                case 2:
                    Screen.SetResolution(1366, 768, fullScreenToggle.isOn);
                    break;
                case 3:
                    Screen.SetResolution(1920, 1080, fullScreenToggle.isOn);
                    break;
                case 4:
                    Screen.SetResolution(2560, 1440, fullScreenToggle.isOn);
                    break;
                default:
                    Screen.SetResolution(1920, 1080, fullScreenToggle.isOn);
                    break;
            }
        }
    }

    public void OnSliderChange()
    {
        lightIntensityInputField.text = lightslider.value.ToString("N2");
        light.intensity = lightslider.value;
    }

    public void OnLigntIntensityInputFieldValueChanged()
    {
        float v = float.Parse(lightIntensityInputField.text);
        if (v < lightslider.minValue)
        {
            v = lightslider.minValue;
        }
        else if (v>lightslider.maxValue)
        {
            v = lightslider.maxValue;
        }
        lightIntensityInputField.text = v.ToString("N2");
        v = float.Parse(lightIntensityInputField.text);
        lightslider.value = v;
        
    }

    private void ConfirmButtonCilck()
    {
        ApplySetUpValue();
        PlayerPrefs.SetString(SetUpKey, JsonMapper.ToJson(setUpValue));
        transform.Find("set_panel").gameObject.SetActive(false);
    }

    private void CloseButtonClick()
    {
        InitSetUpValue();
        transform.Find("set_panel").gameObject.SetActive(false);
    }
}
/// <summary>
/// 设置的值
/// </summary>
public class SetUpValue
{
    /// <summary>
    /// 工具栏toggle的值
    /// </summary>
    public bool ToolToggleValue;
    /// <summary>
    /// 属性列表toggle的值
    /// </summary>
    public bool ObjToggleValue;
    /// <summary>
    /// 是否全屏的值
    /// </summary>
    public bool FullScreenToggleValue;
    /// <summary>
    /// 设置分辨率的值
    /// </summary>
    public int ResolvingDropdownValue;
    /// <summary>
    /// 设置光照的值
    /// </summary>
    public string LightValue;

    public SetUpValue() { }

    public SetUpValue(bool toolValue, bool objValue, bool fullScreenValue, int resolvingValue,string lightvalue)
    {
        ToolToggleValue = toolValue;
        ObjToggleValue = objValue;
        FullScreenToggleValue = fullScreenValue;
        LightValue = lightvalue;
    }

}