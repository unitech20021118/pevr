using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointBase : MonoBehaviour
{

	/// <summary>
	/// 下一个点
	/// </summary>
	public Transform NextPoint;

	private RaycastHit raycastHit;

	public LayerMask layerMask;

	private Transform mouseCheckTransform;

	private SpriteRenderer spriterenderer;
	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
       
	}
	/// <summary>
	/// 向下一个点发射射线
	/// </summary>
	public Vector3 SetRayToNextPoint()
    {
		if (Physics.Raycast(transform.position, Vector3.Normalize(NextPoint.position-transform.position), out raycastHit, Vector3.Distance(transform.position, NextPoint.position), layerMask))
        {
			//鼠标指向了画出的线
			return raycastHit.point;
        }
        else
        {
			return new Vector3(0,-500,0);
        }
    }

	void OnMouseEnter()
    {
        if (spriterenderer==null)
        {
			spriterenderer = transform.GetComponent<SpriteRenderer>();
        }
		spriterenderer.color = Color.blue;
    }

	void OnMouseExit()
    {
		if (spriterenderer == null)
		{
			spriterenderer = transform.GetComponent<SpriteRenderer>();
		}
		spriterenderer.color = Color.white;
	}

	void OnMouseDown()
    {
        if (mouseCheckTransform==null)
        {
			//mouseCheckTransform = transform.GetComponentInParent<LineRendererTest>().mouseCheck.transform;
		}
    }
	void OnMouseDrag()
    {
		transform.position = mouseCheckTransform.position;
	}
}
