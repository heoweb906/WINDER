using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BulBug_WanderingState : BulbBugState
{
    public BulBug_WanderingState(BulbBug bulbBug, BulbBugStateMachine machine) : base(bulbBug, machine) { }

    private float timer = 0f;
    private float stopTime = 0f;  // 멈추는 시간
    private bool isStopped = false;  // 멈추었는지 여부

    public override void OnEnter()
    {
        base.OnEnter();

        bulbBug.anim.SetBool("isWalk", true);

        bulbBug.gameObject.layer = LayerMask.NameToLayer("Default");
        bulbBug.rigid.isKinematic = true;
        bulbBug.nav.enabled = true;

        StartWandering();
    }


    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        // 플레이어가 너무 나까이 오면 잠듦 상태로 변경
        if (bulbBug.CheckingArea_2.isPlayerInArea) machine.OnStateChange(machine.SleepState);


        // 평소의 배회
        if (bulbBug.CheckingArea_1.isPlayerInArea && !isStopped)
        {
            MoveAwayFromPlayer();
        }
        else
        {
            if (isStopped)
            {
                bulbBug.anim.SetBool("isWalk", false);
                stopTime -= Time.fixedDeltaTime;
                if (stopTime <= 0f)
                {
                    isStopped = false;
                    bulbBug.nav.isStopped = false;  // 다시 이동 시작
                    bulbBug.anim.SetBool("isWalk", true);
                }
            }
            else
            {
                timer += Time.fixedDeltaTime;

                if (timer >= Random.Range(3f, 5f) && !bulbBug.CheckingArea_1.isPlayerInArea)  // 3~5초마다
                {
                    timer = 0f;
                    StopMovement();  // 멈추기
                }
                else
                {
                    // 계속 배회하기
                    if (!bulbBug.nav.pathPending && bulbBug.nav.remainingDistance < 0.5f)
                    {
                        SetNewDirection();  // 새로운 방향으로 배회
                    }
                }
            }
        }

    }


    public override void OnExit()
    {
        base.OnExit();

        bulbBug.nav.isStopped = true;
        bulbBug.nav.enabled = false;
    }



    void MoveAwayFromPlayer()
    {
        Vector3 playerPosition = bulbBug.CheckingArea_1.playerPosition.position; // 플레이어 위치
        Vector3 directionAwayFromPlayer = (bulbBug.transform.position - playerPosition).normalized; // 반대 방향
        Vector3 targetPosition = bulbBug.transform.position + directionAwayFromPlayer * bulbBug.patrolRange; // 반대 방향으로 이동할 위치 계산

        // NavMesh 상에서 유효한 위치인지 확인
        if (NavMesh.SamplePosition(targetPosition, out NavMeshHit hit, bulbBug.patrolRange, NavMesh.AllAreas) && bulbBug.nav.enabled)
        {
            bulbBug.nav.SetDestination(hit.position); // 반대 방향으로 이동 설정
        }
        else
        {
            SetNewDirection(); // 반대 방향으로 이동 불가능하면 랜덤으로 이동
        }
    }

    // 배회 방향 설정
    void SetNewDirection()
    {
        Vector3 randomDirection = Random.insideUnitSphere * bulbBug.patrolRange + bulbBug.transform.position;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, bulbBug.patrolRange, NavMesh.AllAreas) && bulbBug.nav.enabled)
        {
            bulbBug.nav.SetDestination(hit.position);  // 새로운 목적지 설정
        }
    }

    // 멈추기
    void StopMovement()
    {
        isStopped = true;
        stopTime = 1f;  // 1초 동안 멈춤
        bulbBug.nav.isStopped = true;  // 이동 멈춤
        bulbBug.anim.SetBool("isWalk", false);  
    }

    // 배회 시작
    void StartWandering()
    {
        SetNewDirection();  // 처음 목표 설정
        bulbBug.nav.isStopped = false;
    }

}
