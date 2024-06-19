using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class InterfaceQualityInforma
{
    public PEVRUI Pevrui;
    public TextQualityInforma TextQualityInforma;
    public ImageQualityInforma ImageQualityInforma;
    public ButtonQualityInforma ButtonQualityInforma;

    public InterfaceQualityInforma() {}

    public InterfaceQualityInforma(TextQualityInforma textQualityInforma)
    {
        TextQualityInforma = textQualityInforma;
        Pevrui = PEVRUI.Text;
    }

    public InterfaceQualityInforma(ImageQualityInforma imageQualityInforma)
    {
        ImageQualityInforma = imageQualityInforma;
        Pevrui = PEVRUI.Image;
    }

    public InterfaceQualityInforma(ButtonQualityInforma buttonQualityInforma)
    {
        ButtonQualityInforma = buttonQualityInforma;
        Pevrui = PEVRUI.Button;
    }
}




[Serializable]
public class TextQualityInforma
{
    public string TextStr;
    public float TextPositionX;
    public float TextPositionY;
    public float TextPositionZ;
    public int TextFontSize;
    public int TextFontStyle;
    public int Font;
    public float TextWidth;
    public float TextHeight;
    public string TextColor;

    public TextQualityInforma()
    {
        TextStr = "";
        TextPositionX = 300;
        TextPositionY = 250;
        TextPositionZ = 0f;
        TextFontSize = 14;
        TextFontStyle = 0;
        TextWidth = 160f;
        TextHeight = 40f;
        Font = 0;
        TextColor = new Color(1,1,1,1).ToString();
    }
}


[Serializable]
public class ImageQualityInforma
{
    public float ImagePositionX;

    public float ImagePositionY;
    public float ImagePositionZ;
    public float ImageWidth;
    public float ImageHeight;
    public string ImageName;
    public int AjustMode;
    public bool IsBg;

    public ImageQualityInforma()
    {
        ImagePositionX = 300f;
        ImagePositionY = 250f;
        ImagePositionZ = 0f;
        ImageWidth = 100f;
        ImageHeight = 100f;
        ImageName = "";
        AjustMode = 0;
        IsBg = false;

    }
}

[Serializable]
public class ButtonQualityInforma
{
    public float ButtonPositionX;
    public float ButtonPositionY;
    public float ButtonPositionZ;
    public float ButtonWidth;
    public float ButtonHeight;
    public string ButtonText;
    public string ImageName;
    public string EventName;
    public int TextFontSize;
    public int TextFontStyle;
    public int Font;
    public string TextColor;

    public ButtonQualityInforma()
    {
        ButtonPositionX = 350f;
        ButtonPositionY = 200f;
        ButtonPositionZ = 0f;
        ButtonWidth = 100f;
        ButtonHeight = 30f;
        ButtonText = "";
        ImageName = "";
        EventName = "";
        TextFontSize = 14;
        TextFontStyle = 0;
        Font = 0;
        TextColor = new Color(0.2f, 0.2f, 0.2f, 1).ToString();
    }

}






public class InterfaceQuality
{
    public PEVRUI Pevrui;
    public TextQuality TextQuality;
    public ImageQuality ImageQuality;
    public ButtonQuality ButtonQuality;

    public InterfaceQuality() { }

    public InterfaceQuality(TextQuality textQuality)
    {
        TextQuality = textQuality;
        Pevrui = PEVRUI.Text;
    }

    public InterfaceQuality(ImageQuality imageAttribute)
    {
        ImageQuality = imageAttribute;
        Pevrui = PEVRUI.Image;
    }

    public InterfaceQuality(ButtonQuality buttonQuality)
    {
        ButtonQuality = buttonQuality;
        Pevrui = PEVRUI.Button;
    }
}


public class TextQuality
{
    public GameObject TextUiGameObject;
    public string TextStr;
    public Vector3 TextPositionVector3;
    public Vector2 TextRectSizeVector2;
    public int TextFontSize;
    public FontStyle TextFontStyle;
    public Font Font;
    public Color TextColor;

    public TextQuality()
    {
        TextStr = "";
        TextPositionVector3 = new Vector3(300f,250f,0f);
        TextFontSize = 14;
        TextFontStyle = 0;
        //this.Font=
        TextRectSizeVector2 = new Vector2(160f,40f);
        TextColor = new Color(1, 1, 1, 1);
    }
}

public class ImageQuality
{
    public GameObject ImageUiGameObject;
    public string ImagePath;
    public Vector3 ImagePositionVector3;
    public Vector2 ImageRectSizeVector2;
    public int AjustMode;
    public bool IsBg;

    public ImageQuality()
    {
        ImagePath = "";
        ImagePositionVector3 = new Vector3(300f, 250f, 0f);
        ImageRectSizeVector2 = new Vector2(100f,100f);
        AjustMode = 0;
        IsBg = false;
    }
}


public class ButtonQuality
{
    public GameObject ButtonUiGameObject;
    public Vector3 ButtonPositionVector3;
    public Vector2 ButtonRectSizeVector2;
    public Events Events;
    public string ButtonText;
    public string ImagePath;
    public string EventName;
    public int TextFontSize;
    public FontStyle TextFontStyle;
    public Font Font;
    public Color TextColor;

    public ButtonQuality()
    {
        ButtonPositionVector3 = new Vector3(350f,200f,0f);
        ButtonRectSizeVector2 = new Vector2(100f,30f);
        ButtonText = "";
        ImagePath = "";
        EventName = "";
        TextFontSize = 14;
        TextFontStyle = FontStyle.Normal;
        TextColor = new Color(0.2f,0.2f,0.2f,1);
    }
}
