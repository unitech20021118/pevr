using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
public class PEServer{
    public static List<PEClient> clientList = new List<PEClient>();
 
    

    public PEServer()
    {
        Socket tcpServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
        IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, 7799);
        tcpServer.Bind(ipEndPoint);
        tcpServer.Listen(100);

        
        while (true)
        {
            Socket socket = tcpServer.Accept();
            Debug.Log("一个客户端连入");
            PEClient client = new PEClient(socket);
            clientList.Add(client);

        }

    }

    public static void BroadMessage(string message)
    {
        var NotConnectClient=new List<PEClient>();
        foreach (PEClient client in clientList)
        {
            if (client.IsConnected())
            {
                client.SendMessage(message);
            }
            else
            {
                NotConnectClient.Add(client);
            }
        }
        foreach (PEClient client in NotConnectClient)
        {
            clientList.Remove(client);
        }
    }
}
