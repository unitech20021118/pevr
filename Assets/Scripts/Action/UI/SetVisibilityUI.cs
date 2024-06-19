using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HighlightingSystem;
using System;

public class SetVisibilityUI : ActionUI
{
    public GameObject target;
    public string targetName;
    public Text targetText;
    public Toggle isVisible;
    public Toggle activeToggle;
    public Dropdown dropdown;
    private List<string> optionList;
    private List<string> targetPath;
    private string targetNamePath, rootName;
    SetVisibility setVisibility;
    SetVisibilityInforma setVInforma;
    GameObject preObj;
    bool isloading;

    public override Action<Main> CreateAction()
    {
        action = new SetVisibility();
        actionInforma = new SetVisibilityInforma(true);
        setVisibility = (SetVisibility) action;
        setVInforma = (SetVisibilityInforma) actionInforma;
        GetStateInfo().actionList.Add(actionInforma);
        actionInforma.name = "SetVisibility";
        return base.CreateAction();
    }

    public override Action<Main> LoadAction(ActionInforma actionInforma)
    {
        setVInforma = (SetVisibilityInforma)actionInforma;

        this.actionInforma = actionInforma;
        action = new SetVisibility();
        setVisibility = (SetVisibility)action;
        isloading = true;
        targetName = setVInforma.targetName;
        rootName = setVInforma.rootName;

        if (setVInforma.isVisible == 0)
        {
            isVisible.isOn = false;
        }
        else if (setVInforma.isVisible == 1)
        {
            isVisible.isOn = true;
           
        }
        else if(setVInforma.isVisible == -1)
        {
            isVisible.isOn = false;
            activeToggle.isOn = true;
        }

        setVisibility.targetName = setVInforma.targetName;


        print(setVInforma.targetName);
        if (!string.IsNullOrEmpty(setVInforma.targetName))
        {
            string[] shortName = setVInforma.targetName.Split('/');
            if (shortName.Length > 0)
                targetText.text = shortName[shortName.Length - 1];
        }
        setVisibility.isVisible = isVisible.isOn;

        isloading = false;
        return action;
    }


    public void UpdateInput()
    {
        if (!isloading)
        {

            setVisibility = (SetVisibility)action;
            setVisibility.isVisible = isVisible.isOn;
            setVisibility.target = target;

            if (target && !target.GetComponent<ConstantHighlighting>())
            {
                target.AddComponent<ConstantHighlighting>();
            }

            if (!string.IsNullOrEmpty(targetName))
            {
                string[] shortName = targetName.Split('/');
                if (shortName.Length > 0)
                    targetText.text = shortName[shortName.Length - 1];
            }
            try
            {
                setVInforma = (SetVisibilityInforma)actionInforma;
                if (isVisible.isOn)
                {
                    setVInforma.isVisible = 1;
                }
                else
                {
                    setVInforma.isVisible = 0;
                }
                targetNamePath = rootName;
                if (targetPath != null)
                {
                    foreach (string item in targetPath)
                    {
                        targetNamePath = targetNamePath.Insert(targetNamePath.Length, "/");
                        targetNamePath = targetNamePath.Insert(targetNamePath.Length, item);
                    }
                }
                if (!string.IsNullOrEmpty(targetNamePath))
                {
                    setVInforma.targetName = targetNamePath;
                    setVInforma.rootName = rootName;
                }
                else
                {
                    setVInforma.targetName = target.name;
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
    }

    public void SelectChildTarget(int index)
    {
        if (preObj != null)
        {
            Destroy(preObj.GetComponent<ConstantHighlighting>());
            Destroy(preObj.GetComponent<Highlighter>());
        }
        if (index > 0)
            target = target.transform.GetChild(index - 1).gameObject;
        preObj = target;
        dropdown.ClearOptions();
        targetName = target.name;
        if (targetPath == null)
        {
            targetPath = new List<string>();
        }
        targetPath.Add(targetName);
        UpdateInput();
    }

    public void InitialChildSelector()
    {
        if (target == null)
        {
            target = Manager.Instace.gonggong;
            rootName = target.name;
        }
        if (optionList == null)
        {
            optionList = new List<string>();
        }
        optionList.Clear();
        optionList.Add("选择子物体");
        foreach (Transform item in target.transform)
        {
            optionList.Add(item.name);
        }
        dropdown.ClearOptions();
        dropdown.AddOptions(optionList);
    }

    public void BackToParent()
    {
        if (target == null)
        {
            target = Manager.Instace.gonggong;
        }
        else
        {
            target = target.transform.parent.gameObject;
        }
        targetName = target.name;
        if (targetPath == null)
        {
            targetPath = new List<string>();
        }
        if (targetPath.Count > 0)
        {
            targetPath.RemoveAt(targetPath.Count - 1);
        }
        dropdown.ClearOptions();
        UpdateInput();
    }

    public void SetGameObject()
    {

    }

    public void ReturnGameObject()
    {

    }

    public void DropGameObject()
    {
        if (item.isDragging)
        {
            target = item.dragedItem.GetTarget();
            rootName = target.name;
            targetName = target.name;
            UpdateInput();
        }
    }

    public void ChangeVisibility()
    {
        if (isVisible.isOn)
        {
            activeToggle.gameObject.SetActive(false);
        }
        else
        {
            activeToggle.gameObject.SetActive(true);
        }
    }
    /// <summary>
    /// 选择不可见的方式
    /// </summary>
    public void SelectModel()
    {
        if (isVisible.isOn == false)
        {
            if (activeToggle.isOn==false)
            {
                setVInforma.isVisible = 0;
                setVisibility.isActive = false;
            }
            else
            {
                setVInforma.isVisible = -1;
                setVisibility.isActive = true;
                
            }
        }
    }
}
