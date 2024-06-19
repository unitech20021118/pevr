using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 离地高度和实际位置的Y值的转换
/// </summary>
public static class NumericalConversion 
{

    public static float GetDistance(Transform target)
    {
        Collider collider = target.GetComponent<Collider>();
        if (collider != null)
        {
            return Vector3.Distance(collider.ClosestPointOnBounds(collider.transform.position + new Vector3(0, -1000f, 0)), target.position);
        }
        else
        {
            return 0f;
        }
    }
    /// <summary>
    /// 获取物体的离地高度
    /// </summary>
    /// <param name="obj"></param>
	public static float GetHeightAboveGround(Transform target)
    {
        return target.localPosition.y - GetDistance(target);
    }
    /// <summary>
    /// 根据离地高度计算物体的实际位置
    /// </summary>
    /// <returns></returns>
    public static float GetTargetPositionYByHeight(Transform target,float height)
    {
        return height + GetDistance(target);
    }


    public static float GetLength(Transform target)
    {
        return target.GetComponent<Renderer>().bounds.size.x;
    }

    public static float GetHeight(Transform target)
    {
        return target.GetComponent<Renderer>().bounds.size.y;
    }
    public static float GetWidth(Transform target)
    {
        return target.GetComponent<Renderer>().bounds.size.z;
    }

    public static Vector3 MySerializableVector3ToV3(MySerializableVector3 mySerializableVector3)
    {
        return new Vector3(mySerializableVector3.X, mySerializableVector3.Y, mySerializableVector3.Z);
    }
    public static MySerializableVector3 V3ToMySerializableVector3(Vector3 vector3)
    {
        return new MySerializableVector3(vector3.x, vector3.y, vector3.z);
    }

    public static Vector3[] MySerializableVector3ToV3(MySerializableVector3[] mySerializableVector3s)
    {
        Vector3[] vector3s = new Vector3[mySerializableVector3s.Length];
        for (int i = 0; i < mySerializableVector3s.Length; i++)
        {
            vector3s[i] = MySerializableVector3ToV3(mySerializableVector3s[i]);
        }
        return vector3s;
    }
    public static MySerializableVector3[] V3ToMySerializableVector3(Vector3[] vector3s)
    {
        MySerializableVector3[] mySerializableVector3s = new  MySerializableVector3[vector3s.Length];
        for (int i = 0; i < vector3s.Length; i++)
        {
            mySerializableVector3s[i] = V3ToMySerializableVector3(vector3s[i]);
        }
        return mySerializableVector3s;
    }


}
