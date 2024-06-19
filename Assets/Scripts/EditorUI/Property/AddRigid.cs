using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRigid : MonoBehaviour,WindowInterface {
    public ObjectInfo b;
    GameObject editorObj;
	// Use this for initialization
	void Start () {


	}

    public void Add()
    {
        editorObj = Manager.Instace.gonggong;

        if (Manager.Instace.gonggong != null)
        {
            if (Manager.Instace.gonggong.GetComponent<Rigidbody>() == null)
            {
                Rigidbody rb = Manager.Instace.gonggong.AddComponent<Rigidbody>();
                rb.useGravity = false;
                rb.isKinematic = true;
                //int num = Manager.Instace.gonggong.GetComponent<GameObjectIndex>().index;
                if (Manager.Instace.dictFromObjectToInforma.ContainsKey(Manager.Instace.gonggong))
                {
                    b = (ObjectInfo)Manager.Instace.dictFromObjectToInforma[Manager.Instace.gonggong];
                    b.hasRigid = true;
                }


            }
        }
        else
        {
            Debug.Log("请选中物体");
        }
        
        Manager.Instace.gameObject.GetComponent<G_MouseListener>().RegisterOnClickObject(ShowObject);
    }

    public void Close()
    {
       b.hasRigid = false;
       Manager.Instace.objectPropertyDic.Remove(Manager.Instace.gonggong);
       Manager.Instace.gameObject.GetComponent<G_MouseListener>().DeleteOnClickObject(ShowObject);
       Destroy(Manager.Instace.gonggong.GetComponent<Rigidbody>());
       Destroy(gameObject);
    }

    public void ShowObject(GameObject obj)
    {
        if (obj == editorObj)
        {

            gameObject.SetActive(true);
        }
        else
        {

            gameObject.SetActive(false);
        }
    }
}
