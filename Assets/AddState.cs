using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AddState : MonoBehaviour
{


    public void Add()
    {
        if (Manager.Instace.prefabState == null)
        {
            Manager.Instace.prefabState = (GameObject)Resources.Load("Prefabs/One");
        }
        GameObject obj = Instantiate(Manager.Instace.prefabState);
        StateNode stateNode = obj.GetComponentInChildren<StateNode>();
        stateNode.TargetTransform = Manager.Instace.gonggong.transform;
        obj.transform.SetParent(Manager.Instace.objectToFsm[ Manager.Instace.gonggong].transform);
        obj.transform.localScale = Vector3.one;
        //obj.transform.localPosition = Vector3.zero;
        Vector3 pos;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(Manager.Instace.StateMachineCanvas.GetComponent<RectTransform>(), Input.mousePosition, Manager.Instace.StateMachineCamera, out pos);
        obj.transform.position = pos ;
        transform.parent.gameObject.SetActive(false);
        Debug.Log(Manager.Instace.gonggong);

        StateInfo stateData = new StateInfo();
        //stateData.StateTargetGameObjectPath = Manager.Instace.GetGongGongPath(Manager.Instace.gonggong.transform);
        //obj.AddComponent<GameObjectIndex>().index = stateData.index;
        //Informa<Base>.allData.Add(stateData);
        Manager.Instace.dictFromObjectToInforma.Add(obj, stateData);
        Debug.LogError(obj.name);
        ListTreeNode<Base> stateParent = Manager.Instace.allDataInformation.listTree.GetNode(Manager.Instace.dictFromObjectToInforma[Manager.Instace.gonggong]).children[0];
        Manager.Instace.allDataInformation.listTree.AddLeave(stateParent, stateData);

        //FSMInfo temp = new FSMInfo();
        //temp.name=
//<<<<<<< .mine
////        StateInfo temp = new StateInfo();
////        temp.name = obj.GetComponentInChildren<Text>().text;
////        temp.parent = Manager.Instace.currentStateStart.transform;
////        temp.transform = obj.transform;
////        temp.prefabPath = StatePath;
////        Base.allData.Add(temp);
////        obj.AddComponent<GameObjectIndex>().index = Base.id;
////        Base b = Base.FindData(Manager.Instace.currentStateStart.GetComponent<GameObjectIndex>().index);
////        ListTreeNode<Base> parent= Manager.Instace.listTree.GetNode(b);//获得双亲节点
//||||||| .r16
//        StateInfo temp = new StateInfo();
//        temp.name = obj.GetComponentInChildren<Text>().text;
//        temp.parent = Manager.Instace.currentStateStart.transform;
//        temp.transform = obj.transform;
//        temp.prefabPath = StatePath;
//        Base.allData.Add(temp);
//        obj.AddComponent<GameObjectIndex>().index = Base.id;
//        Base b = Base.FindData(Manager.Instace.currentStateStart.GetComponent<GameObjectIndex>().index);
//        ListTreeNode<Base> parent= Manager.Instace.listTree.GetNode(b);//获得双亲节点
//=======
//        //StateInfo temp = new StateInfo();
//        //temp.name = obj.GetComponentInChildren<Text>().text;
//        //temp.parent = Manager.Instace.currentStateStart.transform;
//        //temp.transform = obj.transform;
//        ////temp.prefabPath = StatePath;
//        //Base.allData.Add(temp);
//        //obj.AddComponent<GameObjectIndex>().index = Base.id;
//        //Base b = Base.FindData(Manager.Instace.currentStateStart.GetComponent<GameObjectIndex>().index);
//        //ListTreeNode<Base> parent= Manager.Instace.listTree.GetNode(b);//获得双亲节点
//>>>>>>> .r17
//        //FSMInfo temp = new FSMInfo();
//        //temp.name = obj.GetComponentInChildren<Text>().text;
//        //temp.parent = Manager.Instace.currentStateStart.transform;
//        //temp.transform = obj.transform;
//        //temp.prefabPath = StatePath;
//        //Manager.Instace.listTree.AddLeave(parent, temp);
//
//<<<<<<< .mine
////        Manager.Instace.listTree.AddLeave(parent, temp);
//||||||| .r16
//        Manager.Instace.listTree.AddLeave(parent, temp);
//=======
//        //Manager.Instace.listTree.AddLeave(parent, temp);
//>>>>>>> .r17
    }
}
