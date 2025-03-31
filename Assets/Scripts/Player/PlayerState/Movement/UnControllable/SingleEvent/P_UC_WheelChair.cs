using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class P_UC_WheelChair : P_UC_SingleEvent
{
    private int inputCount = 0;
    private const int inputThreshold = 20;
    private float shakeIntensity = 0.03f;

    public P_UC_WheelChair(Player player, PlayerStateMachine machine) : base(player, machine) { }

    public override void OnEnter()
    {
        base.OnEnter();
        machine.StartAnimation(player.playerAnimationData.WheelChairParameterHash);
        inputCount = 0;
    }

    public override void OnExit()
    {
        base.OnExit();
        machine.StopAnimation(player.playerAnimationData.WheelChairParameterHash);
        // 종료 시 흔들림 효과 중단
        player.transform.DOKill();
        player.curInteractableObject = null;
        player.curSingleEventObject = null;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        
        // 수평 또는 수직 입력이 있을 때 카운트 증가
        if (Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Vertical"))
        {
            inputCount++;
            
            // 휠체어 흔들림 효과 추가
            ShakeWheelchair();
            
            // 입력 횟수가 임계값을 넘으면 FallDown 상태로 전환
            if (inputCount >= inputThreshold)
            {
                player.bCanExit = true;
                machine.OnStateChange(machine.UC_FallDownState);
            }
        }
    }
    
    private void ShakeWheelchair()
    {
        // 입력 횟수에 따라 흔들림 강도 증가
        float currentIntensity = shakeIntensity * (1 + (float)inputCount / inputThreshold);
        
        // 랜덤한 방향으로 약간 흔들림
        Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
        player.transform.DOShakePosition(0.1f, randomDirection * currentIntensity, 10, 90, false, false);
        
        // 회전 흔들림도 추가
        player.transform.DOShakeRotation(0.1f, new Vector3(0, 0, currentIntensity * 5), 10, 90, false);
    }
    
    public override void SetDirection()
    {
        player.curDirection = player.curInteractableObject.transform.position - player.transform.position;
    }
}
