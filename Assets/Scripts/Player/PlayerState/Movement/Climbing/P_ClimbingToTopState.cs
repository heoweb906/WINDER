using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class P_ClimbingToTopState : P_ClimbingState
{
    public P_ClimbingToTopState(Player player, PlayerStateMachine machine) : base(player, machine) { }

    public override void OnEnter()
    {
        base.OnEnter();
        machine.StartAnimation(player.playerAnimationData.ClimbingToTopParameterHash);
    }

    public override void OnExit()
    {
        base.OnExit();
        machine.StopAnimation(player.playerAnimationData.ClimbingToTopParameterHash);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnAnimationEnterEvent()
    {
        player.playerAnim.applyRootMotion = true;
        player.playerAnim.updateMode = AnimatorUpdateMode.AnimatePhysics;
        player.SetColliderTrigger(false);
    }

    public override void OnAnimationTransitionEvent()
    {
        //player.StartExitClimbingToTop();
    }

    public override void OnAnimationExitEvent()
     {
        player.transform.position = new Vector3(player.transform.position.x, player.hangingPos.y, player.transform.position.z);
        player.SetColliderTrigger(true);
        player.playerAnim.applyRootMotion = false;
        player.playerAnim.updateMode = AnimatorUpdateMode.Normal;
        machine.OnStateChange(machine.IdleState);

    }

    public override void SetDirection()
    {
        return;
    }

    public override void PlayerRotationControll()
    {
        return;
    }

    
}


