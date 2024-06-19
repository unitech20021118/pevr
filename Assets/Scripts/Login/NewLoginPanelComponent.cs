using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using DG.Tweening;
using LitJson;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using VRTK;
using System.Text.RegularExpressions;

namespace Assets.Scripts.Login
{
    public class LoginPostMsg
    {
        public string username { get; set; }
        public string message { get; set; }
        public string code { get; set; }
    }

    public class VersionPostMsg
    {
        public string body { get; set; }
        public string message { get; set; }
        public string code { get; set; }
    }

    public class PlayerRepotMsg
    {
        public string id { get; set; }
        public string message { get; set; }
        public string code { get; set; }
    }

    public class NewLoginPanelComponent : MonoBehaviour
    {
        public static NewLoginPanelComponent Instance;
        private GameObject loginPanelGameObject;
        private InputField _inputAccount;
        private InputField _inputPassword;
        private Button _btnRegister;
        private Button _btnLogin;
        private GameObject loadingLogin;
        private Button _btnSkipLogin;
        public string LoginUrl = "";
        public string RegisterUrl = "";
        private string VersionUrl = "";
        private string PlayerRepotUrl = "";
        public string version;
        private bool getVersion;
        private string _key = "9QyfK4kopN0AebusJJpdqg==";    //AES密钥
        private MassageBoxPanelComponent _massageBox; //显示提示信息
        private System.Action _loginSuccessAction;  //登录成功后的操作
        private System.Action _registerAction;//注册后的操作
        /// <summary>
        /// 登录时记住账号的toggle
        /// </summary>
        private Toggle keepAccountToggle;

        private string AccountKey = "Account";
        private string keepAccountValue = "";
        
        /// <summary>
        /// 提示版本更新的界面
        /// </summary>
        private GameObject versionTipPanel;

        #region 注册部分
        /// <summary>
        /// 注册界面
        /// </summary>
        private GameObject registerPanelGameObject;
        /// <summary>
        /// 注册的用户名输入框
        /// </summary>
        private InputField registerAccountInputField;
        /// <summary>
        /// 注册的邮箱输入框
        /// </summary>
        private InputField registerEmailInputField;
        /// <summary>
        /// 注册的密码输入框
        /// </summary>
        private InputField registerPassworldInputField;
        /// <summary>
        /// 注册的密码确认框
        /// </summary>
        private InputField confirmPassworldInputField;
        /// <summary>
        /// 注册提交按钮
        /// </summary>
        private Button registeredButton;
        /// <summary>
        /// 去登陆按钮
        /// </summary>
        private Button toLoginButton;

        #endregion

        private void Awake()
        {
            Instance = this;
            loginPanelGameObject = transform.FindChild("Login").gameObject;
            _btnLogin = gameObject.transform.FindChild("Login/BtnLogin").GetComponent<Button>();
            _btnLogin.onClick.AddListener(OnLogin);
            loadingLogin = transform.Find("Login/PanelLoginLoading").gameObject;
            _btnRegister = gameObject.transform.FindChild("Login/BtnRegister").GetComponent<Button>();
            //_btnRegister.onClick.AddListener(OnRerister);
            _btnRegister.onClick.AddListener(ToRegisterPanel);
            _btnSkipLogin = gameObject.transform.FindChild("Login/BtnSkipLogin").GetComponent<Button>();
            _btnSkipLogin.onClick.AddListener(OnSkipBtn);

            _inputAccount = gameObject.transform.FindChild("Login/InputAccount").GetComponent<InputField>();
            _inputPassword = gameObject.transform.FindChild("Login/InputPassword").GetComponent<InputField>();
            var massageBox = gameObject.transform.FindChild("MassageBox").gameObject;

            keepAccountToggle = transform.Find("Login/keep_account_toggle").GetComponent<Toggle>();
            if (!string.IsNullOrEmpty(PlayerPrefs.GetString(AccountKey)))
            {
                keepAccountToggle.isOn = true;
                keepAccountValue = PlayerPrefs.GetString(AccountKey);
                _inputAccount.text = keepAccountValue;
            }
            else
            {
                keepAccountToggle.isOn = false;
            }
            
            versionTipPanel = transform.parent.FindChild("NetMassagePanel").gameObject;
            #region 注册部分

            registerPanelGameObject = transform.FindChild("Register").gameObject;
            registerAccountInputField = transform.FindChild("Register/InputAccount").GetComponent<InputField>();
            registerEmailInputField = transform.FindChild("Register/InputEmail").GetComponent<InputField>();
            registerPassworldInputField = transform.FindChild("Register/InputPassword").GetComponent<InputField>();
            confirmPassworldInputField =
                transform.FindChild("Register/ConfirmInputPassword").GetComponent<InputField>();
            registeredButton = transform.FindChild("Register/BtnRegister").GetComponent<Button>();
            registeredButton.onClick.AddListener(OnRerister);
            toLoginButton = transform.FindChild("Register/closeBtn").GetComponent<Button>();
            toLoginButton.onClick.AddListener(TologinPanel);

            #endregion

            _massageBox = massageBox.AddComponent<MassageBoxPanelComponent>();
            _massageBox.Init();
            _massageBox.HidePanel();
            _loginSuccessAction += HidePanel;

            LoginProxy proxy = new LoginProxy();
        }

