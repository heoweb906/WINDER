using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public enum GuardMType
{
    FrontCompany,
    Wandering
}

public class GuardM : MonoBehaviour
{
    public GuardMType guardMType;

    private GameObject player;
    private LayerMask obstacleLayerMask;
    private Vector3 guardRayOriginOffset = Vector3.up; // 감시자 Ray 시작 위치 오프셋
    private Vector3 playerRayTargetOffset = Vector3.up; // 플레이어 Ray 목표 위치 오프셋
    private Coroutine nowCoroutine;

    private Vector3 transformHome;
    private Quaternion gameObjRotation;
    

    public Animator anim;

    public GuardMStateMachine machine;
    public GuardM_Visualrange visualRange;
    public GuardM_CheckingArea area;
    public NavMeshAgent nav;

    // 머리가 플레이어를 추격
    public TrackingHead_ToPlayer trackingHead;

    // 공격
    public Transform transformGrabPlayer; 
    public float fAttackRange; 

    [Header("배회 경비병이 사용할 컴포넌트들")]
    public Transform transformStart;
    public Transform transformEnd;
    public float fMoveTime;     // 한지점에서 한지점으로 가는 최대 시간


    private void Awake()
    {
        Init();

        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();

        transformHome = transform.position;
        gameObjRotation = transform.rotation;

        player = GameObject.FindWithTag("Player").transform.root.gameObject;
        obstacleLayerMask = LayerMask.GetMask("Obstacle");

        if (transformStart == null) Debug.Log("시작 지점이 설정되지 않았습니다"); 
        if (transformEnd == null) Debug.Log("종료 지점이 설정되지 않았습니다"); 
    }
    private void Init()
    {
        machine = new GuardMStateMachine(this);
    }



    private void Update()
    {
        machine?.OnStateUpdate();
    }

    private void FixedUpdate()
    {
        machine?.OnStateFixedUpdate();
    }


    public void StopGuardCoroutine()
    {
        if(nowCoroutine != null) StopCoroutine(nowCoroutine);

    }
    public void StartGuardCoroutine(IEnumerator coroutine)
    {
        if(nowCoroutine != null ) StopCoroutine(nowCoroutine);
        nowCoroutine = StartCoroutine(coroutine);
    }


    public Vector3 GetHomeTransform()
    {
        return transformHome;
    }

    // Home 위치 Setter
    public void SetHomeTransform(Vector3 newHome)
    {
        transformHome = newHome;
    }

    // 오브젝트 회전 Getter
    public Quaternion GetObjRotation()
    {
        return gameObjRotation;
    }

    // 오브젝트 회전 Setter
    public void SetObjRotation(Quaternion newRotation)
    {
        gameObjRotation = newRotation;
    }



    public GameObject GetPlayerObj()
    {
        return player;
    }

    // #. 플레이어랑 경비병 사이에 장애물이 있는지 확인
    public bool IsObstacleBetween()
    {
        // 감시자와 플레이어의 위치에 오프셋 적용
        Vector3 guardPosition = transform.position + guardRayOriginOffset;
        Vector3 playerPosition = player.transform.position + playerRayTargetOffset;
        Vector3 direction = (playerPosition - guardPosition).normalized;
        float distance = Vector3.Distance(guardPosition, playerPosition);

        // Ray를 눈으로 확인하기 위해 Debug.DrawRay 사용
        Debug.DrawRay(guardPosition, direction * distance, Color.red);

        // Raycast로 감시자와 플레이어 사이를 검사 (Obstacle 레이어만 감지)
        if (Physics.Raycast(guardPosition, direction, out RaycastHit hit, distance, obstacleLayerMask))
        {
            return true;
        }

        return false;
    }



}
