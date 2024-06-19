using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrushedGround : MonoBehaviour 
{
	public string Style;
    /// <summary>
    /// 离地高度
    /// </summary>
    private float heightAboveGround;
    /// <summary>
    /// 是否是作为某一层的天花板
    /// </summary>
    public bool Ceiling { get; set; }

    public float HeightAboveGround 
	{
        get
        {
            heightAboveGround = transform.localPosition.y;
            return heightAboveGround;
        }
        set
        {
            heightAboveGround = value;
            transform.localPosition = new Vector3(transform.localPosition.x, heightAboveGround, transform.localPosition.z);
        }
	}

    //public List<Vector3> points;

    // Use this for initialization
    void Start () 
	{
        if (transform.Find("Back") == null)
        {
            //GameObject chi = new GameObject("Back");
            //chi.AddComponent<MeshFilter>().mesh = GetComponent<MeshFilter>().sharedMesh;
            //chi.AddComponent<MeshRenderer>().material = GetComponent<MeshRenderer>().material;
            //chi.transform.SetParent(transform);
            //chi.transform.localScale = new Vector3(1, -1, 1);
            //chi.transform.localPosition = Vector3.zero;
            
        }
    }
	
	// Update is called once per frame
	void Update () 
	{
        if(transform.Find("Back") != null)
        {
            if (transform.Find("Back").GetComponent<MeshRenderer>().material != transform.GetComponent<MeshRenderer>().material)
            {
                transform.Find("Back").GetComponent<MeshRenderer>().material = transform.GetComponent<MeshRenderer>().material;
            }
        }
	}

	public void Init(List<Vector3> points)
    {
        //Debug.LogError("11111");
		Mesh mesh = BrushManager.Instance.GetComponent<BrushMesh>().GetMeshByPoints(new List<Vector3>(points));
        if (mesh!=null)
        {
            //Debug.LogError("22222");
            gameObject.AddComponent<MeshFilter>().sharedMesh = mesh;
			gameObject.AddComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/BrushWallMaterials/qipange");
			gameObject.AddComponent<MeshCollider>().sharedMesh = mesh;
        }
    }
}
