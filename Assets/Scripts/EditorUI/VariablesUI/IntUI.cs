using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class IntUI :TypeUI{

    public Int intData;
    public InputField inputFieldValue;

        public override void SetValue(Variable data)
        {
            intData = (Int)data;

            if (intData.values != null)
            {

                inputFieldValue.text = intData.data.values;
                 
            }
            base.SetValue(data);

            inputFieldValue.onEndEdit.AddListener(delegate
            {
                if (!string.IsNullOrEmpty(inputFieldValue.text))
                {
                    intData.values = (int)float.Parse(inputFieldValue.text);
                    intData.data.values = intData.values.ToString();
                }
                   

            });

            inputFieldName.onEndEdit.AddListener(delegate
            {
                if (!string.IsNullOrEmpty(inputFieldName.text))
                {
                    intData.name = inputFieldName.text;
                    intData.data.name = data.name;
                    if (Manager.Instace.variableToGameObject.ContainsKey(data))
                    {
                        Manager.Instace.variableToGameObject[intData].transform.FindChild("Text").GetComponent<Text>().text = inputFieldName.text;
                    }
                }

            });
        }

        public override void LoadValue(variableData data)
        {

            inputFieldName.text = data.name;
            inputFieldValue.text = data.values;
        }
}
