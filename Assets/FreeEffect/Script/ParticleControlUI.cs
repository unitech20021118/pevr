using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParticleControlUI : ActionUI
{
    public Dropdown dropdown;
    //public InputField DT;
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
    private ParticleControl _particleControl;
    private ParticleControlInforma _particleControlInforma;

    public override Action<Main> CreateAction()
    {

        action = new ParticleControl();
        actionInforma = new ParticleControlInforma(true);
        _particleControl = (ParticleControl) action;
        _particleControlInforma = (ParticleControlInforma) actionInforma;
        //初始化数据
        dropdown.value = 0;
        _particleControl._Etype = 0;
        _particleControlInforma._Etype = 0;

        GetStateInfo().actionList.Add(actionInforma);
        actionInforma.name = "ParticleControl";
        return base.CreateAction();
    }

    public override Action<Main> LoadAction(ActionInforma a)
    {
        _particleControlInforma = (ParticleControlInforma)a;
        this.actionInforma = a;
        action = new ParticleControl();
        _particleControl = (ParticleControl) action;

        //读取数据
        _particleControl._Etype = _particleControlInforma._Etype;
        dropdown.value = _particleControlInforma._Etype;
        //_particleControl._Dtime = _particleControlInforma.d;

        //dropdown.value = _particleControlInforma._Etype;
        //DT.text = _particleControlInforma.d.ToString();

        targetName = _particleControlInforma.targetName;
        targetPath = _particleControlInforma.rootName;
        _particleControl.targetPath = _particleControlInforma.rootName;
        if (!string.IsNullOrEmpty(_particleControlInforma.targetName))
        {
            TargetText.text = _particleControlInforma.targetName;
        }

        //action = new ParticleControl(particleControlInforma.t, particleControlInforma.o, particleControlInforma.d);
        
        return base.LoadAction(actionInforma);
    }

    //ParticleControl particleControl;
	//public void UpInput(){

 //       ParticleControl particleControl = (ParticleControl)action;

 //       particleControl.target = TargetGameObject;
 //       particleControl._Etype = dropdown.value;
 //       //particleControl._Dtime = float.Parse(DT.text);
 //       try
 //       {
 //           ParticleControlInforma particleControlInforma = (ParticleControlInforma)actionInforma;
 //           particleControlInforma.targetName = targetName;
 //           particleControlInforma._Etype = dropdown.value;
 //           //particleControlInforma.d = float.Parse(DT.text);
 //       }
 //       catch
 //       {
 //       }
	//}
    public void SetGameObject()
    {
        if (item.isDragging)
        {
            TargetGameObject = item.dragedItem.GetTarget();
            targetName = TargetGameObject.name;
            //			UpdateTarget ();
        }
    }

    //public void ReturnGameObject()
    //{
    //    if (item.isDragging)
    //    {
    //        target = lastTarget;
    //        if (lastTarget == null)
    //        {
    //            targetName = "拖拽目标至框内";
    //        }
    //        else
    //        {
    //            targetName = lastTarget.name;
    //        }
    //        //			UpdateTarget ();
    //    }
    //}

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

        _particleControl.target = TargetGameObject;
        //给拖拽的目标物体上一个蓝色的边缘发光效果
        if (TargetGameObject && !TargetGameObject.GetComponent<ConstantHighlighting>())
        {
            TargetGameObject.AddComponent<ConstantHighlighting>();
        }

        if (!string.IsNullOrEmpty(targetName))
        {
            TargetText.text = targetName;
            _particleControlInforma.targetName = targetName;
        }
        if (!string.IsNullOrEmpty(targetPath))
        {
            _particleControlInforma.rootName = targetPath;
        }
    }

    public void OnChangeDropDown()
    {
        _particleControlInforma._Etype = dropdown.value;
        _particleControl._Etype = dropdown.value;
        //Debug.LogError(dropdown.value );
    }
}
