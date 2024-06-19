using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// edit by kuai
/// </summary>
public class WorldTextUI : ActionUI
{
    /// <summary>
    /// 控制开启关闭的toggle
    /// </summary>
    public Toggle CloseToggle;
    /// <summary>
    /// 文本输入框
    /// </summary>
    public InputField message_IF;
    /// <summary>
    /// 大小的文本输入框
    /// </summary>
    public InputField SizeInputField;
    /// <summary>
    /// 预览位置的按钮
    /// </summary>
    public Button show_Btn;
    /// <summary>
    /// 用于预览的世界画布
    /// </summary>
    public Transform WorldCanvas;
    /// <summary>
    /// 文本大小
    /// </summary>
    private float size = 1f;
    /// <summary>
    /// 预览时的目标物体
    /// </summary>
    private Transform targetTransform;
    WorldText worldText;
    WorldTextImforma worldTextInforma;


    public override Action<Main> CreateAction()
    {
        action = new WorldText();
        action.isOnce = true;
        actionInforma = new WorldTextImforma(true);
        worldTextInforma = (WorldTextImforma) actionInforma;
        worldText = (WorldText)action;
        
        GetStateInfo().actionList.Add(actionInforma);
        actionInforma.name = "WorldText";
        return base.CreateAction();
    }

    public override Action<Main> LoadAction(ActionInforma actionInforma)
    {
        worldTextInforma = (WorldTextImforma)actionInforma;
        message_IF.text = worldTextInforma.message;
        action = new WorldText();
        worldText = (WorldText)action;

        worldText.close = worldTextInforma.close;
        worldText.message = worldTextInforma.message;
        worldText.size = worldTextInforma.size;
        SizeInputField.text = worldTextInforma.size.ToString();
        size = worldTextInforma.size;
        //Debug.LogError(worldTextInforma.size);
        return base.LoadAction(actionInforma);
    }


    public void UpdateInput(int a)
    {
        if (worldText == null)
        {
            worldText = (WorldText)action;
        }
        if (worldTextInforma==null)
        {
            worldTextInforma = (WorldTextImforma)actionInforma;
        }
        switch (a)
        {
            case 1://调整大小的文本输入框
                size = float.Parse(SizeInputField.text);
                WorldCanvas = GameObject.Find("WorldCanvas").transform;
                WorldCanvas.GetChild(0).localScale = new Vector3(0.01f * size, 0.01f * size, 1);

                worldText.size = size;
                worldTextInforma.size = size;
                //Debug.LogError(worldText.size);
                break;
            case 2://提示文本的输入框
                worldText.message = message_IF.text;
                worldTextInforma.message = message_IF.text;
                break;
            default:
                break;
        }
        
    }
    /// <summary>
    /// 预览按钮的点击
    /// </summary>
    public void BtnShowClick()
    {
        WorldCanvas = GameObject.Find("WorldCanvas").transform;
        if (WorldCanvas)
        {//Debug.LogError("111");
            if (Manager.Instace.gameObject.GetComponent<G_EditorTarget>().moveTarget != null)
            {
                targetTransform = Manager.Instace.gameObject.GetComponent<G_EditorTarget>().moveTarget.transform;
            }

            if (targetTransform != null)
            {
                //Debug.LogError("222");
                if (targetTransform.position != WorldCanvas.position)
                {
                    WorldCanvas.position = targetTransform.position;
                    WorldCanvas.GetChild(0).localScale = new Vector3(0.01f * size, 0.01f * size, 1);
                    WorldCanvas.GetChild(0).gameObject.SetActive(true);

                    return;
                }

                if (WorldCanvas.GetChild(0).gameObject.activeSelf)
                {
                    WorldCanvas.GetChild(0).gameObject.SetActive(false);
                }
                else
                {
                    WorldCanvas.GetChild(0).gameObject.SetActive(true);
                }
            }
            else
            {
                Debug.LogError("在进行预览前请先选中承载该动作的物体！！！");
            }
        }
    }
    /// <summary>
    /// 当开关的toggle值改变时
    /// </summary>
    public void CloseToggleChanged()
    {
        if (CloseToggle.isOn)
        {
            worldTextInforma.close = true;
            worldText.close = true;
        }
        else
        {
            worldTextInforma.close = false;
            worldText.close = false;
        }
    }
    
}
