using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardM_CheckingArea : MonoBehaviour
{
    public bool isPlayerInArea = false;
    public Transform playerPosition;

    public void OnTriggerEnter(Collider other)
    {
        // 자식 콜라이더에서 Player가 들어오면 감지
        if (IsPlayer(other.transform))
        {
            isPlayerInArea = true;
            playerPosition = other.transform.root; // Player의 최상위 부모 오브젝트 위치 저장
        }
    }

    public void OnTriggerExit(Collider other)
    {
        // 자식 콜라이더에서 Player가 나가면 감지 해제
        if (IsPlayer(other.transform))
        {
            isPlayerInArea = false;
            playerPosition = null; // 플레이어 위치 초기화
        }
    }

    private bool IsPlayer(Transform obj)
    {
        // 객체가 Player인지 확인
        return obj.CompareTag("Player");
    }

    public bool IsPlayerInArea()
    {
        return isPlayerInArea;
    }

    public Transform GetPlayerPosition()
    {
        return playerPosition;
    }
}
