using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM_BackHomeState : GuardMState
{
    public GM_BackHomeState(GuardM guardM, GuardMStateMachine machine) : base(guardM, machine) { }

    bool bAnimEnd;

    public override void OnEnter()
    {
        base.OnEnter();

        guardM.StartGuardCoroutine(AssistAnim(2f));
    }


    public override void OnUpdate()
    {
        base.OnUpdate();

        if (!bAnimEnd) return;

        // 플레이어 위치가 없거나 Obstacle이 있는 경우 집으로 돌아감
        if (guardM.area.playerPosition == null || guardM.IsObstacleBetween())
        {
            ReturnHome();
        }
        else
        {
            // 장애물이 없고 플레이어 위치가 있는 경우 추격 상태로 전환
            guardM.nav.isStopped = true;
            bAnimEnd = false;
            machine.OnStateChange(machine.ChaseState);
        }
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }


    public override void OnExit()
    {
        base.OnExit();
        guardM.anim.SetBool("isWalking", false);
        guardM.anim.SetBool("isRunning", false);

    }

    IEnumerator AssistAnim(float fWaitSecond)
    {
        yield return new WaitForSeconds(fWaitSecond);

        guardM.anim.SetBool("isWalking", true);
        guardM.anim.SetBool("isRunning", false);
        guardM.nav.isStopped = false;

        bAnimEnd = true;

        guardM.nav.SetDestination(guardM.GetHomeTransform());
    }



    private void ReturnHome()
    {
        guardM.nav.SetDestination(guardM.GetHomeTransform());

        // 집에 도착한 경우
        if (Vector3.Distance(guardM.transform.position, guardM.GetHomeTransform()) <= 0.1f)
        {
            guardM.nav.isStopped = true;
            guardM.anim.SetBool("isWalking", false);
            guardM.anim.SetBool("isRunning", false);

            // 상태 전환
            if (guardM.guardMType == GuardMType.Wandering)
            {
                Debug.Log("WanderingState로 상태 전환");
                guardM.StopGuardCoroutine();
                machine.OnStateChange(machine.WanderingState);
            }
            else
            {
                Debug.Log("ReadyState로 상태 전환");
                machine.OnStateChange(machine.ReadyState);
            }
        }
    }


}
