using System.Collections.Generic;
using UnityEngine;

public class P_UC_Turn : P_UnControllable
{
    public P_UC_Turn(Player player, PlayerStateMachine machine) : base(player, machine) { }

    public override void OnEnter()
    {
        base.OnEnter();
        // 회전 방향 체크
        Vector3 targetDirection = player.turnDirection.normalized;
        Vector3 playerForward = player.transform.forward;
        
        // 외적을 통해 왼쪽/오른쪽 회전 여부 판단
        float crossProduct = Vector3.Cross(playerForward, targetDirection).y;
        
        if (crossProduct > 0)
        {
            // 양수면 왼쪽으로 회전
            machine.StartAnimation(player.playerAnimationData.UC_Turn_LeftParameterHash);
        }
        else
        {
            // 음수면 오른쪽으로 회전
            machine.StartAnimation(player.playerAnimationData.UC_Turn_RightParameterHash);
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        machine.StopAnimation(player.playerAnimationData.UC_Turn_LeftParameterHash);
        machine.StopAnimation(player.playerAnimationData.UC_Turn_RightParameterHash);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void SetDirection()
    {
        player.curDirection = player.turnDirection.normalized;
    }
    public override void PlayerRotationControll()
    {
        if (player.turnTargetObject != null)
        {
            Vector3 targetDirection = player.turnDirection.normalized;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            
            // 부드러운 회전 적용
            player.transform.rotation = Quaternion.Slerp(
                player.transform.rotation, 
                targetRotation, 
                Time.deltaTime * 5f
            );
            
            // 회전이 거의 완료되면 상태 변경
            if (Quaternion.Angle(player.transform.rotation, targetRotation) < 1f)
            {
                machine.OnStateChange(machine.UC_IdleState);
            }
        }
    }
}