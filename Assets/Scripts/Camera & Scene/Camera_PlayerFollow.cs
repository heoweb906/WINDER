using Cinemachine;
using UnityEngine;

public class Camera_PlayerFollow : CameraObj
{
    private Transform player;  // 추적할 플레이어의 Transform
    public Vector3 offset;    // 카메라와 플레이어 간의 오프셋
    public Vector3 rotationOffset; // 카메라의 회전 오프셋
    public float smoothSpeed;  // 카메라 이동 속도

    
    private void Start()
    {
        player = GameAssistManager.Instance.player.transform;
        //GameObject.FindWithTag("Player").transform;
    }

    void FixedUpdate()
    {
        if (!GameAssistManager.Instance.GetBoolPlayerDie())
        {
            // 플레이어의 위치에 오프셋을 더한 목표 위치
            Vector3 targetPosition = player.position + offset;

            // 부드럽게 카메라를 목표 위치로 이동
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
            transform.position = smoothedPosition;

            // 카메라의 회전 설정
            Quaternion targetRotation = Quaternion.Euler(rotationOffset);
            transform.rotation = targetRotation;
        }
    }
}
