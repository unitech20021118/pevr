using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    public List<MessageAttribute> MessageAttributes = new List<MessageAttribute>();

    public List<ImageAttribute> ImageAttributes = new List<ImageAttribute>();

    void Awake()
    {
        Instance = this;
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void SetImage(string imagepath, Image image)
    {
        StartCoroutine(GetImgRes(imagepath, image));
    }
    /// <summary>
    /// 获取图片
    /// </summary>
    /// <param name="path"></param>
    /// <param name="targetImg"></param>
    /// <returns></returns>
    IEnumerator GetImgRes(string path, Image targetImg)
    {
        WWW www = new WWW(@"file://" + path);
        yield return www;
        Texture2D tex2d = www.texture;
        targetImg.sprite = Sprite.Create(tex2d, new Rect(0, 0, tex2d.width, tex2d.height), Vector2.zero, 100, 1, SpriteMeshType.Tight);
    }
}









/// <summary>
/// 显示文本信息的属性
/// </summary>
public class MessageAttribute
{
    /// <summary>
    /// 显示的文本信息主要物体
    /// </summary>
    public GameObject MessageGameObject;
    /// <summary>
    /// 是否允许被覆盖
    /// </summary>
    public bool IsCover;

    public MessageAttribute() { }

    public MessageAttribute(GameObject messageGameObject, string message, Font font, FontStyle fontStyle, int fontSize,
        string msgBgImagePath, Vector3 positionVector3,bool isCover)
    {
        MessageGameObject = messageGameObject;
        IsCover = isCover;
        MessageGameObject.transform.GetChild(0).GetComponent<Text>().text = message;
        MessageGameObject.transform.GetChild(0).GetComponent<Text>().font = font;
        MessageGameObject.transform.GetChild(0).GetComponent<Text>().fontSize = fontSize;
        MessageGameObject.transform.GetChild(0).GetComponent<Text>().fontStyle = fontStyle;

        MessageGameObject.transform.localPosition = positionVector3;
        if (!string.IsNullOrEmpty(msgBgImagePath))
        {
            UIController.Instance.SetImage(msgBgImagePath, MessageGameObject.GetComponent<Image>());
        }
    }
}


public class ImageAttribute
{
    /// <summary>
    /// 显示图片的主要主体
    /// </summary>
    public GameObject ImageGameObject;
    /// <summary>
    /// 是否是作为底图
    /// </summary>
    public bool IsBG;


    public ImageAttribute() { }

    public ImageAttribute(GameObject imageGameObject,string imagePath,int ajustMode, Vector3 positionVector3,bool isBg)
    {
        ImageGameObject = imageGameObject;
        IsBG = isBg;
        
        ImageGameObject.transform.localPosition = positionVector3;
        if (!string.IsNullOrEmpty(imagePath))
        {
            UIController.Instance.SetImage(imagePath, ImageGameObject.GetComponent<Image>());
        }
    }
}
