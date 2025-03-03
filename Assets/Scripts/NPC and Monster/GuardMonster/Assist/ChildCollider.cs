using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildCollider : MonoBehaviour
{


    private void OnTriggerEnter(Collider other)
    {
        // �θ𿡼� ó���ϵ��� �̺�Ʈ�� ȣ��
        if (other.CompareTag("Player"))
        {
            var parentScript = GetComponentInParent<GuardM_CheckingArea>();
            if (parentScript != null)
            {
                parentScript.OnTriggerEnter(other);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // �θ𿡼� ó���ϵ��� �̺�Ʈ�� ȣ��
        if (other.CompareTag("Player"))
        {
            var parentScript = GetComponentInParent<GuardM_CheckingArea>();
            if (parentScript != null)
            {
                parentScript.OnTriggerExit(other);
            }
        }
    }
}
