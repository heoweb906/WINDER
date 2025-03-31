using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Simple_Scared : NPC_Simple_State
{
    public NPC_Simple_Scared(NPC_Simple npc, NPC_Simple_StateMachine machine) : base(npc, machine) { }

    public override void OnEnter()
    {
        base.OnEnter();


        npc.GetAnimator().SetTrigger("doScareStart");
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

        npc.GetAnimator().SetTrigger("doScareEnd");
    }
}
