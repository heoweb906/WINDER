using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarShakeEffect : MonoBehaviour
{
    public GameObject CarFrame; // 떨림을 넣을 자동차 프레임
    private Rigidbody rb;       

    private void Start()
    {
        rb = GetComponent<Rigidbody>(); 
        // StartShakeEffect(); 
    }

 

    // CarFrame이 떨리는 효과
    public void StartShakeEffect()
    {
        // CarFrame의 위치를 미세하게 떨리는 효과를 DOTween으로 적용
        CarFrame.transform.DOShakePosition(
            duration: 0.3f, // 떨림 지속 시간
            strength: new Vector3(0.01f, 0.01f, 0f), // 떨림 강도 (X, Y축만 떨림)
            vibrato: 2, // 떨림 빈도
            randomness: 90f, // 랜덤성
            fadeOut: false // 효과가 계속 유지되도록 설정
        ).SetLoops(-1, LoopType.Restart) // 무한 반복
         .SetEase(Ease.Linear)
         .SetRelative(true) // 로컬 위치 기준으로 떨림 적용
         .SetUpdate(UpdateType.Fixed); // FixedUpdate에서 실행되도록 설정
    }

}
