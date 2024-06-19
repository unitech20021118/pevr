using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G_RotateBallColor : MonoBehaviour {
    private Color _preColor;

    private Color _chooseColor;

    private bool _haveOneChoose;

    
	// Use this for initialization
	void Start () {
        G_ObjectMouseListener component = transform.GetComponent<G_ObjectMouseListener>();
        component.onEnter += onEnterAxis;
        component.onExit += onExitAxis;
        Renderer component2 = base.GetComponent<Renderer>();
        Material material = component2.material;
        //this._preColor = material.color;
        this._preColor = material.GetColor("_IlluminCol");
        
	}



    private void onEnterAxis(GameObject obj)
    {
        if (!this._haveOneChoose)
        {
            this.ChangeColor(obj);
        }
    }

    private void onExitAxis(GameObject obj)
    {
        if (!this._haveOneChoose)
        {
            this.RestoreColor(obj);
        }
    }

    private void ChangeColor(GameObject obj)
    {
        Renderer component = obj.GetComponent<Renderer>();
        Material material = component.material;
        //material.color = this._chooseColor;
        material.SetColor("_IlluminCol", this._chooseColor);
    }

    private void RestoreColor(GameObject obj)
    {
        Renderer component = obj.GetComponent<Renderer>();
        Material material = component.material;
        //material.color = this._preColor;
        material.SetColor("_IlluminCol", this._preColor);
    }

    public void SetChooseColor(Color chooseColor)
    {
        this._chooseColor = chooseColor;
    }

    public void SetHaveOneChoose(bool haveOneChoose)
    {
        this._haveOneChoose = haveOneChoose;
        if (!this._haveOneChoose)
        {
            this.RestoreColor(base.gameObject);
        }
    }
}
