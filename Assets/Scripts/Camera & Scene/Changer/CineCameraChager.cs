using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CineCameraChager : MonoBehaviour
{
    public bool bNotChangeSpawnPoint;           // 스폰 포인트를 변경하지 않으면 true
    public bool bTriggerOff;

    public GameObject TargetCamera;
    public Transform TartgetTransform;

    private Camera mainCamera;
    private CinemachineBrain cineBrain;



    private void Awake()
    {
        mainCamera = Camera.main;

        if (mainCamera != null)
        {
            Transform parentTransform = mainCamera.transform;

            // 부모 Transform을 따라가며 Camera 컴포넌트를 가진 최고 부모를 찾습니다.
            while (parentTransform.parent != null)
            {
                parentTransform = parentTransform.parent;

                // 최고 부모가 Camera 컴포넌트를 가지고 있는지 확인합니다.
                if (parentTransform.GetComponent<Camera>() != null)
                {
                    mainCamera = parentTransform.GetComponent<Camera>();
                }
            }

            // 최종적으로 최고 부모 카메라의 CinemachineBrain을 찾습니다.
            cineBrain = mainCamera.GetComponent<CinemachineBrain>();
            if (cineBrain == null)
            {
                Debug.LogWarning("CinemachineBrain component not found on the root camera.");
            }
            else
            {
                Debug.Log("Found root camera: " + mainCamera.name);
            }
        }
        else
        {
            Debug.LogError("No main camera found.");
        }
    }


    // #. 다른 스크립트에서 작동하는 용
    public void CameraChange()
    {
        BlendChanger(TargetCamera);
        if(TartgetTransform!= null)
        {
            GameAssistManager.Instance.RespawnChangeAssist(TartgetTransform);
        }
           
    }


  


       
    // #. CinemachineBrain - 버츄얼 카메라 전환시 값 불러와서 적용
    private void BlendChanger(GameObject targetCamera)
    {
        if (GameAssistManager.Instance.BoolNowActiveCameraObj(targetCamera)) return;

        GameAssistManager.Instance.CameraChangeAssist(targetCamera);
        Debug.Log($"카메라 전환 - 호출한 오브젝트: {gameObject.name}");

        CameraObj camObj = targetCamera.GetComponent<CameraObj>();
        cineBrain.m_DefaultBlend = new CinemachineBlendDefinition(camObj.blendStyle, camObj.duration);
    }



    // #. 트리거로 작동하는 방식
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.root.CompareTag("Player") && !bTriggerOff)
        {
            BlendChanger(TargetCamera);
            GameAssistManager.Instance.RespawnChangeAssist(TartgetTransform);
        }
    }


}
