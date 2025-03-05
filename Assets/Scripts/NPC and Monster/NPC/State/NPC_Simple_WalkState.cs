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

        if (ranNum == 0) npc.GetNav().speed = 0.7f;

        npc.GetAnimator().SetInteger("Walk_Num", ranNum);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        // 위치 이동은 FixedUpdate에서 하므로 여기서는 필요하지 않음.
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        if (npc.checkPoints == null) return;

        // NavMeshAgent가 이동할 목표 위치까지 남은 거리를 수동으로 체크
        if (npc.GetNav().enabled)
        {
            // 목표 지점에 도달하면 다음 체크포인트로 이동
            if (Vector3.Distance(npc.transform.position, npc.GetNav().destination) <= npc.GetNav().stoppingDistance && npc.bWalking)
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