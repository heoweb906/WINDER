using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCheckArea : MonoBehaviour
{
    public bool isPlayerInArea = false; // �÷��̾ ���� ���� �ִ��� ����
    public Transform playerPosition; // �÷��̾��� ��ġ�� ������ ����

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInArea = true;
            playerPosition = other.transform.root; // �÷��̾��� �ֻ��� �θ� ������Ʈ ��ġ ����
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInArea = false;
            playerPosition = null; // �÷��̾� ��ġ �ʱ�ȭ
        }
    }
}
