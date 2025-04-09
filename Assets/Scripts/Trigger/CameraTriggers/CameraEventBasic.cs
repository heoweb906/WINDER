using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEvent : MonoBehaviour
{
    private Coroutine nowCoroutine;

    public Camera mainCamera;
    public CinemachineBrain cineBrain;
    public CameraObj camObj;
    private bool bTrigger = false;

    public GameObject camera;
    public int iEventTime;          // �̺�Ʈ �ð�
    public float fReturnTime;       // ���ư� �� ī�޶� ���� �ӵ�, // ����� ī�޶� ���� �ӵ��� ī�޶� ��ü�� �����س��� ��


    private void Awake()
    {
        mainCamera = Camera.main;

        if (mainCamera != null)
        {
            Transform parentTransform = mainCamera.transform;
            while (parentTransform.parent != null)
            {
                parentTransform = parentTransform.parent;
                if (parentTransform.GetComponent<Camera>() != null)
                {
                    mainCamera = parentTransform.GetComponent<Camera>();
                }
            }
            cineBrain = mainCamera.GetComponent<CinemachineBrain>();
            camObj = camera.GetComponent<CameraObj>();
        }

    }


    // #. Ʈ���ŷ� ����
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !bTrigger)
        {
            bTrigger = true;

            cineBrain.m_DefaultBlend = new CinemachineBlendDefinition(camObj.blendStyle, camObj.duration);
            GameAssistManager.Instance.ImplementCameraEvent(camera, iEventTime);
            nowCoroutine = StartCoroutine(StartDirection(iEventTime));
        }
    }

    // #. �Լ��� ����
    public void CameraTriggerStart(int iEEEventTime)
    {
        cineBrain.m_DefaultBlend = new CinemachineBlendDefinition(camObj.blendStyle, camObj.duration);
        GameAssistManager.Instance.ImplementCameraEvent(camera, iEEEventTime);
        nowCoroutine = StartCoroutine(StartDirection(iEventTime));
    }


    IEnumerator StartDirection(int iEventTime)
    {
        GameAssistManager.Instance.PlayerInputLockOn();

        yield return new WaitForSeconds(1.0f);


        while (iEventTime > 0)
        {





            yield return new WaitForSeconds(1.0f);
            iEventTime -= 1;
        }

        GameAssistManager.Instance.PlayerInputLockOff();
        cineBrain.m_DefaultBlend = new CinemachineBlendDefinition(camObj.blendStyle, fReturnTime);
        StopCoroutine(nowCoroutine);
    }




    


}
