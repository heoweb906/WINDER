using UnityEngine;

public class HandheldCamera : MonoBehaviour
{
    public float rotationAmount = 1f; // 흔들림 크기
    public float rotationSpeed = 1f; // 흔들림 속도

    private Quaternion baseRotation; // 원래 카메라 회전값
    private CameraObj currentCamera; // 현재 활성화된 카메라

    private void Start()
    {
        // 현재 활성화된 카메라 찾기
        currentCamera = GetComponent<CameraObj>();
        //if (currentCamera == null)
        //{
        //    currentCamera = GetComponent<Camera_MaxMinFollow>();
        //}

        // 기본 회전값 설정
        if (currentCamera != null)
        {
            baseRotation = Quaternion.Euler(currentCamera.rotationOffset);
        }
        else
        {
            baseRotation = transform.rotation;
        }
    }

    private void FixedUpdate()
    {
        if (currentCamera == null) return;

        // Perlin Noise를 사용해 흔들림 값 계산
        float xRotation = (Mathf.PerlinNoise(Time.time * rotationSpeed, 0) - 0.5f) * 2 * rotationAmount;
        float yRotation = (Mathf.PerlinNoise(0, Time.time * rotationSpeed) - 0.5f) * 2 * rotationAmount;

        // 기존 회전에 흔들림 효과 추가
        Quaternion shakeRotation = Quaternion.Euler(xRotation, yRotation, 0);
        transform.rotation = baseRotation * shakeRotation;
    }
}
