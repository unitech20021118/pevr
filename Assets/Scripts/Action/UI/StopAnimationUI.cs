public class StopAnimationUI : ActionUI
{
    private StopAnimation stopAnimation;
    private StopAnimationInforma stopAnimationInforma;
    public override Action<Main> CreateAction()
    {
        action = new StopAnimation();
        actionInforma = new StopAnimationInforma(true);
        GetStateInfo().actionList.Add(actionInforma);
        actionInforma.name = "StopAnimation";
        return base.CreateAction();
    }
    public override Action<Main> LoadAction(ActionInforma actionInforma)
    {
        stopAnimationInforma = (StopAnimationInforma)actionInforma;
        this.actionInforma = actionInforma;
        action = new StopAnimation();
        stopAnimation = (StopAnimation)action;


        return base.LoadAction(actionInforma);
    }
}
