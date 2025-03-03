using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulbBugState : BaseState
{
    protected BulbBug bulbBug;
    protected BulbBugStateMachine machine;


    public BulbBugState(BulbBug _bulbBug, BulbBugStateMachine _machine)
    {
        bulbBug = _bulbBug;
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
