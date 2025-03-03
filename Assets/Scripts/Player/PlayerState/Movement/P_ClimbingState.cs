using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_ClimbingState : PlayerMovementState
{
    public P_ClimbingState(Player player, PlayerStateMachine machine) : base(player, machine) { }

    public override void OnEnter()
    {
        base.OnEnter();
        machine.StartAnimation(player.playerAnimationData.ClimbingParameterHash);

        player.rigid.useGravity = false;
        player.rigid.velocity = Vector3.zero;
        player.playerMoveSpeed = 0;
    }

    public override void OnExit()
    {
        base.OnExit();
        machine.StopAnimation(player.playerAnimationData.ClimbingParameterHash);
        player.rigid.useGravity = true;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

    }

    public override void SetDirection()
    {
        player.curDirection = player.cliffRayHit.point - player.transform.position;
    }
}
