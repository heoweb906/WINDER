using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCheckArea : MonoBehaviour
{
    public bool isPlayerInArea = false; // 플레이어가 범위 내에 있는지 여부
    public Transform playerPosition; // 플레이어의 위치를 저장할 변수

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInArea = true;
            playerPosition = other.transform.root; // 플레이어의 최상위 부모 오브젝트 위치 저장
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInArea = false;
            playerPosition = null; // 플레이어 위치 초기화
        }
    }
}
