using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShowImageUI : ActionUI {
    public GameObject ShowImageTarget;
    public Image ImageSelected;
    public InputField left;
    public InputField top;
    public InputField width;
    public InputField height;
    public Button SelectBtn;
    public Sprite sprite;
    public string imagePath;
    public GameObject ShowButtonTarget;
    ShowImage si;
    Vector3 temp;
    protected ShowPCButtonInforma showPCButtonInforma;
	// Use this for initialization
    //public virtual void Start()
    //{
    //    si = (ShowImage)action;
        
    //    if (Manager.Instace.ShowImage == null)
    //    {
    //        Manager.Instace.ShowImage = (GameObject)Resources.Load("Prefabs/ImagePrefab");

    //    }
    //    Init();
    //    ControlSize(ShowImageTarget);
    //}

    public void ControlSize(GameObject obj)
    {
        left.onValueChanged.AddListener(delegate(string str) { LeftChanged(obj); });
        top.onValueChanged.AddListener(delegate(string str) { TopChanged(obj); });
        width.onValueChanged.AddListener(delegate(string str) { WidthChanged(obj); });
        height.onValueChanged.AddListener(delegate(string str) { HeightChanged(obj); });
        //SelectBtn.onClick.AddListener(delegate() { SetChooseImagePanelActive(this); });
        SelectBtn.onClick.AddListener(delegate() { ChooseImage(); });
    }

    //public virtual void Init()
    //{
    //    left.text = "0";
    //    top.text = "0";
    //    width.text = "1";
    //    height.text = "1";
        
    //    ShowImageTarget = Instantiate(Manager.Instace.ShowImage);
        
    //    ShowImageTarget.transform.SetParent(Manager.Instace.transform);
    //    ShowImageTarget.transform.position = ShowImageTarget.transform.parent.position;
    //    si.SetImage(ShowImageTarget);
    //    ShowImageTarget.SetActive(false);
    //    //ShowImageTarget.GetComponent<RectTransform>().sizeDelta = ShowImageTarget.transform.parent.GetComponent<RectTransform>().sizeDelta;
    //}

    void SetChooseImagePanelActive(ShowImageUI actionUI)
    {
        Manager.Instace.ChooseImagePanel.SetActive(true);
        Manager.Instace.ChooseImagePanel.GetComponent<ImageListUI>().SetShowImageUI(actionUI);
    }

    void ChooseImage()
    {
        imagePath = IOHelper.GetImageName();
        StartCoroutine( LoadImage("file://" + imagePath));
        showPCButtonInforma.imagePath = imagePath;


    }

    protected IEnumerator LoadImage(string url)
    {
        WWW www = new WWW(url);
        yield return www;
        if (www != null && string.IsNullOrEmpty(www.error))
        {
            Texture2D texture = www.texture;
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            ImageSelected.sprite = sprite;
            ShowButtonTarget.GetComponent<Image>().sprite = sprite;
        }
    }

    void LeftChanged( GameObject obj)
    {
        temp = obj.transform.position;
        if (left.text == string.Empty)
        {
            left.text = "0";
            obj.transform.position = new Vector3(0f, temp.y, temp.z);
            showPCButtonInforma.left = 0;
        }
        else
        {
            float x = float.Parse(left.text) * 1920;
            obj.transform.position = new Vector3(x, temp.y, temp.z);
            showPCButtonInforma.left = float.Parse(left.text);
        }

    }

    void TopChanged(GameObject obj)
    {
        temp = obj.transform.position;
        if (top.text == string.Empty)
        {
            top.text = "0";
            obj.transform.position = new Vector3(temp.x, 0, temp.z);
            showPCButtonInforma.up = 0;
        }
        else
        {
            float y = float.Parse(top.text) * 1280;
            obj.transform.position = new Vector3(temp.x, y, temp.z);
            showPCButtonInforma.up = float.Parse(top.text);
        }

    }

    void WidthChanged(GameObject obj)
    {
        temp = obj.transform.position;
        Vector2 vector2 = obj.GetComponent<RectTransform>().sizeDelta;
        if (width.text == string.Empty)
        {
            width.text = "0";
            obj.GetComponent<RectTransform>().sizeDelta = new Vector2(0, vector2.y);
            showPCButtonInforma.width = 0;
        }
        else
        {
            float wid = float.Parse(width.text) * 1920;
            obj.GetComponent<RectTransform>().sizeDelta = new Vector2(wid, vector2.y);
            showPCButtonInforma.width = float.Parse(width.text);
        }

    }

    void HeightChanged(GameObject obj)
    {
        temp = obj.transform.position;
        Vector2 vector2 = obj.GetComponent<RectTransform>().sizeDelta;
        if (height.text == string.Empty)
        {
            height.text = "0";
            obj.GetComponent<RectTransform>().sizeDelta = new Vector2(vector2.x, 0f);
            showPCButtonInforma.height = 0;
        }
        float hei = float.Parse(height.text) * 1280;
        showPCButtonInforma.height = float.Parse(height.text);
        obj.GetComponent<RectTransform>().sizeDelta = new Vector2(vector2.x, hei);
    }

    public override Action<Main> CreateAction()
    {
        action = new ShowImage();

        return base.CreateAction();
    }

    public virtual void  GetSprite(Sprite sprite)
    {
        ShowImageTarget.GetComponent<Image>().sprite = sprite;
    }


}