        public MassageBoxPanelComponent MessageBox
        {
            get { return _massageBox; }
        }

        void Start()
        {
            //var login = gameObject.transform.FindChild("Login").transform;
            //Vector3 max = new Vector3(1,1,1);
            //float time = 0.5f;
            //login.DOScale(max, time);
            Vector3 max = new Vector3(1, 1, 1);
            float time = 0.5f;
            loginPanelGameObject.transform.DOScale(max, time);

            string str = File.ReadAllText(Application.streamingAssetsPath + "/loginPath.txt", Encoding.Unicode);
            string[] s = str.Split('|');
            LoginUrl = s[0];
            RegisterUrl = s[1];
            VersionUrl = s[2];
            PlayerRepotUrl = s[3];
            //Debug.LogError(LoginUrl+"|"+RegisterUrl);
            // 获取本地的版本号
            if (!string.IsNullOrEmpty(version))
            {
                transform.FindChild("Login/version").GetComponent<Text>().text = "当前版本：" + version;
            }

        }

        private void OnSkipBtn()
        {
            Debug.Log("跳过登录");
            _loginSuccessAction.Invoke();
        }
        /// <summary>
        /// 去注册页面
        /// </summary>
        private void ToRegisterPanel()
        {
            loginPanelGameObject.SetActive(false);
            registerPanelGameObject.SetActive(true);
            CleanInputField(1);
        }
        /// <summary>
        /// 去登录页面
        /// </summary>
        private void TologinPanel()
        {
            loginPanelGameObject.SetActive(true);
            registerPanelGameObject.SetActive(false);
            CleanInputField(0);
        }
        /// <summary>
        /// 请求版本号
        /// </summary>
        private void GetEdition()
        {
            StartCoroutine(GetVersionSendPost(VersionUrl));

        }
        private void OnRerister()
        {
            //Application.OpenURL(RegisterUrl);
            if (GetInputFieldLegal(registerAccountInputField.gameObject) && GetInputFieldLegal(registerPassworldInputField.gameObject) && GetInputFieldLegal(registerEmailInputField.gameObject))
            {
                var account = registerAccountInputField.text;
                var password = registerPassworldInputField.text;
                var email = registerEmailInputField.text;
                var captcha = "pevr";
                var registerIp = Network.player.ipAddress;
                //获取13位的时间戳
                var timeStamp = GetTimeStamp(System.DateTime.Now);
                var strUsername = timeStamp + account + account;
                var strPassword = timeStamp + account + password;
                var strEmail = timeStamp + account + email;
                var strtCaptcha = timeStamp + account + captcha;
                var strRegisterIp = timeStamp + account + registerIp;
                //获取加密后的账号密码文本，账号密文为时间戳+账号名+账号，密码密文为时间戳+账号名+密码
                var resultUsername = AesEncrypt(strUsername, _key);
                var resultPassword = AesEncrypt(strPassword, _key);
                var resultEmail = AesEncrypt(strEmail, _key);
                var resultCaptcha = AesEncrypt(strtCaptcha, _key);
                var resultRegisterIp = AesEncrypt(strRegisterIp, _key);
                Debug.Log("注册中" + "AesUsername : " + resultUsername + " AesPassword : " + resultPassword + "AesEmail : " + resultEmail);
                //表单信息为账号，密码，标记字段（账号）
                var form = new WWWForm();
                form.AddField("username", resultUsername);
                form.AddField("password", resultPassword);
                form.AddField("email", resultEmail);
                form.AddField("captcha", resultCaptcha);
                form.AddField("registerIp", resultRegisterIp);
                form.AddField("flagStr", account);
                StartCoroutine(RegisterSendPost(RegisterUrl, form));
            }else if (registerAccountInputField.text.Equals(""))
            {
                ShowTips(registerAccountInputField.gameObject,"请输入用户名");
            }
            else if (registerEmailInputField.text.Equals(""))
            {
                ShowTips(registerEmailInputField.gameObject, "请输入邮箱");
            }
            else if (registerPassworldInputField.text.Equals(""))
            {
                ShowTips(registerPassworldInputField.gameObject, "请输入密码");
            }
            else if (confirmPassworldInputField.text.Equals(""))
            {
                ShowTips(confirmPassworldInputField.gameObject, "请确认密码");
            }

        }

