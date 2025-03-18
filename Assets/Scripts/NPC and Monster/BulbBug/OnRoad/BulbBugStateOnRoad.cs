using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulbBugStateOnRoad : BaseState
{
    protected BulbBug_OnRoad bulbBug;
    protected BBBmachine machine;


    public BulbBugStateOnRoad(BulbBug_OnRoad _bulbBug, BBBmachine _machine)
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
