using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GGILICK_Car : MonoBehaviour
{
    [Header("자동차 떨림 애니메이션 관련")]
    public GameObject CarFrame;
    public GameObject TaeYub;

    [Header("자동차 움직임 관련")]
    public float fCarSpeed; // 최고 속력
    public Transform transform_Destroy;
    private Rigidbody rb; // 자동차의 Rigidbody 컴포넌트


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartShakeEffect(); // CarFrame의 떨림 효과 시작
    }

    private void Update()
    {
        MoveCar();

        if (transform.position.z < transform_Destroy.position.z) Destroy(gameObject); ;
    }

    private void FixedUpdate()
    {
        RotateTaeYub(); // TaeYub의 회전 효과 적용
    }


    // CarFrame이 떨리는 효과
    private void StartShakeEffect()
    {
        // CarFrame의 위치를 미세하게 떨리는 효과를 DOTween으로 적용
        CarFrame.transform.DOShakePosition(
            duration: 0.1f, // 떨림 지속 시간
            strength: new Vector3(0.02f, 0.02f, 0f), // 떨림 강도 (X, Y축만 떨림)
            vibrato: 10, // 떨림 빈도
            randomness: 90f, // 랜덤성
            fadeOut: false // 효과가 계속 유지되도록 설정
        ).SetLoops(-1, LoopType.Restart) // 무한 반복
         .SetEase(Ease.Linear)
         .SetRelative(true) // 로컬 위치 기준으로 떨림 적용
         .SetUpdate(UpdateType.Fixed); // FixedUpdate에서 실행되도록 설정
    }

    // TaeYub의 Z축 회전 효과
    private void RotateTaeYub()
    {
        // Z축으로 회전 (Time.deltaTime을 사용해 프레임 독립적 회전)
        TaeYub.transform.Rotate(0f, 0f, 100f * Time.deltaTime);
    }


    private void MoveCar()
    {
        // 자동차를 Z축 방향으로 이동 (fCarSpeed 속도로)
        transform.Translate(Vector3.back * fCarSpeed * Time.deltaTime);
    }



}
