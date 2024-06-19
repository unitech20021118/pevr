using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

public class SetResolution : MonoBehaviour
{

    private string SetUpKey = "SetUpKey";

    private SetUpValue setUpValue;
    // Use this for initialization
    void Start()
    {
        try
        {
            setUpValue = JsonMapper.ToObject<SetUpValue>(PlayerPrefs.GetString(SetUpKey));
            if (setUpValue==null)
            {
                Screen.SetResolution(1920, 1080, false);
            }
            else
            {
                switch (setUpValue.ResolvingDropdownValue)
                {
                    case 0:
                        Screen.SetResolution(1024, 576, setUpValue.FullScreenToggleValue);
                        break;
                    case 1:
                        Screen.SetResolution(1280, 720, setUpValue.FullScreenToggleValue);
                        break;
                    case 2:
                        Screen.SetResolution(1366, 768, setUpValue.FullScreenToggleValue);
                        break;
                    case 3:
                        Screen.SetResolution(1920, 1080, setUpValue.FullScreenToggleValue);
                        break;
                    case 4:
                        Screen.SetResolution(2560, 1440, setUpValue.FullScreenToggleValue);
                        break;
                    default:
                        Screen.SetResolution(1920, 1080, false);
                        break;
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