        private void OnLogin()
        {
            
            if (_inputAccount.text.Equals(""))
            {
                ShowTips(_inputAccount.gameObject,"请输入账号");
                return;
            }else if (_inputPassword.text.Equals(""))
            {
                ShowTips(_inputPassword.gameObject, "请输入密码");
                return;
            }
            loadingLogin.SetActive(true);
            var account = _inputAccount.text;
            var password = _inputPassword.text;
            //获取13位的时间戳
            var timeStamp = GetTimeStamp(System.DateTime.Now);
            var strUsername = timeStamp + account + account;
            var strPassword = timeStamp + account + password;

            //获取加密后的账号密码文本，账号密文为时间戳+账号名+账号，密码密文为时间戳+账号名+密码
            var resultUsername = AesEncrypt(strUsername, _key);
            var resultPassword = AesEncrypt(strPassword, _key);

            Debug.Log("登录中" + "AesUsername : " + resultUsername + " AesPassword : " + resultPassword);
            //表单信息为账号，密码，标记字段（账号）
            var form = new WWWForm();
            form.AddField("username", resultUsername);
            form.AddField("password", resultPassword);
            form.AddField("flagStr", account);
            StartCoroutine(LoginSendPost(LoginUrl, form));
        }
        /// <summary>
        /// 注册请求 post
        /// </summary>
        /// <param name="_url"></param>
        /// <param name="_wwwForm"></param>
        /// <returns></returns>
        public IEnumerator RegisterSendPost(string _url, WWWForm _wwwForm)
        {
            WWW postData = new WWW(_url, _wwwForm);
            yield return postData;
            if (postData.error != null)
            {
                Debug.Log(postData.error);
                _massageBox.ShowMassage("网络连接错误，请检查网络");
            }
            else
            {
                //获取返回值
                Debug.Log(postData.text);
                var resultText = postData.text;
                var msg = JsonMapper.ToObject<LoginPostMsg>(resultText);
                Debug.Log(msg.username + " " + msg.message + " " + msg.code);
                // 判断是否注册成功
                switch (msg.code)
                {
                    case "200":
                        _registerAction += delegate
                        {
                            _inputAccount.text = registerAccountInputField.text;
                            TologinPanel();
                        };
                        _massageBox.ShowMassage("您的注册请求已提交，请在发送给您的邮件中激活！", _registerAction);
                        break;
                    case "201":
                        _massageBox.ShowMassage("网络连接错误，请检查网络");
                        break;
                    case "305":
                        ShowTips(registerAccountInputField.gameObject, "用户名已存在!");
                        //_massageBox.ShowMassage("用户名已存在!");
                        break;
                    case "366":
                        ShowTips(registerEmailInputField.gameObject, "该邮箱已被注册！");
                        //_massageBox.ShowMassage("该邮箱已被注册！");
                        break;
                    case "367":
                        ShowTips(registerEmailInputField.gameObject, "邮箱地址无效！");
                        //_massageBox.ShowMassage("邮箱地址无效！");
                        break;
                    default:
                        _massageBox.ShowMassage("网络连接错误，请检查网络");
                        break;
                }
                //if (msg.code.Equals("200"))
                //{
                //    _registerAction += delegate
                //    {
                //        _inputAccount.text = registerAccountInputField.text;
                //        TologinPanel();
                //    };
                //    _massageBox.ShowMassage("注册成功",_registerAction);
                   
                //}
                //else if (msg.message.Equals("用户名已存在"))
                //{
                //    _massageBox.ShowMassage("用户名已存在!");
                //}else if (msg.code.Equals("366"))
                //{
                //    _massageBox.ShowMassage("该邮箱已被注册！");
                //}
                //else
                //{
                //    _massageBox.ShowMassage("网络连接错误，请检查网络");
                //}

            }
        }
        /// <summary>
        /// 登录请求 post
        /// </summary>
        /// <param name="_url"></param>
        /// <param name="_wwwForm"></param>
        /// <returns></returns>
        public IEnumerator LoginSendPost(string _url, WWWForm _wwwForm)
        {
            WWW postData = new WWW(_url, _wwwForm);
            yield return postData;
            if (postData.error != null)
            {
                Debug.Log(postData.error);
                _massageBox.ShowMassage("网络连接错误，请检查网络");
            }
            else
            {
                //获取返回值
                Debug.Log(postData.text);
                var resultText = postData.text;
                var msg = JsonMapper.ToObject<LoginPostMsg>(resultText);
                Debug.Log(msg.username + " " + msg.message + " " + msg.code);
                // 判断是否登录成功
                switch (msg.code)
                {
                    case "200":
                        //记录保存账号密码
                        LoginProxy.Proxy.SaveAccount(_inputAccount.text, _inputPassword.text);
                        LoadManager.Instance.LoginAccount = _inputAccount.text;
                        LoadManager.Instance.LoginPassword = _inputPassword.text;
                        //判断是否需要记住账号
                        if (keepAccountToggle.isOn)
                        {
                            PlayerPrefs.SetString(AccountKey, _inputAccount.text);
                        }
                        else
                        {
                            PlayerPrefs.DeleteKey(AccountKey);
                        }

                        //隐藏注册登录界面
                        HidePanel();
                        //请求版本信息
                        GetEdition();
                        break;
                    case "201":
                        ShowTips(_inputPassword.gameObject, "缺少参数！");
                        break;
                    case "301":
                        ShowTips(_inputAccount.gameObject, "用户名不存在！");
                        break;
                    case "304":
                        ShowTips(_inputPassword.gameObject, "密码错误！");
                        break;
                    default:
                            _massageBox.ShowMassage("网络连接错误，请检查网络");
                        break;
                }
                
               // if (msg.code.Equals("200"))
               // {
               //     //记录保存账号密码
               //     LoginProxy.Proxy.SaveAccount(_inputAccount.text, _inputPassword.text);
               //     //判断是否需要记住账号
               //     if (keepAccountToggle.isOn)
               //     {
               //         PlayerPrefs.SetString(AccountKey, _inputAccount.text);
               //     }
               //     else
               //     {
               //         PlayerPrefs.DeleteKey(AccountKey);
               //     }
                    
               //     //隐藏注册登录界面
               //     HidePanel();
               //     //请求版本信息
               //     GetEdition();
               //     //暂时不弹出登录成功对话框
               //     //_massageBox.ShowMassage("登录成功", _loginSuccessAction);
               // }
               //else if (msg.message.Equals("用户未找到")|| msg.message.Equals("密码错误！"))
               // {
               //     _massageBox.ShowMassage("用户名或密码错误！");
               // }
               // else
               // {
               //     _massageBox.ShowMassage("网络连接错误，请检查网络");
               // }

            }
            loadingLogin.SetActive(false);
        }
        /// <summary>
        /// 获取版本信息的请求
        /// </summary>
        /// <param name="_url"></param>
        /// <param name="_wwwForm"></param>
        /// <returns></returns>
        public IEnumerator GetVersionSendPost(string _url)
        {
            WWW postData = new WWW(_url);
            yield return postData;
            if (postData.error != null)
            {
                Debug.Log(postData.error);
                _massageBox.ShowMassage(postData.error);
            }
            else
            {
                //获取返回值
                Debug.Log(postData.text);
                var resultText = postData.text;
                var msg = JsonMapper.ToObject<VersionPostMsg>(resultText);
                Debug.Log(msg.body + " " + msg.message + " " + msg.code);
                // 版本不一致
                if (!msg.body.Equals(version))
                {
                    OpenVersionPanel();
                }//版本一致
                else
                {
                    yield return null;
                }
            }
        }

