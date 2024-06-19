using HighlightingSystem;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using Brush;
using UnityEngine.UI;
using System;

public class BrushManager : MonoBehaviour
{
    public static BrushManager Instance;
    /// <summary>
    /// 绘制的物品的父物体
    /// </summary>
    public Transform BrushParent;

    /// <summary>
    /// 绘制模式
    /// </summary>
    public bool BrushMode { get; set; }
    /// <summary>
    /// 切换贴图的目标材质球
    /// </summary>
    public ChangeTextureMaterial ChangeTextureMaterial { get; set; }

    public ChangeTextureMaterial ChangeTextureMarerial_G { get; set; }


    /// <summary>
    /// 开始的点
    /// </summary>
    private Vector3 startPoint = new Vector3(0, -500, 0);
    /// <summary>
    /// 结束的点
    /// </summary>
    private Vector3 endPoint = new Vector3(0, -500, 0);

    private GameObject normalWallPrefab;

    private GameObject normalRoadPrefab;

    private Transform LastBrushedPoint;
    /// <summary>
    /// 是否连续画
    /// </summary>
    private bool continuity;
    /// <summary>
    /// 附着到参考线上
    /// </summary>
    private bool attachToGuides;

    public Canvas canvas;
    public Transform DistanceUIParent { get; set; }
   

    /// <summary>
    /// 上一个在绘制时被选中的已经绘制好的墙体
    /// </summary>
    private BrushedWall lastBrushedWall;

    public List<BrushedWall> BrushedWallList = new List<BrushedWall>();

    public List<Transform> BrushedPointList = new List<Transform>();

    public List<BrushedRoad> BrushedRoadList = new List<BrushedRoad>();

    private BrushMesh brushMesh;
    /// <summary>
    /// 墙壁属性设置页面
    /// </summary>
    public BrushWallInformationSet BrushWallSettings;
    /// <summary>
    /// 路面属性设置页面
    /// </summary>
    public BrushRoadInformationSet BrushRoadSettings;
    /// <summary>
    /// 门的属性设置页面
    /// </summary>
    public BrushDoorInformationSet BrushDoorSettings;
    /// <summary>
    /// 窗的属性设置页面
    /// </summary>
    public BrushWindowInformationSet BrushWindowSettings;
    /// <summary>
    /// 地面的属性设置页面
    /// </summary>
    public BrushGroundInformationSet BrushGroundSettings;
    /// <summary>
    /// 建筑的属性设置页面
    /// </summary>
    public BrushBuildingInformationSet BrushBuildingInformationSettings;
    /// <summary>
    /// 楼层的属性设置页面
    /// </summary>
    public BrushFloorInformationSet BrushFloorInformationSettings;

    public Button EndEditFloorBtn;
    /// <summary>
    /// 选中的物体
    /// </summary>
    private GameObject SelectObj;

    //HighlighterController HC;

    RaycastHit raycastHit;

    LayerMask BrushLayer = (1 << 8) | (1 << 13);
    LayerMask BrushGroundLayer = 1 << 8;
    LayerMask BrushObj = 1 << 13;

    public LayerMask EverythingLayer;

    public List<BrushedBuilding> BrushedBuildingList = new List<BrushedBuilding>();
    

    /// <summary>
    /// 建筑
    /// </summary>
    private GameObject building;

    public BrushedFloor EditFloor { get; set; }


    void Awake()
    {
        Instance = this;
        DistanceUIParent = canvas.transform.Find("BrushDistanceText");
        normalWallPrefab = Resources.Load<GameObject>("Prefabs/Draw/NormalWall");
        normalRoadPrefab = Resources.Load<GameObject>("Prefabs/Draw/NormalRoad");
        brushMesh = GetComponent<BrushMesh>();
    }
    // Use this for initialization
    void Start()
    {
        
    }

    public void StartDraw()
    {
        if (!BrushMode)
        {
            if (EditFloor!=null)
            {
                StartDrawBuildingWall(false);
            }
            else
            {
                StartDraw(false);
            }
        }
    }


