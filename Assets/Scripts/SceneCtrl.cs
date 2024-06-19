using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using Assets.Scripts.Login;
using UnityEngine.UI;

public class SceneCtrl : MonoBehaviour
{
    public GameObject LightSetting;
    public static SceneCtrl instance;
    public static bool isBack;
    public static bool running;
    public static bool isAction;
    public static bool IsOpenNewScene;
    /// <summary>
    /// 新建空场景
    /// </summary>
    public static bool CreateNewScene;
    /// <summary>
    /// 打开存档的存档地址
    /// </summary>
    public static string OpenStatePath;
    public event Action PlayScene;

    /// <summary>
    /// 关闭当前场景返回初始场景的按钮
    /// </summary>
    public GameObject closeThisScene;

    // Use this for initialization
    void Start()
    {

        print("StartFunction");

        if (instance == null)
        {
            instance = this;
        }
        if (isBack)
        {

            StartCoroutine(OpenTemp());
            //Open ();
            isBack = false;
        }
        Manager.Instace.ctrllerEventsR.ButtonTwoPressed += HandBtnTwoFun;

        if (CreateNewScene)
        {
            HideLoginPanelWhileCreateNewScene();
            CreateNewScene = false;
        }
        if (!string.IsNullOrEmpty(OpenStatePath))
        {
            OpenState();
            OpenStatePath = "";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void HandBtnTwoFun(object sender, VRTK.ControllerInteractionEventArgs e)
    {
        StopRun();
    }

    public void PlaySceneClicked()
    {
        if (PlayScene != null)
        {
            PlayScene();
        }
    }

    public void StopRun()
    {

        if (running)
        {
            print("Stop run");
            isBack = true;
            running = false;
            isAction = false;
            Manager.Instace.OpenNewScene();
        }
    }

    /// <summary>
    /// 打开场景
    /// </summary>
    public void OpenScene(bool IsNewScene = false)
    {
        if (IsNewScene == true)
        {
            IsOpenNewScene = true;
            LoadManager.Instance.IsOpenNewScene = true;
        }
        else
        {
            IsOpenNewScene = false;
        }
        //if (IsNewScene==true&&running==true)
        //{
        //    print("Stop run");

        //    IsOpenNewScene = true;

        //}
        //else
        //{
        //    IsOpenNewScene = false;
        //}
        isBack = true;
        running = false;

        Manager.Instace.OpenNewScene();
    }
    IEnumerator OpenTemp()
    {
        print("Open Temp");
        //todo 关闭登录页面
        //Manager.Instace.LoginPanel.GetComponent<NewLoginPanelComponent>().HidePanel();
        yield return Manager.Instace;
        Open(IsOpenNewScene);
    }
    /// <summary>
    /// 新建空场景时隐藏登陆界面
    /// </summary>
    private void HideLoginPanelWhileCreateNewScene()
    {
        Manager.Instace.LoginPanel.GetComponent<NewLoginPanelComponent>().HidePanel();
    }
    /// <summary>
    /// 打开存档
    /// </summary>
    private void OpenState()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(OpenStatePath, FileMode.Open);
        Manager.Instace.allDataInformation = (AllDataInformation)bf.Deserialize(file);
        file.Close();
        StartCoroutine(Manager.Instace.InitialAll());
    }

    public void SetIsBack(bool back)
    {
        isBack = back;
        Manager.Instace.ChangePlaying(false);
    }

    public void Save()
    {
        if (PlayScene != null)
        {
            PlayScene();
        }
        running = true;
        //Manager.Instace._Playing = true;
        //记录物体的scale,pos,rotate
        Manager.Instace.allDataInformation.listTree.Root.data.GetCameraTransform();
        List<ListTreeNode<Base>> objects = Manager.Instace.allDataInformation.listTree.Root.children;
        foreach (ListTreeNode<Base> obj in objects)
        {
            obj.data.GetTransform();
            ObjectInfo objectInfo = (ObjectInfo)obj.data;
            if (!objectInfo.IsScene)
            {
                if (obj.children.Count > 0)
                {
                    ListTreeNode<Base> fsm = obj.children[0];
                    fsm.data.GetTransform();
                    foreach (ListTreeNode<Base> stateInfo in fsm.children)
                    {
                        //获得状态的当前位置，name,description
                        stateInfo.data.GetTransform();
                        foreach (ListTreeNode<Base> eventInfo in stateInfo.children)
                        {
                            eventInfo.data.GetTransform();
                            foreach (ListTreeNode<Base> lineInfo in eventInfo.children)
                            {
                                lineInfo.data.GetTransform();
                            }
                        }
                    }
                }
            }
        }
        BrushManager.Instance.SaveAllBrushObjInformation();
        BinaryFormatter bf = new BinaryFormatter();
        string path = Application.dataPath + @"/temp.txt";
        //FileStream file = File.Create(Application.dataPath + "/SaveFile/save.txt");
        if (!string.IsNullOrEmpty(path))
        {
            FileStream file = File.Create(path);
            bf.Serialize(file, Manager.Instace.allDataInformation);
            file.Close();
        }
    }

    public void Open(bool IsNewScene)
    {

        if (Manager.Instace.allDataInformation.listTree != null)
        {
            foreach (ListTreeNode<Base> node in Manager.Instace.allDataInformation.listTree.Root.children)
            {
                Manager.Instace.DestroyData(node);
            }

        }

        BinaryFormatter bf = new BinaryFormatter();
        string path;
        path = Application.dataPath + @"/temp.txt";
        if (isAction && IsNewScene)
        {
            path = Application.dataPath + @"/" + LoadScene.targetPath;
        }
        else if (isAction && !IsNewScene)
        {
            //isAction = false;
            if (LoadManager.Instance.IsPublish == false)
            {
                path = Application.dataPath + "/SaveFile/automatic.txt";
            }
            else
            {
                DirectoryInfo folder = new DirectoryInfo("SaveFile");
                FileInfo[] files = folder.GetFiles("*.txt");
                path = files[0].FullName;
            }
            //path = Application.dataPath + "/SaveFile/automatic.txt";
        }
        else if (!isAction && !IsNewScene)
        {
            //path = Application.dataPath + @"/temp.txt";
        }
        else
        {
            Debug.LogError("怎么会到这里来");
        }
        //Debug.LogError("读取文本路径：" + path);
        if (!string.IsNullOrEmpty(path))
        {
            FileStream file = File.Open(path, FileMode.Open);
            Manager.Instace.allDataInformation = (AllDataInformation)bf.Deserialize(file);
            file.Close();
            StartCoroutine(LoadManager.Instance.DoWaitLoadScene());
            StartCoroutine(Manager.Instace.InitialAll());
            
            //LightSetting.SetActive(false);

            if (isAction)
            {
                //Manager.Instace._IsPublish = true;
                //Manager.Instace.SetMainActive();
                StartCoroutine(DoWaitToSetMainActive());
            }
        }
    }
    IEnumerator DoWaitToSetMainActive()
    {
        while (Manager.Instace.ISOpen == false)
        {
            yield return null;
        }
        Manager.Instace.SetMainActive();
    }
    /// <summary>
    /// 打开原本的场景
    /// </summary>
    //public void OpenFirstScene()
    //{
    //    if (Manager.Instace.listTree != null)
    //    {
    //        foreach (ListTreeNode<Base> node in Manager.Instace.listTree.Root.children)
    //        {
    //            Manager.Instace.DestroyData(node);
    //        }

    //    }

    //    BinaryFormatter bf = new BinaryFormatter();
    //    string path = Application.dataPath + @"/temp.txt";

    //    if (isAction)
    //    {
    //        path = Application.dataPath + @"/" + LoadScene.targetPath;
    //    }

    //    if (!string.IsNullOrEmpty(path))
    //    {
    //        FileStream file = File.Open(path, FileMode.Open);
    //        Manager.Instace.listTree = (ListTree<Base>)bf.Deserialize(file);
    //        file.Close();
    //        Manager.Instace.InitialAll();
    //        //LightSetting.SetActive(false);

    //        if (isAction)
    //        {
    //            //Manager.Instace._IsPublish = true;
    //            Manager.Instace.SetMainActive();
    //        }
    //    }
    //}
}
