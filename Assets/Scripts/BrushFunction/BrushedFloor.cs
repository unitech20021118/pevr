using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrushedFloor : MonoBehaviour 
{
	/// <summary>
	/// 该层建筑的所有外墙
	/// </summary>
	public List<BrushedWall> peripheralWalls = new List<BrushedWall>();
	/// <summary>
	/// 该层建筑的所有内墙
	/// </summary>
	public List<BrushedWall> InteriorWalls = new List<BrushedWall>();

	/// <summary>
	/// 地板
	/// </summary>
	public BrushedGround ground;
	/// <summary>
	/// 天花板
	/// </summary>
	public BrushedGround ceiling;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
