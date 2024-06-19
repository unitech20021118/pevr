
using UnityEngine;


/// <summary>
/// 在地板上拖动物体
/// </summary>
public class DragObjOnGround : MonoBehaviour 
{
	Vector3 offest;

	Vector3 MousePositionOnGround = new Vector3(5000,-5000,5000);

	Vector3 hitPosition;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
        if (Input.GetMouseButtonUp(0))
        {
			MousePositionOnGround = new Vector3(5000, -5000, 5000);
		}
	}

	void OnMouseDown()
    {
		offest = Vector3.zero;
    }
	void OnMouseDrag()
    {
        if (G_RayCastCheck.QuiescentObjectCheckGround(out hitPosition, G_PubDef.QuiescentObject))
        {
			if (MousePositionOnGround == new Vector3(5000, -5000, 5000))
            {
				offest = Vector3.zero;
            }
            else
            {
				offest = hitPosition - MousePositionOnGround;
			}
			transform.localPosition += offest;
			MousePositionOnGround = hitPosition;
        }
    }

	void OnMouseEndDrag()
    {
		
    }
}
