using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using VRTK;

public class FpersonVR : MonoBehaviour {
    private VRTK_ControllerEvents lctrlEvents;//控制器事件脚本
    private VRTK_ControllerEvents rctrlEvents;//控制器事件脚本
    public Vector2 walkaxis;
    public Vector3 m_Move;
    public bool m_Jump;
    public bool m_Crouch;

    Animator ani;
    private ThirdPersonCharacter m_Character;
    void Start () {
        m_Character = GetComponent<ThirdPersonCharacter>();
        ani = GetComponent<Animator>();
        ani.runtimeAnimatorController = Manager.Instace.Runimatorcontroller;
        lctrlEvents = Manager.Instace.ctrllerEventsL;
        rctrlEvents = Manager.Instace.ctrllerEventsR;
        lctrlEvents.TouchpadPressed += new ControllerInteractionEventHandler(RobotWalk);//触摸板被按下
        lctrlEvents.GripPressed += new ControllerInteractionEventHandler(RobotCrouch);//测键被按下
        lctrlEvents.TouchpadReleased += new ControllerInteractionEventHandler(RobotnoWalk);//抬起
        lctrlEvents.GripReleased += new ControllerInteractionEventHandler(RobotnoCrouch);//测键被抬起
    }
	
	// Update is called once per frame
	void Update () {
        
        if(!m_Jump)
        {
            m_Jump = lctrlEvents.triggerClicked;
        }     
    }
    void FixedUpdate()
    {
        walkaxis = lctrlEvents.GetTouchpadAxis();
        print(m_Move);
        m_Character.Move(m_Move, m_Crouch, m_Jump);
    }
        void RobotWalk(object sender, ControllerInteractionEventArgs e)
    {
        m_Move = transform.right.normalized * walkaxis.x + transform.forward.normalized * walkaxis.y;
        //m_Character.Move(m_Move, m_Crouch, m_Jump);
        m_Jump = false;
    }
    void RobotnoWalk(object sender, ControllerInteractionEventArgs e)
    {
        m_Move = Vector3.zero;
    }
    void RobotCrouch(object sender, ControllerInteractionEventArgs e)
    {
        m_Crouch = true;
    }
    void RobotnoCrouch(object sender, ControllerInteractionEventArgs e)
    {
        m_Crouch = false;
    }
}
