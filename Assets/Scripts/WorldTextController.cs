using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTextController : MonoBehaviour {
    
    /// <summary>
    /// 主相机
    /// </summary>
    public GameObject main_camera;
    /// <summary>
    /// 第一人称相机
    /// </summary>
    public GameObject FPS_camera;
    /// <summary>
    /// vr相机
    /// </summary>
    public GameObject vr_camera;
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        //当主相机激活时使文本面相主相机
        if (main_camera.activeInHierarchy == true && FPS_camera.gameObject.activeInHierarchy == false && vr_camera.activeInHierarchy == false)
        {
            SetRotate(main_camera.gameObject);
        }//当pc第一人称相机激活时使文本面向第一人称
        else if (main_camera.activeInHierarchy == false && FPS_camera.activeInHierarchy == true && vr_camera.activeInHierarchy == false)
        {
            SetRotate(FPS_camera);
        }//当vr相机激活时使文本面向vr相机
        else if (main_camera.activeInHierarchy == true && FPS_camera.activeInHierarchy == false && vr_camera.activeInHierarchy == true)
        {
            SetRotate(vr_camera);
        }
	}

    public void SetRotate(GameObject obj)
    {
        transform.LookAt(obj.transform);
        
    }
}
