using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BulBug_WanderingState : BulbBugState
{
    public BulBug_WanderingState(BulbBug bulbBug, BulbBugStateMachine machine) : base(bulbBug, machine) { }

    private float timer = 0f;
    private float stopTime = 0f;  // ���ߴ� �ð�
    private bool isStopped = false;  // ���߾����� ����

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

        // �÷��̾ �ʹ� ������ ���� ��� ���·� ����
        if (bulbBug.CheckingArea_2.isPlayerInArea) machine.OnStateChange(machine.SleepState);


        // ����� ��ȸ
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
                    bulbBug.nav.isStopped = false;  // �ٽ� �̵� ����
                    bulbBug.anim.SetBool("isWalk", true);
                }
            }
            else
            {
                timer += Time.fixedDeltaTime;

                if (timer >= Random.Range(3f, 5f) && !bulbBug.CheckingArea_1.isPlayerInArea)  // 3~5�ʸ���
                {
                    timer = 0f;
                    StopMovement();  // ���߱�
                }
                else
                {
                    // ��� ��ȸ�ϱ�
                    if (!bulbBug.nav.pathPending && bulbBug.nav.remainingDistance < 0.5f)
                    {
                        SetNewDirection();  // ���ο� �������� ��ȸ
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
        Vector3 playerPosition = bulbBug.CheckingArea_1.playerPosition.position; // �÷��̾� ��ġ
        Vector3 directionAwayFromPlayer = (bulbBug.transform.position - playerPosition).normalized; // �ݴ� ����
        Vector3 targetPosition = bulbBug.transform.position + directionAwayFromPlayer * bulbBug.patrolRange; // �ݴ� �������� �̵��� ��ġ ���

        // NavMesh �󿡼� ��ȿ�� ��ġ���� Ȯ��
        if (NavMesh.SamplePosition(targetPosition, out NavMeshHit hit, bulbBug.patrolRange, NavMesh.AllAreas) && bulbBug.nav.enabled)
        {
            bulbBug.nav.SetDestination(hit.position); // �ݴ� �������� �̵� ����
        }
        else
        {
            SetNewDirection(); // �ݴ� �������� �̵� �Ұ����ϸ� �������� �̵�
        }
    }

    // ��ȸ ���� ����
    void SetNewDirection()
    {
        Vector3 randomDirection = Random.insideUnitSphere * bulbBug.patrolRange + bulbBug.transform.position;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, bulbBug.patrolRange, NavMesh.AllAreas) && bulbBug.nav.enabled)
        {
            bulbBug.nav.SetDestination(hit.position);  // ���ο� ������ ����
        }
    }

    // ���߱�
    void StopMovement()
    {
        isStopped = true;
        stopTime = 1f;  // 1�� ���� ����
        bulbBug.nav.isStopped = true;  // �̵� ����
        bulbBug.anim.SetBool("isWalk", false);  
    }

    // ��ȸ ����
    void StartWandering()
    {
        SetNewDirection();  // ó�� ��ǥ ����
        bulbBug.nav.isStopped = false;
    }

}
