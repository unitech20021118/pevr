using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TransObj : Action<Main> {
	public Vector3 moveTarget, rotateTarget, scaleTarget=Vector3.one;
	public float moveTime, rotateTime, scaleTime;
	public string targetName;
	public GameObject target;
	public bool isKeepGo;

	public override void DoAction (Main m)
	{
		if (target == null) {
			if (string.IsNullOrEmpty (targetName)) {
				target = m.gameObject;
			} else {
				target = GameObject.Find ("Parent/" + targetName);
			}
		}
        //平滑转换
		target.transform.DOLocalMove (moveTarget, moveTime);

		if (isKeepGo) 
        {
			target.transform.Rotate (rotateTarget * Time.deltaTime);
		} else 
        {
			target.transform.DOLocalRotate (rotateTarget, rotateTime);
		}
		target.transform.DOScale (scaleTarget, scaleTime);
	}
}
