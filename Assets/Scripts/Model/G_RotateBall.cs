using System;
using UnityEngine;
//控制旋转
public class G_RotateBall : MonoBehaviour
{
    public GameObject xAxis;

    public GameObject yAxis;

    public GameObject zAxis;

    public Color chooseColor;

    private GameObject _target;

    private Vector3 _clickMousePos=Vector3.zero;

    private GameObject _chooseAxis;

    private G_RotateBallColor xAxisColor;

    private G_RotateBallColor yAxisColor;

    private G_RotateBallColor zAxisColor;

    private struct Temp 
    {
       public Vector3 pos;
       public Vector3 rot;
       public Vector3 scale;
    }
    Temp temp;

    //Vector3 oldMousePos = Vector3.zero;
    /// <summary>
    /// 设定旋转目标
    /// </summary>
    /// <param name="target"></param>
    public void SetTarget(GameObject target)
    {
        this._target = target;
        this.AdjustPos();
    }

    public void SetTargetTwo(GameObject target)
    {
        this._target = target;
        base.transform.position = this._target.transform.position;
    }

    /// <summary>
    /// 放下旋转目标
    /// </summary>
    public void DropTarget()
    {
        _target = null;
        
    }

    private void Start()
    {
        this.SetMouseListener(this.xAxis);
        this.SetMouseListener(this.yAxis);
        this.SetMouseListener(this.zAxis);
        this.xAxisColor = this.xAxis.GetComponent<G_RotateBallColor>();
        this.xAxisColor.SetChooseColor(this.chooseColor);
        this.yAxisColor = this.yAxis.GetComponent<G_RotateBallColor>();
        this.yAxisColor.SetChooseColor(this.chooseColor);
        this.zAxisColor = this.zAxis.GetComponent<G_RotateBallColor>();
        this.zAxisColor.SetChooseColor(this.chooseColor);
        temp.pos = transform.position;
        temp.rot = transform.eulerAngles;
        temp.scale = transform.localScale;
    }

    public void Reset()
    {
        transform.position = temp.pos;
        transform.eulerAngles = temp.rot;
        transform.localScale = temp.scale;
    }

    /// <summary>
    /// 添加委托事件
    /// </summary>
    /// <param name="target"></param>
    private void SetMouseListener(GameObject target)
    {
        G_ObjectMouseListener component = target.GetComponent<G_ObjectMouseListener>();
        if (component)
        {
            G_ObjectMouseListener expr_13 = component;

            expr_13.onClickDown += onClickDownAxis;
            expr_13.onClickUp += onClickUpAxis;
        }
    }



    private void Update()
    {
        if (this._chooseAxis != null&&Manager.Instace._IsAlt)
        {
            this.RotateTarget();
        }
        this.AdjustPos();
    }

