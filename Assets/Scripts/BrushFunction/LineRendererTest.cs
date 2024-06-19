using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineRendererTest : MonoBehaviour 
{
	private LineRenderer lineRenderer;
	private Ray ray;
	private RaycastHit raycastHit;
	public LayerMask GroundLayer;
	/// <summary>
	/// linerenderer的点位坐标集合
	/// </summary>
	public List<Vector3> PointList = new List<Vector3>();

	// Use this for initialization
	void Start()
	{
		lineRenderer = GetComponent<LineRenderer>();
	}

	// Update is called once per frame
	void Update()
	{

	}


	public RaycastHit GetHit(LayerMask layer)
	{
		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out raycastHit, 1000f, layer))
		{
			return raycastHit;
		}
		else
		{
			raycastHit = new RaycastHit();
			return raycastHit;
		}

	}

	public void StartDrawCoroutine()
    {
		StartCoroutine(DoDraw());
    }

	/// <summary>
	/// 绘制
	/// </summary>
	/// <returns></returns>
	public IEnumerator DoDraw()
	{

		while (true)
		{

			if (Input.GetMouseButtonDown(0) && GetHit(GroundLayer).collider != null)
			{
				transform.position = raycastHit.point;
				AddNewPoint(raycastHit.point);
				break;
			}
			yield return null;
		}
		yield return new WaitForSeconds(0.2f);
		while (true)
		{
			if (GetHit(GroundLayer).collider != null)
			{
				PointList[PointList.Count - 2] = raycastHit.point + new Vector3(0,0.01f,0);
				Repaint();
				if (Input.GetMouseButtonDown(0))
				{
					AddNewPoint(raycastHit.point);
				}
			}
			if (Input.GetKeyDown(KeyCode.KeypadMinus))
			{
				if (PointList.Count > 3)
				{
					PointList.Remove(PointList[PointList.Count - 3]);
					lineRenderer.numPositions = PointList.Count;
					Repaint();
				}
				else
				{
					Restart();
					StartCoroutine(DoDraw());
					break;
				}
			}
			if (Input.GetMouseButtonDown(1))
			{
				//移除掉列表中倒数第二个点
				PointList.Remove(PointList[PointList.Count - 2]);
				//重新绘制图形
				lineRenderer.numPositions = PointList.Count;
				Repaint();
				BrushManager.Instance.CanCreateInteriorWalls = true;
				break;
			}
			yield return null;
		}
	}
	/// <summary>
	/// 重置
	/// </summary>
	public void Restart()
    {
		PointList.Clear();
		lineRenderer.numPositions = 2;
		lineRenderer.SetPositions(new Vector3[2] { transform.position, transform.position });
	}

	/// <summary>
	/// 添加节点
	/// </summary>
	/// <param name="point"></param>
	public void AddNewPoint(Vector3 point)
	{
		point = point + new Vector3(0, 0.01f, 0);
		if (PointList.Count == 0)
		{
			PointList.Add(point);
			PointList.Add(point);
		}
		PointList.Add(point);
		PointList[PointList.Count - 2] = point;
		PointList[PointList.Count - 1] = PointList[0];

		lineRenderer.numPositions = PointList.Count;
		Repaint();
	}
	/// <summary>
	/// 绘制图形
	/// </summary>
	public void Repaint()
	{
		lineRenderer.SetPositions(PointList.ToArray());
	}

	//public Vector2 WorldToUGui(Vector3 vector3)
	//   {
	//	Vector2 screenpoint = Camera.main.WorldToScreenPoint(vector3);
	//	Vector2 worldPoint;
	//       if (RectTransformUtility.ScreenPointToLocalPointInRectangle(Canvas.GetComponent<RectTransform>(),screenpoint,Canvas.worldCamera,out worldPoint))
	//       {
	//		return worldPoint;
	//       }
	//       else
	//       {
	//		return Vector2.zero;
	//       }
	//   }


	/// <summary>
	/// 计算两个向量的夹角
	/// </summary>
	/// <param name="form"></param>
	/// <param name="to"></param>
	/// <returns></returns>
	public float DotToAngle(Vector3 form, Vector3 to)
	{
		float rad = 0f;
		rad = Mathf.Acos(Vector3.Dot(form.normalized, to.normalized));
		return rad * Mathf.Rad2Deg;
	}
}
