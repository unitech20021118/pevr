using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiling : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		transform.GetComponent<MeshRenderer>().material.mainTexture.wrapMode = TextureWrapMode.Repeat;
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.GetComponent<MeshRenderer>().material.mainTextureScale = new Vector2(transform.localScale.x, transform.localScale.y);
	}
}
