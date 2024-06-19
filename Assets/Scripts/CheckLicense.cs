using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Net.NetworkInformation;
using UnityEngine.SceneManagement;
using System;
using LitJson;

public class CheckLicense : MonoBehaviour {

    private string activationKey = "activationKey";
    public bool ClearCode;
    private string machineCode;
    public bool NeedActivation;
    void Awake()
    {
        if (ClearCode)
        {
            PlayerPrefs.DeleteKey(activationKey);
        }
        //if (File.Exists (Application.dataPath + @"/license")) {
        //	StreamReader sr = File.OpenText (Application.dataPath + @"/license");
        //	NetworkInterface[] nis = NetworkInterface.GetAllNetworkInterfaces ();
        //	foreach (NetworkInterface item in nis) {
        //		if (item.NetworkInterfaceType == NetworkInterfaceType.Ethernet) {
        //                  if (sr.ReadToEnd().TrimEnd()==AESHelper.AesEncrypt(item.GetPhysicalAddress ().ToString (), "unitech-pevr-key")) {
        //				Debug.Log ("license checked");
        //				sr.Close ();
        //                  }
        //                  else
        //                  {
        //                      SceneManager.LoadScene("activation");
        //                  }
        //              }
        //	}
        //} else {
        //	SceneManager.LoadScene ("activation");
        //	Debug.Log ("No license");
        //}
        //CheckActivation();
    }

	// Use this for initialization
	void Start () {
		GetMachineCode();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    /// <summary>
    /// 检查激活状态
    /// </summary>
    public void CheckActivation()
    {
        if (!NeedActivation)
        {
            //进入主场景
            SceneManager.LoadScene("BBT");
        }
        else
        {
            if (!string.IsNullOrEmpty(PlayerPrefs.GetString(activationKey)))
            {
                ActivationCode activationCode;
                try
                {
                    activationCode = JsonMapper.ToObject<ActivationCode>(PlayerPrefs.GetString(activationKey));
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    activationCode = new ActivationCode();
                    activationCode.C = "";
                    activationCode.T = DateTime.Now;
                }
                if (activationCode.C == machineCode)
                {
                    if (activationCode.T > DateTime.Now)
                    {
                        //进入主场景
                        SceneManager.LoadScene("BBT");
                    }
                    else
                    {
                        //进入激活场景
                        SceneManager.LoadScene("activation");
                    }
                }
                else
                {
                    //进入激活场景
                    SceneManager.LoadScene("activation");
                }

            }
            else
            {
                //进入激活场景
                SceneManager.LoadScene("activation");
            }
        }
        
    }
    public void GetMachineCode()
    {
        NetworkInterface[] nis = NetworkInterface.GetAllNetworkInterfaces();
        foreach (NetworkInterface item in nis)
        {
            if (item.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
            {
                machineCode = item.GetPhysicalAddress().ToString();
                return;
            }
        }
    }
}
