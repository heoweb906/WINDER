using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Simple_WalkState : NPC_Simple_State
{
    public NPC_Simple_WalkState(NPC_Simple npc, NPC_Simple_StateMachine machine) : base(npc, machine) { }

    public override void OnEnter()
    {
        base.OnEnter();

        // Debug.Log("WalkState 진입");

        // npc.GetNav().radius = 0.26f;
        npc.GetNav().isStopped = false;
        npc.GetNav().autoBraking = false;

        npc.GetAnimator().SetInteger("Walk_Num", npc.iAnimWalking);


    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        // 이동 로직은 FixedUpdate에서 처리
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        if (npc.checkPoints == null) return;

        if (npc.GetNav().enabled)
        {
            // 목표 지점 도달 시 다음 체크포인트로 이동하거나, 모두 도달하면 풀로 반환
            if (Vector3.Distance(npc.transform.position, npc.GetNav().destination) <= npc.GetNav().stoppingDistance && npc.bWalking)
            {
                if (npc.CurrentCheckPointIndex < npc.checkPoints.Length)
                {
                    MoveToNextCheckPoint();
                    npc.CurrentCheckPointIndex++;
                }
                else
                {
                    // 부모에 있는 Create_WanderingNPC 스크립트를 찾아 풀로 반환
                    Create_WanderingNPC spawner = npc.transform.parent.GetComponent<Create_WanderingNPC>();
                    if (spawner != null)
                        spawner.ReturnNPCToPool(npc.gameObject);
                    else
                        Object.Destroy(npc.gameObject); // 예외 처리
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
