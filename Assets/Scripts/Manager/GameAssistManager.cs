using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Unity.VisualScripting;

public class GameAssistManager : MonoBehaviour
{
    public static GameAssistManager Instance { get; private set; }

    public GameObject player;
    public bool bCantInpuFunc;

    [Header("스테이지 / 리스폰 관련")]
    public int iStageNum;
    public Transform[] Transforms_Respawn;
    public GameObject[] Cameras;
    private bool bPlayerDie;   // 현재 플레이어가 죽음 상태 -> 죽음 반복 방지

    [Header("연출 관련 내면 세계 진입")]
    public GameObject CameraOverlay;
    public Volume volume_1;


    private void Awake()
    {
        Instance = this; // 인스턴스 생성
        bPlayerDie = false;
       
        player = FindPlayerRoot();

        // #. 스테이지 관리
        SaveData_Manager.Instance.SetStringSceneName(SceneManager.GetActiveScene().name);

        if(SaveData_Manager.Instance.GetIntClearStageNum() < iStageNum)
        {
            SaveData_Manager.Instance.SetIntClearStageNum(iStageNum);
            SaveGameProgress(0, 0);
        }
        PlayerStartSeeting(SaveData_Manager.Instance.GetIntTransformRespawn(), SaveData_Manager.Instance.GetIntCameraNum());

    }




    // #. 갱신해야 할 포지션과 카메라를 저장하는 함수
    public void SaveGameProgress(int iTransform, int iCamera)
    {
        SaveData_Manager.Instance.SetIntTransformRespawn(iTransform);
        SaveData_Manager.Instance.SetIntCameraNum(iCamera);
    }

    // #. 플레이어의 위치와 카메라를 설정해주는 함수
    public void PlayerStartSeeting(int iTransform, int iCamera)
    {
        player.transform.position = Transforms_Respawn[iTransform].position;
        Cameras[iCamera].SetActive(true);

    }




    

    // #. 플레이어가 죽었을 때 실행시킬 함수
    public void DiePlayerReset(float fDieDelay = 2f, int iDieIndex = 0, float fDieAimDuration = 0f)  // 죽음 함수를 실행 시키고 얼마나 뒤에 상태를 리셋할 건지 정할 수 있도록
    {
        if (!bPlayerDie)
        {
            bPlayerDie = true;

            PlayerInputLockOn();    // 플레리어 입력 막음

            StartCoroutine(_DiePlayerReset(fDieDelay)); 
            StartCoroutine(ActionPlayerDieAnimation(iDieIndex, fDieAimDuration));       // 상황별 죽음 애니메이션
            
        }
    }
    IEnumerator _DiePlayerReset(float _fDieDelay) 
    {
        InGameUIController.Instance.bIsUIDoing = true;

        yield return new WaitForSeconds(_fDieDelay);

        SoundAssistManager.Instance.MuteMasterVolume(2f);
        InGameUIController.Instance.FadeInOutImage(1f, 1f);

        yield return new WaitForSeconds(2f);

        string currentSceneName = SceneManager.GetActiveScene().name;
        InGameUIController.Instance.bIsUIDoing = false;

        SceneManager.LoadScene(currentSceneName);
    }
    IEnumerator ActionPlayerDieAnimation(int iDieIndex = 0, float fDieAimDuration = 0f)
    {
        yield return new WaitForSeconds(fDieAimDuration);

        // 0 - 분해되서 죽음
        // 1 - 잡혀서 죽음

        Player _player = player.GetComponent<Player>();
        _player.SetDieState(iDieIndex);
    }







    // #. Respawn 지점 업데이트
    // GameAssist에서 가지고 있는 위치와 동일해야 됨
    public void RespawnChangeAssist(Transform transform)
    {
        for (int i = 0; i < Transforms_Respawn.Length; i++)
        {
            if (Transforms_Respawn[i] == transform)
            {
                SaveData_Manager.Instance.SetIntTransformRespawn(i);
                break;
            } 
        } 
    } 


    // #. 실제로 사용할 카메라만 True로 하고 나머지는 모두 false
    public void CameraChangeAssist(GameObject camera)
    {
        for (int i = 0; i < Cameras.Length; i++)
        {
            if (Cameras[i] != camera) Cameras[i].SetActive(false);
            else
            {
                Cameras[i].SetActive(true);
                SaveData_Manager.Instance.SetIntCameraNum(i);
            }
               
        }
    }
    // #. 현재 활성화된 카메라와 변경하려는 카메라가 다른지 구분하는 함수
    public bool BoolNowActiveCameraObj(GameObject camera)
    {
        GameObject obj = null;
        for(int i = 0; i < Cameras.Length; i++)
        {
            if (Cameras[i].activeSelf == true)
            {
                obj = Cameras[i];
                break;
            }
        }
        if(camera == obj) return true; // 현재 활성화 되어 있는 카메라와 동일하면 true를 반환
                          return false;
    }





