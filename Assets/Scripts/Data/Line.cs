using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Line : MonoBehaviour {

    public int id;
    public string name;
    public LineRenderer lr;
    public EventNode eventnode;
    public StateNode statenode;
    public Events events;
    public RectTransform eventRecstyle;
    GameObject _arrow;
    Vector3 _nomal;
    Vector3 _point;
    int _numi=1;
    int _numj=1;
    float derta;
	// Use this for initialization
    private void Start()
    {
        InitLine();
    }

    public void SetEvents(Events e)
    {
        events = e;
    }
    /// <summary>
    /// 初始化线段
    /// </summary>
    private void InitLine()
    {
        id++;
        name = "line" + id;
        transform.name = name;
        lr = transform.GetComponent<LineRenderer>();
        lr.startWidth = 1.5f;
        lr.endWidth = 1.5f;
        lr.material.color = Color.white;
        //lr.material.SetColor("_EmissionColor", Color.white);
        _arrow = Instantiate(Manager.Instace.LineArrow);
        _arrow.transform.parent = this.transform;
     
    }
	
	// Update is called once per frame
    void Update()
    {
        if (statenode != null) 
        { 
        JudgePoint();
        Lineline(_numi, _numj);
        }
    }
    void JudgePoint()
    {
        float distanMin = 100000;
        List<float> distanIJs = new List<float> { };
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                float distanIJ = (statenode.transform.GetChild(i + 1).transform.position - eventnode.transform.GetChild(j + 1).transform.position).magnitude;
                distanIJs.Add(distanIJ);
            }
        }
        for (int i = 0; i < distanIJs.Count; i++)
        {
            if (distanIJs[i] < distanMin)
            {
                distanMin = distanIJs[i];
                _numi = (int)(i/4)+1;
                _numj = i%4+1;
            }
        }

    }
    void Lineline(int num1,int num2)
    {
        _nomal = statenode.transform.GetChild(num1).transform.position - eventnode.transform.GetChild(num2).position;
        if (eventnode != null && statenode != null)
        {
            lr.SetPosition(0, eventnode.transform.GetChild(num2).position);
            lr.SetPosition(1, statenode.transform.GetChild(num1).transform.position);
            _arrow.transform.position = statenode.transform.GetChild(num1).transform.position;
            _arrow.transform.right = _nomal.normalized;
            //AnchorCenterLine();
        }
    }
    //public void AnchorCenterLine()
    //{
    //    eventRecstyle = eventnode.gameObject.transform.FindChild("Text").GetComponent<RectTransform>();
    //    derta = eventnode.transform.position.x - statenode.transform.position.x;
    //    if (derta < -1.5f)
    //    {
    //        eventRecstyle.anchorMin = new Vector2(1, 0.5f);
    //        eventRecstyle.anchorMax = new Vector2(1, 0.5f);
    //    }
    //    if (derta > 1.5f)
    //    {
    //        eventRecstyle.anchorMin = new Vector2(0, 0.5f);
    //        eventRecstyle.anchorMax = new Vector2(0, 0.5f);
    //    }
    //    if (derta < 1.5f && derta > -1.5f)
    //    {
    //        eventRecstyle.anchorMin = new Vector2(0.5f, 0);
    //        eventRecstyle.anchorMax = new Vector2(0.5f, 0);
    //    }
    //}

    //void OnDestroy()
    //{
    //    if (statenode != null)
    //    {
    //        events.Cancel();
    //        Base data = Base.FindData(id);
    //        ListTreeNode<Base> parent = Manager.Instace.listTree.GetNode(data).parent;
    //        Manager.Instace.listTree.DeleteNode(parent, data);
    //        Destroy(gameObject);
    //    }

    //}
}

