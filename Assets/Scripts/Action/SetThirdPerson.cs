using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class SetThirdPerson :Action<Main> {

    //edit by 王梓亦
    public string num;
    public string task;
    public bool isNet;

    public override void DoAction(Main m)
    {
        //edit by 王梓亦
        if (isNet)
        {
            //edit by 王梓亦
            Manager.Instace.Substance = m.gameObject;
            if (Manager.Instace.GetCurrentRoleType() == num)
            {
                Manager.Instace.SetCurrentRole(m.gameObject);
                MoveRequest moveRequest = Manager.Instace.gameObject.GetComponent<MoveRequest>();
                moveRequest.SetLocalPlayer(m.transform);               
                moveRequest.enabled = true;

                m.gameObject.GetComponent<Rigidbody>().useGravity = true;
                m.gameObject.GetComponent<Rigidbody>().isKinematic = false;
                m.gameObject.GetComponent<Animator>().enabled = true;
                m.gameObject.AddComponent<ThirdPersonCtr>();            
                m.gameObject.AddComponent<ThirdPersonCharacter>();

                m.gameObject.AddComponent<GameObjectId>().TransformName = num;
                Manager.Instace.playerMng.transformDict.Add(num, m.transform);
            }
        }
        else
        {
            m.gameObject.GetComponent<Rigidbody>().useGravity = true;
            m.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            m.gameObject.GetComponent<Animator>().enabled = true;
            m.gameObject.AddComponent<ThirdPersonUserControl>();
            
            m.gameObject.AddComponent<ThirdPersonCharacter>();
          
        }
        //m.gameObject.AddComponent<ThirdPersonCharacter>();


       // Manager.Instace.Substance = m.gameObject;
       // Manager.Instace.ThirdPerson.SetActive(true);
       // Manager.Instace.ThirdPerson.transform.position = Manager.Instace.Substance.transform.position;
       //// Manager.Instace.ThirdPerson.GetComponent<Animator>().avatar = m.gameObject.GetComponent<Animator>().avatar;
       // m.gameObject.GetComponent<Animator>().enabled = false;
       // //foreach (Transform child in m.transform)
        //{
        //    child.transform.parent = Manager.Instace.ThirdPerson.transform;
        //}
    }

    public SetThirdPerson()
    {
        SetSituation();
    }
    
}
