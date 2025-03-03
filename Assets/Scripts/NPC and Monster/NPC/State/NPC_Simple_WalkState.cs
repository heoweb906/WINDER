using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Simple_WalkState : NPC_Simple_State
{
    public NPC_Simple_WalkState(NPC_Simple npc, NPC_Simple_StateMachine machine) : base(npc, machine) { }


    public override void OnEnter()
    {
        base.OnEnter();

        npc.GetNav().enabled = true;
        npc.GetNav().autoBraking = false;

        int ranNum = (Random.value < 0.15f) ? 0 : 1;

        // int ranNum = Random.Range(0, 2);

        // 0. 핸드폰 보면서 걷기
        // 1. 일반 걷기
        if (ranNum == 0) npc.GetNav().speed = 0.7f;

        npc.GetAnimator().SetInteger("Walk_Num", ranNum);

       
    }




    public override void OnUpdate()
    {
        base.OnUpdate();


        if (npc.checkPoints == null) return;

        if (npc.GetNav().remainingDistance <= npc.GetNav().stoppingDistance && npc.bWalking)
        {
            if (npc.CurrentCheckPointIndex < npc.checkPoints.Length)
            {
                MoveToNextCheckPoint();
            }
            else
            {
                Debug.Log("NPC 삭제");
                Object.Destroy(npc.gameObject);
            }
            npc.CurrentCheckPointIndex++;
        }

    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }


    public override void OnExit()
    {
        base.OnExit();

        npc.GetNav().enabled = false;
    }


    private void MoveToNextCheckPoint()
    {
        npc.GetNav().SetDestination(npc.checkPoints[npc.CurrentCheckPointIndex].position);
        Debug.Log("걷는 중입니다!!!!!.");
    }
}
