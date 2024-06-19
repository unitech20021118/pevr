using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class ObjMsgCtrl : MonoBehaviour {
    public VRTK_Pointer vrPointer;
    public VRTK_ObjectTooltip tipCtrller;
    public Transform senceTip;
    public Transform cam;

    // Use this for initialization
    void Start()
    {
        vrPointer.DestinationMarkerEnter += StartTouch;
        vrPointer.DestinationMarkerExit += StopTouch;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void StartTouch(object obj, DestinationMarkerEventArgs e)
    {
        if (e.target.GetComponent<ObjMsg>())
        {
            senceTip.gameObject.SetActive(true);
            senceTip.position = e.destinationPosition;
            tipCtrller.displayText = e.target.GetComponent<ObjMsg>().msg;
            tipCtrller.ResetTooltip();
            senceTip.LookAt(cam);
        }
    }

    void StopTouch(object obj, DestinationMarkerEventArgs e)
    {
        senceTip.gameObject.SetActive(false);
    }
}
