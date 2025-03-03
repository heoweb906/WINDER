using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RoadCar : MonoBehaviour
{
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
        if(CarFrame != null) StartShakeEffect();
    }

    private void Update()
    {
        if (bMoveActive)
        {
            MoveCar(); // 자동차 이동 메서드 호출

            // positionEnd의 z값을 넘어가면 자동차 삭제
            if ((bDirection && transform.position.z < trafficLight.postions_end[0].position.z) ||
                (!bDirection && transform.position.z > trafficLight.postions_end[1].position.z))
            {
                RemoveCar();
            }
        }
    }

    private void MoveCar()
    {
        RaycastHit hit;
        Vector3 rayStart = transform.position + Vector3.up * 2; // Ray 발사 위치를 살짝 올려줌

        // 충돌이 감지되면 감속
        if (Physics.Raycast(rayStart, transform.forward, out hit, safeDistance))
        {
            if (hit.collider.GetComponent<RoadCar>() != null)
            {
                // 안전 거리 확보를 위해 서서히 감속
                currentSpeed -= deceleration * Time.deltaTime;
                currentSpeed = Mathf.Max(currentSpeed, 0);
            }
        }
        else
        {
            // 안전 거리가 확보되면 가속
            if (currentSpeed < maxSpeed)
            {
                currentSpeed += acceleration * Time.deltaTime;
            }
        }

        // MovePosition을 사용해 오브젝트 이동
        Vector3 targetPosition = transform.position + transform.forward * currentSpeed * Time.deltaTime;
        rb.MovePosition(targetPosition);

        // JustRotate 배열의 각 요소 회전 속도 업데이트
        foreach (var rotate in justRotates)
        {
            if (rotate != null)
            {
                rotate.rotationSpeed = currentSpeed * 10f; // 속도에 비례하도록 조정 (10f는 비율 조정값)
            }
        }
    }


    private void RemoveCar()
    {
        if (CarFrame != null) DOTween.Kill(CarFrame);
        DOTween.Kill(gameObject);
       

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
        Vector3 rayStart = transform.position + Vector3.up * 2;
        Gizmos.DrawLine(rayStart, rayStart + transform.forward * safeDistance);
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
