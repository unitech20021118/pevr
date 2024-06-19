using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICtl : MonoBehaviour {
    
    G_CreateObject gc;
    GameObject obj;
    Vector3 targetPos = Vector3.zero;
    Vector3 offset;
    public GameObject cube;
    public GameObject parent;
    public Camera main;
    bool canm = true;

    void Start()
    {
        //sec=gameObject.GetComponent<SE_CreateObject>();
        gc=gameObject.GetComponent<G_CreateObject>();
        //SE_BaseFunction.SetMainCamera(main);
    }
    void Update()
    {
        if(obj!=null){
            //obj.transform.position = Vector3.Lerp(obj.transform.position, targetPos, 0.2f);
        }
        
    }

    public void LoadModel()
    {
        string path = IOHelper.GetModelFileName();
        obj = DataHelper.LoadModel(path);
        //obj.transform.position = new Vector3(15, 0, 0);
        foreach (Transform i in obj.transform)
        {
            i.gameObject.layer = G_PubDef.quiescentObject;
        }
        obj.layer = G_PubDef.quiescentObject;
        //obj.AddComponent<DragItem>();

    }

    //public void LoadModel()
    //{
    //    //string path = IOHelper.GetModelFileName();
    //    //obj = DataHelper.LoadModel(path);
    //    //obj.transform.position = new Vector3(15, 0, 0);
    //    //foreach (Transform i in obj.transform)
    //    //{
    //    //    i.gameObject.layer = G_PubDef.quiescentObject;
    //    //}
    //    //obj.layer = G_PubDef.quiescentObject;
    //    //obj.AddComponent<DragItem>();

    //}

    public void Click()
    {
       // sec.SetChooseObject(cube, parent.transform, canm);
        //gc.CreateGameObject(cube, canm);
    }


   

}
