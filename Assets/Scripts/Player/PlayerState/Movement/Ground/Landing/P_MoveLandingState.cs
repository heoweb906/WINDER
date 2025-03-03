public class P_MoveLandingState : P_LandingState
{
    public P_MoveLandingState(Player player, PlayerStateMachine machine) : base(player, machine) { }

    public override void OnEnter()
    {
        base.OnEnter();
        machine.StartAnimation(player.playerAnimationData.MoveLandingParameterHash);
    }

    public override void OnExit()
    {
        base.OnExit();
        machine.StopAnimation(player.playerAnimationData.MoveLandingParameterHash);
    }

    public override void OnAnimationExitEvent()
    {
        machine.OnStateChange(machine.WalkingState);
    }
}
