//Edit by 潘晓峰
namespace Assets.Scripts.Undo.Commands
{
    public class UndoRedoCommand:object
    {
        public string NameOfCommand { get; set; }
        public virtual void RedoCommand() { }
        public virtual void UndoCommand() { }
    }
}
