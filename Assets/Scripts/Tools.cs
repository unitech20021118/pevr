using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace TheTools
{
    /// <summary>
    /// created by kuai
    /// </summary>
    public class Tools : MonoBehaviour
    {
        public static Tools Instance;

        void Awake()
        {
            Instance = this;
        }
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            //if (Input.GetKeyDown(KeyCode.T))
            //{
            //    GetFolderNameByFolderPath(@"D:/Edward/pevrproject/pevr_5.5/Assets/StreamingAssets/PowerPoint\newTest/111.txt");
            //}
        }
        /// <summary>
        /// 根据文件夹或文件的完整路径获取文件夹或文件名
        /// </summary>
        /// <param name="folderPath">文件的路径</param>
        /// <param name="deleteSuffix">是否删除文件的后缀名</param>
        /// <returns></returns>
        public string GetFolderNameByFolderPath(string folderPath,bool deleteSuffix = false)
        {
            folderPath = folderPath.Replace(@"\", "/");
            string[] str = folderPath.Split('/');
            if (deleteSuffix)
            {
                string[] strs = str[str.Length - 1].Split('.');
                return strs[0];
            }
            return str[str.Length - 1];
        }


        /// <summary>
        /// 删除父级下所有子物体
        /// </summary>
        /// <param name="parent">要删除子物体的物体</param>
        public void DeleteAllChild(Transform parent)
        {
            if (parent.childCount>0)
            {
                for (int i = 0; i < parent.childCount; i++)
                {
                   Destroy(parent.GetChild(i).gameObject);
                }
            }
        }

        /// <summary>
        /// 获取Assetbundles文件夹的路径
        /// </summary>
        /// <returns></returns>
        public string GetAssteBundlesPath()
        {
            DirectoryInfo info = new DirectoryInfo(Application.dataPath);
            return info.Parent.FullName+ "/AssetBundles";
        }
    }

}

