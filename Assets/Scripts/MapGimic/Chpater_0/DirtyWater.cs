using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtyWater : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameAssistManager.Instance.DiePlayerReset(1.2f, 0, 0f);
        }
    }
}