    // #. 내면 진입 
    public void InsideInEffect()
    {
        if (volume_1 == null) return;

        InGameUIController.Instance.bIsUIDoing = true;
        SetVignetteIntensity(volume_1, 1f, 2f);
        SetColorFilter(volume_1, 0f, 0f, 0f, 2f);
        DOTween.To(() => volume_1.weight, x => volume_1.weight = x, 1f, 3f);
    }
    
    // #. 내면 탈출
    public void InsideOutEffect()
    {
        SetVignetteIntensity(volume_1, 0f, 2f);
        DOTween.To(() => volume_1.weight, x => volume_1.weight = x, 0f, 3f)
            .OnComplete(() => {
                InGameUIController.Instance.bIsUIDoing = false;
            });
    }


    public void SetVignetteIntensity(Volume volume, float targetIntensity, float duration)
    {
        if (volume.profile.TryGet<Vignette>(out var vignette))
        {
            Vector3 playerViewportPosition = Camera.main.WorldToViewportPoint(player.transform.position);
            vignette.center.Override(new Vector2(playerViewportPosition.x, playerViewportPosition.y));

            vignette.intensity.Override(vignette.intensity.value); // 현재 값을 설정
            DOTween.To(() => vignette.intensity.value, x => vignette.intensity.Override(x), targetIntensity, duration);

        }
    }
    public void SetColorFilter(Volume volume, float targetR, float targetG, float targetB, float duration)
    {
        if (volume.profile.TryGet<ColorAdjustments>(out var colorAdjustments))
        {
            // 현재 RGB 값을 가져옵니다.
            Color currentColor = colorAdjustments.colorFilter.value;

            // 목표 색상을 설정합니다.
            Color targetColor = new Color(targetR, targetG, targetB);

            // DOTween을 사용하여 ColorFilter의 RGB 값을 애니메이션합니다.
            DOTween.To(() => currentColor, x =>
            {
                currentColor = x;
                colorAdjustments.colorFilter.Override(currentColor);
            }, targetColor, duration);
        }
    }
    public void AnimateFogDensity(float targetValue, float duration)
    {
        DOTween.To(() => RenderSettings.fogDensity, x => RenderSettings.fogDensity = x, targetValue, duration);
    }
    public void AnimateAmbientIntensity(float targetValue, float duration)
    {
        DOTween.To(() => RenderSettings.ambientIntensity, x => RenderSettings.ambientIntensity = x, targetValue, duration);
    }



    // 플레이어 관련
    // 플레이어 관련
    // 플레이어 관련
    #region


    // #. Player 태그가 붙은 오브젝트 중에 가장 부모 오브젝트를 찾아오는 함수
    public GameObject FindPlayerRoot()
    {
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject obj in playerObjects)
        {
            if (obj.transform.root.CompareTag("Player")) return obj.transform.root.gameObject;
        }
        return null; // "Player" 태그의 최고 부모 오브젝트가 없을 경우
    }


    // #. 여러 Get / Set 함수
    public void SetPlaterObj(GameObject obj)
    {
        player = obj;
    }
    public GameObject GetPlayer()
    {
        return player;
    }
    public bool GetBoolPlayerDie()
    {
        return bPlayerDie;
    }





    // #. 플레이어 입력 잠금 On
    public void PlayerInputLockOn()
    {
        Player playerScript = player.GetComponent<Player>();
        if (playerScript == null) return;

        playerScript.machine.OnStateChange(playerScript.machine.UC_IdleState);
    }
    // #. 플레이어 입력 잠금 Off
    public void PlayerInputLockOff()
    {
        if (bCantInpuFunc) return;

        Debug.Log("플레이어를 다시 조작 가능");

        Player playerScript = player.GetComponent<Player>();
        if (playerScript == null) return;

        playerScript.machine.OnStateChange(playerScript.machine.IdleState);

        Rigidbody playerRigidbody = player.GetComponent<Rigidbody>();
        playerRigidbody.constraints = RigidbodyConstraints.None;
        playerRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
    }


    #endregion

}
