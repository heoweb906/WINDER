public class P_RunningState : P_MoveState
{
    public P_RunningState(Player player, PlayerStateMachine machine) : base(player, machine) { }

    public override void OnEnter()
    {
        base.OnEnter();
        machine.StartAnimation(player.playerAnimationData.RunningParameterHash);
        player.playerMoveSpeed = player.playerRunSpeed;
    }

    public override void OnExit()
    {
        base.OnExit();
        machine.StopAnimation(player.playerAnimationData.RunningParameterHash);
    }

}
