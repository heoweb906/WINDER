using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Simple_GrappedState : NPC_Simple_State
{
    public NPC_Simple_GrappedState(NPC_Simple npc, NPC_Simple_StateMachine machine) : base(npc, machine) { }

    private float timer = 0f;
    private bool hasExecuted = false; // 한 번만 실행되도록 체크


    public override void OnEnter()
    {
        base.OnEnter();


        timer = 0f; // 타이머 초기화
        hasExecuted = false; // 실행 여부 초기화
    }


    public override void OnUpdate()
    {
        base.OnUpdate();



        timer += Time.deltaTime; // 시간 증가

        if (timer >= 1f && !hasExecuted) // 1초가 지나고 한 번만 실행
        {
            machine.OnStateChange(machine.ThankState_RotatePlayerClockWork);
           
            hasExecuted = true;
        }

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
