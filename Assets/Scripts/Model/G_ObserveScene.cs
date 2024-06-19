using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TDTK;
using UnityEngine.UI;

public class G_ObserveScene : MonoBehaviour
{
    Transform cameraTransform;
    bool isRMouseDow;
    bool isLMouseDow;
    bool isScrollDown;
    Vector3 originPos;
    Vector3 offset;
    private G_EditorTarget _isChose;
    public GameObject center;
    public bool IsMove = true;
    public static G_ObserveScene Instance;
    /// <summary>
    /// 移动速度
    /// </summary>
    private float Speed;
    /// <summary>
    /// 显示移动速度的文本输入框
    /// </summary>
    private InputField speedInputField;

    public InputField SpeedInputField
    {
        get
        {
            if (speedInputField == null)
            {
                speedInputField = GameObject.Find("Canvas").transform.FindChild("topui/right/set/set_panel/set_box/see_view_speed/SpeedInputField")
                    .GetComponent<InputField>();
            }
            return speedInputField;
        }
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        _isChose = Manager.Instace.gameObject.GetComponent<G_EditorTarget>();

        cameraTransform = Camera.main.transform;
        G_MouseListener.GetInstance().AddRMouseDown(RightMouseDown);
        G_MouseListener.GetInstance().AddRMouseUp(RightMouseUp);
        G_MouseListener.GetInstance().AddLMouseDown(LeftMouseDown);
        G_MouseListener.GetInstance().AddLMouseUp(LeftMouseUp);
        G_MouseListener.GetInstance().AddScrollMouseDown(ScrollMouseDown);
        G_MouseListener.GetInstance().AddScrollMouseUp(ScrollMouseUp);
        offset = center.transform.position - transform.position;
        if (PlayerPrefs.HasKey("Speed"))
        {
            Speed = PlayerPrefs.GetFloat("Speed");
        }
        else
        {
            Speed = 5f;
        }
        SpeedInputField.text = Speed.ToString();


    }
    void ScrollMouseDown()
    {
        isScrollDown = true;
        originPos = Input.mousePosition;
    }

    void ScrollMouseUp()
    {
        isScrollDown = false;
    }

    void RightMouseUp()
    {
        isRMouseDow = false;
    }

    void RightMouseDown()
    {
        isRMouseDow = true;
        originPos = Input.mousePosition;
    }

    void LeftMouseUp()
    {
        isLMouseDow = false;
    }

    void LeftMouseDown()
    {
        isLMouseDow = true;
        originPos = Input.mousePosition;
    }

    void Update()
    {
        //Debug.LogError("555555");
        //if (!UItdtk.IsCursorOnUI(-1))
        //{
        MouseWork();
    }


    /// <summary>
    /// 输入文字时
    /// </summary>
    public void OnInput()
    {
        IsMove = false;
    }
    /// <summary>
    /// 结束输入文字时
    /// </summary>
    public void EndInput()
    {
        IsMove = true;
    }


