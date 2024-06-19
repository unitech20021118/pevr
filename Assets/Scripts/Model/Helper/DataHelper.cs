using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class DataHelper{

    //public static IEnumerator LoadModel(string path)
    //{
    //    FileStream fileStream = new FileStream(path,FileMode.Open,FileAccess.Read);
    //    byte[] bytes = new byte[fileStream.Length];
    //    fileStream.Read(bytes, 0, (int)fileStream.Length);
    //    fileStream.Close();

    //    AssetBundleCreateRequest request = AssetBundle.LoadFromMemoryAsync(bytes);
    //    yield return request;
    //    AssetBundle obj = request.assetBundle;
    //    GameObject.Instantiate(obj);
    //    //return (GameObject)GameObject.Instantiate(loadAsset.mainAsset);

    //}

    //public static GameObject[] LoadModel(string path)
    //{
       
    //    AssetBundle ab = AssetBundle.LoadFromFile(path);
    //    GameObject[] obj= ab.LoadAllAssets<GameObject>();
    //    GameObject[] objects = new GameObject[obj.Length];
    //    for (int i = 0; i < obj.Length; i++)
    //    {
    //        objects[i] = GameObject.Instantiate(obj[i]);
    //    }
    //    return objects;
    //}
    
    /// <summary>
    /// 加载AssetBundle
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static GameObject LoadModel(string path)
    {
        AssetBundle ab = AssetBundle.LoadFromFile(path);
        GameObject obj = ab.LoadAsset<GameObject>(Path.GetFileNameWithoutExtension(path));
        return GameObject.Instantiate(obj);
    }
}
