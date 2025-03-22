using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Simple_ThankRotatePlayerClockWork : NPC_Simple_State
{
    public NPC_Simple_ThankRotatePlayerClockWork(NPC_Simple npc, NPC_Simple_StateMachine machine) : base(npc, machine) { }

    public override void OnEnter()
    {
        base.OnEnter();

        Debug.Log("진입 완료");


    }

    public override void OnUpdate()
    {
        base.OnUpdate();


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
