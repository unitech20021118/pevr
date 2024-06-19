using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using VRTK;
using UnityEngine;

public class FollowMouseMoveControl : MonoBehaviour
{
    public Camera camera;

    private float offsetX;

    private float offsetY;

    private float distance;

    private float x;
    private float y;
    private float z;
    private bool place;
    private string xMin;
    private string xMax;
    private string yMin;
    private string yMax;
    private string zMin;
    private string zMax;

    private bool isFace;
    private bool faceX;
    private bool faceY;
    private bool faceZ;

    private Vector3 positionVector3;

    private bool down;

    private XYZ xyz;

    /// <summary>
    /// 比值
    /// </summary>
    private float yx;
    private float zx;
    private float xy;
    private float zy;
    private float xz;
    private float yz;
    //当前交互的手柄
    private Transform firstControllerTransform;

    private Transform secondControllerTransform;
    private bool clicked;
    private bool leave;
    /// <summary>
    /// 上一帧是否扣下扳机键
    /// </summary>
    private bool previousFrame;
    /// <summary>
    /// 是否是进入后再按下扳机键（这样才允许移动）
    /// </summary>
    private bool canMove;
    /// <summary>
    /// 手柄是否是按着扳机键进入物体的
    /// </summary>
    private bool firstFramCilck;
    /// <summary>
    /// 当前交互的手柄上事件脚本
    /// </summary>
    private VRTK_ControllerEvents controllerEvents;

    public Transform FirstControllerTransform
    {
        get
        {
            return firstControllerTransform;
        }

        set
        {
            firstControllerTransform = value;
            if (value != null)
            {
                controllerEvents = value.GetComponentInChildren<VRTK_ControllerEvents>();
                if (controllerEvents.triggerClicked)
                {
                    firstFramCilck = true;
                }
                else
                {
                    firstFramCilck = false;
                }
            }
            else
            {
                controllerEvents = null;
            }
        }
    }

