using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Boo.Lang;
using DG.Tweening;
using HighlightingSystem;
using RenderHeads.Media.AVProVideo;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VRTK;
using ZenFulcrum.EmbeddedBrowser;
using VRTK.GrabAttachMechanics;
using System;
using UnityEngine.Events;

public class PICamera : MonoBehaviour
{
    /// <summary>
    /// 单例
    /// </summary>
    public static PICamera Instance;
    /// <summary>
    /// 展示物体的预设物
    /// </summary>
    public GameObject ShowChildPrefab;
    /// <summary>
    /// 展示的物体所在列表的content
    /// </summary>
    public Transform PIPanelContent;
    /// <summary>
    /// pi相机
    /// </summary>
    public Camera piCamera;
    /// <summary>
    /// 开始三维展示前是否是在第一人称的状态下
    /// </summary>
    private bool firstPerson;
    /// <summary>
    /// 展示的物体
    /// </summary>
    public GameObject Target;
    /// <summary>
    /// 打开详细的开关
    /// </summary>
    public GameObject openDetailed;
    /// <summary>
    /// 右键菜单
    /// </summary>
    public GameObject OptionGameObject;
    /// <summary>
    /// 添加过组件的子物体与位置列表
    /// </summary>
    System.Collections.Generic.List<GameobjectAndPosition> childList = new System.Collections.Generic.List<GameobjectAndPosition>();
    /// <summary>
    /// 物体原本的旋转角
    /// </summary>
    private Vector3 targetRotate;
    /// <summary>
    /// 物体原本所在的位置
    /// </summary>
    private Vector3 targetPosition;
    /// <summary>
    /// 主相机原本的旋转角
    /// </summary>
    private Vector3 cameraRotate;
    /// <summary>
    /// 主相机原本的位置
    /// </summary>
    private Vector3 cameraPosition;
    /// <summary>
    /// PI展示的面板
    /// </summary>
    public GameObject PIPanel;
    /// <summary>
    /// 相机与目标物体的距离
    /// </summary>
    //float distance;
    /// <summary>
    /// 上一帧鼠标位置
    /// </summary>
    Vector3 originPos;
    /// <summary>
    /// 偏移向量
    /// </summary>
    Vector3 temp = Vector3.zero;
    /// <summary>
    /// PI展示的面板是否打开
    /// </summary>
    bool IsShowPIPanel;
    /// <summary>
    /// 父物体上的碰撞盒
    /// </summary>
    Collider collider;
    /// <summary>
    /// 是否开始了PI展示
    /// </summary>
    bool IsStartPIShow = false;
    /// <summary>
    /// 射线检测的层级
    /// </summary>
    private LayerMask mask;
    /// <summary>
    /// 显示介绍的文本框
    /// </summary>
    public Transform IntroduceTextTransform;
    /// <summary>
    /// 介绍视频播放器
    /// </summary>
    public GameObject IntroduceVideoGameObject;
    /// <summary>
    /// 介绍图片
    /// </summary>
    public GameObject IntroducImageGameObject;
    /// <summary>
    /// 修改介绍的输入框
    /// </summary>
    public InputField inputField;
    /// <summary>
    /// 展示物体的子物体与界面上的关联
    /// </summary>
    Dictionary<GameObject, ChildIndex> GoAndChildIndex = new Dictionary<GameObject, ChildIndex>();
    /// <summary>
    /// 是编辑状态还是运行状态
    /// </summary>
    bool isEdit;
    /// <summary>
    /// 向左转
    /// </summary>
    bool Left;
    /// <summary>
    /// 向上转
    /// </summary>
    bool Up;
    /// <summary>
    /// 向右转
    /// </summary>
    bool Right;
    /// <summary>
    /// 向下转
    /// </summary>
    bool Down;
    /// <summary>
    /// 左下旋
    /// </summary>
    bool LDown;
    /// <summary>
    /// 右下旋
    /// </summary>
    bool RDown;

    private Transform MainCameraTrans;
    private HighlightingRenderer highlightingRenderer;
    public Text IntroduceImagePathText;
    public Text IntroduceVideoPathText;
    private string imagePath;
    private string imageName;
    private string videoPath;
    private string videoName;
    

    /// <summary>
    /// 鼠标悬浮射线
    /// </summary>
    private Ray mouseSuspensionRay;
    /// <summary>
    /// 鼠标悬浮射线检测到的物体
    /// </summary>
    private RaycastHit mouseSuspensionhit;
    /// <summary>
    /// 鼠标悬浮物体
    /// </summary>
    private GameObject mouseSuspensionGameObject;
    /// <summary>
    /// 上一个悬浮到的物体
    /// </summary>
    private GameObject lastMouseSuspensionGameObject;
    /// <summary>
    /// 编辑状态下当前编辑的目标物体名字
    /// </summary>
    private string NowIntroduceGameobjectName;
    
    void Awake()
    {
        Instance = this;
    }
    // Use this for initialization
    void Start()
    {
        piCamera = Camera.main;
        isEdit = true;
        mask = LayerMask.GetMask("PI");
    }

