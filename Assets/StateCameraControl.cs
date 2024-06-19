using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StateCameraControl : MonoBehaviour ,IPointerEnterHandler{
    Vector3 origalPos;
    bool isPressed;
    Vector3 _lastposition;
    Camera _cam;
    bool _yeyeye;
    public GameObject ImageBg;
    void Start()
    {
        _cam = this.GetComponent<Camera>();
        _lastposition = this.transform.position;
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //if (Input.GetAxis("Mouse ScrollWheel") > 0 && EventSystem.current.IsPointerOverGameObject())
        //{
        //    //if (Camera.main.fieldOfView >40)
        //    //{
        //    //    Camera.main.fieldOfView -= 1f;
        //    //}
        //    GetComponent<Camera>().orthographicSize += 3;
        //}
        //if (Input.GetAxis("Mouse ScrollWheel") < 0 && EventSystem.current.IsPointerOverGameObject())
        //{
        //    //if (Camera.main.fieldOfView < 100)
        //    //{
        //    //    Camera.main.fieldOfView += 1f;
        //    //}
        //    GetComponent<Camera>().orthographicSize -= 3;
        //}
    }
    bool IsStateCamera(Camera cam)
    {
        Vector3 position;
        Vector3 posCam = cam.WorldToScreenPoint(new Vector3(0, 0, 0));
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, posCam.z);
        position = cam.ScreenToWorldPoint(mousePos);

        Rect scRect = new Rect(0, 0, Screen.width*cam.rect.width, Screen.height*cam.rect.height);
        if (scRect.Contains(cam.WorldToScreenPoint(position)))
        {
            _yeyeye = true;
        }
        if (!scRect.Contains(cam.WorldToScreenPoint(position)))
        {
            _yeyeye = false;
        }
        return _yeyeye;
    }
	void Update () {
        IsStateCamera(_cam);
        if (_yeyeye)
        {
            StateMouseWork();
            //if (Manager.Instace.StatePanel.GetComponent<StatePanelUI>().currentState != null)
            //{
            //    Manager.Instace.StatePanel.GetComponent<StatePanelUI>().UnableCurrentActionlist();
            //}
        }
	}
    void StateMouseWork()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && EventSystem.current.IsPointerOverGameObject())
        {

            GetComponent<Camera>().orthographicSize += 3;
            ImageBg.transform.localScale = new Vector3((GetComponent<Camera>().orthographicSize / 100f)+0.15f, (GetComponent<Camera>().orthographicSize / 100f)+0.15f, 0);
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0 && EventSystem.current.IsPointerOverGameObject())
        {
            if (GetComponent<Camera>().orthographicSize > 89)
            {
                GetComponent<Camera>().orthographicSize -= 3;
                ImageBg.transform.localScale = new Vector3((GetComponent<Camera>().orthographicSize / 100f) + 0.15f, (GetComponent<Camera>().orthographicSize / 100f) + 0.15f, 0);
            }
            if (GetComponent<Camera>().orthographicSize < 89)
            {
                this.GetComponent<Camera>().orthographicSize = 89;
            }

        }
        if (Input.GetMouseButtonDown(2) && EventSystem.current.IsPointerOverGameObject())
        {
            origalPos = Input.mousePosition;
            isPressed = true;

        }
        if (Input.GetMouseButtonUp(2))
        {
            isPressed = false;
        }
        if (isPressed)
        {
            Vector3 temp = Input.mousePosition - origalPos;
            if (temp.x != 0)
            {
                transform.localPosition -= new Vector3(1, 0, 0) * temp.x;
            }
            if (temp.y != 0)
            {
                transform.localPosition -= new Vector3(0, 1, 0) * temp.y;
            }
            origalPos = Input.mousePosition;
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            this.transform.position = _lastposition;
            this.GetComponent<Camera>().orthographicSize = 89;
        }
    }
}
