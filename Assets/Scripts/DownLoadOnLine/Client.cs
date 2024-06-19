using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Assets.Scripts.Login;
using LitJson;
using UnityEngine;

namespace Assets.Scripts.DownLoadOnLine
{
    public class Client : MonoBehaviour
    {
        /// <summary>
        /// 客户端的socket
        /// </summary>
        private Socket clientSocket;
        /// <summary>
        /// 接收消息的缓冲池
        /// </summary>
        private byte[] data = new byte[1024];
        /// <summary>
        /// 接收消息的队列，这里放拆包过的字符串
        /// </summary>
        public Queue ReceiveQueue = new Queue();
        /// <summary>
        /// 接收消息的线程
        /// </summary>
        public Thread receiveMessage;
        /// <summary>
        /// 处理消息的类
        /// </summary>
        private Message message;
        /// <summary>
        /// 商店数据
        /// </summary>
        public List<ModelInformation> ShopDataList;

        public IPEndPoint ipEndPoint;

        /// <summary>
        /// 单例
        /// </summary>
        public static Client Instance;
        

        void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            ipEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 10001);
        }

        // Use this for initialization
        void Start()
        {
            message = new Message(this);
            string name = LoginProxy.Proxy.AccountId;
            string passworld = LoginProxy.Proxy.Password;
            Debug.LogError(name);
            if (ConnectToServer())
            {
                Datas data = new global::Assets.Scripts.DownLoadOnLine.Datas(name, passworld);
                //string jsonStr = JsonMapper.ToJson(data);
                RequestModel request = new RequestModel(MessageType.Login, data);
                SendMessage(request);
            }
        }
        // Update is called once per frame
        void Update()
        {
            
        }

        void OnDestroy()
        {
            if (receiveMessage != null)
            {
                receiveMessage.Interrupt();
                clientSocket.Close();
                Debug.LogError("关闭");
            }
        }
        /// <summary>
        /// 向服务器发起连接
        /// </summary>
        bool ConnectToServer()
        {
            try
            {
                //发起连接
                clientSocket.Connect(ipEndPoint);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return false;
            }
            
           
            //开启接收消息的线程
            receiveMessage = new Thread(ReceiveMessage);
            receiveMessage.Start();
            StartCoroutine(HandleReceiveMessage());
            return true;


        }

        /// <summary>
        /// 循环处理不同类型消息的协成
        /// </summary>
        /// <returns></returns>
        IEnumerator HandleReceiveMessage()
        {
            while (clientSocket.Connected == true)
            {
                if (ReceiveQueue.Count > 0)
                {
                    Debug.Log("ReceiveQueue.Count:" + ReceiveQueue.Count);
                    //将消息取出队列
                    string message = (string)ReceiveQueue.Dequeue();
                    //将字符串转换为响应模型类对象
                    ResponseModel responseModel = JsonMapper.ToObject<ResponseModel>(message);
                    //判断接收到的是什么类别的消息并分别处理
                    if (responseModel.messageType == MessageType.Login)
                    {
                        //收到登录消息的处理
                        HandleLoginMessage(responseModel);
                    }
                    else if (responseModel.messageType == MessageType.UserData)
                    {
                        //收到服务器的用户数据的处理
                        HandleUserDataMessage(responseModel);
                    }
                    else if (responseModel.messageType == MessageType.ShopData)
                    {
                        //收到商店数据消息的处理
                        HandleShopDataMessage(responseModel);
                    }
                    else if (responseModel.messageType == MessageType.Buy)
                    {
                        //收到购买消息的处理
                        HandleBuyMessage(responseModel);
                    }
                    else if (responseModel.messageType == MessageType.Test)
                    {
                        //测试消息
                        Debug.LogError("收到一条服务器反馈的消息：" + responseModel.msg);
                    }
                }
                yield return null;
            }

        }
        /// <summary>
        /// 循环接收消息的线程
        /// </summary>
        public void ReceiveMessage()
        {
            while (clientSocket.Connected == true)
            {
                if (clientSocket.Connected == false)
                {
                    Debug.LogError("与服务器断开连接");
                    break;
                }
                int length = clientSocket.Receive(data);
                byte[] newdata = new byte[length];
                Array.Copy(data, newdata, length);
                //将有用的数组交给message处理
                message.Getstring(newdata);
            }
        }
        /// <summary>
        /// 发送消息的方法（发送的是请求类型）
        /// </summary>
        /// <param name="request"></param>
        public void SendMessage(RequestModel request)
        {
            //将请求类型转换为json字符串
            string jsonStr = JsonMapper.ToJson(request);
            Debug.Log("发送的数据是：" + jsonStr);
            //再将json字符串通过message粘包后发送
            clientSocket.Send(message.GetBytes(jsonStr));
        }

        /// <summary>
        /// 处理反馈登录消息的方法
        /// </summary>
        /// <param name="response"></param>
        private void HandleLoginMessage(ResponseModel response)
        {
            if (response.msg == "登录失败")
            {
                Debug.LogError("登录失败");
            }

        }

        private void HandleUserDataMessage(ResponseModel response)
        {
            //将存储在服务器的用户数据文本下载下来
            StartCoroutine(DoDownUserData(response.msg));
        }
        /// <summary>
        /// 处理反馈商店数据消息的方法
        /// </summary>
        /// <param name="response"></param>
        private void HandleShopDataMessage(ResponseModel response)
        {
            //ShopDataList = response.ShopDatas;
            //从服务器下载商店数据文本
            StartCoroutine(DoDownShopData(response.msg));
        }
        /// <summary>
        /// 处理反馈购买消息的方法
        /// </summary>
        /// <param name="response"></param>
        private void HandleBuyMessage(ResponseModel response)
        {

        }
        /// <summary>
        /// 从服务器下载商店数据文本
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        IEnumerator DoDownShopData(string url)
        {
            Debug.Log("开始下载商店数据文本");
            WWW www = new WWW(@"file:///" + url);
            yield return www;
            File.WriteAllBytes(Application.streamingAssetsPath + "/shopdata.txt", www.bytes);
            Debug.Log("商店数据文本下载完成");
            //ShopManager.Instance.InitShopData(GetShopDatas());
        }
        /// <summary>
        /// 从服务器下载用户数据文本
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        IEnumerator DoDownUserData(string url)
        {
            WWW www = new WWW(@"file:///" + url);
            yield return www;
            Debug.LogError(url);
            File.WriteAllBytes(Application.streamingAssetsPath + "/userdata.txt", www.bytes);

        }
        /// <summary>
        /// 读取从服务器下载的商店数据文档
        /// </summary>
        /// <returns></returns>
        public List<ModelInformation> GetShopDatas()
        {
            string str = File.ReadAllText(Application.streamingAssetsPath + "/shopdata.txt");
            string[] shopdata = str.Split('\n');
            ShopDataList = new List<ModelInformation>();
            for (int i = 0; i < shopdata.Length; i++)
            {
                string[] s = shopdata[i].Split('\t');
                ModelInformation shopData = new ModelInformation(s[0], s[1], s[2], s[3]);
                ShopDataList.Add(shopData);
            }
            return ShopDataList;
        }
        /// <summary>
        /// 读取从服务器下载的用户数据文档
        /// </summary>
        /// <returns></returns>
        //public UserData GetUserData()
        //{
        //    if (File.Exists(Application.streamingAssetsPath + "/userdata.txt"))
        //    {
        //        string str = File.ReadAllText(Application.streamingAssetsPath + "/userdata.txt");
        //        UserData userData = JsonMapper.ToObject<UserData>(str);
        //        return userData;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
        /// <summary>
        /// 提交数据（将已经下载过的商店的物品的id告诉服务器）
        /// </summary>
        public void CommidData(string id)
        {
            RequestModel requese = new RequestModel(MessageType.ShopData, id);
            SendMessage(requese);
        }
    }

    /// <summary>
    /// 处理信息的类
    /// </summary>
    class Message
    {
        public byte[] data;
        public Client client;

        public Message(Client client)
        {
            data = new byte[0];
            this.client = client;
        }
        /// <summary>
        /// 将字节数组拆包为字符串
        /// </summary>
        /// <param name="datamsg"></param>
        public void Getstring(byte[] datamsg = null)
        {
            int count = BitConverter.ToInt32(datamsg, 0);
            string str = Encoding.UTF8.GetString(datamsg, 4, count);
            Debug.Log("解析出一条数据：" + str);
            client.ReceiveQueue.Enqueue(str);
        }
        /// <summary>
        /// 将要发送的字符串粘包为字节数组
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public byte[] GetBytes(string msg)
        {
            //将字符串转换为byte数组
            byte[] dataBytes = Encoding.UTF8.GetBytes(msg);
            //获取数组的长度
            int datalength = dataBytes.Length;
            //将数组的长度转换为标准四个字节的byte数组
            byte[] lengthBytes = BitConverter.GetBytes(datalength);
            //将表示byte数组长度的数组作为包头（固定是四个字节）和byte数组拼接起来
            byte[] newBytes = lengthBytes.Concat(dataBytes).ToArray();
            return newBytes;
        }

    }

    /// <summary>
    /// 请求模型类
    /// </summary>
    public class RequestModel
    {
        public MessageType messageType;
        public string msg;
        public Datas Datas;
        public ModelInformation ShopData;
        public RequestModel() { }

        public RequestModel(MessageType messageType, string msg)
        {
            this.messageType = messageType;
            this.msg = msg;
        }

        public RequestModel(MessageType messageType, Datas datas)
        {
            this.messageType = messageType;
            this.Datas = datas;
        }
        public RequestModel(MessageType messageType, ModelInformation shopData)
        {
            this.messageType = messageType;
            this.ShopData = shopData;
        }
    }
    /// <summary>
    /// 响应模型类
    /// </summary>
    public class ResponseModel
    {
        public MessageType messageType;
        public string msg;
        public List<ModelInformation> ShopDatas;
        public ResponseModel() { }

        public ResponseModel(MessageType messageType, string msg)
        {
            this.messageType = messageType;
            this.msg = msg;
        }
    }


    /// <summary>
    /// 消息类型
    /// </summary>
    public enum MessageType
    {
        Login,
        Buy,
        ShopData,
        UserData,
        Test
    }


}
