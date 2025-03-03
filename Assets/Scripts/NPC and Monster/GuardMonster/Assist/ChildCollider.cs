using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildCollider : MonoBehaviour
{


    private void OnTriggerEnter(Collider other)
    {
        // 부모에서 처리하도록 이벤트를 호출
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
        // 부모에서 처리하도록 이벤트를 호출
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
