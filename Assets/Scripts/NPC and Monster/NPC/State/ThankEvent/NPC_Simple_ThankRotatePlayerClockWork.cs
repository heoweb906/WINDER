using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// #. NPC가 플레이어의 뒤쪽으로 반원을 그리며 이동하는 상태
public class NPC_Simple_ThankRotatePlayerClockWork : NPC_Simple_State
{
    public NPC_Simple_ThankRotatePlayerClockWork(NPC_Simple npc, NPC_Simple_StateMachine machine) : base(npc, machine) { }

    private bool bStart = false;

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("진입 완료");

        npc.GetNav().isStopped = false;
        npc.GetNav().autoBraking = false;

        npc.checkPoints = new Transform[2];
        GameObject objPlayer = npc.GetPlayerObject();

        // 1. 첫 번째 목표 위치 계산
        Vector3 position1 = objPlayer.transform.position + objPlayer.transform.right + objPlayer.transform.forward * 1f;
        GameObject tempObject1 = new GameObject("Position1");
        tempObject1.transform.position = position1; // 새로운 GameObject로 위치 설정
        npc.checkPoints[0] = tempObject1.transform; // 배열에 저장

        // 2. 두 번째 목표 위치 계산
        Vector3 position2 = objPlayer.transform.position + objPlayer.transform.right + objPlayer.transform.forward * -1f;
        GameObject tempObject2 = new GameObject("Position2");
        tempObject2.transform.position = position2; // 새로운 GameObject로 위치 설정
        npc.checkPoints[1] = tempObject2.transform; // 배열에 저장

        // 배열 내 위치 출력
        for (int i = 0; i < npc.checkPoints.Length; i++)
        {
            Debug.Log($"Position {i + 1}: {npc.checkPoints[i].position}");
        }

        npc.GetAnimator().SetInteger("Walk_Num", npc.iAnimWalking);

        bStart = true;
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        Debug.Log("asdasdasdasdasd");

        // checkPoints 배열이 null이거나 길이가 0일 경우 빠져나옵니다.
        if (npc.checkPoints == null || npc.checkPoints.Length == 0)
        {
            Debug.Log("[OnFixedUpdate] checkPoints가 초기화되지 않았습니다.");
            return;
        }

        if (npc.GetNav().enabled)
        {
            // 현재 목표 지점과 NPC의 거리 확인
            float distance = Vector3.Distance(npc.transform.position, npc.GetNav().destination);
            Debug.Log($"[OnFixedUpdate] 현재 위치: {npc.transform.position}, 목표 위치: {npc.GetNav().destination}, 거리: {distance}");

            // 목표 지점 도달 시 체크포인트로 이동
            if (distance <= npc.GetNav().stoppingDistance)
            {
                if (npc.CurrentCheckPointIndex < npc.checkPoints.Length)
                {
                    MoveToNextCheckPoint();
                    npc.CurrentCheckPointIndex++;
                    Debug.Log($"[OnFixedUpdate] 이동 시작: {npc.checkPoints[npc.CurrentCheckPointIndex].position}");
                }
                else
                {
                    Debug.Log("이동 종료!!!");
                    // 종료 후 처리할 부분이 있다면 추가하세요.
                }
            }
        }
    }

    private void MoveToNextCheckPoint()
    {
        if (npc.CurrentCheckPointIndex < npc.checkPoints.Length)
        {
            npc.GetNav().SetDestination(npc.checkPoints[npc.CurrentCheckPointIndex].position);
            Debug.Log($"목표 지점 설정: {npc.checkPoints[npc.CurrentCheckPointIndex].position}");
        }
    }





    public override void OnExit()
    {
        base.OnExit();
        Debug.Log("종료되어 버렸습니다.");

        // npc.GetNav().isStopped = true;
    }


}
