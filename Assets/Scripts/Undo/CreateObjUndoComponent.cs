using Assets.Scripts.Undo.Commands;
using UnityEngine;

//Editby 潘晓峰
namespace Assets.Scripts.Undo
{
    public class CreateObjUndoComponent:MonoBehaviour
    {
        private CreateUndoObjInfo _beforeObjInfo;

        public void Init(CreateUndoObjInfo objInfo)
        {
            _beforeObjInfo = objInfo;
            AddCommand();
        }

        public void AddCommand()
        {
            // UndoRedoManager.Instance.AddCommand(new CreateObjUndoCommand(this,"添加物体"));
        }

        public void CreateObjUndo()
        {
           // Manager.Instace.GetComponent<G_EditorTarget>().UnDoDelete(_createdObject);
        }

        public void AddDeleteUnDoCommand(CreateUndoObjInfo data)
        {
            data.Tag = _beforeObjInfo.Tag;
            data.ModelPath = _beforeObjInfo.ModelPath;
            data.ImagePath = _beforeObjInfo.ImagePath;
            data.Name = _beforeObjInfo.Name;
            var deleteUndoCommand = new DeleteObjUndoCommand(this, "删除物体", data);
            UndoRedoManager.Instance.AddCommand(deleteUndoCommand);
        }
    }

    public class CreateUndoObjInfo
    {
        public Vector3 LocalScale { get; set; }
        public Quaternion Rotation { get; set; }
        public Vector3 Position { get; set; }
        public string Tag { get; set; }
        public string Name { get; set; }
        public string ModelPath { get; set; }
        public string ImagePath { get; set; }
        public ListTreeNode<Base> Data { get; set; }
    }
}
