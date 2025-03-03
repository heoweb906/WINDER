using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainDoor : MonoBehaviour
{
    public GameObject leftDoor;
    public GameObject rightDoor;

    public Transform position_target_LeftDoor;
    public Transform position_target_RightDoor;

    public float doorMoveDuration;            // 문이 열리는/닫히는 데 걸리는 시간

    private Vector3 originalPosition_LeftDoor;     // 왼쪽 문의 원래 위치
    private Vector3 originalPosition_RightDoor;    // 오른쪽 문의 원래 위치

    private bool bInPlayer;
    


    public void StartOpen_Close(float doorTime)
    {
        StartCoroutine(OpenAndCloseDoors(doorTime));
    }
    // 문을 열고 닫는 동작을 수행하는 코루틴
    private IEnumerator OpenAndCloseDoors(float doorStayOpenDuration)
    {
        // 1. 문을 목표 위치로 이동 (문 열기)
        originalPosition_LeftDoor = leftDoor.transform.position;
        originalPosition_RightDoor = rightDoor.transform.position;


        leftDoor.transform.DOMove(position_target_LeftDoor.position, doorMoveDuration).SetEase(Ease.InOutCubic);
        rightDoor.transform.DOMove(position_target_RightDoor.position, doorMoveDuration).SetEase(Ease.InOutCubic);

        // 2. 정거장 체류 시간보다 조금 적게 문이 열려 있음
        yield return new WaitForSeconds(doorStayOpenDuration - doorMoveDuration);

        // 3. 문을 원래 위치로 이동 (문 닫기)
        if(bInPlayer)
        {
            // 나중에 플레이어 조작 방지 추가
            // 나중에 플레이어 조작 방지 추가
            // 나중에 플레이어 조작 방지 추가
            // 나중에 플레이어 조작 방지 추가
        }
          


        leftDoor.transform.DOMove(originalPosition_LeftDoor, doorMoveDuration).SetEase(Ease.InOutCubic);
        rightDoor.transform.DOMove(originalPosition_RightDoor, doorMoveDuration).SetEase(Ease.InOutCubic);


    }

    public void StartOpen()
    {
        // 1. 문을 목표 위치로 이동 (문 열기)
        originalPosition_LeftDoor = leftDoor.transform.position;
        originalPosition_RightDoor = rightDoor.transform.position;

        if (bInPlayer)
        {
            // 나중에 플레이어 조작 가능 추가
            // 나중에 플레이어 조작 가능 추가
            // 나중에 플레이어 조작 가능 추가
            // 나중에 플레이어 조작 가능 추가
            // 나중에 플레이어 조작 가능 추가
        }

        leftDoor.transform.DOMove(position_target_LeftDoor.position, doorMoveDuration).SetEase(Ease.InOutCubic);
        rightDoor.transform.DOMove(position_target_RightDoor.position, doorMoveDuration).SetEase(Ease.InOutCubic);
    }





    private void OnTriggerStay(Collider other)
    {
        if (other.transform.root.CompareTag("Player"))
        {
            bInPlayer = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.root.CompareTag("Player"))
        {
            bInPlayer = false;
        }
    }

}