        public void OnPlayerRepot(string title,string msg)
        {/*
            //var account = LoginProxy.Proxy.AccountId;
            var account = "gdl";
            //var message = msg;
            ////获取13位的时间戳
            //var timeStamp = GetTimeStamp(System.DateTime.Now);
            //var strUsername = timeStamp + account + account;
            //var strPassword = timeStamp + account + password;
            ////获取加密后的账号密码文本，账号密文为时间戳+账号名+账号，密码密文为时间戳+账号名+密码
            //var resultUsername = AesEncrypt(strUsername, _key);
            //var resultPassword = AesEncrypt(strPassword, _key);
            //Debug.Log("注册中" + "AesUsername : " + resultUsername + " AesPassword : " + resultPassword);
            ////表单信息为账号，密码，标记字段（账号）
            var form = new WWWForm();
            form.AddField("username", account);
            form.AddField("title",title);
            form.AddField("message", msg);
            StartCoroutine(RepotSendPost(PlayerRepotUrl, form));*/
            var account = LoginProxy.Proxy.AccountId;
            string url = PlayerRepotUrl+"?username="+account+"&title="+title+"&message="+msg;
            StartCoroutine(RepotSendGet(url));
        }

        public IEnumerator RepotSendGet(string url)
        {
            Debug.Log(url);
            WWW postData = new WWW(url);
            yield return postData;
            if (postData.error != null)
            {
                Debug.Log(postData.error);
                //显示反馈失败页面
                GameObject.Find("Canvas").transform.Find("PlayerRepotPanel/PlayerRepotFailPanel").gameObject.SetActive(true);
            }
            else
            {
                //获取返回值
                Debug.Log(postData.text);
                var resultText = postData.text;
                var msg = JsonMapper.ToObject<PlayerRepotMsg>(resultText);
                Debug.Log(msg.id + " " + msg.message + " " + msg.code);
                // 反馈成功
                switch (msg.code)
                {
                    case "200":
                        //显示反馈成功界面
                        GameObject.Find("Canvas").transform.Find("PlayerRepotPanel/PlayerRepotFinshPanel").gameObject.SetActive(true);
                        break;
                        default:
                            //显示反馈失败页面
                            GameObject.Find("Canvas").transform.Find("PlayerRepotPanel/PlayerRepotFailPanel").gameObject.SetActive(true);
                        break;
                }
                //if (msg.message.Equals("成功!"))
                //{
                //    //显示反馈成功界面
                //    GameObject.Find("Canvas").transform.Find("PlayerRepotPanel/PlayerRepotFinshPanel").gameObject.SetActive(true);
                //}//反馈失败
                //else
                //{
                //    //显示反馈失败页面
                //    GameObject.Find("Canvas").transform.Find("PlayerRepotPanel/PlayerRepotFailPanel").gameObject.SetActive(true);
                //}
            }
        }

