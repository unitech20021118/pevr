using System.Collections.Generic;
using UnityEngine;
using System.Text;
using DG.Tweening;

public class ChangeColor : Action<Main> {
	public string targetName;
	public GameObject target;
    public Color color;
    public List<Color> colorList = new List<Color>();
	public float time;

    public override void DoAction(Main m)
    {
        if (Manager.Instace.clientSocket != null)
        {
            string s = "改变颜色";
            Manager.Instace.clientSocket.Send(Encoding.UTF8.GetBytes(s));
        }
        

		if (target == null)
        {
			if (string.IsNullOrEmpty (targetName))
            {
				target = m.gameObject;
			} else
            {
				target = GameObject.Find ("Parent/" + targetName);
			}
		}

        if (color != null)
        {
            
			GetColor(target.transform);
        }
       
        
        
    }

    public void GetColor(Transform t)
    {
        if (t.childCount != 0)
        {
            foreach (Transform i in t)
            {
                GetColor(i);
            }
        }
        if (t.GetComponent<Renderer>())
        {
			Material[] mats = t.GetComponent<Renderer> ().materials;
			foreach (Material item in mats) {
				item.DOColor (color, time);
			}
			//t.GetComponent<Renderer>().material.color = color;

        }
    }

    public ChangeColor()
    {
        SetSituation();
    }

    public override void SetSituation()
    {
        //GetAllColor(Manager.Instace.gonggong.transform);
        //color=Manager.Instace.gonggong.transform.GetChild(0).GetComponent<Renderer>().material.color;
        //color = colorList[0];
        base.SetSituation();
    }

    public ChangeColor(string c)
    {

        if (!string.IsNullOrEmpty(c))
        {
            color = Manager.Instace.GetColor(c);
        }
        isOnce = true;

        
    }



    //public void GetAllColor(Transform t)
    //{
    //    if (t.childCount != 0)
    //    {
    //        foreach (Transform trans in t)
    //        {
    //            GetAllColor(trans);
    //        }
    //    }
    //    else
    //    {
    //        if (t.GetComponent<Renderer>() != null)
    //        {
    //            colorList.Add(t.GetComponent<Renderer>().material.color);
    //        }
    //    }

    //}
    

}
