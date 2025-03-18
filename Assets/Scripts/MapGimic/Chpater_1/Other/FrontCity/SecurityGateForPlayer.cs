using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityGateForPlayer : MonoBehaviour
{
    public GameObject[] screenColors;
    public GameObject objDoor;
    public float doorOpenRotationY = 90f;  // 열린 상태의 Y축 회전 값
    public float doorCloseRotationY = 0f;  // 닫힌 상태의 Y축 회전 값
    public float autoCloseDelay = 3f;      // 자동으로 닫히기 전 대기 시간


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        { 
            Debug.Log("감지 성공!!");

            DoorClose();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("감지 성공!!");

            DoorOpen();
        }
    }



    private void ChangeColor(int iIndex) // 0 - Green (닫힘), 1 - Red (열림)
    {
        screenColors[0].SetActive(iIndex == 0);  // 초록색 (닫힘)
        screenColors[1].SetActive(iIndex != 0);  // 빨간색 (열림)
    }

    private void DoorClose() // 호출 시 문이 닫히고, 1초 후 자동으로 열림
    {
        // 기존 DOTween 애니메이션 중단
        objDoor.transform.DOKill();

     
        ChangeColor(1);  
        objDoor.transform.DOLocalRotate(Vector3.up * doorCloseRotationY, 0.2f) 
            .SetEase(Ease.OutQuad);
    }

    private void DoorOpen()
    {
        objDoor.transform.DOLocalRotate(Vector3.up * doorOpenRotationY, 0.5f) // 열기 애니메이션
                   .SetEase(Ease.InQuad)
                   .SetDelay(autoCloseDelay)  // 1초 딜레이
                   .OnComplete(() =>
                   {
                       ChangeColor(0);  // 문 열림 후 색상 변경 (빨간색)
                   });
    }

}
