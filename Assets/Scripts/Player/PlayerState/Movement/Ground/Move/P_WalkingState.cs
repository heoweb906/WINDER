using UnityEngine;

public class P_WalkingState : P_MoveState
{
    public P_WalkingState(Player player, PlayerStateMachine machine) : base(player, machine) { }

    public override void OnEnter()
    {
        base.OnEnter();
        machine.StartAnimation(player.playerAnimationData.WalkingParameterHash);
        player.playerMoveSpeed = player.playerWalkSpeed;

    }

    public override void OnExit()
    {
        base.OnExit();
        machine.StopAnimation(player.playerAnimationData.WalkingParameterHash);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

    }

}
