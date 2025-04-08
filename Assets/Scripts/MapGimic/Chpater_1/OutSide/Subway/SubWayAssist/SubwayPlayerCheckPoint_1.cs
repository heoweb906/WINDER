using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubwayPlayerCheckPoint_1 : MonoBehaviour
{
    private bool bTriiger = false;

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.root.CompareTag("Player") && !SubWayAssist.Instance.bPlayerInSubway && !bTriiger)
        {
            bTriiger = true;

            SubWayAssist.Instance.LetsStartTrain();
        }
    }
}
