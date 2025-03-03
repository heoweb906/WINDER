public class P_JumpStartState : P_OnAirState
{
    public P_JumpStartState(Player player, PlayerStateMachine machine) : base(player, machine) { }

    public override void OnEnter()
    {
        base.OnEnter();
        machine.StartAnimation(player.playerAnimationData.JumpStartParameterHash);
        PlayerJump();
    }

    public override void OnExit()
    {
        base.OnExit();
        machine.StopAnimation(player.playerAnimationData.JumpStartParameterHash);
    }


}
