using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsideAssist_GGILICK : MonoBehaviour
{
    public static InsideAssist_GGILICK Instance;

    [Header("차량 관리")]
    public bool bCarCreating; // 차량 생성 중

    public GameObject[] roadCars;
    public Transform[] positions_carCreate;
    public Transform postion_end;
    public float spawnRate; // 자동차가 생성되는 평균 시간
    public float cooldownDuration; // 생성된 위치가 사용 불가능한 시간
    private float spawnTimer = 0f; // 타이머
    private float[] positionCooldowns; // 각 위치의 쿨다운 타이머



    private void Awake()
    {
        Instance = this;
    }



    private void Start()
    {
        positionCooldowns = new float[positions_carCreate.Length];
    }


    private void Update()
    {
        // 자동차 관련 부분
        if (bCarCreating)
        {
            spawnTimer += Time.deltaTime;

            // 평균적으로 3초에 2대 생성 (1.5초마다 1대)
            if (spawnTimer >= spawnRate)
            {
                SpawnCars();
                spawnTimer = 0f; // 타이머 초기화
            }

            for (int i = 0; i < positionCooldowns.Length; i++) if (positionCooldowns[i] > 0) positionCooldowns[i] -= Time.deltaTime;
        }
      
    }


    private void SpawnCars()
    {
        int ranNum_posotion = Random.Range(0, positions_carCreate.Length);
        int ranNum_car = Random.Range(0, roadCars.Length);

        if (positionCooldowns[ranNum_posotion] <= 0)
        {
            GameObject car = Instantiate(roadCars[ranNum_car], positions_carCreate[ranNum_posotion].position, Quaternion.identity);
            GGILICK_Car ggilcikCar = car.GetComponent<GGILICK_Car>();
            ggilcikCar.transform_Destroy = postion_end;

            positionCooldowns[ranNum_posotion] = cooldownDuration;
        }
        else SpawnCars();
    }


}
