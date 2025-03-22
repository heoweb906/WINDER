using System.Collections;
using System.Collections.Generic;
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
        Stunned         // 유리벽 충돌 후 기절 상태
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
    
    private ChaseState currentState = ChaseState.Idle;
    private float stateTimer = 0f;
    private float currentSpeedFactor = 0f;  // 0~1 사이의 속도 계수

    private Animator animator;
    
    private void Start()
    {
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
        }
    }
    
    private void MoveGgilick()
    {
        // 이동이 필요한 상태인지 확인
        if (currentState == ChaseState.Idle || currentState == ChaseState.Stunned)
            return;
            
        // 플레이어와의 거리 계산
        float distanceToPlayer = Vector3.Distance(ggilick.transform.position, playerTransform.position);
        
        // 거리에 따른 속도 계산 (거리가 멀수록 속도 증가)
        float distanceSpeedMultiplier = Mathf.Clamp01(distanceToPlayer / distanceThreshold);
        
        // 최종 속도 계산 (상태에 따른 속도 계수 적용)
        float currentSpeed = Mathf.Lerp(baseSpeed, maxSpeed, distanceSpeedMultiplier) * currentSpeedFactor;
        
        // 목표 지점을 향해 이동
        Vector3 direction = (TargetPosition.position - ggilick.transform.position).normalized;
        ggilick.transform.position += direction * currentSpeed * Time.fixedDeltaTime;
        
        // 이동 방향으로 회전
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            float rotationSpeed = Mathf.Lerp(2f, 5f, currentSpeedFactor); // 속도에 따른 회전 속도 조정
            ggilick.transform.rotation = Quaternion.Slerp(ggilick.transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }
    
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
                currentSpeedFactor = 0f;
                animator.SetTrigger("Brake");
                // 기절 효과 추가 (필요시 애니메이션 등 추가)
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
}
