using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class G_MouseListener : MonoBehaviour {



    //private static G_MouseListener _instance;
    //public static G_MouseListener Instance
    //{
    //    get
    //    {
    //        if (_instance == null)
    //        {
    //            _instance = new G_MouseListener();
    //        }
    //        return _instance;
    //    }
    //}
    /// <summary>
    /// 是否是运行状态（用于判断如果是运行状态使该脚本仅保留监听鼠标右键的功能）
    /// </summary>
    private bool IsPlaying;
    private static G_MouseListener Instance;
    public static G_MouseListener GetInstance()
    {
        return G_MouseListener.Instance;
    }

    void Awake()
    {
        Instance = this;
    }
    

    public delegate void Listener();  
     Listener LMouseDown;
     Listener LMouseUp;
     Listener RMouseDown;
     Listener RMouseUp;
     Listener ScrollMouseDown;
     Listener ScrollMouseUp;
    /// <summary>
    /// 修改是否是运行状态
    /// </summary>
    public bool ChangePlaying
    {
        get
        {
            return IsPlaying;
        }

        set
        {
            IsPlaying = value;
        }
    }

    public delegate void ClickCurrentObject(GameObject obj);//定义一个委托类型
     public event ClickCurrentObject OnClickCurrentObject;//声明点击物体事件 

     public void AddScrollMouseDown(Listener _ScrollMouseDown)
     {
         ScrollMouseDown += _ScrollMouseDown;
     }

     public void DeleteScrollMouseDown(Listener _ScrollMouseDown)
     {
         ScrollMouseDown -= _ScrollMouseDown;
     }

     public void AddScrollMouseUp(Listener _ScrollMouseUp)
     {
         ScrollMouseUp += _ScrollMouseUp;
     }

     public void DeleteScrollMouseUp(Listener _ScrollMouseUp)
     {
         ScrollMouseUp -= _ScrollMouseUp;
     }

     public void AddLMouseDown(Listener _LMouseDown)
     {
         LMouseDown += _LMouseDown;
     }

     public void DeleteMouseDown(Listener _LMouseDown)
     {
         LMouseDown -= _LMouseDown;
     }

     public void AddLMouseUp(Listener _LMouseUp)
     {
         LMouseUp += _LMouseUp;
     }

     public void DeleteLMouseUp(Listener _LMouseUp)
     {
         LMouseUp -= _LMouseUp;
     }

     public void AddRMouseDown(Listener _RMouseDown)
     {
         RMouseDown += _RMouseDown;
     }

     public void DeleteRMouseDown(Listener _RMouseDown)
     {
         RMouseDown -= _RMouseDown;
     }

     public void AddRMouseUp(Listener _RMouseUp)
     {
         RMouseUp += _RMouseUp;
     }

     public void DeleteRMouseUp(Listener _RMouseUp)
     {
         RMouseUp -= _RMouseUp;
     }

     public void RegisterOnClickObject(ClickCurrentObject _ClickObject)
     {
         OnClickCurrentObject += _ClickObject;
     }

     public void DeleteOnClickObject(ClickCurrentObject _ClickObject)
     {
         OnClickCurrentObject -= _ClickObject;
     }

     public void DoOnCLickObject(GameObject obj)
     {
         if (OnClickCurrentObject != null)
         {
             OnClickCurrentObject(obj);
         }
     }

     void Update()
     {
         //if (LMouseDown != null && Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
         //当是编辑状态时可以监听所有的鼠标功能
         if (!IsPlaying)
         {
            if (Input.GetMouseButtonDown(0) && LMouseDown != null && !EventSystem.current.IsPointerOverGameObject())
             {
                 LMouseDown();
             }
             if (Input.GetMouseButtonUp(0) && LMouseUp != null)// && !EventSystem.current.IsPointerOverGameObject()
             {
                 LMouseUp();
             }
             if (Input.GetMouseButtonUp(1) && RMouseUp != null)
             {
                 RMouseUp();
                 //Debug.LogError("up");
             }
             if (Input.GetMouseButtonDown(1) && RMouseDown != null && !EventSystem.current.IsPointerOverGameObject())
             {
                 RMouseDown();
                 //Debug.LogError("down");
             }

             if (Input.GetMouseButtonDown(2) && ScrollMouseDown != null && !EventSystem.current.IsPointerOverGameObject())
             {
                 ScrollMouseDown();

             }

             if (Input.GetMouseButtonUp(2) && ScrollMouseUp != null)
             {
                 ScrollMouseUp();

             }
        }
         //当不是编辑状态时仅保留监听鼠标右键功能
         else
         {
            if (Input.GetMouseButtonUp(1) && RMouseUp != null)
             {
                 RMouseUp();
                 //Debug.LogError("up");
             }
             if (Input.GetMouseButtonDown(1) && RMouseDown != null)
             {
                 RMouseDown();
                 //Debug.LogError("down");
             }
        }
         
     }
}
