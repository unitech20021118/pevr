using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G_PubDef : MonoBehaviour {

    public static LayerMask QuiescentObject = 1<<8;

    public static LayerMask DynamicObject = LayerMask.GetMask("dynamic");

    public static LayerMask layerMask = LayerMask.GetMask("Water");

    public static LayerMask BrushObj = LayerMask.GetMask("BrushObj");

    public static int quiescentObject = 8;

    public static int dynamicObject = 9;
}
