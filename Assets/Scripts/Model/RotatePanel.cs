using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RotatePanel : MonoBehaviour {

    public Slider X;
    public Slider Y;
    public Slider Z;
    G_EditorTarget editorTarget;

    void Start()
    {
        editorTarget=transform.GetComponent<G_EditorTarget>();
    }

    void Update(){
        if (Input.GetMouseButtonDown(0))
        {
            
            editorTarget.RotateByX(X.value);
            editorTarget.RotateByY(Y.value);
            editorTarget.RotateByZ(Z.value);
        }
    }
}
