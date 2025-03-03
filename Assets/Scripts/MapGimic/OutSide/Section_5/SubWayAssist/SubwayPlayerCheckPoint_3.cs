using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubwayPlayerCheckPoint_3 : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.root.CompareTag("Player") && !SubWayAssist.Instance.bPlayerTeleport)
        {
            SubWayAssist.Instance.bPlayerTeleport = true;
        }
    }
}
