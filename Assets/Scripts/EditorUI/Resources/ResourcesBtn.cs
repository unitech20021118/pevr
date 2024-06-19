using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ResourcesBtn : MonoBehaviour
{
    public static ResourcesBtn lastBtn;
    public GameObject panel;
    public string ButtonName;
    public string ButtonChineseName;
    Image thisImage;
    void Awake()
    {
        thisImage = this.transform.GetComponent<Image>();
        thisImage.enabled = false;
    }
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(delegate() { OnClick(ButtonName); });
        panel.transform.FindChild("Close").GetComponent<Button>().onClick.RemoveAllListeners();
        panel.transform.FindChild("Close").GetComponent<Button>().onClick.AddListener(Close);
    }

    public void OnClick(string name)
    {
        panel.transform.FindChild("Text").GetComponent<Text>().text = ButtonChineseName;
        Transform trans = panel.GetComponentInChildren<GridLayoutGroup>().transform;
        for (int i = 0; i < trans.childCount; i++)
        {
            Destroy(trans.GetChild(i).gameObject);
        }
        SearchBtn.Instance.Panel.SetActive(false);
        if (lastBtn == null)
        {
            panel.SetActive(true);
            thisImage.enabled = true;
            Manager.Instace.nowLoadGameObject.Clear();
        }
        else
        {
            lastBtn.panel.SetActive(false);
            lastBtn.transform.GetComponent<Image>().enabled = false;
            panel.SetActive(true);
            thisImage.enabled = true;
            Manager.Instace.nowLoadGameObject.Clear();

        }
        //panel.SetActive(true);
        //thisImage.enabled = true;
        //Manager.Instace.nowLoadGameObject.Clear();
        lastBtn = this;
        if (!Manager.Instace.isloadedPic.ContainsKey(name))
        {
            Manager.Instace.StartLoadJson(name, panel);
        }
        else
        {
            Transform t = panel.transform.FindChild("Scroll View/Viewport/Content");
            print(t.name);
            for (int i = 0; i < t.childCount; i++)
            {
                Manager.Instace.nowLoadGameObject.Add(t.GetChild(i).gameObject);
            }
        }
        Manager.Instace.dangqianstr = name;
        Manager.Instace.dangqiangobj = panel;
        //edit by kuai
        panel.transform.GetChild(4).name = ButtonName;
        if (ShopManager.Instance.transform.GetChild(0).gameObject.activeInHierarchy==true)
        {
            Manager.Instace.OpenShopPanel(panel.transform.GetChild(4).GetComponent<Button>());
        }
    }

    public void Close()
    {
        panel.SetActive(false);
        lastBtn.transform.GetComponent<Image>().enabled = false;
    }

    public void InitResBtn(string buttonName,string buttonChineseName,GameObject panel)
    {
        this.ButtonName = buttonName;
        this.ButtonChineseName = buttonChineseName;
        transform.FindChild("Image/iconName/Text").GetComponent<Text>().text = buttonChineseName;
        this.panel = panel;
    }
}
