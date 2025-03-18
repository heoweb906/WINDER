using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEvent : MonoBehaviour
{
    public GameObject camera;
    public int iEventTime;

    private bool bTrigger = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameAssistManager.Instance.ImplementCameraEvent(camera, iEventTime);


        }
    }

}
