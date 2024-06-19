using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouseMovement : Action<Main>
{
    public float X;
    public float Y;
    public float Z;
    public string XMin;
    public string XMax;
    public string YMin;
    public string YMax;
    public string ZMin;
    public string ZMax;
    public bool Place;

    public bool isFace;
    public bool faceX;
    public bool faceY;
    public bool faceZ;

    private FollowMouseMoveControl followMouseMoveControl;
    public override void DoAction(Main m)
    {
        
        followMouseMoveControl = m.gameObject.GetComponent<FollowMouseMoveControl>();
        if (followMouseMoveControl==null)
        {
            followMouseMoveControl = m.gameObject.AddComponent<FollowMouseMoveControl>();
        }
        //Debug.LogError("lll");
        //followMouseMoveControl.InitData(X, Y, Z, Place, XMin, XMax, YMin, YMax, ZMin, ZMax,isFace,faceX,faceY,faceZ);
        followMouseMoveControl.StartCoroutine(followMouseMoveControl.Dowait(X, Y, Z, Place, XMin, XMax, YMin, YMax,
            ZMin, ZMax, isFace, faceX, faceY, faceZ));
    }
}
