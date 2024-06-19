using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 设置高光描边动作的UI
/// created by kuai
/// </summary>
public class SetHighLightUI : ActionUI
{
    /// <summary>
    /// 开关
    /// </summary>
    public Toggle SwichToggle;
    /// <summary>
    /// 开关关闭时用于遮罩的image
    /// </summary>
    public Image SwichImage;
    /// <summary>
    /// 持续时间的文本输入框
    /// </summary>
    public InputField DurationInputField;
    /// <summary>
    /// 是否闪烁的勾选框
    /// </summary>
    public Toggle TwinkleToggle;
    /// <summary>
    /// 目标物体
    /// </summary>
    public GameObject TargetGameObject;
    /// <summary>
    /// 显示目标物体名字的文本框
    /// </summary>
    public Text TargetText;

    private string targetPath;
    public string targetName;
    /// <summary>
    /// 用于显示已选颜色的图片
    /// </summary>
    public Image ColorImage;

    private SetHighLight _setHighLight;
    private SetHighLightInforma _setHighLightInforma;
    public override Action<Main> CreateAction()
    {

        action = new SetHighLight();
        actionInforma = new SetHighLightInforma(true);
        _setHighLight = (SetHighLight) action;
        _setHighLightInforma = (SetHighLightInforma) actionInforma;
        _setHighLight.color = new Color32(255, 142, 0, 255);
        _setHighLightInforma.color = _setHighLight.color.ToString();
        ColorImage.color = _setHighLight.color;
        _setHighLight.swich = SwichToggle.isOn;
        _setHighLightInforma.swich = SwichToggle.isOn;
        GetStateInfo().actionList.Add(actionInforma);
        actionInforma.name = "SetHighLight";
        return base.CreateAction();
    }

    public override Action<Main> LoadAction(ActionInforma actionInforma)
    {
        _setHighLightInforma = (SetHighLightInforma)actionInforma;
        this.actionInforma = actionInforma;
        action = new SetHighLight();
        _setHighLight = (SetHighLight)action;
        //读取数据
        targetName = _setHighLightInforma.targetName;
        targetPath = _setHighLightInforma.rootName;
        _setHighLight.targetPath = _setHighLightInforma.rootName;
        SwichToggle.isOn = _setHighLightInforma.swich;
        _setHighLight.swich = _setHighLightInforma.swich;
        if (!string.IsNullOrEmpty(_setHighLightInforma.targetName))
        {
            string[] shortName = _setHighLightInforma.targetName.Split('/');
            if (shortName.Length > 0)
            {
                TargetText.text = shortName[shortName.Length - 1];
            }
        }
        _setHighLight.duration = _setHighLightInforma.duration;
        _setHighLight.isTwinkle = _setHighLightInforma.isTwinkle;
        DurationInputField.text = _setHighLightInforma.duration.ToString();
        TwinkleToggle.isOn = _setHighLightInforma.isTwinkle;
        if (_setHighLightInforma.color!=null)
        {
            ColorImage.color = Manager.Instace.GetColor(_setHighLightInforma.color);
            _setHighLight.color = ColorImage.color;
        }
        return base.LoadAction(actionInforma);
    }
    /// <summary>
    /// 设置持续时间
    /// </summary>
    public void SetDuration()
    {
        //判断输入的数字是否大于等于0
        //如果小于0强制更改为0
        if (float.Parse(DurationInputField.text)<0)
        {
            DurationInputField.text = "0";
        }
        _setHighLight.duration = float.Parse(DurationInputField.text);
        _setHighLightInforma.duration = float.Parse(DurationInputField.text);
    }
    /// <summary>
    /// 设置闪烁
    /// </summary>
    public void SetTwinkle()
    {
        _setHighLight.isTwinkle = TwinkleToggle.isOn;
        _setHighLightInforma.isTwinkle = TwinkleToggle.isOn;
    }
    /// <summary>
    /// 设置开关
    /// </summary>
    public void SetSwich()
    {
        _setHighLightInforma.swich = SwichToggle.isOn;
        _setHighLight.swich = SwichToggle.isOn;
        if (SwichToggle.isOn)
        {
            SwichImage.gameObject.SetActive(false);
        }
        else
        {
            SwichImage.gameObject.SetActive(true);
        }
    }



    public void DropGameObject()
    {
        if (item.isDragging)
        {
            TargetGameObject = item.dragedItem.GetTarget();
            targetPath = item.dragedItem.GetTargetPath();
            targetName = TargetGameObject.name;
            GetTarget();
        }
    }
    public void GetTarget()
    {

        _setHighLight.target = TargetGameObject;
        //给拖拽的目标物体上一个蓝色的边缘发光效果
        if (TargetGameObject && !TargetGameObject.GetComponent<ConstantHighlighting>())
        {
            TargetGameObject.AddComponent<ConstantHighlighting>();
        }

        if (!string.IsNullOrEmpty(targetName))
        {
            string[] shortName = targetName.Split('/');
            if (shortName.Length > 0)
                TargetText.text = shortName[shortName.Length - 1];
        }
        if (!string.IsNullOrEmpty(targetPath))
        {
            _setHighLightInforma.targetName = targetName;
            _setHighLightInforma.rootName = targetPath;
        }
        else
        {
            _setHighLightInforma.targetName = TargetGameObject.name;
        }
    }
    /// <summary>
    /// 设置颜色
    /// </summary>
    public void SetColor()
    {
        Manager.Instace.ColorPicker.SetActive(true);
        Manager.Instace.ColorPicker.GetComponent<ColorPickerUI>().image = ColorImage;
        Manager.Instace.ColorPicker.GetComponent<ColorPickerUI>().SetHightLightColor(_setHighLight,_setHighLightInforma);
    }
}
