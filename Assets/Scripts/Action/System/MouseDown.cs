using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class MouseDown : Action<Main>
{
    public VRTK_ControllerEvents vrControllerEvents;
    public VRTK_Pointer vrPointer;
    Transform a2, t;
    private Ray ray;
    private RaycastHit hit;
    public override void DoAction(Main m)
    {
        if (Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                //Transform a=Manager.Instace.gonggong.transform.GetChild(0);
                Transform a = even.target.gameObject.transform;
                if (hit.collider.gameObject.transform == a)
                {
                    Debug.Log("!!!!!!!!!!" + a);
                    even.Do();
                }
            }
        }

        a2 = even.target.gameObject.transform;
        if (a2.Equals(t) && vrControllerEvents.pointerPressed)
        {
            even.Do();
        }
    }

    public void Ini(VRTK_ControllerEvents vrCevents, VRTK_Pointer vrP)
    {
        vrControllerEvents = vrCevents;
        vrPointer = vrP;
        vrPointer.DestinationMarkerEnter += vrPointer_DestinationMarkerEnter;
    }

    void vrPointer_DestinationMarkerEnter(object sender, DestinationMarkerEventArgs e)
    {
        t = e.target;
    }
}
