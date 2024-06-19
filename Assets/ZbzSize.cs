using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZbzSize : MonoBehaviour {
    double distance;
    //1:9----sczbz=distance/9
    float scZbz;
    Transform zbz, cam;
	// Use this for initialization
	void Start () {
        zbz = this.transform;
        zbz.localScale=new Vector3(1,1,1);
        cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
	}
	
	// Update is called once per frame
	void Update () {
        ZbzScale();
	}
    void ZbzScale()
    {
        distance = (zbz.position - cam.position).magnitude;
        //print(distance);
        if (distance < 1000)
        {
            scZbz = (float)(distance / 9);
            zbz.localScale = new Vector3(scZbz, scZbz, scZbz);
        }     
    }
}
