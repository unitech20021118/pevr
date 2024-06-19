using Brush;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BooleanRtManager : MonoBehaviour
{
    public static BooleanRtManager Instance;

    private BooleanRT booleanRT;
    private RaycastHit raycastHit;

    private LayerMask layer = 1 << 13;

    /// <summary>
    /// 用于布尔减的门
    /// </summary>
    private Transform BooleanDoor;

    private Transform BooleanWindow;

    private Vector3 NormalDoorScale;
    private Vector3 NormalWindowScale;

    private Transform NormalDOorPrefab;
    private Transform NormalDoor;

    private Transform NormalWindowPrefab;
    private Transform NormalWindow;
    /// <summary>
    /// 门在墙壁中的位置
    /// </summary>
    private Vector3 doorPosition;
    /// <summary>
    /// 窗户在墙壁中的位置
    /// </summary>
    private Vector3 windowPosition;
    /// <summary>
    /// 鼠标选中的墙壁
    /// </summary>
    private Transform SelectWall;

    private Mesh CubeMesh;

    private bool StartCreatDoor;
    private bool StartCreatWindow;

    private bool startMove;
    private BrushedDoor moveDoor;
    private BrushedWindow moveWindow;
    /// <summary>
    /// 创建的mesh
    /// </summary>
    public Mesh CreatMesh { get; set; }

    public Material material { get; set; }
    public Material[] materials { get; set; }

    void Awake()
    {
        Instance = this;

        booleanRT = transform.GetComponentInChildren<BooleanRT>();
        BooleanDoor = Instantiate(Resources.Load<Transform>("Prefabs/Draw/BooleanDoor"));

        BooleanDoor.SetParent(transform);
        BooleanDoor.transform.localPosition = BrushRayCastCheck.NullVector3;

        BooleanWindow = Instantiate(Resources.Load<Transform>("Prefabs/Draw/BooleanWindow"));
        BooleanWindow.SetParent(transform);
        BooleanWindow.transform.localPosition = BrushRayCastCheck.NullVector3;


        CubeMesh = Resources.Load<MeshFilter>("Prefabs/Draw/NormalWall").sharedMesh;

        NormalDOorPrefab = Resources.Load<Transform>("Prefabs/Draw/NormalDoor");
        NormalWindowPrefab = Resources.Load<Transform>("Prefabs/Draw/NormalWindow");
        NormalDoorScale = NormalDOorPrefab.localScale;
        NormalWindowScale = NormalWindowPrefab.localScale;
    }
    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (!StartCreatDoor && !StartCreatWindow && Input.GetKeyDown(KeyCode.O))
        //{
        //    StartCreatDoor = true;
        //}
        //if (!StartCreatDoor&&!StartCreatWindow&& Input.GetKeyDown(KeyCode.U))
        //{
        //    StartCreatWindow = true;
        //}
        if (StartCreatDoor)
        {
            CreateADoorInWall();
        }
        if (StartCreatWindow)
        {
            CreateAWindowInWall();
        }
    }
    public void OnCreateDoorButtonClick()
    {
        if (!StartCreatDoor)
        {
            if (StartCreatWindow)
            {
                StartCreatWindow = false;
            }
            StartCreatDoor = true;
        }
    }

    public void OnCreateWindowButtonClick()
    {
        if (!StartCreatWindow)
        {
            if (StartCreatDoor)
            {
                StartCreatDoor = false;
            }
            StartCreatWindow = true;
        }
    }

    public void CreateAWindowInWall()
    {
        booleanRT.obj2 = BooleanWindow;
        raycastHit = BrushRayCastCheck.MouseCheckBreshedWall(layer);

        if (moveWindow != null)
        {
            BooleanWindow.localScale = new Vector3(moveWindow.Width, moveWindow.Height, moveWindow.Length);
        }
        else
        {
            BooleanWindow.localScale = NormalWindowScale;
        }
        if (raycastHit.collider != null)
        {
            if (raycastHit.normal != Vector3.up && raycastHit.normal != -Vector3.up)
            {
                SelectWall = raycastHit.collider.transform;
                BooleanWindow.SetParent(raycastHit.collider.transform.parent);
                windowPosition = raycastHit.point - (raycastHit.normal * SelectWall.localScale.x / 2);
                windowPosition = new Vector3(windowPosition.x, 1, windowPosition.z);
                booleanRT.obj1 = raycastHit.collider.transform;
                BooleanWindow.localScale = new Vector3(raycastHit.collider.transform.localScale.x + 0.1f, BooleanWindow.localScale.y, BooleanWindow.localScale.z);
                BooleanWindow.position = windowPosition;
                BooleanWindow.localPosition = new Vector3(BooleanWindow.localPosition.x, BooleanWindow.localScale.y / 2+1, BooleanWindow.localPosition.z);
                BooleanWindow.localRotation = SelectWall.localRotation;

            }
            else
            {
                SelectWall = null;
                BooleanWindow.localPosition = BrushRayCastCheck.NullVector3;
            }

        }
        else
        {
            SelectWall = null;
            BooleanWindow.localPosition = BrushRayCastCheck.NullVector3;
        }

        if (SelectWall != null && booleanRT.obj1 != null && Input.GetMouseButtonDown(0))
        {
            StartCreatWindow = false;

            if (moveWindow != null)
            {
                //如果要移动的窗还在原来的墙壁上
                if (SelectWall == moveWindow.wall.transform)
                {
                    //修改该窗的位置信息并重置该墙壁
                    moveWindow.transform.position = windowPosition;
                    moveWindow.Init(SelectWall.GetComponent<BrushedWall>());
                    ResetDoorsAndWindowsOfWall(SelectWall.GetComponent<BrushedWall>());
                    moveWindow = null;
                    return;
                }
                else
                {
                    //将原来墙壁上关于该窗的引用删除
                    moveWindow.wall.GetComponent<BrushedWall>().windows.Remove(moveWindow);
                    //重置原墙壁
                    ResetDoorsAndWindowsOfWall(moveWindow.wall);

                    //将原来的窗移动到新的位置
                    moveWindow.transform.SetParent(SelectWall.transform.parent);
                    moveWindow.transform.position = windowPosition;
                    moveWindow.transform.localRotation = SelectWall.localRotation;
                    BrushedWall brushedWall = SelectWall.GetComponent<BrushedWall>();
                    moveWindow.Init(brushedWall);
                    brushedWall.windows.Add(moveWindow);
                    ResetDoorsAndWindowsOfWall(brushedWall);
                    moveWindow = null;
                }
            }
            else
            {
                material = SelectWall.GetComponent<MeshRenderer>().material;
                booleanRT.ExecuteOnClick();
                CreatMesh = booleanRT.GetComponent<MeshFilter>().sharedMesh;
                SelectWall.GetComponent<MeshFilter>().mesh = CreatMesh;
                SelectWall.GetComponent<MeshCollider>().sharedMesh = CreatMesh;
                materials = new Material[booleanRT.GetComponent<MeshFilter>().mesh.subMeshCount];
                for (int i = 0; i < materials.Length; i++)
                {
                    materials[i] = material;
                }
                SelectWall.GetComponent<MeshRenderer>().materials = materials;

                BooleanWindow.transform.localPosition = BrushRayCastCheck.NullVector3;

                //创建窗

                NormalWindow = Instantiate(NormalWindowPrefab);
                NormalWindow.transform.SetParent(SelectWall.transform.parent);
                NormalWindow.transform.position = windowPosition;
                NormalWindow.transform.localRotation = SelectWall.localRotation;
                BrushedWindow brushedWindow = NormalWindow.GetComponent<BrushedWindow>();
                BrushedWall brushedWall = SelectWall.GetComponent<BrushedWall>();
                brushedWindow.Init(brushedWall);
                brushedWall.windows.Add(brushedWindow);
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            StartCreatWindow = false;
            BooleanWindow.position = BrushRayCastCheck.NullVector3;
            if (SelectWall != null)
            {
                SelectWall = null;
                booleanRT.obj1 = null;
            }
        }

    }

    /// <summary>
    /// 在墙壁中生成一扇门
    /// </summary>
    public void CreateADoorInWall()
    {
        booleanRT.obj2 = BooleanDoor;
        raycastHit = BrushRayCastCheck.MouseCheckBreshedWall(layer);

        if (moveDoor != null)
        {
            BooleanDoor.localScale = new Vector3(moveDoor.Width, moveDoor.Height, moveDoor.Length);
        }
        else
        {
            BooleanDoor.localScale = NormalDoorScale;
        }
        if (raycastHit.collider != null)
        {
            if (raycastHit.normal != Vector3.up && raycastHit.normal != -Vector3.up)
            {
                SelectWall = raycastHit.collider.transform;
                BooleanDoor.SetParent(raycastHit.collider.transform.parent);
                doorPosition = raycastHit.point - (raycastHit.normal * SelectWall.localScale.x / 2);
                doorPosition = new Vector3(doorPosition.x, BooleanDoor.localScale.y / 2, doorPosition.z);
                booleanRT.obj1 = raycastHit.collider.transform;
                BooleanDoor.localScale = new Vector3(raycastHit.collider.transform.localScale.x + 0.1f, BooleanDoor.localScale.y, BooleanDoor.localScale.z);
                BooleanDoor.position = doorPosition;
                BooleanDoor.localPosition = new Vector3(BooleanDoor.localPosition.x, BooleanDoor.localScale.y / 2, BooleanDoor.localPosition.z);
                BooleanDoor.localRotation = SelectWall.localRotation;

            }
            else
            {
                SelectWall = null;
                BooleanDoor.localPosition = BrushRayCastCheck.NullVector3;
            }

        }
        else
        {
            SelectWall = null;
            BooleanDoor.localPosition = BrushRayCastCheck.NullVector3;
        }

        if (SelectWall != null && booleanRT.obj1 != null && Input.GetMouseButtonDown(0))
        {
            StartCreatDoor = false;

            if (moveDoor != null)
            {
                //如果要移动的门还在原来的墙壁上
                if (SelectWall == moveDoor.wall.transform)
                {
                    //修改该门的位置信息并重置该墙壁
                    moveDoor.transform.position = doorPosition;
                    moveDoor.Init(SelectWall.GetComponent<BrushedWall>());
                    ResetDoorsAndWindowsOfWall(SelectWall.GetComponent<BrushedWall>());
                    moveDoor = null;
                    return;
                }
                else
                {
                    //将原来墙壁上关于该门的引用删除
                    moveDoor.wall.GetComponent<BrushedWall>().doors.Remove(moveDoor);
                    //重置原墙壁
                    ResetDoorsAndWindowsOfWall(moveDoor.wall);
                    
                    //将原来的门移动到新的位置
                    moveDoor.transform.SetParent(SelectWall.transform.parent);
                    moveDoor.transform.position = doorPosition;
                    moveDoor.transform.localRotation = SelectWall.localRotation;
                    BrushedWall brushedWall = SelectWall.GetComponent<BrushedWall>();
                    moveDoor.Init(brushedWall);
                    brushedWall.doors.Add(moveDoor);
                    ResetDoorsAndWindowsOfWall(brushedWall);
                    moveDoor = null;
                }
            }
            else
            {
                material = SelectWall.GetComponent<MeshRenderer>().material;
                booleanRT.ExecuteOnClick();
                CreatMesh = booleanRT.GetComponent<MeshFilter>().sharedMesh;
                SelectWall.GetComponent<MeshFilter>().mesh = CreatMesh;
                SelectWall.GetComponent<MeshCollider>().sharedMesh = CreatMesh;
                materials = new Material[booleanRT.GetComponent<MeshFilter>().mesh.subMeshCount];
                for (int i = 0; i < materials.Length; i++)
                {
                    materials[i] = material;
                }
                SelectWall.GetComponent<MeshRenderer>().materials = materials;

                

                //创建门

                NormalDoor = Instantiate(NormalDOorPrefab);
                NormalDoor.SetParent(SelectWall.transform.parent);
                NormalDoor.position = doorPosition;
                NormalDoor.localPosition = BooleanDoor.transform.localPosition;
                BooleanDoor.transform.localPosition = BrushRayCastCheck.NullVector3;
                NormalDoor.localRotation = SelectWall.localRotation;
                BrushedDoor brushedDoor = NormalDoor.GetComponent<BrushedDoor>();
                BrushedWall brushedWall = SelectWall.GetComponent<BrushedWall>();
                brushedDoor.Init(brushedWall);
                brushedWall.doors.Add(brushedDoor);
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            StartCreatDoor = false;
            BooleanDoor.position = BrushRayCastCheck.NullVector3;
            if (SelectWall != null)
            {
                SelectWall = null;
                booleanRT.obj1 = null;
            }
        }

    }
    public Transform CreateDoorByInofrmation(Door doorinfo,BrushedWall brushedWall)
    {
        Transform door = Instantiate(NormalDOorPrefab);
        door.GetComponent<MeshRenderer>().material = WallTexture.LoadDoorTexture(doorinfo.style).material;
        door.SetParent(BrushManager.Instance.BrushParent);
        BrushedDoor bd = door.GetComponent<BrushedDoor>();
        bd.Init(brushedWall);
        bd.InitInformation(doorinfo.length,doorinfo.width,doorinfo.height,doorinfo.heightAboveGround,doorinfo.distanceScale);
        bd.transform.SetParent(brushedWall.transform.parent);
        //door.localScale = new Vector3(doorinfo.width, doorinfo.height, doorinfo.length);
        //door.position = brushedWall.StartPoint.position + (brushedWall.EndPoint.position - brushedWall.StartPoint.position).normalized * doorinfo.distanceScale + new Vector3(0, NumericalConversion.GetTargetPositionYByHeight(door, doorinfo.heightAboveGround), 0);
        //door.localEulerAngles = brushedWall.transform.eulerAngles;
        return door;
    }
    public Transform CreateWindowByInformation(Window windowinfo ,BrushedWall brushedWall)
    {
        Transform window = Instantiate(NormalWindowPrefab);
        window.SetParent(BrushManager.Instance.BrushParent);
        BrushedWindow bw = window.GetComponent<BrushedWindow>();
        bw.Init(brushedWall);
        bw.InitInformation(windowinfo.length, windowinfo.width, windowinfo.height, windowinfo.heightAboveGround, windowinfo.distanceScale);
        bw.transform.SetParent(brushedWall.transform.parent);
        //window.localScale = new Vector3(windowinfo.width, windowinfo.height, windowinfo.length);
        //window.position = brushedWall.StartPoint.position + (brushedWall.EndPoint.position - brushedWall.StartPoint.position).normalized * windowinfo.distanceScale+new Vector3(0,NumericalConversion.GetTargetPositionYByHeight(window,windowinfo.heightAboveGround),0);
        //window.localEulerAngles = brushedWall.transform.eulerAngles;
        return window;
    }


    /// <summary>
    /// 重置一堵墙壁上的所有门和窗户
    /// </summary>
    public void ResetDoorsAndWindowsOfWall(BrushedWall wall)
    {
        wall.GetComponent<MeshFilter>().mesh = CubeMesh;
        wall.GetComponent<MeshCollider>().sharedMesh = CubeMesh;
        //StartCoroutine(DoResetDoorsOfWall(wall));
        if (wall.doors != null && wall.doors.Count > 0)
        {
            for (int i = 0; i < wall.doors.Count; i++)
            {
                material = wall.GetComponent<MeshRenderer>().material;
                booleanRT.obj1 = wall.transform;
                BooleanDoor.SetParent(wall.transform.parent);
                BooleanDoor.localScale = wall.doors[i].transform.localScale;
                //BooleanDoor.localPosition = wall.doors[i].transform.localPosition;
                Vector3 pos = new Vector3(0, NumericalConversion.GetTargetPositionYByHeight(wall.doors[i].transform, wall.doors[i].HeightAboveGround), 0) + wall.StartPoint.localPosition + (wall.EndPoint.localPosition - wall.StartPoint.localPosition).normalized * Vector3.Distance(wall.StartPoint.localPosition, wall.EndPoint.localPosition) * wall.doors[i].DistanceScale;
                BooleanDoor.localPosition = pos;
                wall.doors[i].transform.localPosition = pos;
                BooleanDoor.localRotation = wall.transform.localRotation;
                wall.doors[i].transform.localRotation = wall.transform.localRotation;
                booleanRT.obj2 = BooleanDoor;
                booleanRT.ExecuteOnClick();
                wall.GetComponent<MeshFilter>().mesh = booleanRT.GetComponent<MeshFilter>().mesh;
                wall.GetComponent<MeshCollider>().sharedMesh = booleanRT.GetComponent<MeshFilter>().mesh;
                materials = new Material[booleanRT.GetComponent<MeshFilter>().mesh.subMeshCount];
                for (int j = 0; j < materials.Length; j++)
                {
                    materials[j] = material;
                }
                wall.GetComponent<MeshRenderer>().materials = materials;
                BooleanDoor.SetParent(transform);
                BooleanDoor.localPosition = BrushRayCastCheck.NullVector3;

            }
        }
        if (wall.windows!=null && wall.windows.Count > 0)
        {
            for (int i = 0; i < wall.windows.Count; i++)
            {
                material = wall.GetComponent<MeshRenderer>().material;
                booleanRT.obj1 = wall.transform;
                BooleanWindow.localScale = wall.windows[i].transform.localScale;
                //BooleanDoor.localPosition = wall.doors[i].transform.localPosition;
                
                Vector3 pos = new Vector3(0, NumericalConversion.GetTargetPositionYByHeight(wall.windows[i].transform, wall.windows[i].HeightAboveGround), 0) + wall.StartPoint.localPosition + (wall.EndPoint.localPosition - wall.StartPoint.localPosition).normalized * Vector3.Distance(wall.StartPoint.localPosition, wall.EndPoint.localPosition) * wall.windows[i].DistanceScale;
                BooleanWindow.localPosition = pos;
                wall.windows[i].transform.localPosition = pos;
                BooleanWindow.localRotation = wall.transform.localRotation;
                wall.windows[i].transform.localRotation = wall.transform.localRotation;
                booleanRT.obj2 = BooleanWindow;
                Debug.LogError(BooleanWindow.transform.position);
                booleanRT.ExecuteOnClick();
                wall.GetComponent<MeshFilter>().mesh = booleanRT.GetComponent<MeshFilter>().sharedMesh;
                wall.GetComponent<MeshCollider>().sharedMesh = booleanRT.GetComponent<MeshFilter>().sharedMesh;
                materials = new Material[booleanRT.GetComponent<MeshFilter>().mesh.subMeshCount];
                for (int j = 0; j < materials.Length; j++)
                {
                    materials[j] = material;
                }
                wall.GetComponent<MeshRenderer>().materials = materials;
                BooleanWindow.localPosition = BrushRayCastCheck.NullVector3;

            }
        }
    }

    /// <summary>
    /// 移动一扇门的位置
    /// </summary>
    /// <param name="selectDoor"></param>
    public void MoveDoorPosition(BrushedDoor selectDoor)
    {
        StartCreatDoor = true;
        moveDoor = selectDoor;
    }
    public void MoveWindowPosition(BrushedWindow selectWindow)
    {
        StartCreatWindow = true;
        moveWindow = selectWindow;
    }
}
