using System;
using UnityEngine;
//控制旋转
public class G_PositionAxis: MonoBehaviour
{
    public GameObject xAxis;

    public GameObject yAxis;

    public GameObject zAxis;

    public Color chooseColor;

    private GameObject _target;

    private Vector3 _clickMousePos;

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

    Vector3 oldMousePos = new Vector3(0, 0, 0);
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
        if (this._chooseAxis != null && Manager.Instace._IsAlt)
        {
            this.RotateTarget2();
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
        //Vector3 objPos = _target.transform.position;
        //Vector3 temp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //this._clickMousePos = new Vector3(temp.x, temp.y, objPos.z);
        Vector3 objPos = Camera.main.WorldToScreenPoint(_target.transform.position);
        Vector3 temp = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, objPos.z));
        this._clickMousePos = temp;
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
    /// 使旋转球与旋转的物体对象位置相同
    /// </summary>
    private void AdjustPos()
    {
        if (this._target == null)
        {
            return;
        }
        base.transform.position = this._target.transform.position;
        //_target.transform.position = transform.position;
        //base.transform.rotation = _target.transform.rotation;
    }

    /// <summary>
    /// 使物体在X、Y、Z轴上
    /// </summary>
    private void RotateTarget3()
    {
        Vector3 mousePosition = Input.mousePosition;
        if (this._chooseAxis == this.xAxis)
        {
            float delta = (mousePosition.y - this._clickMousePos.y) / 20f;
            transform.Translate(new Vector3(delta,0,0));
        }
        else if (this._chooseAxis == this.yAxis)
        {
            float delta = (mousePosition.y - this._clickMousePos.y) / 20f;
            transform.Translate(new Vector3(0, delta, 0));
        }
        else if (this._chooseAxis == this.zAxis)
        {
            float delta = (mousePosition.x - this._clickMousePos.x) / 20f;
            transform.Translate(new Vector3(0, 0, delta));
        }
        this._clickMousePos = mousePosition;
    }

    private void RotateTarget()
    {
        Vector3 objPos =Camera.main.WorldToScreenPoint( _target.transform.position);
        Vector3 temp = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,objPos.z));
        Vector3 mousePosition = temp;
        if (this._chooseAxis == this.xAxis)
        {
            Vector3 move=Caculate(_clickMousePos,mousePosition);//鼠标在世界坐标下移动的向量
            Vector3 normalizeX=Vector3.Normalize(Caculate(_chooseAxis.transform.position,transform.position));//在世界坐标下X轴的向量
            float delta = Vector3.Dot(normalizeX, move);
            //transform.Translate(new Vector3(delta, 0, 0));
            transform.position = new Vector3(transform.position.x - delta, transform.position.y, transform.position.z);
        }
        else if (this._chooseAxis == this.yAxis)
        {
            Vector3 move = Caculate(_clickMousePos, mousePosition);//鼠标在世界坐标下移动的向量
            Vector3 normalizeY = Vector3.Normalize(Caculate(_chooseAxis.transform.position, transform.position));//在世界坐标下X轴的向量
            float delta = Vector3.Dot(move, normalizeY);
            //transform.Translate(new Vector3(0, delta, 0));
            transform.position = new Vector3(transform.position.x, transform.position.y - delta, transform.position.z);
        }
        else if (this._chooseAxis == this.zAxis)
        {
            Vector3 move = Caculate(_clickMousePos, mousePosition);//鼠标在世界坐标下移动的向量
            Vector3 normalizeZ = Vector3.Normalize(Caculate(_chooseAxis.transform.position, transform.position));//在世界坐标下X轴的向量
            float delta = Vector3.Dot(move, normalizeZ);
            //transform.Translate(new Vector3(0, 0, delta));
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - delta);
        }
        this._clickMousePos = mousePosition;
        _target.transform.position = transform.position;
    }

    private void RotateTarget2()
    {
        Vector3 vector = Camera.main.WorldToScreenPoint(_target.transform.position);
        Vector3 vector2 = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, vector.z));
        if (this._chooseAxis == this.xAxis)
        {
            Vector3 dir = (vector2 - oldMousePos).normalized;
            float dot = Vector3.Dot(transform.right, dir);
            float mouseY = Input.GetAxis("Mouse Y");
            float mouseX = Input.GetAxis("Mouse X");
            Vector3 mouseXY = new Vector3(mouseX, mouseY, 0);
            float distance = Vector3.Distance(Camera.main.transform.position, transform.position);
            float d = Vector3.Magnitude(mouseXY) * distance * 0.07f;
            Vector3 position = _target.transform.position + xAxis.transform.forward * dot * d;
            _target.transform.position = position;
            G_TranformUI.Instance.SetObj(_target);
            transform.position = _target.transform.position;
            oldMousePos = vector2;
        }
        else if (this._chooseAxis == this.yAxis)
        {
            Vector3 dir = (vector2 - oldMousePos).normalized;
            float dot = Vector3.Dot(transform.up, dir);
            float mouseY = Input.GetAxis("Mouse Y");
            float mouseX = Input.GetAxis("Mouse X");
            Vector3 mouseXY = new Vector3(mouseX, mouseY, 0);
            float distance = Vector3.Distance(Camera.main.transform.position, transform.position);//照相机到坐标轴的距离
            float d = Vector3.Magnitude(mouseXY) * distance * 0.07f;
            Vector3 position = _target.transform.position + yAxis.transform.forward * dot * d;
            _target.transform.position = position;
            G_TranformUI.Instance.SetObj(_target);
            transform.position = _target.transform.position;
            oldMousePos = vector2;
        }
        else if (this._chooseAxis == this.zAxis)
        {
            Vector3 dir = (vector2 - oldMousePos).normalized;
            float dot = Vector3.Dot(transform.forward, dir);
            float mouseY = Input.GetAxis("Mouse Y");
            float mouseX = Input.GetAxis("Mouse X");
            Vector3 mouseXY = new Vector3(mouseX, mouseY, 0);
            float distance = Vector3.Distance(Camera.main.transform.position, transform.position);//照相机到坐标轴的距离
            float d = Vector3.Magnitude(mouseXY) * distance * 0.07f;
            Vector3 position = _target.transform.position + zAxis.transform.forward * dot * d;
            _target.transform.position = position;
            G_TranformUI.Instance.SetObj(_target);
            transform.position = _target.transform.position;
            oldMousePos = vector2;
        }
    }

    private Vector3 Caculate(Vector3 a, Vector3 b)
    {
        Vector3 temp;
        temp.x = b.x - a.x;
        temp.y = b.y - a.y;
        temp.z = b.z - a.z;
        return temp;
    }
}
