using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Undo.Commands
{
    public class DeleteObjUndoCommand:UndoRedoCommand
    {
        private CreateObjUndoComponent _undoComponent;
        private CreateUndoObjInfo _objInfo;


        public DeleteObjUndoCommand(CreateObjUndoComponent undoComponent, string commandName, CreateUndoObjInfo data)
        {
            _undoComponent = undoComponent;
            NameOfCommand = commandName;
            _objInfo = data;
        }

        public override void RedoCommand()
        {
            base.RedoCommand();
        }

        public override void UndoCommand()
        {
            base.UndoCommand();
            UndoRedoManager.Instance.StartCoroutine(UndoRedoManager.Instance.CreateGameObjByUndo(_objInfo));
           // Manager.Instace.StartCoroutine(Manager.Instace.CreateGameObj(_objInfo.Tag, _objInfo.ModelPath, _objInfo.Name, _objInfo.ImagePath));
        }
    }
}
