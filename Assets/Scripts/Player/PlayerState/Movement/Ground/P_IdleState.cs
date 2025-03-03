public class P_IdleState : P_GroundState
{
    public P_IdleState(Player player, PlayerStateMachine machine) : base(player, machine) { }

    public override void OnEnter()
    {
        base.OnEnter();
        machine.StartAnimation(player.playerAnimationData.IdleParameterHash);
    }

    public override void OnExit()
    {
        base.OnExit();
        machine.StopAnimation(player.playerAnimationData.IdleParameterHash);
    }
    public override void OnAnimationExitEvent()
    {
        player.playerAnim.applyRootMotion = false;
    }
}
