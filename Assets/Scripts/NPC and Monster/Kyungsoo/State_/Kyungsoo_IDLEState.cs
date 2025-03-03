using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kyungsoo_IDLEState : Kyungsoo_State
{
    public Kyungsoo_IDLEState(Kyungsoo kyungsoo, Kyungsoo_StateMachine machine) : base(kyungsoo, machine) { }


    public override void OnEnter()
    {
        base.OnEnter();

        Debug.Log("IDLE 스테이트 진입");

        kyungsoo.GetAnimator().SetBool("Bool_Walk", false);
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
