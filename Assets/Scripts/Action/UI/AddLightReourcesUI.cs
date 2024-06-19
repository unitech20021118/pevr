using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddLightReourcesUI :ActionUI {
	public GameObject target,lastTarget;
	public string targetName;
	public Text targetText;
    public GameObject LightSetting;
    //edit by 王梓亦
    public string gstyle;//灯光名
    public InputField lightname;
    //要添加灯光的目标物体
    public Transform Target;
    //添加的灯光在物体上的位置
    public Vector3 LightLocalPosition;

    public GameObject keyObj;
    private GameObject keyObjClone;
    /// <summary>
    /// 灯光强度滑条
    /// </summary>
    public Slider IntensitySlider;
    /// <summary>
    /// 阴影强度滑动条
    /// </summary>
    public Slider ShadowStrengthSlider;

    AddLightReources addlightreourcse;
    AddLightInforma addlightinforma;

    public override Action<Main> CreateAction()
    {
        
        action = new AddLightReources();
        actionInforma = new AddLightInforma(true);
        //edit by 王梓亦
        addlightreourcse = (AddLightReources)action;
        addlightinforma = (AddLightInforma)actionInforma;
        if (addlightinforma != null)
        {
            addlightinforma.gstyle = lightname.text;
            addlightreourcse.gstyle = lightname.text;
        }
        Target = ActionTargetTransform;
        LightLocalPosition = Vector3.zero;
        addlightinforma.LightLocalPositionX = LightLocalPosition.x;
        addlightinforma.LightLocalPositionY = LightLocalPosition.y;
        addlightinforma.LightLocalPositionZ = LightLocalPosition.z;
        addlightreourcse.LightLocalPosition = LightLocalPosition;
        IntensitySlider.value = 3f;
        ShadowStrengthSlider.value = 1f;
        GetStateInfo().actionList.Add(actionInforma);
        //Debug.LogError(GetStateInfo().StateTargetGameObjectPath);
        actionInforma.name = "AddLightReources";
        return base.CreateAction();
    }
    
    public override Action<Main> LoadAction(ActionInforma a)
    {
        addlightinforma = (AddLightInforma)a;
        this.actionInforma = a;
//		AddLightReources addLight = (AddLightReources)action;
		LightSetting.SetActive(true);
		action = new AddLightReources();

        addlightreourcse = (AddLightReources)action;


		if (!string.IsNullOrEmpty (addlightinforma.lightColor)) {
		    addlightreourcse.lightColor = Manager.Instace.GetColor (addlightinforma.lightColor);
		}
        addlightreourcse.z = float.Parse(addlightinforma.z);
        addlightreourcse.x = float.Parse(addlightinforma.x);
        addlightreourcse.targetName = addlightinforma.targetName;
        //edit by 王梓亦
        addlightreourcse.gstyle = addlightinforma.gstyle;
		targetText.text= addlightinforma.targetName;
        //edit by kuai
        LightLocalPosition = new Vector3(addlightinforma.LightLocalPositionX, addlightinforma.LightLocalPositionY, addlightinforma.LightLocalPositionZ);
        addlightreourcse.LightLocalPosition = LightLocalPosition;
        IntensitySlider.value = float.Parse(addlightinforma.z);
        ShadowStrengthSlider.value = float.Parse(addlightinforma.x);
        targetName = addlightinforma.targetName;

        //TargetTransform = addlightinforma.

        //        action = new AddLightReources(addLightInforma.lightColor, addLightInforma.z, addLightInforma.x);
        return action;
    }


    public void SetLightColor()
    {
        Manager.Instace.ColorPicker.SetActive(true);
        AddLightReources cc = (AddLightReources)action;
        //Manager.Instace.gonggong.GetComponent<MeshRenderer>().material.color = Color.red;
        Manager.Instace.ColorPicker.GetComponent<ColorPickerUI>().SetCurrentLightColor(cc,actionInforma);
    }
    public void SetLight()
    {     
        AddLightReources dd = (AddLightReources)action;
        //Manager.Instace.gonggong.GetComponent<MeshRenderer>().material.color = Color.red;
        LightSetting.GetComponent<LightSetting>().SetCurrentLight(dd, actionInforma);
        LightSetting.SetActive(false);

    }
    public void Lightset()
    {
        AddLightReources dd = (AddLightReources)action;
        LightSetting.SetActive(true);
    }

	public void UpdateInput(){
		AddLightReources addLight = (AddLightReources)action;
		addLight.target = target;

		targetText.text = targetName;
		try{
			AddLightInforma addLightInfo = (AddLightInforma)actionInforma;
			addLightInfo.targetName=targetName;
		}
        catch
        {
		}
	}

	public void SetGameObject(){
		if (item.isDragging) {
			target = item.dragedItem.GetTarget ();
			targetName = target.name;
//			UpdateInput ();
		}
	}

	public void ReturnGameObject(){
		if (item.isDragging) {
			target = lastTarget;
			if (lastTarget == null) {
				targetName = "拖拽目标至框内";
			} else {
				targetName = lastTarget.name;
			}
//			UpdateInput ();
		}
	}

	public void DropGameObject(){
		if (item.isDragging) {
			lastTarget =  item.dragedItem.GetTarget ();
			UpdateInput ();
		}
	}
    /// <summary>
    /// 打开灯光位置的修改箭头
    /// </summary>
    public void ChangeLightPosition()
    {
        if (!string.IsNullOrEmpty(targetName) && targetName != "拖拽目标至框内")
        {
            Target = GameObject.Find("Parent/" + targetName).transform;
        }
        else
        {
            Target = ActionTargetTransform;
        }
        if (keyObjClone != null)
        {
            keyObjClone.SetActive(true);
            keyObjClone.GetComponent<VTest>().SetTarget();
        }
        else
        {
            keyObjClone = Instantiate<GameObject>(keyObj, Target.position, Quaternion.identity);
            keyObjClone.transform.SetParent(Target);
            keyObjClone.SetActive(true);
            keyObjClone.transform.localPosition = LightLocalPosition;
            keyObjClone.GetComponent<VTest>().owner = gameObject;
            keyObjClone.GetComponent<VTest>().SetTarget();
        }
    }
    /// <summary>
    /// 结束灯光位置的修改
    /// </summary>
    public void EndChangeLightPosition()
    {
        if (keyObjClone!=null)
        {
            keyObjClone.SetActive(false);
        }
        LightLocalPosition = keyObjClone.transform.localPosition;
        addlightreourcse.LightLocalPosition = LightLocalPosition;
        addlightinforma.LightLocalPositionX = LightLocalPosition.x;
        addlightinforma.LightLocalPositionY = LightLocalPosition.y;
        addlightinforma.LightLocalPositionZ = LightLocalPosition.z;
    }
    /// <summary>
    /// 当灯光强度滑动条变化时
    /// </summary>
    public void OnIntensitySliderChange()
    {
        addlightinforma.z = IntensitySlider.value.ToString();
        addlightreourcse.z = IntensitySlider.value;
    }
    /// <summary>
    /// 当阴影强度滑动条变化时
    /// </summary>
    public void OnShadowStrengthSliderChange()
    {
        addlightinforma.x = ShadowStrengthSlider.value.ToString();
        addlightreourcse.x = ShadowStrengthSlider.value;

    }
}