    // Update is called once per frame
    void Update()
    {
        //当三维展示的物体加载完成时
        if (IsStartPIShow)
        {
            if (!vrMode)
            {
                if (Input.GetAxis("Mouse ScrollWheel") < 0)
                {
                    piCamera.fieldOfView += 2f;
                    piCamera.fieldOfView = Mathf.Clamp(piCamera.fieldOfView, 2f, 60f);
                }
                else if (Input.GetAxis("Mouse ScrollWheel") > 0)
                {
                    piCamera.fieldOfView -= 2f;
                    piCamera.fieldOfView = Mathf.Clamp(piCamera.fieldOfView, 2f, 60f);
                }
                if (Input.GetMouseButton(1))
                {
                    float distance = 0f;
                    if (originPos != Vector3.zero)
                    {
                        temp = Input.mousePosition - originPos;
                        distance = Vector3.Distance(Input.mousePosition, originPos);
                    }
                    if (temp != Vector3.zero)
                    {
                        //piCamera.transform.RotateAround(TargetGameObject.transform.position, new Vector3(temp.y,temp.x,0), 2f);
                        Target.transform.Rotate(new Vector3(-temp.y, -temp.x, 0), distance / 5, 0);
                    }
                    originPos = Input.mousePosition;
                }
                if (Input.GetMouseButtonUp(1))
                {
                    originPos = Vector3.zero;
                    temp = Vector3.zero;
                }
                //鼠标悬浮状态检测物体
                MouseSuspension();
                


                
                IntroduceMove();
            }
            if (vrMode)
            {
                ControlDistance();
            }

        }
        if (!vrMode)
        {
            if (IsShowPIPanel == true)
            {
                PIPanel.transform.localPosition = Vector3.Lerp(PIPanel.transform.localPosition, new Vector3(298, PIPanel.transform.localPosition.y, 0), 0.2f);
                if (openDetailed.transform.localEulerAngles != new Vector3(0, 0, 0))
                {
                    openDetailed.transform.localEulerAngles = new Vector3(0, 0, 0);
                }
            }
            else if (IsShowPIPanel == false)
            {
                PIPanel.transform.localPosition = Vector3.Lerp(PIPanel.transform.localPosition, new Vector3(505, PIPanel.transform.localPosition.y, 0), 0.2f);
                if (openDetailed.transform.localEulerAngles != new Vector3(0, 0, 180))
                {
                    openDetailed.transform.localEulerAngles = new Vector3(0, 0, 180);
                }
            }
        }
       

        //按下PI两按键打开子物体面板
        //if (Input.GetKey(KeyCode.P) && Input.GetKeyDown(KeyCode.I))
        //{
        //    if (GameObject.Find("Canvas").GetComponent<G_EditorTarget>().moveTarget != null)
        //    {
        //        ShowChildren(GameObject.Find("Canvas").GetComponent<G_EditorTarget>().moveTarget.transform);
        //        NowIntroduceGameobjectName =
        //            GameObject.Find("Canvas").GetComponent<G_EditorTarget>().moveTarget.gameObject.name;
        //    }
        //}

        //按下鼠标左键展示介绍文字
        if (Input.GetMouseButtonDown(0))
        {
            ShowIntroduce();
        }
        if (isEdit == false)
        {
            if (Up == true)
            {
                if (ChildIndex.dragedChildIndex != null)
                {
                    ChildIndex.dragedChildIndex.Target.transform.Rotate(new Vector3(-30 * Time.deltaTime, 0, 0), Space.World);
                }
                else
                {
                    Target.transform.Rotate(new Vector3(-30 * Time.deltaTime, 0, 0), Space.World);
                }
            }
            else if (Down == true)
            {
                if (ChildIndex.dragedChildIndex != null)
                {
                    ChildIndex.dragedChildIndex.Target.transform.Rotate(new Vector3(30 * Time.deltaTime, 0, 0), Space.World);
                }
                else
                {
                    Target.transform.Rotate(new Vector3(30 * Time.deltaTime, 0, 0), Space.World);
                }

            }
            else if (Left == true)
            {
                if (ChildIndex.dragedChildIndex != null)
                {
                    ChildIndex.dragedChildIndex.Target.transform.Rotate(new Vector3(0, 30 * Time.deltaTime, 0), Space.World);
                }
                else
                {
                    Target.transform.Rotate(new Vector3(0, 30 * Time.deltaTime, 0), Space.World);
                }

            }
            else if (Right == true)
            {
                if (ChildIndex.dragedChildIndex != null)
                {
                    ChildIndex.dragedChildIndex.Target.transform.Rotate(new Vector3(0, -30 * Time.deltaTime, 0), Space.World);
                }
                else
                {
                    Target.transform.Rotate(new Vector3(0, -30 * Time.deltaTime, 0), Space.World);
                }

            }
            else if (LDown == true)
            {
                if (ChildIndex.dragedChildIndex != null)
                {
                    ChildIndex.dragedChildIndex.Target.transform.Rotate(new Vector3(0, 0, -30 * Time.deltaTime), Space.World);
                }
                else
                {
                    Target.transform.Rotate(new Vector3(0, 0, -30 * Time.deltaTime), Space.World);
                }

            }
            else if (RDown == true)
            {
                if (ChildIndex.dragedChildIndex != null)
                {
                    ChildIndex.dragedChildIndex.Target.transform.Rotate(new Vector3(0, 0, 30 * Time.deltaTime), Space.World);
                }
                else
                {
                    Target.transform.Rotate(new Vector3(0, 0, 30 * Time.deltaTime), Space.World);
                }


            }
        }

        if (Input.GetMouseButtonUp(0) && OptionGameObject.activeSelf)
        {
            OptionGameObject.SetActive(false);
        }
    }
    /// <summary>
    /// pi相机属性设置
    /// </summary>
    public void InitPiCamera()
    {
        if (vrMode)
        {
            piCamera = Manager.Instace.cameraEye.GetComponent<Camera>();
            Destroy(Camera.main.GetComponent<HighlightingRenderer>());
            piCamera.gameObject.AddComponent<HighlightingRenderer>();
        }
        else
        {
            if (Manager.Instace.FirstPerson.activeSelf)
            {
                firstPerson = true;
                piCamera.gameObject.SetActive(true);
                Manager.Instace.FirstPerson.SetActive(false);
            }
            //记录原本属性
            MainCameraTrans = Camera.main.transform;
            //关闭主相机用于管理鼠标操作的脚本
            piCamera.gameObject.GetComponent<G_ObserveScene>().enabled = false;
            piCamera.fieldOfView = 20f;
            piCamera.cullingMask = LayerMask.GetMask("PI");

            
        }
        piCamera.clearFlags = CameraClearFlags.SolidColor;
        piCamera.backgroundColor = new Color32(105, 98, 93, 0);

        highlightingRenderer = piCamera.gameObject.GetComponent<HighlightingRenderer>();
        highlightingRenderer.downsampleFactor = 1;
        highlightingRenderer.iterations = 1;
        highlightingRenderer.blurMinSpread = 1f;
        highlightingRenderer.blurSpread = 0f;
        highlightingRenderer.blurIntensity = 1f;
    }
    /// <summary>
    /// 主相机属性设置
    /// </summary>
    public void InitMainCamera()
    {
        if (vrMode )
        {
            //Debug.LogError("VR相机还原");

            //Destroy(piCamera.GetComponent<HighlightingRenderer>());
            //Camera.main.gameObject.AddComponent<HighlightingRenderer>();
            piCamera.clearFlags = CameraClearFlags.Skybox;
            piCamera.backgroundColor = new Color32(49,77,121,5);
            piCamera.cullingMask = LayerMask.NameToLayer("Everything");

            //同时将目标物体位置和旋转还原
            //Target.transform.localPosition = poisitionVector3;
            //Target.transform.localEulerAngles = rotationVector3;

        }
        else
        {
            if (firstPerson)
            {
                piCamera.gameObject.SetActive(false);
                Manager.Instace.FirstPerson.SetActive(true);
            }
            piCamera.fieldOfView = 60f;
            piCamera.transform.position = MainCameraTrans.position;
            piCamera.transform.eulerAngles = MainCameraTrans.eulerAngles;
            piCamera.gameObject.GetComponent<G_ObserveScene>().enabled = true;
        }
        
        piCamera.clearFlags = CameraClearFlags.Skybox;
        piCamera.backgroundColor = new Color32(49, 77, 121, 0);
        piCamera.cullingMask = LayerMask.NameToLayer("Everything");
        highlightingRenderer.downsampleFactor = 4;
        highlightingRenderer.iterations = 2;
        highlightingRenderer.blurMinSpread = 0.5f;
        highlightingRenderer.blurSpread = 0.15f;
        highlightingRenderer.blurIntensity = 0.325f;
    }

