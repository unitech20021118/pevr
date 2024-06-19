using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTransform : Action<Main>
{
    public string xp;
    public string yp;
    public string zp;
    public string xr;
    public string yr;
    public string zr;
    public string xs;
    public string ys;
    public string zs;

    private Vector3 positionVector3;
    private Vector3 rotationVector3;
    private Vector3 scaleVector3;

    /// <summary>
    /// 目标物体
    /// </summary>
    public GameObject target;
    public string targetPath;
    public override void DoAction(Main m)
    {
        if (target == null)
        {
            if (string.IsNullOrEmpty(targetPath))
            {
                target = m.gameObject;
            }
            else
            {
                target = GameObject.Find("Parent/" + targetPath);
            }
        }

        if (!string.IsNullOrEmpty(xp))
        {
            positionVector3.x = float.Parse(xp);
        }
        else
        {
            positionVector3.x = target.transform.position.x;
        }
        if (!string.IsNullOrEmpty(yp))
        {
            positionVector3.y = float.Parse(yp);
        }
        else
        {
            positionVector3.y = target.transform.position.y;
        }
        if (!string.IsNullOrEmpty(zp))
        {
            positionVector3.z = float.Parse(zp);
        }
        else
        {
            positionVector3.z = target.transform.position.z;
        }
        if (!string.IsNullOrEmpty(xr))
        {
            rotationVector3.x = float.Parse(xr);
        }
        else
        {
            rotationVector3.x = target.transform.localEulerAngles.x;
        }
        if (!string.IsNullOrEmpty(yr))
        {
            rotationVector3.y = float.Parse(yr);
        }
        else
        {
            rotationVector3.y = target.transform.localEulerAngles.y;
        }
        if (!string.IsNullOrEmpty(zr))
        {
            rotationVector3.z = float.Parse(zr);
        }
        else
        {
            rotationVector3.z = target.transform.localEulerAngles.z;
        }
        if (!string.IsNullOrEmpty(xs))
        {
            scaleVector3.x = float.Parse(xs);
        }
        else
        {
            scaleVector3.x = target.transform.localScale.x;
        }
        if (!string.IsNullOrEmpty(ys))
        {
            scaleVector3.y = float.Parse(ys);
        }
        else
        {
            scaleVector3.y = target.transform.localScale.y;
        }
        if (!string.IsNullOrEmpty(zs))
        {
            scaleVector3.z = float.Parse(zs);
        }
        else
        {
            scaleVector3.z = target.transform.localScale.z;
        }



        target.transform.position = positionVector3;
        target.transform.localEulerAngles = rotationVector3;
        target.transform.localScale = scaleVector3;

    }
}
