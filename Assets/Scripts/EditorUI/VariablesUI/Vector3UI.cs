using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Vector3UI :TypeUI{

    public _Vector3 vector3Data;
    public InputField X;
    public InputField Y;
    public InputField Z;

    public override void SetValue(Variable data)
    {
        vector3Data = (_Vector3)data;
        if (vector3Data.vector_3 != null)
        {

            if (vector3Data.data.values == null)
            {
                X.text = vector3Data.vector_3.x.ToString();
                Y.text = vector3Data.vector_3.y.ToString();
                Z.text = vector3Data.vector_3.z.ToString();
            }
            else
            {
                X.text = Manager.Instace.GetVector3_X(vector3Data.data.values).ToString();
                Y.text = Manager.Instace.GetVector3_Y(vector3Data.data.values).ToString();
                Z.text = Manager.Instace.GetVector3_Z(vector3Data.data.values).ToString();
            }

        }

        X.onEndEdit.AddListener(delegate
        {
            if (!string.IsNullOrEmpty(X.text))
                vector3Data.vector_3.x = float.Parse(X.text);
                vector3Data.data.values = vector3Data.vector_3.ToString();
        });
        Y.onEndEdit.AddListener(delegate
        {
            if (!string.IsNullOrEmpty(Y.text))
                vector3Data.vector_3.y = float.Parse(Y.text);
                vector3Data.data.values = vector3Data.vector_3.ToString();
        });
        Z.onEndEdit.AddListener(delegate
        {
            if (!string.IsNullOrEmpty(Z.text))
                vector3Data.vector_3.z = float.Parse(Z.text);
                vector3Data.data.values = vector3Data.vector_3.ToString();
        });

        inputFieldName.onEndEdit.AddListener(delegate
        {
            if (!string.IsNullOrEmpty(inputFieldName.text))
            {
                vector3Data.name = inputFieldName.text;
                vector3Data.data.name = data.name;
                if (Manager.Instace.variableToGameObject.ContainsKey(data))
                {
                    Manager.Instace.variableToGameObject[vector3Data].transform.FindChild("Text").GetComponent<Text>().text = inputFieldName.text;
                }
            }

        });

        base.SetValue(data);
    }
}
