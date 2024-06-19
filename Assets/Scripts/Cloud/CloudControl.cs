using Assets.Scripts.Login;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class CloudControl : MonoBehaviour
{
    public static CloudControl Instance;
    public GameObject box;

    private bool isOpen;

    public CloudTxtPath cloudTxtPath;

    public CloudDownloadControl CloudDownloadControl;
    public Button DownButton;


    void Awake()
    {
        Instance = this;

        cloudTxtPath = JsonMapper.ToObject<CloudTxtPath>(File.ReadAllText(Application.streamingAssetsPath + "/txtcloudPath.txt", Encoding.Unicode));
    }

    void Start () {
        isOpen = false;
        DownButton.onClick.AddListener(delegate
        {
            CloudDownloadControl.gameObject.SetActive(true);
            CloudDownloadControl.Refresh = true;
            box.SetActive(false);

        });
    }
	
	
	void Update () {
		
	}

    /// <summary>
    /// 当打开云平台按钮点击时
    /// </summary>
    public void OpenCloudButtonClick()
    {
        //判断是否已经登录
        //如果没登录弹出登录提示框
        if (string.IsNullOrEmpty(LoginProxy.Proxy.AccountId))
        {

            Manager.Instace.LoginPanel.SetActive(true);
            Manager.Instace.LoginPanel.transform.Find("BackGround").gameObject.SetActive(true);
            Manager.Instace.LoginPanel.transform.Find("Login").gameObject.SetActive(true);
            Manager.Instace.LoginPanel.GetComponent<NewLoginPanelComponent>().MessageBox.ShowMassage("请先登录");
        }
        else//如果登录了，打开用户反馈界面
        {
            isOpen = !isOpen;
            box.SetActive(isOpen);
        }
    }
}


/// <summary>
/// 云端存档请求地址的类
/// </summary>
public class CloudTxtPath
{
    public string CloudUploadPath;
    public string CloudFindALLPath;
    public string CloudFindSinglePath;
    public string CloudDeletePath;

    public CloudTxtPath() { }

    public CloudTxtPath(string cloudUploadPath, string cloudFindALLPath, string cloudFindSinglePath, string cloudDeletePath)
    {
        CloudUploadPath = cloudUploadPath;
        CloudFindALLPath = cloudFindALLPath;
        CloudFindSinglePath = cloudFindSinglePath;
        CloudDeletePath = cloudDeletePath;
    }

}
