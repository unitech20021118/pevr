using Assets.Scripts.Login;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class TXTCloudUploadMsg
{
    
    public string message { get; set; }
    public string code { get; set; }
}

public class CloudUploadControl : MonoBehaviour {

    public GameObject file_image;
    public Text project_file;
    public Text Message;

    private string s;
    private string url;
    private byte[] buffur;


    void Start () {
        s = CloudControl.Instance.cloudTxtPath.CloudUploadPath;
       
    }
	
	
	void Update () {
		
	}

    public void Find_File()
    {

      file_image.SetActive(true);
      Open();
        
    }

    /// <summary>
    /// 打开存档
    /// </summary>
    private void OpenState(string OpenStatePath)
    {

        //FileStream file = File.Open(OpenStatePath, FileMode.Open);

        //BinaryFormatter bf = new BinaryFormatter();
        //AllDataInformation alldata = (AllDataInformation)bf.Deserialize(file);
        //BinaryFormatter formatter = new BinaryFormatter();
        //MemoryStream rems = new MemoryStream();
        //formatter.Serialize(rems, alldata);
        //buffur = rems.GetBuffer();



        //string[] temp = file.Name.Split('\\');
        //project_file.text = temp[temp.Length - 1];
        //file.Close();
        //string account = LoadManager.Instance.LoginAccount;
        //string content = File.ReadAllText(OpenStatePath, Encoding.Default);
        // url = s + "username=" + account  + "&title=" + project_file.text;

        buffur = File.ReadAllBytes(OpenStatePath);
        string[] temp = OpenStatePath.Split('\\');
        project_file.text = temp[temp.Length - 1];
        string account = LoadManager.Instance.LoginAccount;
        //string content = File.ReadAllText(OpenStatePath, Encoding.Default);
        url = s + "username=" + account + "&title=" + project_file.text;
        File.WriteAllBytes(Application.dataPath + "/123.pevrsf",buffur);

    }

    public void FileUpload()
    {

         StartCoroutine(RepotSendGet(url));
    }

    public IEnumerator RepotSendGet(string url)
    {
        //url = url.Replace("pevrsf", "txt");
        Debug.Log(url);
        
        WWWForm form = new WWWForm();
        form.AddBinaryData("txt", buffur);
        //WWW postData = new WWW(url, buffur);
        WWW postData = new WWW(url, form);
        yield return postData;
        if (postData.error != null)
        {
            Debug.Log(postData.error);
            //显示反馈失败页面
           
        }
        else
        {
            //获取返回值
            Debug.Log(postData.text);
            var resultText = postData.text;
            var msg = JsonMapper.ToObject<TXTCloudUploadMsg>(resultText);
            Debug.Log(msg.message + " " + msg.code);
            // 反馈成功
            switch (msg.code)
            {
                case "200":
                    //显示反馈成功界面
                    Debug.Log(msg.message + " " + msg.code);
                    Message.text = "上传成功!";
                    transform.Find("MassageBox").gameObject.SetActive(true);
                    
                    break;
                case "201":
                    //显示反馈成功界面
                    Debug.Log(msg.message + " " + msg.code);
                    Message.text = "已经存在相同标题脚本文件!";
                    transform.Find("MassageBox").gameObject.SetActive(true);

                    break;
                    
                default:
                    //显示反馈失败页面
                    Message.text = "上传失败!";
                    transform.Find("MassageBox").gameObject.SetActive(true);
                    Debug.Log(msg.message + " " + msg.code);
                    break;
            }
           
        }
    }

    /// <summary>
    /// 加载存档
    /// </summary>
    private void Open()
    {
        string path = IOHelper.OpenFileDlgToLoad();
        if (!string.IsNullOrEmpty(path))
        {
            OpenState(path);

        }
        else
        {
            Debug.LogError("未选择存档");
        }

        
    }

    public void Clean()
    {
        file_image.SetActive(false);
        project_file.text = "";

    }
}
