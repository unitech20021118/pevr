using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddLightReources : Action<Main>{
	public GameObject target;
	public string targetName;
    public float z=3, x=1f;
    public Color lightColor=Color.white;
    public GameObject light;
    public string gstyle;
    public Vector3 LightLocalPosition;
    public override void DoAction(Main m)
    {
		if (target == null) {
			if (string.IsNullOrEmpty (targetName)) {
				target = m.gameObject;
			} else {
				target = GameObject.Find ("Parent/" + targetName);
			}
		}

        light = new GameObject(gstyle + "light");
        Debug.Log("设置灯光");
        gstyle = null;
        light.AddComponent<Light>();
        //edit by kuai
		//light.transform.position = target.transform.position;
        light.transform.parent = target.transform;
        light.transform.localPosition = LightLocalPosition;
        Light lg;
        lg = light.GetComponent<Light>();
        lg.range = 7;
        lg.shadows = LightShadows.Soft;
        lg.intensity = z;
        lg.shadowStrength = x;
        lg.color = lightColor;
    }
    public AddLightReources()
    {
        SetSituation();
    }


    public AddLightReources(string c,string z1,string x1)
    {

        if (!string.IsNullOrEmpty(c))
        {
            lightColor = Manager.Instace.GetColor(c);
        }
        z=float.Parse(z1);
        x=float.Parse(x1);
        isOnce = true;
    }
    
}

    
