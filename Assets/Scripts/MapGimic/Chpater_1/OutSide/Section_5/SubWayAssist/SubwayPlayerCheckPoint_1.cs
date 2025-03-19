using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubwayPlayerCheckPoint_1 : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.root.CompareTag("Player") && !SubWayAssist.Instance.bPlayerInSubway)
        {
            SubWayAssist.Instance.LetsStartTrain();
        }
    }
}
