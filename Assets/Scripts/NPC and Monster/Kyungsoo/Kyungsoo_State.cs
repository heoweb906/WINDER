using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kyungsoo_State : BaseState
{
    protected Kyungsoo kyungsoo;
    protected Kyungsoo_StateMachine machine;


    public Kyungsoo_State(Kyungsoo _kyungsoo, Kyungsoo_StateMachine _machine)
    {
        kyungsoo = _kyungsoo;
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
