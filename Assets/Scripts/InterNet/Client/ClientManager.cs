using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;
using Common;
public class ClientManager :BaseManager {

    public string IP = "192.168.1.13";
    private const int PORT = 6699;
    Socket clientSocket;
    Message message = new Message();
    public override void OnInit()
    {
        base.OnInit();
        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            clientSocket.Connect(IP, PORT);
            Start();
        }
        catch (Exception e)
        {
            Debug.Log("无法连接到服务器"+e);
        }
    }

    private void Start()
    {
        clientSocket.BeginReceive(message.Data,message.StartIndex,message.RemainSize,SocketFlags.None,ReceiveCallback,null);
    }

    private void ReceiveCallback(IAsyncResult ar)
    {
        try
        {
            if (clientSocket == null || clientSocket.Connected == false) return;
            int count = clientSocket.EndReceive(ar);
            message.ReadMessage(count, ReadMessageCallback);
            Start();
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

    }

    private void ReadMessageCallback(ActionCode actionCode,string data)
    {
        Manager.Instace.HandleResponse(actionCode, data);
    }

    public void SendRequest(RequestCode requestCode,ActionCode actionCode,string data)
    {
        byte[] bytes = Message.PackMessage(requestCode, actionCode, data);
        clientSocket.Send(bytes);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        try
        {
            clientSocket.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}
