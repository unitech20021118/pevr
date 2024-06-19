using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class ChangeColorUI :ActionUI {
	public GameObject target,lastTarget;
	public Text targetText;
	public string targetName;
    public Image image;
	public InputField time;

	public Dropdown dropdown;
	private List<string> optionList;
	private List<string> targetPath;
	private string targetNamePath,rootName;

    public override Action<Main> CreateAction()
    {
        action = new ChangeColor();
        actionInforma = new ChangeColorInforma(true);
        GetStateInfo().actionList.Add(actionInforma);
        actionInforma.name = "ChangeColor";
        return base.CreateAction();
    }

    public override Action<Main> LoadAction(ActionInforma a)
    {
        ChangeColorInforma changeColorInforma = (ChangeColorInforma)a;
        this.actionInforma = a;
        action = new ChangeColor(changeColorInforma.color);
		ChangeColor changeColor = (ChangeColor)action;
		changeColor.targetName = changeColorInforma.targetName;
		rootName = changeColorInforma.rootName;
		//targetText.text = changeColorInforma.targetName;
        //Debug.LogError(changeColorInforma.targetName);
//		if (string.IsNullOrEmpty (changeColorInforma.targetName)) {
//			string[] shortName = changeColorInforma.targetName.Split ('/');
//			if (shortName.Length > 0)
//				targetText.text = shortName [shortName.Length - 1];
//		}

		//time.text = changeColorInforma.time.ToString ();
		//changeColor.time = changeColorInforma.time;
		
        if (changeColorInforma.color != null)
        {
            image.color = Manager.Instace.GetColor(changeColorInforma.color);
        }
        //更新ui界面的显示         edit by kuai  
        if (changeColorInforma.targetName!=null)
        {
            targetText.text = changeColorInforma.targetName;
        }
        
        return base.LoadAction(a);
    }

    public void SetColor()
    {
        
        
        Manager.Instace.ColorPicker.SetActive(true);
        Manager.Instace.ColorPicker.GetComponent<ColorPickerUI>().image = image;
        ChangeColor cc = (ChangeColor)action;

        //Manager.Instace.gonggong.GetComponent<MeshRenderer>().material.color = Color.red;
        Manager.Instace.ColorPicker.GetComponent<ColorPickerUI>().SetCurrentColor(cc,actionInforma);
        

    }

	public void UpdateTarget(){
		ChangeColor cc = (ChangeColor)action;
		cc.target = target;
//		if (string.IsNullOrEmpty (targetName)) {
//			string[] shortName = targetName.Split ('/');
//			if (shortName.Length > 0)
//				targetText.text = shortName [shortName.Length - 1];
//		}
		try{
			ChangeColorInforma changeColorInfo=(ChangeColorInforma)actionInforma;
			changeColorInfo.targetName = targetName;
            
			//cc.time=float.Parse(time.text);
			//changeColorInfo.time=float.Parse(time.text);
			targetNamePath = rootName;

		    if (targetPath!=null)
		    {
		        foreach (string item in targetPath)
		        {
		            targetNamePath = targetNamePath.Insert(targetNamePath.Length, "/");
		            targetNamePath = targetNamePath.Insert(targetNamePath.Length, item);

		        }
            }
            changeColorInfo.targetName = targetNamePath;
			changeColorInfo.rootName = rootName;
            //更新ui界面的显示
		    targetText.text = changeColorInfo.targetName;
		}catch(System.Exception e)
        {
            Debug.LogError(e);
		}
	}

	public void SelectChildTarget(int index){
		if(index>0)
			target = target.transform.GetChild (index - 1).gameObject;
		dropdown.ClearOptions ();
		targetName = target.name;
		if (targetPath == null) {
			targetPath = new List<string> ();
		}
		targetPath.Add (targetName);
		UpdateTarget ();
	}

	public void InitialChildSelector(){
		if (target == null) {
			target = Manager.Instace.gonggong;
			rootName = target.name;
		}
		if (optionList == null) {
			optionList = new List<string> ();
		}
		optionList.Clear ();
		optionList.Add ("选择子物体");
		foreach (Transform item in target.transform) {
			optionList.Add (item.name);
		}
		dropdown.ClearOptions ();
		dropdown.AddOptions (optionList);
	}

	public void BackToParent(){
		if (target == null) {
			target = Manager.Instace.gonggong;
		} else {
			target = target.transform.parent.gameObject;
		}
		targetName = target.name;
		if (targetPath == null) {
			targetPath = new List<string> ();
		}
		if (targetPath.Count > 0) {
			targetPath.RemoveAt (targetPath.Count - 1);
		}
		//InitialChildSelector ();
		dropdown.ClearOptions ();
		//UpdateInput();
	}

	public void SetGameObject(){
//		if (item.isDragging) {
//			target = item.dragedItem.GetTarget ();
//			targetName = target.name;
////			UpdateTarget ();
//		}
	}

	public void ReturnGameObject(){

	}

	public void DropGameObject(){
		if (item.isDragging)
        {
			lastTarget =  item.dragedItem.GetTarget ();
            target = item.dragedItem.GetTarget();
			if(target)
				rootName = target.name;
            targetName = target.name;

			//targetText.text = targetName;
			UpdateTarget ();
		}
	}
}
