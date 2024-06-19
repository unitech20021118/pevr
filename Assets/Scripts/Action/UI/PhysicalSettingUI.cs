using System;
using UnityEngine;
using UnityEngine.UI;

public class PhysicalSettingUI : ActionUI {
    /// <summary>
    /// 摩擦系数和质量大小输入框
    /// </summary>
	public InputField factor,value;
    /// <summary>
    /// 是否受重力影响的单选框
    /// </summary>
	public Toggle isGravity;
    //public Toggle isBoxcollider;
    private PhysicalSetting physicalSetting;

    private PhysicalSettingInforma physicalSettingInforma;

	public override Action<Main> CreateAction()
	{
		action = new PhysicalSetting ();
		actionInforma = new PhysicalSettingInforma(true);
	    physicalSetting = (PhysicalSetting) action;
	    physicalSettingInforma = (PhysicalSettingInforma) actionInforma;
        GetStateInfo().actionList.Add(actionInforma);
		actionInforma.name = "PhysicalSetting";
		return base.CreateAction();
	}

	public override Action<Main> LoadAction (ActionInforma actionInforma)
	{
	    physicalSettingInforma = (PhysicalSettingInforma)actionInforma;
	    action = new PhysicalSetting();
	    physicalSetting = (PhysicalSetting)action;
        this.actionInforma = actionInforma;

        //if (psInforma.isBoxCollider==null)
        //{
        //    psInforma.isBoxCollider = true;
        //    Debug.LogError("1221212");
        //}
        //Debug.LogError(psInforma.isBoxCollider);
        //isBoxcollider.isOn = psInforma.isBoxCollider; 

        value.text = physicalSettingInforma.massNum.ToString();
	    factor.text = physicalSettingInforma.factor.ToString();
	    isGravity.isOn = physicalSettingInforma.isGravity;

        physicalSetting.massNum = physicalSettingInforma.massNum;
	    physicalSetting.factor = physicalSettingInforma.factor;
	    physicalSetting.isGravity = physicalSettingInforma.isGravity;
        //ps.isBoxCollider = psInforma.isBoxCollider;
		return action;
	}

    /// <summary>
    /// 设置质量
    /// </summary>
    public void SetMass()
    {
        //edit by kuai
        float va = float.Parse(value.text);
        physicalSetting.massNum = va;
        physicalSettingInforma.massNum = va;
        /*
        physicalSetting = (PhysicalSetting)action;
		try
        {
			float va=float.Parse(value.text);
            //float fa = float.Parse(factor.text);
            physicalSetting.massNum = va;
            //ps.factor = fa;
			physicalSettingInforma = (PhysicalSettingInforma)actionInforma;
            physicalSettingInforma.massNum=va;
			//psInforma.factor=fa;
		}
        catch (Exception e)
        {
		    Debug.LogError(e);	
		}
        */
    }
    /// <summary>
    /// 设置摩擦系数
    /// </summary>
    public void SetFactor()
    {
        //edit by kuai
        float fa = float.Parse(factor.text);
        physicalSetting.factor = fa;
        physicalSettingInforma.factor = fa;
        
        //physicalSetting = (PhysicalSetting)action;
        //try
        //{
            // va = float.Parse(value.text);
            //float fa = float.Parse(factor.text);
            //ps.massNum = va;
            //physicalSetting.factor = fa;
            //physicalSettingInforma = (PhysicalSettingInforma)actionInforma;
            //psInforma.massNum = va;
            //physicalSettingInforma.factor = fa;
        //}
        //catch(Exception e)
        //{
        //    Debug.LogError(e);
        //}
    }

    //设置接受重力
    public void SetUseGravity(bool useGravity)
    {
        //edit by kuai
        physicalSetting.isGravity = useGravity;
        physicalSettingInforma.isGravity = useGravity;
	}
    /// <summary>
    /// 设置碰撞盒
    /// </summary>
    //public void SetBoxCollider(bool useBoxcollider)
    //{
    //    try
    //    {
    //        PhysicalSetting ps = (PhysicalSetting)action;
    //        ps.isBoxCollider = useBoxcollider;
    //        PhysicalSettingInforma psInformal = (PhysicalSettingInforma)actionInforma;
    //        //psInformal.isBoxCollider = useBoxcollider;
    //    }
    //    catch (System.Exception e)
    //    {

    //        Debug.Log(e);
    //    }
    //}

	//输入检查
	public void CheckNumInput(InputField ipt)
    {
		try
        {
			float v=float.Parse(ipt.text);
		}
        catch (System.Exception ex)
        {
			ipt.text = "0";
		}
	}
}
