using System.Collections.Generic;
using UnityEngine;

public class P_UC_SingleEvent : P_UnControllable
{
    public P_UC_SingleEvent(Player player, PlayerStateMachine machine) : base(player, machine) { }

    public override void OnEnter()
    {
        base.OnEnter();
        machine.StartAnimation(player.playerAnimationData.SingleEventParameterHash);
        player.curSingleEventObject.ActiveEvent();
        if(player.curSingleEventObject.singleEventType == SingleEventType.WheelChair){
            machine.OnStateChange(machine.UC_WheelChairState);
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        machine.StopAnimation(player.playerAnimationData.SingleEventParameterHash);
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
