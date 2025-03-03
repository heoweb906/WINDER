using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM_AttackState : GuardMState
{
    public GM_AttackState(GuardM guardM, GuardMStateMachine machine) : base(guardM, machine) { }

    public bool bAnimEnd;

    public override void OnEnter()
    {
        base.OnEnter();

        guardM.anim.SetTrigger("doAttack");
        GameAssistManager.Instance.DiePlayerReset(3f, 1, 0.6f);



        GameObject player = GameAssistManager.Instance.GetPlayer();
        Rigidbody playerRigidbody = player.GetComponent<Rigidbody>();
        playerRigidbody.isKinematic = true;
        player.transform.SetParent(guardM.transformGrabPlayer);

        // 0.85초 뒤에 이동 (0.4초 동안)
        player.transform.DOLocalMove(Vector3.zero, 0.4f)
            .SetEase(Ease.OutQuint)
            .SetDelay(0.85f);

        // 0.85초 뒤에 회전 (0.4초 동안)
        Vector3 targetRotation = new Vector3(1.431f, 180f, 175.697f); // 예제 값
        player.transform.DOLocalRotate(targetRotation, 0.4f, RotateMode.FastBeyond360)
            .SetEase(Ease.OutQuint)
            .SetDelay(0.9f);


        // 위치 & 회전 고정
        playerRigidbody.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;



        guardM.StartGuardCoroutine(AssistAnim(2f));
    }


    public override void OnUpdate()
    {
        base.OnUpdate();

        //if (bAnimEnd)
        //{
        //    bAnimEnd = false;
        //    machine.OnStateChange(machine.BackHomeState);
        //}
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }


    public override void OnExit()
    {
        base.OnExit();
    }



    IEnumerator AssistAnim(float fWaitSecond)
    {
        yield return new WaitForSeconds(fWaitSecond);

        bAnimEnd = true;
    }

}
