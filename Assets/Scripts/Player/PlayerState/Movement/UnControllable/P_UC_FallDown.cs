using System.Collections.Generic;
using UnityEngine;

public class P_UC_FallDown : P_UnControllable
{
    public P_UC_FallDown(Player player, PlayerStateMachine machine) : base(player, machine) { }


    public override void OnEnter()
    {
        base.OnEnter();
        machine.StartAnimation(player.playerAnimationData.UC_FallDownParameterHash);
        ;
    }

    public override void OnExit()
    {
        base.OnExit();
        machine.StopAnimation(player.playerAnimationData.UC_FallDownParameterHash);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }
    public override void SetDirection()
    {
        if(!player.bCanExit) return;
        if(Input.GetButton("Horizontal") || Input.GetButton("Vertical") || Input.GetButton("Fire1")){
            machine.OnStateChange(machine.UC_WakeUpState);
        }
        return;
    }

    public override void OnAnimationTransitionEvent()
    {
        base.OnAnimationTransitionEvent();
    }

}
