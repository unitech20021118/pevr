using System;
using System.IO;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using LitJson;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LicenseCheck : MonoBehaviour {

    private string activationKey = "activationKey";
    public InputField MachineCodeInputField;
    public InputField ActivationCodeInputField;
    public InputField GetActivationCodeInputField;
    public InputField ActivationDaysInputField;
    public Button GetMachineCodeButton;
    public Button ActivationButton;
    public Button GetActivationCodeButton;
    public Text msg;
    
    /// <summary>
    /// 这台机器的机器码
    /// </summary>
    private string machineCode;

    void Awake()
    {

    }

    public void ActivationButtonClick()
    {
        ActivationCode activationCode =
            JsonMapper.ToObject<ActivationCode>(AESHelper.AesDecrypt(ActivationCodeInputField.text,
                "unitech-pevr-key"));
        if (activationCode.C == machineCode)
        {
            if (activationCode.T > DateTime.Now)
            {
                msg.text = "激活成功";
                string json = JsonMapper.ToJson(activationCode);
                PlayerPrefs.SetString(activationKey, json);
                SceneManager.LoadScene("BBT");
            }
            else
            {
                msg.text = "该激活码已失效";
            }
        }
        else
        {
            msg.text = "该激活码错误";
        }


        //if (AESHelper.AesDecrypt(ActivationCodeInputField.text, "unitech-pevr-key") == MachineCodeInputField.text)
        //{
        //    msg.text = "激活成功,请重启软件";
        //    StreamWriter sw = File.CreateText(Application.dataPath + "/license");
        //    sw.WriteLine(ActivationCodeInputField.text);
        //    sw.Close();
        //}
    }

    public void GetMachineCode()
    {
        NetworkInterface[] nis = NetworkInterface.GetAllNetworkInterfaces();
        foreach (NetworkInterface item in nis)
        {
            if (item.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
            {
                MachineCodeInputField.text = item.GetPhysicalAddress().ToString();
                machineCode = MachineCodeInputField.text;
                return;
            }
        }
    }

    public void GetCdkey(string request)
    {
        print(AESHelper.AesEncrypt(request, "unitech-pevr-key"));
    }

    // Use this for initialization
    void Start()
    {


        MachineCodeInputField = transform.Find("MachineCode").GetComponent<InputField>();
        ActivationCodeInputField = transform.Find("AcivationCode").GetComponent<InputField>();
        GetActivationCodeInputField = transform.Find("GetCode").GetComponent<InputField>();
        ActivationDaysInputField = transform.Find("ActivationDays").GetComponent<InputField>();
        GetMachineCodeButton = transform.Find("GetMachineCode").GetComponent<Button>();
        //GetMachineCodeButton.onClick.AddListener(GetMachineCode);
        ActivationButton = transform.Find("Activation").GetComponent<Button>();
        ActivationButton.onClick.AddListener(ActivationButtonClick);
        GetActivationCodeButton = transform.Find("GetActivationCode").GetComponent<Button>();
        GetActivationCodeButton.onClick.AddListener(GetActivationCode);
        msg = transform.Find("Text").GetComponent<Text>();

        //CheckActivation();
        GetMachineCode();

        //string result = AESHelper.AesEncrypt("123456", "unitech-pevr-key");
        //print(result);
        //print(AESHelper.AesDecrypt(result, "unitech-pevr-key"));
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 生成激活码
    /// </summary>
    public void GetActivationCode()
    {
        ActivationCode activationCode = new ActivationCode();
        activationCode.C = GetActivationCodeInputField.text;
        activationCode.T = GetActivationDate(int.Parse(ActivationDaysInputField.text));
        string code = JsonMapper.ToJson(activationCode);
        Debug.Log(code);
        Debug.Log(AESHelper.AesEncrypt(code, "unitech-pevr-key"));

        //Debug.Log(Encrypt(code));
    }
    /// <summary>
    /// 获取激活有效日期
    /// </summary>
    public DateTime GetActivationDate(int days)
    {
        DateTime activationDateTime = DateTime.Now.AddDays(days);
        //activationDateTime = activationDateTime.ToShortDateString();
        Debug.Log(activationDateTime);
        return activationDateTime;
    }

    /// <summary>
    /// 检查激活状态
    /// </summary>
    void CheckActivation()
    {
        if (!string.IsNullOrEmpty(PlayerPrefs.GetString(activationKey)))
        {
            DateTime dateTime = DateTime.Parse(PlayerPrefs.GetString(activationKey));
            if (dateTime > DateTime.Now)
            {
                msg.text = "已经激活成功了";
            }
            else
            {
                msg.text = "激活已到期";
            }
        }
        else
        {
            msg.text = "请激活";
        }
    }

    private string encryptKey = "pevr";
    string Encrypt(string str)
    {
        DESCryptoServiceProvider descsp = new DESCryptoServiceProvider();
        byte[] key = Encoding.Unicode.GetBytes(encryptKey);
        byte[] data = Encoding.Unicode.GetBytes(str);
        MemoryStream MStream = new MemoryStream();
        CryptoStream CStream = new CryptoStream(MStream, descsp.CreateEncryptor(key, key), CryptoStreamMode.Write);
        CStream.Write(data, 0, data.Length);
        CStream.FlushFinalBlock();
        return Convert.ToBase64String(MStream.ToArray());
    }
}


public class ActivationCode
{
    /// <summary>
    /// 机器码
    /// </summary>
    public string C;
    /// <summary>
    /// 有效时间
    /// </summary>
    public DateTime T;
}