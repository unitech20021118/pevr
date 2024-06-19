namespace Assets.Scripts.Undo.Commands
{
    //Editby 潘晓峰
    public class CreateObjUndoCommand:UndoRedoCommand
    {
        private CreateObjUndoComponent _undoComponent;

        public CreateObjUndoCommand(CreateObjUndoComponent undoComponent,string commandName)
        {
            _undoComponent = undoComponent;
            NameOfCommand = commandName;
        }

        public override void RedoCommand()
        {
            base.RedoCommand();
        }

        public override void UndoCommand()
        {
            base.UndoCommand();
            _undoComponent.CreateObjUndo();
        }
    }
}
