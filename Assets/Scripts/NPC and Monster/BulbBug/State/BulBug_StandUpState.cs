using DG.Tweening;
using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BulBug_StandUpState : BulbBugState
{
    public BulBug_StandUpState(BulbBug bulbBug, BulbBugStateMachine machine) : base(bulbBug, machine) { }

    private float duration = 3f;
    private Tweener rotateTweener;

    private float startTime;
    private bool isWaiting = false;

    public override void OnEnter()
    {
        base.OnEnter();

        Debug.Log("StanUp ���� ����");

        Vector3 rotation = bulbBug.gameObject.transform.rotation.eulerAngles;

        if (Mathf.Abs(Mathf.DeltaAngle(rotation.x, 0)) <= 4f &&
       
        Mathf.Abs(Mathf.DeltaAngle(rotation.z, 0)) <= 4f)
        {
            bulbBug.anim.SetTrigger("doStandUp");

            startTime = Time.time;
            isWaiting = true;
        }
        else
        {
            bulbBug.anim.SetTrigger("doWiggle");

            bulbBug.rigid.isKinematic = true;

            rotateTweener = bulbBug.transform.DORotate(Vector3.zero, duration, RotateMode.Fast)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    rotateTweener = null; // Ʈ���� �Ϸ� �� null ó��
                    bulbBug.anim.SetTrigger("doStanUpFinish");
                    machine.OnStateChange(machine.WanderingState);
                });

        }
    }


    public override void OnUpdate()
    {
        base.OnUpdate();

        if (isWaiting && Time.time - startTime >= 2f)
        {
            isWaiting = false;
            bulbBug.anim.SetTrigger("doStanUpFinish");
            machine.OnStateChange(machine.WanderingState);
        }

        if (bulbBug.CheckingArea_2.isPlayerInArea)
        {
            // ȸ�� �ִϸ��̼��� ����
            if (rotateTweener != null && rotateTweener.IsPlaying())
            {
                rotateTweener.Kill(); // �ִϸ��̼� ����
                rotateTweener = null;  // Tweener �ʱ�ȭ

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

}
