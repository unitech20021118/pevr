using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TheTools;

public class ShowPowerpointUI : ActionUI
{
    /// <summary>
    /// 文件夹名（也是该ppt的名字）
    /// </summary>
    private string folderName;
    /// <summary>
    /// 打开ppt转图片的工具
    /// </summary>
    public Button OpenPPT;
    /// <summary>
    /// 用于打开文件夹或图片文件的按钮
    /// </summary>
    public Button OpenButton;
    /// <summary>
    /// 自动播放选项
    /// </summary>
    public Toggle AutoPlayToggle;
    /// <summary>
    /// 循环播放选项
    /// </summary>
    public Toggle LoopToggle;
    /// <summary>
    /// 间隔时间的输入框
    /// </summary>
    public InputField IntervalInputField;

    private GameObject selectDirectoryGameObject;
    private GameObject selectDirectoryItemPrefab;
    private ShowPowerPointInforma _showPowerPointInforma;
    private ShowPowerPoint _showPowerPoint;
   

    public GameObject SelectDirectoryGameObject
    {
        get
        {
            if (selectDirectoryGameObject==null)
            {
                selectDirectoryGameObject = Instantiate(Resources.Load<GameObject>("Prefabs/PPTSelectDirectory"));
                selectDirectoryGameObject.transform.SetParent(Manager.Instace.transform);
                selectDirectoryGameObject.transform.localPosition = new Vector3(215f,70f,0f);
                
                selectDirectoryItemPrefab = Resources.Load<GameObject>("Prefabs/PPTSelectItem");

            }
            return selectDirectoryGameObject;
        }
    }

    public override Action<Main> CreateAction()
    {
        action = new ShowPowerPoint();
        actionInforma = new ShowPowerPointInforma(true);
        _showPowerPoint = (ShowPowerPoint)action;
        _showPowerPointInforma = (ShowPowerPointInforma)actionInforma;
        //初始化相关变量
        _showPowerPointInforma.FolderName = "";
        _showPowerPoint.FolderName = "";
        _showPowerPointInforma.AutoPlay = false;
        _showPowerPoint.AutoPlay = false;
        _showPowerPointInforma.Loop = false;
        _showPowerPoint.Loop = false;
        _showPowerPointInforma.Interval = 2f;
        _showPowerPoint.Interval = 2f;

        GetStateInfo().actionList.Add(actionInforma);
        actionInforma.name = "ShowPowerPoint";

        return base.CreateAction();
    }

    public override Action<Main> LoadAction(ActionInforma actionInforma)
    {
        _showPowerPointInforma = (ShowPowerPointInforma)actionInforma;
        this.actionInforma = actionInforma;
        action = new ShowPowerPoint();
        _showPowerPoint = (ShowPowerPoint)action;

        //读取数据
        _showPowerPoint.FolderName = _showPowerPointInforma.FolderName;
        _showPowerPoint.AutoPlay = _showPowerPointInforma.AutoPlay;
        _showPowerPoint.Loop = _showPowerPointInforma.Loop;
        _showPowerPoint.Interval = _showPowerPointInforma.Interval;
        folderName = _showPowerPointInforma.FolderName;
        OpenButton.GetComponentInChildren<Text>().text = folderName;
        AutoPlayToggle.isOn = _showPowerPointInforma.AutoPlay;
        LoopToggle.isOn = _showPowerPointInforma.Loop;
        IntervalInputField.text = _showPowerPointInforma.Interval.ToString();


        return base.LoadAction(actionInforma);
    }

    public void OnAutoPlayChanged()
    {
        _showPowerPointInforma.AutoPlay = AutoPlayToggle.isOn;
        _showPowerPoint.AutoPlay = AutoPlayToggle.isOn;
    }

    public void OnLoopChanged()
    {
        _showPowerPointInforma.Loop = LoopToggle.isOn;
        _showPowerPoint.Loop = LoopToggle.isOn;
    }

    public void OnIntervalChanged()
    {
        _showPowerPointInforma.Interval = float.Parse(IntervalInputField.text);
        _showPowerPoint.Interval = float.Parse(IntervalInputField.text);
    }
    

    /// <summary>
    /// 打开文件夹的方法
    /// </summary>
    public void OpenFile()
    {
        GameObject PPTItem;
        //FileInfo fileInfo = new FileInfo(IOHelper.GetImageName());
        //fileName = fileInfo.Name;
        //Debug.LogError(fileName);
        //打开选择文件夹的选择界面
        SelectDirectoryGameObject.SetActive(true);
        string[] paths = Directory.GetDirectories(Application.streamingAssetsPath + "/PowerPoint");
        Tools.Instance.DeleteAllChild(selectDirectoryGameObject.transform.Find("Viewport/Content"));
        foreach (string path in paths)
        {
            PPTItem = Instantiate(selectDirectoryItemPrefab);
            PPTItem.transform.SetParent(selectDirectoryGameObject.transform.Find("Viewport/Content"));
            PPTItem.GetComponentInChildren<Text>().text = Tools.Instance.GetFolderNameByFolderPath(path);
            string folName = Tools.Instance.GetFolderNameByFolderPath(path);
            PPTItem.GetComponent<Button>().onClick.AddListener(delegate
            {
                OpenButton.GetComponentInChildren<Text>().text = folName;
                _showPowerPoint.FolderName = folName;
                _showPowerPointInforma.FolderName = folName;
                selectDirectoryGameObject.SetActive(false);
            });
        }
    }
    /// <summary>
    /// 打开ppt转图片工具
    /// </summary>
    public void OpenPPTToPicture()
    {
        File.WriteAllText(Application.streamingAssetsPath+ "/PPTToPicture/Path.txt",Application.streamingAssetsPath+ "/PowerPoint",Encoding.Unicode);
        ProcessStartInfo startInfo = new ProcessStartInfo();
        startInfo.WorkingDirectory = Application.streamingAssetsPath + "/PPTToPicture";
        startInfo.FileName = Application.streamingAssetsPath + "/PPTToPicture/PPTToPictures.exe";
        Process.Start(startInfo);
    }

}
