public class P_FallingMoveState : P_FallingState
{
    public P_FallingMoveState(Player player, PlayerStateMachine machine) : base(player, machine) { }

    public override void OnEnter()
    {
        base.OnEnter();
        machine.StartAnimation(player.playerAnimationData.FallingMoveParameterHash);
    }

    public override void OnExit()
    {
        base.OnExit();
        machine.StopAnimation(player.playerAnimationData.FallingMoveParameterHash);
    }


}


