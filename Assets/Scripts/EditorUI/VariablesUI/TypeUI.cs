using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TypeUI : MonoBehaviour {

    public Variable data;
    public InputField inputFieldName;

    


    //public virtual void  Clear()
    //{
    //    inputFieldName.text = "";
        
    //}

    public virtual void SetValue(Variable data)
    {

        this.data = data;
        inputFieldName.text = data.name;


    }

    public virtual void LoadValue(variableData data)
    {
        Debug.Log(222);
    }
}
