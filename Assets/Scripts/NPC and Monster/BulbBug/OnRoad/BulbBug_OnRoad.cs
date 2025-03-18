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


    [Header("��ȸ ���� ��ġ��")]
    public float timeBetweenDestinations = 3f;  // ������ ���̿� ����� �ð�
    public float patrolRange = 3f;  // ��ȸ ���� (���� ��ġ ����)
    public float waitTime = 1f; // ������ ���� �� ��� �ð�
    public bool isWaiting = false;
    public float waitTimer = 0f; // ��� �ð� ī��Ʈ

    [Header("")]
    public Transform[] checkPoints; // ���������� �湮�� ��ǥ ���� �迭
    private int currentIndex = 0; // ���� ��ǥ ���� �ε���



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
