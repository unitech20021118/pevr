using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragUIMoveObjOnGround : MonoBehaviour {

	public Transform Target;

	Vector3 offest;

	Vector3 MousePositionOnGround = new Vector3(5000, -5000, 5000);

	Vector3 hitPosition;
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
       
		
	}
	void LateUpdate()
    {
		if (Target != null)
		{
			transform.localPosition = BrushManager.Instance.WorldToUGui(Target.localPosition);
		}
	}

	public void Init(Transform target)
    {
		Target = target;
    }

	public void OnPointDown()
	{
		offest = Vector3.zero;
	}
	public void OnPointDrag()
	{
		if (G_RayCastCheck.QuiescentObjectCheckGround(out hitPosition, G_PubDef.QuiescentObject))
		{
			if (MousePositionOnGround == new Vector3(5000, -5000, 5000))
			{
				offest = Vector3.zero;
			}
			else
			{
				offest = hitPosition - MousePositionOnGround;
			}
			Target.localPosition += offest;
			MousePositionOnGround = hitPosition;
			
		}
	}
	public void OnEndPointDrag()
    {
		offest = Vector3.zero;
		MousePositionOnGround = new Vector3(5000, -5000, 5000);
	}
}
