public class P_SoftLandingState : P_LandingState
{
    public P_SoftLandingState(Player player, PlayerStateMachine machine) : base(player, machine) { }

    public override void OnEnter()
    {
        base.OnEnter();
        machine.StartAnimation(player.playerAnimationData.SoftLandingParameterHash);
    }

    public override void OnExit()
    {
        base.OnExit();
        machine.StopAnimation(player.playerAnimationData.SoftLandingParameterHash);
    }

    public override void OnAnimationExitEvent()
    {
        machine.OnStateChange(machine.IdleState);
    }
}
