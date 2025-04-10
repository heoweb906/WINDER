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
        
        // 플레이어 떨어지는 속도 제한
        if (player.rigid.velocity.y < -player.maxFallingSpeed)
        {
            Vector3 limitedVelocity = player.rigid.velocity;
            limitedVelocity.y = -player.maxFallingSpeed;
            player.rigid.velocity = limitedVelocity;
        }
    }
}