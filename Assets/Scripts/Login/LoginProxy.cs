using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Login
{
    public class LoginProxy
    {
        public static LoginProxy Proxy = null;
        private string _accountId;
        private string _password;


        public LoginProxy()
        {
            Proxy = this;
        }

        public string AccountId
        {
            get
            {
                if (!string.IsNullOrEmpty(_accountId))
                {
                    return _accountId;
                }else
                {
                    return LoadManager.Instance.LoginAccount;
                }
            }
        }

        public string Password
        {
            get
            {
                if (!string.IsNullOrEmpty(_password))
                {
                    return _password;
                }
                else
                {
                    return LoadManager.Instance.LoginPassword;
                }
            }
        }

        public void SaveAccount(string id, string password)
        {
            _accountId = id;
            _password = password;
            //当登录成功时就从服务器下载用户数据和商店数据文本
            ShopManager.Instance.StartDown();
        }
    }
}
