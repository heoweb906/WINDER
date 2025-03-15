using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotLightRatate : MonoBehaviour
{
    public Vector3 vecStartRotate;
    public float rotationAngle; // 좌우로 회전할 각도
    public float duration; // 한쪽으로 회전하는 시간
    

    void Start()
    {
        RotateLight();
    }

    void RotateLight()
    {
        float startY = transform.rotation.eulerAngles.y;

        // Y축만 회전하도록 설정
        transform.DORotate(new Vector3(vecStartRotate.x, startY + rotationAngle, vecStartRotate.z), duration)
            .SetEase(Ease.InOutSine) // 부드럽게 이동
            .SetLoops(-1, LoopType.Yoyo); // 무한 반복 (왔다 갔다)
    }
}
