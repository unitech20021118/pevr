using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StatePanelUI : MonoBehaviour {

    public InputField nameInput;
    public InputField description;
    public State<Main> currentState = null;
    private StateNode currentStateNode;

    Image lastImage;
	// Use this for initialization
	void Start () {
        nameInput.onEndEdit.AddListener(NameInputValueChange);
        description.onEndEdit.AddListener(DescriptionValueChange);
	}
	
	// Update is called once per frame
	void Update () {

	}

    /// <summary>
    /// 设置名称、描述
    /// </summary>
    /// <param name="name"></param>
    public void SetName(State<Main> s,StateNode  sn)
    {
        currentState = s;
        currentStateNode = sn;
        nameInput.text = s.name;
        description.text = s.description;
        foreach (ActionUI a in s.actionUIlist)
        {
            //显示当前状态中的动作列表
            a.actionUIObject.transform.parent.gameObject.SetActive(true);
            Manager.Instace._SSST= true;//层级菜单高度判定
        }
        if (lastImage != null)
        {
            lastImage.sprite = (Sprite)Resources.Load("ink2/normal", typeof(Sprite));
            sn.GetComponent<Image>().sprite = (Sprite)Resources.Load("ink2/choosed", typeof(Sprite));
            lastImage = sn.GetComponent<Image>();
        }
        else
        {
            sn.GetComponent<Image>().sprite = (Sprite)Resources.Load("ink2/choosed" , typeof(Sprite));
            
            lastImage = sn.GetComponent<Image>();
        }
    }

    /// <summary>
    /// 编辑状态名称
    /// </summary>
    /// <param name="name"></param>
    void NameInputValueChange(string name)
    {
        if (currentStateNode.GetComponentInChildren<Text>().text != "开始状态")
        {
            currentState.name = name;
            currentStateNode.GetComponent<StateNode>().name = name;
            if (currentStateNode != null)
            {
                string _text = name;
                currentStateNode.text.text = LimitText(_text);
                StateInfo stateInfo = (StateInfo)Manager.Instace.dictFromObjectToInforma[currentStateNode.transform.parent.gameObject];
                stateInfo.name = name;
            }
        }
        else
        {
            nameInput.text = "";
        }

        
    }
    /// <summary>
    /// 限制 状态名称字数，当过长的时候以...结尾；
    /// </summary>
    string  LimitText(string text)
    {
        Font font = Font.CreateDynamicFontFromOSFont("Arial", 16);
        font.RequestCharactersInTexture(text, 10, 0);
        CharacterInfo characterInfo;
        float width = 0;
        for (int i = 0; i < text.Length; i++)
        {
            font.GetCharacterInfo(text[i], out characterInfo, 10);
            width += characterInfo.advance;
        }
        if (width > 64)
        {
            while (width> 48)
            {
                text = text.Substring(0, text.Length - 1);
                CharacterInfo characterInfo2;
                float width2 = 0f;
                for (int i = 0; i < text.Length; i++)
                {
                    font.GetCharacterInfo(text[i], out characterInfo2, 10);
                    width2 += characterInfo2.advance;
                }
                width = width2;
            }
            text = text + "...";
        }
        return text;
    }
    /// <summary>
    /// 编辑状态描述
    /// </summary>
    /// <param name="des"></param>
    void DescriptionValueChange(string des)
    {
        currentState.description = des;
        currentStateNode.GetComponent<StateNode>().description = des;
    }
    /// <summary>
    /// 创建动作
    /// </summary>
    //public void CreateAction()
    //{
    //    ChangeColor colorAction = new ChangeColor();
    //    colorAction.even = currentState.eventList[0];//给动作产生的事件赋值
    //    currentState.actionList.Add(colorAction);
    //}

    /// <summary>
    ///显示动作列表组件
    /// </summary>
    public void ShowActionListPanel()
    {
        if (currentState != null)
        {
            Manager.Instace.ActionList.SetActive(true);
            Manager.Instace.actionList.currentState = currentState;
        }

    }

    /// <summary>
    /// 使当前状态编辑面板中的actionlist不可见
    /// </summary>
    public void UnableCurrentActionlist()
    {
        foreach (ActionUI a in currentState.actionUIlist)
        {
            a.actionUIObject.transform.parent.gameObject.SetActive(false);
            Manager.Instace._SSST = true;
        }
    }
}
