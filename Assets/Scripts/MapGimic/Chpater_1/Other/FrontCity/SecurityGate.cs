using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityGate : MonoBehaviour
{
    public GameObject[] screenColors;
    public GameObject objDoor;
    public float doorOpenRotationY = 90f;  // ���� ������ Y�� ȸ�� ��
    public float doorCloseRotationY = 0f;  // ���� ������ Y�� ȸ�� ��
    public float doorSpeed = 0.5f;         // ���� ������ ������ �ӵ�
    public float autoCloseDelay = 3f;      // �ڵ����� ������ �� ��� �ð�



    private void OnTriggerEnter(Collider other)
    {
        // �θ𿡼� ó���ϵ��� �̺�Ʈ�� ȣ��
        if (other.GetComponent<NPC_Simple>() != null)
        {
            // Debug.Log("NPC ����");

            WorkDoor();
        }
    }

  


    private void ChangeColor(int iIndex) // 0 - Green, 1 - red
    {
        screenColors[0].SetActive(iIndex == 0);
        screenColors[1].SetActive(iIndex != 0);
    }


    private void WorkDoor() // ȣ���ϸ� ������ ������, 1�� �� �ڵ����� ����
    {
        // ���� DOTween �ִϸ��̼� �ߴ�
        objDoor.transform.DOKill();

        // �� ����
        ChangeColor(0);
        objDoor.transform.DOLocalRotate(Vector3.up * doorOpenRotationY, doorSpeed) //  DOLocalRotate ���
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                objDoor.transform.DOLocalRotate(Vector3.up * doorCloseRotationY, doorSpeed)
                .SetEase(Ease.InQuad)
                .SetDelay(autoCloseDelay)
                .OnComplete(() =>
                {
                    ChangeColor(1); // ���� ������ �ִϸ��̼� �Ϸ� �� �� ����
                });
            });
    }


}
