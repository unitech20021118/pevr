using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Brush;

namespace Brush
{
    [System.Serializable]
    public class BrushedObjInformation
    {
        /// <summary>
        /// 绘制出的建筑
        /// </summary>
        public List<Building> buildings = new List<Building>();
        /// <summary>
        /// 绘制出的墙壁
        /// </summary>
        public List<Wall> walls = new List<Wall>();
        /// <summary>
        /// 绘制出的路面
        /// </summary>
        public List<Road> roads = new List<Road>();

    }
    [System.Serializable]
    public class Wall
    {
        /// <summary>
        /// 样式名
        /// </summary>
        public string style;
        public float width;
        public float height;
        public float heightAboveGround;
        public MySerializableVector3 startPoint;
        public MySerializableVector3 endPoint;
        /// <summary>
        /// 该墙壁上的所有门
        /// </summary>
        public List<Door> doors = new List<Door>();
        /// <summary>
        /// 该墙壁上的所有窗户
        /// </summary>
        public List<Window> windows = new List<Window>();
    }
    [System.Serializable]
    public class Door
    {
        /// <summary>
        /// 样式名
        /// </summary>
        public string style;
        public float width;
        public float height;
        public float length;
        public float heightAboveGround;
        public float distanceScale;
    }
    [System.Serializable]
    public class Window
    {
        /// <summary>
        /// 样式名
        /// </summary>
        public string style;
        public float width;
        public float height;
        public float length;
        public float heightAboveGround;
        public float distanceScale;
    }
    [System.Serializable]
    public class Ground
    {
        /// <summary>
        /// 样式名
        /// </summary>
        public string style;
        public float heightAboveGround;
        public MySerializableVector3[] points;
    }
    [System.Serializable]
    public class Floor
    {
        /// <summary>
        /// 该层建筑的墙壁
        /// </summary>
        public List<Wall> walls = new List<Wall>();
        /// <summary>
        /// 地板
        /// </summary>
        public Ground ground;
        /// <summary>
        /// 天花板
        /// </summary>
        public Ground ceiling;
    }
    [System.Serializable]
    public class Building
    {
        /// <summary>
        /// 建筑的层
        /// </summary>
        public List<Floor> floors = new List<Floor>();
        /// <summary>
        /// 建筑地基的点
        /// </summary>
        public MySerializableVector3[] points;
        /// <summary>
        /// 该建筑的位置
        /// </summary>
        public MySerializableVector3 position;
        /// <summary>
        /// 该建筑的旋转
        /// </summary>
        public MySerializableVector3 rotation;
    }
    [System.Serializable]
    public class Road
    {
        /// <summary>
        /// 样式名
        /// </summary>
        public string style;
        public float width;
        public float heightAboveGround;
        public MySerializableVector3 startPoint;
        public MySerializableVector3 endPoint;
    }


    
}

[System.Serializable]
public class AllDataInformation
{
    public  ListTree<Base> listTree;
    public BrushedObjInformation brushedObjInformation;
    public AllDataInformation()
    {

    }
    public AllDataInformation(ListTree<Base> listTree,BrushedObjInformation brushedObjInformation)
    {
        this.listTree = listTree;
        this.brushedObjInformation = brushedObjInformation;
    }
}
[System.Serializable]
public class MySerializableVector3
{
    public float X;
    public float Y;
    public float Z;
    public MySerializableVector3(float x,float y,float z)
    {
        X = x;
        Y = y;
        Z = z;
    }
}