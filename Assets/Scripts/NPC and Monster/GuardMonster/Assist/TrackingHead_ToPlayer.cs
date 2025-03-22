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
    private bool hasRotated = false;

    void Start()
    {
        DOTween.Init(); // DOTween 초기화
        initialRotation = transform.localRotation;
    }

    void Update()
    {
        if (bFindPlayer && GameAssistManager.Instance.player != null)
        {
            Debug.Log("플레이어를 바라보는 중");

            Vector3 direction = GameAssistManager.Instance.player.transform.position - transform.position;

            // 필요하면 Y축 고정
            // direction = new Vector3(direction.x, 0, direction.z);

            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Y축 +90도 회전 추가
            targetRotation *= Quaternion.Euler(15, 90, 0);

            if (!hasRotated) // 처음 회전할 때만 부드럽게
            {
                transform.DOKill(); // DOTween 애니메이션 중지
                transform.DORotateQuaternion(targetRotation, 0.5f)
                    .SetEase(Ease.OutSine)
                    .OnComplete(() => hasRotated = true);
            }
            else
            {
                transform.rotation = targetRotation; // 이후에는 즉시 적용
            }
        }
        else
        {
            Debug.Log("두리번거리는 중");

            // 플레이어를 놓쳤다면 다시 부드러운 회전을 허용
            hasRotated = false;

            timeSinceLastRotation += Time.deltaTime;

            if (timeSinceLastRotation >= fRotateInterval)
            {
                float randomX = Random.Range(minRotate.x, maxRotate.x);
                float randomY = Random.Range(minRotate.y, maxRotate.y);
                float randomZ = Random.Range(minRotate.z, maxRotate.z);
                Quaternion randomRotation = Quaternion.Euler(randomX, randomY, randomZ);

                transform.DOKill(); // 기존 Tween 제거
                transform.DOLocalRotate(randomRotation.eulerAngles, fRotationSpeed).SetEase(Ease.InOutSine);

                timeSinceLastRotation = 0f;
            }
        }
    }

}
