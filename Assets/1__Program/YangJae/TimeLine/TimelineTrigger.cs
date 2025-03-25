using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineTrigger : MonoBehaviour
{
    [SerializeField]
    private TimelineManager timelineManager;

    [SerializeField]
    private int timelineID;



    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            timelineManager.PlayTimeline(timelineID);
            gameObject.SetActive(false);
        }
    }
}
