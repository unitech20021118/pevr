using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class backGround3 : MonoBehaviour, IPointerDownHandler
{
    public GameObject AddState;
    public RectTransform canvas;
    public Camera camera;
    public GameObject editor;

    public void OnPointerDown(PointerEventData eventData)
    {

        if (Input.GetMouseButtonDown(1))
        {
            AddState.SetActive(true);
            AddState.transform.position = Manager.Instace.ScreenPointToWorldPos(canvas, camera);
        }
        if (Input.GetMouseButtonDown(0))
        {
            AddState.SetActive(false);
            foreach (Transform t in editor.transform)
            {
                t.gameObject.SetActive(false);
            }
            
        }
  

    }

    //public void OnPointerEnter(PointerEventData eventData)
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        GameObject obj = CheckLine();
    //        if (obj != null)
    //        {
    //            Debug.Log(obj.name);
    //        }
    //    }
    //}

    

    //public GameObject CheckLine()
    //{
    //    Ray ray =Manager.Instace.stateMachineCamera.ScreenPointToRay(Input.mousePosition);
    //    RaycastHit hit;
    //    if (Physics.Raycast(ray, out hit))
    //    {
    //        return hit.collider.gameObject;
    //    }
    //    return null;
    //}
}
