using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Simple_GrappedState : NPC_Simple_State
{
    public NPC_Simple_GrappedState(NPC_Simple npc, NPC_Simple_StateMachine machine) : base(npc, machine) { }


    private float elapsedTime; // 시간 측정용 변수
    private const float duration = 1.5f; // 2초 후에 상태 변경


    public override void OnEnter()
    {
        base.OnEnter();


        npc.GetNav().radius = 0.5f;
        npc.GetAnimator().SetTrigger("doReactionGrapped");

        Debug.Log("NPC가 잡힌 상태가 됨");

        // elapsedTime = 0f; // 상태 진입 시 시간 초기화
    }


    public override void OnUpdate()
    {
        base.OnUpdate();

        //elapsedTime += Time.deltaTime;

        //// 2초가 지나면 ChangeStateNPC 실행
        //if (elapsedTime >= duration)
        //{
        //    ChangeStateNPC();
        //}
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }


    public override void OnExit()
    {
        base.OnExit();
    }


  

    
}
