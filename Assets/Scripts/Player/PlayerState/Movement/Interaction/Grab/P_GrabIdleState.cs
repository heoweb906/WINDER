using UnityEngine;

public class P_GrabIdleState : P_GrabState
{
    public P_GrabIdleState(Player player, PlayerStateMachine machine) : base(player, machine) { }

    public override void OnEnter()
    {
        base.OnEnter();
        machine.StartAnimation(player.playerAnimationData.GrabIdleParameterHash);
    }

    public override void OnExit()
    {
        base.OnExit();
        machine.StopAnimation(player.playerAnimationData.GrabIdleParameterHash);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

}
