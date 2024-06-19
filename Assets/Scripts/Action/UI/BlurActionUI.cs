using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlurActionUI : ActionUI
{
    /// <summary>
    /// 显示蜡烛名字的文本框
    /// </summary>
    public Text CandleNameText;
    /// <summary>
    /// 显示凸透镜名字的文本框
    /// </summary>
    public Text ConvexLensNameText;
    /// <summary>
    /// 显示光屏名字的文本框
    /// </summary>
    public Text OpticalScreenNameText;

    /// <summary>
    /// 凸透镜的焦距
    /// </summary>
    private float focalLength;
    /// <summary>
    /// 凸透镜的输入文本框
    /// </summary>
    public InputField FocalLengthInputField;

    private float blurLevel;
    public InputField BlurLevelInputField;

    private float proportion;
    public InputField ProportionInputField;
    private BlurActionInforma _blurActionInforma;
    private BlurAction _blurAction;

    public override Action<Main> CreateAction()
    {
        action = new BlurAction();
        actionInforma = new BlurActionInforma(true);
        _blurAction = (BlurAction)action;
        _blurActionInforma = (BlurActionInforma)actionInforma;
        //初始化一些相关变量
        _blurAction.FocalLength = 10f;
        _blurActionInforma.FocalLength = 10f;

        _blurAction.BlurLevel = 2f;
        _blurActionInforma.BlurLevel = 2f;

        _blurAction.Proportion = 1f;
        _blurActionInforma.Proportion = 1f;

        GetStateInfo().actionList.Add(actionInforma);
        actionInforma.name = "BlurAction";
        return base.CreateAction();
    }

    public override Action<Main> LoadAction(ActionInforma actionInforma)
    {
        _blurActionInforma = (BlurActionInforma)actionInforma;
        this.actionInforma = actionInforma;
        action = new BlurAction();
        _blurAction = (BlurAction)action;
        //读取数据
        CandleNameText.text = _blurActionInforma.CandleName;
        _blurAction.CandleName = _blurActionInforma.CandleName;

        ConvexLensNameText.text = _blurActionInforma.ConvexLensName;
        _blurAction.ConvexLensName = _blurActionInforma.ConvexLensName;

        OpticalScreenNameText.text = _blurActionInforma.OpticalScreenName;
        _blurAction.OpticalScreenName = _blurActionInforma.OpticalScreenName;

        focalLength = _blurActionInforma.FocalLength;
        FocalLengthInputField.text = focalLength.ToString();
        _blurAction.FocalLength = _blurActionInforma.FocalLength;

        blurLevel = _blurActionInforma.BlurLevel;
        BlurLevelInputField.text = blurLevel.ToString();
        _blurAction.BlurLevel = _blurActionInforma.BlurLevel;

        proportion = _blurActionInforma.Proportion;
        ProportionInputField.text = proportion.ToString();
        _blurAction.Proportion = _blurActionInforma.Proportion;

        return base.LoadAction(actionInforma);
    }



    public void DropGameObject(int num)
    {
        if (item.isDragging)
        {
            switch (num)
            {
                case 1:
                    CandleNameText.text = item.dragedItem.GetTargetPath();
                    _blurActionInforma.CandleName = CandleNameText.text;
                    _blurAction.CandleName = CandleNameText.text;
                    break;
                case 2:
                    ConvexLensNameText.text = item.dragedItem.GetTargetPath();
                    _blurActionInforma.ConvexLensName = ConvexLensNameText.text;
                    _blurAction.ConvexLensName = ConvexLensNameText.text;
                    break;
                case 3:
                    OpticalScreenNameText.text = item.dragedItem.GetTargetPath();
                    _blurActionInforma.OpticalScreenName = OpticalScreenNameText.text;
                    _blurAction.OpticalScreenName = OpticalScreenNameText.text;
                    break;
                default:
                    break;
            }
        }
    }

    /// <summary>
    /// 当输入焦距的文本框内容改变时
    /// </summary>
    public void OnFocalLengthChanged()
    {
        focalLength = float.Parse(FocalLengthInputField.text);
        _blurAction.FocalLength = focalLength;
        _blurActionInforma.FocalLength = focalLength;
    }

    public void OnBlurLevelChanged()
    {
        blurLevel = float.Parse(BlurLevelInputField.text);
        _blurAction.BlurLevel = blurLevel;
        _blurActionInforma.BlurLevel = blurLevel;
    }

    public void OnProportionChanged()
    {
        proportion = float.Parse(ProportionInputField.text);
        _blurAction.Proportion = proportion;
        _blurActionInforma.Proportion = proportion;
    }
}
