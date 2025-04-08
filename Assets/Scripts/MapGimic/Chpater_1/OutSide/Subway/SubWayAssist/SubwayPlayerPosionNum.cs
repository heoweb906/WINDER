using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubwayPlayerPosionNum : MonoBehaviour
{
    public int iPositonNum;


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.CompareTag("Player"))
        {
            SubWayAssist.Instance.iCrowedRanNum = iPositonNum;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.root.CompareTag("Player"))
        {
            if(SubWayAssist.Instance.iCrowedRanNum != iPositonNum)
            {
                SubWayAssist.Instance.iCrowedRanNum = 0;
            }
        }
    }
}
