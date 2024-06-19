using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpringGUI;
using UnityEngine.UI;
public class ActionUI : MonoBehaviour, WindowInterface
{

    public Action<Main> action = null;
    public GameObject actionUIObject;
    public ActionInforma actionInforma;
    protected float duringtime;
    public InputField timeInputField;
    /// <summary>
    /// 该动作所附着的物体
    /// </summary>
    public Transform ActionTargetTransform;
    public void SetAction(Action<Main> a)
    {
        action = a;
    }

    public virtual Action<Main> CreateAction()
    {
        Manager.Instace.actionInfomaToGameobject.Add(actionInforma, this.gameObject);
        if (action == null)
        {
            action = new Action<Main>();
        }
        return action;
    }

    public virtual Action<Main> LoadAction(ActionInforma actionInforma)
    {
        Manager.Instace.actionInfomaToGameobject.Add(actionInforma, this.gameObject);
        return action;
    }
    /// <summary>
    /// 设置物体对象
    /// </summary>
    public void SetObject(GameObject obj)
    {
        actionUIObject = obj;
    }

    /// <summary>
    /// 设置颜色
    /// </summary>
    //public void SetColor()
    //{
    //    Manager.Instace.ColorPicker.SetActive(true);
    //    ChangeColor cc = (ChangeColor)action; 
    //    //Manager.Instace.gonggong.GetComponent<MeshRenderer>().material.color = Color.red;
    //    Manager.Instace.ColorPicker.GetComponent<ColorPickerUI>().SetCurrentColor(cc);

    //}


    public void SetActive()
    {
        //Manager.Instace.isChooseEvent = true;

        Manager.Instace.ChooseEventPanel.SetActive(true);
        Manager.Instace.ChooseEventPanel.GetComponent<CurrentEditorActon>().SetEditorAction(gameObject);
    }

    /// <summary>
    /// 获得状态信息
    /// </summary>
    /// <returns></returns>
    public StateInfo GetStateInfo()
    {
        GameObject obj = StateNode.dict[Manager.Instace.ActionList.GetComponent<ActionList>().currentState];
        // int num = obj.transform.parent.GetComponent<GameObjectIndex>().index;
        StateInfo stateInfo = (StateInfo)Manager.Instace.dictFromObjectToInforma[obj.transform.parent.gameObject];
        return stateInfo;
    }

    public virtual void Close()
    {

        State<Main> state = Manager.Instace.ActionList.GetComponent<ActionList>().currentState;
        state.actionUIlist.Remove(this);
        if (action.isOnce)
        {
            state.onceActionList.Remove(action);
        }
        else
        {
            state.updateActionList.Remove(action);
        }
        
        Destroy(gameObject);
        GetStateInfo().actionList.Remove(actionInforma);
        Manager.Instace.actionInfomaToGameobject.Remove(actionInforma);
    }
}
