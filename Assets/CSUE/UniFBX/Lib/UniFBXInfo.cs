using UnityEngine;
using System.Collections;

public class UniFBXInfo : MonoBehaviour {

    public Texture unityLogo;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI ( ) {
        Matrix4x4 matrixBackup = GUI.matrix;
        GUI.matrix = Matrix4x4.TRS (Vector3.zero, Quaternion.identity, new Vector3 (Screen.width / 1280.0f, Screen.height / 720.0f, 1.0f));
        GUI.color = new Color (1, 1, 1, 0.6f);
        GUI.DrawTexture (new Rect (1280 - 215, 720 - 70, 205, 62), unityLogo);
        GUI.matrix = matrixBackup;
    }
}
