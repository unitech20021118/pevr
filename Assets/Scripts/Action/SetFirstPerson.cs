using System.Collections;
using System.Collections.Generic;
using HighlightingSystem;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.UI;
using System;

public class SetFirstPerson :Action<Main>{
    /// <summary>
    /// 步行速度
    /// </summary>
    public float walkSpeed;
    /// <summary>
    /// 奔跑速度
    /// </summary>
    public float runSpeed;
    /// <summary>
    /// 跳跃速度（高度）
    /// </summary>
    public float jumpSpeed;
    /// <summary>
    /// 第一人称高度
    /// </summary>
    public float fpsHeight;

    //edit by 吕存全
    public string num;
    public string task;
    public bool isNet;
    //-----------
    //2019-12-12-wzy
    public Dictionary<string, bool> persontaskDic = new Dictionary<string, bool>();

    public override void DoAction(Main m)
    {
        Manager.Instace.isnet = isNet;
        Debug.Log(Manager.Instace.isnet);
        //edit by 吕存全
        if (isNet)
        {

            if (Manager.Instace.GetCurrentRoleType() == num)
            {
                Manager.Instace.Substance = m.gameObject;
                //edit by 王梓亦
                if (VRSwitch.isVR)
                {
                    //---------------------------------------------------
                    Canvas mainCanvas = Manager.Instace.GetComponent<Canvas>();
                    mainCanvas.renderMode = RenderMode.WorldSpace;
                    Transform camHead = Manager.Instace.VRCamera.transform.GetChild(2);
                    mainCanvas.transform.parent = camHead;
                    mainCanvas.transform.localScale = Vector3.one * 0.0148f;
                    mainCanvas.transform.rotation = Quaternion.identity;
                    mainCanvas.transform.localPosition = new Vector3(-0.48f, 0, 6);

                    //VRSwitch.SetVR(true);
                    //Manager.Instace.VRCamera.SetActive(true);
                    //Manager.Instace.VRCamera.transform.position = Manager.Instace.Substance.transform.position;
                    //设置第一人称的朝向（旋转）
                    //Manager.Instace.VRCamera.transform.rotation = Manager.Instace.Substance.transform.rotation;
                    //---------------------------------------------------
                    Manager.Instace.SetCurrentRole(m.gameObject);
                    //VRMoveRequest moveRequest = Manager.Instace.gameObject.GetComponent<VRMoveRequest>();
                    MoveRequest moveRequest = Manager.Instace.gameObject.GetComponent<MoveRequest>();
                    moveRequest.SetLocalPlayer(m.transform);
                    //AnimatorRequest arRequest = Manager.Instace.gameObject.GetComponent<AnimatorRequest>();
                    //arRequest.SetLocalPlayer(m.transform);
                    //arRequest.isnet = true;
                    moveRequest.enabled = true;
                    //arRequest.enabled = true;//动画同步

                    
                }
                else
                {
                    Manager.Instace.SetCurrentRole(m.gameObject);
                    MoveRequest moveRequest = Manager.Instace.gameObject.GetComponent<MoveRequest>();
                    moveRequest.SetLocalPlayer(m.transform);
                    //AnimatorRequest arRequest = Manager.Instace.gameObject.GetComponent<AnimatorRequest>();
                    //arRequest.SetLocalPlayer(m.transform);
                    //arRequest.isnet = true;
                    moveRequest.enabled = true;
                    //arRequest.enabled = true;//动画同步
                }

            //Manager.Instace.Substance.SetActive(false);
            }
            m.gameObject.AddComponent<GameObjectId>().TransformId = GameObjectId.GetTransformId();
            m.gameObject.GetComponent<GameObjectId>().TransformName = num;
            //m.gameObject.AddComponent<ThirdPersonUserControl>();
            //m.gameObject.AddComponent<ThirdPersonCtr>();
            //Debug.Log(num + "    " + m.transform);
            Manager.Instace.playerMng.transformDict.Add(num, m.transform);

        }
        else
        {
        //-----------
            Manager.Instace.Substance = m.gameObject;
		    if (VRSwitch.isVR) {
			    //---------------------------------------------------
			    Canvas mainCanvas = Manager.Instace.GetComponent<Canvas> ();
			    mainCanvas.renderMode = RenderMode.WorldSpace;
			    Transform camHead = Manager.Instace.VRCamera.transform.GetChild (2);
			    mainCanvas.transform.parent = camHead;
			    mainCanvas.transform.localScale = Vector3.one * 0.0148f;
			    mainCanvas.transform.rotation = Quaternion.identity;
			    mainCanvas.transform.localPosition = new Vector3 (-0.48f, 0, 6);

			    VRSwitch.SetVR (true);
			    Manager.Instace.VRCamera.SetActive (true);
			    Manager.Instace.VRCamera.transform.position=Manager.Instace.Substance.transform.position;
                //设置第一人称的朝向（旋转）
                Manager.Instace.VRCamera.transform.rotation = Manager.Instace.Substance.transform.rotation;
                //edit by kuai
                //等待三秒后给 vr相机添加用于观察边缘发光物体的脚本
		        //Manager.Instace.StartAddComoent();
		        //---------------------------------------------------
		    } else {
                Manager.Instace.FirstPerson.SetActive(true);
                //设置第一人称的坐标
			    Manager.Instace.FirstPerson.transform.position = Manager.Instace.Substance.transform.position+new Vector3(0,1,0);
                //设置第一人称的朝向（旋转）
                Manager.Instace.FirstPerson.transform.rotation = Manager.Instace.Substance.transform.rotation;
                Manager.Instace.FirstPerson.GetComponent<FirstPersonController>().SetWalkSpeed(walkSpeed);
                Manager.Instace.FirstPerson.GetComponent<FirstPersonController>().SetRunSpeed(runSpeed); 
                Manager.Instace.FirstPerson.GetComponent<FirstPersonController>().SetJumpSpeed(jumpSpeed);
                Manager.Instace.FirstPerson.transform.localScale=new Vector3(1,fpsHeight,1);
            
            }
            Manager.Instace.Substance.SetActive(false);
        }
    }
    public SetFirstPerson()
    {
        id = 4;
    }
}
