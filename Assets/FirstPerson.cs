using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPerson : MonoBehaviour {

	
	public virtual void Start () {
        Manager.Instace.mainCamera.gameObject.SetActive(false);
	}
	
	
}
