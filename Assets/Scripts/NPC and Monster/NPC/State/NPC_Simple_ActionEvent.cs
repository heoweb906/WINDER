using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPC_Simple_ActionEvent : NPC_Simple_State
{
    public NPC_Simple_ActionEvent(NPC_Simple npc, NPC_Simple_StateMachine machine) : base(npc, machine) { }

    public override void OnEnter()
    {
        base.OnEnter();

        // npc에 actionEventList이 enum으로 정리되어 있음, 그에 맞는 액션의 순서에 맞는 애니메이션이 작동됨

        switch ((int)npc.actionEventList)
        {
            case 0:
                machine.OnStateChange(machine.IDLEState);
                break;

            case 1:
                machine.OnStateChange(machine.SpinTaeYubState);
                break;

            case 2: 
                machine.OnStateChange(machine.TextingSmartPhoneState);
                break;

            default:
                break; 

        }
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
