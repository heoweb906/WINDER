using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


// #. 태엽을 돌려주고 난 이후에 취할 액션
public enum ClockWorkEventList
{
    None,
    RotatePlayerClockwork
}

// #. 평소에 취하고 있을 액션
public enum ActionEventList
{ 
    None,               // Num 0, IDLE or WALK;
    WorkInCompany,      // Num 1;
    TextingSmartPhone   // Num 2
}


public class NPC_Simple : MonoBehaviour
{
    private Animator anim;
    private NavMeshAgent agent;
    public NPC_Simple_StateMachine machine;

    [Header("NPC 상태들")]
    public bool bSad; // true = Sad, false = NotSad
    public bool bWalking; // true = Walking, false = IDLE
    public int iIDLE_Num;
    public int iAnimWalking;        // 걷기 애니메이션 인덱스
    public bool bClockWorkEventNPC; // 태엽을 돌려준 이후에 이벤트 수행 여부
    public bool bActionEventNPC; // 생성 시점부터 특정 행동 수행 여부
    public ClockWorkEventList clockworkEvent;
    public ActionEventList actionEventList;

    [Header("기타 오브젝트들")]
    public NPCHeart npcHeart;
    public NPC_ClockWork npcClockWork;
    private ClockWork clockWork;

    [Header("Walk 관련 컴포넌트들")]
    public Transform[] checkPoints; // 목표 지점 배열
    private int currentCheckPointIndex = 0; // 현재 목표 지점 인덱스

    private void Awake()
    {
        Init();
    }



    // NPC를 풀에서 꺼냈을 때마다 초기화하도록 OnEnable()에 ResetState() 호출
    private void OnEnable()
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();

        if (agent.isOnNavMesh)
        {
            ResetState();
        }
        else
        {
            Debug.LogWarning($"{gameObject.name} is not on a NavMesh. ResetState() skipped.");
        }
    }

    private void Init()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        // 최초 초기화
        anim.SetInteger("IDLE_Num", iIDLE_Num);

        anim.SetBool("Bool_Walk", bWalking);
        anim.SetBool("Bool_Sad", bSad);
        anim.SetBool("Bool_ActionEvent", bActionEventNPC);

        //if (npcClockWork != null)
        //    npcClockWork.canInteract = bSad;

        machine = new NPC_Simple_StateMachine(this);

        npcHeart.machine = machine;
        npcHeart.SetAnimator(anim);

        if (npcHeart != null)
        {

        }
        else
        {
            Debug.Log("심장이 비어있습니다");
        }
    }







    // ResetState()를 통해 풀에서 재사용 시 필요한 변수들을 재설정
    public void ResetState()
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();

        if (!agent.isOnNavMesh)
        {
            Debug.LogWarning($"{gameObject.name} is not on a NavMesh! Skipping ResetState().");
            return;
        }

        // 체크포인트 인덱스 초기화
        currentCheckPointIndex = 0;

        // NavMeshAgent 초기화
        agent.ResetPath();
        agent.isStopped = false;

        // 애니메이터 파라미터 초기화
        if (anim != null)
        {
            anim.SetInteger("IDLE_Num", iIDLE_Num);

            anim.SetBool("Bool_Walk", bWalking);
            anim.SetBool("Bool_Sad", bSad);
            anim.SetBool("Bool_ActionEvent", bActionEventNPC);
        }

        // 상태 머신 재시작 (필요한 경우)
        machine = new NPC_Simple_StateMachine(this);
    }




    private void Update()
    {
        machine?.OnStateUpdate();



    }





    private void FixedUpdate()
    {
        machine?.OnStateFixedUpdate();
    }

    public Animator GetAnimator()
    {
        return anim;
    }

    public NavMeshAgent GetNav()
    {
        return agent;
    }

    public int CurrentCheckPointIndex
    {
        get => currentCheckPointIndex;
        set => currentCheckPointIndex = value;
    }

    public ClockWork ClockWork
    {
        get { return clockWork; }
        set { clockWork = value; }
    }

    public void SpinClockWork()
    {
        clockWork.ClockWorkRotate();
    }


    public int avoid_Num;

    public void SetAvoidState()
    {
        anim.SetTrigger("doAvoid");
        anim.SetInteger("Avoid_Num", avoid_Num);
        bWalking = false;
        anim.SetBool("Bool_Walk", bWalking);

    }

    public void SetDropState()
    {
        anim.SetTrigger("doDrop");
    }





    // 
    public GameObject GetPlayerObject()
    {
        return GameAssistManager.Instance.GetPlayer();
    }












    // 플레이어 태엽 돌려주기
    public void RotatePlayerTaeyub()
    {
        StartCoroutine(RotatePlayerTaeyub_());
    }
    IEnumerator RotatePlayerTaeyub_()
    {
        yield return new WaitForSeconds(1f);


        anim.SetTrigger("doThankyouAction");


        yield return new WaitForSeconds(2.7f);


        agent.isStopped = false;
        agent.SetDestination(GameAssistManager.Instance.GetPlayer().transform.position);
        anim.SetInteger("Walk_Num", 1);
        anim.SetBool("Bool_Walk", true);
        while (agent.pathPending || agent.remainingDistance > 1f) yield return null; // 다음 프레임까지 대기
        agent.isStopped = true;
        anim.SetTrigger("ddddStop");
        anim.SetBool("Bool_Walk", false);


        yield return new WaitForSeconds(1f);


        anim.SetTrigger("doHandGesture");

        yield return new WaitForSeconds(1.5f);

        GameAssistManager.Instance.GetPlayerScript().SetTurnState(gameObject);


        yield return new WaitForSeconds(2f);

        GameAssistManager.Instance.GetPlayerScript().playerAnim.SetTrigger("doClockWork_Grapped");

        yield return new WaitForSeconds(0.5f);


        anim.SetTrigger("doRoateTaeyubStart");
        GameAssistManager.Instance.GetPlayerScript().playerAnim.SetTrigger("doClockWork_RotateStart");


        yield return new WaitForSeconds(2.6f);

        anim.SetTrigger("doRoateTaeyubEnd");
        machine.OnStateChange(machine.IDLEState);
        GameAssistManager.Instance.GetPlayerScript().playerAnim.SetTrigger("doClockWork_RotateEnd");
        GameAssistManager.Instance.PlayerInputLockOff();


    }


    public void ChangeStateToSubWay()
    {
        machine.OnStateChange(machine.SubwayState);
    }
}
