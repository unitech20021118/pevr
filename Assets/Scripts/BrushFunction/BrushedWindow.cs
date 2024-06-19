using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrushedWindow : MonoBehaviour 
{

    /// <summary>
    /// 该窗户所在的墙壁
    /// </summary>
    public BrushedWall wall;
    /// <summary>
    /// 长度
    /// </summary>
    public float length;
    /// <summary>
    /// 宽度
    /// </summary>
    public float width;
    /// <summary>
    /// 高度
    /// </summary>
    public float height;
    public float heightAboveGround;
    /// <summary>
    /// 该窗户的样式
    /// </summary>
    public string Style;
    public float distanceScale;
    /// <summary>
    /// 窗户的位置到墙壁起点的距离和墙壁长度的比例(以此作为门在墙壁上实际位置的标准)
    /// </summary>
    public float DistanceScale
    {
        get
        {
            if (distanceScale == 0)
            {
                distanceScale = Vector3.Distance(wall.StartPoint.localPosition, transform.localPosition) / Vector3.Distance(wall.EndPoint.localPosition, wall.StartPoint.localPosition);
            }
            return distanceScale;
        }
        set
        {
            distanceScale = value;
        }
    }

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
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, value);
            ResetDoor();
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
            transform.localScale = new Vector3(value, transform.localScale.y, transform.localScale.z);
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
            transform.localScale = new Vector3(transform.localScale.x, value, transform.localScale.z);
            ResetDoor();
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
            ResetDoor();
        }
    }

    public void Init(BrushedWall brushedWall)
    {
        wall = brushedWall;
        //DistanceScale = Vector3.Distance(brushedWall.StartPoint.localPosition, transform.localPosition) / Vector3.Distance(brushedWall.EndPoint.localPosition, brushedWall.StartPoint.localPosition);
    }
    public void InitInformation(float length, float width, float height, float heightAboveGround, float distanceScale)
    {
        this.length = length;
        this.width = width;
        this.height = height;
        this.heightAboveGround = heightAboveGround;
        this.DistanceScale = distanceScale;
    }
    /// <summary>
    /// 窗户的长宽高以及位置属性发生变化时重新从墙上挖洞放置窗户
    /// </summary>
    public void ResetDoor()
    {
        BooleanRtManager.Instance.ResetDoorsAndWindowsOfWall(wall);
    }
}