        public IEnumerator RepotSendPost(string _url, WWWForm _wwwForm)
        {
            Debug.Log(_url + _wwwForm);
            WWW postData = new WWW(_url, _wwwForm);
            yield return postData;
            if (postData.error != null)
            {
                Debug.Log(postData.error);
                //显示反馈失败页面
                GameObject.Find("Canvas").transform.Find("PlayerRepotPanel/PlayerRepotFailPanel").gameObject.SetActive(true);
            }
            else
            {
                //获取返回值
                Debug.Log(postData.text);
                var resultText = postData.text;
                var msg = JsonMapper.ToObject<PlayerRepotMsg>(resultText);
                Debug.Log(msg.id + " " + msg.message + " " + msg.code);
                // 反馈成功
                if (msg.message.Equals("成功！"))
                {
                    //显示反馈成功界面
                    GameObject.Find("Canvas").transform.Find("PlayerRepotPanel/PlayerRepotFinshPanel").gameObject.SetActive(true);
                }//反馈失败
                else
                {
                    //显示反馈失败页面
                    GameObject.Find("Canvas").transform.Find("PlayerRepotPanel/PlayerRepotFailPanel").gameObject.SetActive(true);
                }
            }
        }

        //http get
        public IEnumerator SendGet(string _url)
        {
            WWW getData = new WWW(_url);
            yield return getData;
            if (getData.error != null)
            {
                Debug.Log(getData.error);
            }
            else
            {
                Debug.Log(getData.text);
            }
        }

