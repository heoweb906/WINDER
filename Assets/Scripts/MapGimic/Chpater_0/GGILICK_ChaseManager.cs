using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GGILICK_ChaseManager : MonoBehaviour
{
    // 추격 상태를 정의하는 열거형
    public enum ChaseState
    {
        Idle,           // 대기 상태
        Starting,       // 추격 시작 (가속)
        Chasing,        // 추격 중
        Stopping,       // 멈추는 중
        Stunned,        // 유리벽 충돌 후 기절 상태
        Attacking,      // 공격 상태
        AttackExecute,  // 공격 실행 상태
        Falling,        // 낙하 중
        Lying           // 낙하 후 누워있는 상태
    }
    
    [Header("참조")]
    [SerializeField] private Transform TargetPosition;
    [SerializeField] private GameObject ggilick;
    [SerializeField] private Transform playerTransform;
    
    [Header("속도 설정")]
    [SerializeField] private float baseSpeed = 3.0f;      // 기본 이동 속도
    [SerializeField] private float maxSpeed = 6.0f;       // 최대 이동 속도
    [SerializeField] private float distanceThreshold = 10.0f;  // 속도 증가가 시작되는 거리
    [SerializeField] private float accelerationTime = 1.5f;    // 가속 시간
    [SerializeField] private float decelerationTime = 1.0f;    // 감속 시간
    
    [Header("조건 설정")]
    [SerializeField] private float stopDistance = 1.0f;   // 목표 지점에 도달했다고 판단할 거리
    [SerializeField] private float stunDuration = 3.0f;   // 유리벽 충돌 후 기절 시간
    
    [Header("경사로 설정")]
    [SerializeField] private Transform slopeStartObject;  // 경사로 시작 지점 오브젝트
    [SerializeField] private Transform slopeEndObject;    // 경사로 종료 지점 오브젝트
    [SerializeField] private float slopeXRotation = 5f;   // 경사로에서의 X축 회전 (도)
    
    [Header("낙하 설정")]
    [SerializeField] private Transform endPoint;          // 낙하 시작 지점
    [SerializeField] private Transform fallingPoint;      // 낙하 도착 지점
    [SerializeField] private Transform fallingFloorCollider;   
    [SerializeField] private float fallingDuration = 2.0f; // 낙하 지속 시간
    [SerializeField] private float lyingDuration = 5.0f;   // 낙하 후 누워있는 시간

    [Header("공격 설정")]
    [SerializeField] private float attackSpeed = 8.0f;     // 공격 속도
    [SerializeField] private float attackDuration = 1.0f;  // 공격 지속 시간
    [SerializeField] private float attackExecuteDuration = 0.5f; // 공격 실행 지속 시간
    [SerializeField] private float attackRange = 2.0f;     // 공격 범위
    [SerializeField] private float returnSpeed = 3.0f;     // 타겟 위치로 돌아갈 때 X축 보간 속도
    [SerializeField] private float rotationSpeed = 2.0f;   // 회전 속도 계수
    [SerializeField] private float attackDistance = 1.5f;  // 공격 실행 거리

    public CineCameraChager cineChager;   // 끼릭이의 모습을 보여줌
    public Transform transformTeleport_FrontGgilick;     // 플레이어 순간이동 위치
    public Camera_Falling cameraFalling;
    public HandheldCamera handHeld_falling;

    public HandheldCamera handheld_first_wall;
    public HandheldCamera_DollyCart handheld_Chase_wall;


    [SerializeField] private List<GlassWall> glassWalls;

    
    private ChaseState currentState = ChaseState.Idle;
    private float stateTimer = 0f;
    private float currentSpeedFactor = 0f;  // 0~1 사이의 속도 계수
    private Vector3 fallingStartPosition;   // 낙하 시작 위치
    private float fallingProgress = 0f;     // 낙하 진행도 (0~1)

    private Animator animator;
    
    private Vector3 attackTargetPosition;    // 공격 목표 위치(플레이어 위치)


    [SerializeField]
    private ChaseStartBattery chaseStartBattery;

    private void Start()
    {
        playerTransform = GameAssistManager.Instance.GetPlayer().transform;
        // 플레이어 참조 확인
        if (playerTransform == null)
        {
            Debug.LogWarning("플레이어가 할당되지 않았습니다. 인스펙터에서 할당해주세요.");
        }
        
        // 충돌 감지를 위해 ggilick에 Collider가 있는지 확인
        if (ggilick.GetComponent<Collider>() == null)
        {
            Debug.LogWarning("ggilick에 Collider가 없습니다. 충돌 감지를 위해 Collider를 추가해주세요.");
        }

        animator = ggilick.GetComponent<Animator>();
    }


    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Y))
        //{
        //    cameraFalling.DropShake(3f);
        //    handHeld_falling.PulseShake(1.2f, 5f, 7f);
        //    TransitionToState(ChaseState.Falling);
        //}
    }




    private void FixedUpdate()
    {
        UpdateState();
        MoveGgilick();
    }
    
    private void UpdateState()
    {
        stateTimer += Time.fixedDeltaTime;
        
        // 현재 상태에 따른 처리
        switch (currentState)
        {
            case ChaseState.Idle:
                // 대기 상태에서는 아무것도 하지 않음
                break;
                
            case ChaseState.Starting:
                // 가속 단계
                currentSpeedFactor = Mathf.Clamp01(stateTimer / accelerationTime);
                if (stateTimer >= accelerationTime)
                {
                    TransitionToState(ChaseState.Chasing);
                }
                break;
                
            case ChaseState.Chasing:
                // 목표 지점에 도달했는지 확인
                if (Vector3.Distance(ggilick.transform.position, TargetPosition.position) <= stopDistance)
                {
                    TransitionToState(ChaseState.Stopping);
                }
                
                // 낙하 지점에 도달했는지 확인 (z축 기준)
                if (endPoint != null && ggilick.transform.position.z >= endPoint.position.z)
                {
                    cameraFalling.DropShake(3f);
                    handHeld_falling.PulseShake(1.2f, 5f, 7f);

                    TransitionToState(ChaseState.Falling);
                }
                break;
                
            case ChaseState.Stopping:
                // 감속 단계
                currentSpeedFactor = Mathf.Clamp01(1.0f - (stateTimer / decelerationTime));
                if (stateTimer >= decelerationTime)
                {
                    TransitionToState(ChaseState.Idle);
                }
                break;
                
            case ChaseState.Stunned:
                // 기절 시간이 지나면 다시 추격 시작
                if (stateTimer >= stunDuration)
                {
                    TransitionToState(ChaseState.Starting);
                }
                break;
                
            case ChaseState.Attacking:
                // 목표 위치에 충분히 가까워졌는지 확인 (XZ 평면만 고려)
                Vector3 currentPosXZ = new Vector3(ggilick.transform.position.x, 0, ggilick.transform.position.z);
                Vector3 targetPosXZ = new Vector3(attackTargetPosition.x, 0, attackTargetPosition.z);
                float distanceToTargetXZ = Vector3.Distance(currentPosXZ, targetPosXZ);
                
                // 디버그 로그 추가
                if (distanceToTargetXZ <= attackDistance + 0.1f)
                {
                    Debug.Log($"목표 접근 중: {distanceToTargetXZ}m, 필요 거리: {attackDistance}m");
                }
                
                if (distanceToTargetXZ <= attackDistance)
                {
                    Debug.Log($"공격 실행 상태로 전환! 거리: {distanceToTargetXZ}m");
                    // 공격 실행 상태로 전환
                    TransitionToState(ChaseState.AttackExecute);
                }
                
                // 시간이 너무 오래 지나면 강제로 공격 상태 전환 (안전장치)
                if (stateTimer > 3.0f)
                {
                    Debug.LogWarning("공격 타임아웃! 강제로 공격 실행 상태로 전환");
                    TransitionToState(ChaseState.AttackExecute);
                }
                break;
                
            case ChaseState.AttackExecute:
                // 공격 실행 후 시간이 지나면 다시 추격 시작
                if (stateTimer >= attackExecuteDuration)
                {
                    // 공격 후 Starting 상태로 전환
                    TransitionToState(ChaseState.Starting);
                }
                break;
                
            case ChaseState.Falling:
                // 낙하 진행
                fallingProgress = stateTimer / fallingDuration;
                if (fallingPoint != null)
                {
                    // 낙하 궤적 계산 (포물선 형태)
                    float t = fallingProgress;
                    if (t > 1f) t = 1f;
                    
                    // 시작 위치에서 도착 위치로 선형 보간
                    Vector3 linearPosition = Vector3.Lerp(fallingStartPosition, fallingPoint.position, t);
                    
                    // Y축에 포물선 효과 추가 (처음에는 위로 올라갔다가 내려옴)
                    float heightOffset = Mathf.Sin(t * Mathf.PI) * 2f; // 최대 높이를 조절할 수 있음
                    linearPosition.y += (1 - t) * heightOffset;  // 낙하 후반부로 갈수록 높이 줄임
                    
                    // 위치 적용
                    ggilick.transform.position = linearPosition;
                }
                
                // 낙하 완료 후 Lying 상태로 전환
                if (stateTimer >= fallingDuration)
                {
                    TransitionToState(ChaseState.Lying);
                }
                break;
                
            case ChaseState.Lying:
                // Lying은 최종 상태이므로 다른 상태로 전환하지 않음
                // 이벤트가 종료된 시점으로 간주하고 이 상태를 유지함
                break;
        }
    }
    
    private void MoveGgilick()
    {
        // 이동이 필요한 상태인지 확인
        if (currentState == ChaseState.Idle || currentState == ChaseState.Stunned 
            || currentState == ChaseState.Falling || currentState == ChaseState.Lying
            || currentState == ChaseState.AttackExecute)
            return;
            
        // 공격 상태일 때 플레이어를 향해 빠르게 이동
        if (currentState == ChaseState.Attacking)
        {
            // 공격 방향 계산 (XZ 평면에서만)
            Vector3 targetPosXZ = new Vector3(attackTargetPosition.x, ggilick.transform.position.y, attackTargetPosition.z);
            Vector3 attackDirection = (targetPosXZ - ggilick.transform.position).normalized;
            Vector3 attackMovement = attackDirection * attackSpeed * Time.fixedDeltaTime;
            
            // 경사로 고려 (y축 이동 확인)
            bool isOnSlopeAttacking = false;
            
            // 경사로 참조 오브젝트가 설정되어 있는지 확인
            if (slopeStartObject != null && slopeEndObject != null)
            {
                // 현재 Z 위치에 따라 경사로에 있는지 확인
                float currentZ = ggilick.transform.position.z;
                float slopeStartZ = slopeStartObject.position.z;
                float slopeEndZ = slopeEndObject.position.z;
                
                // 시작과 끝 지점 순서를 정규화 (시작이 더 작은 값, 끝이 더 큰 값)
                if (slopeStartZ > slopeEndZ)
                {
                    float temp = slopeStartZ;
                    slopeStartZ = slopeEndZ;
                    slopeEndZ = temp;
                }
                
                // +z 방향 이동에 맞게 경사로 체크
                isOnSlopeAttacking = currentZ >= slopeStartZ && currentZ <= slopeEndZ;
                
                // 경사로에 있을 경우 y축 이동 추가
                if (isOnSlopeAttacking)
                {
                    // 경사로의 길이 계산
                    float slopeLength = Mathf.Abs(slopeStartZ - slopeEndZ);
                    
                    // 경사로에서의 진행도 (0~1) 계산
                    float slopeProgress = Mathf.Abs(currentZ - slopeStartZ) / slopeLength;
                    
                    // 진행 방향에 따라 올라가거나 내려가는 y 이동 계산
                    float slopeAngleRad = slopeXRotation * Mathf.Deg2Rad;
                    
                    // z 이동 거리에 따른 y 변화량 계산 (탄젠트 이용)
                    float yOffset = attackMovement.z * Mathf.Tan(slopeAngleRad);
                    
                    // 움직임 벡터에 y 성분 추가
                    attackMovement.y = yOffset;
                }
            }
            
            ggilick.transform.position += attackMovement;
            
            // 공격 방향으로 회전 (XZ 평면 기준)
            if (attackDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(attackDirection);
                
                // 경사로에 있을 경우 X축 회전 적용
                if (isOnSlopeAttacking)
                {
                    Vector3 eulerAngles = targetRotation.eulerAngles;
                    eulerAngles.x = -slopeXRotation;
                    targetRotation = Quaternion.Euler(eulerAngles);
                }
                
                ggilick.transform.rotation = Quaternion.Slerp(ggilick.transform.rotation, targetRotation, 10f * Time.fixedDeltaTime);
            }
            
            return;
        }
        
        // 플레이어와의 거리 계산
        float distanceToPlayer = Vector3.Distance(ggilick.transform.position, playerTransform.position);
        
        // 거리에 따른 속도 계산 (거리가 멀수록 속도 증가)
        float distanceSpeedMultiplier = Mathf.Clamp01(distanceToPlayer / distanceThreshold);
        
        // 최종 속도 계산 (상태에 따른 속도 계수 적용)
        float currentSpeed = Mathf.Lerp(baseSpeed, maxSpeed, distanceSpeedMultiplier) * currentSpeedFactor;
        
        // 목표 위치 계산
        Vector3 targetPosition;
        
        // 추격 상태에서 공격 후 복귀하는 경우 X축을 보간
        bool isReturningToPath = currentState == ChaseState.Chasing && 
                                (Mathf.Abs(ggilick.transform.position.x - TargetPosition.position.x) > 0.5f);
        
        if (isReturningToPath)
        {
            // X축 위치를 목표 지점의 X축 위치로 서서히 보간
            float newX = Mathf.Lerp(ggilick.transform.position.x, TargetPosition.position.x, returnSpeed * Time.fixedDeltaTime);
            targetPosition = new Vector3(newX, ggilick.transform.position.y, TargetPosition.position.z);
        }
        else
        {
            targetPosition = new Vector3(TargetPosition.position.x, ggilick.transform.position.y, TargetPosition.position.z);
        }
        
        Vector3 direction = (targetPosition - ggilick.transform.position).normalized;
        Vector3 movement = direction * currentSpeed * Time.fixedDeltaTime;
        
        // 기본적으로 y축 이동을 제거
        movement.y = 0f;
        
        // 경사로 체크
        bool isOnSlope = false;
        
        // 경사로 참조 오브젝트가 설정되어 있는지 확인
        if (slopeStartObject != null && slopeEndObject != null)
        {
            // 현재 Z 위치에 따라 경사로에 있는지 확인
            float currentZ = ggilick.transform.position.z;
            float slopeStartZ = slopeStartObject.position.z;
            float slopeEndZ = slopeEndObject.position.z;
            
            // 시작과 끝 지점 순서를 정규화 (시작이 더 작은 값, 끝이 더 큰 값)
            if (slopeStartZ > slopeEndZ)
            {
                float temp = slopeStartZ;
                slopeStartZ = slopeEndZ;
                slopeEndZ = temp;
            }
            
            // +z 방향 이동에 맞게 경사로 체크
            isOnSlope = currentZ >= slopeStartZ && currentZ <= slopeEndZ;
            
            // 경사로에 있을 경우 y축 이동 추가
            if (isOnSlope)
            {
                // 경사로의 길이 계산
                float slopeLength = Mathf.Abs(slopeStartZ - slopeEndZ);
                
                // 경사로에서의 진행도 (0~1) 계산
                float slopeProgress = Mathf.Abs(currentZ - slopeStartZ) / slopeLength;
                
                // 진행 방향에 따라 올라가거나 내려가는 y 이동 계산
                // 기울기를 음수로 적용 (경사로 각도의 반대)
                float slopeAngleRad = slopeXRotation * Mathf.Deg2Rad; // 부호 변경: 음수에서 양수로
                
                // z 이동 거리에 따른 y 변화량 계산 (탄젠트 이용)
                float yOffset = movement.z * Mathf.Tan(slopeAngleRad);
                
                // 움직임 벡터에 y 성분 추가
                movement.y = yOffset;
            }
        }
        
        // 이동 적용
        ggilick.transform.position += movement;
        
        // 이동 방향으로 회전
        if (direction != Vector3.zero)
        {
            // 기본 회전 (진행 방향)
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            
            // 경사로에 있을 경우 X축 회전 적용
            if (isOnSlope)
            {
                // 기존 회전에 X축 회전(5도) 추가 (경사로 각도는 그대로 양수로 적용)
                Vector3 eulerAngles = targetRotation.eulerAngles;
                eulerAngles.x = -slopeXRotation;
                targetRotation = Quaternion.Euler(eulerAngles);
            }
            else
            {
                // 경사로가 아닌 경우 X축 회전 0도
                Vector3 eulerAngles = targetRotation.eulerAngles;
                eulerAngles.x = 0f;
                targetRotation = Quaternion.Euler(eulerAngles);
            }
            
            // 공격 후 경로로 복귀하는 경우 더 부드러운 회전 적용
            float actualRotationSpeed;
            if (isReturningToPath)
            {
                // 타겟 위치와의 각도 차이에 따라 회전 속도 조절
                Vector3 targetDir = (TargetPosition.position - ggilick.transform.position).normalized;
                float dotProduct = Vector3.Dot(ggilick.transform.forward, targetDir);
                float angleNormalized = (1 - dotProduct) * 0.5f; // 0(같은방향)~1(반대방향) 범위로 정규화
                
                // 각도 차이가 클수록 천천히 회전
                actualRotationSpeed = Mathf.Lerp(rotationSpeed * 3f, rotationSpeed, angleNormalized);
            }
            else
            {
                // 일반 추격 중에는 기본 회전 속도 사용
                actualRotationSpeed = Mathf.Lerp(2f, 5f, currentSpeedFactor);
            }
            
            ggilick.transform.rotation = Quaternion.Slerp(ggilick.transform.rotation, targetRotation, actualRotationSpeed * Time.fixedDeltaTime);
        }
    }

    private bool isFirstWall = true;
    
    private void TransitionToState(ChaseState newState)
    {
        // 상태 전환 시 로그 출력
        Debug.Log($"GGILICK 상태 변경: {currentState} -> {newState}");
        
        // 상태 변경 및 타이머 초기화
        currentState = newState;
        stateTimer = 0f;
        
        // 새 상태에 진입할 때 초기 설정
        switch (newState)
        {
            case ChaseState.Idle:
                currentSpeedFactor = 0f;
                break;
                
            case ChaseState.Starting:
                currentSpeedFactor = 0f;
                animator.SetTrigger("Walk");
                break;
                
            case ChaseState.Chasing:
                currentSpeedFactor = 1f;
                animator.SetTrigger("Run");
                break;
                
            case ChaseState.Stopping:
                // 현재 속도 유지하면서 감속 시작
                break;
                
            case ChaseState.Stunned:
                if(isFirstWall){
                    handheld_first_wall.PulseShake(1.2f, 5f, 7f);
                    isFirstWall = false;
                }
                else{
                    handheld_Chase_wall.TriggerStrongShake(4f,0.6f);
                }
                currentSpeedFactor = 0f;
                animator.SetTrigger("Brake");
                // 기절 효과 추가 (필요시 애니메이션 등 추가)
                break;
                
            case ChaseState.Attacking:
                currentSpeedFactor = 1f;
                // 현재 플레이어 위치를 공격 목표로 설정 (y 좌표는 사용하지 않음)
                attackTargetPosition = new Vector3(playerTransform.position.x, 
                                                 0f, // y 좌표는 중요하지 않음, 이동 시 현재 위치 사용
                                                 playerTransform.position.z);
                break;
                
            case ChaseState.AttackExecute:
                currentSpeedFactor = 0f;
                animator.SetTrigger("Brake"); // 공격 실행 시 Brake 애니메이션 실행
                // 공격 범위 내에 플레이어가 있는지 확인
                CheckPlayerInAttackRange();
                break;
                
            case ChaseState.Falling:
                currentSpeedFactor = 0f;
                animator.SetTrigger("Fall");
                // 낙하 시작 위치 저장
                fallingStartPosition = ggilick.transform.position;
                fallingProgress = 0f;
                fallingFloorCollider.gameObject.SetActive(false);
                foreach (var glassWall in glassWalls)
                {
                    glassWall.CrashGlassWall();
                }
                playerTransform.GetComponent<Player>().SetFallingState();


                StartCoroutine(PlayerTeleport());

                break;
                
            case ChaseState.Lying:
                currentSpeedFactor = 0f;
                animator.SetTrigger("Lying");
                break;
        }
    }
    
    // 충돌 감지
    private void OnTriggerEnter(Collider other)
    {
        // GlassWall 스크립트가 있는 오브젝트와 충돌했는지 확인
        if (other.GetComponent<GlassWall>() != null)
        {
            Debug.Log("유리벽과 충돌! 기절 상태로 전환");
            TransitionToState(ChaseState.Stunned);
            other.gameObject.GetComponent<GlassWall>().CrashGlassWall();
            
            // 필요시 충돌 효과 추가 (소리, 파티클 등)
            // PlayCollisionEffect();
        }
        // 공격 트리거와 충돌했는지 확인
        if(other.GetComponent<GGilicAttackTrigger>() != null){
            Debug.Log("공격 트리거와 충돌! 공격 상태로 전환");
            TransitionToState(ChaseState.Attacking);
        }
    }
    
    // 외부에서 호출할 수 있는 공개 메서드들
    public void ChaseStart()
    {
        Debug.Log("추격 시작");
        TransitionToState(ChaseState.Starting);
    }
    
    public void ChaseStop()
    {
        Debug.Log("추격 종료");
        TransitionToState(ChaseState.Stopping);
    }
    
    public void StunGgilick()
    {
        Debug.Log("GGILICK 기절");
        TransitionToState(ChaseState.Stunned);
    }
    
    public ChaseState GetCurrentState()
    {
        return currentState;
    }
    

    IEnumerator PlayerTeleport()
    {
        InGameUIController.Instance.bIsUIDoing = true;


        yield return new WaitForSeconds(2f);

        InGameUIController.Instance.FadeInOutImage(1f, 0.5f);

        yield return new WaitForSeconds(2f);
        cineChager.CameraChange();
        Vector3 teleportPosition = transformTeleport_FrontGgilick.position;    // 플레이어랑 카메라 순간이동
        Vector3 playerPosition = GameAssistManager.Instance.GetPlayer().transform.position;
        GameAssistManager.Instance.GetPlayer().transform.position = teleportPosition; // 플레이어를 순간이동


        GameAssistManager.Instance.GetPlayerScript().SetFallDownState();
        GameAssistManager.Instance.GetPlayerScript().SetCanExit(false);

        yield return new WaitForSeconds(2f);
       
        InGameUIController.Instance.FadeInOutImage(0f, 2f);

        yield return new WaitForSeconds(2f);


        InGameUIController.Instance.bIsUIDoing = false;
        GameAssistManager.Instance.GetPlayerScript().SetCanExit(true);
    }




    // 필요시 충돌 효과를 재생하는 메서드 추가
    /*
    private void PlayCollisionEffect()
    {
        // 충돌 소리 재생
        // AudioSource.PlayClipAtPoint(collisionSound, transform.position);
        
        // 파티클 효과 재생
        // Instantiate(collisionParticle, transform.position, Quaternion.identity);
    }
    */

    // 공격 범위 내에 플레이어가 있는지 확인하는 함수
    private void CheckPlayerInAttackRange()
    {
        // 끼릭과 플레이어 사이의 거리 계산 (XZ 평면만 고려)
        Vector3 ggilickPosXZ = new Vector3(ggilick.transform.position.x, 0, ggilick.transform.position.z);
        Vector3 playerPosXZ = new Vector3(playerTransform.position.x, 0, playerTransform.position.z);
        float distanceToPlayer = Vector3.Distance(ggilickPosXZ, playerPosXZ);
        
        // 끼릭의 전방 방향과 플레이어 방향이 일치하는지 확인 (XZ 평면만 고려)
        Vector3 directionToPlayer = (playerPosXZ - ggilickPosXZ).normalized;
        Vector3 forwardXZ = new Vector3(ggilick.transform.forward.x, 0, ggilick.transform.forward.z).normalized;
        float dotProduct = Vector3.Dot(forwardXZ, directionToPlayer);
        
        Debug.Log($"피격 체크: 거리={distanceToPlayer}m, 각도={dotProduct:F2}, 공격범위={attackRange}m");
        
        // 플레이어가 공격 범위 내에 있고, 끼릭의 전방에 있는 경우
        if (distanceToPlayer <= attackRange && dotProduct > 0.5f) // 0.5는 약 60도 각도 내
        {
            Debug.Log("플레이어가 공격 범위 내에 있습니다! 플레이어 사망 판정!");
            GameAssistManager.Instance.DiePlayerReset(2f, 0, 0f);
            // 여기에 플레이어 사망 처리 추가 (현재는 디버그 로그만 출력)
            // PlayerDeath();
        }
        else
        {
            Debug.Log("플레이어가 공격 범위 밖에 있습니다.");
        }
    }

}
