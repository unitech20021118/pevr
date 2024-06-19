using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using LitJson;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using Assets.Scripts.DownLoadOnLine;
using System.Linq;

public class PreviewItem : MonoBehaviour
{
    public ModelInformation shopData;
    /// <summary>
    /// 下载按钮
    /// </summary>
    public Button DownLoadButton;
    /// <summary>
    /// 下载进度条
    /// </summary>
    public Scrollbar Progress;
    /// <summary>
    /// 按钮提示文字
    /// </summary>
    //public Text ButtonText;
    /// <summary>
    /// 该件物品是否已下载
    /// </summary>
    private bool downLoaded;
    /// <summary>
    /// 该件物品是否正在下载
    /// </summary>
    public bool DownLoading;
    //private string modelPath;
    //private string typeTXT;
    private bool startDownLoad;
    /// <summary>
    /// 是否选中的toggle
    /// </summary>
    public Toggle SelectedToggle;
    void Start()
    {

    }

    void Update()
    {
        //if (startDownLoad)
        //{
        //    if (!Progress.gameObject.activeSelf)
        //    {
        //        Progress.gameObject.SetActive(true);
        //    }
        //    Progress.size = Mathf.Lerp(Progress.size, 0.9f, 0.01f);
        //}
    }
    public void SetShopData(ModelInformation shopData,bool downloaded)
    {
        this.shopData = shopData;
        this.downLoaded = downloaded;
        DownLoadButton.gameObject.SetActive(true);
        Progress.gameObject.SetActive(false);
        transform.FindChild("NameText").GetComponent<Text>().text = shopData.ModelName_CH;
        DownLoadButton.onClick.RemoveAllListeners();
        DownLoadButton.onClick.AddListener(DownLoad);
        //transform.GetChild(1).GetComponent<Text>().text = shopData.Price;

        if (ShopManager.Instance.BulkDownloadList.FirstOrDefault(a=>a.ModelID==shopData.ModelID)!=null)
        {
            SelectedToggle.isOn = true;
        }
        else
        {
            SelectedToggle.isOn = false;
        }
        SetDownLoaded(downloaded);
        
    }
    //public void SetUrl(string imgUrl,string modelUrl,string name)
    //{
    //    //将字符串末尾的换行符和回车符删除
    //    modelUrl = modelUrl.TrimEnd((char[])"\n\r".ToCharArray());
    //    this.imgUrl = imgUrl;
    //    this.modelUrl = modelUrl;
    //    this.name = name;
    //}
    public void DownLoad()
    {
        //GetTypePath();
        DownLoadButton.enabled = false;
        //ButtonText.text = "下载中";
        startDownLoad = true;
        DownLoadButton.gameObject.SetActive(false);
        Progress.gameObject.SetActive(true);
        StartCoroutine(DoDownLoad());
        StartCoroutine(DoProgress());
    }
    /// <summary>
    /// 下载图片和模型的协成
    /// </summary>
    /// <returns></returns>
    IEnumerator DoDownLoad()
    {
        DownLoading = true;
        //预览图已经加载出来了
        if (transform.Find("icon").gameObject.activeSelf)
        {
            byte[] imgbyte = transform.Find("icon").GetComponent<Image>().sprite.texture.EncodeToJPG();
            File.WriteAllBytes(Application.streamingAssetsPath + "/ink2/" + shopData.ModelName_EN + ".png", imgbyte);
        }
        else
        {
            WWW image = new WWW(ShopManager.Instance.SpritePath + shopData.ModelID);
            //WWW image = new WWW(@"file:///"+shopData.SpritePath);
            yield return image;
            if (image.error == null)
            {
                byte[] dataBytes = image.bytes;
                File.WriteAllBytes(Application.streamingAssetsPath + "/ink2/" + shopData.ModelName_EN + ".png", dataBytes);
            }
            else
            {
                Debug.LogError("下载模型预览图时出现错误：" + image.error);
            }
            yield return null;
        }
        
        //将字符串末尾的换行符和回车符删除
        //string url = shopData.downLoadPath.TrimEnd((char[]) "\n\r".ToCharArray());
        //WWW model = new WWW(@"file:///" + url);
        WWW model =new WWW(ShopManager.Instance.ModelPath+shopData.ModelID);
        while (!model.isDone)
        {
            yield return null;
            //Progress.size = model.progress;
        }
        if (model.error==null)
        {
            startDownLoad = false;
            byte[] dataBytes = model.bytes;
            //Debug.LogError(TheTools.Tools.Instance.GetAssteBundlesPath() + "/"+shopData.ModelTypeName_EN+"/" + shopData.ModelName_EN + ".3dpro");
            File.WriteAllBytes(TheTools.Tools.Instance.GetAssteBundlesPath() + "/" + shopData.ModelTypeName_EN + "/" + shopData.ModelName_EN + ".3dpro", dataBytes);
            //ButtonText.text = "下载完成";
            //WriteText();
        }
        else
        {
            Debug.LogError("下载模型时出现错误："+model.error);
        }
        //DownLoading = false;
    }

