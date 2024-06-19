using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BoolUI : TypeUI
{
    Bool Booldata;
    public Dropdown dropdown;
    List<string> list;

    public override void SetValue(Variable variable)
    {
        Booldata = (Bool)variable;
        if (Booldata.values != null)
        {
            //
            if (Booldata.data.values == null)
            {
                dropdown.value = Booldata.values ? 1 : 0;
            }
            else
            {
                dropdown.value = Booldata.data.values.Equals("真") ? 1 : 0;
            }
            
        }
        dropdown.onValueChanged.AddListener(delegate
        {

            Booldata.values = dropdown.value == 1 ? true : false;
            Booldata.data.values = Booldata.values ? "真" : "假";
        });

        inputFieldName.onEndEdit.AddListener(delegate
        {
            if (!string.IsNullOrEmpty(inputFieldName.text))
            {
                Booldata.name = inputFieldName.text;
                Booldata.data.name = data.name;
                if (Manager.Instace.variableToGameObject.ContainsKey(data))
                {
                    Manager.Instace.variableToGameObject[Booldata].transform.FindChild("Text").GetComponent<Text>().text = inputFieldName.text;
                }
            }

        });

        string[] strs = { "假", "真" };
        list = new List<string>(strs);
        UpdateDropDownItem(list);

        base.SetValue(Booldata);
    }

    void UpdateDropDownItem(List<string> showNames)
    {
        dropdown.options.Clear();
        Dropdown.OptionData tempData;
        for (int i = 0; i < showNames.Count; i++)
        {
            tempData = new Dropdown.OptionData();
            tempData.text = showNames[i];
            dropdown.options.Add(tempData);
        }
        if (Booldata.data.values == null)
        {
            dropdown.captionText.text = showNames[0];
        }        
        dropdown.RefreshShownValue();
    }

    public override void LoadValue(variableData data)
    {
        inputFieldName.name = data.name;
        if (data.values.Equals("true"))
        {
            dropdown.value = 1;
        }
        else
        {
            dropdown.value = 0;
        }
    }
}
