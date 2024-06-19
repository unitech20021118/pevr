using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRTK;

public class PhysicalSetting : Action<Main> {
    /// <summary>
    /// 质量
    /// </summary>
	public float massNum=1;
    /// <summary>
    /// 是否受重力
    /// </summary>
	public bool isGravity;
    /// <summary>
    /// 摩擦系数
    /// </summary>
	public float factor=0;
    //public bool isBoxCollider = true;
	public VRTK_InteractableObject interactObj;
	Transform handL,handR;

	public override void DoAction(Main m)
	{
		Rigidbody rig=m.gameObject.GetComponent<Rigidbody> ();
        
		if (rig == null) {
			rig = m.gameObject.AddComponent<Rigidbody> ();
		}

        /*
        //不知道什么作用
        interactObj = m.gameObject.GetComponent<VRTK_InteractableObject>();
        if (interactObj == null)
        {
            interactObj = m.gameObject.AddComponent<VRTK_InteractableObject>();
        }
        if (handL == null && handR == null)
        {
            handL = Manager.Instace.ctrllerEventsL.transform;
            handR = Manager.Instace.ctrllerEventsR.transform;
        }
        if (interactObj != null)
        {
            handR.GetComponent<HandForceCtrl>().handCollider.SetActive(true);
            handL.GetComponent<HandForceCtrl>().handCollider.SetActive(true);
            if (interactObj.IsTouched())
            {
                handR.GetComponent<HandForceCtrl>().SetCollider(true);
                handL.GetComponent<HandForceCtrl>().SetCollider(true);
                if (handR.GetComponent<HandForceCtrl>().forceValue > factor * massNum)
                {
                    rig.isKinematic = false;
                }
                else
                {
                    rig.isKinematic = true;
                }
            }
            else
            {
                handR.GetComponent<HandForceCtrl>().SetCollider(false);
                handL.GetComponent<HandForceCtrl>().SetCollider(false);
            }
        }
        */
        rig.mass = massNum;
	    rig.drag = factor;
		rig.useGravity = isGravity;
        rig.isKinematic = false;
        //if (m.gameObject.GetComponent<Collider>()!=null)
        //{
        //    if (isBoxCollider==false)
        //    {
        //        m.gameObject.GetComponent<Collider>().enabled = false;
        //    }
        //    else if (isBoxCollider==true)
        //    {
        //        m.gameObject.GetComponent<Collider>().enabled = true;
        //    }
        //}
        //if (m.gameObject.GetComponent<Collider>()==null)
        //{
        //    if (isBoxCollider==true)
        //    {
        //        m.gameObject.AddComponent<BoxCollider>();
        //    }
        //}
	}
}
