using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


// #. 태엽을 돌려주고 난 이후에 취할 액션
public enum ClockWorkEventList
{
    None   // Num 0
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
    public bool bSad; // true = Sad <-> false = NotSad
    public bool bWalking; // true = Walking <-> false = IDLE
    public bool bClockWorkEventNPC; // true = 태엽을 돌려준 이후에 특정한 이벤트를 취하는 캐릭터인지
    public bool bActionEventNPC; // 생성 시점부터 특정한 행동을 취하고 있는 NPC;
    public ClockWorkEventList clockworkEvent;
    public ActionEventList actionEventList;

    [Header("기타 오브젝트들")]
    public NPC_ClockWork npc_ClockWork; // 자기 등에 꽂혀 있는 태엽
    private ClockWork clockWork;        // 직접 회전 시킬 태엽


    [Header("Walk 관련 컴포넌트들")]
    public Transform[] checkPoints; // 목표 지점 배열
    private int currentCheckPointIndex = 0; // 현재 목표 지점 인덱스


    private void Awake()
    {
        Init();
    }
    private void Init()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("Bool_Walk", bWalking);
        anim.SetBool("Bool_Sad", bSad);
        anim.SetBool("Bool_ActionEvent", bActionEventNPC);

        agent = GetComponent<NavMeshAgent>();


        machine = new NPC_Simple_StateMachine(this);
        npc_ClockWork.machine = machine;
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





}