    IEnumerator DoProgress()
    {
        float t = 0f;
        float a = 0f;
        while (startDownLoad)
        {
            t += Time.deltaTime;
            if (t>4.5f)
            {
                t = 4.5f;
            }
            a = t / 5f;
            Progress.size = Mathf.Lerp(Progress.size, a, 0.5f);
            yield return null;
        }
        t = 0f;
        while (t<1f)
        {
            t += Time.deltaTime;
            Progress.size = Mathf.Lerp(Progress.size, 1f, 0.05f);
            yield return null;
        }
        Progress.size = 1f;
        //ButtonText.text = "下载完成";
        yield return new WaitForSeconds(0.1f);
        Progress.gameObject.SetActive(false);
        WriteText();
        ShopManager.Instance.AddIDName(shopData.ModelName_EN, shopData.ModelID);
        DownLoading = false;
    }

    /// <summary>
    /// 写文件
    /// </summary>
    public void WriteText()
    {
        Debug.Log("开始写文件");
        //读取文件
        string jsonstr = File.ReadAllText(Application.streamingAssetsPath + "/" +shopData.ModelTypeName_EN+ ".txt",Encoding.GetEncoding("Unicode"));
        JsonData charactersJd = JsonMapper.ToObject(jsonstr);
        JsonData character = charactersJd[shopData.ModelTypeName_EN];
        JsonData xxx = new JsonData();
        xxx["name"] = shopData.ModelName_EN;
        xxx["modelpath"] = "AssetBundles/" + shopData.ModelTypeName_EN + "/" + shopData.ModelName_EN + ".3dpro";
        xxx["name2"] = shopData.ModelName_CH;
        character.ValueAsArray().Add(xxx);
        charactersJd[shopData.ModelTypeName_EN] = character;
        jsonstr = JsonMapper.ToJson(charactersJd);
        jsonstr= Encoding.Unicode.GetString(Encoding.Unicode.GetBytes(jsonstr));
        File.WriteAllText(Application.streamingAssetsPath + "/"+ shopData.ModelTypeName_EN + ".txt", jsonstr, Encoding.GetEncoding("Unicode"));
        
        Debug.Log("写入完成");
        //将已下载的商店物品的id记录下来
        ShopManager.Instance.CommitDownId(shopData);
        Manager.Instace.RefershResourcePanel();
    }

    public void SetDownLoaded(bool isdown)
    {
        if (isdown)
        {
            DownLoadButton.gameObject.SetActive(false);
            //Progress.size = 1;
            //ButtonText.text = "已下载";
            StartCoroutine(DownLocalIcon(Application.streamingAssetsPath + "/ink2/" + shopData.ModelName_EN + ".png"));
        }
        else
        {
            DownLoadButton.gameObject.SetActive(true);
            //Progress.size = 0;
            //ButtonText.text = "下载";
        }
    }

    IEnumerator DownLocalIcon(string url)
    {
        WWW www = new WWW(@"file://"+url);
        yield return www;
        if (www.error != null)
        {
            Debug.LogError(www.error);
        }
        Texture2D tex2d = www.texture;
        transform.FindChild("icon").GetComponent<Image>().sprite = Sprite.Create(tex2d, new Rect(0, 0, tex2d.width, tex2d.height), new Vector2(0.5f, 0.5f));
        transform.FindChild("icon").gameObject.SetActive(true);
    }
    public void OnMouseEnter()
    {
        transform.FindChild("NameText").gameObject.SetActive(true);
    }

    public void OnMouseExit()
    {
        transform.FindChild("NameText").gameObject.SetActive(false);
    }

    public void OnToggleValueChange(bool tog)
    {
        ModelInformation model = ShopManager.Instance.BulkDownloadList.FirstOrDefault(a => a.ModelID == shopData.ModelID);
        if (tog)
        {
            if (model != null)
            {
                return;
            }
            ShopManager.Instance.BulkDownloadList.Add(shopData);
        }
        else
        {
           
            if (model != null)
            {
                ShopManager.Instance.BulkDownloadList.Remove(model);
            }
            
        }
    }
}