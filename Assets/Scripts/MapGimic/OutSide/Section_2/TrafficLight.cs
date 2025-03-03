using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UIElements;

public class TrafficLight : ClockBattery, IPartsOwner
{
    [Header("실제 신호등 작동 관리")]
    [SerializeField] private bool bTrafficLightOnOff; // 현재 신호등의 상태
    public GameObject crossWalk_Assist; // 횡단보도 작동 시에 장애물 역할 해줄 오브젝트 
    
    [Header("신호등 불빛 관리")]
    public GameObject[] TrafficThreeColors;   // 차량 신호등
    public GameObject[] TrafficTwoClolors;    // 도보 신호등
    public TrafficLight_2 trraficLight_2;

    [Header("추가 태엽들")]
    public GameObject testObj;
    private bool bInClockWork; // 고장난 태엽을 가져다 넣었는지
    private List<TrafficClockWorkAssist> trafficClockWorkAssists = new List<TrafficClockWorkAssist>();
    public TrafficClockWorkAssist plusClockWorkObj;
    

    [Space(30f)]

    // 따로 분리할 필요가 없다고 판단되어 신호등 스크립트 하나에서 관리합니다.
    [Header("차량 관리")] 
    public GameObject[] roadCars;
    public Transform[] positions_carCreate;
    public Transform[] positions_carCreate_2;
    public Transform[] postions_end;


    private float[] positionCooldowns; // 각 위치의 쿨다운 타이머
    public float spawnRate_1; // 자동차가 생성되는 평균 시간
    public float cooldownDuration_1; // 생성된 위치가 사용 불가능한 시간
    public int iMaxCarCnt_1; // 하나의 도로에서 생성될 수 있는 최대 차량 수
    private float spawnTimer_1 = 0f; // 타이머
    public List<GameObject> spawnedCars_1 = new List<GameObject>(); // 생성된 자동차를 담을 리스트
    public List<GameObject> spawnedCars_2 = new List<GameObject>(); 


    private Coroutine nowCoroutine;

    
    [Header("파츠 관련")] 
    public Transform partsTransform;
    public Transform partsInteractTransform;

    private void Awake()
    {
        ChangeTrafficColor(2);
        positionCooldowns = new float[positions_carCreate.Length];
    }

    private void Update()
    {
        //if(Input.GetKeyDown(KeyCode.T))
        //{
        //    InsertClockWorkPiece(testObj);
        //}

        // 자동차 관련 부분
        spawnTimer_1 += Time.deltaTime;
        if (spawnedCars_1.Count < iMaxCarCnt_1 && bTrafficLightOnOff) // 생성된 자동차가 100대 미만일 때
        {
            if (spawnTimer_1 >= spawnRate_1)
            {
                SpawnCars_1();
                spawnTimer_1 = 0f;
            }
        }
        for (int i = 0; i < positionCooldowns.Length; i++) if (positionCooldowns[i] > 0) positionCooldowns[i] -= Time.deltaTime;
    }




    public override void TurnOnObj()
    {
        base.TurnOnObj();
        RotateObject((int)fCurClockBattery + 2);

        if(bInClockWork)
        {
            for (int i = 0; i < trafficClockWorkAssists?.Count; i++)
            {
                trafficClockWorkAssists[i].RotateObject((int)fCurClockBattery + 3, i % 2 == 0 ? 1f : -1f);
                trraficLight_2.SpinClockWork((int)fCurClockBattery);
            }
        }

        nowCoroutine = StartCoroutine(ChangeToYellowAndRed());
    }

    public override void TurnOffObj()
    {
        base.TurnOffObj();

        if (nowCoroutine != null) StopCoroutine(nowCoroutine);
        ChangeTrafficColor(2);
    }





