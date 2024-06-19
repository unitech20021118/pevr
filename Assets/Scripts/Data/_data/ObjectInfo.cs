using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ObjectInfo :Base {

    public string modelPath;

    public string imgPath;
    
    public bool IsScene;

    public bool hasRigid;

    public bool hasCollider;
  
    public ObjectInfo()
    {

    }

    public ObjectInfo(string modelP,string imgpath,Transform trans,string name,bool isccence)
    {
        modelPath = modelP;
        imgPath = imgpath;
        //pos = transform.localPosition;
        //scale = transform.localScale;
        //rotate = transform.eulerAngles;
        //transform = trans;
        this.name = name;
        IsScene = isccence;
        id++;
        index = id;
        
    }


}
