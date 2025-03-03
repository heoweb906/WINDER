using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class NPC_Simple_Action_TextingSmartPhone : NPC_Simple_State
{
    public NPC_Simple_Action_TextingSmartPhone(NPC_Simple npc, NPC_Simple_StateMachine machine) : base(npc, machine) { }

    public override void OnEnter()
    {
        base.OnEnter();


        Debug.Log("태엽 돌리기 상태 진입 완료");

        npc.GetAnimator().SetInteger("ActionEvent_Num", (int)npc.actionEventList);

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
