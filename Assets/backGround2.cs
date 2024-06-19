using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class backGround2 : MonoBehaviour,IPointerDownHandler {
    public GameObject AddFSM;
    public RectTransform canvas;
    public Camera camera;
    

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Input.GetMouseButtonDown(1))
        {
            AddFSM.SetActive(true);
            AddFSM.transform.position = Manager.Instace.ScreenPointToWorldPos(canvas, camera);
        }

        if (Input.GetMouseButtonDown(0))
        {
            AddFSM.SetActive(false);           
        }
    }


}
