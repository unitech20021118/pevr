using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShowImage : Action<Main> {
    GameObject image;
    public override void DoAction(Main m)
    {
        //if (Manager.Instace.ShowImage == null)
        //{
        //    Manager.Instace.ShowImage = (GameObject)Resources.Load("Prefabs/ShowImage");
        //}
        //显示图片
        if (image != null)
        {
            image.SetActive(true);
        }
        
    }

    public void SetImage(GameObject obj)
    {
        image = obj;
    }
}
