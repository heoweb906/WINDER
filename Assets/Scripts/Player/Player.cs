using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    public bool stateChangeDebug;

    [Header("월드 기준 이동")]
    public bool isWorldAxis;
    [Range(0, 360)]
    public float yAxis;

    public PlayerStateMachine machine;

    public Transform camTransform;

    [Header("ThisComponent")]
    [SerializeField]
    public Rigidbody rigid;
    public List<BoxCollider> playerCollider;
    [Header("Animation")]
    public Animator playerAnim;
    [field: SerializeField] public PlayerAnimationData playerAnimationData { get; private set; }

    [Header("이동속도")]
    [SerializeField, Range(0, 60)]
    public float playerWalkSpeed;
    [SerializeField, Range(0, 60)]
    public float playerRunSpeed;
    [SerializeField, Range(0, 60)]
    public float playerMoveLerpSpeed;
    [SerializeField, Range(0, 60)]
    public float playerMoveLerpSpeedOnJump;
    [SerializeField, Range(0, 60)]
    public float playerDefaultRotateLerpSpeed;
    [Header("점프")]
    [SerializeField, Range(0, 20)]
    public float firstJumpPower;
    [SerializeField, Range(0, 100)]
    public float fallingPower;
    [SerializeField, Range(0, 20)]
    public float maxFallingSpeed;

    [HideInInspector]
    public float moveLerpSpeed;

    [Header("경사로")]
    [SerializeField, Range(0, 2)]
    public float rayDistance = 1f;
    public RaycastHit slopeHit;
    public int groundLayer;
    public int cliffLayer;
    [SerializeField, Range(0, 90)]
    public int maxSlopeAngle;
    [SerializeField]
    public GameObject raycastOrigin;


    [Header("Platform")]
    [Range(0, 60)] public float platformVelocityLerp;
    [HideInInspector] public Vector3 platformVelocity;

    [Header("Foot IK")]
    [SerializeField, Range(0, 1f)]
    public float distanceGround;


    public float playerMoveSpeed;
    public float rotateLerpSpeed;
    public bool isRun = false;

    public Vector3 curDirection = Vector3.zero;
    public Vector3 preDirection = Vector3.zero;

    [Header("상호작용 오브젝트")]
    public InteractableObject curInteractableObject;

    [Header("태엽 오브젝트")]
    public float detectionRadius = 10f; // 탐지 반경
    public float clockWorkInteractionDistance_Wall = 1f; // 상호작용 거리
    public float clockWorkInteractionDistance_Floor = 1f; // 상호작용 거리
    public ClockWork curClockWork; // 가장 가까운 ClockWork 오브젝트
    public Vector3 targetPos; // 가장 가까운 ClockWork 오브젝트
    public bool isGoToTarget;

    [Header("물건 옮기기")]
    public CarriedObject curCarriedObject;
    public Transform CarriedObjectPos;
    public float carriedObjectInteractionDistance = 1f; // 상호작용 거리
    public bool isCarryObject;
    [Range(0, 50)]
    public float throwPower;
    public PartsArea partsArea;

    [Header("물건 밀기")]
    public GrabObject curGrabObject;
    public Transform grabPos;
    public float grabObjectInteractionDistance = 1f; // 상호작용 거리
    [SerializeField, Range(0, 60)]
    public float playerGrapRotateLerpSpeed;
    [SerializeField, Range(0, 60)]
    public float playerGrapMoveSpeed;

    [Header("Climb")]
    public float cliffCheckRayDistance = 1f; // 탐지 반경
    public RaycastHit cliffRayHit;
    [HideInInspector]
    public Vector3 hangingPos;
    public float hangingPosOffset_Front;
    public float hangingPosOffset_Height;


    [Header("물건 잡기 IK")]
    public bool isHandIK = false;

    private Tween armAngleTween;

    [Header("Ragdoll")]
    public Transform pelvis;
    public Transform leftHips;
    public Transform leftKnee;
    public Transform leftFoot;
    public Transform rightHips;
    public Transform rightKnee;
    public Transform rightFoot;
    public Transform leftArm;
    public Transform leftElbow;
    public Transform rightArm;
    public Transform rightElbow;
    public Transform middleSpine;
    public Transform head;
    [Range(0, 100)]
    public float totalMass = 20f;
    [Range(0, 100)]
    public float strength = 0f;
    public bool flipForward = false;

    public int dieIndex = 1;

    private void Awake()
    {
        playerAnimationData.Initialize();
        Init();
        rigid = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        isGoToTarget = false;
        Application.targetFrameRate = 180;
        isHandIK = false;
        camTransform = FindObjectOfType<Camera>().transform;

        int layer1 = LayerMask.NameToLayer("Player");
        int layer2 = LayerMask.NameToLayer("Carry");
        Physics.IgnoreLayerCollision(layer1, layer2, true);
    }

    private void Init()
    {
        machine = new PlayerStateMachine(this);
    }

    private void Start()
    {
        groundLayer = ~(1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Ignore Raycast") | 1 << LayerMask.NameToLayer("Carry"));
        cliffLayer = (1 << LayerMask.NameToLayer("Cliff"));
    }

    private void Update()
    {
        machine?.OnStateUpdate();
    }

    private void FixedUpdate()
    {
        machine?.OnStateFixedUpdate();
    }


    public void OnMovementStateAnimationEnterEvent()
    {
        machine.OnAnimationEnterEvent();
    }

    public void OnMovementStateAnimationExitEvent()
    {
        machine.OnAnimationExitEvent();
    }

    public void OnMovementStateAnimationTransitionEvent()
    {
        machine.OnAnimationTransitionEvent();
    }


    [SerializeField]
    public List<GameObject> groundList = new List<GameObject>();
    public MovingPlatform curMovingPlatform;
    private void OnTriggerEnter(Collider other)
    {
        machine.OnTriggerEnter(other);
    }


    private void OnTriggerExit(Collider other)
    {
        machine.OnTriggerExit(other);
    }

    ///////////////////
    private void OnDrawGizmos()
    {
        // 탐지 반경을 시각적으로 표시
        Gizmos.color = Color.green; // 기즈모 색상 설정
        Gizmos.DrawWireSphere(transform.position, detectionRadius); // WireSphere로 탐지 범위 그리기
    }


    public Transform tf_L_UpperArm;
    public Transform tf_L_Hand;
    public Transform tf_R_UpperArm;
    public Transform tf_R_Hand;
    public int _x;
    public float angle = 0;
    public bool isSetAngleZero = false;

    public void LateUpdate()
    {
        if (!isHandIK)
            return;

        if (Input.GetKey(KeyCode.Z))
        {
            if (Input.GetKey(KeyCode.LeftShift))
                _x--;
            else
                _x++;
        }

        BoxCollider _col = curCarriedObject.GetComponent<BoxCollider>();
        Vector3 boxScale = Vector3.Scale(_col.size * 0.5f, _col.transform.lossyScale);
        Vector3 leftTargetPos3D = curCarriedObject.transform.position - transform.right * boxScale.x;
        Vector2 leftTargetPos = new Vector2(leftTargetPos3D.x, leftTargetPos3D.z);

        Vector2 _leftUpperArm = new Vector2(tf_L_UpperArm.transform.position.x, tf_L_UpperArm.transform.position.z);
        Vector2 _leftHand = new Vector2(tf_L_Hand.transform.position.x, tf_L_Hand.transform.position.z);

        Vector2 upperArmToHand = _leftHand - _leftUpperArm;
        Vector2 upperArmToTarget = leftTargetPos - _leftUpperArm;

        float dotProduct = Vector2.Dot(upperArmToHand.normalized, upperArmToTarget.normalized);


        float targetAngle = Mathf.Acos(dotProduct) * Mathf.Rad2Deg + _x;
        if(isSetAngleZero)
            targetAngle = 0;
        
        SetArmAngle(targetAngle);
        
        Vector3 leftArmRotation = tf_L_UpperArm.eulerAngles;
        leftArmRotation.y += angle;
        tf_L_UpperArm.eulerAngles = leftArmRotation;

        Vector3 rightArmRotation = tf_R_UpperArm.eulerAngles;
        rightArmRotation.y -= angle;
        tf_R_UpperArm.eulerAngles = rightArmRotation;
    }
    public float carryWeight = 1;

    public void SetCarryWeight()
    {
        carryWeight = 1;
        DOTween.To(() => carryWeight, x => carryWeight = x, 0, 0.3f);
    }

    public void SetHandIKAngle()
    {

    }

    public void SetColliderTrigger(bool _bool)
    {
        foreach (var item in playerCollider)
        {
            item.enabled = _bool;
        }
    }

    public void SetRootMotion()
    {
        StartCoroutine(C_SetRootMotion());
    }
    IEnumerator C_SetRootMotion()
    {
        yield return new WaitForSeconds(0.1f);
        playerAnim.applyRootMotion = false;
        playerAnim.updateMode = AnimatorUpdateMode.Normal;
    }

    public void SetPlayerPhysicsIgnore(Collider _col, bool _bool)
    {
        if(_bool)
            _col.gameObject.layer = LayerMask.NameToLayer("Carry");
        else
            _col.gameObject.layer = LayerMask.NameToLayer("Interactable");
    }

    public void StartExitClimbingToTop()
    {
        StartCoroutine(I_ExitClimbingToTop());
    }

    private IEnumerator I_ExitClimbingToTop()
    {
        machine.OnStateChange(machine.IdleState);
        SetColliderTrigger(true);

        yield return new WaitForSeconds(0.2f);

        playerAnim.applyRootMotion = false;
        playerAnim.updateMode = AnimatorUpdateMode.Normal;
    }

    public void SetArmAngle(float targetAngle)
    {
        if (armAngleTween != null)
        {
            armAngleTween.Kill();
        }
        armAngleTween = DOTween.To(() => angle, x => angle = x, targetAngle, 0.5f);
    }

    public void KillArmAngleTween()
    {
        if (armAngleTween != null)
        {
            armAngleTween.Kill();
        }
    }

    public void SetDieState(int index)
    {
        // 0 = 분해
        // 1 = 잡혀서 죽음
        dieIndex = index;
        machine.OnStateChange(machine.UC_DieState);
    }

}
