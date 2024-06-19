using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Text;
public class PEClient{
    private Socket clientSocket;
    byte[] values = new byte[1024];
    public PEClient(Socket socket)
    {
        clientSocket = socket;
        Thread thread = new Thread(ReceiveMessage);

        thread.Start();
    }

    void ReceiveMessage()
    {
        while (true)
        {
            if (clientSocket.Poll(10,SelectMode.SelectRead))
            {
                clientSocket.Close();
                break;
            }
            int length = clientSocket.Receive(values);

            string message = Encoding.UTF8.GetString(values, 0, length);
            Debug.Log(message);
            PEServer.BroadMessage(message);
        }
        


    }

    public void SendMessage(string message)
    {

        clientSocket.Send(Encoding.UTF8.GetBytes(message));
    }

    public bool IsConnected()
    {
        return clientSocket.Connected;
    }
}
