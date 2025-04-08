using System.Collections.Generic;
using UnityEngine;

public class P_UC_Falling : P_UnControllable
{
    public P_UC_Falling(Player player, PlayerStateMachine machine) : base(player, machine) { }

    public override void OnEnter()
    {
        base.OnEnter();
        machine.StartAnimation(player.playerAnimationData.UC_FallingParameterHash);
    }

    public override void OnExit()
    {
        base.OnExit();
        machine.StopAnimation(player.playerAnimationData.UC_FallingParameterHash);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }
}