    void MouseWork()
    {

        //if ( Input.GetKeyDown(KeyCode.Tab))
        //{

        if (isLMouseDow)
        {

            if (Input.GetKey(KeyCode.LeftAlt))
            {
                Manager.Instace._IsAlt = false;
                Vector3 temp1 = Input.mousePosition - originPos;
                //通过两点获得法线和距离
                if (temp1.x != 0)
                {
                    //Vector3 temp2 = Vector3.up - originPos;
                    //Vector3 axis = Vector3.Cross(temp1, temp2);
                    float angle = temp1.x / offset.magnitude;
                    if (_isChose.isChose)
                    {
                        if (_isChose.moveTarget == null)
                        {
                            Camera.main.transform.RotateAround(_isChose.moveTargetPI.transform.position, Vector3.up, angle);
                        }
                        else
                        {
                            Camera.main.transform.RotateAround(_isChose.moveTarget.transform.position, Vector3.up, angle);
                        }
                    }
                    else
                    {
                        Camera.main.transform.RotateAround(center.transform.position, Vector3.up, angle);
                    }
                    //Camera.main.transform.LookAt(center.transform);
                }
                if (temp1.y != 0)
                {
                    float angle = temp1.y / offset.magnitude;
                    if (_isChose.isChose)
                    {
                        if (_isChose.moveTarget == null)
                        {
                            Camera.main.transform.RotateAround(_isChose.moveTargetPI.transform.position, -Camera.main.transform.right, angle);
                        }
                        else
                        {
                            Camera.main.transform.RotateAround(_isChose.moveTarget.transform.position, -Camera.main.transform.right, angle);
                        }
                    }
                    else
                    {
                        Camera.main.transform.RotateAround(center.transform.position, -Camera.main.transform.right, angle);
                    }
                    //Camera.main.transform.LookAt(center.transform);
                }
                originPos = Input.mousePosition;
            }
            else Manager.Instace._IsAlt = true;

            //Vector3 temp1 = Input.mousePosition - originPos;
            //Vector3 temp2=Vector3.forward-originPos;
            //Vector3 axis=Vector3.Cross(temp1,temp2);
            ////通过两点获得法线和距离
            //Vector3 rotateCenter = offset + transform.position;
            //float angle=Vector3.Distance(Input.mousePosition,originPos)/offset.magnitude;
            //Camera.main.transform.RotateAround(center.transform.position, axis, angle);
            //originPos = Input.mousePosition;
        }
        //}
        //isLMouseDow &&

        else if (isRMouseDow)//1.修改了鼠标左键右键同时按下能同时触发左右键逻辑的问题（添加了else）
        {
            //Debug.Log("111");
            Vector3 temp = Input.mousePosition - originPos;
            Vector3 eulerAngle = transform.rotation.eulerAngles;
            if (temp.x != 0)
            {
                eulerAngle.y += temp.x * 0.1f;
            }
            if (temp.y != 0)
            {
                eulerAngle.x -= temp.y * 0.1f;//2.修改了右键拖动摄像机旋转为鼠标向上相机向上看，鼠标向下摄像机向下看（+= ==> -=）
            }
            transform.eulerAngles = eulerAngle;
            originPos = Input.mousePosition;

            //只有按下了鼠标右键的情况下才能移动
            if (IsMove)
            {   //向右
                if (Input.GetKey(KeyCode.D))// && !EventSystem.current.IsPointerOverGameObject()
                {
                    cameraTransform.position += cameraTransform.right * Speed * Time.deltaTime;
                }//向左
                if (Input.GetKey(KeyCode.A))
                {
                    cameraTransform.position += -cameraTransform.right * Speed * Time.deltaTime;
                }//前进
                if (Input.GetKey(KeyCode.W))
                {
                    cameraTransform.position += cameraTransform.forward * Speed * Time.deltaTime;
                }//后退
                if (Input.GetKey(KeyCode.S))
                {
                    cameraTransform.position += -cameraTransform.forward * Speed * Time.deltaTime;
                }//上升
                if (Input.GetKey(KeyCode.Space))
                {
                    //Vector3 eulerAngle1 = cameraTransform.eulerAngles;
                    //eulerAngle1.y -= 0.1f;
                    //cameraTransform.eulerAngles = eulerAngle1;
                    cameraTransform.position += new Vector3(0, 1, 0) * Speed * Time.deltaTime;
                }//下降
                if (Input.GetKey(KeyCode.LeftShift))// && !EventSystem.current.IsPointerOverGameObject()
                {
                    //Vector3 eulerAngle2 = cameraTransform.eulerAngles;
                    //eulerAngle2.y += 0.1f;
                    //cameraTransform.eulerAngles = eulerAngle2;
                    cameraTransform.position += new Vector3(0, -1, 0) * Speed * Time.deltaTime;
                }
            }
        }

        if (isScrollDown)
        {
            Vector3 temp = Input.mousePosition - originPos;
            if (temp.x != 0)
            {
                transform.localPosition -= transform.right * temp.x * Time.deltaTime;
            }
            if (temp.y != 0)
            {
                transform.localPosition -= new Vector3(0, temp.y * Time.deltaTime, 0);
            }
            originPos = Input.mousePosition;
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && !EventSystem.current.IsPointerOverGameObject())
        {
            //if (Camera.main.fieldOfView >40)
            //{
            //    Camera.main.fieldOfView -= 1f;
            //}
            Camera.main.transform.localPosition += Camera.main.transform.forward;

        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0 && !EventSystem.current.IsPointerOverGameObject())
        {
            //if (Camera.main.fieldOfView < 100)
            //{
            //    Camera.main.fieldOfView += 1f;
            //}
            Camera.main.transform.localPosition -= Camera.main.transform.forward;

        }


        //-------------------------------------

        if (_isChose.isChose)
        {
            if (Input.GetKey(KeyCode.F) && IsMove)
            {
                Vector3 _direction;
                if (_isChose.moveTarget == null)
                {
                    _direction = _isChose.moveTargetPI.transform.position - cameraTransform.position;
                }
                else { _direction = _isChose.moveTarget.transform.position - cameraTransform.position; }

                cameraTransform.forward = _direction.normalized;
                cameraTransform.localEulerAngles = new Vector3(cameraTransform.localEulerAngles.x, cameraTransform.localEulerAngles.y, 0);
                if (_direction.magnitude > 5)
                {
                    cameraTransform.Translate(_direction.normalized * 50 * Time.deltaTime, Space.World);
                }
            }
        }
    }
    /// <summary>
    /// 增加移动速度
    /// </summary>
    public void AddSpeed()
    {
        Speed += 1f;
        if (Speed >= 10000f)
        {
            Speed = 9999f;
        }
        SpeedInputField.text = Speed.ToString();
    }
    /// <summary>
    /// 减少
    /// </summary>
    public void ReduceSpeed()
    {
        Speed -= 1f;
        if (Speed < 0f)
        {
            Speed = 0f;
        }
        SpeedInputField.text = Speed.ToString();
    }
    /// <summary>
    /// 修改移动速度
    /// </summary>
    public void ModifySpeed()
    {
        Speed = float.Parse(SpeedInputField.text);
        if (Speed < 0f)
        {
            Speed = 0f;
            SpeedInputField.text = Speed.ToString();
        }
        if (Speed > 9999)
        {
            Speed = 9999f;
            speedInputField.text = Speed.ToString();
        }
        
        //将修改后的值存储
        PlayerPrefs.SetFloat("Speed", Speed);
    }
}
