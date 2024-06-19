using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.DownLoadOnLine
{
    public class UserData
    {
        public List<string> GeometryList;
        /// <summary>
        /// 账户拥有的物品
        /// </summary>
        public List<string> AnimalList;
        public List<string> AnimationList;
        public List<string> CharacterList;
        public List<string> PlantList;
        public List<string> EquipList;
        public List<string> WeaponList;
        public List<string> FoodList;
        public List<string> EstateList;
        public List<string> DianliList;
        public List<string> SceneList;
        public List<string> SignalList;
        public List<string> TerrainList;
        public List<string> TexiaoList;
        public List<string> ToolList;
        public List<string> VehicleList;
        public List<string> WindoorList;
        public List<string> OtherList;
        public List<string> AllList;


        public UserData() { }
    }

    public class Datas
    {
        public string username;
        public string passWorld;
        public Datas() { }

        public Datas(string username, string passWorld)
        {
            this.username = username;
            this.passWorld = passWorld;
        }
    }
    /// <summary>
    /// 商店数据
    /// </summary>
    public class ShopData
    {
        /// <summary>
        /// ID
        /// </summary>
        public string ID;
        /// <summary>
        /// 英文名称
        /// </summary>
        public string Name;
        /// <summary>
        /// 中文名称
        /// </summary>
        public string Name2;
        /// <summary>
        /// 价格
        /// </summary>
        //public string Price;
        /// <summary>
        /// 类别
        /// </summary>
        public string type;
        /// <summary>
        /// 预览图下载地址
        /// </summary>
        public string SpritePath;
        /// <summary>
        /// 下载地址
        /// </summary>
        public string downLoadPath;
        public ShopData() { }

        public ShopData(string id, string name, string name2, string type)
        {
            ID = id;
            Name = name;
            Name2 = name2;
            this.type = type;
            //this.SpritePath = SpritePath;
            //this.downLoadPath = downLoadPath;
        }
    }

    public class IDName
    {
        /// <summary>
        /// 该模型的名字
        /// </summary>
        public string Name;
        /// <summary>
        /// 该模型在服务器上的ID
        /// </summary>
        public string ID;
        
        public IDName() { }

        public IDName(string id, string name)
        {
            ID = id;
            Name = name;
        }
    }
    ///// <summary>
    ///// 下载item的对象池
    ///// </summary>
    //public class PreviewItemGameObjectPool
    //{
    //    public Dictionary<string, GameObject> PreviewItemObjectsDictionary = new Dictionary<string, GameObject>();
    //    private Queue<string> idQueue = new Queue<string>();
    //    private List<string> idList = new List<string>();
    //    private int MaxNum = 100;
    //    private GameObject obj;
    //    /// <summary>
    //    /// 从对象池拿对象
    //    /// </summary>
    //    /// <param name="id"></param>
    //    /// <returns></returns>
    //    public GameObject GetPreviewItemGameObjectPool(string id)
    //    {
    //        if (PreviewItemObjectsDictionary.ContainsKey(id))
    //        {
    //            obj = PreviewItemObjectsDictionary[id];
    //            PreviewItemObjectsDictionary.Remove(id);
    //            idList.Remove(id);
    //            return obj;
    //        }
    //        return null;
    //    }
    //    /// <summary>
    //    /// 将对象添加到对象池
    //    /// </summary>
    //    /// <param name="id"></param>
    //    /// <param name="previewItemGameObject"></param>
    //    public void AddPreviewItemObjectPool(string id, GameObject previewItemGameObject)
    //    {
    //        if (PreviewItemObjectsDictionary.ContainsKey(id))
    //        {
    //            return;
    //        }
    //        if (idList.Count>=MaxNum)
    //        {
    //            PreviewItemObjectsDictionary.Remove(idList[0]);
    //            idList.Remove(idList[0]);
    //        }
    //        PreviewItemObjectsDictionary.Add(id,previewItemGameObject);
    //        idList.Add(id);
    //    }
    //}

    
    ///// <summary>
    ///// 正在下载的缓存池
    ///// </summary>
    //public class DownLoadingPool
    //{
    //    public Dictionary<string, GameObject> DownLoadingItemPoolDictionary = new Dictionary<string, GameObject>();
    //    private Queue<string> idQueue = new Queue<string>();
    //    private int MaxNum = 50;

    //    /// <summary>
    //    /// 从对象池获取Item
    //    /// </summary>
    //    public GameObject GetPreviewItemPool(string id)
    //    {
    //        if (DownLoadingItemPoolDictionary.ContainsKey(id))
    //        {
    //            return DownLoadingItemPoolDictionary[id];
    //        }
    //        return null;
    //    }
    //    /// <summary>
    //    /// 将对象添加到对象池
    //    /// </summary>
    //    /// <param name="id"></param>
    //    /// <param name="sprite"></param>
    //    public void AddPreviewItemPool(string id, GameObject PreviewItemObj)
    //    {
    //        if (DownLoadingItemPoolDictionary.ContainsKey(id))
    //        {
    //            return;
    //        }
    //        if (idQueue.Count >= MaxNum)
    //        {
    //            DownLoadingItemPoolDictionary.Remove(idQueue.Dequeue());
    //        }
    //        DownLoadingItemPoolDictionary.Add(id, PreviewItemObj);
    //        idQueue.Enqueue(id);
    //    }

    //}
}