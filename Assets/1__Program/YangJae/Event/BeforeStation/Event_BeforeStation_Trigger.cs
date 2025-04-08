using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_BeforeStation_Trigger : MonoBehaviour
{
    [SerializeField]
    private Event_BeforeStation_Controller eventController;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player")){
            eventController.StartEvent();
            this.gameObject.SetActive(false);
        }
    }
}