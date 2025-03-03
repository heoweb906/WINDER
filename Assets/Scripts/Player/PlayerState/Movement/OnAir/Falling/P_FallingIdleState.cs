public class P_FallingIdleState : P_FallingState
{
    public P_FallingIdleState(Player player, PlayerStateMachine machine) : base(player, machine) { }

    public override void OnEnter()
    {
        base.OnEnter();
        machine.StartAnimation(player.playerAnimationData.FallingIdleParameterHash);
    }

    public override void OnExit()
    {
        base.OnExit();
        machine.StopAnimation(player.playerAnimationData.FallingIdleParameterHash);
    }


}

