using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G_Interface : MonoBehaviour {

    public enum EditMenuDef
    {
        Scale,
        Rotate,
        Delete,
        AddNote
    }
    public delegate void MouseTouch(GameObject obj, Vector3 pos);
    public delegate void MouseClick(EditMenuDef id);
    MouseTouch mouseTouch;
    MouseClick mouseClick;
    public void SendMouseTouchEvent(GameObject obj, Vector3 pos)
    {
        if (mouseTouch !=null)
        {
            mouseTouch(obj,pos);
        }
    }

    public void SendMouseClickEvent(EditMenuDef id)
    {
        if (mouseClick != null)
        {
            mouseClick(id);
        }
    }
}
