using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityGateForPlayer : MonoBehaviour
{
    public GameObject[] screenColors;
    public GameObject objDoor;
    public float doorOpenRotationY = 90f;  // ���� ������ Y�� ȸ�� ��
    public float doorCloseRotationY = 0f;  // ���� ������ Y�� ȸ�� ��
    public float autoCloseDelay = 3f;      // �ڵ����� ������ �� ��� �ð�


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        { 
            Debug.Log("���� ����!!");

            DoorClose();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("���� ����!!");

            DoorOpen();
        }
    }



    private void ChangeColor(int iIndex) // 0 - Green (����), 1 - Red (����)
    {
        screenColors[0].SetActive(iIndex == 0);  // �ʷϻ� (����)
        screenColors[1].SetActive(iIndex != 0);  // ������ (����)
    }

    private void DoorClose() // ȣ�� �� ���� ������, 1�� �� �ڵ����� ����
    {
        // ���� DOTween �ִϸ��̼� �ߴ�
        objDoor.transform.DOKill();

     
        ChangeColor(1);  
        objDoor.transform.DOLocalRotate(Vector3.up * doorCloseRotationY, 0.2f) 
            .SetEase(Ease.OutQuad);
    }

    private void DoorOpen()
    {
        objDoor.transform.DOLocalRotate(Vector3.up * doorOpenRotationY, 0.5f) // ���� �ִϸ��̼�
                   .SetEase(Ease.InQuad)
                   .SetDelay(autoCloseDelay)  // 1�� ������
                   .OnComplete(() =>
                   {
                       ChangeColor(0);  // �� ���� �� ���� ���� (������)
                   });
    }

}
