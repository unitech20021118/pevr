using System;
using UnityEngine;
//控制旋转
public class G_ScaleAxis : MonoBehaviour
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

    /// <summary>
    /// 设定旋转目标
    /// </summary>
    /// <param name="target"></param>
    public void SetTarget(GameObject target)
    {
        this._target = target;
        this.AdjustPos();
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
        this._clickMousePos = Input.mousePosition;
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
        transform.rotation = _target.transform.rotation;
        //base.transform.rotation = _target.transform.rotation;
    }

    /// <summary>
    /// 使物体在X、Y、Z轴上旋转
    /// </summary>
    private void RotateTarget()
    {
        Vector3 mousePosition = Input.mousePosition;
        if (this._chooseAxis == this.xAxis)
        {
            float angle = (mousePosition.x - this._clickMousePos.x) / 20f;
            Vector3 temp = new Vector3(_target.transform.localScale.x - angle, _target.transform.localScale.y, _target.transform.localScale.z);
            this._target.transform.localScale = temp;
        }
        else if (this._chooseAxis == this.yAxis)
        {
            float angle2 = (mousePosition.y - this._clickMousePos.y) / 20f;
            Vector3 temp = new Vector3(_target.transform.localScale.x, _target.transform.localScale.y + angle2, _target.transform.localScale.z);
            this._target.transform.localScale = temp;
        }
        else if (this._chooseAxis == this.zAxis)
        {
            float angle3 = (mousePosition.x - this._clickMousePos.x) / 20f;
            Vector3 temp = new Vector3(_target.transform.localScale.x, _target.transform.localScale.y, _target.transform.localScale.z + angle3);
            this._target.transform.localScale = temp;
        }
        G_TranformUI.Instance.SetObj(this._target);
        this._clickMousePos = mousePosition;
    }
}
