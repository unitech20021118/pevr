using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class G_MoveObject : MonoBehaviour {

    private GameObject target;
    Vector3 lastPos;
    Vector3 offset;
    public Text text;

    void Start()
    {
        G_MouseListener.GetInstance().AddLMouseUp(Drop);
    }

    public void Drop()
    {
        if (!Input.GetKeyDown(KeyCode.LeftAlt))
        {
            target = null;
            Manager.Instace.ShowGameObjectProperty();
        }

    }


    /// <summary>
    /// 选中目标
    /// </summary>
    /// <param name="gameobject"></param>
    public void SetTarget(GameObject gameobject)
    {
        target = gameobject;
        
        if (target == null)
        {
            return;
        }
        //G_TranformUI.Instance.moveTarget = target;  //给属性窗口选中显示目标
        G_TranformUI.Instance.SetObj(target);
        //Debug.LogError("!!!!!!!!!!!!!!!");
        Manager.Instace.SetEditorObjectTarget(target);
        Manager.Instace.AddFSM.SetActive(false);
        Manager.Instace.addStatePanel.SetActive(false);
        //foreach (Transform t in Manager.Instace.pannel)
        //{
        //    t.gameObject.SetActive(false);
        //}
        
        
        text.text = target.name;
        lastPos = Input.mousePosition;
        Vector3 temp = G_RayCastCheck.WorldPosToScreenPos(target.transform.position);
        offset = temp - lastPos;
    }

    void Update()
    {
        if (target == null)
        {
            return;
        }
        //MoveTarget();//物体通过拖拽移动
    }

    void MoveTarget()
    {
        target.transform.position = GetTargetWorldPos();
        lastPos = Input.mousePosition;
    }

    /// <summary>
    /// 获得目标的世界坐标
    /// </summary>
    /// <returns></returns>
    Vector3  GetTargetWorldPos()
    {
        Vector3 groundPos;
        if (G_RayCastCheck.QuiescentObjectCheckGround(out groundPos, G_PubDef.QuiescentObject))
        {
            return groundPos;
        }
        Vector3 tempos = offset + lastPos;
        tempos = G_RayCastCheck.ScreenPosToWorldPos(tempos);
        return tempos;
    }
    /// <summary>
    /// 选中展示目标(PI)
    /// </summary>
    public void SetPITarget(GameObject obj)
    {
        

        if (obj == null)
        {
            return;
        }
        else
        {
            target = obj;
        }
        Manager.Instace.gameObject.GetComponent<G_EditorTarget>().SetTargetPRSTrue(target);
    }
}
