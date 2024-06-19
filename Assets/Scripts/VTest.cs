using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VTest : MonoBehaviour {
	public GameObject owner;
	public GameObject target;
	public G_EditorTarget g_target;
	public CamRayCtrl camRayCtrl;

	void Awake(){
		g_target = Manager.Instace.GetComponent<G_EditorTarget> ();
	}

	// Use this for initialization
	void Start () {
		camRayCtrl = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<CamRayCtrl> ();
	}
	
	// Update is called once per frame
	void Update () {
//		HighlighterController hc = gameObject.GetComponentInParent<HighlighterController>();
//		hc.MouseOver ();
		if (Manager.Instace._Playing) {
			gameObject.SetActive (false);
		}
		if (owner == null) {
			Destroy (gameObject);
		}
	}

	void OnMouseDown(){
		target = gameObject;
//		StartCoroutine (EndFrame());
		camRayCtrl.target=gameObject.transform;
		camRayCtrl.SetSelected ();
		g_target.positionCenter.transform.gameObject.SetActive (false);
	}

	public void SetTarget(){
        //		g_target.positionCenter.SetActive(true);
        //		g_target.scaleCenter.SetActive(false);
        //		g_target.positionCenter.transform.position = target.transform.position;
        //		G_PositionAxis positionAxis=g_target.positionCenter.GetComponent<G_PositionAxis>();
	    //		positionAxis.SetTarget(target);
	    if (camRayCtrl==null)
	    {
	        camRayCtrl = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CamRayCtrl>();
        }
        camRayCtrl.target=gameObject.transform;
		camRayCtrl.SetSelected ();
	    if (g_target==null)
	    {
	        g_target = Manager.Instace.GetComponent<G_EditorTarget>();
        }
		g_target.positionCenter.transform.gameObject.SetActive (false);
	}

	IEnumerator EndFrame(){
		yield return new WaitForEndOfFrame ();
		SetTarget ();
	}
}
