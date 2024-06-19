using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveTowardUI : ActionUI {
	public GameObject keyObj;
	public GameObject target;
	public string targetName;
	public Text targetText;
	public InputField timeInput;
	public List<GameObject> keyObjs;
    public Toggle WalkToggle;
	public Toggle RunToggle;

    MoveTowardInforma moveTowardInfo;
    MoveToward moveToward;

	public override Action<Main> CreateAction()
	{
		action = new MoveToward();
		action.isOnce = true;
		actionInforma = new MoveTowardInforma(true);
	    moveToward = (MoveToward) action;
	    moveTowardInfo = (MoveTowardInforma) actionInforma;
		GetStateInfo().actionList.Add(actionInforma);
	    RunToggle.isOn = false;
	    moveTowardInfo.RunToggle = false;
	    moveToward.RunToggle = false;
	    moveTowardInfo.t = 0f;
	    moveToward.time = 0f;
		actionInforma.name = "MoveToward";
		return base.CreateAction();
	}

	public override Action<Main> LoadAction(ActionInforma actionInforma)
	{
		moveTowardInfo = (MoveTowardInforma)actionInforma;
//		this.actionInforma = actionInforma;
		action = new MoveToward();
		moveToward = (MoveToward)action;
		targetName = moveTowardInfo.targetName;
		targetText.text = moveTowardInfo.targetName;
		timeInput.text = moveTowardInfo.t.ToString ();
	    WalkToggle.isOn = !moveTowardInfo.RunToggle;
		RunToggle.isOn = moveTowardInfo.RunToggle;
	    moveToward.RunToggle = moveTowardInfo.RunToggle;
		moveToward.targetName = moveTowardInfo.targetName;

		if (keyObjs == null) {
			keyObjs = new List<GameObject> ();
		} else {
			keyObjs.Clear ();
		}
		for (int i = 0; i < moveTowardInfo.xs.Count; i++) {
			Vector3 pos = new Vector3 (moveTowardInfo.xs [i], moveTowardInfo.ys [i], moveTowardInfo.zs [i]);
			GameObject go = Instantiate<GameObject> (keyObj, pos, Quaternion.identity);
			go.GetComponent<VTest> ().owner = gameObject;
			go.SetActive (true);
			keyObjs.Add (go);
		}
		this.actionInforma = actionInforma;
		return base.LoadAction(actionInforma);
	}

	void Start () {
		target = Manager.Instace.gonggong;
        timeInputField.onValueChanged.AddListener(delegate(string a) { ActionTimeChanged(); });
	}

    void ActionTimeChanged()
    {
        if (moveToward != null)
        {
            moveToward.duringTime = float.Parse(timeInputField.text);
            moveTowardInfo.durtime = moveToward.duringTime;
        }
    }
	
	void Update () {
		if (keyObjs != null ) {
			if (Manager.Instace._Playing) {
				SetKeyObjsVisibility (false);
			}
			GetComponent<LineRenderer> ().SetPosition (0, target.transform.position);
			GetComponent<LineRenderer> ().numPositions = keyObjs.Count + 1;
			for (int i = 0; i < keyObjs.Count; i++) {
				GetComponent<LineRenderer> ().SetPosition (i + 1, keyObjs [i].transform.position);
			}
		}
		//SetKeyObjsVisibility (gameObject.activeSelf);
	}

	public void UpdateInput(){
		moveToward = (MoveToward)action;
		targetText.text = targetName;
		moveToward.target = target;
		moveToward.time = float.Parse (timeInput.text);
		moveToward.keyObjs = keyObjs;
		moveToward.RunToggle = RunToggle.isOn;
		try{
            moveTowardInfo = (MoveTowardInforma)actionInforma;
			//moveToInforma.points=points;
            moveTowardInfo.targetName = targetName;
            moveTowardInfo.t = moveToward.time;
            moveTowardInfo.RunToggle = RunToggle.isOn;
		}catch{
		}
	}

	public void AddKeyPoint(){
		if (keyObjs == null) {
			keyObjs = new List<GameObject> ();
		}
		GameObject go = Instantiate<GameObject> (keyObj, target.transform.position, Quaternion.identity);
		go.SetActive (true);
		keyObjs.Add (go);
		go.GetComponent<VTest> ().owner = gameObject;
		UpdateInput ();
		try{
            moveTowardInfo = (MoveTowardInforma)actionInforma;
            moveTowardInfo.Addxs(go.transform.position.x);
            moveTowardInfo.Addys(go.transform.position.y);
            moveTowardInfo.Addzs(go.transform.position.z);
		}catch{

		}
	}

	public void RemoveKeyPoint(){
		if (keyObjs == null) {
			keyObjs = new List<GameObject> ();
		}
		GameObject go = keyObjs [keyObjs.Count - 1];
		keyObjs.Remove (go);
		Destroy (go);
		UpdateInput ();
		try{
            moveTowardInfo = (MoveTowardInforma)actionInforma;
            moveTowardInfo.Removexs();
            moveTowardInfo.Removeys();
            moveTowardInfo.Removezs();
		}catch{

		}
	}

	public void SavePath(bool isOk){
		try{
            moveTowardInfo = (MoveTowardInforma)actionInforma;
			for (int i = 0; i < keyObjs.Count; i++) {
                moveTowardInfo.xs[i] = keyObjs[i].transform.position.x;
                moveTowardInfo.ys[i] = keyObjs[i].transform.position.y;
                moveTowardInfo.zs[i] = keyObjs[i].transform.position.z;
			}
			SetKeyObjsVisibility(!isOk);
		}catch{

		}
	}

	public void SetKeyObjsVisibility(bool isVisible){
		GetComponent<LineRenderer> ().enabled = isVisible;
		foreach (GameObject item in keyObjs) {
			item.SetActive (isVisible);
		}
	}

	public void SelectLastKeyObj(){
		if (keyObjs != null && keyObjs.Count > 0) {
			keyObjs [keyObjs.Count - 1].GetComponent<VTest> ().SetTarget ();
		}
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

	public void ClearKeyObjs(){
		foreach (GameObject item in keyObjs) {
			Destroy (item);
		}
		keyObjs.Clear ();
	}
    /// <summary>
    /// 当移动模式改变时
    /// </summary>
    public void OnRunToggleChange()
    {
        if (RunToggle.isOn)
        {
            moveToward.RunToggle = true;
            moveTowardInfo.RunToggle = true;
        }
        else
        {
            moveToward.RunToggle = false;
            moveTowardInfo.RunToggle = false;
        }
    }
}
