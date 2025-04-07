using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Simple_TakeSubway : NPC_Simple_State
{
    public NPC_Simple_TakeSubway(NPC_Simple npc, NPC_Simple_StateMachine machine) : base(npc, machine) { }

    public override void OnEnter()
    {
        base.OnEnter();

        Debug.Log("상태 진입 완료");


        npc.GetNav().isStopped = false;
        npc.GetNav().autoBraking = false;

        npc.bWalking = true;
        npc.GetAnimator().SetBool("Bool_Walk", true);
        npc.GetAnimator().SetInteger("Walk_Num", npc.iAnimWalking);

        if (npc.checkPoints != null && npc.checkPoints.Length > 0)
        {
            npc.CurrentCheckPointIndex = 0;
            MoveToNextCheckPoint();
        }
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        if (npc.checkPoints == null) return;

        if (npc.GetNav().enabled)
        {
            if (Vector3.Distance(npc.transform.position, npc.GetNav().destination) <= npc.GetNav().stoppingDistance)
            {
                if (npc.CurrentCheckPointIndex < npc.checkPoints.Length)
                {
                    MoveToNextCheckPoint();
                    npc.CurrentCheckPointIndex++;
                }
                else
                {
                    Debug.Log("이동 끝!");

                    npc.GetAnimator().SetTrigger("ddddStop");
                    npc.GetAnimator().SetBool("Bool_Walk", false);
                    npc.GetNav().isStopped = true;

                    machine.OnStateChange(machine.IDLEState);
                }
            }
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        npc.GetNav().isStopped = true;
    }

    private void MoveToNextCheckPoint()
    {
        npc.GetNav().SetDestination(npc.checkPoints[npc.CurrentCheckPointIndex].position);
    }
}
