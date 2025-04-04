using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BulBug_StandUpState : BulbBugState
{
    public BulBug_StandUpState(BulbBug bulbBug, BulbBugStateMachine machine) : base(bulbBug, machine) { }

    private float duration = 3f;
    private Tweener rotateTweener;

    public override void OnEnter()
    {
        base.OnEnter();

        bulbBug.rigid.isKinematic = true;

        rotateTweener = bulbBug.transform.DORotate(Vector3.zero, duration, RotateMode.Fast)
           .SetEase(Ease.OutQuad)
           .OnComplete(OnRotationComplete); // 애니메이션이 끝났을 때 호출될 메서드 지정

    }


    public override void OnUpdate()
    {
        base.OnUpdate();

        if (bulbBug.CheckingArea_2.isPlayerInArea)
        {
            // 회전 애니메이션을 중지
            if (rotateTweener != null && rotateTweener.IsPlaying())
            {
                rotateTweener.Kill(); // 애니메이션 중지

                machine.OnStateChange(machine.SleepState); 
            }
        }

    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }


    public override void OnExit()
    {
        base.OnExit();
    }

    void OnRotationComplete()
    {
        machine.OnStateChange(machine.WanderingState);
    }

}
