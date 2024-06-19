using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRayCtrl : MonoBehaviour {
	private Camera cam;
	public Transform target;
	public float speed=0;
	public static CamRayCtrl axisCam;
	public bool isX,isY,isZ;
	public Vector3 startPos,currentPos;
	public Transform axisTrans;
	private Vector3 relativeVector;

	// Use this for initialization
	void Start () {
		cam=GetComponent<Camera> ();
		axisCam = this;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			Ray ray = cam.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			LayerMask mask = 1 << 10;
			if (Physics.Raycast(ray,out hit ,30,mask))
			{
				startPos = currentPos = Input.mousePosition;
				hit.transform.GetComponent<AxisCtrl> ().SetAxis ();
				SetRelativeVector ();
			}
			if (!Physics.Raycast(ray,out hit ,30))
			{
				axisTrans.gameObject.SetActive (false);
			}
		}
		if (Manager.Instace._Playing) {
			axisTrans.gameObject.SetActive (false);
		}
		SetAxisToTarget ();
		MoveTarget ();
		if (Input.GetMouseButtonUp (0)) {
			isX = isY = isZ = false;
		}
	}

	public void SetSelected(){
		axisTrans.gameObject.SetActive (true);
	}

	public void SetRelativeVector(){
		relativeVector = Input.mousePosition - GetAxisPosition ();
	}

	Vector3 GetAxisPosition(){
		return cam.WorldToScreenPoint (axisTrans.position);
	}

	void SetAxisToTarget(){
		if (target) {
			Vector3 targetPos=cam.WorldToScreenPoint (target.position);
			axisTrans.position = cam.ScreenToWorldPoint (new Vector3(targetPos.x, targetPos.y, 20));
		}
	}

	int GetMoveDir(){
		if (Vector3.Dot (relativeVector, Input.mousePosition-currentPos) > 0) {
			return 1;
		} else {
			return -1;
		}
	}

	void MoveTarget(){
		if (currentPos != Input.mousePosition) {
			if (isX) {
				target.Translate ((Input.mousePosition-currentPos).magnitude*GetMoveDir () * speed * Time.deltaTime, 0, 0);
				currentPos = Input.mousePosition;
			} else if (isY) {
				target.Translate (0, (Input.mousePosition-currentPos).magnitude*GetMoveDir () * speed * Time.deltaTime, 0);
				currentPos = Input.mousePosition;
			} else if (isZ) {
				target.Translate (0, 0, (Input.mousePosition-currentPos).magnitude*GetMoveDir () * speed * Time.deltaTime);
				currentPos = Input.mousePosition;
			}
		}
	}
}
