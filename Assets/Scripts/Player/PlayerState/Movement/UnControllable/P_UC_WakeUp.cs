using System.Collections.Generic;
using UnityEngine;

public class P_UC_WakeUp : P_UnControllable
{
    public P_UC_WakeUp(Player player, PlayerStateMachine machine) : base(player, machine) { }

    public override void OnEnter()
    {
        base.OnEnter();
        machine.StartAnimation(player.playerAnimationData.UC_WakeUpParameterHash);
        ;
    }

    public override void OnExit()
    {
        base.OnExit();
        machine.StopAnimation(player.playerAnimationData.UC_WakeUpParameterHash);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }
    public override void SetDirection()
    {
        return;
    }

    public override void OnAnimationExitEvent()
    {
        base.OnAnimationExitEvent();
        machine.OnStateChange(machine.IdleState);
    }
}
