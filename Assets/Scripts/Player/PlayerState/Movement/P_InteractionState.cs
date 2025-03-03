using System.Collections.Generic;
using UnityEngine;

public class P_InteractionState : PlayerMovementState
{
    public P_InteractionState(Player player, PlayerStateMachine machine) : base(player, machine) { }

    public override void OnEnter()
    {
        base.OnEnter();
        machine.StartAnimation(player.playerAnimationData.InteractionParameterHash);
        player.playerMoveSpeed = 0;
        player.isRun = false;
    }

    public override void OnExit()
    {
        base.OnExit();
        machine.StopAnimation(player.playerAnimationData.InteractionParameterHash);
        player.isGoToTarget = false;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }
    public override void SetDirection()
    {
        return;
    }

}
