using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InsideAssist : MonoBehaviour
{
    public static InsideAssist Instance { get; private set; }

    [Header("카메라 전환 장치들, 연출")]
    public Light light_1;
    public Light light_2;
    public Light light_3;
    public Light light_4;


    public Camera_PlayerFollow cameraFollow_1;
    public CineCameraChager cineChager_1;   // 처음 돌돌이에게 카메라가 
    public CineCameraChager cineChager_2;   // 끼릭이의 모습을 보여줌
    public CineCameraChager cineChager_InsideFinish;   // 내면 세계가 끝남

    public Transform transformTeleport_Inside;     // 플레이어 순간이동 위치
    public Transform transformTeleport_InsdieFinish;     // 플레이어 순간이동 위치
    public GameObject transform_GGILICK;
    public GameObject nowCamera;        // 카메라?
    public GameObject nowCamera2;


    [Header("자동차 생성 관련")]
    public bool bCanCarCreate;
    public CinemachineSmoothPath Path_Special;
    public CinemachineSmoothPath[] path;
    public GameObject Car_Special;
    public GameObject[] Cars;
    private float fTimer;
    public float fCreateInterval = 3f; // 3초마다 생성



    [Header("내면에서 나왔을 때 사용할 세팅")]
    public GameObject GgilcikBad;       // 내면에서 나오고 나면 삭제시켜야 함

    public Material skyboxMaterial; // 사용할 스카이박스
    public Color targetFogColor = new Color(168f / 255f, 168f / 255f, 168f / 255f); // 목표 포그 색상
    public float targetFogDensity = 0.03f; // 목표 포그 밀도
    public float targetHaloStrength = 0.267f; // 목표 할로 강도
    public float targetFlareFadeSpeed = 3f; // 목표 플레어 페이드 속도
    public float targetIntensityMultiplier = 1f; // 목표 환경광 강도
    public Color targetShadowColor = new Color(147f / 255f, 147f / 255f, 147f / 255f); // 목표 실시간 그림자 색상



    [Header("끼릭이들")]
    public GameObject obj_Ggilick_CutScene_1;            // 버림받는 끼릭
    public GameObject obj_Ggilick_Beetles;      // 고속도로 한복판에 버려져 있을 끼릭
    






    private void Start()
    {
        Instance = this;

        Debug.Log(SaveData_Manager.Instance.GetIntInside());

        if (SaveData_Manager.Instance.GetIntInside() == 1)
        {
            StartCoroutine(StartInsideAgain());
        }
        else if (SaveData_Manager.Instance.GetIntInside() == 2)
        {
            AnimateLightIntensity(light_1, 0f, 3f);
            AnimateLightIntensity(light_2, 0f, 3f);
            AnimateLightIntensity(light_3, 0.8f, 1.5f);
            AnimateLightIntensity(light_4, 0.38f, 1.5f);
            LightSettingNew(2f);
            GgilcikBad.SetActive(false);
        }
    }

    private void Update()
    {
        if (bCanCarCreate) CalculateCarTime();
    }


    // #. 내면 세계에서 죽었을 때 호출할 함수
    //    플레이어 스폰 위치, 카메라 위치 등을 수정
    private IEnumerator StartInsideAgain()
    {
        Debug.Log("실행됨");

        AnimateLightIntensity(light_1,0f, 2f);
        GameAssistManager.Instance.AnimateAmbientIntensity(0.3f, 1.7f);
        GameAssistManager.Instance.AnimateFogDensity(0f, 1.7f);
      

        yield return new WaitForSecondsRealtime(6f);

        bCanCarCreate = true;
    }


    // 연출 관련
    #region

    // #. 내면 세계 첫 진입
    public void StartDirect_1()
    {
        GameAssistManager.Instance.InsideInEffect();
        StartCoroutine(InsideOn_GGILICK());
    }
    IEnumerator InsideOn_GGILICK()
    {
        GameAssistManager.Instance.PlayerInputLockOn();

        //Rigidbody rigid = GameAssistManager.Instance.player.GetComponent<Rigidbody>();
        //rigid.constraints = RigidbodyConstraints.FreezePositionY;

        yield return new WaitForSeconds(1.2f);

        cineChager_1.CameraChange();

        yield return new WaitForSeconds(1.5f);


        // #. 여기가 포그랑 라이팅 조절하는 구간임
        // #. 여기가 포그랑 라이팅 조절하는 구간임
        AnimateLightIntensity(light_1, 0f, 2f);
        GameAssistManager.Instance.AnimateAmbientIntensity(0.3f, 1.7f);
        GameAssistManager.Instance.AnimateFogDensity(0f, 1.7f);
      


        yield return new WaitForSeconds(3.5f);  // 카메라 보간 시간이랑 어느 정도 안맞추면 
                                                // 다음 플레이어 이동 시 뚝 끊기는 것처럼 보임


        Vector3 teleportPosition = transformTeleport_Inside.position;    // 플레이어랑 카메라 순간이동
        Vector3 playerPosition = GameAssistManager.Instance.GetPlayer().transform.position;
        Vector3 offset = nowCamera.transform.position - playerPosition; // 플레이어와 gamObject 간의 상대적 위치
        nowCamera.transform.position = teleportPosition + offset; // gamObject를 새로운 위치에 배치


        offset = nowCamera2.transform.position - playerPosition;
        nowCamera2.transform.position = teleportPosition + offset;

        yield return new WaitForEndOfFrame();
        GameAssistManager.Instance.GetPlayer().transform.position = teleportPosition; // 플레이어를 순간이동



        yield return new WaitForSeconds(3.0f);

        InGameUIController.Instance.FadeInOutImage(1f, 2f);

        yield return new WaitForSeconds(2.1f);

        HandheldCamera handle = nowCamera.GetComponent<HandheldCamera>();
        handle.enabled = false;
        CameraObj camObj = nowCamera.GetComponent<CameraObj>();
        camObj.rotationOffset = new Vector3(20f, -113.1f, 0f);
        Camera_PlayerFollow camObj_ = nowCamera.GetComponent<Camera_PlayerFollow>();
        camObj_.offset = new Vector3(0.3f, 2.03f, 5.81f);


        GameAssistManager.Instance.InsideOutEffect();

        yield return new WaitForSeconds(2.5f);
        handle.enabled = true;

        cameraFollow_1.SetPlayer(transform_GGILICK.transform);






        yield return new WaitForSeconds(2f);

        // #. 끼릭이 연출 보여주는 구간
        InGameUIController.Instance.FadeInOutImage(0f, 2.5f);
        Animator tempAnim = obj_Ggilick_CutScene_1.GetComponent<Animator>();
        tempAnim.SetTrigger("doCutSceneStart");


        yield return new WaitForSeconds(19f); 
         







        cineChager_2.CameraChange();


        yield return new WaitForSeconds(5.8f);
        GameAssistManager.Instance.PlayerInputLockOff();


        yield return new WaitForSeconds(0.2f);

        obj_Ggilick_CutScene_1.SetActive(false);
        obj_Ggilick_Beetles.SetActive(true);

        CreateCarFrontPlayer();
        SaveData_Manager.Instance.SetIntInside(1);
        InsideAssist.Instance.bCanCarCreate = true;
    }




    // #. 내면 세계 탈출
    public void StartDirect_2()
    {
        GameAssistManager.Instance.InsideInEffect();
        InGameUIController.Instance.bIsUIDoing = true;
        StartCoroutine(InsideOff_GGILICK());
    }
    IEnumerator InsideOff_GGILICK()
    {
        GameAssistManager.Instance.PlayerInputLockOn();
        GgilcikBad.SetActive(false);

        yield return new WaitForSeconds(1.2f);

        cineChager_InsideFinish.CameraChange();

        yield return new WaitForSeconds(1.5f);


        // #. 여기가 포그랑 라이팅 조절하는 구간임
        // #. 여기가 포그랑 라이팅 조절하는 구간임
        AnimateLightIntensity(light_1, 0f, 3f);
        AnimateLightIntensity(light_2, 0f, 3f);
        AnimateLightIntensity(light_3, 0.8f, 1.5f);
        AnimateLightIntensity(light_4, 0.38f, 1.5f);
        LightSettingNew(2f);



        yield return new WaitForSeconds(3.5f);  // 카메라 보간 시간이랑 어느 정도 안맞추면 
                                                // 다음 플레이어 이동 시 뚝 끊기는 것처럼 보

        Vector3 teleportPosition = transformTeleport_InsdieFinish.position;    // 플레이어랑 카메라 순간이동
        Vector3 playerPosition = GameAssistManager.Instance.GetPlayer().transform.position;
        Vector3 offset = nowCamera2.transform.position - playerPosition; // 플레이어와 gamObject 간의 상대적 위치
        nowCamera2.transform.position = teleportPosition + offset; // gamObject를 새로운 위치에 배치
        yield return new WaitForEndOfFrame();
        GameAssistManager.Instance.GetPlayer().transform.position = teleportPosition; // 플레이어를 순간이동



        yield return new WaitForSeconds(3f);
        InGameUIController.Instance.FadeInOutImage(1f, 2f);
    


        yield return new WaitForSeconds(3f);
        GameAssistManager.Instance.InsideOutEffect();
        InGameUIController.Instance.FadeInOutImage(0f, 2f);

        yield return new WaitForSeconds(3f);


        GameAssistManager.Instance.PlayerInputLockOff();
        SaveData_Manager.Instance.SetIntInside(2);
        InsideAssist.Instance.bCanCarCreate = false;




        InGameUIController.Instance.bIsUIDoing = false;
    }

    private void LightSettingNew(float duration)
    {
        if (skyboxMaterial != null)
        {
            RenderSettings.skybox = skyboxMaterial;
        }

        // AnimateLightIntensity()


        // 환경광 강도 조절
        DOTween.To(() => RenderSettings.ambientIntensity, x => RenderSettings.ambientIntensity = x, targetIntensityMultiplier, duration);

        // 실시간 그림자 색상 조절
        DOTween.To(() => RenderSettings.subtractiveShadowColor, x => RenderSettings.subtractiveShadowColor = x, targetShadowColor, duration);

        // Fog 설정
        RenderSettings.fogMode = FogMode.ExponentialSquared;
        DOTween.To(() => RenderSettings.fogColor, x => RenderSettings.fogColor = x, targetFogColor, duration);
        DOTween.To(() => RenderSettings.fogDensity, x => RenderSettings.fogDensity = x, targetFogDensity, duration);

        // 할로 및 플레어 설정
        DOTween.To(() => RenderSettings.haloStrength, x => RenderSettings.haloStrength = x, targetHaloStrength, duration);
        DOTween.To(() => RenderSettings.flareFadeSpeed, x => RenderSettings.flareFadeSpeed = x, targetFlareFadeSpeed, duration);
    }











    #endregion


    public void AnimateLightIntensity(Light light, float targetValue, float duration)
    {
        DOTween.To(() => light.intensity, x => light.intensity = x, targetValue, duration);
    }





    // 차량 관련
    #region

    // #. 차량 생성 함수
    private void CalculateCarTime()
    {
        if (path == null || path.Length == 0 || Cars.Length == 0) return;

        fTimer += Time.deltaTime;

        if (fTimer >= fCreateInterval)
        {
            CreateCarAtPath();
            fTimer = 0f;
        }
    }
    private void CreateCarAtPath()
    {
        // 경로에서 랜덤한 인덱스 선택
        int iRandomNum = Random.Range(0, path.Length);
        Vector3 spawnPosition = path[iRandomNum].EvaluatePosition(0f);

        // 랜덤한 차량을 확률에 따라 선택
        int rand = Random.Range(0, 100); // 0 ~ 99 사이 난수 생성
        GameObject selectedCar;

        if (rand < 60) selectedCar = Cars[0];  // 60% 확률
        else if (rand < 85) selectedCar = Cars[1]; // 25% 확률
        else selectedCar = Cars[2]; // 15% 확률

        // 선택한 차량 인스턴스화 및 설정
        RoadCarOnTrack roadCar = Instantiate(selectedCar, spawnPosition, Quaternion.identity).GetComponent<RoadCarOnTrack>();
        roadCar.transform.SetParent(transform);
        roadCar.m_Path = path[iRandomNum];
    }


    private void CreateCarFrontPlayer()
    {
        Vector3 spawnPosition = Path_Special.EvaluatePosition(0f);

        // 선택한 차량 인스턴스화 및 설정
        RoadCarOnTrack roadCar = Instantiate(Car_Special, spawnPosition, Quaternion.identity).GetComponent<RoadCarOnTrack>();
        roadCar.transform.SetParent(transform);
        roadCar.m_Path = Path_Special;
    }


    #endregion




}
