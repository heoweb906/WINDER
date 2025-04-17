using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RoadCar : MonoBehaviour
{
    public int iCarType;

    [HideInInspector] public TrafficLight trafficLight; // 교통 신호
    public bool bMoveActive;
    public bool bDirection;

    public float maxSpeed; // 최고 속력
    public float safeDistance; // 안전거리
    public float deceleration; // 감속 비율
    public float acceleration; // 가속 비율

    private Rigidbody rb; // 자동차의 Rigidbody 컴포넌트
    private float currentSpeed; // 현재 속력

    public JustRotate[] justRotates;  // 타이어들 회전 관리
    public GameObject CarFrame;



    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        currentSpeed = maxSpeed; // 초기 속력을 최고 속력으로 설정
        if (CarFrame != null) StartShakeEffect();
    }



    private void FixedUpdate()
    {
        if (bMoveActive)
        {
            MoveCar(); // 자동차 이동 메서드 호출

            // positionEnd의 z값을 넘어가면 자동차 삭제
            if ((bDirection && transform.position.x > trafficLight.postions_end[0].position.x) ||
               (!bDirection && transform.position.x < trafficLight.postions_end[1].position.x))
            {
                RemoveCar();
            }
        }
    }



    private void MoveCar()
    {
        bool isCarAhead = false;

        // 첫 번째 Ray
        Vector3 rayStart1 = transform.position + Vector3.up * 0.5f;
        if (Physics.Raycast(rayStart1, transform.forward, out RaycastHit hit1, safeDistance))
        {
            if (hit1.collider.GetComponent<RoadCar>() != null)
            {
                isCarAhead = true;
            }
        }

        // 두 번째 Ray
        Vector3 rayStart2 = transform.position + Vector3.up * 2f;
        if (Physics.Raycast(rayStart2, transform.forward, out RaycastHit hit2, safeDistance))
        {
            if (hit2.collider.GetComponent<RoadCar>() != null)
            {
                isCarAhead = true;
            }
        }

        // 감속 또는 가속
        if (isCarAhead)
        {
            currentSpeed -= deceleration * Time.fixedDeltaTime;
            currentSpeed = Mathf.Max(currentSpeed, 0);
        }
        else
        {
            if (currentSpeed < maxSpeed)
            {
                currentSpeed += acceleration * Time.fixedDeltaTime;
            }
        }

        // 이동 처리
        Vector3 targetPosition = transform.position + transform.forward * currentSpeed * Time.fixedDeltaTime;
        rb.MovePosition(targetPosition);

        // 바퀴 회전 처리
        foreach (var rotate in justRotates)
        {
            if (rotate != null)
            {
                rotate.rotationSpeed = currentSpeed * 10f;
            }
        }
    }


    private void RemoveCar()
    {
        if (CarFrame != null) DOTween.Kill(CarFrame);
        DOTween.Kill(gameObject);
        DOTween.Kill(CarFrame.transform);

        // 리스트에서 이 자동차 제거
        if (bDirection)
        {
            if (trafficLight != null && trafficLight.spawnedCars_1.Contains(gameObject)) trafficLight.spawnedCars_1.Remove(gameObject);
        }
        else
        {
            if (trafficLight != null && trafficLight.spawnedCars_2.Contains(gameObject)) trafficLight.spawnedCars_2.Remove(gameObject);
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        // 첫 번째 Ray (0.5f 높이)
        Vector3 rayStart1 = transform.position + Vector3.up * 0.5f;
        Gizmos.DrawLine(rayStart1, rayStart1 + transform.forward * safeDistance);

        // 두 번째 Ray (1.5f 높이)
        Vector3 rayStart2 = transform.position + Vector3.up * 2f;
        Gizmos.DrawLine(rayStart2, rayStart2 + transform.forward * safeDistance);
    }


    private void OnCollisionEnter(Collision collision)
    {
        // 충돌한 오브젝트의 태그가 "Player"인지 확인
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Detect On Player");


            GameAssistManager.Instance.DiePlayerReset(2f, 0);
        }
    }


    public void StartShakeEffect()
    {
        // CarFrame의 위치를 미세하게 떨리는 효과를 DOTween으로 적용
        CarFrame.transform.DOShakePosition(
            duration: 0.3f,
            strength: new Vector3(0.01f, 0.01f, 0f),
            vibrato: 2,
            randomness: 90f,
            fadeOut: false
        )
        .SetLoops(-1, LoopType.Restart)
        .SetEase(Ease.Linear)
        .SetRelative(true)
        .SetUpdate(UpdateType.Fixed)
        .SetAutoKill(true); // 자동으로 Kill
    }


}