    void Awake()
    {
        camera = Camera.main;

        x = 0f;
        y = 0f;
        z = 0f;
        SetXYZ();
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Manager.Instace.FirstPerson.activeSelf && camera != Manager.Instace.FirstPersonCamera)
        {
            camera = Manager.Instace.FirstPersonCamera;
        }
        if (controllerEvents != null)
        {Debug.LogError("11223");
            if (firstFramCilck)
            {
                if (controllerEvents.triggerClicked == false)
                {
                    firstFramCilck = false;
                }
            }
            else
            {
                if (clicked && !controllerEvents.triggerClicked)
                {
                    clicked = false;
                }
                if (leave && !controllerEvents.triggerClicked)
                {
                    FirstControllerTransform = null;
                    leave = false;
                    return;
                }//如果上一帧还没有按下扳机键
                if (!previousFrame)
                {//这一帧按下了扳机键
                    if (controllerEvents.triggerClicked)
                    {
                        canMove = true;
                        positionVector3 = transform.position;
                    }
                }
                if (!controllerEvents.triggerClicked)
                {
                    canMove = false;
                }
                previousFrame = controllerEvents.triggerClicked;
                if (canMove)
                {
                    Vector3 controllerVector3 = FirstControllerTransform.position;
                    Vector3 transformVector3;
                    //执行位置转化
                    if (isFace)
                    {
                        if (faceX)
                        {
                            transformVector3 = new Vector3(transform.position.x, controllerVector3.y, controllerVector3.z);
                        }
                        else if (faceY)
                        {
                            transformVector3 = new Vector3(controllerVector3.x, transform.position.y, controllerVector3.z);
                        }
                        else
                        {
                            transformVector3 = new Vector3(controllerVector3.x, controllerVector3.y, transform.position.z);
                        }
                    }
                    else
                    {
                        switch (xyz)
                        {
                            case XYZ.X:
                                if (x == 0f)
                                {
                                    transformVector3 = transform.position;
                                }
                                else
                                {
                                    SetPlace(ref controllerVector3, xyz);
                                    transformVector3 = new Vector3(controllerVector3.x, (controllerVector3.x - positionVector3.x) * yx + positionVector3.y, (controllerVector3.x - positionVector3.x) * zx + positionVector3.z);
                                }
                                break;
                            case XYZ.Y:
                                SetPlace(ref controllerVector3, xyz);
                                transformVector3 = new Vector3((controllerVector3.y - positionVector3.y) * xy + positionVector3.x, controllerVector3.y, (controllerVector3.y - positionVector3.y) * zy + positionVector3.z);
                                break;
                            case XYZ.Z:
                                SetPlace(ref controllerVector3, xyz);
                                transformVector3 = new Vector3((controllerVector3.z - positionVector3.z) * xz + positionVector3.x, (controllerVector3.z - positionVector3.z) * yz + positionVector3.y, controllerVector3.z);
                                break;
                            default:
                                transformVector3 = transform.position;
                                break;
                        }
                    }
                    transform.position = transformVector3;
                }
            }
        }
    }

    private bool mouseCanMove;
    public IEnumerator Dowait(float x, float y, float z, bool place, string xmin, string xmax, string ymin, string ymax, string zmin, string zmax, bool isface, bool facex, bool facey, bool facez)
    {
        while (Input.GetMouseButton(0))
        {
            yield return null;
        }
        InitData(x, y, z, place, xmin, xmax, ymin, ymax, zmin, zmax, isface, facex, facey, facez);
        mouseCanMove = true;

    }
    /// <summary>
    /// 初始化数据
    /// </summary>
    public void InitData(float x, float y, float z, bool place, string xmin, string xmax, string ymin, string ymax, string zmin, string zmax, bool isface, bool facex, bool facey, bool facez)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.place = place;
        xMin = xmin;
        xMax = xmax;
        yMin = ymin;
        yMax = ymax;
        zMin = zmin;
        zMax = zmax;
        isFace = isface;
        faceX = facex;
        faceY = facey;
        faceZ = facez;
        SetXYZ();
    }
    void OnMouseDrag()
    {
        if (mouseCanMove)
        {
            Vector3 tarMousePos = camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x + offsetX, Input.mousePosition.y + offsetY, distance));
            Vector3 transformVector3;
            if (isFace)
            {
                if (faceX)
                {
                    transformVector3 = new Vector3(positionVector3.x, tarMousePos.y, tarMousePos.z);
                }
                else if (faceY)
                {
                    transformVector3 = new Vector3(tarMousePos.x, positionVector3.y, tarMousePos.z);
                }
                else
                {
                    transformVector3 = new Vector3(tarMousePos.x, tarMousePos.y, positionVector3.z);
                }
            }
            else
            {
                switch (xyz)
                {
                    case XYZ.X:
                        if (x == 0f)
                        {
                            transformVector3 = transform.position;
                        }
                        else
                        {
                            SetPlace(ref tarMousePos, xyz);
                            transformVector3 = new Vector3(tarMousePos.x, (tarMousePos.x - positionVector3.x) * yx + positionVector3.y, (tarMousePos.x - positionVector3.x) * zx + positionVector3.z);
                        }
                        break;
                    case XYZ.Y:
                        SetPlace(ref tarMousePos, xyz);
                        transformVector3 = new Vector3((tarMousePos.y - positionVector3.y) * xy + positionVector3.x, tarMousePos.y, (tarMousePos.y - positionVector3.y) * zy + positionVector3.z);
                        break;
                    case XYZ.Z:
                        SetPlace(ref tarMousePos, xyz);
                        transformVector3 = new Vector3((tarMousePos.z - positionVector3.z) * xz + positionVector3.x, (tarMousePos.z - positionVector3.z) * yz + positionVector3.y, tarMousePos.z);
                        break;
                    default:
                        transformVector3 = transform.position;
                        break;
                }
            }


            transform.position = transformVector3;
        }
        
    }

    private void SetPlace(ref Vector3 transVector3, XYZ xyz)
    {
        switch (xyz)
        {
            case XYZ.X:
                //求x的限制区间
                if (place)
                {
                    float num;
                    if (xMin != "")
                    {
                        num = float.Parse(xMin);
                        if (transVector3.x < num)
                        {
                            transVector3.x = num;
                        }
                    }
                    if (xMax != "")
                    {
                        num = float.Parse(xMax);
                        if (transVector3.x > num)
                        {
                            transVector3.x = num;
                        }
                    }
                    if (yMin != "")
                    {
                        num = float.Parse(yMin);
                        float num1 = (num - positionVector3.y) / yx + positionVector3.x;
                        if (yx < 0)
                        {
                            if (transVector3.x > num1)
                            {
                                transVector3.x = num1;
                            }
                        }
                        else
                        {
                            if (transVector3.x < num1)
                            {
                                transVector3.x = num1;
                            }
                        }

                    }
                    if (yMax != "")
                    {
                        num = float.Parse(yMax);
                        float num1 = (num - positionVector3.y) / yx + positionVector3.x;
                        if (yx < 0)
                        {
                            if (transVector3.x < num1)
                            {
                                transVector3.x = num1;
                            }
                        }
                        else
                        {
                            if (transVector3.x > num1)
                            {
                                transVector3.x = num1;
                            }
                        }

                    }
                    if (zMin != "")
                    {
                        num = float.Parse(zMin);
                        float num1 = (num - positionVector3.z) / zx + positionVector3.x;
                        if (zx < 0)
                        {
                            if (transVector3.x > num1)
                            {
                                transVector3.x = num1;
                            }
                        }
                        else
                        {
                            if (transVector3.x < num1)
                            {
                                transVector3.x = num1;
                            }
                        }

                    }
                    if (zMax != "")
                    {
                        num = float.Parse(zMax);
                        float num1 = (num - positionVector3.z) / zx + positionVector3.x;
                        if (zx < 0)
                        {
                            if (transVector3.x < num1)
                            {
                                transVector3.x = num1;
                            }
                        }
                        else
                        {
                            if (transVector3.x > num1)
                            {
                                transVector3.x = num1;
                            }
                        }

                    }
                }
                break;
            case XYZ.Y:
                //求Y的限制区间
                if (place)
                {
                    float num;
                    if (xMin != "")
                    {
                        num = float.Parse(xMin);
                        float num1 = (num - positionVector3.x) / xy + positionVector3.y;
                        if (xy < 0)
                        {
                            if (transVector3.y > num1)
                            {
                                transVector3.y = num1;
                            }
                        }
                        else
                        {
                            if (transVector3.y < num1)
                            {
                                transVector3.y = num1;
                            }
                        }

                    }
                    if (xMax != "")
                    {
                        num = float.Parse(xMax);
                        float num1 = (num - positionVector3.x) / xy + positionVector3.y;
                        if (xy < 0)
                        {
                            if (transVector3.y < num1)
                            {
                                transVector3.y = num1;
                            }
                        }
                        else
                        {
                            if (transVector3.y > num1)
                            {
                                transVector3.y = num1;
                            }
                        }

                    }
                    if (yMin != "")
                    {
                        num = float.Parse(yMin);
                        if (transVector3.y < num)
                        {
                            transVector3.y = num;
                        }
                    }
                    if (yMax != "")
                    {
                        num = float.Parse(yMax);
                        if (transVector3.y > num)
                        {
                            transVector3.y = num;
                        }
                    }
                    if (zMin != "")
                    {
                        num = float.Parse(zMin);
                        float num1 = (num - positionVector3.z) / zy + positionVector3.z;
                        if (zy < 0)
                        {
                            if (transVector3.y > num1)
                            {
                                transVector3.y = num1;
                            }
                        }
                        else
                        {
                            if (transVector3.y < num1)
                            {
                                transVector3.y = num1;
                            }
                        }

                    }
                    if (zMax != "")
                    {
                        num = float.Parse(zMax);
                        float num1 = (num - positionVector3.z) / zy + positionVector3.z;
                        if (zy < 0)
                        {
                            if (transVector3.y < num1)
                            {
                                transVector3.y = num1;
                            }
                        }
                        if (transVector3.y > num1)
                        {
                            transVector3.y = num1;
                        }
                    }
                }
                break;
            case XYZ.Z:
                //求z的限制区间
                if (place)
                {
                    float num;
                    if (xMin != "")
                    {
                        num = float.Parse(xMin);
                        float num1 = (num - positionVector3.x) / xz + positionVector3.y;
                        if (xz < 0)
                        {
                            if (transVector3.z > num1)
                            {
                                transVector3.z = num1;
                            }
                        }
                        else
                        {
                            if (transVector3.z < num1)
                            {
                                transVector3.z = num1;
                            }
                        }

                    }
                    if (xMax != "")
                    {
                        num = float.Parse(xMax);
                        float num1 = (num - positionVector3.x) / xz + positionVector3.y;
                        if (xz < 0)
                        {
                            if (transVector3.z < num1)
                            {
                                transVector3.z = num1;
                            }
                        }
                        else
                        {
                            if (transVector3.z > num1)
                            {
                                transVector3.z = num1;
                            }
                        }

                    }
                    if (yMin != "")
                    {
                        num = float.Parse(yMin);
                        float num1 = (num - positionVector3.y) / yz + positionVector3.x;
                        if (yz < 0)
                        {
                            if (transVector3.z > num1)
                            {
                                transVector3.z = num1;
                            }
                        }
                        else
                        {
                            if (transVector3.z < num1)
                            {
                                transVector3.z = num1;
                            }
                        }

                    }
                    if (yMax != "")
                    {
                        num = float.Parse(yMax);
                        float num1 = (num - positionVector3.y) / yz + positionVector3.x;
                        if (yz < 0)
                        {
                            if (transVector3.z < num1)
                            {
                                transVector3.z = num1;
                            }
                        }
                        else
                        {
                            if (transVector3.z > num1)
                            {
                                transVector3.z = num1;
                            }
                        }
                    }
                    if (zMin != "")
                    {
                        num = float.Parse(zMin);
                        if (transVector3.z < num)
                        {
                            transVector3.z = num;
                        }
                    }
                    if (zMax != "")
                    {
                        num = float.Parse(zMax);
                        if (transVector3.z > num)
                        {
                            transVector3.z = num;
                        }
                    }
                }
                break;
            default:
                break;
        }

    }
    void OnMouseDown()
    {
        if (!down)
        {
            positionVector3 = transform.position;
            down = true;
        }
        Vector3 objScreenPos = camera.WorldToScreenPoint(transform.position);
        offsetX = objScreenPos.x - Input.mousePosition.x;
        offsetY = objScreenPos.y - Input.mousePosition.y;
        Vector3 objToCam = transform.position - camera.transform.position;
        float angle = Vector3.Angle(camera.transform.forward, objToCam);
        distance = Mathf.Cos(angle * Mathf.PI / 180f) * objToCam.magnitude;
    }

    void OnMouseUp()
    {
        down = false;
    }

    public void SetXYZ()
    {
        float xx = Mathf.Abs(x);
        float yy = Mathf.Abs(y);
        float zz = Mathf.Abs(z);
        if (xx >= yy)
        {
            if (xx >= zz)
            {
                xyz = XYZ.X;
                if (x != 0f)
                {
                    yx = y / x;
                    zx = z / x;
                }
            }
            else
            {
                xyz = XYZ.Z;
                xz = x / z;
                yz = y / z;
            }
        }
        else
        {
            if (yy >= zz)
            {
                xyz = XYZ.Y;
                xy = x / y;
                zy = z / y;
            }
            else
            {
                xyz = XYZ.Z;
                xz = x / z;
                yz = y / z;
            }
        }
    }

    public void OnTriggerEnter(Collider collider)
    {
        if (leave)
        {
            if (collider.transform.parent.parent.parent == FirstControllerTransform)
            {
                leave = false;
            }
            return;
        }
        if (collider.gameObject.name == "Head" && collider.transform.parent.name == "VRTK_ControllerCollidersContainer")
        {
            if (controllerEvents == null)
            {
                if (FirstControllerTransform == null)
                {
                    FirstControllerTransform = collider.transform.parent.parent.parent;
                }
                else if (FirstControllerTransform != null && secondControllerTransform == null)
                {
                    secondControllerTransform = collider.transform.parent.parent.parent;
                }
            }
        }
    }

    public void OnTriggerExit(Collider collider)
    {
        if (FirstControllerTransform != null && secondControllerTransform != null)
        {
            if (collider.transform.parent.parent.parent == FirstControllerTransform)
            {
                FirstControllerTransform = secondControllerTransform;
                secondControllerTransform = null;
            }
            else if (collider.transform.parent.parent.parent == secondControllerTransform)
            {
                secondControllerTransform = null;
            }
        }
        if (FirstControllerTransform != null && secondControllerTransform == null)
        {
            if (collider.transform.parent.parent.parent == FirstControllerTransform && !controllerEvents.triggerClicked)
            {
                FirstControllerTransform = null;
            }
            else if (collider.transform.parent.parent.parent == FirstControllerTransform && controllerEvents.triggerClicked)
            {
                if (!firstFramCilck)
                {
                    leave = true;
                }
            }
        }
    }

    private enum XYZ
    {
        X, Y, Z
    }
}
