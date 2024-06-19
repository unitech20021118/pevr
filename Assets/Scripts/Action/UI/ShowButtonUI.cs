using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
public class ShowButtonUI :ShowImageUI{
    ShowButton showButton;
    //ShowPCButtonInforma showPCButtonInforma;
    /// <summary>
    /// 显示事件名的文本框
    /// </summary>
    public Text eventText;
    /// <summary>
    /// 预览按钮
    /// </summary>
    public Button PreviewBtn;
    /// <summary>
    /// 在按钮上显示文本的文本输入框
    /// </summary>
    public InputField buttonText;
    //实例化按钮
    public  void Init(ShowPCButtonInforma spc)
    {
        left.text = spc.left.ToString();
        top.text = spc.up.ToString();
        width.text = spc.width.ToString();
        height.text = spc.height.ToString();

        PreviewBtn.onClick.AddListener(PreviewButton);
       ShowButtonTarget = Instantiate(Manager.Instace.ShowButton);
       if (spc.imagePath != null)
       {
           FileStream fileStream = new FileStream(spc.imagePath, FileMode.Open, FileAccess.Read);
           fileStream.Seek(0, SeekOrigin.Begin);
           byte[] bytes = new byte[fileStream.Length];
           fileStream.Read(bytes, 0, (int)fileStream.Length);
           fileStream.Close();
           fileStream.Dispose();
           fileStream = null;

           Texture2D texture = new Texture2D(800, 640);
           texture.LoadImage(bytes);
           Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
           ImageSelected.sprite = sprite;
           ShowButtonTarget.GetComponent<Image>().sprite = sprite;
           
       }
       ShowButtonTarget.transform.SetParent(Manager.Instace.transform.Find("PCBtnParent"));
       //ShowButtonTarget.transform.position = ShowButtonTarget.transform.parent.position;
       SetShowButtonTargetPos(spc);
       showButton.SetButton(ShowButtonTarget);

       ShowButtonTarget.SetActive(false);
    }

    void PreviewButton()
    {
        if (!ShowButtonTarget.activeSelf)
        {
            ShowButtonTarget.SetActive(true);
            ShowButtonTarget.GetComponent<Button>().enabled = false;
        }
        else
        {
            ShowButtonTarget.SetActive(false);
            ShowButtonTarget.GetComponent<Button>().enabled = true;
        }

    }

    public void SetShowButtonTargetPos(ShowPCButtonInforma spc)
    {
        float x = spc.left * 1920;
        float y = spc.up * 1280;
        ShowButtonTarget.transform.position = new Vector3(x, y, 0);
        float wid = spc.width * 1920;
        float heigh = spc.height * 1280;
        ShowButtonTarget.GetComponent<RectTransform>().sizeDelta = new Vector2(wid, heigh);
    }

    public override Action<Main> CreateAction()
    {
        action = new ShowButton();
        action.SetSituation();
        showButton = (ShowButton)action;
        showPCButtonInforma = new ShowPCButtonInforma(true);
        actionInforma = showPCButtonInforma;
        showPCButtonInforma.name = "ShowButton";
        GetStateInfo().actionList.Add(showPCButtonInforma);
        showButton.ParentTransform = GameObject.Find("Canvas/PCBtnParent").transform;
        if (Manager.Instace.ShowButton == null)
        {
            Manager.Instace.ShowButton = (GameObject)Resources.Load("Prefabs/ButtonPrefab");
        }
        Init(showPCButtonInforma);
        ControlSize(ShowButtonTarget);
        return action;
    }

    public override Action<Main> LoadAction(ActionInforma actionInforma)
    {
        showPCButtonInforma = (ShowPCButtonInforma)actionInforma;
        this.actionInforma = actionInforma;
        action = new ShowButton();
        showButton = (ShowButton)action;
        if (Manager.Instace.ShowButton == null)
        {
            Manager.Instace.ShowButton = (GameObject)Resources.Load("Prefabs/ButtonPrefab");
        }
        Init(showPCButtonInforma);
        showButton.button.GetComponentInChildren<Text>().text = showPCButtonInforma.buttonText;
        showButton.ParentTransform = GameObject.Find("Canvas/PCBtnParent").transform;
        buttonText.text = showPCButtonInforma.buttonText;
        ControlSize(ShowButtonTarget);
        foreach (Events e in Manager.Instace.eventlist)
        {
            if (e.name == showPCButtonInforma.eventName)
            {
                showButton.even = e;
                eventText.text = e.name;
            }
        }
        return base.LoadAction(actionInforma);
    }

    public override void  GetSprite(Sprite sprite)
    {
        ShowButtonTarget.GetComponent<Image>().sprite = sprite;
    }

    public void SetBtnText(InputField inputField)
    {
        showButton.button.GetComponentInChildren<Text>().text = inputField.text;
        showPCButtonInforma.buttonText = inputField.text;
    }

    void OnDestory()
    {
        ShowButtonTarget.SetActive(false);
        ShowButtonTarget.GetComponent<Button>().enabled = true;
    }
}
