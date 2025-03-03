using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BulbBug : MonoBehaviour
{
    public Animator anim;
    public NavMeshAgent nav;
    public BulbBugStateMachine machine;
    public Rigidbody rigid;
    public Collider myCollider;

    public LayerMask layer; // gameObject.layer = LayerMask.NameToLayer("Player");
    public CarriedObject carriedObj;

    [Header("배회 관련 수치들")]
    public float timeBetweenDestinations = 3f;  // 목적지 사이에 대기할 시간
    public float patrolRange = 3f;  // 배회 범위 (현재 위치 기준)
    public float waitTime = 1f; // 목적지 도달 후 대기 시간
    public bool isWaiting = false;
    public float waitTimer = 0f; // 대기 시간 카운트

    [Header("플레이어 감지 Ray")]
    public GameObject CheckingAreaObj_1;     // 큰 확인 범위
    public GameObject CheckingAreaObj_2;     // 작은 확인 범위
    public PlayerCheckArea CheckingArea_1;
    public PlayerCheckArea CheckingArea_2;

    [Header("테스트용")]
    public GameObject ObjFilament;
    private Material mat_Filament;

    // public GameObject LightObj;



    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        myCollider = GetComponent<Collider>();

        carriedObj = GetComponent<CarriedObject>();
        CheckingArea_1 = CheckingAreaObj_1.GetComponent<PlayerCheckArea>();
        CheckingArea_2 = CheckingAreaObj_2.GetComponent<PlayerCheckArea>();


        Init();
    }
    private void Init()
    {
        machine = new BulbBugStateMachine(this);


        if (ObjFilament != null)
        {
            // 원본 머티리얼을 복제하여 개별적으로 사용
            mat_Filament = new Material(ObjFilament.GetComponent<Renderer>().sharedMaterial);
            ObjFilament.GetComponent<Renderer>().material = mat_Filament;
        }
        else Debug.Log("ObjFilament가 없습니다.");
    }



    private void Update()
    {
        machine?.OnStateUpdate();

    }

    private void FixedUpdate()
    {
        machine?.OnStateFixedUpdate();
    }



    public void ToggleEmission(bool isOn)
    {
        if (mat_Filament != null)
        {
            if (isOn)
            {
                mat_Filament.EnableKeyword("_EMISSION");
                mat_Filament.SetColor("_EmissionColor", Color.white * 2f); // 원하는 색상과 강도 조절 가능
            }
            else
            {
                mat_Filament.DisableKeyword("_EMISSION");
                mat_Filament.SetColor("_EmissionColor", Color.black);
            }
        }
    }





}
