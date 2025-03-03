using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InsideAssist : MonoBehaviour
{
    public static InsideAssist Instance { get; private set; }

    [Header("카메라 전환 장치들, 연출")]
    public Camera_PlayerFollow cameraFollow_1;
    public CineCameraChager cineChager_1;   // 처음 돌돌이에게 카메라가 
    public CineCameraChager cineChager_2;   // 끼릭이의 모습을 보여줌
    

    public Transform transformTeleport_Inside;     // 플레이어 순간이동 위치
    public GameObject transform_GGILICK;
    public GameObject nowCamera;        // 카메라?


    [Header("자동타 생성 관련")]
    public bool bCanCarCreate;
    public CinemachineSmoothPath Path_Special;
    public CinemachineSmoothPath[] path;
    public GameObject Car_Special;
    public GameObject[] Cars;
    private float fTimer;

    public float fCreateInterval = 3f; // 3초마다 생성

    private void Start()
    {
        Instance = this;


        if(SaveData_Manager.Instance.GetBoolInside())
        {
            StartCoroutine(StartInsideAgain());
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

        GameAssistManager.Instance.AnimateFogDensity(0f, 2f);
        GameAssistManager.Instance.AnimateAmbientIntensity(0.8f, 2f);

        yield return new WaitForSecondsRealtime(6f);

        bCanCarCreate = true;
    }


    // 연출 관련
    #region

    // #. 내면 세계 첫진입
    public void StartDirect_1()
    {
        GameAssistManager.Instance.InsideInEffect();
        StartCoroutine(InsideOn_GGILICK());
    }
    IEnumerator InsideOn_GGILICK()
    {
        Rigidbody rigid = GameAssistManager.Instance.player.GetComponent<Rigidbody>();
        rigid.constraints = RigidbodyConstraints.FreezePositionY;

        yield return new WaitForSeconds(1.2f);

        cineChager_1.CameraChange();

        yield return new WaitForSeconds(1.0f);

        GameAssistManager.Instance.AnimateFogDensity(0f, 2f);
        GameAssistManager.Instance.AnimateAmbientIntensity(0.3f, 2f);

        yield return new WaitForSeconds(3.5f);  // 카메라 보간 시간이랑 어느 정도 안맞추면 
                                                // 다음 플레이어 이동 시 뚝 끊기는 것처럼 보임


     
        Vector3 teleportPosition = transformTeleport_Inside.position;    // 플레이어랑 카메라 순간이동
        Vector3 playerPosition = GameAssistManager.Instance.GetPlayer().transform.position;
        Vector3 offset = nowCamera.transform.position - playerPosition; // 플레이어와 gamObject 간의 상대적 위치
        nowCamera.transform.position = teleportPosition + offset; // gamObject를 새로운 위치에 배치
        yield return new WaitForEndOfFrame();
        GameAssistManager.Instance.GetPlayer().transform.position = teleportPosition; // 플레이어를 순간이동




        //yield return new WaitForSeconds(0.5f);
        //SoundAssistManager.Instance.GetSFXAudioBlock("Voice_ggilick_1", this.transform, true);
        //yield return new WaitForSeconds(4.0f);
        //SoundAssistManager.Instance.GetSFXAudioBlock("Voice_ggilick_2", this.transform, true);



        yield return new WaitForSeconds(3.0f);
        rigid.constraints = RigidbodyConstraints.None; // FreezePosition X, Y, Z 모두 false
        rigid.constraints = RigidbodyConstraints.FreezeRotationX |
                            RigidbodyConstraints.FreezeRotationY |
                            RigidbodyConstraints.FreezeRotationZ;

       
    
        yield return new WaitForSeconds(1f);

        InGameUIController.Instance.FadeInOutImage(1f, 2f);
        GameAssistManager.Instance.InsideOutEffect();

        yield return new WaitForSeconds(2.5f);

        cameraFollow_1.SetPlayer(transform_GGILICK.transform);

        yield return new WaitForSeconds(2f);



        // #. 끼릭이 연출 보여주는 구간
        InGameUIController.Instance.FadeInOutImage(0f, 2.5f);







        yield return new WaitForSeconds(3f);

        cineChager_2.CameraChange();

        yield return new WaitForSeconds(6f);

        CreateCarFrontPlayer();

        yield return new WaitForSeconds(2f);


        SaveData_Manager.Instance.SetBoolInside(true);  
        InsideAssist.Instance.bCanCarCreate = true;     
    }



    #endregion





    // 차령 관련
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
