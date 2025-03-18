using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BulbBug_OnRoad : MonoBehaviour
{
    public Animator anim;
    public NavMeshAgent nav;
    public BBBmachine machine;
    public Rigidbody rigid;
    public Collider myCollider;


    [Header("배회 관련 수치들")]
    public float timeBetweenDestinations = 3f;  // 목적지 사이에 대기할 시간
    public float patrolRange = 3f;  // 배회 범위 (현재 위치 기준)
    public float waitTime = 1f; // 목적지 도달 후 대기 시간
    public bool isWaiting = false;
    public float waitTimer = 0f; // 대기 시간 카운트

    [Header("")]
    public Transform[] checkPoints; // 순차적으로 방문할 목표 지점 배열
    private int currentIndex = 0; // 현재 목표 지점 인덱스



    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        myCollider = GetComponent<Collider>();





        Init();
    }
    private void Init()
    {
        machine = new BBBmachine(this);
    }


    private void Update()
    {
        machine?.OnStateUpdate();

    }

    private void FixedUpdate()
    {
        machine?.OnStateFixedUpdate();
    }

    public NavMeshAgent GetNav() { return nav;  }


    public int CurrentCheckPointIndex
    {
        get => currentIndex;
        set => currentIndex = value;
    }

}
