using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotLightRatate : MonoBehaviour
{
    public Vector3 vecStartRotate;
    public float rotationAngle; // �¿�� ȸ���� ����
    public float duration; // �������� ȸ���ϴ� �ð�
    

    void Start()
    {
        RotateLight();
    }

    void RotateLight()
    {
        float startY = transform.rotation.eulerAngles.y;

        // Y�ุ ȸ���ϵ��� ����
        transform.DORotate(new Vector3(vecStartRotate.x, startY + rotationAngle, vecStartRotate.z), duration)
            .SetEase(Ease.InOutSine) // �ε巴�� �̵�
            .SetLoops(-1, LoopType.Yoyo); // ���� �ݺ� (�Դ� ����)
    }
}
