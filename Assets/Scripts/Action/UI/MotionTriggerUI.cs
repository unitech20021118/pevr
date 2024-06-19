using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MotionTriggerUI : ActionUI {
//	public GameObject target;
//	public string targetName;
//	public Text targetText;
	public InputField px,py,pz,rx,ry,rz,sx,sy,sz;
	MotionTrigger motionTrigger;
	MotionTriggerInforma motionInforma;
	GameObject motionSensor;

	bool isLoading;

	public override Action<Main> CreateAction()
	{
		action = new MotionTrigger();
		//		action.isOnce = true;
		actionInforma = new MotionTriggerInforma(true);
		GetStateInfo().actionList.Add(actionInforma);
		actionInforma.name = "MotionTrigger";


		motionSensor = Instantiate<GameObject> (Resources.Load<GameObject> ("MotionArea"), Manager.Instace.gonggong.transform);
		motionSensor.transform.localPosition = Vector3.zero;
		motionSensor.transform.localScale = Vector3.one;

		return base.CreateAction();
	}

	public override Action<Main> LoadAction(ActionInforma actionInforma)
	{
		motionInforma = (MotionTriggerInforma)actionInforma;
		//actionInforma = a;
		//this.actionInforma = a;
		this.actionInforma = actionInforma;
		action = new MotionTrigger();
		motionTrigger = (MotionTrigger)action;

		if (motionSensor == null) {
			motionSensor = Instantiate<GameObject> (Resources.Load<GameObject> ("MotionArea"), Manager.Instace.gonggong.transform);
			motionSensor.transform.localPosition = Vector3.zero;
			motionSensor.transform.localScale = Vector3.one;
		}


		isLoading = true;

		px.text = motionInforma.px.ToString ();
		py.text = motionInforma.py.ToString ();
		pz.text = motionInforma.pz.ToString ();
		rx.text = motionInforma.rx.ToString ();
		ry.text = motionInforma.ry.ToString ();
		rz.text = motionInforma.rz.ToString ();
		sx.text = motionInforma.sx.ToString ();
		sy.text = motionInforma.sy.ToString ();
		sz.text = motionInforma.sz.ToString ();
		motionSensor.transform.localPosition = new Vector3 (motionInforma.px, motionInforma.py, motionInforma.pz);
		motionSensor.transform.localEulerAngles = new Vector3 (motionInforma.rx, motionInforma.ry, motionInforma.rz);
		motionSensor.transform.localScale = new Vector3 (motionInforma.sx, motionInforma.sy, motionInforma.sz);

		isLoading = false;
		//timeInputField.text = motionTrigger.duringTime.ToString();
		return action;
	}

	// Use this for initialization
//	void Start () {
//		timeInputField.onValueChanged.AddListener(delegate(string a) { ActionTimeChanged(); });
//	}
//	void ActionTimeChanged()
//	{
//		if (motionTrigger != null)
//		{
//			motionTrigger.duringTime = float.Parse(timeInputField.text);
//			motionInforma.durtime = motionTrigger.duringTime;
//		}
//	}
	// Update is called once per frame
	void Update () {

	}

	public void UpdateInput(){
		motionTrigger = (MotionTrigger)action;
		motionInforma = (MotionTriggerInforma)actionInforma;
		print (motionSensor);
		try{
			if(!isLoading){
				Vector3 position=new Vector3(float.Parse(px.text),float.Parse(py.text),float.Parse(pz.text));
				Vector3 rotateAngle=new Vector3(float.Parse(rx.text),float.Parse(ry.text),float.Parse(rz.text));
				Vector3 Scale=new Vector3(float.Parse(sx.text),float.Parse(sy.text),float.Parse(sz.text));
				motionSensor.transform.localPosition=position;
				motionSensor.transform.localEulerAngles=rotateAngle;
				motionSensor.transform.localScale=Scale;
				motionInforma.px=position.x;
				motionInforma.py=position.y;
				motionInforma.pz=position.z;
				motionInforma.rx=rotateAngle.x;
				motionInforma.ry=rotateAngle.y;
				motionInforma.rz=rotateAngle.z;
				motionInforma.sx=Scale.x;
				motionInforma.sy=Scale.y;
				motionInforma.sz=Scale.z;
			}
		}catch(Exception e){
			print (e);
		}
	}


//	public void DropGameObject(){
//		if (item.isDragging) {
//			target =  item.dragedItem.GetTarget ();
//			rootName = target.name;
//			targetName = target.name;
//			UpdateInput ();
//		}
//	}
}
