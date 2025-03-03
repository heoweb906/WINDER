using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Simple_State : BaseState
{
    protected NPC_Simple npc;
    protected NPC_Simple_StateMachine machine;


    public NPC_Simple_State(NPC_Simple _guardM, NPC_Simple_StateMachine _machine)
    {
        npc = _guardM;
        machine = _machine;
    }

    public virtual void OnEnter()
    {
        
    }

    public virtual void OnExit()
    {

    }

    public virtual void OnFixedUpdate()
    {

    }

    public virtual void OnUpdate()
    {

    }





    public virtual void OnAnimationEnterEvent() { }
    public virtual void OnAnimationExitEvent() { }
    public virtual void OnAnimationTransitionEvent() { }
    public virtual void OnTriggerEnter(Collider other) { }
    public virtual void OnTriggerExit(Collider other) { }
    public virtual void OnTriggerStay(Collider other) { }
}
