using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DeleteObjUI : ActionUI {
    /// <summary>
    /// 目标物体
    /// </summary>
    public GameObject target;
    /// <summary>
    /// 目标的名称
    /// </summary>
    public string targetName;
    /// <summary>
    /// UI显示目标物体的名称的文本
    /// </summary>
    public Text targetText;
    //private List<string> targetPath;
    public bool isloading;
    DeleteObj deleteObj;
    DeleteObjInforma delInformal;
    
    private string targetNamePath, rootName;
	public override Action<Main> CreateAction()
	{
		action = new DeleteObj();
		DeleteObj transObj = (DeleteObj)action;
		actionInforma = new DeleteObjInforma (false);
		GetStateInfo().actionList.Add(actionInforma);
		actionInforma.name = "DeleteObj";
		return base.CreateAction();
	}

	public override Action<Main> LoadAction (ActionInforma actionInforma)
    {
        delInformal = (DeleteObjInforma)actionInforma;
        this.actionInforma = actionInforma;
        action = new DeleteObj();
        deleteObj = (DeleteObj)action;
        isloading = true;
        targetName = delInformal.targetName;
        rootName = delInformal.rootName;
        deleteObj.targetName = delInformal.targetName;

        if (!string.IsNullOrEmpty(delInformal.targetName))
        {
            string[] shortName = delInformal.targetName.Split('/');
            if (shortName.Length>0)
            {
                
                targetText.text = shortName[shortName.Length - 1];
                
            }
            
        }
        isloading = false;
        
        
        
        
        return action;
	}


    public void UpdateInput()
    {
        if (!isloading)
        {

            deleteObj = (DeleteObj)action;
            deleteObj.target = target;
            //deleteObj.targetName=
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
            //targetText.text = targetName;
            //		if (target.Equals (null)) {
            //			targetText.text = "拖拽目标至框内";
            //		}
            try
            {
                delInformal = (DeleteObjInforma)actionInforma;
                //string targetNamePath=target.name;
                targetNamePath = rootName;
                //if (targetPath != null)
                //{
                //    foreach (string item in targetPath)
                //    {
                //        targetNamePath = targetNamePath.Insert(targetNamePath.Length, "/");
                //        targetNamePath = targetNamePath.Insert(targetNamePath.Length, item);
                //    }
                //}
                if (!string.IsNullOrEmpty(targetNamePath))
                {
                    delInformal.targetName = targetNamePath;
                    delInformal.rootName = rootName;
                }
                else
                {
                    delInformal.targetName = target.name;
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
    }


    public void SetGameObject()
    {
        //		if (item.isDragging) {
        //			target = item.dragedItem.GetTarget ();
        //			targetName = target.name;
        //			UpdateInput ();
        //		}
    }

    public void ReturnGameObject()
    {
        //		if (item.isDragging) {
        //			target = lastTarget;
        //			if (lastTarget == null) {
        //				targetName = "拖拽目标至框内";
        //			} else {
        //				targetName = lastTarget.name;
        //			}
        //			UpdateInput ();
        //		}
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
}
