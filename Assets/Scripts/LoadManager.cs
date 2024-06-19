using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Login;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Login;

/// <summary>
/// 用来管理加载场景动作中一些不便于删除的数据的脚本  edit by kuai
/// </summary>
public class LoadManager : MonoBehaviour {
    /// <summary>
    /// 单例
    /// </summary>
    public static LoadManager Instance;

    /// <summary>
    /// 是否是打开新场景
    /// </summary>
    private bool isOpenNewScene;
    /// <summary>
    /// 正在打开（过程中）
    /// </summary>
    public bool opening;
    /// <summary>
    /// 新打开的场景是不是vr
    /// </summary>
    public bool IsVR;
    /// <summary>
    /// 是不是发布后的场景
    /// </summary>
    public bool IsPublish;
    /// <summary>
    /// 加载新场景时用于遮盖的界面
    /// </summary>
    private GameObject panel_Loading;
    /// <summary>
    /// 管理发布的物体
    /// </summary>
    private GameObject loader;
    /// <summary>
    /// 关闭当前场景的按钮
    /// </summary>
    public GameObject CloseThisSceneGameObject;
    public string LoginAccount { get; set; }
    public string LoginPassword { get; set; }

    private bool open;
    public bool IsOpenNewScene
    {
        get
        {
            return isOpenNewScene;
        }

        set
        {
            isOpenNewScene = value;
        }
    }

    /// <summary>
    /// 全局只执行一次保证该脚本不会被删除不会被重新加载
    /// </summary>
    static LoadManager()
    {
        GameObject go = new GameObject("LoadManager");
        DontDestroyOnLoad(go);
        Instance = go.AddComponent<LoadManager>();
    }
    
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (isOpenNewScene == true)
        {
            if (loader == null)
            {

                loader = GameObject.Find("Loader");
            }
            if (loader != null && loader.activeSelf == true)
            {
                loader.SetActive(false);
                opening = true;
                StartCoroutine(DoWaitOpened());
            }
            else if (loader == null && open == false)
            {
                open = true;
                opening = true;
                StartCoroutine(DoWaitOpened());
            }
            if (panel_Loading == null)
            {
                GameObject a = GameObject.Find("Canvas");
                panel_Loading = a.transform.Find("Panel_Loading").gameObject;
            }
            if (panel_Loading != null)
            {
                //Debug.LogError(opening+"   "+panel_Loading.activeSelf);
                if (opening == true && panel_Loading.activeSelf == false)
                {
                    panel_Loading.SetActive(true);
                }
                if (opening == false && panel_Loading.activeSelf == true)
                {
                    panel_Loading.SetActive(false);
                }

            }
            if (CloseThisSceneGameObject != null)
            {
                if (CloseThisSceneGameObject.activeSelf == false)
                {
                    CloseThisSceneGameObject.SetActive(true);
                }
            }
            else
            {
                CloseThisSceneGameObject = SceneCtrl.instance.closeThisScene;
            }
        }
	}
    /// <summary>
    /// 等待打开完成的协成
    /// </summary>
    /// <returns></returns>
    public IEnumerator DoWaitOpened()
    {
        while (Manager.Instace.ISOpen==false)
        {
            yield return null;
        }
        
        if (IsVR == true)
        {
            VRSwitch.Instance.SetVRState(true);
        }
        yield return new WaitForSeconds(0.1f);
        opening = false;
        yield return new WaitForSeconds(1f);
        isOpenNewScene = false;
        open = false;
    }

    /// <summary>
    /// 设置打开的是否是vr
    /// </summary>
    /// <param name="isVr"></param>
    public void SetIsVR(bool isVr)
    {
        IsVR = isVr;
    }

    //public void SetIsOpenNewScene(bool b)
    //{
    //    IsOpenNewScene = b;
    //}

    public IEnumerator DoWaitLoadScene()
    {
        opening = true;
        StartCoroutine(DoWaitOpened());
        while (opening)
        {
            if (panel_Loading == null)
            {
                GameObject a = GameObject.Find("Canvas");
                panel_Loading = a.transform.Find("Panel_Loading").gameObject;
            }
            if (panel_Loading != null)
            {
                panel_Loading.GetComponent<Image>().color = new Color32(255, 255, 255, 80);
                //Debug.LogError(opening+"   "+panel_Loading.activeSelf);
                if (panel_Loading.activeSelf == false)
                {
                    panel_Loading.SetActive(true);
                }
                
            }
            yield return null;
        }
        if (panel_Loading.activeSelf)
        {
            panel_Loading.SetActive(false);
        }
    }
}