    /// <summary>
    /// 在编辑模式下显示物体上的原本介绍信息
    /// </summary>
    public void ShowOriginalIntroduce()
    {

        if (ChildIndex.dragedChildIndex.Target.GetComponent<Introduce>() != null)
        {
            inputField.text = ChildIndex.dragedChildIndex.Target.GetComponent<Introduce>().introduce;
            string videoPath = ChildIndex.dragedChildIndex.Target.GetComponent<Introduce>().videoPath;
            string imagePath = ChildIndex.dragedChildIndex.Target.GetComponent<Introduce>().imagePath;
            if (!string.IsNullOrEmpty(videoPath))
            {
                IntroduceVideoPathText.text = videoPath;
            }
            else
            {
                IntroduceVideoPathText.text = "添加介绍视频";
            }
            if (!string.IsNullOrEmpty(imagePath))
            {
                IntroduceImagePathText.text = imagePath;
            }
            else
            {
                IntroduceImagePathText.text = "添加介绍图片";
            }
        }
        else
        {
            inputField.text = "";
            IntroduceImagePathText.text = "添加介绍图片";
            IntroduceVideoPathText.text = "添加介绍视频";
        }
    }
    /// <summary>
    /// 修改介绍信息
    /// </summary>
    public void SetIntroduce()
    {
        string str = inputField.text;
        if (ChildIndex.dragedChildIndex != null)
        {
            if (ChildIndex.dragedChildIndex.Target.GetComponent<Introduce>() != null)
            {
                ChildIndex.dragedChildIndex.Target.GetComponent<Introduce>().SetIntroduce(str);
            }
            else { ChildIndex.dragedChildIndex.Target.AddComponent<Introduce>().SetIntroduce(str); }
        }
        else
        {
            inputField.text = "请先选择要添加介绍的物体！！";
        }
    }

