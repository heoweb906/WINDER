using System.Collections;
using UnityEngine;

public class GM_WanderingState : GuardMState
{
    public GM_WanderingState(GuardM guardM, GuardMStateMachine machine) : base(guardM, machine) { }

    // �̵�
    private Transform targetPosition;  
    private float fWaitingTime = 1.8f; 
    private bool bIsWaiting = false;   
   
    // ���� �θ����Ÿ�                        
    private bool bRandomStopChecked = false;  
    private float fRandomStopPercent = 0.8f;

    public override void OnEnter()
    {
        base.OnEnter();

        guardM.anim.SetBool("isWalking", true);
        guardM.anim.SetBool("isRunning", false);
        targetPosition = guardM.transformEnd; // ó������ End �������� �̵� ����
        guardM.nav.isStopped = false;
        guardM.nav.SetDestination(targetPosition.position);

        bRandomStopChecked = false; // ���ο� �̵� ���� �� �ٽ� ���� üũ �����ϵ��� ����
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (guardM.area.isPlayerInArea && guardM.area.playerPosition != null && guardM.visualRange.isPlayerInArea)
        {
            guardM.SetHomeTransform(guardM.transform.position);
            guardM.SetObjRotation(guardM.transform.rotation);
            machine.OnStateChange(machine.ChaseState);
        }

        if (bIsWaiting) return;
        if (/*!guardM.nav.pathPending && */guardM.nav.remainingDistance <= 0.2f)
        {
            guardM.StartGuardCoroutine(WaitAndMove(fWaitingTime)); // ���� �� 1.2�� ����
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    private IEnumerator WaitAndMove(float waitTime)
    {
        bIsWaiting = true;
        guardM.nav.isStopped = true; // ���߱�
        guardM.anim.SetBool("isWalking", false); // �ȴ� �ִϸ��̼� ����

        yield return new WaitForSeconds(waitTime); // ������ �ð� ���� ���
        if (Random.value < fRandomStopPercent) bRandomStopChecked = true;

        targetPosition = (targetPosition == guardM.transformStart) ? guardM.transformEnd : guardM.transformStart;

        guardM.nav.SetDestination(targetPosition.position);
        guardM.nav.isStopped = false; // �ٽ� �̵� ����
        guardM.anim.SetBool("isWalking", true); // �ȱ� �ִϸ��̼� �簳

        bIsWaiting = false;

        if(bRandomStopChecked)
        {
            yield return new WaitForSeconds(Random.Range(1f, guardM.fMoveTime));

            guardM.anim.SetTrigger("doLookAround");
            bIsWaiting = true;
            guardM.nav.isStopped = true; // ���߱�
            guardM.anim.SetBool("isWalking", false);

            yield return new WaitForSeconds(4.7f);

            guardM.anim.SetTrigger("doLookAroundEnd"); ;
            guardM.anim.SetBool("isWalking", true); // �ȱ� �ִϸ��̼� �簳
            bIsWaiting = false;

            yield return new WaitForSeconds(0.35f);

            guardM.nav.isStopped = false; // �ٽ� �̵� ����
        }
    }

}
