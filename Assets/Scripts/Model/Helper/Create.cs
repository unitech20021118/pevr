using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Create : MonoBehaviour
{

    GameObject mainCamera;
    public int DefaultZ = 10;
    private GameObject createObject;
    public GameObject parent;

    void Awake()
    {
        G_MouseListener.GetInstance().AddLMouseUp(DropObject);
        //G_MouseListener.Instance.AddLMouseDown(DropObject);
        mainCamera = GameObject.Find("Main Camera");
    }

    void Start()
    {

    }

    void Update()
    {
        if (createObject == null)
        {
            return;
        }
        createObject.transform.position = GetWorldPos();
    }

    public void DropObject()
    {

        createObject = null;
    }

    public void CreateGameObject(GameObject creatObject, bool canMove)
    {
        createObject = Instantiate<GameObject>(creatObject);
        
        createObject.transform.parent = parent.transform;
        createObject.transform.position = GetWorldPos();
        if (canMove)
        {
            createObject.layer = G_PubDef.dynamicObject;
            foreach (Transform i in createObject.transform)
            {
                i.gameObject.layer = G_PubDef.dynamicObject;
            }
        }
        else
        {
            createObject.layer = G_PubDef.quiescentObject;
            foreach (Transform i in createObject.transform)
            {
                i.gameObject.layer = G_PubDef.quiescentObject;
            }
        }

    }


    public Vector3 GetWorldPos()
    {
        Vector3 groundPos;
        if (G_RayCastCheck.QuiescentObjectCheckGround(out groundPos, G_PubDef.QuiescentObject))
        {
            return groundPos;
        }

        Vector3 tempPos = Input.mousePosition;
        tempPos.z = mainCamera.transform.position.z + DefaultZ;
        tempPos = G_RayCastCheck.ScreenPosToWorldPos(tempPos);
        return tempPos;
    }
}
