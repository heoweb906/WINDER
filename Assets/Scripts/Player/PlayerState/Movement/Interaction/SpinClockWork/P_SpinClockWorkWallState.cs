using System.Collections.Generic;
using UnityEngine;

public class P_SpinClockWorkWallState : P_SpinClockWorkState
{
    public P_SpinClockWorkWallState(Player player, PlayerStateMachine machine) : base(player, machine) { }

    public override void OnEnter()
    {
        base.OnEnter();
        machine.StartAnimation(player.playerAnimationData.SpinClockWorkWallParameterHash);
        player.playerMoveSpeed = 0;
    }

    public override void OnExit()
    {
        base.OnExit();
        machine.StopAnimation(player.playerAnimationData.SpinClockWorkFloorParameterHash);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }
}
