using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 创建出的建筑
/// </summary>
public class BrushedBuilding : MonoBehaviour 
{
	/// <summary>
	/// 地基的所有拐角的点的位置
	/// </summary>
	public Vector3[] Foundation;

	/// <summary>
	/// 楼层
	/// </summary>
	public List<BrushedFloor> Floor = new List<BrushedFloor>();

	private BrushedFloor editFloor;



	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void SetFloorNum(int floorNum)
    {
        if (Floor!=null&&Floor.Count>0)
        {
            if (Floor.Count>floorNum)
            {
                for (int i = Floor.Count-1; i > floorNum-1; i--)
                {
					GameObject floor = Floor[i].gameObject;
					Floor.Remove(Floor[i]);
					Destroy(floor);
                }
            }
            else if (Floor.Count<floorNum)
            {
                for (int i = Floor.Count; i < floorNum; i++)
                {
					GameObject newFloor = Instantiate(Floor[0].gameObject);
					newFloor.transform.SetParent(transform);
					Floor.Add(newFloor.GetComponent<BrushedFloor>());
                }
            }
        }
		InitAllFloor();
    }
	

	/// <summary>
	/// 重置每层楼层的高度
	/// </summary>
	public void InitAllFloor()
	{
		if (Floor != null && Floor.Count > 0)
		{
            for (int i = 0; i < Floor.Count; i++)
            {
				Floor[i].transform.localPosition = new Vector3(0, i*3, 0);
            }
		}

	}
}
