using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarShakeEffect : MonoBehaviour
{
    public Transform carFrame; // 떨림을 넣을 자동차 프레임

    private Vector3 initialPosition; // 원래 위치 저장

    private void Start()
    {
        initialPosition = carFrame.localPosition; // 원래 위치 저장
        StartShakeEffect();
    }

    // CarFrame이 떨리는 효과
    public void StartShakeEffect()
    {
        // CarFrame의 위치를 미세하게 떨리는 효과를 DOTween으로 적용
        carFrame.DOShakePosition(
            duration: 0.3f, // 떨림 지속 시간
            strength: new Vector3(0.01f, 0.01f, 0f), // 떨림 강도 (X, Y축만 떨림)
            vibrato: 2, // 떨림 빈도
            randomness: 90f, // 랜덤성
            fadeOut: false // 효과가 계속 유지되도록 설정
        ).SetLoops(-1, LoopType.Yoyo) // 무한 반복 (Yoyo로 하면 부드럽게 흔들림)
         .SetEase(Ease.Linear)
         .SetRelative(true) // 로컬 위치 기준으로 떨림 적용
         .OnKill(() => carFrame.localPosition = initialPosition); // Tween 종료 시 원래 위치 복구
    }

    // 떨림 효과 중지
    public void StopShakeEffect()
    {
        carFrame.DOKill(); // DOTween 애니메이션 중지
        carFrame.localPosition = initialPosition; // 원래 위치로 복구
    }
}
