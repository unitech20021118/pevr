using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransObjUI : ActionUI {
	public GameObject translateUI, rotateUI, scaleUI;//移动、旋转、缩放UI物体
	public InputField tx, ty, tz, tt, rx, ry, rz, rt, sx, sy, sz, st;//移动、旋转、缩放输入框
	public GameObject target;
	public Text targetText;
	public Toggle keepGo;
	string targetName;
	bool isLoadData;

	// Use this for initialization
	void Start () {
		if (!isLoadData) {
			isLoadData = false;
			try {
				Transform current = Manager.Instace.gonggong.transform;
//				rx.text = current.eulerAngles.x.ToString ();
//				ry.text = current.eulerAngles.y.ToString ();
//				rz.text = current.eulerAngles.z.ToString ();
				sx.text = current.localScale.x.ToString();
				sy.text = current.localScale.y.ToString();
				sz.text = current.localScale.z.ToString();
			} catch {
			}
		}
		if (string.IsNullOrEmpty (targetName)) {
			targetText.text = "拖拽物体至此框内";
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override Action<Main> CreateAction()
	{
		action = new TransObj();
		TransObj transObj = (TransObj)action;
		transObj.moveTarget = Manager.Instace.gonggong.transform.position;
		actionInforma = new TranslateInforma (false);
		GetStateInfo().actionList.Add(actionInforma);
		actionInforma.name = "Translate";
		return base.CreateAction();
	}

	public override Action<Main> LoadAction (ActionInforma actionInforma)
	{
		TranslateInforma transInforma = (TranslateInforma)actionInforma;
		action = new TransObj();
		TransObj transObj = (TransObj)action;
		//this.actionInforma = actionInforma;
        //设置UI界面属性
		tx.text = transInforma.tx.ToString();
		ty.text = transInforma.ty.ToString();
		tz.text = transInforma.tz.ToString();
		tt.text = transInforma.tt.ToString();
		rx.text = transInforma.rx.ToString();
		ry.text = transInforma.ry.ToString();
		rz.text = transInforma.rz.ToString();
		rt.text = transInforma.rt.ToString();
		sx.text = transInforma.sx.ToString();
		sy.text = transInforma.sy.ToString();
		sz.text = transInforma.sz.ToString();
		st.text = transInforma.st.ToString();
		targetName = transInforma.targetName;
//		keepGo.isOn = transInforma.isKeepGo;
		UpdateInput ();
		isLoadData = true;
		this.actionInforma = actionInforma;
		return action;
	}

    //切换UI界面
	public void SwitchUI(int id){
		switch (id) {
		case 0:
			translateUI.SetActive (true);
			rotateUI.SetActive (false);
			scaleUI.SetActive (false);
			break;
		case 1:
			translateUI.SetActive (false);
			rotateUI.SetActive (true);
			scaleUI.SetActive (false);
			break;
		case 2:
			translateUI.SetActive (false);
			rotateUI.SetActive (false);
			scaleUI.SetActive (true);
			break;
		}
	}

    //将界面属性导入action
	public void UpdateInput(){
		//if (!isLoadData) {

			TransObj transobj = (TransObj)action;

			try {
				transobj.moveTarget = new Vector3 (float.Parse (tx.text), float.Parse (ty.text), float.Parse (tz.text)) + Manager.Instace.gonggong.transform.position;
				transobj.moveTime = float.Parse (tt.text);
				transobj.rotateTarget = new Vector3 (float.Parse (rx.text), float.Parse (ry.text), float.Parse (rz.text));
//				if (keepGo.isOn) {
//					transobj.rotateTarget = Manager.Instace.gonggong.transform.InverseTransformDirection (transobj.rotateTarget);
//				} else {
					Vector3 temp = Manager.Instace.gonggong.transform.eulerAngles;
					Manager.Instace.gonggong.transform.Rotate (transobj.rotateTarget, Space.World);
					transobj.rotateTarget = Manager.Instace.gonggong.transform.eulerAngles;
					Manager.Instace.gonggong.transform.eulerAngles = temp;
//				}
				transobj.rotateTime = float.Parse (rt.text);
				transobj.scaleTarget = new Vector3 (float.Parse (sx.text), float.Parse (sy.text), float.Parse (sz.text));
				transobj.scaleTime = float.Parse (st.text);

				transobj.target = target;
				transobj.targetName = targetName;
				targetText.text = targetName;
//				transobj.isKeepGo = keepGo.isOn;
		
				TranslateInforma transInforma = (TranslateInforma)actionInforma;
//				transInforma.isKeepGo = keepGo.isOn;
				transInforma.tx = float.Parse (tx.text);
				transInforma.ty = float.Parse (ty.text);
				transInforma.tz = float.Parse (tz.text);
				transInforma.tt = float.Parse (tt.text);
				transInforma.rx = float.Parse (rx.text);
				transInforma.ry = float.Parse (ry.text);
				transInforma.rz = float.Parse (rz.text);
				transInforma.rt = float.Parse (rt.text);
				transInforma.sx = float.Parse (sx.text);
				transInforma.sy = float.Parse (sy.text);
				transInforma.sz = float.Parse (sz.text);
				transInforma.st = float.Parse (st.text);
				transInforma.targetName = target.name;
			} catch {
			}

		//}
	}

	public void SetKeepOn(){
		if (keepGo.isOn) {
			rt.enabled = false;
		} else {
			rt.enabled = true;
		}
		UpdateInput ();
	}

	public void SetGameObject(){
//		if (item.isDragging) {
//			target = item.dragedItem.GetTarget ();
//			targetName = target.name;
//			UpdateInput ();
//		}
	}

	public void ReturnGameObject(){
//		if (item.isDragging) {
//			target = null;
//			targetName = "";
//			UpdateInput ();
//		}
	}

	public void DropGameObject(){
		if (item.isDragging) {
			target = item.dragedItem.GetTarget ();
			targetName = target.name;
			UpdateInput ();
		}
	}
}