    // Update is called once per frame
    void Update()
    {

        //if (!BrushMode && Input.GetKeyDown(KeyCode.K))
        //{
        //    if (EditFloor == null)
        //    {
        //        StartDrawRoad(false);
        //    }
        //}
        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    bool t = false;
        //    List<Vector3> posList = new List<Vector3>();
        //    for (int i = 0; i < BrushedWallList.Count; i++)
        //    {
        //        if (!t && BrushedWallList[i].GetComponent<BrushedWall>().StartPoint != BrushedWallList[BrushedWallList.Count - 1].GetComponent<BrushedWall>().EndPoint)
        //        {
        //            continue;
        //        }
        //        else if (!t && BrushedWallList[i].GetComponent<BrushedWall>().StartPoint == BrushedWallList[BrushedWallList.Count - 1].GetComponent<BrushedWall>().EndPoint)
        //        {
        //            t = true;
        //            posList.Add(BrushedWallList[i].GetComponent<BrushedWall>().StartPoint.position);
        //        }
        //        else if (t)
        //        {
        //            posList.Add(BrushedWallList[i].GetComponent<BrushedWall>().StartPoint.position);
        //        }
        //    }
        //    //Debug.LogError("posList.Count" + posList.Count);
        //    brushMesh.BrushOneMesh(posList);
        //}
        //if (Input.GetKeyDown(KeyCode.P))
        //{
        //    SaveAllBrushObjInformation();

        //    StartCoroutine(DoAutoCreatWalls());
        //}
        //if (Input.GetKeyDown(KeyCode.B))
        //{
        //    //创建建筑地基
        //    CreatBudindingFoundation();
        //}
        //if (Input.GetKeyDown(KeyCode.I))
        //{
        //    //建筑地基创建完成
        //    CompleteBuildingFoundation();
        //}
        if (!attachToGuides && Input.GetKeyDown(KeyCode.V))
        {
            attachToGuides = true;
        }
        else if (attachToGuides && Input.GetKeyUp(KeyCode.V))
        {
            attachToGuides = false;
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (ChangeTextureMaterial!=null)
            {
                ChangeTextureMaterial = null;
            }
        }
        //if (Input.GetKeyDown(KeyCode.Z))
        //{
        //    //AutoCreatBrushedObjsByInformation();
        //}

        //如果不是绘制模式下点击鼠标
        if (!BrushMode && Input.GetMouseButtonUp(0))
        {
            
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                raycastHit = BrushRayCastCheck.MouseCheckBrushObj(BrushObj);

                if (raycastHit.collider != null)
                {   //判断点击的是不是建筑中的物体
                    if (raycastHit.collider.GetComponentInParent<BrushedBuilding>()&&EditFloor==null)
                    {
                        //不能直接替换建筑中物体的贴图，要在楼层编辑中替换
                        if (ChangeTextureMaterial != null)
                        {
                            return;
                        }
                        BrushedBuilding building = raycastHit.collider.GetComponentInParent<BrushedBuilding>();
                        if (!building.GetComponent<FlashingController>())
                        {
                            if (SelectObj != null && SelectObj != raycastHit.collider.gameObject)
                            {
                                Destroy(SelectObj.GetComponent<FlashingController>());
                                Destroy(SelectObj.GetComponent<Highlighter>());
                                SelectObj = raycastHit.collider.gameObject;
                                SelectObj.AddComponent<FlashingController>();
                            }
                            else if (SelectObj == null)
                            {
                                SelectObj = building.gameObject;
                                SelectObj.AddComponent<FlashingController>();
                            }
                        }
                        OpenSettingPanel(BrushBuildingInformationSettings.gameObject);
                        Manager.Instace.dragUIMoveObjOnGround.Init(raycastHit.collider.GetComponentInParent<BrushedBuilding>().transform);
                        Manager.Instace.dragUIMoveObjOnGround.gameObject.SetActive(true);
                        //for (int i = 0; i < building.Floor[0].walls.Count; i++)
                        //{
                        //    building.Floor[0].walls[i].StartPoint.SetParent(building.Floor[0].transform);
                        //    building.Floor[0].walls[i].EndPoint.SetParent(building.Floor[0].transform);
                        //}
                        BrushBuildingInformationSettings.Init(building);
                    }
                    else
                    {
                        if (ChangeTextureMaterial != null)
                        {
                            //点击到的是墙壁
                            if (raycastHit.collider != null && raycastHit.collider.tag == "BrushWall")
                            {
                                if (ChangeTextureMaterial.type == ChangeTextureMaterialType.wall)
                                {
                                    //替换墙壁的贴图
                                    raycastHit.collider.GetComponent<MeshRenderer>().material = ChangeTextureMaterial.material;
                                }
                                
                            }//点击到的是地面
                            else if (raycastHit.collider != null && raycastHit.collider.tag == "BrushGround")
                            {
                                if (ChangeTextureMaterial.type == ChangeTextureMaterialType.wall)
                                {
                                    //替换地面的贴图
                                    raycastHit.collider.GetComponent<MeshRenderer>().material = ChangeTextureMarerial_G.material;
                                }
                                    
                            }//点击到的是门,门没有贴图的替换
                            else if (raycastHit.collider != null && raycastHit.collider.tag == "BrushDoor")
                            {
                                if (ChangeTextureMaterial.type == ChangeTextureMaterialType.door)
                                {
                                    //替换地面的贴图
                                    raycastHit.collider.GetComponent<MeshRenderer>().material = ChangeTextureMaterial.material;
                                }
                            }
                        }
                        else
                        {
                            //todo 给物体添加高光描边
                            if (raycastHit.collider != null && !raycastHit.collider.gameObject.GetComponent<FlashingController>())
                            {
                                if (SelectObj != null && SelectObj != raycastHit.collider.gameObject)
                                {
                                    Destroy(SelectObj.GetComponent<FlashingController>());
                                    Destroy(SelectObj.GetComponent<Highlighter>());
                                    SelectObj = raycastHit.collider.gameObject;
                                    SelectObj.AddComponent<FlashingController>();
                                }
                                else if (SelectObj == null)
                                {
                                    SelectObj = raycastHit.collider.gameObject;
                                    SelectObj.AddComponent<FlashingController>();
                                }

                            }
                            //点击到的是墙壁
                            if (raycastHit.collider != null && raycastHit.collider.tag == "BrushWall")
                            {
                                //获取脚本，显示相关的属性信息
                                BrushedWall bw = raycastHit.collider.GetComponent<BrushedWall>();
                                BrushWallSettings.Init(bw);
                                OpenSettingPanel(BrushWallSettings.gameObject);
                            }//点击到的是地面
                            else if (raycastHit.collider != null && raycastHit.collider.tag == "BrushGround")
                            {
                                BrushedGround bg = raycastHit.collider.GetComponent<BrushedGround>();
                                BrushGroundSettings.Init(bg);
                                OpenSettingPanel(BrushGroundSettings.gameObject);

                            }//点击到的是门
                            else if (raycastHit.collider != null && raycastHit.collider.tag == "BrushDoor")
                            {
                                BrushedDoor bd = raycastHit.collider.GetComponent<BrushedDoor>();
                                BrushDoorSettings.Init(bd);
                                OpenSettingPanel(BrushDoorSettings.gameObject);
                            }//点击到的是窗
                            else if (raycastHit.collider != null && raycastHit.collider.tag == "BrushWindow")
                            {
                                BrushedWindow bw = raycastHit.collider.GetComponent<BrushedWindow>();
                                BrushWindowSettings.Init(bw);
                                OpenSettingPanel(BrushWindowSettings.gameObject);
                            }//点击到的是路面
                            else if (raycastHit.collider != null && raycastHit.collider.tag == "BrushRoad")
                            {
                                BrushedRoad br = raycastHit.collider.GetComponent<BrushedRoad>();
                                BrushRoadSettings.Init(br);
                                OpenSettingPanel(BrushRoadSettings.gameObject);
                            }
                        }
                        

                    }
                }
                else
                {
                    OpenSettingPanel(null);
                    if (SelectObj != null && SelectObj.GetComponent<FlashingController>())
                    {
                        Destroy(SelectObj.GetComponent<FlashingController>());
                        Destroy(SelectObj.GetComponent<Highlighter>());
                        SelectObj = null;
                        //HC = null;
                    }
                }
            }
        }
        if (CanCreateInteriorWalls)
        {
            if (Input.GetKeyDown(KeyCode.G)&& creatingInteriorwall == false)
            {
                StartCoroutine(DoDrawInteriorWalls());
            }
        }
    }

    /// <summary>
    /// 获取和某一段墙壁的startpoint相连的所有墙壁
    /// </summary>
    /// <returns></returns>
    public BrushedWall[] GetLastWall(BrushedWall wall)
    {
        List<BrushedWall> brushedWalls = new List<BrushedWall>();
        for (int i = 0; i < BrushedWallList.Count; i++)
        {
            if (BrushedWallList[i] == wall)
            {
                continue;
            }
            if (BrushedWallList[i].StartPoint == wall.StartPoint || BrushedWallList[i].EndPoint == wall.StartPoint)
            {
                brushedWalls.Add(BrushedWallList[i]);
            }
        }
        return brushedWalls.ToArray();
    }
    /// <summary>
    /// 获取某一段墙壁连接的后一段墙壁
    /// </summary>
    /// <returns></returns>
    public BrushedWall[] GetNextWall(BrushedWall wall)
    {
        List<BrushedWall> brushedWalls = new List<BrushedWall>();
        for (int i = 0; i < BrushedWallList.Count; i++)
        {
            if (BrushedWallList[i] == wall)
            {
                continue;
            }
            if (BrushedWallList[i].StartPoint == wall.EndPoint || BrushedWallList[i].EndPoint == wall.EndPoint)
            {
                brushedWalls.Add(BrushedWallList[i]);
            }
        }
        return brushedWalls.ToArray();
    }

    public Vector2 WorldToUGui(Vector3 vector3)
    {
        Vector3 screenpoint = Camera.main.WorldToScreenPoint(vector3);
        //Vector3 worldPoint;
        Vector3 UIPoint = (screenpoint - (new Vector3(Screen.width, Screen.height, 0) / 2)) * 800 / Screen.width;

        return UIPoint;
    }


    public Transform NewPoint(Vector3 vector3, bool end = false)
    {
        if (LastBrushedPoint != null && LastBrushedPoint.localPosition == vector3 && !end)
        {
            return LastBrushedPoint;
        }
        GameObject newpoint = new GameObject("point");
        newpoint.transform.SetParent(transform.Find("Points"));
        newpoint.transform.localPosition = vector3;
        LastBrushedPoint = newpoint.transform;
        return newpoint.transform;
    }
    public Transform NewBuildingPoint(Vector3 vector3,Transform parent,bool foundation, bool end = false)
    {
        if (LastBrushedPoint != null && LastBrushedPoint.localPosition == vector3 && !end)
        {
            return LastBrushedPoint;
        }
        GameObject newpoint = new GameObject("point");
        newpoint.transform.SetParent(parent);
        if (foundation)
        {
            newpoint.transform.localPosition = vector3;
        }
        else
        {
            newpoint.transform.position = vector3;
        }
        LastBrushedPoint = newpoint.transform;
        return newpoint.transform;
    }
    /// <summary>
    /// 将一堵墙分割为两堵相连的墙壁
    /// </summary>
    public void Separate(BrushedWall wall)
    {
        GameObject nextWall = Instantiate(normalWallPrefab);
        nextWall.GetComponent<BrushedWall>().StartPoint = wall.MiddlePoint;
        nextWall.GetComponent<BrushedWall>().EndPoint = wall.EndPoint;
        nextWall.transform.SetParent(BrushParent);
        nextWall.layer = 13;
        nextWall.transform.localScale = new Vector3(wall.transform.localScale.x, wall.transform.localScale.y, Vector3.Distance(wall.MiddlePoint.position, wall.EndPoint.position));
        nextWall.transform.localPosition = new Vector3((wall.MiddlePoint.position.x + wall.EndPoint.position.x) / 2, wall.transform.localScale.y / 2, (wall.MiddlePoint.position.z + wall.EndPoint.position.z) / 2);
        nextWall.transform.LookAt(wall.EndPoint.position + new Vector3(0, (wall.transform.localScale.y / 2), 0));

        wall.transform.localScale = new Vector3(wall.transform.localScale.x, wall.transform.localScale.y, Vector3.Distance(wall.StartPoint.position, wall.MiddlePoint.position));
        wall.transform.localPosition = new Vector3((wall.StartPoint.position.x + wall.MiddlePoint.position.x) / 2, wall.transform.localScale.y / 2, (wall.StartPoint.position.z + wall.MiddlePoint.position.z) / 2);
        wall.EndPoint = wall.MiddlePoint;
        wall.MiddlePoint = null;

        BrushedWallList.Insert(BrushedWallList.LastIndexOf(wall) + 1, nextWall.GetComponent<BrushedWall>());

    }

    public void StartDraw(bool drawing)
    {
        BrushMode = true;
        StartCoroutine(DoDrawWall(drawing));
    }
    public void StartDrawBuildingWall(bool drawing)
    {
        BrushMode = true;
        StartCoroutine(DoDrawBuildingFloorWall(drawing,EditFloor));
    }

    IEnumerator DoDrawWall(bool drawing)
    {
        GameObject wall;
        BrushedWall bw;
        bool mouseCheckWall = false;
        //创建默认的墙壁模型
        wall = Instantiate(normalWallPrefab);
        wall.transform.SetParent(BrushParent);
        wall.transform.localScale = new Vector3(wall.transform.localScale.x, wall.transform.localScale.y, 0f);
        bw = wall.GetComponent<BrushedWall>();
        if (continuity && drawing)
        {
            bw.StartPoint = NewPoint(startPoint);
        }
        else
        {
            while (!drawing)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (attachToGuides)
                    {
                        startPoint = BrushRayCastCheck.MouseCheckGround(BrushGroundLayer).point;
                        startPoint = new Vector3(Mathf.Round(startPoint.x), 0, Mathf.Round(startPoint.z));
                    }
                    else
                    {
                        raycastHit = BrushRayCastCheck.MouseCheckGround(BrushLayer);
                        //鼠标检测到的是绘制的墙壁
                        if (raycastHit.collider != null && raycastHit.collider.gameObject.tag == "BrushWall")
                        {
                            startPoint = raycastHit.point - raycastHit.normal * raycastHit.collider.transform.localScale.x / 2;
                            startPoint = new Vector3(startPoint.x, 0, startPoint.z);
                        }//鼠标检测到的是地面
                        else if (raycastHit.collider != null && raycastHit.collider.gameObject.layer == 8)
                        {
                            startPoint = raycastHit.point;
                        }
                        else
                        {
                            startPoint = BrushRayCastCheck.NullVector3;
                        }
                    }
                    if (startPoint != BrushRayCastCheck.NullVector3)
                    {
                        drawing = true;
                        bw.StartPoint = NewPoint(startPoint);
                    }
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    Destroy(wall);
                    continuity = false;
                    break;
                }
                yield return null;
            }
        }
        while (drawing)
        {
            //附着到网格
            if (attachToGuides)
            {
                endPoint = BrushRayCastCheck.MouseCheckGround(BrushGroundLayer).point;
                endPoint = new Vector3(Mathf.Round(endPoint.x), 0, Mathf.Round(endPoint.z));
            }//不附着网格
            else
            {
                raycastHit = BrushRayCastCheck.MouseCheckGround(BrushLayer);
                //鼠标检测到的是绘制的墙壁
                if (raycastHit.collider != null && raycastHit.collider.gameObject.tag == "BrushWall")
                {
                    endPoint = raycastHit.point - raycastHit.normal * raycastHit.collider.transform.localScale.x / 2;
                    endPoint = new Vector3(endPoint.x, 0, endPoint.z);
                    mouseCheckWall = true;
                    if (lastBrushedWall == null)
                    {
                        lastBrushedWall = raycastHit.collider.GetComponent<BrushedWall>();
                        if (bw.EndPoint == null)
                        {
                            lastBrushedWall.MiddlePoint = null;
                        }
                        else
                        {
                            lastBrushedWall.MiddlePoint = bw.EndPoint;
                        }
                    }
                    else
                    {
                        if (lastBrushedWall != raycastHit.collider.GetComponent<BrushedWall>())
                        {
                            raycastHit.collider.GetComponent<BrushedWall>().MiddlePoint = lastBrushedWall.MiddlePoint;
                            lastBrushedWall.MiddlePoint = null;
                            lastBrushedWall = raycastHit.collider.GetComponent<BrushedWall>();
                            if (bw.EndPoint == null)
                            {
                                lastBrushedWall.MiddlePoint = null;
                            }
                            else
                            {
                                lastBrushedWall.MiddlePoint = bw.EndPoint;
                            }
                        }
                        else
                        {
                            if (bw.EndPoint == null)
                            {
                                lastBrushedWall.MiddlePoint = null;
                            }
                            else
                            {
                                lastBrushedWall.MiddlePoint = bw.EndPoint;
                            }
                        }
                    }
                }//鼠标检测到的是地面
                else if (raycastHit.collider != null && raycastHit.collider.gameObject.layer == 8)
                {
                    endPoint = raycastHit.point;
                    mouseCheckWall = false;
                    if (lastBrushedWall != null)
                    {
                        lastBrushedWall.MiddlePoint = null;
                        lastBrushedWall = null;
                    }
                }
                else
                {
                    endPoint = BrushRayCastCheck.NullVector3;
                }
            }
            if (endPoint != BrushRayCastCheck.NullVector3)
            {
                wall.transform.localPosition = new Vector3((startPoint.x + endPoint.x) / 2, wall.transform.localScale.y / 2, (startPoint.z + endPoint.z) / 2);
                wall.transform.localScale = new Vector3(wall.transform.localScale.x, wall.transform.localScale.y, Vector3.Distance(startPoint, endPoint));
                wall.transform.LookAt(endPoint + new Vector3(0, (wall.transform.localScale.y / 2), 0));
                if (bw.EndPoint == null)
                {
                    bw.EndPoint = NewPoint(endPoint, true);
                }
                else
                {
                    bw.EndPoint.position = endPoint;
                }

                if (Input.GetMouseButtonDown(0))
                {

                    drawing = false;
                    startPoint = endPoint;
                    continuity = true;
                    BrushedWallList.Add(bw);
                    if (BrushedWallList.Count >= 2)
                    {
                        BrushedWallList[BrushedWallList.Count - 2].gameObject.layer = 13;
                    }
                    if (mouseCheckWall)
                    {
                        lastBrushedWall.Separate();
                    }
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    Destroy(bw.TextObj);
                    Destroy(bw.EndPoint.gameObject);
                    Destroy(wall);
                    drawing = false;
                    startPoint = BrushRayCastCheck.NullVector3;
                    endPoint = BrushRayCastCheck.NullVector3;
                    continuity = false;
                    if (BrushedWallList.Count >= 1)
                    {
                        BrushedWallList[BrushedWallList.Count - 1].gameObject.layer = 13;
                    }
                }
            }
            yield return null;
        }
        if (continuity)
        {
            StartDraw(true);
        }
        else
        {
            BrushMode = false;
        }
    }
    /// <summary>
    /// 根据数据自动创建
    /// </summary>
    public IEnumerator DoAutoCreatWalls()
    {
        if (Manager.Instace.allDataInformation.brushedObjInformation.walls != null && Manager.Instace.allDataInformation.brushedObjInformation.walls.Count > 0)
        {
            for (int i = 0; i < Manager.Instace.allDataInformation.brushedObjInformation.walls.Count; i++)
            {
                CreateWallByInformation(Manager.Instace.allDataInformation.brushedObjInformation.walls[i]);
                yield return null;
            }
        }
    }

    

    public void CreateWallByInformation(Wall wallinfo)
    {
        GameObject wall = Instantiate(normalWallPrefab);
        BrushedWall bw = wall.GetComponent<BrushedWall>();
        bw.StartPoint = NewPoint(NumericalConversion.MySerializableVector3ToV3(wallinfo.startPoint));
        bw.EndPoint = NewPoint(NumericalConversion.MySerializableVector3ToV3(wallinfo.endPoint));
        wall.GetComponent<MeshRenderer>().material = WallTexture.LoadWallTexture(wallinfo.style).material;
        wall.transform.localScale = new Vector3(wallinfo.width, wallinfo.height, Vector3.Distance(NumericalConversion.MySerializableVector3ToV3(wallinfo.startPoint), NumericalConversion.MySerializableVector3ToV3(wallinfo.endPoint)));
        wall.transform.SetParent(BrushParent);
        wall.transform.localPosition = (bw.StartPoint.localPosition + bw.EndPoint.localPosition) / 2 + new Vector3(0, wall.transform.localScale.y / 2, 0);
        wall.transform.LookAt(bw.EndPoint.localPosition + new Vector3(0, wall.transform.localScale.y / 2, 0));
        wall.layer = 13;
        if (wallinfo.doors != null && wallinfo.doors.Count > 0)
        {
            for (int i = 0; i < wallinfo.doors.Count; i++)
            {
                Transform door = BooleanRtManager.Instance.CreateDoorByInofrmation(wallinfo.doors[i],bw);
                //door.localRotation = wall.transform.localRotation;
                bw.doors.Add(door.GetComponent<BrushedDoor>());
            }
            
        }
        if (wallinfo.windows!=null&&wallinfo.windows.Count>0)
        {
            for (int i = 0; i < wallinfo.windows.Count; i++)
            {
                Transform window = BooleanRtManager.Instance.CreateWindowByInformation(wallinfo.windows[i],bw);
                //window.localRotation = wall.transform.localRotation;
                bw.windows.Add(window.GetComponent<BrushedWindow>());
            }
        }
        //将墙壁扣一下
        BooleanRtManager.Instance.ResetDoorsAndWindowsOfWall(bw);
        BrushedWallList.Add(bw);
    }
    public void CreateFloorWallByInformation(Wall wallinfo, BrushedFloor brushedFloor)
    {
        GameObject wall = Instantiate(normalWallPrefab);
        BrushedWall bw = wall.GetComponent<BrushedWall>();
        wall.GetComponent<MeshRenderer>().material = WallTexture.LoadWallTexture(wallinfo.style).material;
        wallinfo.startPoint = new MySerializableVector3(wallinfo.startPoint.X, 0, wallinfo.startPoint.Z);
        wallinfo.endPoint = new MySerializableVector3(wallinfo.endPoint.X, 0, wallinfo.endPoint.Z);
        bw.StartPoint = NewBuildingPoint(NumericalConversion.MySerializableVector3ToV3(wallinfo.startPoint),brushedFloor.transform,false);
        //bw.StartPoint.SetParent(brushedFloor.transform);
        bw.EndPoint = NewBuildingPoint(NumericalConversion.MySerializableVector3ToV3(wallinfo.endPoint),brushedFloor.transform,false);
        //bw.EndPoint.SetParent(brushedFloor.transform);
        wall.transform.localScale = new Vector3(wallinfo.width, wallinfo.height, Vector3.Distance(NumericalConversion.MySerializableVector3ToV3(wallinfo.startPoint), NumericalConversion.MySerializableVector3ToV3(wallinfo.endPoint)));
        
        wall.transform.position = (bw.EndPoint.position + bw.StartPoint.position) / 2 + new Vector3(0, NumericalConversion.GetTargetPositionYByHeight(wall.transform,wallinfo.heightAboveGround),0);
        wall.transform.LookAt(bw.EndPoint.position + new Vector3(0, NumericalConversion.GetTargetPositionYByHeight(wall.transform, wallinfo.heightAboveGround), 0));
        wall.transform.position = wall.transform.position - brushedFloor.transform.parent.position;
        //bw.StartPoint.position = bw.StartPoint.position - brushedFloor.transform.parent.position;
        //bw.EndPoint.position = bw.EndPoint.position - brushedFloor.transform.parent.position;
        //Debug.LogError(NumericalConversion.MySerializableVector3ToV3(wallinfo.startPoint) + "     "+ NumericalConversion.MySerializableVector3ToV3(wallinfo.endPoint));

        wall.transform.SetParent(brushedFloor.transform);
        wall.layer = 13;
        if (wallinfo.doors != null && wallinfo.doors.Count > 0)
        {
            for (int i = 0; i < wallinfo.doors.Count; i++)
            {
                Transform door = BooleanRtManager.Instance.CreateDoorByInofrmation(wallinfo.doors[i], bw);
                //door.localRotation = wall.transform.localRotation;
                bw.doors.Add(door.GetComponent<BrushedDoor>());
            }
            //将墙壁扣一下
            BooleanRtManager.Instance.ResetDoorsAndWindowsOfWall(bw);
        }
        if (wallinfo.windows != null && wallinfo.windows.Count > 0)
        {
            for (int i = 0; i < wallinfo.windows.Count; i++)
            {
                Transform window = BooleanRtManager.Instance.CreateWindowByInformation(wallinfo.windows[i], bw);
                //window.localRotation = wall.transform.localRotation;
                bw.windows.Add(window.GetComponent<BrushedWindow>());
            }
        }
        brushedFloor.InteriorWalls.Add(bw);
    }
    /// <summary>
    /// 根据保存的信息自动创建建筑和墙壁等
    /// </summary>
    public void AutoCreatBrushedObjsByInformation()
    {
        Manager.Instace.allDataInformation.brushedObjInformation = Manager.Instace.allDataInformation.brushedObjInformation;
        if (Manager.Instace.allDataInformation.brushedObjInformation.buildings!= null&& Manager.Instace.allDataInformation.brushedObjInformation.buildings.Count >0)
        {
            StartCoroutine(CreateBuilidngByInformation());
        }
        if (Manager.Instace.allDataInformation.brushedObjInformation.walls!=null&& Manager.Instace.allDataInformation.brushedObjInformation.walls.Count>0)
        {
            StartCoroutine(DoAutoCreatWalls());
        }
        if (Manager.Instace.allDataInformation.brushedObjInformation.roads!=null&&Manager.Instace.allDataInformation.brushedObjInformation.roads.Count>0)
        {
            StartCoroutine(DoAutoCreatRoads());
        }

    }

    public IEnumerator CreateBuilidngByInformation()
    {
        if (Manager.Instace.allDataInformation.brushedObjInformation.buildings.Count>0)
        {
            for (int i = 0; i < Manager.Instace.allDataInformation.brushedObjInformation.buildings.Count; i++)
            {
                AutoCreatBuildingByInformation(Manager.Instace.allDataInformation.brushedObjInformation.buildings[i]);
                yield return null;
            }
        }
    }
    /// <summary>
    /// 根据记录的信息自动创建建筑
    /// </summary>
    public void AutoCreatBuildingByInformation(Building buildingInfo)
    {
        building = Instantiate(Resources.Load<GameObject>("Prefabs/Draw/Building"));
        BrushedBuilding brushedBuilding = building.GetComponent<BrushedBuilding>();
        BrushedBuildingList.Add(brushedBuilding);
        building.transform.SetParent(BrushParent);
        building.transform.localPosition = NumericalConversion.MySerializableVector3ToV3( buildingInfo.position);
        building.transform.localEulerAngles = NumericalConversion.MySerializableVector3ToV3(buildingInfo.rotation);
        building.transform.Find("Foundation").gameObject.SetActive(true);
        building.transform.Find("Cube").gameObject.SetActive(false);
        //if (building.GetComponentInChildren<LineRendererTest>().DirectionTexts != null && building.GetComponentInChildren<LineRendererTest>().DirectionTexts.Length > 0)
        //{
        //    foreach (var item in building.GetComponentInChildren<LineRendererTest>().DirectionTexts)
        //    {
        //        item.SetActive(false);
        //    }
        //}
        building.GetComponentInChildren<LineRendererTest>().gameObject.SetActive(false);
        building.GetComponent<BrushedBuilding>().Foundation = NumericalConversion.MySerializableVector3ToV3(buildingInfo.points);
        brushedBuilding.Floor.Clear();
        for (int i = 0; i < buildingInfo.floors.Count; i++)
        {
            AutoCreateWallsOfFloorByInformation(buildingInfo.floors[i], brushedBuilding);
        }
        brushedBuilding.InitAllFloor();
        //AutoCreatGroundOfBuilding(buildingInfo.points, building.GetComponent<BrushedBuilding>().Floor[0]);
        //AutoCreatCeilingOfBuilding(buildingInfo.points, building.GetComponent<BrushedBuilding>().Floor[0]);
        //building.transform.Find("Foundation").gameObject.SetActive(false);
    }

    /// <summary>
    /// 根据数据自动创建一层地板上的所有墙壁和地板天花板
    /// </summary>
    public void AutoCreateWallsOfFloorByInformation(Floor floorInfo,BrushedBuilding brushedBuilding)
    {

        GameObject floor = Instantiate(brushedBuilding.transform.Find("Floor1").gameObject);
        floor.transform.SetParent(brushedBuilding.transform);
        BrushedFloor brushedFloor = floor.GetComponent<BrushedFloor>();
        for (int i = 0; i < floorInfo.walls.Count; i++)
        {
            CreateFloorWallByInformation(floorInfo.walls[i], brushedFloor);
        }
        AutoCreatCeilingOfBuilding(brushedBuilding.Foundation, brushedFloor,floorInfo);
        AutoCreatGroundOfBuilding(brushedBuilding.Foundation, brushedFloor,floorInfo);
        brushedBuilding.Floor.Add(brushedFloor);
    }





    /// <summary>
    /// 保存所有创建出来的物体的信息
    /// </summary>
    public void SaveAllBrushObjInformation()
    {
        Manager.Instace.allDataInformation.brushedObjInformation.buildings.Clear();
        Manager.Instace.allDataInformation.brushedObjInformation.walls.Clear();
        SaveBuildingInformation();
        SaveWallsInformation();
        SaveRoadsInformation();

        //if (BrushedWallList != null && BrushedWallList.Count > 0)
        //{
        //    BrushedObjInformation.walls.Clear();
        //    Debug.Log("BrushedWallList.Count" + BrushedWallList.Count);
        //    for (int i = 0; i < BrushedWallList.Count; i++)
        //    {
        //        Wall wall = new Wall();
        //        wall.style = "";
        //        wall.width = BrushedWallList[i].Width;
        //        wall.height = BrushedWallList[i].Height;
        //        wall.heightAboveGround = BrushedWallList[i].HeightAboveGround;
        //        wall.startPoint = BrushedWallList[i].StartPoint.localPosition;
        //        wall.endPoint = BrushedWallList[i].EndPoint.localPosition;
        //        if (BrushedWallList[i].doors != null && BrushedWallList[i].doors.Count > 0)
        //        {
        //            for (int j = 0; j < BrushedWallList[i].doors.Count; j++)
        //            {
        //                Door door = new Door();
        //                door.style = "";
        //                door.height = BrushedWallList[i].doors[j].Height;
        //                door.width = BrushedWallList[i].doors[j].Width;
        //                door.length = BrushedWallList[i].doors[j].Length;
        //                door.distanceScale = BrushedWallList[i].doors[j].DistanceScale;
        //                wall.doors.Add(door);
        //            }
        //        }
        //        BrushedObjInformation.walls.Add(wall);
        //    }
        //    for (int i = 0; i < BrushedBuildingList.Count; i++)
        //    {
        //        Building building = new Building();
        //        building.position = BrushedBuildingList[i].transform.localPosition;
        //        for (int j = 0; j < BrushedBuildingList[i].Floor.Count; j++)
        //        {
        //            Floor floor = new Floor();
        //            for (int k = 0; k < BrushedBuildingList[i].Floor[j].walls.Count; k++)
        //            {
        //                Wall wall = new Wall();
        //                wall.style = "";
        //                wall.width = BrushedBuildingList[i].Floor[j].walls[k].Width;
        //                wall.height = BrushedBuildingList[i].Floor[j].walls[k].Height;
        //                wall.heightAboveGround = BrushedBuildingList[i].Floor[j].walls[k].HeightAboveGround;
        //                wall.startPoint = BrushedBuildingList[i].Floor[j].walls[k].StartPoint.position;
        //                wall.endPoint = BrushedBuildingList[i].Floor[j].walls[k].EndPoint.position;
        //            }
        //        }
        //    }
        //}
    }
    /// <summary>
    /// 保存创建出的不属于建筑的所有墙壁的信息
    /// </summary>
    public void SaveWallsInformation()
    {
        if (BrushedWallList.Count>0)
        {
            for (int i = 0; i < BrushedWallList.Count; i++)
            {
                Wall wall = new Wall();
                
                wall.style = WallTexture.GetMaterialName(BrushedWallList[i].gameObject);
                wall.width = BrushedWallList[i].Width;
                wall.height = BrushedWallList[i].Height;
                wall.heightAboveGround = BrushedWallList[i].HeightAboveGround;
                wall.startPoint = NumericalConversion.V3ToMySerializableVector3( BrushedWallList[i].StartPoint.position);
                wall.endPoint = NumericalConversion.V3ToMySerializableVector3( BrushedWallList[i].EndPoint.position);
                SaveDoorOrWindowInformationOfWall(wall, BrushedWallList[i]);
                Manager.Instace.allDataInformation.brushedObjInformation.walls.Add(wall);
            }
        }
    }
    /// <summary>
    /// 记录一睹墙壁上所有物体的信息
    /// </summary>
    /// <param name="wall"></param>
    /// <param name="brushedWall"></param>
    public void SaveDoorOrWindowInformationOfWall(Wall wall,BrushedWall brushedWall)
    {
        if (brushedWall.doors.Count>0)
        {
            for (int i = 0; i < brushedWall.doors.Count; i++)
            {
                Door door = new Door();
                door.style = WallTexture.GetMaterialName(brushedWall.doors[i].gameObject);
                door.width = brushedWall.doors[i].Width;
                door.height = brushedWall.doors[i].Height;
                door.length = brushedWall.doors[i].Length;
                door.heightAboveGround = brushedWall.doors[i].HeightAboveGround;
                door.distanceScale = brushedWall.doors[i].DistanceScale;
                wall.doors.Add(door);
            }
        }
        if (brushedWall.windows.Count > 0)
        {
            for (int i = 0; i < brushedWall.windows.Count; i++)
            {
                Window window = new Window();
                window.style = WallTexture.GetMaterialName(brushedWall.windows[i].gameObject);
                window.width = brushedWall.windows[i].Width;
                window.height = brushedWall.windows[i].Height;
                window.length = brushedWall.windows[i].Length;
                window.heightAboveGround = brushedWall.windows[i].HeightAboveGround;
                window.distanceScale = brushedWall.windows[i].DistanceScale;
                wall.windows.Add(window);
            }
        }
    }
    /// <summary>
    /// 记录某一层楼层上的所有物体的信息
    /// </summary>
    /// <param name="floor"></param>
    /// <param name="brushedFloor"></param>
    public void SaveWallInformationOfFloor(Floor floor,BrushedFloor brushedFloor)
    {
        if (brushedFloor.InteriorWalls.Count>0)
        {
            for (int i = 0; i < brushedFloor.InteriorWalls.Count; i++)
            {
                Wall wall = new Wall();
                wall.style = WallTexture.GetMaterialName( brushedFloor.InteriorWalls[i].gameObject);
                wall.width = brushedFloor.InteriorWalls[i].Width;
                wall.height = brushedFloor.InteriorWalls[i].Height;
                wall.heightAboveGround = brushedFloor.InteriorWalls[i].HeightAboveGround;
                wall.startPoint = NumericalConversion.V3ToMySerializableVector3(brushedFloor.InteriorWalls[i].StartPoint.position);
                wall.endPoint = NumericalConversion.V3ToMySerializableVector3(brushedFloor.InteriorWalls[i].EndPoint.position);
                //Debug.LogError(brushedFloor.walls[i].StartPoint.position + "     " + brushedFloor.walls[i].EndPoint.position);
                //Debug.LogError(NumericalConversion.MySerializableVector3ToV3( wall.startPoint) + "    " + NumericalConversion.MySerializableVector3ToV3(wall.endPoint));
                SaveDoorOrWindowInformationOfWall(wall, brushedFloor.InteriorWalls[i]);
                floor.walls.Add(wall);
            }
        }
        if (brushedFloor.ground!=null)
        {
            Ground ground = new Ground();
            ground.style = WallTexture.GetMaterialName(brushedFloor.ground.gameObject);
            ground.heightAboveGround = brushedFloor.ground.HeightAboveGround;
            //todo
            //ground.points = brushedFloor.ground.
        }
        if (brushedFloor.ceiling!=null)
        {
            Ground ceiling = new Ground();
            ceiling.style = WallTexture.GetMaterialName(brushedFloor.ceiling.gameObject);
            ceiling.heightAboveGround = brushedFloor.ceiling.HeightAboveGround;
            //todo
            //ceiling.points = brushedFloor.ground.
        }
    }
    /// <summary>
    /// 记录一个建筑中所有楼层的信息
    /// </summary>
    public void SaveFloorInofrmationOfBuilding(Building building , BrushedBuilding brushedBuilding)
    {
        if (brushedBuilding.Floor.Count>0)
        {
            for (int i = 0; i < brushedBuilding.Floor.Count; i++)
            {
                Floor floor = new Floor();
                SaveWallInformationOfFloor(floor, brushedBuilding.Floor[i]);
                if (brushedBuilding.Floor[i].ground!=null)
                {
                    Ground ground = new Ground();
                    ground.style = WallTexture.GetMaterialName(brushedBuilding.Floor[i].ground.gameObject);
                    floor.ground = ground;
                }
                if (brushedBuilding.Floor[i].ceiling != null)
                {
                    Ground ceiling = new Ground();
                    ceiling.style = WallTexture.GetMaterialName(brushedBuilding.Floor[i].ceiling.gameObject);
                    floor.ceiling = ceiling;
                }
                building.floors.Add(floor);
            }
        }
    }

    public void SaveBuildingInformation()
    {
        if (BrushedBuildingList.Count > 0)
        {
            for (int i = 0; i < BrushedBuildingList.Count; i++)
            {
                Building building = new Building();
                building.points = NumericalConversion.V3ToMySerializableVector3(BrushedBuildingList[i].Foundation);
                building.position = NumericalConversion.V3ToMySerializableVector3(BrushedBuildingList[i].transform.position);
                building.rotation = NumericalConversion.V3ToMySerializableVector3(BrushedBuildingList[i].transform.eulerAngles);
                
                SaveFloorInofrmationOfBuilding(building, BrushedBuildingList[i]);
                Manager.Instace.allDataInformation.brushedObjInformation.buildings.Add(building);
            }
        }
    }


    #region 建筑
    /// <summary>
    /// 绘制内部墙壁的linerenderer的载体
    /// </summary>
    public GameObject InteriorWallsPrefab;
    /// <summary>
    /// 绘制内部墙壁时记录每个点位置的数组
    /// </summary>
    Vector3[] pointArray;
    /// <summary>
    /// 是否可以开始创建内部墙壁
    /// </summary>
    public bool CanCreateInteriorWalls { get; set; }
    /// <summary>
    /// 是不是正在创建建筑的内部墙壁，用于防止开启多个创建内部墙壁的携程
    /// </summary>
    private bool creatingInteriorwall;
    /// <summary>
    /// 在场景中创建一个新建筑
    /// </summary>
    public void CreateNewBuilding()
    {
        building = Instantiate(Resources.Load<GameObject>("Prefabs/Draw/Building"));
        BrushedBuildingList.Add(building.GetComponent<BrushedBuilding>());
        building.transform.SetParent(BrushParent);
        StartCoroutine(DoSetBuildingPosition());
    }
    public void SetBuildingPosition()
    {
        StartCoroutine(DoSetBuildingPosition());
    }
    /// <summary>
    /// 拖动鼠标调整建筑的大概位置
    /// </summary>
    /// <returns></returns>
    IEnumerator DoSetBuildingPosition()
    {
        yield return new WaitForSeconds(0.2f);
        G_CreateObject g_CreateObject = canvas.GetComponent<G_CreateObject>();
        while (true)
        {
            building.transform.localPosition = g_CreateObject.GetWorldPos();
            if (Input.GetMouseButtonDown(0))
            {
                break;
            }
            yield return null;
        }
    }
    /// <summary>
    /// 自由绘制建筑
    /// </summary>
    public void CreateBuilidingByFreeDraw()
    {

    }
    /// <summary>
    /// 设置结构图
    /// </summary>
    public void SetStructureChart(Sprite sprite)
    {
        if (building!=null)
        {
            building.transform.Find("StructureChart").GetComponent<SpriteRenderer>().sprite = sprite;
        }
    }
    /// <summary>
    /// 开始创建建筑的外围墙壁的地基
    /// </summary>
    public void CreatePeripheralWalls()
    {
        
        if (building != null)
        {
            //将蓝色立方体隐藏
            building.transform.Find("Cube").gameObject.SetActive(false);
            building.transform.Find("Foundation").gameObject.SetActive(true);
            //将摄像头拉到建筑正上方
            Camera.main.transform.position = new Vector3(building.transform.position.x, 20, building.transform.position.z);
            Camera.main.transform.eulerAngles = new Vector3(90f, 0f, 0f);
            building.GetComponentInChildren<LineRendererTest>().StartDrawCoroutine();
        }
    }
    /// <summary>
    /// 创建建筑内部墙壁的地基
    /// </summary>
    public void CreateInteriorWalls()
    {

    }
    /// <summary>
    /// 绘制内部墙壁
    /// </summary>
    /// <returns></returns>
    IEnumerator DoDrawInteriorWalls()
    {
        creatingInteriorwall = true;
        //鼠标左击次数，用于记录该条线创建了多个节点
        int mouseCount = 0;
        pointArray = new Vector3[2];
        GameObject lineObj = Instantiate(InteriorWallsPrefab);
        lineObj.transform.SetParent(building.transform.Find("Foundation"));
        LineRenderer line = lineObj.GetComponent<LineRenderer>();
        Vector3 point;
        while (true)
        {
            if (BrushRayCastCheck.MouseCheckGroundOnly().collider != null)
            {
                point  = BrushRayCastCheck.MouseCheckGroundOnly().point;
                //创建一个新的linerenderer并设定起点，另一个点跟随鼠标
                if (Input.GetMouseButtonUp(0))
                {
                    if (mouseCount == 0)
                    {
                        pointArray[0] = new Vector3(point.x, 0.01f, point.z);
                    }
                    else
                    {
                        Array.Resize(ref pointArray, pointArray.Length + 1);
                        pointArray[pointArray.Length - 2] = new Vector3(point.x,0.01f,point.z);
                    }
                    mouseCount++;
                }
                if (mouseCount != 0)
                {
                    pointArray[pointArray.Length - 1] = new Vector3(point.x,0.01f,point.z);
                    Repaint(line, pointArray);
                }
            }
            yield return null;
            if (Input.GetMouseButtonDown(1))
            {
                Array.Resize(ref pointArray, pointArray.Length - 1);
                lineObj.GetComponent<InteriorWallsPoints>().SetWallPoints(pointArray);
                for (int i = 0; i < pointArray.Length; i++)
                {

                }
                Repaint(line, pointArray);
                break;
            }
        }
        creatingInteriorwall = false;
    }
    public void Repaint(LineRenderer lineRenderer, Vector3[] vector3s)
    {
        lineRenderer.numPositions = vector3s.Length;
        lineRenderer.SetPositions(vector3s);
    }





    #endregion







    /// <summary>
    /// 创建建筑的预设
    /// </summary>
    public void CreatBuildingPrefab()
    {
        building = Instantiate(Resources.Load<GameObject>("Prefabs/Draw/Building"));
        BrushedBuildingList.Add(building.GetComponent<BrushedBuilding>());
        building.transform.SetParent(BrushParent);
        StartCoroutine(DoSetBuildingPosition());
    }
    

    /// <summary>
    /// 修改建筑地基
    /// </summary>
    public void CreatBudindingFoundation()
    {
        //building = Instantiate(Resources.Load<GameObject>("Prefabs/Draw/Building"));
        //BrushedBuildingList.Add(building.GetComponent<BrushedBuilding>());
        //building.transform.SetParent(BrushParent);
        //building.transform.localPosition = Vector3.zero;
        building.transform.Find("Foundation").gameObject.SetActive(true);
        building.transform.Find("Cube").gameObject.SetActive(false);
        //building.GetComponentInChildren<LineRendererTest>().Canvas = Manager.Instace.GetComponent<Canvas>();
        //将摄像头拉到高处并使其只能看到地基相关的物体
        Camera.main.transform.position = new Vector3(building.transform.position.x, 20, building.transform.position.z);
        Camera.main.transform.eulerAngles = new Vector3(90f, 0f, 0f);
        Camera.main.cullingMask = 1 << 15;
    }
    /// <summary>
    /// 建筑地基创建完成，创建第一层的地板和墙壁
    /// </summary>
    public void CompleteBuildingFoundation()
    {
        //关闭创建内部墙壁的可行性
        CanCreateInteriorWalls = false;
        

        
        if (building != null)
        {

            //获取地基的点的数组创建第一层的最外层墙壁
            LineRendererTest lineRendererTest = building.GetComponentInChildren<LineRendererTest>();
            //将墙壁的点位的世界坐标转为楼层下的局部坐标
            Vector3[] worldPoints = lineRendererTest.PointList.ToArray();
            Vector3[] floorLoacalPoints = new Vector3[worldPoints.Length];
            for (int i = 0; i < worldPoints.Length; i++)
            {
                floorLoacalPoints[i] = worldPoints[i];
            }
            
            building.GetComponent<BrushedBuilding>().Foundation = floorLoacalPoints;
            AutoCreatFirstFloorWallsOfBuilding(floorLoacalPoints, building.GetComponent<BrushedBuilding>().Floor[0]);
            
            AutoCreatGroundOfBuilding(floorLoacalPoints, building.GetComponent<BrushedBuilding>().Floor[0]);
            AutoCreatCeilingOfBuilding(floorLoacalPoints, building.GetComponent<BrushedBuilding>().Floor[0]);

            if (building.transform.Find("Foundation").childCount != 0)
            {
                List<Vector3[]> vector3s = new List<Vector3[]>();
                for (int i = 0; i < building.transform.Find("Foundation").childCount; i++)
                {
                    vector3s.Add(building.transform.Find("Foundation").GetChild(i).GetComponent<InteriorWallsPoints>().GetWallPoints());
                }
                AutoCreateFirstFloorInteriorWallsOfBuilding(building.GetComponent<BrushedBuilding>().Floor[0], vector3s);
            }
            //将地上画的线隐藏
            building.transform.Find("Foundation").gameObject.SetActive(false);
            //将结构图隐藏
            building.transform.Find("StructureChart").gameObject.SetActive(false);
        }
        //将相机位置和显示层级复原
        //Camera.main.transform.localPosition = new Vector3(0, 5, -5);
        //if (building !=null)
        //{
        //    Camera.main.transform.localPosition = new Vector3(building.transform.position.x, building.transform.position.y + 5, building.transform.position.z - 5);
        //}
        //Camera.main.transform.localEulerAngles = new Vector3(45, 0, 0);
        //Camera.main.cullingMask = EverythingLayer;

    }
    /// <summary>
    /// 自动创建建筑第一层默认的墙壁和地板
    /// </summary>
    public void AutoCreatFirstFloorWallsOfBuilding(Vector3[] vectors, BrushedFloor floor)
    {
        //Debug.LogError(vectors[0]);
        for (int i = 0; i < vectors.Length; i++)
        {
            vectors[i] = vectors[i] - floor.transform.position;
        }
        for (int i = 0; i < vectors.Length - 1; i++)
        {
            GameObject FloorWall = Instantiate(normalWallPrefab);
            BrushedWall brushedWall = FloorWall.GetComponent<BrushedWall>();
            brushedWall.StartPoint = NewBuildingPoint(vectors[i],floor.transform,true);
            //brushedWall.StartPoint.SetParent(floor.transform);
            if (i == vectors.Length - 2)
            {
                brushedWall.EndPoint = floor.InteriorWalls[0].StartPoint;
            }
            else
            {
                brushedWall.EndPoint = NewBuildingPoint(vectors[i + 1],floor.transform,true);
                //brushedWall.EndPoint.SetParent(floor.transform);
            }
            FloorWall.transform.SetParent(floor.transform);
            FloorWall.transform.localPosition = (vectors[i] + vectors[i + 1]) / 2 + new Vector3(0, FloorWall.transform.localScale.y / 2,0);
            FloorWall.transform.localScale = new Vector3(FloorWall.transform.localScale.x, FloorWall.transform.localScale.y, Vector3.Distance(vectors[i], vectors[i + 1]));
            FloorWall.transform.LookAt(brushedWall.EndPoint.position + new Vector3(0, FloorWall.transform.localScale.y / 2, 0));
            FloorWall.layer = 13;
            floor.InteriorWalls.Add(brushedWall);
        }
    }
    /// <summary>
    /// 自动创建建筑第一层的内部墙壁
    /// </summary>
    public void AutoCreateFirstFloorInteriorWallsOfBuilding(BrushedFloor floor,List<Vector3[]> vectors)
    {
        for (int i = 0; i < vectors.Count; i++)
        {
            for (int j = 0; j < vectors[i].Length; j++)
            {
                vectors[i][j] = vectors[i][j] - floor.transform.position;
            }
        }
        for (int i = 0; i < vectors.Count; i++)
        {
            for (int j = 0; j < vectors[i].Length-1; j++)
            {
                GameObject FloorWall = Instantiate(normalWallPrefab);
                BrushedWall brushedWall = FloorWall.GetComponent<BrushedWall>();
                brushedWall.StartPoint = NewBuildingPoint(vectors[i][j], floor.transform, true);
                //brushedWall.StartPoint.SetParent(floor.transform);
                  brushedWall.EndPoint = NewBuildingPoint(vectors[i][j + 1], floor.transform, true);
                    //brushedWall.EndPoint.SetParent(floor.transform);
                
                FloorWall.transform.SetParent(floor.transform);
                FloorWall.transform.localPosition = (vectors[i][j] + vectors[i][j + 1]) / 2 + new Vector3(0, FloorWall.transform.localScale.y / 2, 0);
                FloorWall.transform.localScale = new Vector3(FloorWall.transform.localScale.x, FloorWall.transform.localScale.y, Vector3.Distance(vectors[i][j], vectors[i][j + 1]));
                FloorWall.transform.LookAt(brushedWall.EndPoint.position + new Vector3(0, FloorWall.transform.localScale.y / 2, 0));
                FloorWall.layer = 13;
                floor.InteriorWalls.Add(brushedWall);
            }
        }
    }

    public IEnumerator DoDrawBuildingFloorWall(bool drawing, BrushedFloor floor)
    {
        GameObject wall;
        BrushedWall bw;
        //bool mouseCheckWall = false;
        //创建默认的墙壁模型
        wall = Instantiate(normalWallPrefab);
        wall.transform.SetParent(floor.transform);
        wall.transform.localScale = new Vector3(wall.transform.localScale.x, wall.transform.localScale.y, 0f);
        wall.gameObject.layer = 16;
        bw = wall.GetComponent<BrushedWall> ();
        if (continuity && drawing)
        {
            bw.StartPoint = NewBuildingPoint(startPoint,floor.transform,false);
            //bw.StartPoint.SetParent(floor.transform);
        }
        else
        {
            while (!drawing)
            {
                if (Input.GetMouseButtonDown(0))
                {

                        raycastHit = BrushRayCastCheck.MouseCheckBuildingGround(BrushLayer);
                        //鼠标检测到的是绘制的墙壁
                        if (raycastHit.collider != null && raycastHit.collider.gameObject.tag == "BrushWall")
                        {
                            startPoint = raycastHit.point - raycastHit.normal * raycastHit.collider.transform.localScale.x / 2;
                            startPoint = new Vector3(startPoint.x, raycastHit.collider.transform.parent.position.y, startPoint.z);
                            Debug.LogError(startPoint);
                        }//鼠标检测到的是地面
                        else if (raycastHit.collider != null && raycastHit.collider.gameObject.tag == "BrushGround")
                        {
                            startPoint = raycastHit.point;
                        }
                        else
                        {
                            startPoint = BrushRayCastCheck.NullVector3;
                        }
                    
                    if (startPoint != BrushRayCastCheck.NullVector3)
                    {
                        drawing = true;
                        bw.StartPoint = NewPoint(startPoint);
                        bw.StartPoint.SetParent(floor.transform);
                    }
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    Destroy(wall);
                    continuity = false;
                    break;
                }
                yield return null;
            }
        }
        while (drawing)
        {
            
                raycastHit = BrushRayCastCheck.MouseCheckBuildingGround(BrushLayer);
                //鼠标检测到的是绘制的墙壁
                if (raycastHit.collider != null && raycastHit.collider.gameObject.tag == "BrushWall")
                {
                    endPoint = raycastHit.point - raycastHit.normal * raycastHit.collider.transform.localScale.x / 2;
                    endPoint = new Vector3(endPoint.x, raycastHit.collider.transform.parent.position.y, endPoint.z);
                //Debug.LogError(endPoint);

                }//鼠标检测到的是地板
                else if (raycastHit.collider != null && raycastHit.collider.gameObject.tag == "BrushGround")
                {
                    endPoint = raycastHit.point;
                    
                }
                else
                {
                    //endPoint = BrushRayCastCheck.NullVector3;
                }
            
            if (endPoint != BrushRayCastCheck.NullVector3)
            {
                //Debug.LogError(endPoint);
                wall.transform.position = new Vector3((startPoint.x + endPoint.x) / 2, floor.transform.position.y+wall.transform.localScale.y/2, (startPoint.z + endPoint.z) / 2);
                //wall.transform.localPosition = new Vector3()
                wall.transform.localScale = new Vector3(wall.transform.localScale.x, wall.transform.localScale.y, Vector3.Distance(startPoint, endPoint));
                wall.transform.LookAt(endPoint + new Vector3(0, (wall.transform.localScale.y / 2), 0));
                if (bw.EndPoint == null)
                {
                    bw.EndPoint = NewPoint(endPoint, true);
                    bw.EndPoint.SetParent(floor.transform);
                }
                else
                {
                    bw.EndPoint.position = endPoint;
                }

                if (Input.GetMouseButtonDown(0))
                {

                    drawing = false;
                    startPoint = endPoint;
                    continuity = true;
                    floor.InteriorWalls.Add(bw);
                    if (floor.InteriorWalls.Count >= 2)
                    {
                        floor.InteriorWalls[floor.InteriorWalls.Count - 2].gameObject.layer = 13;
                    }
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    Destroy(bw.TextObj);
                    Destroy(bw.EndPoint.gameObject);
                    Destroy(wall);
                    drawing = false;
                    startPoint = BrushRayCastCheck.NullVector3;
                    endPoint = BrushRayCastCheck.NullVector3;
                    continuity = false;
                    if (floor.InteriorWalls.Count >= 1)
                    {
                        floor.InteriorWalls[floor.InteriorWalls.Count - 1].gameObject.layer = 13;
                    }
                }
            }
            yield return null;
        }
        if (continuity)
        {
            StartDrawBuildingWall(true);
        }
        else
        {
            BrushMode = false;
        }
        
    }

    public void OpenSettingPanel(GameObject panel)
    {
        BrushWallSettings.gameObject.SetActive(false);
        BrushDoorSettings.gameObject.SetActive(false);
        BrushGroundSettings.gameObject.SetActive(false);
        BrushWindowSettings.gameObject.SetActive(false);
        BrushBuildingInformationSettings.gameObject.SetActive(false);
        if (panel != null && panel != BrushBuildingInformationSettings)
        {
            Manager.Instace.dragUIMoveObjOnGround.gameObject.SetActive(false);
        }
        if (panel == null)
        {
            Manager.Instace.dragUIMoveObjOnGround.gameObject.SetActive(false);
            return;
        }
        panel.SetActive(true);
    }
    public void AutoCreatGroundOfBuilding(Vector3[] vectors, BrushedFloor floor,Floor floor1 = null)
    {
        if (floor1 != null)
        {
            if (floor1.ground != null)
            {
                List<Vector3> vectorList = new List<Vector3>();
                for (int i = 0; i < vectors.Length - 1; i++)
                {
                    vectorList.Add(vectors[i]);
                }
                GameObject ground = Instantiate(Resources.Load<GameObject>("Prefabs/Draw/Ground"));
                ground.transform.SetParent(floor.transform);
                ground.transform.localPosition = new Vector3(0, 0.001f, 0);
                ground.transform.localEulerAngles = Vector3.zero;
                ground.transform.localScale = Vector3.one;
                ground.GetComponent<BrushedGround>().Init(vectorList);
                ground.GetComponent<MeshRenderer>().material = WallTexture.LoadWallTexture(floor1.ground.style).material;
                floor.ground = ground.GetComponent<BrushedGround>();
            }
        }
        else
        {
            List<Vector3> vectorList = new List<Vector3>();
            for (int i = 0; i < vectors.Length - 1; i++)
            {
                vectorList.Add(new Vector3(vectors[i].x, 0, vectors[i].z));
            }
            GameObject ground = Instantiate(Resources.Load<GameObject>("Prefabs/Draw/Ground"));
            ground.transform.SetParent(floor.transform);
            ground.transform.localPosition = new Vector3(0, 0.001f, 0);
            ground.transform.localEulerAngles = Vector3.zero;
            ground.transform.localScale = Vector3.one;
            ground.GetComponent<BrushedGround>().Init(vectorList);

            floor.ground = ground.GetComponent<BrushedGround>();
        }
        
    }

    public void AutoCreatCeilingOfBuilding(Vector3[] vectors,BrushedFloor floor,Floor floor1 = null)
    {
        if (floor1 != null)
        {
            if (floor1.ceiling != null)
            {
                List<Vector3> vectorList = new List<Vector3>();
                for (int i = 0; i < vectors.Length - 1; i++)
                {
                    vectorList.Add(vectors[i]);
                }
                GameObject ceiling = Instantiate(Resources.Load<GameObject>("Prefabs/Draw/Ground"));
                ceiling.transform.SetParent(floor.transform);
                ceiling.transform.localPosition = new Vector3(0, 2.999f, 0);
                ceiling.transform.localEulerAngles = Vector3.zero;
                ceiling.transform.localScale = Vector3.one;
                ceiling.GetComponent<BrushedGround>().Init(vectorList);
                ceiling.GetComponent<BrushedGround>().Ceiling = true;
                //Debug.LogError(floor1.ceiling.style);
                ceiling.GetComponent<MeshRenderer>().material = WallTexture.LoadWallTexture(floor1.ceiling.style).material;
                //ceiling.GetComponent<BrushedGround>().
                floor.ceiling = ceiling.GetComponent<BrushedGround>();
            }
        }
        else
        {
            List<Vector3> vectorList = new List<Vector3>();
            for (int i = 0; i < vectors.Length - 1; i++)
            {
                vectorList.Add(vectors[i]);
            }
            GameObject ceiling = Instantiate(Resources.Load<GameObject>("Prefabs/Draw/Ground"));
            ceiling.transform.SetParent(floor.transform);
            ceiling.transform.localPosition = new Vector3(0, 2.999f, 0);
            ceiling.transform.localEulerAngles = Vector3.zero;
            ceiling.transform.localScale = Vector3.one;
            ceiling.GetComponent<BrushedGround>().Init(vectorList);
            ceiling.GetComponent<BrushedGround>().Ceiling = true;
            //ceiling.GetComponent<BrushedGround>().
            floor.ceiling = ceiling.GetComponent<BrushedGround>();
        }

    }

    public void EditBuildingFloor(BrushedBuilding editBuilding,int floorNum)
    {
        EditFloor = editBuilding.Floor[floorNum - 1];
        BrushFloorInformationSettings.gameObject.SetActive(true);
        BrushFloorInformationSettings.Init(EditFloor);
        foreach (var item in editBuilding.Floor)
        {
            item.gameObject.SetActive(false);
        }
        EditFloor.gameObject.SetActive(true);
        BrushedGround[] grounds = EditFloor.transform.GetComponentsInChildren<BrushedGround>();
        
        Camera.main.transform.position = new Vector3(0, 5, -5) + EditFloor.transform.position;
        EndEditFloorBtn.gameObject.SetActive(true);
        OpenSettingPanel(null);
        G_PubDef.QuiescentObject = 1 << 8 | 1 << 13;
    }

    public void EndEditBuildingFloor()
    {
        foreach (var item in EditFloor.GetComponentInParent<BrushedBuilding>().Floor)
        {
            item.gameObject.SetActive(true);
        }
        BrushedGround[] grounds = EditFloor.transform.GetComponentsInChildren<BrushedGround>();
        
        EditFloor = null;
        BrushFloorInformationSettings.gameObject.SetActive(false);

        EndEditFloorBtn.gameObject.SetActive(false);
        G_PubDef.QuiescentObject = 1 << 8;
    }








    #region Road



    public void StartDrawRoad(bool drawing)
    {
        BrushMode = true;
        StartCoroutine(DoDrawRoad(drawing));
    }

    IEnumerator DoDrawRoad(bool drawing)
    {
        GameObject road;
        BrushedRoad br;
        //创建默认的路面模型
        road = Instantiate(normalRoadPrefab);
        road.transform.SetParent(BrushParent);
        road.transform.localScale = new Vector3(road.transform.localScale.x, road.transform.localScale.y, 0f);
        br = road.GetComponent<BrushedRoad>();
        if (continuity && drawing)
        {
            br.StartPoint = NewPoint(startPoint);
        }
        else
        {
            while (!drawing)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (attachToGuides)
                    {
                        startPoint = BrushRayCastCheck.MouseCheckGroundOnly().point;
                        startPoint = new Vector3(Mathf.Round(startPoint.x), 0, Mathf.Round(startPoint.z));
                    }
                    else
                    {
                        raycastHit = BrushRayCastCheck.MouseCheckGroundOnly();
                        ////鼠标检测到的是绘制的墙壁
                        //if (raycastHit.collider != null && raycastHit.collider.gameObject.tag == "BrushWall")
                        //{
                        //    startPoint = raycastHit.point - raycastHit.normal * raycastHit.collider.transform.localScale.x / 2;
                        //    startPoint = new Vector3(startPoint.x, 0, startPoint.z);
                        //}//鼠标检测到的是地面
                        //else 
                        if (raycastHit.collider != null && raycastHit.collider.gameObject.layer == 8)
                        {
                            startPoint = raycastHit.point;
                        }
                        else
                        {
                            startPoint = BrushRayCastCheck.NullVector3;
                        }
                    }
                    if (startPoint != BrushRayCastCheck.NullVector3)
                    {
                        drawing = true;
                        br.StartPoint = NewPoint(startPoint);
                    }
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    Destroy(road);
                    continuity = false;
                    break;
                }
                yield return null;
            }
        }
        while (drawing)
        {
            //附着到网格
            if (attachToGuides)
            {
                endPoint = BrushRayCastCheck.MouseCheckGroundOnly().point;
                endPoint = new Vector3(Mathf.Round(endPoint.x), 0, Mathf.Round(endPoint.z));
            }//不附着网格
            else
            {
                raycastHit = BrushRayCastCheck.MouseCheckGroundOnly();
                //鼠标检测到的是绘制的墙壁
                //if (raycastHit.collider != null && raycastHit.collider.gameObject.tag == "BrushWall")
                //{
                //    endPoint = raycastHit.point - raycastHit.normal * raycastHit.collider.transform.localScale.x / 2;
                //    endPoint = new Vector3(endPoint.x, 0, endPoint.z);
                //    mouseCheckWall = true;
                //    if (lastBrushedWall == null)
                //    {
                //        lastBrushedWall = raycastHit.collider.GetComponent<BrushedWall>();
                //        if (bw.EndPoint == null)
                //        {
                //            lastBrushedWall.MiddlePoint = null;
                //        }
                //        else
                //        {
                //            lastBrushedWall.MiddlePoint = bw.EndPoint;
                //        }
                //    }
                //    else
                //    {
                //        if (lastBrushedWall != raycastHit.collider.GetComponent<BrushedWall>())
                //        {
                //            raycastHit.collider.GetComponent<BrushedWall>().MiddlePoint = lastBrushedWall.MiddlePoint;
                //            lastBrushedWall.MiddlePoint = null;
                //            lastBrushedWall = raycastHit.collider.GetComponent<BrushedWall>();
                //            if (bw.EndPoint == null)
                //            {
                //                lastBrushedWall.MiddlePoint = null;
                //            }
                //            else
                //            {
                //                lastBrushedWall.MiddlePoint = bw.EndPoint;
                //            }
                //        }
                //        else
                //        {
                //            if (bw.EndPoint == null)
                //            {
                //                lastBrushedWall.MiddlePoint = null;
                //            }
                //            else
                //            {
                //                lastBrushedWall.MiddlePoint = bw.EndPoint;
                //            }
                //        }
                //    }
                //}//鼠标检测到的是地面
                //else 
                if (raycastHit.collider != null && raycastHit.collider.gameObject.layer == 8)
                {
                    endPoint = raycastHit.point;
                    //mouseCheckWall = false;
                    if (lastBrushedWall != null)
                    {
                        lastBrushedWall.MiddlePoint = null;
                        lastBrushedWall = null;
                    }
                }
                else
                {
                    endPoint = BrushRayCastCheck.NullVector3;
                }
            }
            if (endPoint != BrushRayCastCheck.NullVector3)
            {
                road.transform.localPosition = new Vector3((startPoint.x + endPoint.x) / 2, road.transform.localScale.y / 2, (startPoint.z + endPoint.z) / 2);
                road.transform.localScale = new Vector3(road.transform.localScale.x, road.transform.localScale.y, Vector3.Distance(startPoint, endPoint));
                road.transform.LookAt(endPoint + new Vector3(0, (road.transform.localScale.y / 2), 0));
                if (br.EndPoint == null)
                {
                    br.EndPoint = NewPoint(endPoint, true);
                }
                else
                {
                    br.EndPoint.position = endPoint;
                }

                if (Input.GetMouseButtonDown(0))
                {

                    drawing = false;
                    startPoint = endPoint;
                    continuity = true;
                    BrushedRoadList.Add(br);
                    if (BrushedRoadList.Count >= 2)
                    {
                        //BrushedRoadList[BrushedRoadList.Count - 2].gameObject.layer = 13;
                    }
                    //if (mouseCheckWall)
                    //{
                    //    lastBrushedWall.Separate();
                    //}
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    //Destroy(bw.TextObj);
                    Destroy(br.EndPoint.gameObject);
                    Destroy(road);
                    drawing = false;
                    startPoint = BrushRayCastCheck.NullVector3;
                    endPoint = BrushRayCastCheck.NullVector3;
                    continuity = false;
                    if (BrushedRoadList.Count >= 1)
                    {
                        //BrushedRoadList[BrushedRoadList.Count - 1].gameObject.layer = 13;
                    }
                }
            }
            yield return null;
        }
        if (continuity)
        {
            StartDrawRoad(true);
        }
        else
        {
            BrushMode = false;
        }
    }



    /// <summary>
    /// 根据数据自动创建路面
    /// </summary>
    public IEnumerator DoAutoCreatRoads()
    {
        if (Manager.Instace.allDataInformation.brushedObjInformation.roads != null && Manager.Instace.allDataInformation.brushedObjInformation.roads.Count > 0)
        {
            for (int i = 0; i < Manager.Instace.allDataInformation.brushedObjInformation.roads.Count; i++)
            {
                CreateRoadsByInformation(Manager.Instace.allDataInformation.brushedObjInformation.roads[i]);
                yield return null;
            }
        }
    }

    public void CreateRoadsByInformation(Road roadInfo)
    {
        GameObject road = Instantiate(normalRoadPrefab);
        BrushedRoad br = road.GetComponent<BrushedRoad>();
        br.StartPoint = NewPoint(NumericalConversion.MySerializableVector3ToV3(roadInfo.startPoint));
        br.EndPoint = NewPoint(NumericalConversion.MySerializableVector3ToV3(roadInfo.endPoint));
        //road.GetComponent<MeshRenderer>().material = WallTexture.LoadWallTexture(roadInfo.style).material;
        road.transform.localScale = new Vector3(roadInfo.width, 0.001f, Vector3.Distance(NumericalConversion.MySerializableVector3ToV3(roadInfo.startPoint), NumericalConversion.MySerializableVector3ToV3(roadInfo.endPoint)));
        road.transform.SetParent(BrushParent);
        road.transform.localPosition = (br.StartPoint.localPosition + br.EndPoint.localPosition) / 2 + new Vector3(0, road.transform.localScale.y / 2, 0);
        road.transform.LookAt(br.EndPoint.localPosition + new Vector3(0, road.transform.localScale.y / 2, 0));
        road.layer = 13;
        
        BrushedRoadList.Add(br);
    }


    /// <summary>
    /// 保存创建出的的所有路面的信息
    /// </summary>
    public void SaveRoadsInformation()
    {
        if (BrushedRoadList.Count > 0)
        {
            for (int i = 0; i < BrushedRoadList.Count; i++)
            {
                Road road = new Road();

                road.style = WallTexture.GetMaterialName(BrushedRoadList[i].gameObject);
                road.width = BrushedRoadList[i].Width;
                road.heightAboveGround = BrushedRoadList[i].HeightAboveGround;
                road.startPoint = NumericalConversion.V3ToMySerializableVector3(BrushedRoadList[i].StartPoint.position);
                road.endPoint = NumericalConversion.V3ToMySerializableVector3(BrushedRoadList[i].EndPoint.position);
                Manager.Instace.allDataInformation.brushedObjInformation.roads.Add(road);
            }
        }
    }


    /// <summary>
    /// 获取和某一段道路的startpoint相连的所有道路
    /// </summary>
    /// <returns></returns>
    public BrushedRoad[] GetLastRoad(BrushedRoad road)
    {
        List<BrushedRoad> brushedRoads = new List<BrushedRoad>();
        for (int i = 0; i < BrushedRoadList.Count; i++)
        {
            if (BrushedRoadList[i] == road)
            {
                continue;
            }
            if (BrushedWallList[i].StartPoint == road.StartPoint || BrushedWallList[i].EndPoint == road.StartPoint)
            {
                brushedRoads.Add(BrushedRoadList[i]);
            }
        }
        return brushedRoads.ToArray();
    }

    /// <summary>
    /// 获取某一段道路连接的后一段道路
    /// </summary>
    /// <returns></returns>
    public BrushedRoad[] GetNextRoad(BrushedRoad road)
    {
        List<BrushedRoad> brushedRoads = new List<BrushedRoad>();
        for (int i = 0; i < BrushedRoadList.Count; i++)
        {
            if (BrushedRoadList[i] == road)
            {
                continue;
            }
            if (BrushedRoadList[i].StartPoint == road.EndPoint || BrushedRoadList[i].EndPoint == road.EndPoint)
            {
                brushedRoads.Add(BrushedRoadList[i]);
            }
        }
        return brushedRoads.ToArray();
    }

    #endregion


}


public enum ModifyWallMode
{
    //主动修改的startpoint
    ActiveStartPoint,
    //主动修改的endpoint
    ActiveEndPoint,
    //被动修改
    Passive
}
