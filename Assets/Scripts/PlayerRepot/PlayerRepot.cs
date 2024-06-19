using Assets.Scripts.Login;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using UnityEngine;
using UnityEngine.UI;

public class PlayerRepot : MonoBehaviour
{
    private Transform playerRepotPanel;
    private InputField titleInputField;
    private InputField contentInputField;

    private GameObject titleTipText;

    private GameObject contentTipText;
	// Use this for initialization
	void Start ()
	{
	    playerRepotPanel = GameObject.Find("Canvas").transform.Find("PlayerRepotPanel");

        titleInputField = playerRepotPanel.Find("report").GetComponent<InputField>();
        contentInputField = playerRepotPanel.Find("content").GetComponent<InputField>();
	    titleTipText = playerRepotPanel.Find("TitleTip").gameObject;
	    contentTipText = playerRepotPanel.Find("ContentTip").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    /// <summary>
    /// 当打开用户反馈按钮点击时
    /// </summary>
    public void OpenPlayerReportPanelButtonClick()
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
            GameObject.Find("Canvas").transform.Find("PlayerRepotPanel").gameObject.SetActive(true);
        }
    }
    /// <summary>
    /// 当发送反馈按钮点击时
    /// </summary>
    public void SendButtonClick()
    {
        if (string.IsNullOrEmpty(titleInputField.text))
        {
            titleTipText.SetActive(true);
            return;
        }
        else
        {
            titleTipText.SetActive(false);
        }
        //todo 提交反馈信息
        if (string.IsNullOrEmpty(contentInputField.text))
        {
            contentTipText.SetActive(true);
            return;
        }
        else
        {
            contentTipText.SetActive(false);
        }
        NewLoginPanelComponent.Instance.OnPlayerRepot(titleInputField.text,contentInputField.text);
    }
    
}
