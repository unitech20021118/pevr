using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
[System.Serializable]
public class LineInfo : Base{

    public string beginPos;
    public string endPos;
    public StateInfo stateInfo;

    public LineInfo(StateInfo state)
    {
        id++;
        index = id;
        stateInfo = state;
    }

    public override void GetTransform()
    {
        Transform t = Manager.Instace.dictFromObjectToInforma.FirstOrDefault(q => q.Value == this).Key.transform;
        LineRenderer lr = t.GetComponent<LineRenderer>();
        beginPos= lr.GetPosition(0).ToString();
        endPos = lr.GetPosition(1).ToString();
        //lr.startWidth = 0.1f;
        //lr.endWidth = 0.1f;
        //lr.material.color = Color.black;
        lr.startWidth = 1.5f;
        lr.endWidth = 1.5f;
        lr.material.color = Color.white;
    }
}
