using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using System;
using System.Text;
using System.Linq;
public class Message  {

	    private byte[] data = new byte[1024];
        private int startIndex = 0;

        public byte[] Data
        {
            get{ return data;}
        }

        public int StartIndex
        {
            get { return startIndex; }
        }

        public int RemainSize
        {
            get { return data.Length - startIndex; }  
        }

        public void ReadMessage(int newDataAmout,Action<RequestCode,ActionCode,string> Callback)
        {
            startIndex += newDataAmout;
            while (true)
            {
                if (startIndex <= 4) return;
                int cout = BitConverter.ToInt32(data, 0);
                if ((startIndex - 4) >= cout)
                {
                    RequestCode requestCode=(RequestCode)BitConverter.ToInt32(data,4);
                    ActionCode actionCode=(ActionCode)BitConverter.ToInt32(data,8);
                    string s = Encoding.UTF8.GetString(data, 12, cout-8);
                    Callback(requestCode,actionCode,s);
                    Array.Copy(data, cout + 4, data, 0, startIndex - 4 - cout);
                    startIndex -= 4 + cout;
                }
                else
                {
                    return;
                }
            }

            
        }

        public void ReadMessage(int newDataAmout, Action<ActionCode, string> Callback)
        {
            startIndex += newDataAmout;
            while (true)
            {
                if (startIndex <= 4) return;
                int cout = BitConverter.ToInt32(data, 0);
                if ((startIndex - 4) >= cout)
                {
                    ActionCode actionCode = (ActionCode)BitConverter.ToInt32(data, 4);
                    string s = Encoding.UTF8.GetString(data, 8, cout - 4);
                    Callback(actionCode, s);
                    Array.Copy(data, cout + 4, data, 0, startIndex - 4 - cout);
                    startIndex -= 4 + cout;
                }
                else
                {
                    return;
                }
            }


        }

        //public static byte[] PackMessage(ActionCode actionCode, string data)
        //{
        //    byte[] requestCodeBytes = BitConverter.GetBytes((int)actionCode);
        //    byte[] dataBytes = Encoding.UTF8.GetBytes(data);
        //    int count = requestCodeBytes.Length + dataBytes.Length;
        //    byte[] coutBytes = BitConverter.GetBytes(count);
        //    byte[] resultBytes = coutBytes.Concat(requestCodeBytes).ToArray().Concat(dataBytes).ToArray();
        //    return resultBytes;
        //}

        public static byte[] PackMessage(RequestCode requestCode,ActionCode actionCode,string data)
        {
            byte[] requestCodeBytes = BitConverter.GetBytes((int)requestCode);
            byte[] actionCodeBytes = BitConverter.GetBytes((int)actionCode);
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            int count = requestCodeBytes.Length +actionCodeBytes.Length+ dataBytes.Length;
            byte[] coutBytes = BitConverter.GetBytes(count);
            byte[] resultBytes = coutBytes.Concat(requestCodeBytes).ToArray().Concat(actionCodeBytes).ToArray().Concat(dataBytes).ToArray();
            return resultBytes;
        }
    }

