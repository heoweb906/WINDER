public class P_JumpStartMoveState : P_JumpStartState
{
    public P_JumpStartMoveState(Player player, PlayerStateMachine machine) : base(player, machine) { }

    public override void OnEnter()
    {
        base.OnEnter();
        machine.StartAnimation(player.playerAnimationData.JumpStartMoveParameterHash);
    }

    public override void OnExit()
    {
        base.OnExit();
        machine.StopAnimation(player.playerAnimationData.JumpStartMoveParameterHash);
    }

    public override void OnAnimationExitEvent()
    {
        machine.OnStateChange(machine.FallingMoveState);
    }
}

