public class P_JumpStartIdleState : P_JumpStartState
{
    public P_JumpStartIdleState(Player player, PlayerStateMachine machine) : base(player, machine) { }

    public override void OnEnter()
    {
        base.OnEnter();
        machine.StartAnimation(player.playerAnimationData.JumpStartIdleParameterHash);
    }

    public override void OnExit()
    {
        base.OnExit();
        machine.StopAnimation(player.playerAnimationData.JumpStartIdleParameterHash);
    }

    public override void OnAnimationExitEvent()
    {
        machine.OnStateChange(machine.FallingIdleState);
    }
}
