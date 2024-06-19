using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Product : MonoBehaviour {
    //public static Dictionary<string, List<GameObject>> dict = new Dictionary<string, List<GameObject>>();
    public static Dictionary<string, List<int>> dict = new Dictionary<string, List<int>>();
    public static Dictionary<string, List<GameObject>> removeList = new Dictionary<string, List<GameObject>>();

    //public static void ManagerGameobject(string name,GameObject obj)
    //{
    //    if (!dict.ContainsKey(name))
    //    {
    //        List<GameObject> gameList = new List<GameObject>();
    //        obj.name = name +"_" +gameList.Count.ToString();
    //        gameList.Add(obj);
    //        dict.Add(name, gameList);
            
    //    }
    //    else
    //    {
    //        List<GameObject> temp = dict[name];           
    //        int length = temp.Count;
    //        obj.name = name + "_" + length.ToString();
    //        temp.Add(obj);
            
            
    //    }
    //}

    public static void ManagerGameobject(string name, GameObject obj)
    {
        if (!dict.ContainsKey(name))
        {
            List<int> gameList = new List<int>();
            gameList.Add(0);
            print(obj.name);
            obj.name = name + "*"+gameList[0];
            //gameList.Add(gameList.Count);
            dict.Add(name, gameList);

        }
        else
        {
            List<int> temp = dict[name];
            int num;
            Traverse(temp, out num);
            obj.name = name + "*" + num;
            



        }
    }

    static void  Traverse(List<int> list,out int n)
    {
        n=0;
        while (true)
        {
            if(list[n]==n)
            {
                n++;
                if (n>list.Count-1)
                {
                    list.Add(n);
                    break;
                }
            }
            else
            {
                list[n] = n;
                break;
            }
        }
    }

    // <summary>
    // 删除问题？？
    // </summary>
    // <param name="obj"></param>
    //public static void DeleteGameObject(GameObject obj)
    //{
    //    string[] names = obj.name.Split('_');

    //    List<GameObject> temp = dict[names[0]];
    //    temp.Remove(obj);

    //    if (!removeList.ContainsKey(names[0]))
    //    {
    //        List<GameObject> gameList = new List<GameObject>();
            
    //        gameList.Add(obj);
    //        removeList.Add(names[0], gameList);
    //    }
    //    else
    //    {
    //        List<GameObject> temps = removeList[names[0]];
    //        temps.Add(obj);
    //    }
       
    //}
    public static void DeleteGameObject(GameObject obj)
    {
        string[] names = obj.name.Split('*');

        List<int> temp = dict[names[0]];
        //temp.Remove(obj);
        //temp.RemoveAt(int.Parse(names[1]));
        temp[int.Parse(names[1])] = -1;
        //if (!removeList.ContainsKey(names[0]))
        //{
        //    List<GameObject> gameList = new List<GameObject>();

        //    gameList.Add(obj);
        //    removeList.Add(names[0], gameList);
        //}
        //else
        //{
        //    List<GameObject> temps = removeList[names[0]];
        //    temps.Add(obj);
        //}

    }

}
