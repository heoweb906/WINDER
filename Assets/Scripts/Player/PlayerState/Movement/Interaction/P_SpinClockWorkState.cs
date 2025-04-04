using System;
using System.Collections.Generic;
using UnityEngine;

public class P_SpinClockWorkState : P_InteractionState
{
    public P_SpinClockWorkState(Player player, PlayerStateMachine machine) : base(player, machine) { }

    bool bCanExit = false;

    public override void OnEnter()
    {
        base.OnEnter();
        machine.StartAnimation(player.playerAnimationData.SpinClockWorkParameterHash);
    }

    public override void OnExit()
    {
        base.OnExit();
        machine.StopAnimation(player.playerAnimationData.SpinClockWorkParameterHash);
        player.curClockWork.EndCharging_To_BatteryStart();
        // player.curClockWork = null;
        player.curInteractableObject = null;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        CheckCanExit();
    }

    public override void SetDirection()
    {
        player.curDirection = player.curInteractableObject.transform.position - player.transform.position;
    }

    private void CheckCanExit()
    {
        if (player.curClockWork.BoolBatteryFullCharging())
        {
            machine.OnStateChange(machine.IdleState);
        }

        if (!bCanExit) return;
        if(player.curClockWork == null) Debug.Log("player.curClockWork == null");
        if (player.curClockWork.isSingleEvent) return;
        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical") || !Input.GetButton("Fire1"))
        {
            machine.OnStateChange(machine.IdleState);
            return;
        }
        
    }

    public override void OnAnimationEnterEvent()
    {
        bCanExit = false;
        player.curClockWork.ClockWorkRotate();
    }

    public override void OnAnimationExitEvent()
    {
        bCanExit = true;
        player.curClockWork.ChargingBattery();

        
    }

}
