using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 生成的一段墙壁
/// </summary>
public class BrushedWall : MonoBehaviour 
{

	/// <summary>
	/// 开始的点
	/// </summary>
	public Transform StartPoint;
	/// <summary>
	/// 结束的点
	/// </summary>
	public Transform EndPoint;
	/// <summary>
	/// 中间的点
	/// </summary>
	public Transform MiddlePoint;

	public GameObject TextObj;

	public GameObject TextObj2;
	/// <summary>
	/// 长度
	/// </summary>
	private float length;
	/// <summary>
	/// 宽度
	/// </summary>
	private float width;
	/// <summary>
	/// 高度
	/// </summary>
	private float height;
	/// <summary>
	/// 离地高度
	/// </summary>
	private float heightAboveGround;
	/// <summary>
	/// 获取到的其他墙壁
	/// </summary>
	private BrushedWall[] walls;
	/// <summary>
	/// 附属于该墙壁的门
	/// </summary>
	public List<BrushedDoor> doors = new List<BrushedDoor>();
	/// <summary>
	/// 附属于该墙壁的窗户
	/// </summary>
	public List<BrushedWindow> windows = new List<BrushedWindow>();


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
    public float Height 
	{
		get
		{
			height = transform.localScale.y;
			return height;
		}
		set
		{
			height = value;
			transform.localScale = new Vector3(transform.localScale.x, height, transform.localScale.z);
			transform.localPosition = new Vector3(transform.localPosition.x, NumericalConversion.GetTargetPositionYByHeight(transform, heightAboveGround), transform.localPosition.z);
            if (doors!=null&&doors.Count>0||windows!=null&&windows.Count>0)
            {
				BooleanRtManager.Instance.ResetDoorsAndWindowsOfWall(this);
			}
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
			transform.localPosition = new Vector3(transform.localPosition.x, NumericalConversion.GetTargetPositionYByHeight(transform,value), transform.localPosition.z);
			if (doors != null && doors.Count > 0 || windows != null && windows.Count > 0)
			{
				BooleanRtManager.Instance.ResetDoorsAndWindowsOfWall(this);
			}
		}
	}


    /// <summary>
    /// 墙壁的的起点或终点的位置被修改
    /// </summary>
    public void ChangeStartOrEndPointPosition(ModifyWallMode modifyWallMode)
    {
        switch (modifyWallMode)
        {
            case ModifyWallMode.ActiveStartPoint:
				ModifyAttribute();
				//使与该起点链接的墙壁修改属性
				walls = BrushManager.Instance.GetLastWall(this);
                if (walls!=null&&walls.Length>0)
                {
                    for (int i = 0; i < walls.Length; i++)
                    {
						walls[i].ChangeStartOrEndPointPosition(ModifyWallMode.Passive);
                    }
				}
				break;
            case ModifyWallMode.ActiveEndPoint:
				ModifyAttribute();
				//使与该终点链接的墙壁修改属性
				walls = BrushManager.Instance.GetNextWall(this);
				if (walls != null && walls.Length > 0)
				{
					for (int i = 0; i < walls.Length; i++)
					{
						walls[i].ChangeStartOrEndPointPosition(ModifyWallMode.Passive);
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
		transform.localScale = new Vector3(width, height, length);
		transform.localPosition = (StartPoint.position + EndPoint.position) / 2+new Vector3(0,height/2,0);
		transform.LookAt(EndPoint.position + new Vector3(0, height/2, 0));
        if (doors!=null&&doors.Count>0)
        {
			BooleanRtManager.Instance.ResetDoorsAndWindowsOfWall(this);
		}
    }
	/// <summary>
	/// 修改属性
	/// </summary>
	public void ModifyAttribute()
    {
		length = Vector3.Distance(StartPoint.position, EndPoint.position);
		transform.localScale = new Vector3(width, height, length);
		transform.localPosition = (StartPoint.position + EndPoint.position) / 2 + new Vector3(0, height / 2, 0);
		transform.LookAt(EndPoint.position + new Vector3(0, height / 2, 0));
	}

    // Use this for initialization
    void Start ()
	{
        if (TextObj==null)
        {
			TextObj = Instantiate(Resources.Load<GameObject>("Prefabs/Draw/DistanceText"));
			TextObj.transform.SetParent(BrushManager.Instance.DistanceUIParent);
			TextObj.transform.localScale = Vector3.one;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
        if (BrushManager.Instance.BrushMode)
        {
			TextObj.SetActive(true);
			ShowDistanceText();
		}
		else
        {
			TextObj.SetActive(false);
        }
	}

	public void ShowDistanceText()
    {
        if (StartPoint != null&&EndPoint != null)
        {
			if (MiddlePoint != null)
            {
                if (TextObj2==null)
                {
					TextObj2 = Instantiate(Resources.Load<GameObject>("Prefabs/Draw/DistanceText"));
					TextObj2.transform.SetParent(BrushManager.Instance.DistanceUIParent);
					TextObj2.transform.localScale = Vector3.one;
				}
				TextObj.transform.localPosition = BrushManager.Instance.WorldToUGui((StartPoint.position + MiddlePoint.position) / 2);
				TextObj.GetComponent<Text>().text = Vector3.Distance(StartPoint.position, MiddlePoint.position).ToString("F2") + "M";

				TextObj2.transform.localPosition = BrushManager.Instance.WorldToUGui((MiddlePoint.position + EndPoint.position) / 2);
				TextObj2.GetComponent<Text>().text = Vector3.Distance(MiddlePoint.position, EndPoint.position).ToString("F2") + "M";
				return;
            }
            else if (MiddlePoint==null&&TextObj2!=null)
            {
				Destroy(TextObj2);
            }
			TextObj.transform.localPosition = BrushManager.Instance.WorldToUGui((StartPoint.position + EndPoint.position) / 2);
			TextObj.GetComponent<Text>().text = Vector3.Distance(StartPoint.position, EndPoint.position).ToString("F2") + "M";
			//Debug.LogError(StartPoint.position + "    " + EndPoint.position);
		}
	}
	/// <summary>
	/// 分隔为两堵连续的墙壁
	/// </summary>
	public void Separate()
    {
		BrushManager.Instance.Separate(this);
	}
}
