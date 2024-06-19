using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddProperty : MonoBehaviour {
    GameObject prefab;
    GameObject currentObject;

    

    public void AddComponnent(int num)
    {
        switch (num)
        {
            case 1:
                    prefab = (GameObject)Resources.Load("Prefabs/AddCollider");


                break;
            case 2:                
                    prefab = (GameObject)Resources.Load("Prefabs/AddRigid");
                                    GameObject obj = Instantiate(prefab);                                    
                                    obj.GetComponent<AddRigid>().Add();               
                    if (!Manager.Instace.objectPropertyDic.ContainsKey(Manager.Instace.gonggong))
                     {

                         obj.transform.SetParent(transform);
                         obj.transform.localScale = Vector3.one;
                         List<GameObject> temp=new List<GameObject>();
                        temp.Add(obj);
                        Manager.Instace.objectPropertyDic.Add(Manager.Instace.gonggong, temp);
                       
                     }
                    else
                    {
                        //int ni = Manager.Instace.gonggong.GetComponent<GameObjectIndex>().index;
                        ObjectInfo o = (ObjectInfo)Manager.Instace.dictFromObjectToInforma[Manager.Instace.gonggong];
                        if (!o.hasRigid)
                        {
                            obj.transform.SetParent(transform);
                            obj.transform.localScale = Vector3.one;
                            Manager.Instace.objectPropertyDic[Manager.Instace.gonggong].Add(obj);
                            
                        }
                        else
                        {
                            Destroy(obj);
                        }
                    }
                break;
        } 
    }

    public void close()
    {
        this.gameObject.SetActive(false);

    }
}
