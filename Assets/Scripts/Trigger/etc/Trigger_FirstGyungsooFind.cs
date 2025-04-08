using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_FirstGyungsooFind : MonoBehaviour
{
    private bool bFlag = false;

    [Header("카메라 이벤트")]
    public CameraEvent cameraEvent;
    public int iCameraTime;

    [Header("걸어가는 경수")]
    public GyungSooWalkStartTrigger gyungSooWalkStartTrigger;



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !bFlag)
        {
            bFlag = true;
            cameraEvent.CameraTriggerStart(iCameraTime);
            gyungSooWalkStartTrigger.CutScencStart();
        }
    }

    

}
