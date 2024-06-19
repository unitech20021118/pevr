using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Win32;
using UnityEngine;
using UnityEngine.UI;

public class FollowMouseMovementUI : ActionUI
{
    /// <summary>
    /// 是否线
    /// </summary>
    public Toggle WireToggle;
    /// <summary>
    /// 是否面
    /// </summary>
    public Toggle FaceToggle;
    /// <summary>
    /// 是否限制范围
    /// </summary>
    public Toggle PlaceToggle;
    /// <summary>
    /// x输入框
    /// </summary>
    public InputField XInputField;
    /// <summary>
    /// y输入框
    /// </summary>
    public InputField YInputField;
    /// <summary>
    /// z输入框
    /// </summary>
    public InputField ZInputField;
    /// <summary>
    /// x最小值输入框
    /// </summary>
    public InputField XMinInputField;
    /// <summary>
    /// x最大值输入框
    /// </summary>
    public InputField XMaxInputField;
    /// <summary>
    /// y最小值输入框
    /// </summary>
    public InputField YMinInputField;
    /// <summary>
    /// y最大值输入框
    /// </summary>
    public InputField YMaxInputField;
    /// <summary>
    /// z最小值输入框
    /// </summary>
    public InputField ZMinInputField;
    /// <summary>
    /// z最大值输入框
    /// </summary>
    public InputField ZMaxInputField;
    /// <summary>
    /// 遮罩
    /// </summary>
    public GameObject MaskGameObject;

    public Toggle XToggle;
    public Toggle YToggle;
    public Toggle ZToggle;

    public GameObject WireGameObject;
    public GameObject FaceGameObject;
    public GameObject PlacePositionGameObject;

    private FollowMouseMovement _followMouseMovement;
    private FollowMouseMovementInforma _followMouseMovementInforma;

    public override Action<Main> CreateAction()
    {
        action = new FollowMouseMovement();
        actionInforma = new FollowMouseMovementInforma(true);
        _followMouseMovement = (FollowMouseMovement)action;
        _followMouseMovementInforma = (FollowMouseMovementInforma)actionInforma;
        //初始化一些变量
        _followMouseMovementInforma.X = 0;
        _followMouseMovementInforma.Y = 0;
        _followMouseMovementInforma.Z = 0;
        _followMouseMovementInforma.Place = false;
        _followMouseMovementInforma.XMin = "";
        _followMouseMovementInforma.XMax = "";
        _followMouseMovementInforma.YMin = "";
        _followMouseMovementInforma.YMax = "";
        _followMouseMovementInforma.ZMin = "";
        _followMouseMovementInforma.ZMax = "";

        _followMouseMovementInforma.isFace = false;
        _followMouseMovementInforma.faceX = false;
        _followMouseMovementInforma.faceY = false;
        _followMouseMovementInforma.faceZ = true;

        _followMouseMovement.X = 0;
        _followMouseMovement.Y = 0;
        _followMouseMovement.Z = 0;
        _followMouseMovement.Place = false;
        _followMouseMovement.XMin = "";
        _followMouseMovement.XMax = "";
        _followMouseMovement.YMin = "";
        _followMouseMovement.YMax = "";
        _followMouseMovement.ZMin = "";
        _followMouseMovement.ZMax = "";

        _followMouseMovement.isFace = false;
        _followMouseMovement.faceX = false;
        _followMouseMovement.faceY = false;
        _followMouseMovement.faceZ = true;



        GetStateInfo().actionList.Add(actionInforma);
        actionInforma.name = "FollowMouseMovement";
        return base.CreateAction();
    }

