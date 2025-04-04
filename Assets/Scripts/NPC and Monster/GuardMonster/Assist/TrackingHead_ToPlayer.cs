using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TrackingHead_ToPlayer : MonoBehaviour
{
    public GameObject Parent; // 부모 오브젝트
    public bool bFindPlayer;


    public Vector3 maxRotate; // 최대 회전값
    public Vector3 minRotate; // 최소 회전값
    public float fRotationSpeed; // 회전 속도 (1초에 한번 회전)
    public float fRotateInterval;
    private float timeSinceLastRotation = 0f;

    private Quaternion initialRotation; // 초기 로테이션 값 저장

    void Start()
    {
        // 시작할 때 자기 자신의 로컬 초기 회전값 저장
        initialRotation = transform.localRotation;
    }

    void Update()
    {
        if (bFindPlayer && GameAssistManager.Instance.player != null)
        {
            Vector3 direction = GameAssistManager.Instance.player.transform.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            Vector3 targetEulerAngles = targetRotation.eulerAngles;
            targetEulerAngles.x += -35f;  
            targetEulerAngles.y += 90f;  
            targetEulerAngles.z += 10f;  
            targetRotation = Quaternion.Euler(targetEulerAngles);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, fRotationSpeed);

        }
        else
        {
            // 플레이어를 찾지 못했을 때, 랜덤 회전값 적용
            timeSinceLastRotation += Time.deltaTime;

            if (timeSinceLastRotation >= fRotateInterval) // 지정한 간격마다 랜덤 회전값 갱신
            {
                // 최소값과 최대값 사이에서 랜덤한 로컬 회전값 계산
                float randomY = Random.Range(minRotate.y, maxRotate.y);
                float randomZ = Random.Range(minRotate.z, maxRotate.z);

                // 초기 로테이션 기준으로 랜덤 회전값 계산
                Quaternion randomRotation = initialRotation * Quaternion.Euler(0f, randomY, randomZ);

                // DOTween을 사용하여 부드럽게 로테이션 처리
                transform.DOLocalRotateQuaternion(randomRotation, fRotationSpeed).SetEase(Ease.InOutSine);

                // 타이머 초기화
                timeSinceLastRotation = 0f;
            }
        }
    }

}