        //AES加密
        public static string AesEncrypt(string value, string key, string iv = "")
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            if (key == null) throw new Exception("未将对象引用设置到对象的实例。");
            if (key.Length < 16) throw new Exception("指定的密钥长度不能少于16位。");
            if (key.Length > 32) throw new Exception("指定的密钥长度不能多于32位。");
            if (key.Length != 16 && key.Length != 24 && key.Length != 32) throw new Exception("指定的密钥长度不明确。");
            if (!string.IsNullOrEmpty(iv))
            {
                if (iv.Length < 16) throw new Exception("指定的向量长度不能少于16位。");
            }

            //var _keyByte = Encoding.UTF8.GetBytes(key);
            var _valueByte = Encoding.UTF8.GetBytes(value);
            using (var aes = new RijndaelManaged())
            {
                aes.IV = !string.IsNullOrEmpty(iv)
                    ? Encoding.UTF8.GetBytes(iv)
                    : Encoding.UTF8.GetBytes(key.Substring(0, 16));
                aes.Key = Convert.FromBase64String(key);
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.PKCS7;
                var cryptoTransform = aes.CreateEncryptor();
                var resultArray = cryptoTransform.TransformFinalBlock(_valueByte, 0, _valueByte.Length);
                return Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }
        }

        //AES解密
        public static string AesDecrypt(string value, string key, string iv = "")
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            if (key == null) throw new Exception("未将对象引用设置到对象的实例。");
            if (key.Length < 16) throw new Exception("指定的密钥长度不能少于16位。");
            if (key.Length > 32) throw new Exception("指定的密钥长度不能多于32位。");
            if (key.Length != 16 && key.Length != 24 && key.Length != 32) throw new Exception("指定的密钥长度不明确。");
            if (!string.IsNullOrEmpty(iv))
            {
                if (iv.Length < 16) throw new Exception("指定的向量长度不能少于16位。");
            }

