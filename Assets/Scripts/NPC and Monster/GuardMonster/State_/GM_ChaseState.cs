using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class GM_ChaseState : GuardMState
{
    public GM_ChaseState(GuardM guardM, GuardMStateMachine machine) : base(guardM, machine) { }

    bool bAnimEnd;


    public override void OnEnter()
    {
        base.OnEnter();

        guardM.trackingHead.bFindPlayer = true;
        guardM.nav.isStopped = true;
        guardM.anim.SetTrigger("doFindPlayer");

        guardM.StartGuardCoroutine(AssistAnim(1.1f));
    }

    
    public override void OnUpdate()
    {
        base.OnUpdate();

        if (bAnimEnd)
        {

            guardM.nav.isStopped = false;
            if (guardM.area.playerPosition != null && guardM.area.isPlayerInArea)
            {
                if (!guardM.IsObstacleBetween())
                {
                    guardM.nav.SetDestination(GameAssistManager.Instance.GetPlayer().transform.position /*guardM.area.playerPosition.position*/); 

                    float distanceToTarget = Vector3.Distance(guardM.transform.position, guardM.area.playerPosition.position);
                    if (distanceToTarget <= guardM.fAttackRange)
                    {
                        bAnimEnd = false;
                        guardM.nav.isStopped = true;
                        machine.OnStateChange(machine.AttackState);
                    }
                }
                else
                {
                    bAnimEnd = false;
                    guardM.nav.isStopped = true;
                    machine.OnStateChange(machine.BackHomeState);
                }


            }
            else
            {
                bAnimEnd = false;
                guardM.nav.isStopped = true;
                machine.OnStateChange(machine.BackHomeState);
            }
            
        }
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }


    public override void OnExit()
    {
        base.OnExit();

        guardM.trackingHead.bFindPlayer = false;
        guardM.anim.SetBool("isWalking", false);
        guardM.anim.SetBool("isRunning", false);
    }



    IEnumerator AssistAnim(float fWaitSecond)
    {
        yield return new WaitForSeconds(fWaitSecond);

        guardM.anim.SetTrigger("doFindPlayer_End");
        guardM.anim.SetBool("isWalking", false);
        guardM.anim.SetBool("isRunning", true);
        guardM.nav.isStopped = false;

        bAnimEnd = true;
    }
   


}
