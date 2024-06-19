using System.Collections;
using System.Collections.Generic;
using System.IO;
using LitJson;
using UnityEngine;


namespace Assets.Scripts.DownLoadOnLine
{
    public class ShopImformation : MonoBehaviour
    {
        public static ShopImformation Instance;
        public List<ModelTypeInformation> ModelTypes = new List<ModelTypeInformation>();

        void Awake()
        {
            Instance = this;
        }
        void Start()
        {
            ModelTypes.Add(new ModelTypeInformation("geometry", "几何体"));
            ModelTypes.Add(new ModelTypeInformation("animation", "动画"));
            ModelTypes.Add(new ModelTypeInformation("animal", "动物"));
            ModelTypes.Add(new ModelTypeInformation("character", "角色"));
            ModelTypes.Add(new ModelTypeInformation("plant", "石头植物"));
            ModelTypes.Add(new ModelTypeInformation("equip", "生活用品"));
            ModelTypes.Add(new ModelTypeInformation("estate", "家具"));
            ModelTypes.Add(new ModelTypeInformation("dianli", "生活电器"));
            ModelTypes.Add(new ModelTypeInformation("scene", "场景"));
            ModelTypes.Add(new ModelTypeInformation("signal", "灯光"));
            ModelTypes.Add(new ModelTypeInformation("terrain", "地形"));
            ModelTypes.Add(new ModelTypeInformation("texiao", "特效"));
            ModelTypes.Add(new ModelTypeInformation("vehicle", "车辆"));
            ModelTypes.Add(new ModelTypeInformation("weapon", "武器"));
            ModelTypes.Add(new ModelTypeInformation("food", "食品饮料"));


            
            
            //注意将other放在最后
            ModelTypes.Add(new ModelTypeInformation("other", "无类别"));
        }





        public ModelTypeInformation GetModelTypeByTypeName_CH(string Name_CH)
        {
            ModelTypeInformation modelTypeInformation = ModelTypes.Find(mt => mt.ModelTypeName_CH == Name_CH);
            if (modelTypeInformation==null)
            {
                return ModelTypes[ModelTypes.Count - 1];
            }
            return modelTypeInformation;
        }

        public ModelTypeInformation GetModelTypeByTypeName_EN(string Name_EN)
        {
            ModelTypeInformation modelTypeInformation = ModelTypes.Find(mt => mt.ModelTypeName_EN == Name_EN);
            if (modelTypeInformation == null)
            {
                return ModelTypes[ModelTypes.Count - 1];
            }
            return modelTypeInformation;
        }
    }

    public class ModelTypeInformation
    {
        /// <summary>
        /// 类别的英文名
        /// </summary>
        public string ModelTypeName_EN;
        /// <summary>
        /// 类别的中文名
        /// </summary>
        public string ModelTypeName_CH;
        /// <summary>
        /// 该类别未下载的列表
        /// </summary>
        public List<ModelInformation> NotDownModelInformations;
        /// <summary>
        /// 该类别已下载的列表
        /// </summary>
        public List<ModelInformation> DownModelInformations;
        /// <summary>
        /// 该分类中用户已下载的模型的ID的列表
        /// </summary>
        public List<string> UserDownModelIDList;
        public ModelTypeInformation() { }

        public ModelTypeInformation(string EN, string CH)
        {
            ModelTypeName_EN = EN;
            ModelTypeName_CH = CH;
            


            NotDownModelInformations = new List<ModelInformation>();
            DownModelInformations = new List<ModelInformation>();
            UserDownModelIDList = new List<string>();
        }

        private void AddNewType(string typeName_EN)
        {
            //检查assetBundles文件夹是否存在，不存在创建一个
            if (!Directory.Exists(TheTools.Tools.Instance.GetAssteBundlesPath() + "/" + typeName_EN))
            {
                Directory.CreateDirectory(TheTools.Tools.Instance.GetAssteBundlesPath() + "/" + typeName_EN);
            }
            //检查json文本文件是否存在
            if (!File.Exists(Application.streamingAssetsPath+"/"+typeName_EN+".txt"))
            {
                string str = File.ReadAllText(Application.streamingAssetsPath + "/model.txt");
                str.Replace("model", typeName_EN);
                JsonData jsondata = new JsonData();
                jsondata[typeName_EN] = new JsonData();
                File.WriteAllText(Application.streamingAssetsPath + "/" + typeName_EN + ".txt", str);
            }

        }
    }

    public class ModelInformation
    {
        /// <summary>
        /// ID
        /// </summary>
        public string ModelID;
        /// <summary>
        /// 英文名称
        /// </summary>
        public string ModelName_EN;
        /// <summary>
        /// 中文名称
        /// </summary>
        public string ModelName_CH;
        /// <summary>
        /// 类别的中文名
        /// </summary>
        public string ModelTypeName_CH;
        /// <summary>
        /// 类别的英文名
        /// </summary>
        public string ModelTypeName_EN;

        public ModelInformation (){}

        public ModelInformation(string id,string modelName_EN,string modelName_CH,string modeTypeName_CH)
        {
            ModelID = id;
            ModelName_EN = modelName_EN;
            ModelName_CH = modelName_CH;
            ModelTypeName_CH = modeTypeName_CH;
            ModelTypeName_EN = ShopImformation.Instance.GetModelTypeByTypeName_CH(modeTypeName_CH).ModelTypeName_EN;

        }
    }
}