    public override Action<Main> LoadAction(ActionInforma actionInforma)
    {
        _followMouseMovementInforma = (FollowMouseMovementInforma)actionInforma;
        this.actionInforma = actionInforma;
        action = new FollowMouseMovement();
        _followMouseMovement = (FollowMouseMovement)action;
        //读取数据
        _followMouseMovement.X = _followMouseMovementInforma.X;
        _followMouseMovement.Y = _followMouseMovementInforma.Y;
        _followMouseMovement.Z = _followMouseMovementInforma.Z;
        _followMouseMovement.Place = _followMouseMovementInforma.Place;
        _followMouseMovement.XMin = _followMouseMovementInforma.XMin;
        _followMouseMovement.XMax = _followMouseMovementInforma.XMax;
        _followMouseMovement.YMin = _followMouseMovementInforma.YMin;
        _followMouseMovement.YMax = _followMouseMovementInforma.YMax;
        _followMouseMovement.ZMin = _followMouseMovementInforma.ZMin;
        _followMouseMovement.ZMax = _followMouseMovementInforma.ZMax;
        
        _followMouseMovement.isFace = _followMouseMovementInforma.isFace;
        _followMouseMovement.faceX = _followMouseMovementInforma.faceX;
        _followMouseMovement.faceY = _followMouseMovementInforma.faceY;
        _followMouseMovement.faceZ = _followMouseMovementInforma.faceZ;


        XInputField.text = _followMouseMovementInforma.X.ToString();
        YInputField.text = _followMouseMovementInforma.Y.ToString();
        ZInputField.text = _followMouseMovementInforma.Z.ToString();
        PlaceToggle.isOn = _followMouseMovementInforma.Place;
        XMinInputField.text= _followMouseMovementInforma.XMin;
        XMaxInputField.text= _followMouseMovementInforma.XMax;
        YMinInputField.text= _followMouseMovementInforma.YMin;
        YMaxInputField.text= _followMouseMovementInforma.YMax;
        ZMinInputField.text= _followMouseMovementInforma.ZMin;
        ZMaxInputField.text= _followMouseMovementInforma.ZMax;

        FaceToggle.isOn = _followMouseMovementInforma.isFace;
        if (_followMouseMovementInforma.isFace)
        {
            WireToggle.isOn = false;
        }
        else
        {
            WireToggle.isOn = true;
        }
        XToggle.isOn = _followMouseMovementInforma.faceX;
        YToggle.isOn = _followMouseMovementInforma.faceY;
        ZToggle.isOn = _followMouseMovementInforma.faceZ;


        return base.LoadAction(actionInforma);
    }

    /// <summary>
    /// 当是否限制范围的值改变时
    /// </summary>
    public void PlaceChanged()
    {
        if (PlaceToggle.isOn)
        {
            MaskGameObject.SetActive(false);
            _followMouseMovementInforma.Place = true;
            _followMouseMovement.Place = true;
        }
        else
        {
            MaskGameObject.SetActive(true);
            _followMouseMovementInforma.Place = false;
            _followMouseMovement.Place = false;
        }
    }

