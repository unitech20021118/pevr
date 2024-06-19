using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPerson :MonoBehaviour{
    Vector3 offset;
    void Start()
    {
       offset = Manager.Instace.mainCamera.transform.position- transform.position;
      
    }

    void Update()
    {
        Manager.Instace.mainCamera.transform.position = transform.position + offset;
    }

    
	
	
}
