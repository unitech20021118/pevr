using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 控制蜡烛火焰特效
/// </summary>
public class CandleFire : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        transform.GetComponent<ParticleEmitter>().maxSize = 0.1f + 0.01f * 1f;
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
