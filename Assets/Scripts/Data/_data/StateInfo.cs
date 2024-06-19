using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
[System.Serializable]
public class StateInfo :FSMInfo {

    public string Description;
    public bool IsStart;
    public List<ActionInforma> actionList=new List<ActionInforma>();
    public LineInfo lineInfo;
    /// <summary>
    /// 这个状态机附着的目标物体在Parent下的路径
    /// </summary>
    //public string StateTargetGameObjectPath;
    //public List<ActionInforma> alwaysActionList;
    //public State<Main> state;
    //public StateInfo(State<Main> s)
    //{
    //    state = s;
        
    //}
    public StateInfo(bool isSt)
    {
        this.name = "开始状态";
        id++;
        index = id;
        IsStart = true;
        lineInfo = null;
    }

    public StateInfo()
    {
        this.name = "新状态";
        id++;
        index = id;
        IsStart = false;
        lineInfo = null;
    }

    public override void GetTransform()
    {
        //存在问题
        //Transform t = GameObjectIndex.GameObjectList[index].transform;
        Transform t = Manager.Instace.dictFromObjectToInforma.FirstOrDefault(q => q.Value == this).Key.transform;
        pos = t.localPosition.ToString();
        scale = t.localScale.ToString();
        rotate = t.eulerAngles.ToString();
        t.GetChild(0).GetComponent<StateNode>().name=name;
        Description = t.GetChild(0).GetComponent<StateNode>().description;
        isActive = t.gameObject.activeSelf;
    }
}
