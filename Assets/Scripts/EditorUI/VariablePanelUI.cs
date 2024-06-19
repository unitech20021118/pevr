using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class VariablePanelUI : MonoBehaviour {
    public Dropdown dropDown;
    public InputField inputField;
    public Transform parent;
    public static List<string> list;
    GameObject obj;
    //public Dictionary<string, string> dict = new Dictionary<string, string>();
    void Start()
    {
        
        string[] strs = {"浮点型","整型","布尔型","三维坐标"};
        list = new List<string>(strs);
        UpdateDropDownItem(list);
    }



    void UpdateDropDownItem(List<string> showNames)
    {
        dropDown.options.Clear();
        Dropdown.OptionData tempData;
        for (int i = 0; i < showNames.Count; i++)
        {
            tempData = new Dropdown.OptionData();
            tempData.text = showNames[i];
            dropDown.options.Add(tempData);
        }
        dropDown.captionText.text = showNames[0];
        dropDown.RefreshShownValue();
    }

    public void AddVariable()
    {
        if (!string.IsNullOrEmpty(inputField.text))
        {
                AddItem(dropDown.value);
        }
    }

    public GameObject AddItem( int num)
    {
        if (obj == null)
        {
            obj = (GameObject)Resources.Load("Prefabs/VariableTypePrefab");
        }        
        GameObject temp = Instantiate(obj);
        temp.transform.SetParent(parent);
        temp.transform.localScale = Vector3.one;
        temp.transform.localPosition = Vector3.zero;
        Variable t=null;
        switch (num)
        {
            case 0:
                t = new Float();
                t.name = inputField.text;
                t.data = new variableData(0);
                t.type = 0;
                break;
            case 1:
                t = new Int();
                t.name = inputField.text;
                t.data = new variableData(1);
                t.type = 1;
                break;

            case 2:
                t = new Bool();
                t.name = inputField.text;
                t.data = new variableData(2);
                t.type = 2;
                break;
            case 3:
                t = new _Vector3();
                t.name = inputField.text;
                t.data = new variableData(3);
                t.type = 3;
                break;

        }
        Manager.Instace.allDataInformation.listTree.Root.data.customVariable.Add(t.data);
        t.data.name = inputField.text;
        temp.GetComponent<VariableControl>().variable = t;
        return temp;
    }

    public GameObject LoadItem(int num)
    {
        if (obj == null)
        {
            obj = (GameObject)Resources.Load("Prefabs/VariableTypePrefab");
        }
        GameObject temp = Instantiate(obj);
        temp.transform.SetParent(parent);
        temp.transform.localScale = Vector3.one;
        temp.transform.localPosition = Vector3.zero;
        Variable t = null;
        switch (num)
        {
            case 0:
                t = new Float();
                t.name = inputField.text;
                t.data = new variableData(0);
                t.type = 0;
                break;
            case 1:
                t = new Int();
                t.name = inputField.text;
                t.data = new variableData(1);
                t.type = 1;
                break;

            case 2:
                t = new Bool();
                t.name = inputField.text;
                t.data = new variableData(2);
                t.type = 2;
                break;
            case 3:
                t = new _Vector3();
                t.name = inputField.text;
                t.data = new variableData(3);
                t.type = 3;
                break;

        }
        
        t.data.name = inputField.text;
        temp.GetComponent<VariableControl>().variable = t;
        return temp;
    }
}

public enum PathType
{
    Int=0,
    Float,
    Bool,
}




