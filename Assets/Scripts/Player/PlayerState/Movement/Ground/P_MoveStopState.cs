public class P_MoveStopState : P_GroundState
{
    public P_MoveStopState(Player player, PlayerStateMachine machine) : base(player, machine) { }

    public override void OnEnter()
    {
        base.OnEnter();
        machine.StartAnimation(player.playerAnimationData.MoveStopParameterHash);
    }

    public override void OnExit()
    {
        base.OnExit();
        machine.StopAnimation(player.playerAnimationData.MoveStopParameterHash);
    }

    public override void OnAnimationExitEvent()
    {
        machine.OnStateChange(machine.IdleState);
    }
}
