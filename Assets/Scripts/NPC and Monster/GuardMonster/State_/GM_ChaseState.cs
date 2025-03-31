using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Drawing;

public class GM_ChaseState : GuardMState
{
    public GM_ChaseState(GuardM guardM, GuardMStateMachine machine) : base(guardM, machine) { }

    bool bAnimEnd;


    public override void OnEnter()
    {
        base.OnEnter();

        Debug.Log("플레이어 추격 시작");

        if (guardM.creatorNPC != null)
        {
            Debug.Log("공포에 떨어라");
            guardM.creatorNPC.NPCCreatOff_Sacred();
        }
           

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
                guardM.nav.SetDestination(GameAssistManager.Instance.GetPlayer().transform.position /*guardM.area.playerPosition.position*/);

                float distanceToTarget = Vector3.Distance(guardM.transform.position, guardM.area.playerPosition.position);
                if (distanceToTarget <= guardM.fAttackRange)
                {
                    Debug.Log(GameAssistManager.Instance.GetPlayerScript().machine.CurrentState);

                    if(GameAssistManager.Instance.GetPlayer().transform.parent == null)
                    {
                        bAnimEnd = false;
                        guardM.nav.isStopped = true;
                        machine.OnStateChange(machine.AttackState);
                    }
                    else
                    {
                        bAnimEnd = false;
                        guardM.nav.isStopped = true;
                        machine.OnStateChange(machine.BackHomeState);
                    }
                     
                  
                }

                //if (!guardM.IsObstacleBetween())
                //{
                    
                //}
                //else
                //{
                //    bAnimEnd = false;
                //    guardM.nav.isStopped = true;
                //    machine.OnStateChange(machine.BackHomeState);
                //}


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
