using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;

public class RoleSelectClient : MonoBehaviour {
	public static string serverIP="192.168.1.13";
	public static string roleInfo;
	public string receivedMsg=@"none:none";
	Socket client;

	// Use this for initialization
	void Start () {
		client = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		client.Connect (new IPEndPoint (IPAddress.Parse (serverIP), 7000));
		Thread th = new Thread (new ThreadStart(ReceiveMsg));
		th.Start ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SendRoleInfo(){
		print (roleInfo);
		client.Send (Encoding.UTF8.GetBytes ("selectedrole:" + roleInfo));
	}

	public void SendRoleInfo2(){
		client.Send (Encoding.UTF8.GetBytes ("disselectedrole:" + roleInfo));
	}

	void ReceiveMsg(){
		while (true) {
			byte[] buffer=new byte[1024];
			int length = client.Receive (buffer);
			receivedMsg = Encoding.UTF8.GetString (buffer, 0, length);
			Debug.Log ("收到信息:" + receivedMsg);
		}
	}

	void OnDestroy(){
		client.Close ();
	}
}
