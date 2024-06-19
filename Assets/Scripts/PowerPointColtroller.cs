using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PowerPointColtroller : MonoBehaviour
{
    /// <summary>
    /// 图片列表
    /// </summary>
    private List<Sprite> pptSpriteList = new List<Sprite>();
    /// <summary>
    ///图片地址列表
    /// </summary>
    private List<string> spritePathList = new List<string>();
    /// <summary>
    /// 文件夹名
    /// </summary>
    private string pptFileName;
    /// <summary>
    /// 是否自动播放
    /// </summary>
    private bool autoPlay;
    /// <summary>
    /// 是否循环播放
    /// </summary>
    private bool loop;
    /// <summary>
    /// 自动播放情况下的间隔时间
    /// </summary>
    private float interval;
    /// <summary>
    /// 用于显示待放映的图片
    /// </summary>
    private Image spriteImage;

    private int count;
    private float T;
    public void InitData(string fileName, bool autoplay, bool loop, float interval)
    {
        pptFileName = fileName;
        this.autoPlay = autoplay;
        this.loop = loop;
        this.interval = interval;
        pptSpriteList.Clear();
        spritePathList.Clear();
        ReadSprites();
    }

    void Awake()
    {
        spriteImage = transform.Find("BG/Canvas/PowerPointImage").gameObject.GetComponent<Image>();
    }
    // Use this for initialization
    void Start()
    {

    }
    /// <summary>
    /// 读取数据
    /// </summary>
    public void ReadSprites()
    {
        Debug.LogError(pptFileName);
        string path = Application.streamingAssetsPath + "/PowerPoint/" + pptFileName;

        DirectoryInfo rootDirectoryInfo = new DirectoryInfo(path);
        foreach (FileInfo file in rootDirectoryInfo.GetFiles())
        {
            if (file.Name.IndexOf(".meta") == -1)
            {
                spritePathList.Add(file.FullName);
            }
        }
        StartCoroutine(LoadSprite(spritePathList));
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.transform.parent == transform)
                {
                    ChangePPt(1);
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(1))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.transform.parent == transform)
                    {
                        ChangePPt(-1);
                    }
                }
            }
        }
    }
    IEnumerator LoadSprite(List<string> spritePath)
    {
        for (int i = 0; i < spritePath.Count; i++)
        {
            //Debug.LogError(Application.streamingAssetsPath + "/PPTTexure/Test/" + spritePath[i]);
            WWW www = new WWW("file:///" + spritePath[i]);
            yield return www;
            if (www.error != null)
            {
                Debug.LogError("加载图片失败：" + www.error);
            }
            else
            {
                Texture2D texture = www.texture;
                pptSpriteList.Add(Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)));
            }
        }
        count = 0;
        T = 0f;
        if (autoPlay)
        {
            StartCoroutine(DoStartAutoPPT());
        }
        else
        {
            ChangePPt(0);
        }

    }

    IEnumerator DoStartAutoPPT()
    {
        spriteImage.sprite = pptSpriteList[0];
        bool change = false;
        while (count <= pptSpriteList.Count)
        {
            T += Time.deltaTime;
            if (T >= interval)
            {
                T = 0f;
                count++;
                if (loop)
                {
                    if (count >= pptSpriteList.Count)
                    {
                        count = 0;
                    }
                }
                change = true;
            }
            if (change)
            {
                spriteImage.sprite = pptSpriteList[count];
            }
            yield return null;
        }
    }

    private void ChangePPt(int add)
    {
        T = 0f;
        count += add;
        if (loop)
        {
            if (count >= pptSpriteList.Count || count < 0)
            {
                count = 0;
            }
        }
        else
        {
            if (count >= pptSpriteList.Count || count < 0)
            {
                return;
            }
        }

        spriteImage.sprite = pptSpriteList[count];

    }
}
