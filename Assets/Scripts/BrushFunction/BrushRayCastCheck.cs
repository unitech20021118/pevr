using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BrushRayCastCheck 
{
    private static Ray ray;
    private static RaycastHit raycastHit;

    private static Vector3 vec;
    private static Vector3 nullVector3 = new Vector3(0, -500, 0);

    public static Vector3 NullVector3 { get { return nullVector3; } }

    public static LayerMask StaticLayer = 1 << 8;

    public static RaycastHit MouseCheckGround(LayerMask layerMask)
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray,out raycastHit,1000f, layerMask))
        {
            if (raycastHit.collider.gameObject.layer == 13 && raycastHit.collider.tag == "BrushWall")
            {
                return raycastHit;
            }
            else if (raycastHit.collider.gameObject.layer == 8)
            {
                return raycastHit;
            }
            else
            {
                raycastHit = new RaycastHit();
                return raycastHit;
            }
        }
        else
        {
            raycastHit = new RaycastHit();
            return raycastHit;
        }
    }

    public static RaycastHit MouseCheckGroundOnly()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out raycastHit, 1000f, StaticLayer))
        {
            return raycastHit;
        }
        else
        {
            raycastHit = new RaycastHit();
            return raycastHit;
        }
    }
    public static RaycastHit MouseCheckBuildingGround(LayerMask layerMask)
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out raycastHit, 1000f, layerMask))
        {
            if (raycastHit.collider.tag == "BrushWall")
            {
                return raycastHit;
            }
            else if (raycastHit.collider.tag == "BrushGround")
            {
                return raycastHit;
            }
            else
            {
                raycastHit = new RaycastHit();
                return raycastHit;
            }
        }
        else
        {
            raycastHit = new RaycastHit();
            return raycastHit;
        }
    }

    public static RaycastHit MouseCheckBreshedWall(LayerMask layerMask)
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out raycastHit, 1000f, layerMask))
        {
            if (raycastHit.collider.tag == "BrushWall")
            {
                
                return raycastHit;
            }
            else
            {
                raycastHit = new RaycastHit();
                return raycastHit;
            }
        }
        else
        {
            raycastHit = new RaycastHit();
            return raycastHit;
        }
    }

    public static RaycastHit MouseCheckBrushObj(LayerMask layer)
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out raycastHit, 1000f, layer))
        {
            return raycastHit;
        }
        else
        {
            raycastHit = new RaycastHit();
            return raycastHit;
        }
    }

}

public class PointAndObj
{
    public GameObject Object;
    public Vector3 Point;
}
