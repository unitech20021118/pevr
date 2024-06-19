using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;
using System.Linq;
public class PEMessage  {

    private byte[] data = new byte[1024];
    int Index = 0;

    public static byte[] GetBytes (string str)
    {
        byte[] value = Encoding.UTF8.GetBytes(str);
        byte[] headNum = BitConverter.GetBytes(value.Length);
        byte[] message = headNum.Concat(value).ToArray();
        return message;
    }
}
