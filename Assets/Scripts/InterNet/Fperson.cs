using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityStandardAssets.CrossPlatformInput;

public class Fperson : MonoBehaviour
{
    //2019-11-25-wzy
    Animator ani;
    public int isWalk = 0;
    //public RuntimeAnimatorController Ranimatorcontroller;

    public Vector3 m_Move;
    public bool m_Jump;
    public bool m_Crouch;
    private ThirdPersonCharacter m_Character;
    // Use this for initialization
    void Start()
    {
        ani = GetComponent<Animator>();
        ani.runtimeAnimatorController = Manager.Instace.Runimatorcontroller;
        m_Character = GetComponent<ThirdPersonCharacter>();
    }
    void Update()
    {
        if (!m_Jump)
        {
            m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
        }
        if(m_Crouch)
        {
            Manager.Instace.FirstPerson.transform.localPosition = new Vector3(0, -0.85f, 0.25f);
        }
        if (!m_Crouch)
        {
            Manager.Instace.FirstPerson.transform.localPosition = Vector3.zero;
        }

    }
    void FixedUpdate()
    {
        float h = CrossPlatformInputManager.GetAxis("Horizontal");
        float v = CrossPlatformInputManager.GetAxis("Vertical");
        m_Crouch = Input.GetKey(KeyCode.C);       
        m_Move = transform.right.normalized * h * 0.5f + transform.forward.normalized * v * 0.5f;
        if (Input.GetKey(KeyCode.LeftShift))
            m_Move *= 2f;
        m_Character.Move(m_Move, m_Crouch, m_Jump);
        m_Jump = false;
    }
    
}
