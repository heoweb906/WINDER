using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class NPC_Simple_ReactionThankState : NPC_Simple_State
{
    public NPC_Simple_ReactionThankState(NPC_Simple npc, NPC_Simple_StateMachine machine) : base(npc, machine) { }


    private float elapsedTime; // 시간 측정용 변수
    private const float duration = 2f; // 2초 후에 상태 변경


    public override void OnEnter()
    {
        base.OnEnter();

        npc.bSad = false;
        npc.GetAnimator().SetBool("Bool_Sad", false);
        npc.GetAnimator().SetTrigger("doClockWorkStart");



        if (npc.clockworkEvent == ClockWorkEventList.None)
        {
            npc.GetAnimator().SetTrigger("doReactionHappy");

            elapsedTime = 0f;

        }
        else
        {


            ChangeStateNPC();
        }

       

    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        Debug.Log("감사하는 상태가 동작중입니다. ");

        elapsedTime += Time.deltaTime;

        // 2초가 지나면 ChangeStateNPC 실행
        if (elapsedTime >= duration)
        {
            npc.CurrentCheckPointIndex--;
            machine.OnStateChange(npc.bWalking ? machine.WalkState : machine.IDLEState);
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

    private void ChangeStateNPC()
    {
        if (npc.clockworkEvent == ClockWorkEventList.RotatePlayerClockwork)
        {
            machine.OnStateChange(machine.ThankState_RotatePlayerClockWork);

        }
        //else
        //{
        //    machine.OnStateChange(machine.ThankState);
        //}
    }

}
