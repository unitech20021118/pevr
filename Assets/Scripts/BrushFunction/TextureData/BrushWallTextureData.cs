using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// 墙壁的贴图
/// </summary>
public class BrushWallTextureData 
{
    public int ID;
    public string E_name;
    public string C_name;
    public BrushWallTextureData() { }
    public BrushWallTextureData(int id, string e_name,string c_name)
    {
        ID = id;
        E_name = e_name;
        C_name = c_name;
    }
}
public class BrushDoorTextureData
{
    public int ID;
    public string E_name;
    public string C_name;
    public BrushDoorTextureData() { }
    public BrushDoorTextureData(int id, string e_name, string c_name)
    {
        ID = id;
        E_name = e_name;
        C_name = c_name;
    }
}



public class WallTexture
{
    public static List<BrushWallTextureData> brushWallTextureDatas = new List<BrushWallTextureData>();
    public static List<BrushDoorTextureData> brushDoorTextureDatas = new List<BrushDoorTextureData>();
    public static ChangeTextureMaterial changeTextureMaterial;
    public static string WallMaterialPath = "Materials/BrushWallMaterials/";
    public static string DoorMaterialPath = "Materials/BrushWallMaterials/Door/";
    public static void Init()
    {
        if (brushWallTextureDatas.Count>0)
        {
            return;
        }
        //brushWallTextureDatas.Add(new BrushWallTextureData(1,"baiqiang","白墙")); 
        brushWallTextureDatas.Add(new BrushWallTextureData(2, "zhuanqiang", "砖墙"));
        brushWallTextureDatas.Add(new BrushWallTextureData(3, "baizhuan", "白砖"));
        brushWallTextureDatas.Add(new BrushWallTextureData(5, "caizhuan", "彩砖"));
        brushWallTextureDatas.Add(new BrushWallTextureData(6, "huangseyama", "黄色亚麻"));
        brushWallTextureDatas.Add(new BrushWallTextureData(7, "jiuzhuan", "旧砖"));
        brushWallTextureDatas.Add(new BrushWallTextureData(8, "liewen", "裂纹砖"));
        brushWallTextureDatas.Add(new BrushWallTextureData(9, "liubianxingzhuan", "六边形砖"));
        brushWallTextureDatas.Add(new BrushWallTextureData(10, "qipange", "棋盘格"));
        brushWallTextureDatas.Add(new BrushWallTextureData(11, "shuinizhuan", "水泥砖"));
        brushWallTextureDatas.Add(new BrushWallTextureData(12, "suizhuan", "碎砖"));


        brushDoorTextureDatas.Add(new BrushDoorTextureData(1, "baisemen", "白色门"));
        brushDoorTextureDatas.Add(new BrushDoorTextureData(2, "anjingsemen", "暗金色门"));
        brushDoorTextureDatas.Add(new BrushDoorTextureData(3, "maobolimen", "玻璃移门"));
        brushDoorTextureDatas.Add(new BrushDoorTextureData(4, "shengsemumen", "深色木门"));
        brushDoorTextureDatas.Add(new BrushDoorTextureData(5, "tiemen", "铁门")); 


    }

    public static ChangeTextureMaterial LoadDoorTexture(BrushDoorTextureData brushDoorTextureData)
    {
        if (brushDoorTextureData!=null)
        {
            return new ChangeTextureMaterial(Resources.Load<Material>(DoorMaterialPath + brushDoorTextureData.E_name), ChangeTextureMaterialType.door);
        }
        else
        {
            return null;
        }
    }
    /// <summary>
    /// 加载墙壁的贴图的材质球
    /// </summary>
    public static ChangeTextureMaterial LoadWallTexture(BrushWallTextureData brushTextureData, bool ground = false)
    {
        if (brushTextureData!=null)
        {
            if (ground)
            {
                //Debug.LogError(WallMaterialPath + brushTextureData.E_name + "_G");
                return new ChangeTextureMaterial(Resources.Load<Material>(WallMaterialPath + brushTextureData.E_name + "_G"), ChangeTextureMaterialType.wall);
            }
            return new ChangeTextureMaterial(Resources.Load<Material>(WallMaterialPath + brushTextureData.E_name), ChangeTextureMaterialType.wall);
        }
        else
        {
            return null;
        }
    }
    public static ChangeTextureMaterial LoadDoorTexture(string name)
    {
        var data = brushDoorTextureDatas.Find(a => a.E_name == name);
        if (data!=null)
        {
            return LoadDoorTexture(data);
        }
        else
        {
            return null;
        }
    }
    public static ChangeTextureMaterial LoadWallTexture(string name)
    {
        var data = brushWallTextureDatas.Find(a => a.E_name == name);
        if (data==null)
        {
            string str = name.Substring(0, name.Length - 2);
            data = brushWallTextureDatas.Find(a => a.E_name == str);
            return LoadWallTexture(data, true);
        }
        return LoadWallTexture(data);
    }

    public static string GetMaterialName(GameObject obj)
    {
        string str = obj.GetComponent<MeshRenderer>().material.name;
        //Debug.LogError(str);
        str = str.Replace(" (Instance)", "");
        //str = str.Substring(0, str.Length - 11);
        //Debug.LogError(str);
        return str;
    }
    public static string GetMaterialName(string str)
    {
        str = str.Substring(0, str.Length - 11);
        return str;
    }
}

