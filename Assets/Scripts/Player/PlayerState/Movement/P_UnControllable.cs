using System.Collections.Generic;
using UnityEngine;

public class P_UnControllable : PlayerMovementState
{
    public P_UnControllable(Player player, PlayerStateMachine machine) : base(player, machine) { }

    public override void OnEnter()
    {
        base.OnEnter();
        machine.StartAnimationTrigger(player.playerAnimationData.UnControllableParameterHash);
        player.playerMoveSpeed = 0;
        player.curDirection = Vector3.zero;
    }

    public override void OnExit()
    {
        base.OnExit();
        machine.StartAnimationTrigger(player.playerAnimationData.UnControllableParameterHash);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void SetDirection()
    {
        return;
    }
    
    public override void OnTriggerExit(Collider other)
    {
        return;
    }
    public override void OnAnimationExitEvent()
    {
        base.OnAnimationExitEvent();
    }
}
