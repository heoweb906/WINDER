using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_FirstGyungsooFind : MonoBehaviour
{
    private bool bFlag = false;

    [Header("ī�޶� �̺�Ʈ")]
    public CameraEvent cameraEvent;
    public int iCameraTime;

    [Header("�ɾ�� ���")]
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
