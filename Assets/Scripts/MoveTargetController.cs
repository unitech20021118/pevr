using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTargetController : MonoBehaviour
{
    /// <summary>
    /// 目标物体
    /// </summary>
    private GameObject targetGameObject;
    /// <summary>
    /// 移动速度
    /// </summary>
    private float speed;
    /// <summary>
    /// 与目标物体位置的向量
    /// </summary>
    private Vector3 directionVector3;
    /// <summary>
    /// 旋转
    /// </summary>
    private Vector3 engleVector3;
    /// <summary>
    /// 重力
    /// </summary>
    private bool noGravity;
    /// <summary>
    /// 设置要前往的目标物体
    /// </summary>
    public void SetTargetGameObject(GameObject obj)
    {
        targetGameObject = obj;
    }
    /// <summary>
    /// 设置移动速度
    /// </summary>
    /// <param name="speed"></param>
    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }
    /// <summary>
    /// 设置重力
    /// </summary>
    public void SetGravity(bool noGravity)
    {
        this.noGravity = noGravity;
        AddRigidbody();
    }

    void Start()
    {
        engleVector3 = transform.eulerAngles;
    }

    void Update()
    {
        if (targetGameObject != null)
        {
            if (!noGravity)
            {
                directionVector3 = new Vector3(targetGameObject.transform.position.x, transform.position.y, targetGameObject.transform.position.z) - transform.position;
                if (directionVector3.magnitude >= 0.2f)
                {
                    transform.position += directionVector3.normalized * speed * Time.deltaTime;
                }
            }
            else
            {
                directionVector3 = targetGameObject.transform.position - transform.position;
                if (directionVector3.magnitude >= 0.2f)
                {
                    transform.position += directionVector3.normalized * speed * Time.deltaTime;
                }
            }
        }
        //if (transform.eulerAngles != engleVector3)
        //{
        //    transform.eulerAngles = engleVector3;
        //}
    }
    
    /// <summary>
    /// 添加刚体组件
    /// </summary>
    void AddRigidbody()
    {
        Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();
        if (!rigidbody)
        {
           rigidbody = gameObject.AddComponent<Rigidbody>();
        }
        rigidbody.useGravity = !noGravity;
    }
}
