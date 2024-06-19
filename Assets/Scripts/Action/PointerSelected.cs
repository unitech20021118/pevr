using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class PointerSelected : Action<Main> {
    public VRTK_ControllerEvents vrControllerEvents;
    public VRTK_Pointer vrPointer;
    Transform a,t;
    bool isSelected;
    private bool click;

    public override void DoAction(Main m)
    {
        a = even.target.gameObject.transform;
        if (!click && !vrControllerEvents.pointerPressed)
        {
            click = true;
        }
        
        if (a.Equals(t)&&click&& vrControllerEvents.pointerPressed)
        {
            even.Do();
            click = false;
        }
    }

    public void Ini(VRTK_ControllerEvents vrCevents,VRTK_Pointer vrP)
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
