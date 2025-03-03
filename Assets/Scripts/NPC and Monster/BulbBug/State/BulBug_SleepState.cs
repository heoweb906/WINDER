using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulBug_SleepState : BulbBugState
{
    public BulBug_SleepState(BulbBug bulbBug, BulbBugStateMachine machine) : base(bulbBug, machine) { }

    private float elapsedTime = 0f;  // ��� �ð� ����

    public override void OnEnter()
    {
        base.OnEnter();

        bulbBug.anim.SetTrigger("doHide");

        Debug.Log("��� ���� ���� �Ϸ�");
        bulbBug.ToggleEmission(false);
        bulbBug.carriedObj.enabled = true;
        bulbBug.nav.enabled = false;
        bulbBug.rigid.velocity = Vector3.zero;
        bulbBug.rigid.isKinematic = false;

        bulbBug.gameObject.layer = LayerMask.NameToLayer("Interactable");
    }


    public override void OnUpdate()
    {
        base.OnUpdate();

        if (!bulbBug.CheckingArea_2.isPlayerInArea && bulbBug.carriedObj.canInteract)
        {
            
            elapsedTime += Time.deltaTime;  // �� ������ ��� �ð� �߰�

            if (elapsedTime >= 1f)
            {
                machine.OnStateChange(machine.StandUpState);
            }
        }

        if(bulbBug.CheckingArea_2.isPlayerInArea) elapsedTime = 0f;
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }


    public override void OnExit()
    {
        base.OnExit();

        bulbBug.ToggleEmission(true);
        bulbBug.carriedObj.enabled = false;
        
    }

}
