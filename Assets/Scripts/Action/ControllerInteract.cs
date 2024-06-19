using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using VRTK.GrabAttachMechanics;

public class ControllerInteract : Action<Main>
{
    public enum InteractType
    {
        touch, grab, untouch, ungrab, moveThrough
    }

    public enum CurrentState
    {
        nostate, touched, untouched, grabed, ungrabed
    }
    public InteractType type;
    MotionSensor motionSensor;
    static CurrentState cState;
    
    //private bool isTouched;
    Transform a;
    VRTK_InteractableObject interactObj;
    VRTK_FixedJointGrabAttach fixJoint;
    /// <summary>
    /// 右控制器的脚本
    /// </summary>
    private VRTK_InteractTouch rightInteractTouch;

    private Main m;
    public override void DoAction(Main m)
    {
        this.m = m;
        a = even.target.gameObject.transform;
        //a = even.target.gameObject.transform;
        //if (a != null)
        //{
        //    switch (type)
        //    {
        //        case InteractType.touch:
        //            if (cState == CurrentState.touched)
        //            {
        //                if (rightInteractTouch.GetTouchedGameObjects()!=null&& rightInteractTouch.GetTouchedGameObjects().Count>0)
        //                {
        //                    for (int i = 0; i < rightInteractTouch.GetTouchedGameObjects().Count; i++)
        //                    {
        //                        if (rightInteractTouch.GetTouchedGameObjects()[i]==m.gameObject)
        //                        {
        //                            even.Do();
        //                            cState = CurrentState.nostate;
        //                        }
        //                    }
        //                }

        //                //if (rightInteractTouch.GetTouchedObject() == m.gameObject||leftInteractTouch.GetTouchedObject()==m.gameObject)
        //                //{
        //                //    even.Do();
        //                //    cState = CurrentState.nostate;
        //                //}
        //            }
        //            break;
        //        case InteractType.grab:
        //            if (cState == CurrentState.grabed && rightInteractTouch.GetTouchedObject() == m.gameObject)
        //            {
        //                even.Do();
        //                cState = CurrentState.nostate;

        //            }
        //            break;
        //        case InteractType.untouch:
        //            if (cState == CurrentState.untouched&&rightInteractTouch.GetUnTouchedGameObject()==m.gameObject)
        //            {
        //                even.Do();
        //                cState = CurrentState.nostate;

        //            }//added by kuai
        //            else if(cState==CurrentState.touched)
        //            {
        //                if (rightInteractTouch.GetUnTouchedGameObject()==m.gameObject)
        //                {
        //                    if (rightInteractTouch.GetTouchedGameObjects()!=null&&rightInteractTouch.GetTouchedGameObjects().Count>0)
        //                    {
        //                        for (int i = 0; i < rightInteractTouch.GetTouchedGameObjects().Count; i++)
        //                        {
        //                            if (rightInteractTouch.GetTouchedGameObjects()[i]==m.gameObject)
        //                            {
        //                                return;
        //                            }
        //                        }
        //                        even.Do();
        //                        cState = CurrentState.nostate;
        //                    }
        //                }
        //            }
        //            else if(cState == CurrentState.nostate)
        //            {
        //                if (rightInteractTouch.GetTouchedGameObjects()!=null&&rightInteractTouch.GetTouchedGameObjects().Count==0)
        //                {
        //                    even.Do();
        //                    cState = CurrentState.nostate;
        //                }
        //            }
        //            break;
        //        case InteractType.ungrab:
        //            Debug.LogError(cState);
        //            if (cState == CurrentState.ungrabed)
        //            {
        //                even.Do();
        //                cState = CurrentState.nostate;
        //                Debug.LogError(cState);
        //            }
        //            break;
        //        case InteractType.moveThrough:
        //            if (motionSensor != null && motionSensor.motion == Motion.moveThrough)
        //            {
        //                even.Do();
        //            }
        //            break;
        //        default:
        //            break;
        //    }
        //}
    }

