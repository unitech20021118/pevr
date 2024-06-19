using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddCollider : MonoBehaviour, WindowInterface
{
    ObjectInfo b;
	// Use this for initialization
	void Start () {
        if (Manager.Instace.gonggong != null)
        {
            if (Manager.Instace.gonggong.GetComponent<Collider>() == null)
            {
                Manager.Instace.gonggong.AddComponent<Collider>();
                //int num = Manager.Instace.gonggong.GetComponent<GameObjectIndex>().index;
                b = (ObjectInfo)Manager.Instace.dictFromObjectToInforma[Manager.Instace.gonggong];
                b.hasCollider = true;
            }
        }
        else
        {
            Debug.Log("请选中物体");
        }
	}

    public void Close()
    {
        b.hasCollider = false;
        Destroy(Manager.Instace.gonggong.GetComponent<Collider>());
        Destroy(gameObject);
    }
}
