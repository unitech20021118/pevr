using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G_RayCastCheck : MonoBehaviour {


    public static bool MouseCheckGameObject(out GameObject gameobject, LayerMask maskLay)
    {
        Vector3 hitPos;
        float maxDistance = 1000f;
        if (CameraRayCastCheckGameObject(out gameobject, out hitPos, maxDistance, maskLay))
        {
            return true;
        }
        return false;
    }

    public static bool MouseCheckGameObejctReturnPos(out GameObject obj, out Vector3 hitP, LayerMask maskLay)
    {        
        float maxDistance = 1000f;
        if (CameraRayCastCheckGameObject(out obj, out hitP, maxDistance, maskLay))
        {
            return true;
        }
        return false;
    }

    public static bool QuiescentObjectCheckGround(out Vector3 hitPos, LayerMask maskLay)
    {
        GameObject ground;
        float maxDistance=1000f;
        if (CameraRayCastCheckGameObject(out ground,out hitPos, maxDistance, maskLay))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 检测有没有在Laymask的物体
    /// </summary>
    /// <param name="gameObjectChecked"></param>
    /// <param name="hitPos"></param>
    /// <param name="maxDistance"></param>
    /// <param name="masklay"></param>
    /// <returns></returns>
    private static bool CameraRayCastCheckGameObject(out GameObject gameObjectChecked,out Vector3 hitPos,float maxDistance,LayerMask masklay)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray,out hit ,maxDistance, masklay))
        {
            gameObjectChecked = hit.collider.gameObject;
            hitPos = hit.point;
            return true;
        }
        gameObjectChecked = null;
        hitPos = Vector3.zero;
        return false;
    }

    public static Vector3 ScreenPosToWorldPos(Vector3 ScreenPos)
    {
        return Camera.main.ScreenToWorldPoint(ScreenPos);
    }

    public static Vector3 WorldPosToScreenPos(Vector3 WorldPos)
    {
        return Camera.main.WorldToScreenPoint(WorldPos);
    }

    
}
