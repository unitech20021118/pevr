using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {
    public float forward = 0;
    private float speed = 3;

    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (Mathf.Abs(h) > 0 || Mathf.Abs(v) > 0)
        {
            transform.Translate(new Vector3(h, 0, v) * speed * Time.deltaTime, Space.World);
            transform.rotation = Quaternion.LookRotation(new Vector3(h, 0, v));

        }
    }
}
