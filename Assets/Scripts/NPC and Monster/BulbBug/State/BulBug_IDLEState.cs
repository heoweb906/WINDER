using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulBug_IDLEState : BulbBugState
{
    public BulBug_IDLEState(BulbBug bulbBug, BulbBugStateMachine machine) : base(bulbBug, machine) { }

    public override void OnEnter()
    {
        base.OnEnter(); 
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
