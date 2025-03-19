using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_ObjectFollow : CameraObj
{
    public Transform target;  // ������ �÷��̾��� Transform
    public Vector3 offset;    // ī�޶�� �÷��̾� ���� ������
    public Vector3 rotationOffset; // ī�޶��� ȸ�� ������
    public float smoothSpeed;  // ī�޶� �̵� �ӵ�

    void FixedUpdate()
    {
        if (!GameAssistManager.Instance.GetBoolPlayerDie())
        {
            // �÷��̾��� ��ġ�� �������� ���� ��ǥ ��ġ
            Vector3 targetPosition = target.position + offset;

            // �ε巴�� ī�޶� ��ǥ ��ġ�� �̵�
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
            transform.position = smoothedPosition;

            // ī�޶��� ȸ�� ����
            Quaternion targetRotation = Quaternion.Euler(rotationOffset);
            transform.rotation = targetRotation;
        }
    }
    // -5.65   3.41    
    // 17.6   90
    // 0.125

}
