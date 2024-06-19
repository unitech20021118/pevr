using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using LitJson;
using UnityEngine;
using UnityEngine.UI;

public class CloudDownloadControl : MonoBehaviour
{
    public CloudDownWWW CloudDownWww = new CloudDownWWW();
    public Transform ItemContentTransform;
    public GameObject CommodityPrefab;

    public GameObject MessageBoxGameObject;
    public Button MessageBoxConformBtn;

    public bool Refresh = true;


	// Use this for initialization
	void Start () 
    {
    }
	
	// Update is called once per frame
	void Update () 
    {
        if (Refresh)
        {
            Refresh = false;
            StartCoroutine(DoCloudDownLoadButtonClick());
        }
	}

    public void CloudDOwnLoadButtonClick()
    {
        StartCoroutine(DoCloudDownLoadButtonClick());
    }
	/// <summary>
	/// 云端的下载按钮点击时
	/// </summary>
    public IEnumerator DoCloudDownLoadButtonClick()
    {
		//请求云端所有存档的列表
		Debug.Log(CloudControl.Instance.cloudTxtPath.CloudFindALLPath + LoadManager.Instance.LoginAccount);
		WWW	www = new WWW(CloudControl.Instance.cloudTxtPath.CloudFindALLPath+ LoadManager.Instance.LoginAccount);
        yield return www;
        if (www.error!=null)
        {
            Debug.LogError(www.error);
        }
        else
        {
            CloudDownWww = JsonMapper.ToObject<CloudDownWWW>(www.text);
            ShowItemsByArchiveInformation();
        }
    }

    /// <summary>
    /// 显示用户上传到云端的存档文件
    /// </summary>
    public void ShowItemsByArchiveInformation()
    {
        TheTools.Tools.Instance.DeleteAllChild(ItemContentTransform);
        if (CloudDownWww.body.Count>0)
        {
            for (int i = 0; i < CloudDownWww.body.Count; i++)
            {
                GameObject obj = Instantiate(CommodityPrefab);
                obj.transform.SetParent(ItemContentTransform);
                obj.transform.localScale = Vector3.one;
                obj.transform.Find("NameText").GetComponent<Text>().text = CloudDownWww.body[i].title;
                string theid = CloudDownWww.body[i].id;
                obj.transform.Find("DownLoadBtn").GetComponent<Button>().onClick.AddListener(delegate
                {
                    StartCoroutine(DoDownArchive(theid));
                });
                obj.transform.Find("BtnClose").GetComponent<Button>().onClick.AddListener(delegate
                {
                    MessageBoxGameObject.SetActive(true);
                    MessageBoxGameObject.transform.Find("Text").GetComponent<Text>().text = "确定要删除该存档吗？";
                    MessageBoxConformBtn.onClick.RemoveAllListeners();
                    MessageBoxConformBtn.onClick.AddListener(delegate
                    {
                        StartCoroutine(DoDeleteArchive(theid));
                    });
                });
            }
        }
    }

    //public void DownLoadArchive(string id)
    //{
    //    Debug.LogError(id);
    //    StartCoroutine(DoDownArchive(id));
    //}

    //public void DeleteArchive(string id)
    //{
    //    Debug.LogError(id);
    //    StartCoroutine(DoDeleteArchive(id));
    //}

    
    public IEnumerator DoDeleteArchive(string id)
    {
        WWW www = new WWW(CloudControl.Instance.cloudTxtPath.CloudDeletePath+id);
        yield return www;
        if (www.error!=null)
        {
            Debug.LogError(www.error);
        }
        else
        {
            if (JsonMapper.ToObject<DeleteArchive>(www.text).code=="200")
            {
                Debug.Log("删除成功！");
                MessageBoxGameObject.SetActive(false);
                Refresh = true;
            }
        }
    }


    public IEnumerator DoDownArchive(string id)
    {
        string path = IOHelper.OpenFileDlgToSave();
        string url = CloudControl.Instance.cloudTxtPath.CloudFindSinglePath + id;

        WWW www = new WWW(CloudControl.Instance.cloudTxtPath.CloudFindSinglePath+id);

        yield return www;
        if (www.error!=null)
        {
            Debug.LogError(www.error);
        }
        else
        {
            if (!string.IsNullOrEmpty(path))
            {
                if (!File.Exists(path))
                {
                    File.Create(path).Close();
                    
                }
                // string str =  System.Text.Encoding.Default.GetString(JsonMapper.ToObject<DownArchive>(www.text).body);
                // Debug.Log(str);
                // File.WriteAllText(path, JsonMapper.ToObject<DownArchive>(www.text).body);
               
                if (www.bytes != null)
                {
                    for (int i = 0; i < 20; i++)
                    {
                        //Debug.LogError(www.bytes[i]);
                    }
                    //string str = Encoding.GetEncoding("GB2312").GetString(www.bytes);
                    // File.WriteAllBytes(path, www.bytes);
                    File.WriteAllBytes(path, www.bytes);
                    //Debug.LogError(www.bytes.Length);
                    //byte[] buffur = www.bytes;
                    //BinaryFormatter bf3 = new BinaryFormatter();
                    //MemoryStream stream = new MemoryStream(buffur, 0, buffur.Length);
                    //AllDataInformation alldata = (AllDataInformation)bf3.Deserialize(stream);
                    //FileStream file2 = File.Create(path);
                    //BinaryFormatter bf2 = new BinaryFormatter();
                    //bf2.Serialize(file2, alldata);
                    //file2.Close();

                }
               

            }
        }
    }
}

public class DeleteArchive
{
    public DeleteArchiveBody body;
    public string message;
    public string code;
}

public class DeleteArchiveBody
{
    public string id;
}
public class DownArchive
{
    public string body;
    public string message;
    public string code;
    public DownArchive(){}

    public DownArchive(string body, string message, string code)
    {
        this.body = body;
        this.message = message;
        this.code = code;
    }
}
public class CloudDownWWW
{
    public List<ArchiveInformation> body;
    public string message;
    public string code;
    public CloudDownWWW(){}

    public CloudDownWWW(List<ArchiveInformation> body, string message, string code)
    {
        this.body = body;
        this.message = message;
        this.code = code;
    }
}

/// <summary>
/// 云端的存档的相关信息
/// </summary>
public class ArchiveInformation
{
    public string id;
    public string title;
    public string creatTime;
    public string username;
}
