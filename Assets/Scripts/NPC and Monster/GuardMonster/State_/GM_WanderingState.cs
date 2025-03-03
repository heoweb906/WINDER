using System.Collections;
using UnityEngine;

public class GM_WanderingState : GuardMState
{
    public GM_WanderingState(GuardM guardM, GuardMStateMachine machine) : base(guardM, machine) { }

    // 이동
    private Transform targetPosition;  
    private float fWaitingTime = 1.8f; 
    private bool bIsWaiting = false;   
   
    // 랜덤 두리번거림                        
    private bool bRandomStopChecked = false;  
    private float fRandomStopPercent = 0.8f;

    public override void OnEnter()
    {
        base.OnEnter();

        guardM.anim.SetBool("isWalking", true);
        guardM.anim.SetBool("isRunning", false);
        targetPosition = guardM.transformEnd; // 처음에는 End 지점으로 이동 시작
        guardM.nav.isStopped = false;
        guardM.nav.SetDestination(targetPosition.position);

        bRandomStopChecked = false; // 새로운 이동 시작 시 다시 랜덤 체크 가능하도록 설정
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
            guardM.StartGuardCoroutine(WaitAndMove(fWaitingTime)); // 도착 후 1.2초 멈춤
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    private IEnumerator WaitAndMove(float waitTime)
    {
        bIsWaiting = true;
        guardM.nav.isStopped = true; // 멈추기
        guardM.anim.SetBool("isWalking", false); // 걷는 애니메이션 중지

        yield return new WaitForSeconds(waitTime); // 지정된 시간 동안 대기
        if (Random.value < fRandomStopPercent) bRandomStopChecked = true;

        targetPosition = (targetPosition == guardM.transformStart) ? guardM.transformEnd : guardM.transformStart;

        guardM.nav.SetDestination(targetPosition.position);
        guardM.nav.isStopped = false; // 다시 이동 시작
        guardM.anim.SetBool("isWalking", true); // 걷기 애니메이션 재개

        bIsWaiting = false;

        if(bRandomStopChecked)
        {
            yield return new WaitForSeconds(Random.Range(1f, guardM.fMoveTime));

            guardM.anim.SetTrigger("doLookAround");
            bIsWaiting = true;
            guardM.nav.isStopped = true; // 멈추기
            guardM.anim.SetBool("isWalking", false);

            yield return new WaitForSeconds(4.7f);

            guardM.anim.SetTrigger("doLookAroundEnd"); ;
            guardM.anim.SetBool("isWalking", true); // 걷기 애니메이션 재개
            bIsWaiting = false;

            yield return new WaitForSeconds(0.35f);

            guardM.nav.isStopped = false; // 다시 이동 시작
        }
    }

}