    public void Ini()
    {
        if (Manager.Instace.gonggong != null)
        {
            GameObject currentObj = Manager.Instace.gonggong;
            if (!motionSensor)
            {
                motionSensor = currentObj.GetComponentInChildren<MotionSensor>();
            }

            interactObj = currentObj.gameObject.GetComponent<VRTK_InteractableObject>();
            if (interactObj==null)
            {
                interactObj = currentObj.gameObject.AddComponent<VRTK_InteractableObject>();
            }
            interactObj.grabOverrideButton = VRTK_ControllerEvents.ButtonAlias.Trigger_Press;
            interactObj.isGrabbable = true;
            interactObj.isUsable = true;
            interactObj.InteractableObjectTouched += StartTouch;
            interactObj.InteractableObjectUntouched += StopTouch;
            interactObj.InteractableObjectGrabbed += StartGrab;
            interactObj.InteractableObjectUngrabbed += StopGrab;

            fixJoint = currentObj.gameObject.GetComponent<VRTK_FixedJointGrabAttach>();
            if (fixJoint==null)
            {
                fixJoint= currentObj.gameObject.AddComponent<VRTK_FixedJointGrabAttach>();
            }
            fixJoint.precisionGrab = true;
            fixJoint.onGrabCollisionDelay = 50;
            fixJoint.breakForce = float.MaxValue;
            //if (!interactObj)
            //{
            //    interactObj = currentObj.gameObject.AddComponent<VRTK_InteractableObject>();
            //    interactObj.grabOverrideButton = VRTK_ControllerEvents.ButtonAlias.Trigger_Press;
            //    interactObj.isGrabbable = true;
            //    interactObj.isUsable = true;
            //    interactObj.InteractableObjectTouched += StartTouch;
            //    interactObj.InteractableObjectUntouched += StopTouch;
            //    interactObj.InteractableObjectGrabbed += StartGrab;
            //    interactObj.InteractableObjectUngrabbed += StopGrab;
            //}
            //if (!fixJoint)
            //{
            //    fixJoint = currentObj.gameObject.AddComponent<VRTK_FixedJointGrabAttach>();
            //    fixJoint.precisionGrab = true;
            //    fixJoint.onGrabCollisionDelay = 50;
            //    fixJoint.breakForce = float.MaxValue;
            //}
            rightInteractTouch = Manager.Instace.RightControllerGameObject.GetComponent<VRTK_InteractTouch>();
        }
    }

    void StartTouch(object obj, InteractableObjectEventArgs e)
    {
        cState = CurrentState.touched;
        DoActionMe();
    }
    void StopTouch(object obj, InteractableObjectEventArgs e)
    {
        cState = CurrentState.untouched;
        DoActionMe();
    }

    void StartGrab(object obj, InteractableObjectEventArgs e)
    {
        cState = CurrentState.grabed;
        DoActionMe();
    }

    void StopGrab(object obj, InteractableObjectEventArgs e)
    {
        cState = CurrentState.ungrabed;
        DoActionMe();
    }

    public void SetType(InteractType interactType)
    {
        type = interactType;
    }

    public void DoActionMe()
    {
        if (a != null)
        {
            switch (type)
            {
                case InteractType.touch:
                    if (cState == CurrentState.touched)
                    {
                        if (rightInteractTouch.GetTouchedGameObjects() != null && rightInteractTouch.GetTouchedGameObjects().Count > 0)
                        {
                            for (int i = 0; i < rightInteractTouch.GetTouchedGameObjects().Count; i++)
                            {
                                if (rightInteractTouch.GetTouchedGameObjects()[i] == m.gameObject)
                                {
                                    even.Do();
                                    cState = CurrentState.nostate;
                                }
                            }
                        }

                        //if (rightInteractTouch.GetTouchedObject() == m.gameObject||leftInteractTouch.GetTouchedObject()==m.gameObject)
                        //{
                        //    even.Do();
                        //    cState = CurrentState.nostate;
                        //}
                    }
                    break;
                case InteractType.grab:
                    if (cState == CurrentState.grabed && rightInteractTouch.GetTouchedObject() == m.gameObject)
                    {
                        even.Do();
                        cState = CurrentState.nostate;
                    }
                    break;
                case InteractType.untouch:
                    if (cState == CurrentState.untouched && rightInteractTouch.GetUnTouchedGameObject() == m.gameObject)
                    {
                        even.Do();
                        cState = CurrentState.nostate;

                    }//added by kuai
                    else if (cState == CurrentState.touched)
                    {
                        if (rightInteractTouch.GetUnTouchedGameObject() == m.gameObject)
                        {
                            if (rightInteractTouch.GetTouchedGameObjects() != null && rightInteractTouch.GetTouchedGameObjects().Count > 0)
                            {
                                for (int i = 0; i < rightInteractTouch.GetTouchedGameObjects().Count; i++)
                                {
                                    if (rightInteractTouch.GetTouchedGameObjects()[i] == m.gameObject)
                                    {
                                        return;
                                    }
                                }
                                even.Do();
                                cState = CurrentState.nostate;
                            }
                        }
                    }
                    else if (cState == CurrentState.nostate)
                    {
                        if (rightInteractTouch.GetTouchedGameObjects() != null && rightInteractTouch.GetTouchedGameObjects().Count == 0)
                        {
                            even.Do();
                            cState = CurrentState.nostate;
                        }
                    }
                    break;
                case InteractType.ungrab:
                    if (cState == CurrentState.ungrabed)
                    {
                        even.Do();
                    }
                    break;
                case InteractType.moveThrough:
                    if (motionSensor != null && motionSensor.motion == Motion.moveThrough)
                    {
                        even.Do();
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
