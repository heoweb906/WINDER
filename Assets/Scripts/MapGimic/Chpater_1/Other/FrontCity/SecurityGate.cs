using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityGate : MonoBehaviour
{
    public GameObject[] screenColors;
    public GameObject objDoor;
    public float doorOpenRotationY = 90f;  // 열린 상태의 Y축 회전 값
    public float doorCloseRotationY = 0f;  // 닫힌 상태의 Y축 회전 값
    public float doorSpeed = 0.5f;         // 문이 열리고 닫히는 속도
    public float autoCloseDelay = 3f;      // 자동으로 닫히기 전 대기 시간



    private void OnTriggerEnter(Collider other)
    {
        // 부모에서 처리하도록 이벤트를 호출
        if (other.GetComponent<NPC_Simple>() != null)
        {
            // Debug.Log("NPC 입장");

            WorkDoor();
        }
    }

  


    private void ChangeColor(int iIndex) // 0 - Green, 1 - red
    {
        screenColors[0].SetActive(iIndex == 0);
        screenColors[1].SetActive(iIndex != 0);
    }


    private void WorkDoor() // 호출하면 무조건 열리고, 1초 후 자동으로 닫힘
    {
        // 기존 DOTween 애니메이션 중단
        objDoor.transform.DOKill();

        // 문 열기
        ChangeColor(0);
        objDoor.transform.DOLocalRotate(Vector3.up * doorOpenRotationY, doorSpeed) //  DOLocalRotate 사용
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                objDoor.transform.DOLocalRotate(Vector3.up * doorCloseRotationY, doorSpeed)
                .SetEase(Ease.InQuad)
                .SetDelay(autoCloseDelay)
                .OnComplete(() =>
                {
                    ChangeColor(1); // 문이 닫히는 애니메이션 완료 후 색 변경
                });
            });
    }


}
