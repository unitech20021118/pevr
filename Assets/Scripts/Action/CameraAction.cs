using UnityEngine;

public class CameraAction : Action<Main>
{
    public bool close;
    /// <summary>
    /// 镜头移动持续时间
    /// </summary>
	public float time=0;
    /// <summary>
    /// 记录镜头实际移动时间
    /// </summary>
    private float T = 0;
	public GameObject target;
	public int type;
	public float px, py, pz, rx, ry, rz;
	public float speed;
	//Camera mainCam;
	//bool isComplete;
	//bool isArrived;
    //private HighlightingRenderer hr;
	public override void DoAction(Main m)
	{
        CameraActionController.Instance.StartCameraAction(m.gameObject,close,type,time,speed,new Vector3(px,py,pz),new Vector3(rx,ry,rz));
	    /*
		if (!isComplete) 
        {
            //if (Manager.Instace.mainCamera.gameObject.activeSelf==false)
            //{
            //    mainCam = GameObject.Find("FPSController").transform.GetChild(0).GetComponent<Camera>();
            //    Debug.LogError(mainCam.gameObject.name);
            //}
            //else { mainCam = Manager.Instace.mainCamera; }
            mainCam = Manager.Instace.mainCamera;
			if (Manager.Instace.FirstPerson.activeSelf) 
            {
                
				//Manager.Instace.FirstPerson.SetActive (false);
                Manager.Instace.FirstPerson.transform.GetChild(0).gameObject.SetActive(false);
                //Manager.Instace.FirstPerson.transform.GetChild(0).gameObject.GetComponent<HighlightingRenderer>().enabled = false;
                //Manager.Instace.DestroyGameobject(Manager.Instace.FirstPerson.transform.GetChild(0).gameObject.GetComponent<HighlightingRenderer>());
                Manager.Instace.FirstPerson.transform.GetChild(1).gameObject.SetActive(true);
                //Manager.Instace.FirstPerson.transform.GetChild(1).gameObject.GetComponent<HighlightingRenderer>().enabled = true;
                Manager.Instace.FirstPerson.transform.GetChild(1).gameObject.AddComponent<HighlightingRenderer>();
                //ExtraCamera.extraCam.gameObject.SetActive (true);
                mainCam = ExtraCamera.extraCam.cam;
			}
			if (Input.GetMouseButtonDown(1)&& Manager.Instace.FirstPerson.transform.GetChild(1).gameObject.activeSelf) 
            {
                Manager.Instace.FirstPerson.transform.GetChild(0).gameObject.SetActive(true);
                Manager.Instace.FirstPerson.transform.GetChild(1).gameObject.SetActive(false);
                Manager.Instace.FirstPerson.transform.GetChild(1).transform.localPosition = Manager.Instace.FirstPerson.transform.GetChild(0).transform.localPosition;
                Manager.Instace.FirstPerson.transform.GetChild(1).transform.localEulerAngles = Manager.Instace.FirstPerson.transform.GetChild(0).transform.localEulerAngles;
				isComplete = true;
			}
			if (type == 0)
            {
                
                //mainCam.transform.DOMove (new Vector3 (px, py, pz) + mainCam.transform.position, time);
				mainCam.transform.DOMove (new Vector3 (px, py, pz) + m.gameObject.transform.position, time);
				mainCam.transform.DORotate (new Vector3 (rx, ry, rz), time);
                T += Time.deltaTime;
                if (T>=time)
                {
                    isComplete = true;
                }
            }
			if (type == 1) 
            {
				if (!isArrived) 
                {
					mainCam.transform.DOMove (new Vector3 (px, py, pz) + m.gameObject.transform.position, 0).OnComplete (() => isArrived = true);
					mainCam.transform.DORotate (new Vector3 (rx, ry, rz), 0);
                    
				}
				mainCam.transform.RotateAround (m.gameObject.transform.position, Vector3.up, speed * Time.deltaTime);
                
//					if (Input.GetMouseButtonDown (1)) {
//						isComplete = true;
//					}
			}
		}
  //      else if(Input.GetMouseButtonDown (0))
  //      {
		//	isComplete = false;
		//}*/
	}
}
