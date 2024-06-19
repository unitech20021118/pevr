using UnityEngine;
using System.Collections;

[RequireComponent (typeof(UniFBXImport))]
public class UniFBXLoader : MonoBehaviour
{
	public UniFBXImport ufbx;
    private float progress = 0.0f;    

    void Start ( ) { }

    void Update ( ) {
        if (ufbx) {
            var p = ufbx.GetProcentage ();
            progress = Mathf.Lerp (progress, p, 2.0f * Time.deltaTime);
        }        
    }

	void OnGUI ()
	{
		if (GUI.Button (new Rect (0, 0, 100, 40), "Load")) {
            if (ufbx) {
                ufbx.Load ();
            }
		}
        if (ufbx) {
            GUI.Label (new Rect (0, Screen.height - 36, Screen.width, 20), "Completed: " + this.progress.ToString ("0") + "%");
            GUI.Label (new Rect (0, Screen.height - 20, Screen.width, 20), "Status: " + ufbx.setting.Status.ToString ());
        }
        
	}

}