    /// <summary>
    /// 当x、y、z的值被修改时
    /// </summary>
    public void OnPointValueChanged(int num)
    {
        float a;
        switch (num)
        {
            case 1:
                a = float.Parse(XInputField.text);
                a = Mathf.Clamp(a, -1f, 1f);
                _followMouseMovementInforma.X = a;
                _followMouseMovement.X = a;
                XInputField.text = a.ToString();
                break;
            case 2:
                a = float.Parse(YInputField.text);
                a = Mathf.Clamp(a, -1f, 1f);
                _followMouseMovementInforma.Y = a;
                _followMouseMovement.Y = a;
                YInputField.text = a.ToString();
                break;
            case 3:
                a = float.Parse(ZInputField.text);
                a = Mathf.Clamp(a, -1f, 1f);
                _followMouseMovementInforma.Z = a;
                _followMouseMovement.Z = a;
                ZInputField.text = a.ToString();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 当限制范围中的值被修改时
    /// </summary>
    public void OnPlaceValueChanged(int num)
    {
        string min;
        string max;
        switch (num)
        {
            case 1:

                min = XMinInputField.text;
                max = XMaxInputField.text;
                PlaceChange(true, ref min, ref max);
                XMinInputField.text = min;
                XMaxInputField.text = max;
                //if (XMaxInputField.text!="")
                //{
                //    if (float.Parse(XMinInputField.text)> float.Parse(XMaxInputField.text))
                //    {
                //        XMinInputField.text = XMaxInputField.text;
                //    }
                //}
                _followMouseMovementInforma.XMin = XMinInputField.text;
                _followMouseMovement.XMin = _followMouseMovementInforma.XMin;
                break;
            case 2:
                 min = XMinInputField.text;
                 max = XMaxInputField.text;
                PlaceChange(false, ref min, ref max);
                XMinInputField.text = min;
                XMaxInputField.text = max;
                //if (XMinInputField.text != "")
                //{
                //    if (float.Parse(XMaxInputField.text) < float.Parse(XMinInputField.text))
                //    {
                //        XMaxInputField.text = XMinInputField.text;
                //    }
                //}
                _followMouseMovementInforma.XMax = XMaxInputField.text;
                _followMouseMovement.XMax = _followMouseMovementInforma.XMax;
                break;
            case 3:
                min = YMinInputField.text;
                max = YMaxInputField.text;
                PlaceChange(true, ref min, ref max);
                YMinInputField.text = min;
                YMaxInputField.text = max;
                //if (YMaxInputField.text != "")
                //{
                //    if (float.Parse(YMinInputField.text) > float.Parse(YMaxInputField.text))
                //    {
                //        YMinInputField.text = YMaxInputField.text;
                //    }
                //}
                _followMouseMovementInforma.YMin = YMinInputField.text;
                _followMouseMovement.YMin = _followMouseMovementInforma.YMin;
                break;
            case 4:
                min = YMinInputField.text;
                max = YMaxInputField.text;
                PlaceChange(false, ref min, ref max);
                YMinInputField.text = min;
                YMaxInputField.text = max;
                _followMouseMovementInforma.YMax = YMaxInputField.text;
                _followMouseMovement.YMax = _followMouseMovementInforma.YMax;
                break;
            case 5:
                min = ZMinInputField.text;
                max = ZMaxInputField.text;
                PlaceChange(true, ref min, ref max);
                ZMinInputField.text = min;
                ZMaxInputField.text = max;
                _followMouseMovementInforma.ZMin = ZMinInputField.text;
                _followMouseMovement.ZMin = _followMouseMovementInforma.ZMin;
                break;
            case 6:
                min = ZMinInputField.text;
                max = ZMaxInputField.text;
                PlaceChange(false, ref min, ref max);
                ZMinInputField.text = min;
                ZMaxInputField.text = max;
                _followMouseMovementInforma.ZMax = ZMaxInputField.text;
                _followMouseMovement.ZMax = _followMouseMovementInforma.ZMax;
                break;
            default:
                break;
        }
    }

    public void PlaceChange(bool changeMin, ref string min, ref string max)
    {
        if (min != "" && max != "")
        {
            if (changeMin)
            {
                if (float.Parse(min) > float.Parse(max))
                {
                    min = max;
                }
            }
            else
            {
                if (float.Parse(min) > float.Parse(max))
                {
                    max = min;
                }
            }
        }
    }

    public void OnFaceToggleChanged()
    {
        if (WireToggle.isOn)
        {
            _followMouseMovementInforma.isFace = false;
            _followMouseMovement.isFace = false;
            WireGameObject.SetActive(true);
            FaceGameObject.SetActive(false);
            PlaceToggle.gameObject.SetActive(true);
            PlacePositionGameObject.SetActive(true);
        }
        if (FaceToggle.isOn)
        {
            _followMouseMovementInforma.isFace = true;
            _followMouseMovement.isFace = true;
            WireGameObject.SetActive(false);
            FaceGameObject.SetActive(true);
            PlaceToggle.gameObject.SetActive(false);
            PlacePositionGameObject.SetActive(false);
        }
    }

    public void OnFaceXYZChanged()
    {
        if (XToggle.isOn)
        {
            _followMouseMovementInforma.faceX = true;
            _followMouseMovement.faceX = true;
            _followMouseMovementInforma.faceY = false;
            _followMouseMovement.faceY = false;
            _followMouseMovementInforma.faceZ = false;
            _followMouseMovement.faceZ = false;
        }
        else if(YToggle.isOn)
        {
            _followMouseMovementInforma.faceX = false;
            _followMouseMovement.faceX = false;
            _followMouseMovementInforma.faceY = true;
            _followMouseMovement.faceY = true;
            _followMouseMovementInforma.faceZ = false;
            _followMouseMovement.faceZ = false;
        }
        else if (ZToggle.isOn)
        {
            _followMouseMovementInforma.faceX = false;
            _followMouseMovement.faceX = false;
            _followMouseMovementInforma.faceY = false;
            _followMouseMovement.faceY = false;
            _followMouseMovementInforma.faceZ = true;
            _followMouseMovement.faceZ = true;
        }
    }
}
