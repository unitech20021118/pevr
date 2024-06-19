using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragaction : Action<Main>
{
    public float rotateSensitivity;

    public override void DoAction(Main m)
    {
        if (m.gameObject.GetComponent<DragableObj>()==null)
        {
            DragableObj dra = m.gameObject.AddComponent<DragableObj>();
            dra.rotateSensitivity = rotateSensitivity;
        }
        //Rigidbody[] parts = m.gameObject.GetComponentsInChildren<Rigidbody>();
        //if (parts.Length == 0)
        //{
        //    if (m.gameObject.GetComponent<Rigidbody>() != null)
        //    {
        //        if (m.gameObject.GetComponent<DragableObj>() == null)
        //        {
        //            m.gameObject.AddComponent<DragableObj>().rotateSensitivity = rotateSensitivity;
        //        }
        //        else
        //        {
        //            m.gameObject.GetComponent<DragableObj>().rotateSensitivity = rotateSensitivity;
        //        }
        //    }
        //    else
        //    {
        //        Rigidbody rbody = m.gameObject.AddComponent<Rigidbody>();
        //        rbody.useGravity = false;
        //        rbody.isKinematic = true;
        //        m.gameObject.AddComponent<DragableObj>().rotateSensitivity = rotateSensitivity;
        //    }
        //}
        //else
        //{
        //    foreach (Rigidbody item in parts)
        //    {
        //        item.gameObject.AddComponent<DragableObj>().rotateSensitivity = rotateSensitivity;
        //    }
        //}
    }
}
