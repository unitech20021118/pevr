using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using HighlightingSystem;
using UnityEngine;

public class CameraActionController : MonoBehaviour
{
    public static CameraActionController Instance;
    /// <summary>
    /// 运行特写镜头动作前生效的摄像机
    /// </summary>
    private Camera lastCamera;
    /// <summary>
    /// 特写镜头的相机
    /// </summary>
    private Camera actionCamera;

    private AudioListener audioListener;

    private bool close;
    /// <summary>
    /// 镜头特写模式
    /// </summary>
    private int type;
    /// <summary>
    /// 放大模式下镜头移动时间
    /// </summary>
    private float moveTime;
    /// <summary>
    /// 环绕模式下旋转速度
    /// </summary>
    private float rotateSpeed;
    /// <summary>
    /// 镜头的位置
    /// </summary>
    private Vector3 positionVector3;
    /// <summary>
    /// 镜头的旋转角度
    /// </summary>
    private Vector3 rotateVector3;

    private GameObject mGameObject;
    private bool isArrived;

    private Coroutine coroutine;

    void Awake()
    {
        Instance = this;
    }
    // Use this for initialization
    void Start()
    {
        actionCamera = gameObject.GetComponent<Camera>();
        audioListener = gameObject.GetComponent<AudioListener>();
    }

    // Update is called once per frame
    void Update()
    {
        if (lastCamera != null)
        {
            if (Input.GetMouseButtonDown(1) && actionCamera.enabled)
            {
                if (coroutine != null)
                {
                    StopCoroutine(coroutine);
                }
                lastCamera.gameObject.SetActive(true);
                lastCamera.gameObject.AddComponent<HighlightingRenderer>();

                Destroy(actionCamera.gameObject.GetComponent<HighlightingRenderer>());
                actionCamera.gameObject.tag = "Untagged";
                actionCamera.enabled = false;
                audioListener.enabled = false;
            }
        }
    }

    public void StartCameraAction(GameObject mGameObject, bool close, int type, float movetime, float rotatespeed, Vector3 position, Vector3 rotate)
    {
        this.mGameObject = mGameObject;
        this.close = close;
        this.type = type;
        moveTime = movetime;
        rotateSpeed = rotatespeed;
        positionVector3 = position;
        rotateVector3 = rotate;
        if (this.close == false)
        {
            //判断在开始特写镜头动作前是哪个相机在执行任务
            if (Manager.Instace.FirstPerson.activeSelf)
            {
                Manager.Instace.FirstPersonCamera.gameObject.SetActive(false);
                lastCamera = Manager.Instace.FirstPersonCamera;
                Destroy(Manager.Instace.FirstPersonCamera.gameObject.GetComponent<HighlightingRenderer>());
                transform.position = Manager.Instace.FirstPersonCamera.transform.position;
                actionCamera.gameObject.tag = "MainCamera";
                actionCamera.enabled = true;
                audioListener.enabled = true;
                gameObject.AddComponent<HighlightingRenderer>();
            }
            else if (Manager.Instace.mainCamera.gameObject.activeSelf && !Manager.Instace.cameraEye.activeSelf)
            {
                Manager.Instace.mainCamera.gameObject.SetActive(false);
                lastCamera = Manager.Instace.mainCamera;
                Destroy(Manager.Instace.mainCamera.gameObject.GetComponent<HighlightingRenderer>());
                transform.position = Manager.Instace.mainCamera.transform.position;
                actionCamera.gameObject.tag = "MainCamera";
                actionCamera.enabled = true;
                audioListener.enabled = true;
                gameObject.AddComponent<HighlightingRenderer>();
            }else if (Manager.Instace.cameraEye.gameObject.activeSelf)
            {
                actionCamera.gameObject.tag = "MainCamera";
                actionCamera.enabled = true;
                audioListener.enabled = false;
                //Manager.Instace.cameraEye.gameObject.SetActive(false);
                Manager.Instace.mainCamera.gameObject.SetActive(false); 
                lastCamera = Manager.Instace.cameraEye.GetComponent<Camera>();
                Destroy(lastCamera.gameObject.GetComponent<HighlightingRenderer>());
                transform.position = lastCamera.transform.position;
                
                gameObject.AddComponent<HighlightingRenderer>();
            }
            //开始特写镜头动作
            coroutine = StartCoroutine(DoCameraAction());
        }
        else
        {
            if (lastCamera != null)
            {
                if (coroutine != null)
                {
                    StopCoroutine(coroutine);
                }
                lastCamera.gameObject.SetActive(true);
                lastCamera.gameObject.AddComponent<HighlightingRenderer>();

                Destroy(actionCamera.gameObject.GetComponent<HighlightingRenderer>());
                actionCamera.gameObject.tag = "Untagged";
                actionCamera.enabled = false;
                audioListener.enabled = false;
            }
        }
    }

    IEnumerator DoCameraAction()
    {
        if (type == 0)
        {
            float T = 0f;
            while (true)
            {
                actionCamera.transform.DOMove(positionVector3 + mGameObject.transform.position, moveTime);
                actionCamera.transform.DORotate(rotateVector3, moveTime);
                T += Time.deltaTime;
                if (T >= moveTime)
                {
                    break;
                }
                yield return null;
            }
        }
        else if (type == 1)
        {
            while (true)
            {
                if (!isArrived)
                {
                    actionCamera.transform.DOMove(positionVector3 + mGameObject.transform.position, 0).OnComplete(() => isArrived = true);
                    actionCamera.transform.DORotate(positionVector3, 0);

                }
                actionCamera.transform.RotateAround(mGameObject.transform.position, Vector3.up, rotateSpeed * Time.deltaTime);
                yield return null;
            }

        }
        yield return null;
    }

}
