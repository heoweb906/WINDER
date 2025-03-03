using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DollyCartControll : MonoBehaviour
{
    private Transform player; // 플레이어의 Transform
    public CinemachineDollyCart dollyCart; // Cinemachine Dolly Cart
    public CinemachineSmoothPath dollyPath; // Cinemachine Path
    public DollyRotationAndPositonOffset dollyRotation;

    // 카트가 플레이어 위치에 맞춰 이동하는 속도 조정
    public float followSpeed;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    void FixedUpdate()
    {
        if (player != null && dollyPath != null)
        {
            // 플레이어 위치에서 가장 가까운 지점을 찾음
            float closestPoint = dollyPath.FindClosestPoint(player.position, 0, -1, 10);
            float targetPosition = Mathf.Clamp(closestPoint, 0, dollyPath.PathLength);

            // 현재 인덱스와 다음 인덱스 계산
            int currentIndex = Mathf.FloorToInt(targetPosition);
            int nextIndex = currentIndex + 1;

            // 인덱스가 배열 범위를 벗어나는 경우, 마지막 인덱스를 사용
            if (currentIndex >= dollyRotation.Offsets.Length - 1)
            {
                currentIndex = dollyRotation.Offsets.Length - 1;
                nextIndex = currentIndex;
            }
            else if (nextIndex >= dollyRotation.Offsets.Length)
            {
                nextIndex = dollyRotation.Offsets.Length - 1;
            }

            // 보간 비율 계산
            float t = targetPosition - currentIndex;

            // fPositionOffset 보간
            float startOffset = dollyRotation.Offsets[currentIndex].fPositionOffest;
            float endOffset = dollyRotation.Offsets[nextIndex].fPositionOffest;
            float interpolatedOffset = Mathf.Lerp(startOffset, endOffset, t);

            // 보정된 목표 위치 계산
            float adjustedTargetPosition = Mathf.Clamp(targetPosition + interpolatedOffset, 0, dollyPath.PathLength);

            // 현재 위치가 목표 위치와 다를 경우에만 이동
            if (Mathf.Abs(dollyCart.m_Position - adjustedTargetPosition) > 0.01f)
            {
                DOTween.To(() => dollyCart.m_Position, x => dollyCart.m_Position = x, adjustedTargetPosition, followSpeed);
            }
        }
    }
}
