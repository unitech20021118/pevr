using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrushedRoad : MonoBehaviour 
{


	/// <summary>
	/// 开始的点位置
	/// </summary>
	public Transform StartPoint;
	/// <summary>
	/// 结束的点位置
	/// </summary>
	public Transform EndPoint;


	/// <summary>
	/// 长度
	/// </summary>
	private float length;
	/// <summary>
	/// 宽度
	/// </summary>
	private float width;
	/// <summary>
	/// 离地高度
	/// </summary>
	private float heightAboveGround;


	private Transform headCorner;
	private Transform tailCorner;

	/// <summary>
	/// 获取到的其他道路
	/// </summary>
	private BrushedRoad[] roads;

	public float Length
	{
		get
		{
			length = transform.localScale.z;
			return length;
		}
		set
		{
			length = value;
			//transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, length);
			EndPoint.position = StartPoint.position + (EndPoint.position - StartPoint.position).normalized * value;
			ChangeStartOrEndPointPosition(ModifyWallMode.ActiveEndPoint);
		}
	}
	public float Width
	{
		get
		{
			width = transform.localScale.x;
			return width;
		}
		set
		{
			width = value;
			transform.localScale = new Vector3(width, transform.localScale.y, transform.localScale.z);
		}
	}
	public float HeightAboveGround 
	{
		get
		{
			heightAboveGround = NumericalConversion.GetHeightAboveGround(transform);
			return heightAboveGround;
		}
		set
		{
			heightAboveGround = value;
			transform.localPosition = new Vector3(transform.localPosition.x, NumericalConversion.GetTargetPositionYByHeight(transform, value), transform.localPosition.z);
		}
	}


    // Use this for initialization
    void Start () 
	{
		headCorner = transform.Find("headCorner");
		tailCorner = transform.Find("tailCorner");
	}
	
	// Update is called once per frame
	void Update () 
	{
        
		
	}
	void LateUpdate()
    {
		if (transform.localScale.z >= 0.5f)
		{
			headCorner.localPosition = new Vector3(0, 0, -0.5f);
			headCorner.localScale = new Vector3(1, 1, transform.localScale.x / transform.localScale.z);
			tailCorner.localPosition = new Vector3(0, 0, 0.5f);
			tailCorner.localScale = new Vector3(1, 1, transform.localScale.x / transform.localScale.z);
		}
	}



	/// <summary>
	/// 路面的的起点或终点的位置被修改
	/// </summary>
	public void ChangeStartOrEndPointPosition(ModifyWallMode modifyWallMode)
	{
		switch (modifyWallMode)
		{
			case ModifyWallMode.ActiveStartPoint:
				ModifyAttribute();
				//使与该起点链接的墙壁修改属性
				roads = BrushManager.Instance.GetLastRoad(this);
				if (roads != null && roads.Length > 0)
				{
					for (int i = 0; i < roads.Length; i++)
					{
						roads[i].ChangeStartOrEndPointPosition(ModifyWallMode.Passive);
					}
				}
				break;
			case ModifyWallMode.ActiveEndPoint:
				ModifyAttribute();
				//使与该终点链接的墙壁修改属性
				roads = BrushManager.Instance.GetNextRoad(this);
				if (roads != null && roads.Length > 0)
				{
					for (int i = 0; i < roads.Length; i++)
					{
						roads[i].ChangeStartOrEndPointPosition(ModifyWallMode.Passive);
					}
				}
				break;
			case ModifyWallMode.Passive:
				ModifyAttribute();
				break;
			default:
				break;
		}
		length = Vector3.Distance(StartPoint.position, EndPoint.position);
		transform.localScale = new Vector3(width, 0.001f, length);
		transform.localPosition = (StartPoint.position + EndPoint.position) / 2 + new Vector3(0, 0.0005f, 0);
		transform.LookAt(EndPoint.position + new Vector3(0, 0.0005f, 0));
		
	}
	/// <summary>
	/// 修改属性
	/// </summary>
	public void ModifyAttribute()
	{
		length = Vector3.Distance(StartPoint.position, EndPoint.position);
		transform.localScale = new Vector3(width, 0.001f, length);
		transform.localPosition = (StartPoint.position + EndPoint.position) / 2 + new Vector3(0, 0.0005f, 0);
		transform.LookAt(EndPoint.position + new Vector3(0, 0.0005f, 0));
	}
}
