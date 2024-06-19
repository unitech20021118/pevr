using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveToward : Action<Main> {
	public GameObject target;
	public string targetName;
	public List<GameObject> keyObjs;
	public float time=0;
	public bool RunToggle;
	List<Vector3> points;
	Animator animator;

	public override void DoAction(Main m)
	{
		if (target == null) {
			if (string.IsNullOrEmpty (targetName)) {
				target = m.gameObject;
			} else {
				target = GameObject.Find ("Parent/" + targetName);
			}
		}

		animator = target.GetComponent<Animator> ();
		if (target.GetComponent<Rigidbody> () == null) {
			target.AddComponent<Rigidbody> ().useGravity=false;
			target.GetComponent<Rigidbody> ().isKinematic = true;
		}

	    if (points == null)
	    {
	        points = new List<Vector3>();
	        for (var i = 0; i < keyObjs.Count; i++)
	            points.Add(keyObjs[i].transform.position);
	    }

	    if (animator != null)
	    {
	        if (!RunToggle)
	        {
	            animator.SetTrigger("走loop");
            }else
	        {
	            animator.SetTrigger("跑loop");
            }
	        
        }
            
            //animator.SetFloat("Forward", (forward + 1f) / 2f);
	    target.transform.DOPath(points.ToArray(), time, DG.Tweening.PathType.CatmullRom, PathMode.Full3D).SetLookAt(0)
	        .SetEase(Ease.Linear).OnComplete(() =>
	        {
	            //if (animator != null) animator.SetFloat("Forward", 0);
	            if (animator != null) animator.SetTrigger("站立loop");
	        });
	}
}
