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
    public int iEventTime;          // 이벤트 시간
    public float fReturnTime;       // 돌아갈 때 카메라 보간 속도, // 연출용 카메라 보간 속도는 카메라 자체에 설정해놔야 함


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


    // #. 트리거로 실행
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

    // #. 함수로 실행
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
