using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TrackingHead_ToPlayer : MonoBehaviour
{
    // public GameObject Parent; // 부모 오브젝트
    public bool bFindPlayer;

    public Vector3 maxRotate; // 최대 회전값
    public Vector3 minRotate; // 최소 회전값
    public float fRotationSpeed = 30f; // 회전 속도 (초당 각속도)
    public float fRotateInterval = 2f; // 회전 갱신 주기

    private float timeSinceLastRotation = 0f;
    private Quaternion initialRotation; // 초기 로테이션 값 저장

    void Start()
    {
        DOTween.Init(); // DOTween 초기화
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

            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, fRotationSpeed * Time.deltaTime);
        }
        else
        {
            timeSinceLastRotation += Time.deltaTime;

            if (timeSinceLastRotation >= fRotateInterval)
            {
                float randomY = Random.Range(minRotate.y, maxRotate.y);
                float randomZ = Random.Range(minRotate.z, maxRotate.z);
                Quaternion randomRotation = Quaternion.Euler(0f, randomY, randomZ);

                transform.DOKill(); // 기존 Tween 제거
                transform.DOLocalRotate(randomRotation.eulerAngles, fRotationSpeed).SetEase(Ease.InOutSine);

                timeSinceLastRotation = 0f;
            }
        }
    }

}
