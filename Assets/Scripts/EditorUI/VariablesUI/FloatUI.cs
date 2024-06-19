using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FloatUI :TypeUI{

    public Float floatData;
    public InputField inputFieldValue;

        public override void SetValue(Variable data)
        {
            floatData = (Float)data;
            
            
            if (floatData.values != null)
            {

                inputFieldValue.text = floatData.data.values;
                
                //floatData.data.values = inputFieldValue.text;

            }

            inputFieldValue.onEndEdit.AddListener(delegate
            {
                if (!string.IsNullOrEmpty(inputFieldValue.text))
                {
                    floatData.values = float.Parse(inputFieldValue.text);
                    floatData.data.values = floatData.values.ToString();
                }
                    

            });

            inputFieldName.onEndEdit.AddListener(delegate
            {
                if (!string.IsNullOrEmpty(inputFieldName.text))
                {
                    floatData.name = inputFieldName.text;
                    floatData.data.name = data.name;
                    if (Manager.Instace.variableToGameObject.ContainsKey(data))
                    {
                        Manager.Instace.variableToGameObject[floatData].transform.FindChild("Text").GetComponent<Text>().text = inputFieldName.text;
                    }
                }

            });

            base.SetValue(data);
        }

        public override void LoadValue(variableData data)
        {
            Debug.Log(111);
            inputFieldValue.text = data.values;
            inputFieldName.text = data.name;
        }
}
