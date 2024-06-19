using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragableObj : MonoBehaviour {
	public float rotateSensitivity=1;
	public Transform cameraTransform;
    public Camera camera;
	bool isDragging,rotateMode;
	float distance;
	float offsetX,offsetY;

	// Use this for initialization
	void Start () {
        SetCamera();
	}
    public void SetCamera(Camera cam=null)
    {
        if (cam == null && cameraTransform==null)
        {
            cameraTransform = Camera.main.transform;
            camera = Camera.main;
        }
        else if(cam!=null)
        {
            cameraTransform = cam.transform;
            camera = cam;
        }
    }
	
	void Update () {
        ApplyDragging ();
		SwitchMode ();
	    RotateGameObject();
	}

	void ApplyDragging(){
        //if (isDragging) {
        //    Vector3 tarMousePos = Camera.main.ScreenToWorldPoint (new Vector3(Input.mousePosition.x+offsetX, Input.mousePosition.y+offsetY, distance));
        //    transform.position = tarMousePos;
        //    if(Input.GetMouseButtonUp(0)){
        //        isDragging=false;
        //    }
        //}
        if (isDragging)
        {
            Vector3 tarMousePos = camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x + offsetX, Input.mousePosition.y + offsetY, distance));
            transform.position = tarMousePos;
            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
            }
        }
	}

	void OnMouseDown(){
	    if (camera.gameObject.activeSelf==false&&Manager.Instace.FirstPersonCamera.gameObject.activeSelf)
	    {
	        camera = Manager.Instace.FirstPersonCamera;
	        cameraTransform = Manager.Instace.FirstPersonCamera.transform;
	    }
		if (!rotateMode) {
            Vector3 objScreenPos = camera.WorldToScreenPoint(transform.position);
			offsetX = objScreenPos.x - Input.mousePosition.x;
			offsetY = objScreenPos.y - Input.mousePosition.y;
			isDragging = true;
			Vector3 objToCam = transform.position - camera.transform.position;
            float angle = Vector3.Angle(camera.transform.forward, objToCam);
			distance = Mathf.Cos (angle * Mathf.PI / 180f) * objToCam.magnitude;
		}
	}

	void OnMouseExit(){

	}

	void OnMouseOver(){

	}

	void OnMouseDrag(){
        if (rotateMode) {
            //			cameraTransform.RotateAround (transform.position, Vector3.up, Input.GetAxis ("Mouse X") * 60 * Time.deltaTime);
            //			cameraTransform.RotateAround (transform.position, Vector3.right, Input.GetAxis ("Mouse Y") * 60 * Time.deltaTime);
            
            transform.RotateAround (transform.position, Vector3.up, -Input.GetAxis ("Mouse X") * rotateSensitivity * Time.deltaTime);
			transform.RotateAround (transform.position, cameraTransform.right,  Input.GetAxis ("Mouse Y") * rotateSensitivity * Time.deltaTime);
		}
	    if (Input.GetKey(KeyCode.Q))
	    {
	        transform.Rotate(new Vector3(0, 30*Time.deltaTime, 0),Space.World);
        }
        if (Input.GetKey(KeyCode.E))
	    {
	        transform.Rotate(new Vector3(0, -30*Time.deltaTime, 0), Space.World);
        }
	    if (Input.GetKey(KeyCode.Z))
	    {
            transform.Rotate(new Vector3(30*Time.deltaTime, 0, 0), Space.Self);
        }
	    if (Input.GetKey(KeyCode.X))
	    {
	        transform.Rotate(new Vector3(-30 * Time.deltaTime, 0, 0), Space.Self);
	    }
    }

	void SwitchMode(){
		if (Input.GetKeyDown (KeyCode.LeftControl)) {
			rotateMode = true;
		}
		if (Input.GetKeyUp (KeyCode.LeftControl)) {
			rotateMode = false;
        }
	}

    void RotateGameObject()
    {
        
    }
}
