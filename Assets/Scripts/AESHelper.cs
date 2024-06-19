using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System;

public class AESHelper {

	public static string AesEncrypt(string str,string key){
		if (string.IsNullOrEmpty (str))
			return null;
		Byte[] toEncryptArray = Encoding.UTF8.GetBytes (str);
		RijndaelManaged rm = new RijndaelManaged {
			Key=Encoding.UTF8.GetBytes(key),
			Mode=CipherMode.ECB,
			Padding=PaddingMode.PKCS7
		};

		ICryptoTransform cTransform = rm.CreateEncryptor ();
		Byte[] resultArray = cTransform.TransformFinalBlock (toEncryptArray, 0, toEncryptArray.Length);
		return Convert.ToBase64String (resultArray);
	}

	public static string AesDecrypt(string str,string key){
		if(string.IsNullOrEmpty(str))
			return null;
		Byte[] toEncryptArray = Convert.FromBase64String (str);

		RijndaelManaged rm = new RijndaelManaged {
			Key=Encoding.UTF8.GetBytes(key),
			Mode=CipherMode.ECB,
			Padding=PaddingMode.PKCS7
		};

		ICryptoTransform cTransform = rm.CreateDecryptor ();
		Byte[] resultArray = cTransform.TransformFinalBlock (toEncryptArray, 0, toEncryptArray.Length);

		return Encoding.UTF8.GetString (resultArray);
	}
}
