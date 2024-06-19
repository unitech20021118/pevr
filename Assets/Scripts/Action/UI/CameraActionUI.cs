using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CameraActionUI : ActionUI {
	public GameObject target;
	public InputField timeInput,speedInput;
	public Toggle typeA,typeB;
    public Toggle CloseToggle;
    public Image MaskImage;
	Vector3 offset,eulerAngle;
    private CameraActionInforma cameraActionInforma;
	int type;
	bool preShowRotate;

	public override Action<Main> CreateAction()
	{
		action = new CameraAction();
        action.isOnce = true;
		actionInforma = new CameraActionInforma(true);
        
        cameraActionInforma = (CameraActionInforma)actionInforma;
		GetStateInfo().actionList.Add(actionInforma);
		actionInforma.name = "CameraAction";
		return base.CreateAction();
	}

	public override Action<Main> LoadAction(ActionInforma a)
	{
	    cameraActionInforma = (CameraActionInforma)a;
		action = new CameraAction();
		CameraAction camAction = (CameraAction)action;
		camAction.type=type= cameraActionInforma.type;
		offset = new Vector3 (cameraActionInforma.px, cameraActionInforma.py, cameraActionInforma.pz);
		eulerAngle = new Vector3 (cameraActionInforma.rx, cameraActionInforma.ry, cameraActionInforma.rz);
	    camAction.close = cameraActionInforma.close;
	    CloseToggle.isOn = !cameraActionInforma.close;
		camAction.px= cameraActionInforma.px;
		camAction.py= cameraActionInforma.py;
		camAction.pz= cameraActionInforma.pz;
		camAction.rx= cameraActionInforma.rx;
		camAction.ry= cameraActionInforma.ry;
		camAction.rz= cameraActionInforma.rz;
		camAction.time = cameraActionInforma.t;
		camAction.speed = cameraActionInforma.speed;
		timeInput.text = cameraActionInforma.t.ToString ();
		speedInput.text = cameraActionInforma.speed.ToString();

		if (camAction.type == 0) {
			
			typeB.isOn = false;typeA.isOn = true;
		} else if (camAction.type == 1) {
			
			typeA.isOn = false;typeB.isOn = true;
		}
		UpdateInput ();
		this.actionInforma = a;
		return base.LoadAction(a);
	}

	void Start () {
		target = Manager.Instace.gonggong;
	}
	
	void Update () {
		if (preShowRotate) {
			if (SceneCtrl.running) {
				preShowRotate = false;
			}
			Manager.Instace.mainCamera.transform.RotateAround (target.transform.position, Vector3.up, float.Parse (speedInput.text)*Time.deltaTime);
			if (Input.GetMouseButtonDown (1)) {
				preShowRotate = false;
			}
		}
	}

	public void UpdateInput(){
		CameraAction camAction = (CameraAction)action;
	    camAction.close = !CloseToggle.isOn;
		camAction.type = type;
		camAction.time = float.Parse (timeInput.text);
		camAction.speed = float.Parse (speedInput.text);
		try{
			//CameraActionInforma camActionInfo = (CameraActionInforma)actionInforma;
            if (cameraActionInforma==null)
            {
                cameraActionInforma = (CameraActionInforma)actionInforma;
            }
			//moveToInforma.points=points;
            cameraActionInforma.type = type;
            cameraActionInforma.t = camAction.time;
            cameraActionInforma.speed = camAction.speed;
		    cameraActionInforma.close = !CloseToggle.isOn;
		}catch(System.Exception e)
        {
            Debug.LogError(e);
		}
	}

	public void SetType(int typeId){
		type = typeId;
		UpdateInput ();
	}

	public void SetCamPosition(){
		Transform camTrans = Manager.Instace.mainCamera.transform;
		eulerAngle = camTrans.eulerAngles;
		offset = camTrans.position - target.transform.position;
		CameraAction camAction = (CameraAction)action;
		camAction.px = offset.x;
		camAction.py = offset.y;
		camAction.pz = offset.z;
		camAction.rx = camTrans.eulerAngles.x;
		camAction.ry = camTrans.eulerAngles.y;
		camAction.rz = camTrans.eulerAngles.z;
		try{
			CameraActionInforma camActionInfo = (CameraActionInforma)actionInforma;
			camActionInfo.px=camAction.px;
			camActionInfo.py=camAction.py;
			camActionInfo.pz=camAction.pz;
			camActionInfo.rx=camAction.rx;
			camActionInfo.ry=camAction.ry;
			camActionInfo.rz=camAction.rz;
		}catch{
		}
	}

	public void PreShow(){
		if (Manager.Instace.mainCamera != null&&target!=null) {
			Manager.Instace.mainCamera.transform.DOMove (target.transform.position + offset, float.Parse (timeInput.text));
			Manager.Instace.mainCamera.transform.DORotate (eulerAngle, float.Parse (timeInput.text));
		}
	}

	public void PreShowRotate(){
		if (Manager.Instace.mainCamera != null&&target!=null) {
			Manager.Instace.mainCamera.transform.DOMove (target.transform.position + offset, 0);
			Manager.Instace.mainCamera.transform.DORotate (eulerAngle, 0);
			preShowRotate = true;
		}
	}

    public void OnCloseToggleChanged()
    {
        if (CloseToggle.isOn)
        {
            MaskImage.gameObject.SetActive(false);
        }
        else
        {
            MaskImage.gameObject.SetActive(true);
        }
        UpdateInput();
    }
}
