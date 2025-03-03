using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class GuardMState : BaseState
{
    protected GuardM guardM;
    protected GuardMStateMachine machine;


    public GuardMState(GuardM _guardM, GuardMStateMachine _machine)
    {
        guardM = _guardM;
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
