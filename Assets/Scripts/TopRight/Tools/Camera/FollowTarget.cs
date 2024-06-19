using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour {
    public Transform target;
    private Vector3 offset = new Vector3(0,1.61f, 0.37f);
    private float smoothing = 2;

	void Update () {
        Vector3 targetPosition = target.localPosition + offset;
        //transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing * Time.deltaTime);
        transform.position = targetPosition;
        transform.eulerAngles = target.eulerAngles;
        //transform.LookAt(target);
	}
}
