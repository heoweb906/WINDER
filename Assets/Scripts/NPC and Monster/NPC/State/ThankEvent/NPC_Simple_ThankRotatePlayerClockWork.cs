using DG.Tweening;
using UnityEngine;

public class NPC_Simple_ThankRotatePlayerClockWork : NPC_Simple_State
{
    public NPC_Simple_ThankRotatePlayerClockWork(NPC_Simple npc, NPC_Simple_StateMachine machine) : base(npc, machine) { }

    private bool bFlag = false;

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("플레이어 태엽 돌려주기 시작");


        GameAssistManager.Instance.PlayerInputLockOn();


        npc.GetNav().isStopped = false;
        npc.GetNav().SetDestination(GameAssistManager.Instance.GetPlayer().transform.position);

        // (필요 시 걷는 애니메이션 트리거)
        npc.GetAnimator().SetInteger("Walk_Num", 1);
        npc.GetAnimator().SetBool("Bool_Walk",true);
       
    }



    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        if (!npc.GetNav().pathPending && npc.GetNav().remainingDistance <= 2f && !bFlag)
        {
            // 방향 체크
            Vector3 toPlayer = GameAssistManager.Instance.GetPlayer().transform.position - npc.transform.position;
            toPlayer.y = 0; // y축 회전 무시 (수평 기준)
            Vector3 forward = npc.transform.forward;

            float angle = Vector3.Angle(forward, toPlayer);

            // 플레이어를 충분히 바라보고 있는지 확인 (예: 10도 이하)
            if (angle <= 3f)
            {
                bFlag = true;
                npc.GetNav().isStopped = true;

                npc.GetAnimator().SetTrigger("ddddStop");
                npc.GetAnimator().SetBool("Bool_Walk", false);

                machine.OnStateChange(machine.IDLEState);

                npc.RotatePlayerTaeyub();
            }
            else
            {
                // 도착했지만 방향이 안 맞으면 회전 유도
                Quaternion lookRotation = Quaternion.LookRotation(toPlayer);
                npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation, lookRotation, Time.deltaTime * 5f); // 부드럽게 회전
            }
        }
    }

    public override void OnExit()
    {
        base.OnExit();
      
    }
}
