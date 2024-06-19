using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetVisibility : Action<Main>
{
	public GameObject target;
	public string targetName;
	public bool isVisible=true;
    public bool isActive;

	public override void DoAction(Main m)
	{
		if (target == null)
        {
			if (string.IsNullOrEmpty (targetName))
            {
				target = m.gameObject;
			} else
            {
                target = Manager.Instace.parent.Find(targetName).gameObject;
            }
		}
	    if (isVisible)
	    {
	        if (target!=null)
	        {
	            target.SetActive(true);

	            MeshRenderer[] renders = target.GetComponentsInChildren<MeshRenderer>();
	            foreach (MeshRenderer item in renders)
	            {
	                item.enabled = true;
	            }
	            SkinnedMeshRenderer[] skinneds = target.GetComponentsInChildren<SkinnedMeshRenderer>();
	            foreach (SkinnedMeshRenderer item in skinneds)
	            {
	                item.enabled = true;
	            }
	            if (!target.name.Contains("cube"))
	            {
	                Collider[] colliders = target.GetComponentsInChildren<Collider>();
	                foreach (Collider item in colliders)
	                {
	                    item.enabled = true;
	                }
	            }
            }
	        else
	        {
	            Debug.LogError("没有找到目标物体："+ targetName);
	        }
	    }
	    else
	    {
	        if (isActive)
	        {
	            target.SetActive(isVisible);
	        }
	        else
	        {
	            MeshRenderer[] renders = target.GetComponentsInChildren<MeshRenderer>();
	            foreach (MeshRenderer item in renders)
	            {
	                item.enabled = isVisible;
	            }
	            SkinnedMeshRenderer[] skinneds = target.GetComponentsInChildren<SkinnedMeshRenderer>();
	            foreach (SkinnedMeshRenderer item in skinneds)
	            {
	                item.enabled = isVisible;
	            }
	            if (!target.name.Contains("cube"))
	            {
	                Collider[] colliders = target.GetComponentsInChildren<Collider>();
	                foreach (Collider item in colliders)
	                {
	                    item.enabled = isVisible;
	                }
	            }
	        }
        }
	    
		
//			if (renders.Length > 0||skinneds.Length>0||colliders.Length>0) {
//				isComplete = true;Debug.Log ("iscomplete is true");
//			}
	}
}
