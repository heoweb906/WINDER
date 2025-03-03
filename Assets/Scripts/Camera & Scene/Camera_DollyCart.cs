using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_DollyCart : CameraObj
{
    private Transform player;  // 추적할 플레이어의 Transform
    private CinemachineVirtualCamera virtualCamera;
    private CinemachineDollyCart dollyCart;
    public DollyRotationAndPositonOffset dollyRotation;
    public GameObject FollowCart;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        dollyCart = FollowCart.GetComponent<CinemachineDollyCart>();

        virtualCamera.Follow = FollowCart.transform;
    }

    public void FixedUpdate()
    {
        if(!GameAssistManager.Instance.GetBoolPlayerDie()) RoateCameraOnDollyTrack();
    }

    private void RoateCameraOnDollyTrack()
    {
        float currentPosition = dollyCart.m_Position;
        int currentIndex = Mathf.FloorToInt(currentPosition);
        int nextIndex = currentIndex + 1;

        // 인덱스가 배열 범위를 벗어나지 않도록 체크
        if (currentIndex >= dollyRotation.Offsets.Length - 1) return;
        if (nextIndex >= dollyRotation.Offsets.Length) return;

        // 현재 위치에서의 보간 비율 계산
        float t = currentPosition - currentIndex;  // 부드러운 보간을 위해 float 사용

        // 회전 보간 (Quaternion.Slerp를 사용하여 부드럽게 회전)
        Quaternion startRotation = Quaternion.Euler(dollyRotation.Offsets[currentIndex].lookAtOffset);
        Quaternion endRotation = Quaternion.Euler(dollyRotation.Offsets[nextIndex].lookAtOffset);
        Quaternion interpolatedRotation = Quaternion.Slerp(startRotation, endRotation, t);

        // 카메라의 회전 적용
        // 기존 회전값에 더 부드럽게 적용되도록 회전값을 조금씩 보정
        virtualCamera.transform.rotation = Quaternion.Slerp(virtualCamera.transform.rotation, interpolatedRotation, Time.deltaTime * 5f); // 5f는 부드러운 속도를 위한 값입니다.
    }



}
