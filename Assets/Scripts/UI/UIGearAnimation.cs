using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGearAnimation : MonoBehaviour
{
    public enum RotationAxis
    {
        X,
        Y,
        Z
    }

    [Header("Rotation Settings")]
    public RotationAxis axis = RotationAxis.Y; // �⺻ ��: Y��
    public float rotationSpeed;          // ȸ�� �ӵ�
    public bool clockwise = true;             // �ð� ���� ����

    void Update()
    {
        float direction = clockwise ? 1f : -1f;
        Vector3 rotationVector = Vector3.zero;

        switch (axis)
        {
            case RotationAxis.X:
                rotationVector = Vector3.right;
                break;
            case RotationAxis.Y:
                rotationVector = Vector3.up;
                break;
            case RotationAxis.Z:
                rotationVector = Vector3.forward;
                break;
        }

        transform.Rotate(rotationVector * rotationSpeed * direction * Time.unscaledDeltaTime);
    }
}