    /// <summary>
    /// 点击到某一轴，对它进行上色
    /// </summary>
    /// <param name="obj"></param>
    private void onClickDownAxis(GameObject obj)
    {
        this.xAxisColor.SetHaveOneChoose(true);
        this.yAxisColor.SetHaveOneChoose(true);
        this.zAxisColor.SetHaveOneChoose(true);
        this._chooseAxis = obj;

        Vector3 vector = Camera.main.WorldToScreenPoint(transform.position);
        this._clickMousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, vector.z));
        
    }

    /// <summary>
    /// 鼠标抬起，退色
    /// </summary>
    /// <param name="obj"></param>
    private void onClickUpAxis(GameObject obj)
    {
        this.xAxisColor.SetHaveOneChoose(false);
        this.yAxisColor.SetHaveOneChoose(false);
        this.zAxisColor.SetHaveOneChoose(false);
        this._chooseAxis = null;
    }

    /// <summary>
    /// 使旋转球与旋转的物体对象位置相同   旋转角度也相同
    /// </summary>
    private void AdjustPos()
    {
        if (this._target == null)
        {
            return;
        }
        base.transform.position = this._target.transform.position;

        //base.transform.rotation = this._target.transform.rotation;
        //base.transform.rotation = _target.transform.rotation;
    }

    /// <summary>
    /// 使物体在X、Y、Z轴上旋转
    /// </summary>
    //private void RotateTarget()
    //{
    //    Vector3 mousePosition = Input.mousePosition;
    //    if (this._chooseAxis == this.xAxis)
    //    {
    //        float angle = (mousePosition.x - this._clickMousePos.x) / 2f;
    //        this._target.transform.Rotate(new Vector3(0f, -1f, 0f), angle);
    //        base.transform.Rotate(new Vector3(0f, -1f, 0f), angle);
    //    }
    //    else if (this._chooseAxis == this.yAxis)
    //    {
    //        float angle2 = (mousePosition.y - this._clickMousePos.y) / 2f;
    //        this._target.transform.Rotate(new Vector3(-1f, 0f, 0f), angle2);
    //        base.transform.Rotate(new Vector3(-1f, 0f, 0f), angle2);
    //    }
    //    else if (this._chooseAxis == this.zAxis)
    //    {
    //        float angle3 = (mousePosition.y - this._clickMousePos.y) / 2f;
    //        this._target.transform.Rotate(new Vector3(0f, 0f, 1f), angle3);
    //        base.transform.Rotate(new Vector3(0f, 0f, 1f), angle3);
    //    }

       
    //    Manager.Instace.transform.GetComponent<G_EditorTarget>().positionCenter.transform.rotation = _target.transform.rotation;
    //    G_TranformUI.Instance.SetObj(this._target);
    //    this._clickMousePos = mousePosition;
    //}

    private void RotateTarget()
    {
        
        Vector3 vector = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 vector2 = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, vector.z));
        if (this._chooseAxis == this.xAxis)
        {
            //GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //obj.transform.position = vector2;
            Vector3 dir = vector2 - _clickMousePos;
            float dot = Vector3.Dot(transform.right, Camera.main.transform.forward);            
            Vector3 c =  dot * (Camera.main.transform.forward);
            Vector3 result = transform.right -c;
            float app = Vector3.Dot(result, Camera.main.transform.forward);
            //Debug.Log("Result:" + app);
            //Debug.Log("First:" +  result.normalized);
            //Debug.Log("Second" + Camera.main.transform.forward);
            //GameObject line = new GameObject();
            //LineRenderer lr = line.AddComponent<LineRenderer>();
            //lr.SetPosition(0, transform.position);
            //lr.SetPosition(1, result);
            float dot2 = Vector3.Dot(result, dir);
            Vector3 d = dot2 * result.normalized;            
            Vector3 temp = dir - d;

            Vector3 faxiang = Vector3.Cross(result, temp);
            float angle = 0;
            if ((faxiang.normalized + Camera.main.transform.forward) == Vector3.zero)
            {
                Debug.Log("顺时针旋转");
                angle = -Vector3.Magnitude(temp);
            }
            else
            {
                Debug.Log("逆时针旋转");
                angle = Vector3.Magnitude(temp);
            }
            this._target.transform.RotateAround(transform.position, transform.right, angle*8 );
            base.transform.RotateAround(transform.position, transform.right, angle*8 );
            this._clickMousePos = vector2;
        }
        else if (this._chooseAxis == this.yAxis)
        {
            Vector3 dir = vector2 - _clickMousePos;
            float dot = Vector3.Dot(transform.up, Camera.main.transform.forward);
            Vector3 c = dot * (Camera.main.transform.forward);
            Vector3 result = transform.up - c;
            float dot2 = Vector3.Dot(result, dir);
            Vector3 d = dot2 * Vector3.Normalize(result);
            Vector3 temp = dir - d;

            Vector3 faxiang = Vector3.Cross(result, temp);
            float angle = 0;
            if ((faxiang.normalized + Camera.main.transform.forward) == Vector3.zero)
            {
                Debug.Log("顺时针旋转");
                angle = -Vector3.Magnitude(temp);
            }
            else
            {
                Debug.Log("逆时针旋转");
                angle = Vector3.Magnitude(temp);
            }
            this._target.transform.Rotate(transform.up, angle * 8, Space.World);
            base.transform.Rotate(transform.up, angle * 8, Space.World);
            this._clickMousePos = vector2;
        }
        else if (this._chooseAxis == this.zAxis)
        {
            Vector3 dir = vector2 - _clickMousePos;
            float dot = Vector3.Dot(transform.forward, Camera.main.transform.forward);
            Vector3 c = dot * (Camera.main.transform.forward);
            Vector3 result = transform.forward- c;
            float dot2 = Vector3.Dot(result, dir);
            Vector3 d = dot2 * Vector3.Normalize(result);
            Vector3 temp = dir - d;
            Vector3 faxiang = Vector3.Cross(result, temp);
            float angle = 0;
            if ((faxiang.normalized + Camera.main.transform.forward) == Vector3.zero)
            {
                Debug.Log("顺时针旋转");
                angle = -Vector3.Magnitude(temp);
            }
            else
            {
                Debug.Log("逆时针旋转");
                angle = Vector3.Magnitude(temp);
            }
            this._target.transform.Rotate(transform.forward, angle * 8, Space.World);
            base.transform.Rotate(transform.forward, angle * 8, Space.World);
            this._clickMousePos = vector2;
        }


        Manager.Instace.transform.GetComponent<G_EditorTarget>().positionCenter.transform.rotation = _target.transform.rotation;
        G_TranformUI.Instance.SetObj(this._target);
        

        

    }
}