            // var _keyByte = Encoding.UTF8.GetBytes(key);
            var _valueByte = Convert.FromBase64String(value);
            using (var aes = new RijndaelManaged())
            {
                aes.IV = !string.IsNullOrEmpty(iv)
                    ? Encoding.UTF8.GetBytes(iv)
                    : Encoding.UTF8.GetBytes(key.Substring(0, 16));
                aes.Key = Convert.FromBase64String(key);
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.PKCS7;
                var cryptoTransform = aes.CreateDecryptor();
                var resultArray = cryptoTransform.TransformFinalBlock(_valueByte, 0, _valueByte.Length);
                return Encoding.UTF8.GetString(resultArray);
            }
        }

        //获取时间戳
        public static string GetTimeStamp(System.DateTime time)
        {
            long ts = ConvertDataTimeToInt(time);
            return ts.ToString();
        }

        public static long ConvertDataTimeToInt(System.DateTime time)
        {
            System.DateTime startTime =
                TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            long t = (time.Ticks - startTime.Ticks) / 10000;
            return t;
        }

        //将时间戳转化成Datatime
        public static DateTime ConvertStringToDateTime(string timeStamp)
        {
            System.DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }

        public void ShowPanel()
        {
            //gameObject.SetActive(true);
            transform.Find("BackGround").gameObject.SetActive(true);
            transform.Find("Login").gameObject.SetActive(true);
        }

        public void HidePanel()
        {
            transform.Find("BackGround").gameObject.SetActive(false);
            transform.Find("Login").gameObject.SetActive(false);
            //gameObject.SetActive(false);
        }
        /// <summary>
        /// 获取该项输入框内容是否合理
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool GetInputFieldLegal(GameObject obj)
        {
            if (!obj.transform.FindChild("TipLable").gameObject.activeSelf)
            {
                return true;
            }
            return false;
        }
        public void ShowTips(GameObject obj, string str,bool refresh = false)
        {
            if (refresh)
            {
                obj.transform.FindChild("TipLable").gameObject.SetActive(true);
                obj.transform.FindChild("TipLable").GetComponent<Text>().text = str;
            }
            else
            {
                if (!string.IsNullOrEmpty(str))
                {
                    obj.transform.FindChild("TipLable").gameObject.SetActive(true);
                    obj.transform.FindChild("TipLable").GetComponent<Text>().text = str;
                }
                else
                {
                    obj.transform.FindChild("TipLable").gameObject.SetActive(false);
                    obj.transform.FindChild("TipLable").GetComponent<Text>().text = str;
                }
            }
        }

        public void OnInputFieldEndEdit(InputField inf)
        {
            if (inf == registerAccountInputField)
            {
                if (inf.text == "")
                {
                    ShowTips(inf.gameObject, "请输入账号！");
                }
                else if (inf.text.Length < 4 || inf.text.Length > 12)
                {
                    ShowTips(inf.gameObject, "账号的长度应在4到12字符之间");
                }
                else
                {
                    ShowTips(inf.gameObject, "");
                }
            }
            else if (inf == registerEmailInputField)
            {
                if (inf.text != "")
                {
                    var reg = new Regex(@"^([a-zA-Z0-9_-])+@([a-zA-Z0-9_-])+((\.[a-zA-Z0-9_-]{2,3}){1,2})$");
                    if (!reg.IsMatch(inf.text))
                    {
                        ShowTips(inf.gameObject, "邮箱格式不正确！");
                    }
                    else
                    {
                        ShowTips(inf.gameObject, "");
                    }
                }
                else
                {
                    ShowTips(inf.gameObject, "请输入邮箱地址！");
                }
            }
            else if (inf == registerPassworldInputField)
            {
                if (inf.text == "")
                {
                    ShowTips(inf.gameObject, "请输入密码！");
                }//长度判断
                else if (inf.text.Length < 6 || inf.text.Length > 20)
                {
                    ShowTips(inf.gameObject, "密码的长度应在6到20位字符之间");
                }
                else
                {
                    ShowTips(inf.gameObject, "");
                }
            }
            else if (inf == confirmPassworldInputField)
            {
                if (inf.text == "")
                {
                    ShowTips(inf.gameObject, "请输入确认密码！");
                }
                else if (inf.text != registerPassworldInputField.text)
                {
                    ShowTips(inf.gameObject, "确认密码内容与密码内容不一致");
                }
                else
                {
                    ShowTips(inf.gameObject, "");
                }
            }
            else if (inf == _inputAccount)
            {
                if (inf.text == "")
                {
                    ShowTips(inf.gameObject, "请输入账号！");
                }
                else if (inf.text.Length < 4 || inf.text.Length > 12)
                {
                    ShowTips(inf.gameObject, "账号的长度应在4到12字符之间");
                }
                else
                {
                    ShowTips(inf.gameObject, "");
                }
            }
            else if (inf == _inputPassword)
            {
                if (inf.text == "")
                {
                    ShowTips(inf.gameObject, "请输入密码！");
                }
                else //长度判断
                if (inf.text.Length < 6 || inf.text.Length > 20)
                {
                    ShowTips(inf.gameObject, "密码的长度应在6到20位字符之间");
                }
                else
                {
                    ShowTips(inf.gameObject, "");
                }
            }
        }
        /// <summary>
        /// 清空注册或登录界面输入框中的内容
        /// </summary>
        /// <param name="num"></param>
        public void CleanInputField(int num)
        {
            switch (num)
            {
                case 0://清空注册页面的内容
                    registerAccountInputField.text = "";
                    ShowTips(registerAccountInputField.gameObject, "", true);
                    registerPassworldInputField.text = "";
                    ShowTips(registerPassworldInputField.gameObject, "", true);
                    confirmPassworldInputField.text = "";
                    ShowTips(confirmPassworldInputField.gameObject, "", true);
                    registerEmailInputField.text = "";
                    ShowTips(registerEmailInputField.gameObject, "", true);
                    break;
                case 1://清空登录页面的内容
                    _inputAccount.text = "";
                    ShowTips(_inputAccount.gameObject, "", true);
                    _inputPassword.text = "";
                    ShowTips(_inputPassword.gameObject, "", true);
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 打开提示版本更新界面
        /// </summary>
        public void OpenVersionPanel()
        {
            versionTipPanel.SetActive(true);
            versionTipPanel.transform.FindChild("BtnConform").GetComponent<Button>().onClick.AddListener(delegate { Application.OpenURL("http://www.pevrcloud.com/download/index.jhtml"); });
        }
    }
}