    public void SelectImage()
    {
        if (ChildIndex.dragedChildIndex != null)
        {
            
            //选择图片
            try
            {
                FileInfo fileInfo = new FileInfo(IOHelper.GetImageName());
                imagePath = fileInfo.FullName;
                imageName = fileInfo.Name;
                CopyImage();
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
            string path = imageName;
            if (ChildIndex.dragedChildIndex.Target.GetComponent<Introduce>() != null)
            {
                ChildIndex.dragedChildIndex.Target.GetComponent<Introduce>().SetImagePath(path);
            }
            else
            {
                ChildIndex.dragedChildIndex.Target.AddComponent<Introduce>().SetImagePath(path);
            }
            IntroduceImagePathText.text = imageName;
        }
        else
        {
            inputField.text = "请先选择要添加介绍的物体！！";
        }
    }
    
    /// <summary>
    /// 选择介绍视频
    /// </summary>
    public void SelectVideo()
    {
        if (ChildIndex.dragedChildIndex != null)
        {
            //选择视频
            try
            {
                FileInfo fileInfo = new FileInfo(IOHelper.GetVideoName());
                videoPath = fileInfo.FullName;
                videoName = fileInfo.Name;
                CopyVideo();
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
            }

            string path = videoName;
            if (ChildIndex.dragedChildIndex.Target.GetComponent<Introduce>() != null)
            {
                ChildIndex.dragedChildIndex.Target.GetComponent<Introduce>().SetVideoPath(path);
            }
            else { ChildIndex.dragedChildIndex.Target.AddComponent<Introduce>().SetVideoPath(path); }
            IntroduceVideoPathText.text = videoName;
        }
        else
        {
            inputField.text = "请先选择要添加介绍的物体！！";
        }

    }

    void CopyImage()
    {
        if (Directory.Exists(Application.streamingAssetsPath + "/PIShow/images"))
        {
            File.Copy(imagePath, Application.streamingAssetsPath + "/PIShow/images/"+imageName, true);
        }
        else
        {
            Directory.CreateDirectory(Application.streamingAssetsPath + "/PIShow/images");
            File.Copy(imagePath, Application.streamingAssetsPath + "/PIShow/images/" + imageName, true);
        }
    }
    void CopyVideo()
    {
        if (Directory.Exists(Application.streamingAssetsPath + "/PIShow/videos"))
        {
            File.Copy(videoPath, Application.streamingAssetsPath + "/PIShow/videos/" + videoName, true);
        }
        else
        {
            Directory.CreateDirectory(Application.streamingAssetsPath + "/PIShow/videos");
            File.Copy(videoPath, Application.streamingAssetsPath + "/PIShow/videos/" + videoName, true);
        }
    }
    /// <summary>
    /// 显示介绍信息
    /// </summary>
    public void ShowIntroduce(GameObject go = null)
    {
        string str;
        string videoPath;
        string imagePath;
        if (go!=null)
        {
            if (go.GetComponent<Introduce>())
            {
                //显示介绍文字
                str = go.GetComponent<Introduce>().introduce;
                videoPath = go.GetComponent<Introduce>().videoPath;
                imagePath = go.GetComponent<Introduce>().imagePath;
                if (!string.IsNullOrEmpty(str))
                {
                    IntroduceTextTransform.GetComponentInChildren<Text>().text = str;
                }
                else
                {
                    str = "暂无简介";
                }
                

                //显示介绍视频
                if (!string.IsNullOrEmpty(videoPath))
                {
                    videoPath = Application.streamingAssetsPath + "/PIShow/videos/" + videoPath;
                    IntroduceVideoGameObject.SetActive(true);
                    StartCoroutine(DoWaitToLoadVideo(videoPath));
                }
                else
                {
                    IntroduceVideoGameObject.SetActive(false);
                }
                //显示介绍图片
                if (!string.IsNullOrEmpty(imagePath))
                {
                    //IntroducImageGameObject.GetComponent<Image>().sprite = new Sprite
                    imagePath = Application.streamingAssetsPath + "/PIShow/images/" + imagePath;
                    IntroducImageGameObject.SetActive(true);
                    StartCoroutine(DoWaitToLoadImage(imagePath, IntroducImageGameObject.GetComponent<Image>()));
                }
                else
                {
                    IntroducImageGameObject.SetActive(false);
                }
            }

            return;
        }
        Ray ray = piCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 80, mask))
        {

            if (hit.transform.GetComponent<Introduce>())
            {
                str = hit.transform.GetComponent<Introduce>().introduce;
                
                
                //显示介绍文字
                if (!string.IsNullOrEmpty(str))
                {
                    IntroduceTextTransform.gameObject.SetActive(true);
                    IntroduceTextTransform.GetComponentInChildren<Text>().text = str;
                }
                else
                {
                    IntroduceTextTransform.gameObject.SetActive(false);
                }
                //显示介绍视频
                if (!string.IsNullOrEmpty(hit.transform.GetComponent<Introduce>().videoPath))
                {
                    videoPath = Application.streamingAssetsPath + "/PIShow/videos/" + hit.transform.GetComponent<Introduce>().videoPath;
                    IntroduceVideoGameObject.SetActive(true);
                    //设置视频地址
                    IntroduceVideoGameObject.GetComponentInChildren<MediaPlayer>().OpenVideoFromFile(MediaPlayer.FileLocation.RelativeToDataFolder, videoPath, false);
                }
                else
                {
                    IntroduceVideoGameObject.SetActive(false);
                }
                //显示介绍图片
                if (!string.IsNullOrEmpty(hit.transform.GetComponent<Introduce>().imagePath))
                {
                    imagePath = Application.streamingAssetsPath+ "/PIShow/images/" + hit.transform.GetComponent<Introduce>().imagePath;
                    IntroducImageGameObject.SetActive(true);
                    StartCoroutine(DoWaitToLoadImage(imagePath, IntroducImageGameObject.GetComponent<Image>()));
                }
                else
                {
                    IntroducImageGameObject.SetActive(false);
                }
            }
            else
            {
                IntroduceTextTransform.gameObject.SetActive(false);
                IntroduceTextTransform.GetComponentInChildren<Text>().text = "";
                IntroduceVideoGameObject.SetActive(false);
            }
            GoAndChildIndex[hit.transform.gameObject].ShowTarget();
        }
    }

    private bool videoShow;
    private bool textShow;

    private bool imageShow;
    //private Coroutine videoCoroutine;
    //private Coroutine textCoroutine;
    public void ShowIntroduce(int a)
    {
        if (a==1 && !videoShow)
        {
            videoShow = true;
        }
        else if (a==1 && videoShow)
        {
            videoShow = false;
        }
        else if (a==2 && !imageShow)
        {
            imageShow = true;
        }
        else if (a==2 && imageShow)
        {
            imageShow = false;
        }
        else if (a==0 && !textShow)
        {
            textShow = true;
        }
        else
        {
            textShow = false;
        }
    }

    public void IntroduceMove()
    {
        if (videoShow)
        {
            IntroduceVideoGameObject.transform.localPosition = Vector3.Lerp(IntroduceVideoGameObject.transform.localPosition,
                new Vector3(-300f,IntroduceVideoGameObject.transform.localPosition.y, 0), 0.2f);
        }
        else
        {
            IntroduceVideoGameObject.transform.localPosition = Vector3.Lerp(IntroduceVideoGameObject.transform.localPosition,
                new Vector3(-500f, IntroduceVideoGameObject.transform.localPosition.y, 0), 0.2f);
        }
        if (textShow)
        {
            IntroduceTextTransform.transform.localPosition = Vector3.Lerp(IntroduceTextTransform.transform.localPosition,
                new Vector3(-300f, IntroduceTextTransform.transform.localPosition.y, 0), 0.2f);
        }
        else
        {
            IntroduceTextTransform.transform.localPosition = Vector3.Lerp(IntroduceTextTransform.transform.localPosition,
                new Vector3(-500f, IntroduceTextTransform.transform.localPosition.y, 0), 0.2f);
        }
        if (imageShow)
        {
            IntroducImageGameObject.transform.localPosition = Vector3.Lerp(
                IntroducImageGameObject.transform.localPosition,
                new Vector3(-300f, IntroducImageGameObject.transform.localPosition.y, 0), 0.2f);
        }
        else
        {
            IntroducImageGameObject.transform.localPosition = Vector3.Lerp(
                IntroducImageGameObject.transform.localPosition,
                new Vector3(-500f, IntroducImageGameObject.transform.localPosition.y, 0), 0.2f);
        }
    }
    /// <summary>
    /// 显示pi展示的面板
    /// </summary>
    public void ShowPIPanel()
    {
        if (IsShowPIPanel == true)
        {
            IsShowPIPanel = false;
        }
        else
        {
            if (NowIntroduceGameobjectName != null)
            {
                if (GameObject.Find("Canvas").GetComponent<G_EditorTarget>().moveTarget != null)
                {
                    if (NowIntroduceGameobjectName != GameObject.Find("Canvas").GetComponent<G_EditorTarget>()
                            .moveTarget.gameObject.name)
                    {
                        ShowChildren(GameObject.Find("Canvas").GetComponent<G_EditorTarget>().moveTarget.transform);
                        NowIntroduceGameobjectName =
                            GameObject.Find("Canvas").GetComponent<G_EditorTarget>().moveTarget.gameObject.name;
                    }

                }
            }
            else
            {
                if (GameObject.Find("Canvas").GetComponent<G_EditorTarget>().moveTarget==null)
                {
                    IsShowPIPanel = true;
                }
                else
                {
                    if (NowIntroduceGameobjectName != GameObject.Find("Canvas").GetComponent<G_EditorTarget>()
                            .moveTarget.gameObject.name)
                    {
                        ShowChildren(GameObject.Find("Canvas").GetComponent<G_EditorTarget>().moveTarget.transform);
                        NowIntroduceGameobjectName =
                            GameObject.Find("Canvas").GetComponent<G_EditorTarget>().moveTarget.gameObject.name;
                    }
                }
            }
            IsShowPIPanel = true;
        }
    }
    /// <summary>
    /// 开始三维展示
    /// </summary>
    /// <param name="obj"></param>
    public void StartPIShow(GameObject obj)
    {
        
        InitPiCamera();
        isEdit = false;
        inputField.gameObject.SetActive(false);
        StartCoroutine(DoStartPIShow(obj));
        if (GoAndChildIndex != null && GoAndChildIndex.Count > 0)
        {
            GoAndChildIndex.Clear();
        }
    }
    /// <summary>
    /// 开始三维展示的协程
    /// </summary>
    public IEnumerator DoStartPIShow(GameObject target)
    {
        Target = target;
        targetRotate = target.transform.localEulerAngles;
        targetPosition = Target.transform.localPosition;
        //cameraPosition = 
        //vr模式的话记录物体原本位置
        if (vrMode)
        {
            //poisitionVector3 = Target.transform.localPosition;
            //rotationVector3 = Target.transform.localEulerAngles;
            InitVrMode();
            targetOriginalTransform = Target.transform;
            //Target.transform.position = piCamera.transform.position + new Vector3(0, 0, -20f);
            //MoveVrModeUI();
        }
        else
        {
            //将目标物体移动到远处
            Target.transform.localPosition = Target.transform.localPosition + new Vector3(1000, 0, 0);
            cameraPosition = piCamera.transform.position;
            cameraRotate = piCamera.transform.eulerAngles;
            piCamera.transform.position = Target.transform.position + new Vector3(0, 0, 20);
            piCamera.transform.LookAt(Target.transform);
        }
        
        GameobjectAndPosition go;
        //暂时关闭父物体上的碰撞盒子
        if (target.transform.GetComponent<Collider>())
        {
            collider = target.transform.GetComponent<Collider>();
            target.transform.GetComponent<Collider>().enabled = false;
        }
        //如果父物体上有模型
        if (target.GetComponent<MeshRenderer>() != null)
        {
            go = new GameobjectAndPosition(target, target.transform.localPosition, target.transform.localEulerAngles);
            target.layer = 11;
            childList.Add(go);
        }


        Transform[] tra = target.GetComponentsInChildren<Transform>();

        for (int i = 1; i < tra.Length; i++)
        {
            if (tra[i].GetComponent<MeshRenderer>() == null)
            {
                continue;
            }
            //为所有子物体添加刚体并设置不受力
            tra[i].gameObject.AddComponent<Rigidbody>().isKinematic = true;
            //为所有子物体添加碰撞盒子
            tra[i].gameObject.AddComponent<MeshCollider>();
            if (vrMode)
            {
                //为所有子物体添加可以被手柄交互的脚本
                interactableObject = tra[i].gameObject.AddComponent<VRTK_InteractableObject>();
                interactableObject.grabOverrideButton = VRTK_ControllerEvents.ButtonAlias.Trigger_Press;
                interactableObject.isGrabbable = true;
                //interactableObject.isUsable = true;
                interactableObject.allowedGrabControllers = VRTK_InteractableObject.AllowedController.Right_Only;
                interactableObject.allowedTouchControllers = VRTK_InteractableObject.AllowedController.Right_Only;
                fixedJointGrabAttach = tra[i].gameObject.AddComponent<VRTK_FixedJointGrabAttach>();
                fixedJointGrabAttach.precisionGrab = true;
                fixedJointGrabAttach.onGrabCollisionDelay = 50;
                fixedJointGrabAttach.breakForce = float.MaxValue;
            }
            else
            {
                //为所有子物体添加允许拖动的脚本并设置观察的相机
                tra[i].gameObject.AddComponent<DragableObj>().SetCamera(Instance.piCamera);
                //设置层级
                tra[i].gameObject.layer = 11;
            }
            
            //将所有添加过组件的子物体加入列表
            go = new GameobjectAndPosition(tra[i].gameObject, tra[i].localPosition, tra[i].localEulerAngles);
            childList.Add(go);
            yield return null;
        }
        IsStartPIShow = true;
        if (!vrMode)
        {
            PIPanel.SetActive(true);
        }
        PIPanel.transform.FindChild("Menu/InputField_Introduce").gameObject.SetActive(false);
        //openDetailed.SetActive(true);
        //openDetailed.transform.GetChild(0).gameObject.SetActive(true);
        ShowChildren(Target.transform);
    }

    private bool a;
    /// <summary>
    /// 结束三维展示
    /// </summary>
    public void EndPIShow()
    {
        ShowPIPanel();
        IntroduceTextTransform.gameObject.SetActive(false);
        for (int i = 0; i < childList.Count; i++)
        {
            Destroy(childList[i].gameobject.GetComponent<MeshCollider>());
            Destroy(childList[i].gameobject.GetComponent<Rigidbody>());
            Destroy(childList[i].gameobject.GetComponent<DragableObj>());
            childList[i].gameobject.layer = 9;
        }
        Reduction(true);

        Target.transform.localEulerAngles = targetRotate;
        Target.transform.localPosition = targetPosition;
        piCamera.transform.position = cameraPosition;
        piCamera.transform.eulerAngles = cameraRotate;
        if (collider != null)
        {
            collider.enabled = true;
        }
        IsStartPIShow = false;
        InitMainCamera();
        Target = null;
       
        if (GoAndChildIndex != null && GoAndChildIndex.Count > 0)
        {
            GoAndChildIndex.Clear();
        }

        //Camera.main.cullingMask = LayerMask.GetMask("Everything");
    }

    /// <summary>
    /// 还原
    /// </summary>
    public void Reduction(bool IsEnd)
    {
        //清除选中
        ClearSelected();
        if (IsEnd)
        {//如果是结束展示直接还原
            for (int i = 0; i < childList.Count; i++)
            {
                childList[i].gameobject.transform.localPosition = childList[i].position;
                childList[i].gameobject.transform.localEulerAngles = childList[i].localEulerAngles;
            }
        }
        else
        {
            //如果不是结束展示启用带动画效果的还原
            for (int i = 0; i < childList.Count; i++)
            {
                StartCoroutine(DoReduction(childList[i]));
            }
        }
    }

    /// <summary>
    /// 清除选中物体
    /// </summary>
    public void ClearSelected()
    {
        if (ChildIndex.dragedChildIndex != null)
        {
            ChildIndex.dragedChildIndex.Reduce();
        }
    }
    /// <summary>
    /// 有动画效果的还原
    /// </summary>
    /// <param name="go"></param>
    /// <returns></returns>
    IEnumerator DoReduction(GameobjectAndPosition go)
    {
        float time = 0f;
        go.gameobject.transform.localEulerAngles = go.localEulerAngles;
        while (time <= 1.5f)
        {
            time += Time.deltaTime;
            go.gameobject.transform.localPosition = Vector3.Lerp(go.gameobject.transform.localPosition, go.position, 0.2f);

            yield return null;
        }
        go.gameobject.transform.localPosition = go.position;
    }
    /// <summary>
    /// 在物体列表中显示内容
    /// </summary>
    /// <param name="tra"></param>
    public void ShowChildren(Transform tra)
    {
        if (PIPanelContent.childCount > 0)
        {
            for (int i = 0; i < PIPanelContent.childCount; i++)
            {
                Destroy(PIPanelContent.GetChild(i).gameObject);
            }
        }
        StartCoroutine(DOShowChildren(tra));
    }
    /// <summary>
    /// 在物体列表中显示内容的协程
    /// </summary>
    /// <param name="tf"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    public IEnumerator DOShowChildren(Transform tf, Transform parent = null)
    {
        if (parent == null)
        {
            parent = CreateChild(tf.gameObject, null).transform;
        }
        else
        {
            parent = CreateChild(tf.gameObject, parent).transform;
        }
        if (tf.childCount > 0)
        {
            for (int i = 0; i < tf.childCount; i++)
            {
                StartCoroutine(DOShowChildren(tf.GetChild(i), parent));
            }
        }
        yield return null;

    }
    /// <summary>
    /// 创建一个子物体的记录
    /// </summary>
    /// <param name="obj"></param>
    public GameObject CreateChild(GameObject obj, Transform par)
    {
        if (ShowChildPrefab == null)
        {
            ShowChildPrefab = Resources.Load<GameObject>("Prefabs/Child");
        }
        GameObject clone = Instantiate(ShowChildPrefab);
        clone.name = obj.name;
        clone.transform.SetParent(PIPanelContent);
        clone.transform.localScale = Vector3.one;
        clone.transform.localPosition = Vector3.zero;
        clone.transform.localEulerAngles = Vector3.zero;
        
        clone.GetComponent<ChildIndex>().SetInformation(obj, par);
        clone.GetComponent<ChildIndex>().SetFindTransform(PIPanelContent);
        clone.GetComponent<ChildIndex>().IsEdit = isEdit;
        if (!GoAndChildIndex.ContainsKey(obj))
        {
            GoAndChildIndex.Add(obj, clone.GetComponent<ChildIndex>());
        }
        
        if (par != null)
        {
            clone.tag = "SCI";
            clone.gameObject.SetActive(false);
        }
        return clone;
    }
    /// <summary>
    /// 鼠标悬浮射线检测
    /// </summary>
    public void MouseSuspension()
    {
        mouseSuspensionRay = piCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(mouseSuspensionRay, out mouseSuspensionhit, 80, mask))
        {
            if (lastMouseSuspensionGameObject != null)
            {
                if (lastMouseSuspensionGameObject != mouseSuspensionhit.transform.gameObject)
                {
                    GoAndChildIndex[mouseSuspensionhit.transform.gameObject].ShowSuspensionTarget();
                    GoAndChildIndex[lastMouseSuspensionGameObject].StopShowSuspensionTarget();
                }
            }
            else
            {
                GoAndChildIndex[mouseSuspensionhit.transform.gameObject].ShowSuspensionTarget();
            }
            lastMouseSuspensionGameObject = mouseSuspensionhit.collider.gameObject;
        }
        else
        {
            if (lastMouseSuspensionGameObject != null)
            {
                GoAndChildIndex[lastMouseSuspensionGameObject].StopShowSuspensionTarget();
                lastMouseSuspensionGameObject = null;
            }
        }
    }
    /// <summary>
    /// 三维展示模式下旋转的按钮点击
    /// </summary>
    /// <param name="a"></param>
    public void RotateButtonDown(int a)
    {
        switch (a)
        {
            case 1:
                Up = true;
                break;
            case 2:
                Down = true;
                break;
            case 3:
                Left = true;
                break;
            case 4:
                Right = true;
                break;
            case 5:
                LDown = true;
                break;
            case 6:
                RDown = true;
                break;
            default:
                break;
        }
    }
    public void RotateButtonUp(int a)
    {
        switch (a)
        {
            case 1:
                Up = false;
                break;
            case 2:
                Down = false;
                break;
            case 3:
                Left = false;
                break;
            case 4:
                Right = false;
                break;
            case 5:
                LDown = false;
                break;
            case 6:
                RDown = false;
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 显示右键菜单
    /// </summary>
    public void ShowOption(bool hasChild)
    {
        OptionGameObject.SetActive(true);
        OptionGameObject.transform.position = Input.mousePosition;
        if (!hasChild)
        {
            OptionGameObject.transform.GetChild(0).GetComponent<Button>().enabled = false;
            OptionGameObject.transform.GetChild(0).GetComponent<Image>().color = Color.gray;

            OptionGameObject.transform.GetChild(1).GetComponent<Button>().enabled = false;
            OptionGameObject.transform.GetChild(1).GetComponent<Image>().color = Color.gray;
        }
        else
        {
            OptionGameObject.transform.GetChild(0).GetComponent<Button>().enabled = true;
            OptionGameObject.transform.GetChild(0).GetComponent<Image>().color = Color.white;

            OptionGameObject.transform.GetChild(1).GetComponent<Button>().enabled = true;
            OptionGameObject.transform.GetChild(1).GetComponent<Image>().color = Color.white;
        }
    }
    /// <summary>
    /// 在场景中展开当前选中物体的子物体
    /// </summary>
    public void OpenChildInScene()
    {
        ChildIndex.dragedChildIndex.OpenChildInScene();
    }
    /// <summary>
    ///还原在场景中被展开的子物体
    /// </summary>
    public void Reduction()
    {
        ChildIndex.dragedChildIndex.Reduction();
    }


    #region VR状态下的三维展示部分
    /// <summary>
    /// 开始三维展示时是否是vr视角
    /// </summary>
    public bool vrMode;
    /// <summary>
    /// vr三维展示模式下物体原本的trans
    /// </summary>
    private Transform targetOriginalTransform;

    private Vector3 poisitionVector3;
    private Vector3 rotationVector3;

    private VRTK_InteractableObject interactableObject;
    private VRTK_FixedJointGrabAttach fixedJointGrabAttach;
    /// <summary>
    /// 手柄上的按钮事件脚本
    /// </summary>
    private VRTK_ControllerEvents vce;
    /// <summary>
    /// 手柄上的手柄抓取物体的事件脚本
    /// </summary>
    private VRTK_InteractGrab vig;
    /// <summary>
    /// 手柄上手柄接触物体的事件脚本
    /// </summary>
    private VRTK_InteractTouch vit;
    
    /// <summary>
    /// 右手柄扳机键是否按下
    /// </summary>
    private bool rightClick;
    /// <summary>
    /// 左手柄扳机键是否按下
    /// </summary>
    private bool leftClick;
    /// <summary>
    /// 右手柄侧键是否按下
    /// </summary>
    private bool rightGripPressed;
    /// <summary>
    /// 上一帧的两手柄距离
    /// </summary>
    private float lastControllerDistance;
    /// <summary>
    /// 两个手柄之间的直线距离
    /// </summary>
    private float twoControllerDistance;
    /// <summary>
    /// 左手柄的Trans
    /// </summary>
    private Transform leftControllerTransform;
    /// <summary>
    /// 右手柄的Trans
    /// </summary>
    private Transform rightControllerTransform;
    /// <summary>
    /// vr状态下需要交互的ui
    /// </summary>
    private GameObject vrCanvas;
    /// <summary>
    /// 左手手柄触摸盘临时操作的物体
    /// </summary>
    private GameObject temporaryGameObject;
    /// <summary>
    /// 左手手柄触摸盘临时操作
    /// </summary>
    private bool temporaryOpen;
    /// <summary>
    /// 左手柄触摸盘当前触摸区域
    /// </summary>
    private int leftTouchPadTouchingArea;
    private EventTrigger trigger;
    public GameObject VRCanvas
    {
        get
        {
            if (vrCanvas==null)
            {
                vrCanvas = Instantiate(Resources.Load<GameObject>("PI/VRCanvas"));
                vrCanvas.transform.SetParent(Manager.Instace.LeftControllerGameObject.transform.parent);
                vrCanvas.transform.localPosition = new Vector3(0.01f, 0.07f, 0.3f);
                vrCanvas.transform.localEulerAngles = new Vector3(38f, 2f, 1f);
                VRCanvas.AddComponent<VRTK_UICanvas>();
                vrCanvas.GetComponent<Canvas>().worldCamera = Manager.Instace.cameraEye.GetComponent<Camera>();
                vrCanvas.GetComponent<GraphicRaycaster>().enabled = true;

                vrCanvas.transform.FindChild("DetailedList/Menu/ButtonReduce").GetComponent<Button>().onClick.AddListener(ClearSelected);
                vrCanvas.transform.FindChild("DetailedList/Menu/ButtonOpen").GetComponent<Button>().onClick.AddListener(OpenChildInScene);
                vrCanvas.transform.FindChild("DetailedList/Menu/ButtonReduction").GetComponent<Button>().onClick.AddListener(delegate ()
                    {
                        Reduction(false);
                    });
                vrCanvas.transform.FindChild("DetailedList/Menu/ButtonEndShow").GetComponent<Button>().onClick.AddListener(EndPIShow);
                AddEventTrigger(vrCanvas.transform.FindChild("DetailedList/Menu/Rotate/Button_Up").gameObject,1);
                AddEventTrigger(vrCanvas.transform.FindChild("DetailedList/Menu/Rotate/Button_Down").gameObject,2);
                AddEventTrigger(vrCanvas.transform.FindChild("DetailedList/Menu/Rotate/Button_Left").gameObject,3);
                AddEventTrigger(vrCanvas.transform.FindChild("DetailedList/Menu/Rotate/Button_Right").gameObject,4);
                AddEventTrigger(vrCanvas.transform.FindChild("DetailedList/Menu/Rotate/Button_LD").gameObject,5);
                AddEventTrigger(vrCanvas.transform.FindChild("DetailedList/Menu/Rotate/Button_RD").gameObject,6);
            }
            return vrCanvas;
        }
    }

    public void AddEventTrigger(GameObject go, int a)
    {
        
        trigger = go.GetComponent<EventTrigger>();
        if (trigger==null)
        {
            trigger = go.AddComponent<EventTrigger>();
        }
        
        trigger.triggers = new System.Collections.Generic.List<EventTrigger.Entry>();
        EventTrigger.Entry enter = new EventTrigger.Entry();

        enter.eventID = EventTriggerType.PointerDown;
        enter.callback = new EventTrigger.TriggerEvent();
        UnityAction<BaseEventData> action;
        switch (a)
        {
            case 1:
                action = new UnityAction<BaseEventData>(delegate (BaseEventData baseEvent) { RotateButtonDown(1); });
                break;
            case 2:
                action = new UnityAction<BaseEventData>(delegate (BaseEventData baseEvent) { RotateButtonDown(2); });
                break;
            case 3:
                action = new UnityAction<BaseEventData>(delegate (BaseEventData baseEvent) { RotateButtonDown(3); });
                break;
            case 4:
                action = new UnityAction<BaseEventData>(delegate (BaseEventData baseEvent) { RotateButtonDown(4); });
                break;
            case 5:
                action = new UnityAction<BaseEventData>(delegate (BaseEventData baseEvent) { RotateButtonDown(5); });
                break;
            case 6:
                action = new UnityAction<BaseEventData>(delegate (BaseEventData baseEvent) { RotateButtonDown(6); });
                break;
                default:
                    action = new UnityAction<BaseEventData>(delegate (BaseEventData baseEvent) { RotateButtonDown(0); });
                break;

        }
        
        enter.callback.AddListener(action);
        trigger.triggers.Add(enter);

        enter = new EventTrigger.Entry();
        enter.eventID = EventTriggerType.PointerUp;
        enter.callback = new EventTrigger.TriggerEvent();
        switch (a)
        {
            case 1:
                action = new UnityAction<BaseEventData>(delegate (BaseEventData baseEvent) { RotateButtonUp(1); });
                break;
            case 2:
                action = new UnityAction<BaseEventData>(delegate (BaseEventData baseEvent) { RotateButtonUp(2); });
                break;
            case 3:
                action = new UnityAction<BaseEventData>(delegate (BaseEventData baseEvent) { RotateButtonUp(3); });
                break;
            case 4:
                action = new UnityAction<BaseEventData>(delegate (BaseEventData baseEvent) { RotateButtonUp(4); });
                break;
            case 5:
                action = new UnityAction<BaseEventData>(delegate (BaseEventData baseEvent) { RotateButtonUp(5); }); 
                break;
            case 6:
                action = new UnityAction<BaseEventData>(delegate (BaseEventData baseEvent) { RotateButtonUp(6); });
                break;
            default:
                action = new UnityAction<BaseEventData>(delegate (BaseEventData baseEvent) { RotateButtonUp(0); });
                break;

        }
        enter.callback.AddListener(action);
        trigger.triggers.Add(enter);
    }

    public int LeftTouchPadTouchingArea
    {
        get
        {
            return leftTouchPadTouchingArea;
        }

        set
        {
            if (leftTouchPadTouchingArea!=value)
            {
                leftTouchPadTouchingArea = value;
                //Debug.LogError("触摸区域改变了");
                ShowVrModeUI(value);
            }
           
        }
    }

    /// <summary>
    /// 在vr状态下时重置部分引用的物体
    /// </summary>
    public void SetVrModeObjects()
    {
        PIPanel = VRCanvas.transform.FindChild("DetailedList").gameObject;
        PIPanelContent = PIPanel.transform.FindChild("Viewport/Content");

        IntroduceTextTransform = VRCanvas.transform.FindChild("IntroduceText");
        IntroduceVideoGameObject = VRCanvas.transform.FindChild("Video_BG").gameObject;


    }
    public void InitVrMode()
    {
        //注册右手柄相关事件
        vce = Manager.Instace.ctrllerEventsR;
        vce.TriggerClicked += new ControllerInteractionEventHandler(RightTriggerClick);
        vce.TriggerUnclicked += new ControllerInteractionEventHandler(RightTriggerUnclick);
        vce.GripPressed += new ControllerInteractionEventHandler(RightGripPressed);
        vce.GripReleased +=new ControllerInteractionEventHandler(RightGripReleased);
        //注册左手柄相关事件
        vce = Manager.Instace.ctrllerEventsL;
        vce.TriggerClicked += new ControllerInteractionEventHandler(LeftTriggerClick);
        vce.TriggerUnclicked += new ControllerInteractionEventHandler(LeftTriggerUnclick);
        vce.TouchpadPressed+=new ControllerInteractionEventHandler(LeftTouchPadPressed);
        vce.TouchpadReleased += new ControllerInteractionEventHandler(LeftTouchPadRelease);
        vce.TouchpadAxisChanged +=new ControllerInteractionEventHandler(LeftTouchPadTouchedChanged);
        vce.TouchpadTouchEnd+=new ControllerInteractionEventHandler(LeftTouchpadTouchEnd);
        leftControllerTransform = Manager.Instace.LeftControllerGameObject.transform;
        rightControllerTransform = Manager.Instace.RightControllerGameObject.transform;
        //注册右手柄抓取物体相关事件
        vig = Manager.Instace.RightControllerGameObject.GetComponent<VRTK_InteractGrab>();
        vig.ControllerGrabInteractableObject += new ObjectInteractEventHandler(RightGraspGameobject);
        //注册右手柄触摸物体相关事件
        vit = Manager.Instace.RightControllerGameObject.GetComponent<VRTK_InteractTouch>();
        vit.ControllerTouchInteractableObject+=new ObjectInteractEventHandler(RightTouchGameObject);
        vit.ControllerUntouchInteractableObject+=new ObjectInteractEventHandler(RightUnTouchGameobject);
    }
    /// <summary>
    /// 右手柄按下扳机键
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void RightTriggerClick(object sender, ControllerInteractionEventArgs e)
    {
        rightClick = true;
    }
    /// <summary>
    /// 左手柄按下扳机键
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void LeftTriggerClick(object sender, ControllerInteractionEventArgs e)
    {
        leftClick = true;
    }
    /// <summary>
    /// 右手柄松开扳机键
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void RightTriggerUnclick(object sender, ControllerInteractionEventArgs e)
    {
        rightClick = false;
    }
    /// <summary>
    /// 左手柄松开扳机键
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void LeftTriggerUnclick(object sender, ControllerInteractionEventArgs e)
    {
        leftClick = false;
    }
    /// <summary>
    /// 左手柄触摸盘触摸位置改变时
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void LeftTouchPadTouchedChanged(object sender, ControllerInteractionEventArgs e)
    {
        if (e.touchpadAngle >= 0 && e.touchpadAngle < 90)
        {
            LeftTouchPadTouchingArea = 1;
        }
        else if (e.touchpadAngle >= 90 && e.touchpadAngle < 180)
        {
            LeftTouchPadTouchingArea = 2;
        }
        else if (e.touchpadAngle >= 180 && e.touchpadAngle < 270)
        {
            LeftTouchPadTouchingArea = 3;
        }
        else if (e.touchpadAngle >= 270 && e.touchpadAngle < 360)
        {
            LeftTouchPadTouchingArea = 4;
        }
    }
    /// <summary>
    /// 左手柄触摸盘停止触摸
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void LeftTouchpadTouchEnd(object sender, ControllerInteractionEventArgs e)
    {
        //Debug.LogError("左手柄触摸盘停止触摸");
        //ShowVrModeUI(0, false);
        LeftTouchPadTouchingArea = 0;
    }
    /// <summary>
    /// 左手柄的触摸盘被按下
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void LeftTouchPadPressed(object sender, ControllerInteractionEventArgs e)
    {
        ShowVrModeUI(LeftTouchPadTouchingArea, true);
    }
    /// <summary>
    /// 左手柄触摸盘被按下后的松开
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void LeftTouchPadRelease(object sender, ControllerInteractionEventArgs e)
    {
        //Debug.LogError("左手柄触摸盘被按下后的松开");
    }
    /// <summary>
    /// 右手柄抓住了物体
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void RightGraspGameobject(object sender, ObjectInteractEventArgs e)
    {
        //Debug.LogError("右手柄抓住了物体："+e.target.name);
        if (e.target!=null)
        {
            GoAndChildIndex[e.target].ShowTarget();

            ShowIntroduce(e.target);
        }
       
        //StartCoroutine(DoWaitTime());
    }
    /// <summary>
    /// 右手柄接触到物体
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void RightTouchGameObject(object sender, ObjectInteractEventArgs e)
    {
        //Debug.LogError("右手柄接触到了物体：" + e.target.name);
        GoAndChildIndex[e.target].ShowSuspensionTarget();
    }
    /// <summary>
    /// 右手柄离开物体
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void RightUnTouchGameobject(object sender, ObjectInteractEventArgs e)
    {
        //Debug.LogError("右手柄离开了物体：" + e.target.name);
        GoAndChildIndex[e.target].StopShowSuspensionTarget();
    }
    /// <summary>
    /// 右手柄侧键按下时
    /// </summary>
    private void RightGripPressed(object sender, ControllerInteractionEventArgs e)
    {
        //Debug.LogError("右手柄侧键按下");
        rightGripPressed = true;
    }
    /// <summary>
    /// 右手柄侧键松开
    /// </summary>
    private void RightGripReleased(object sender, ControllerInteractionEventArgs e)
    {
        //Debug.LogError("右手柄侧键松开");
        rightGripPressed = false;
    }

    //通过两手柄控制目标物体与自己的距离
    private void ControlDistance()
    {
        if (rightClick && leftClick)
        {
            twoControllerDistance = Vector3.Distance(leftControllerTransform.position, rightControllerTransform.position);
            if (lastControllerDistance != 0f)
            {
                if (twoControllerDistance - lastControllerDistance >= 0.1f)
                {
                    ShortenDistance();
                    lastControllerDistance = twoControllerDistance;
                }
                else if (twoControllerDistance - lastControllerDistance <= -0.1f)
                {
                    IncreaseDistance();
                    lastControllerDistance = twoControllerDistance;
                }
            }
            else
            {
                lastControllerDistance = twoControllerDistance;
            }
        }
        else
        {
            lastControllerDistance = 0f;
        }
    }
    /// <summary>
    /// 将显示物体信息的列表的ui移动到世界canvas下
    /// </summary>
    private void MoveVrModeUI()
    {
        VRCanvas.transform.SetParent(Manager.Instace.LeftControllerGameObject.transform.parent);
        VRCanvas.transform.localPosition = new Vector3(0.01f,0.07f,0.3f);
        vrCanvas.transform.localEulerAngles=new Vector3(38f,2f,1f);
        
        VRCanvas.SetActive(false);
        VRCanvas.transform.eulerAngles = new Vector3(0, 180, 0);
        PIPanel.transform.SetParent(VRCanvas.transform);
        PIPanel.transform.localPosition = Vector3.zero;
        PIPanel.transform.localScale = Vector3.one;
        PIPanel.transform.localEulerAngles = Vector3.zero;
    }
    /// <summary>
    /// 通过手柄操作显示或隐藏显示物体信息列表的ui
    /// </summary>
    public void ShowVrModeUI(int area,bool y = false)
    {
        
        if (!y)
        {
            switch (area)
            {
                case 0:
                    //取消操作
                    if (temporaryGameObject!=null)
                    {
                        //Debug.LogError("清除临时操作："+temporaryGameObject.name+"  "+temporaryOpen);
                        temporaryGameObject.SetActive(!temporaryOpen);
                        temporaryGameObject = null;
                    }
                   
                    break;
                case 1:
                    //显示文本介绍
                    if (temporaryGameObject != null)
                    {
                        temporaryGameObject.SetActive(!temporaryOpen);
                    }
                    IntroduceTextTransform.gameObject.SetActive(!IntroduceTextTransform.gameObject.activeSelf);
                    temporaryGameObject = IntroduceTextTransform.gameObject;
                    temporaryOpen = IntroduceTextTransform.gameObject.activeSelf;
                    break;
                case 2:
                    //显示视频介绍
                    if (temporaryGameObject != null)
                    {
                        temporaryGameObject.SetActive(!temporaryOpen);
                    }
                    IntroduceVideoGameObject.SetActive(!IntroduceVideoGameObject.activeSelf);
                    temporaryGameObject = IntroduceVideoGameObject;
                    temporaryOpen = IntroduceVideoGameObject.activeSelf;
                    break;
                case 3:
                    //打开列表
                    if (temporaryGameObject != null)
                    {
                        temporaryGameObject.SetActive(!temporaryOpen);
                    }
                    PIPanel.SetActive(!PIPanel.activeSelf);
                    temporaryGameObject = PIPanel;
                    temporaryOpen = PIPanel.activeSelf;
                    break;
                case 4://暂定
                    break;
                default:
                    break;
            }
        }
        else
        {
            temporaryGameObject = null;
            //Debug.LogError("确认了操作");
        }
        
        //VRCanvas.transform.position = Manager.Instace.cameraEye.transform.position + new Vector3(0, 0, -500f);
            //VRCanvas.SetActive(false);
            //vrUIShow = false;
        
    }

    private void ShortenDistance()
    {
        Target.transform.position+=new Vector3(0,0,0.5f);
    }

    private void IncreaseDistance()
    {
        Target.transform.position += new Vector3(0, 0, -0.5f);
    }

    IEnumerator DoWaitToLoadVideo(string path)
    {
        while (true)
        {
            if (IntroduceVideoGameObject.activeSelf)
            {
                break;
            }
            yield return null;
        }
        yield return null;
        IntroduceVideoGameObject.GetComponentInChildren<MediaPlayer>().OpenVideoFromFile(MediaPlayer.FileLocation.RelativeToDataFolder, path, false);
    }

    IEnumerator DoWaitToLoadImage(string path,Image targetImg)
    {
        WWW www = new WWW(@"file://"+path);
        yield return www;
        Texture2D tex2d = www.texture;
        targetImg.sprite = Sprite.Create(tex2d, new Rect(0, 0, tex2d.width, tex2d.height), Vector2.zero, 100, 1, SpriteMeshType.Tight);
    }


    #endregion

    private string childpath;
    public string GetChildFullName(Transform transform)
    {
        childpath = "";
        GetChildFull(transform);
        Debug.LogError(childpath);
        return childpath;
    }
    
    void GetChildFull(Transform transform)
    {
        if (transform.parent.parent != Manager.Instace.parent)
        {
            childpath = "/"+transform.name + childpath;
            GetChildFull(transform.parent);
        }
        else
        {
            childpath = transform.name + childpath;
        }
        
    }

}
/// <summary>
/// 物体与位置以及旋转的临时记录
/// </summary>
class GameobjectAndPosition
{
    public GameObject gameobject;
    public Vector3 position;
    public Vector3 localEulerAngles;
    public GameobjectAndPosition() { }
    public GameobjectAndPosition(GameObject obj, Vector3 pos, Vector3 rot)
    {
        gameobject = obj;
        position = pos;
        localEulerAngles = rot;
    }
}
/// <summary>
/// 记录物体名字和介绍的类
/// </summary>
[System.Serializable]
public class ChildAndIntroduce
{
    public string childName;
    public string introduce;
    public string videoPath;
    public string imagePath;
    public ChildAndIntroduce() { }
    public ChildAndIntroduce(string cn, string itro,string ipath, string vpath)
    {
        childName = cn;
        introduce = itro;
        imagePath = ipath;
        videoPath = vpath;
    }
}
