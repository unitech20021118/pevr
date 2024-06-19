using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitTest : MonoBehaviour {
	int a = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void OnApplicationQuit()
    {
		a++;
        if (a>=3)
        {
			return;
        }
		Application.CancelQuit();
    }

	public void Quit()
    {
		Application.Quit();
    }
}
