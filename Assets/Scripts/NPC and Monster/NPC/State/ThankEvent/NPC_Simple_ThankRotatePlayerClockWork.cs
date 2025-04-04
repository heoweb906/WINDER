using DG.Tweening;
using UnityEngine;

public class NPC_Simple_ThankRotatePlayerClockWork : NPC_Simple_State
{
    public NPC_Simple_ThankRotatePlayerClockWork(NPC_Simple npc, NPC_Simple_StateMachine machine) : base(npc, machine) { }

    private bool bStart = false;
    public bool bHasFinishedRotation = false;
    private Quaternion targetRotation;

    public override void OnEnter()
    {
        base.OnEnter();

        GameAssistManager.Instance.PlayerInputLockOn();

        npc.GetAnimator().SetBool("Bool_Walk", true);
        npc.GetAnimator().SetInteger("Walk_Num", npc.iAnimWalking);

        npc.GetNav().isStopped = false;
        npc.GetNav().autoBraking = false;

        npc.checkPoints = new Transform[3];
        GameObject objPlayer = npc.GetPlayerObject();

        // 1. 첫 번째 목표 위치 계산
        Vector3 position1 = objPlayer.transform.position + objPlayer.transform.right + objPlayer.transform.forward * 1f;
        npc.checkPoints[0] = new GameObject().transform; // 임시 Transform 생성
        npc.checkPoints[0].position = position1; // 위치만 설정하고 GameObject는 사용 안 함

        // 2. 두 번째 목표 위치 계산
        Vector3 position2 = objPlayer.transform.position + objPlayer.transform.right + objPlayer.transform.forward * -1f;
        npc.checkPoints[1] = new GameObject().transform; // 임시 Transform 생성
        npc.checkPoints[1].position = position2; // 위치만 설정하고 GameObject는 사용 안 함

        Vector3 position3 = objPlayer.transform.position + objPlayer.transform.forward * -1f;
        npc.checkPoints[2] = new GameObject().transform; // 임시 Transform 생성
        npc.checkPoints[2].position = position3;

        // 배열 내 위치 출력
        for (int i = 0; i < npc.checkPoints.Length; i++)
        {
            Debug.Log($"Position {i + 1}: {npc.checkPoints[i].position}");
        }

        npc.CurrentCheckPointIndex = 0; // 시작할 때 첫 번째 체크포인트부터

        MoveToNextCheckPoint(); // 첫 번째 체크포인트로 이동

        bStart = true;
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        if (bStart)
        {
            if (npc.GetNav().enabled)
            {
                float distance = Vector3.Distance(npc.transform.position, npc.GetNav().destination);

                // 목표 지점 도달 시 다음 체크포인트로 이동
                if (distance <= npc.GetNav().stoppingDistance)
                {
                    npc.CurrentCheckPointIndex++;

                    if (npc.CurrentCheckPointIndex < npc.checkPoints.Length)
                    {
                        MoveToNextCheckPoint();
                    }
                    else
                    {
                        bStart = false;

                        npc.GetAnimator().SetTrigger("ddddStop");
                        npc.GetAnimator().SetBool("Bool_Walk", false);
                        LookAtPlayerAndExecuteFunction();
                    }
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

    private void LookAtPlayerAndExecuteFunction()
    {
        GameObject objPlayer = npc.GetPlayerObject();

        // NPC가 플레이어를 바라보도록 회전
        Vector3 directionToPlayer = objPlayer.transform.position - npc.transform.position;
        directionToPlayer.y = 0; // Y축 회전 제외 (수평 회전만)
        targetRotation = Quaternion.LookRotation(directionToPlayer);

        // DOTween을 사용하여 부드럽게 회전하고, 딜레이 후 함수 실행
        float rotationSpeed = 1f;
        npc.transform.DORotateQuaternion(targetRotation, rotationSpeed)
            .SetDelay(1.5f) // 회전 후 1초 딜레이
            .OnComplete(() =>
            {
               
                npc.GetAnimator().SetTrigger("doRoateTaeyubStart");

                // 2초 후 다른 함수 실행
                DOVirtual.DelayedCall(3f, () =>
                {
                    // 2초 후 실행할 함수 호출
                    GameAssistManager.Instance.PlayerInputLockOff();
                    npc.GetAnimator().SetTrigger("doRoateTaeyubEnd");
                    machine.OnStateChange(machine.IDLEState);
                });
            });
    }


    public override void OnExit()
    {
        base.OnExit();
        Debug.Log("종료되어 버렸습니다.");
    }
}
