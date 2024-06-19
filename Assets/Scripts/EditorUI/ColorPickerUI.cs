using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpringGUI;
using UnityEngine.UI;
public class ColorPickerUI : MonoBehaviour
{
    //改变材质颜色的颜色设置--------------------------
    ChangeColor currentColor;
    ChangeColorInforma informa;
	// Use this for initialization
    //=====================light color
    //添加灯光的颜色设置------------------------------
    AddLightReources LightColor;
    AddLightInforma informe;
    //设置高光描边的颜色设置---------------------------
    private SetHighLight setHighLight;
    private SetHighLightInforma setHighLightInforma;
    //显示文本信息的字体颜色
    private PCShowMsg pcShowMsg;
    private PCShowMsgInforma pcShowMsgInforma;
    private TextQuality _textQuality;
    private TextQualityInforma _textQualityInforma;
    private ButtonQuality _buttonQuality;
    private ButtonQualityInforma _buttonQualityInforma;
    public Image image;

	
	// Update is called once per frame
	void Update ()
    {
        //currentColor.color = transform.GetComponent<ColorPicker>().Color;
        //informa.color = transform.GetComponent<ColorPicker>().Color;s
	}
    /// <summary>
    /// 设置改变材质颜色动作的颜色
    /// </summary>
    /// <param name="a"></param>
    /// <param name="info"></param>
    public void SetCurrentColor(ChangeColor a,ActionInforma info)
    {
        currentColor = a;
        informa = (ChangeColorInforma)info;
    }
    /// <summary>
    /// 设置添加灯光的颜色
    /// </summary>
    /// <param name="d"></param>
    /// <param name="info"></param>
    public void SetCurrentLightColor(AddLightReources d, ActionInforma info)
    {
        LightColor = d;
        informe = (AddLightInforma)info;
    }
    /// <summary>
    /// 设置高光描边的颜色
    /// </summary>
    /// <param name="setHighLight"></param>
    /// <param name="setHighLightInforma"></param>
    public void SetHightLightColor(SetHighLight setHighLight, SetHighLightInforma setHighLightInforma)
    {
        this.setHighLight = setHighLight;
        this.setHighLightInforma = setHighLightInforma;
    }

    public void SetPcShowMessageFontColor(PCShowMsg pcShowMsg, PCShowMsgInforma pcShowMsgInforma)
    {
        this.pcShowMsg = pcShowMsg;
        this.pcShowMsgInforma = pcShowMsgInforma;
    }

    public void SetShowInterfaceFontColor(TextQuality textQuality, TextQualityInforma textQualityInforma)
    {
        _textQuality = textQuality;
        _textQualityInforma = textQualityInforma;
    }

    public void SetShowInterfaceFontColor(ButtonQuality buttonQuality, ButtonQualityInforma buttonQualityInforma)
    {
        _buttonQuality = buttonQuality;
        _buttonQualityInforma = buttonQualityInforma;
    }
    public void Close()
    {
        if (currentColor != null)
        {
            currentColor.color = transform.GetComponent<ColorPicker>().Color;
            informa.color = transform.GetComponent<ColorPicker>().Color.ToString();
        }
        if (LightColor != null)
        {
            LightColor.lightColor = transform.GetComponent<ColorPicker>().Color;
            informe.lightColor = transform.GetComponent<ColorPicker>().Color.ToString();
        }
        if (setHighLight!=null)
        {
            setHighLight.color = transform.GetComponent<ColorPicker>().Color;
            setHighLightInforma.color = transform.GetComponent<ColorPicker>().Color.ToString();
        }
        if (pcShowMsg!=null)
        {
            pcShowMsg.color = transform.GetComponent<ColorPicker>().Color;
            pcShowMsgInforma.color = transform.GetComponent<ColorPicker>().Color.ToString();
        }
        if (_textQualityInforma!=null)
        {
            _textQuality.TextColor = transform.GetComponent<ColorPicker>().Color;
            _textQualityInforma.TextColor = transform.GetComponent<ColorPicker>().Color.ToString();
        }
        if (_buttonQualityInforma!=null)
        {
            _buttonQuality.TextColor = transform.GetComponent<ColorPicker>().Color;
            _buttonQualityInforma.TextColor = transform.GetComponent<ColorPicker>().Color.ToString();
        }
        gameObject.SetActive(false);
        image = null;
    }
}
