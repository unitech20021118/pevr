using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class VariableControl : MonoBehaviour,WindowInterface {
    public Text name;
    public Text type;
    public Variable variable;
    public Button btn;
    private static GameObject lastobj;
    private GameObject currentobj;

	// Use this for initialization
	void Start () {
        name.text = variable.name;
        type.text = VariablePanelUI.list[variable.type];
        Manager.Instace.variables.Add(variable);
        btn.onClick.AddListener(delegate (){ShowValueEditor(variable.type);});
        Manager.Instace.variableToGameObject.Add(variable, gameObject);
	}
	

    public void Close()
    {
        Manager.Instace.variableToGameObject.Remove(variable);
        Destroy(gameObject);
        Manager.Instace.variables.Remove(variable);
        Manager.Instace.allDataInformation.listTree.Root.data.customVariable.Remove(variable.data);
        //if (currentobj != null)
        //{
        //    currentobj.GetComponent<TypeUI>().Clear();
        //}
        Manager.Instace.AddAvariable.SetActive(true);
        Manager.Instace.typeList.SetActive(false);
        
    }

    public void ShowValueEditor(int num)
    {
        Manager.Instace.AddAvariable.SetActive(false);
        Manager.Instace.typeList.SetActive(true);
        currentobj=Manager.Instace.typeList.transform.GetChild(num).gameObject;
        if (lastobj != currentobj)
        {
            if (lastobj != null)
            {
                lastobj.SetActive(false);
            }            
            currentobj.SetActive(true);
            //currentobj.GetComponent<TypeUI>().SetValue(variable);
            lastobj = currentobj;
        }
        currentobj.GetComponent<TypeUI>().SetValue(variable);
    }
}