    // #. 태엽 동작
    private IEnumerator ChangeToYellowAndRed()
    {
        if (bInClockWork)
        {
            yield return new WaitForSeconds(1.6f);
            ChangeTrafficColor(1);
            yield return new WaitForSeconds(1.5f);
            ChangeTrafficColor(0);

            // 배터리가 있는 동안 fCurClockBattery 감소
            while (fCurClockBattery > 0)
            {
                fCurClockBattery -= Time.deltaTime;
                yield return null;
            }

            TurnOffObj();
        }
        else
        {
            yield return new WaitForSeconds(4.1f);
            TurnOffObj();
        }

           
    }

   

    // #. 신호등 불 교체 함수 
    // 0 = 빨간불, 1 = 노란불, 2 = 초록불
    private void ChangeTrafficColor(int index) 
    {
        trraficLight_2.ChangeTrafficColor_(index);

        if (index < 0 || index >= TrafficThreeColors.Length) return;

        for (int i = 0; i < TrafficThreeColors.Length; i++) TrafficThreeColors[i].SetActive(false);
        for (int i = 0; i < TrafficTwoClolors.Length; i++) TrafficTwoClolors[i].SetActive(false);

        bTrafficLightOnOff = (index == 2);
        crossWalk_Assist.SetActive(!bTrafficLightOnOff);

        // 차량 신호등 관리
        TrafficThreeColors[index].SetActive(true);


        // 인도 신호등 관리
        if(index == 0) TrafficTwoClolors[1].SetActive(true);
        else TrafficTwoClolors[0].SetActive(true);
    }






    // #. 자동차 관련 부분
    private void SpawnCars_1()
    {
        int ranNum_posotion = UnityEngine.Random.Range(0, positions_carCreate.Length);
        int ranNum_car = UnityEngine.Random.Range(0, roadCars.Length);


        if (ranNum_posotion < 3)
        {
            if (positionCooldowns[ranNum_posotion] <= 0)
            {
                Quaternion rotation = Quaternion.Euler(0, 180, 0);
                GameObject car = Instantiate(roadCars[ranNum_car], positions_carCreate[ranNum_posotion].position, rotation);
                car.transform.SetParent(gameObject.transform);
                RoadCar roadCar = car.GetComponent<RoadCar>();

                roadCar.trafficLight = this;
                roadCar.bMoveActive = true;
                roadCar.bDirection = true;

                spawnedCars_1.Add(car); // 생성된 자동차를 리스트에 추가
                positionCooldowns[ranNum_posotion] = cooldownDuration_1;
            }
            else SpawnCars_1();
        }
        else
        {
            if (positionCooldowns[ranNum_posotion] <= 0)
            {
                GameObject car = Instantiate(roadCars[ranNum_car], positions_carCreate[ranNum_posotion].position, Quaternion.identity);
                car.transform.SetParent(gameObject.transform);
                RoadCar roadCar = car.GetComponent<RoadCar>();

                roadCar.trafficLight = this;
                roadCar.bMoveActive = true;

                spawnedCars_2.Add(car); // 생성된 자동차를 리스트에 추가
                positionCooldowns[ranNum_posotion] = cooldownDuration_1;
            }
            else SpawnCars_1();
        }
    }



  

    // #. 태엽을 꽂아서 넣어주는 함수
    public void InsertOwnerFunc(GameObject clockWorkObj,int iIndex)
    {
        TrafficClockWorkAssist assist = clockWorkObj.GetComponent<TrafficClockWorkAssist>();
        trafficClockWorkAssists.Add(assist);  
        trafficClockWorkAssists.Add(plusClockWorkObj);  // plusClockWorkObj도 추가


        ClockWork clockWorkMine = clockWork.GetComponent<ClockWork>(); ;

        ClockWork clockwork_1 = trafficClockWorkAssists[0].GetComponent<ClockWork>();
        ClockWork clockwork_2 = trafficClockWorkAssists[1].GetComponent<ClockWork>();

        // clockWorkMine.plusClockWorks를 List로 변경하여 추가
        clockWorkMine.plusClockWorksList.Add(clockwork_1);
        clockWorkMine.plusClockWorksList.Add(clockwork_2);
 
        bInClockWork = true;
    }

    public void RemoveOwnerFunc(int iIndex)
    {

    }

   

}
