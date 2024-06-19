using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using VRTK;

public class VRSwitch : MonoBehaviour {
    public static VRSwitch Instance;
    public Camera MainCam;
    public SteamVR_Camera vrCam;
    public GameObject firstCam,VRFirstCam;
    public static VRTK_UIPointer uiPointer;
    public static bool isVR;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UnityEngine.VR.VRSettings.enabled = false;
        MainCam.fieldOfView = 60;
        uiPointer = Manager.Instace.ctrllerEventsR.GetComponent<VRTK_UIPointer>();
        ////Instantiate<GameObject>(vrPre);
        //SteamVR.enabled = true;
        //SteamVR vr = SteamVR.instance;
        //SteamVR_Render render = SteamVR_Render.instance;
        //render.enabled = true;
        //vrCam.enabled = true;
    }

    public static void SetVR(bool enable)
    {
        UnityEngine.VR.VRSettings.enabled = enable;
        //Instantiate<GameObject>(vrPre);
        SteamVR.enabled = true;
        SteamVR vr = SteamVR.instance;
        SteamVR_Render render = SteamVR_Render.instance;
        render.enabled = true;
//		Manager.Instace.editorCamera.rect = new Rect (0, 0, 0, 0);
//		Manager.Instace.stateMachineCamera.rect=new Rect(0,0,0,0);
        uiPointer.enabled = enable;
        //vrCam.enabled = true;
    }

    public void SetVRState(bool state)
    {
        isVR = state;

    }
}
