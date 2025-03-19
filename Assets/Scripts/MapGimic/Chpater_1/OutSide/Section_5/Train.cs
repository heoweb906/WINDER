using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour
{
    // #. 시작 지점, 정거장 지점, 끝 지점 Transform 다 설정 해야 함
    public Transform position_StartPoint;
    public Transform position_StationPoint;
    public Transform position_EndPoint;

    public float travelDuration;        // 출발점 -> 정거장, 정거장 -> 최종 목적지 이동 시간
    public float stopDuration;          // 정거장에서 멈추는 시간

    public TrainDoor[] trainDoors;
    public GameObject[] crowds;

    [Header("지하철 바닥 활성화 여부 관리")]
    public Train_PlayerCheck trainPlayerCheck;
    public GameObject Floor;
    public GameObject Wall;



    public void StartTrain()
    {
        transform.position = position_StartPoint.position;  // 기차를 StartPoint 위치로 이동시킴
        StartCoroutine(StartTrainJourney());
    }

    // 기차 여정을 시작하는 코루틴
    private IEnumerator StartTrainJourney()
    {
        transform.position = position_StartPoint.position;

        // 열리는 문 중에서 하나의 문에만 탑승할 수 있도록
        SubWayAssist.Instance.iCrowedRanNum = Random.Range(0, trainDoors.Length);
        for(int i = 0; i < trainDoors.Length; i++) crowds[i].SetActive(true);
        crowds[SubWayAssist.Instance.iCrowedRanNum].SetActive(false);


        // 1. StartPoint에서 StationPoint로 이동 (서서히 멈추는 효과)
        transform.DOMove(position_StationPoint.position, travelDuration)
            .SetEase(Ease.OutCubic)  // 이동이 끝나갈 때 점점 느려짐
            .SetUpdate(UpdateType.Fixed, true);

        yield return new WaitForSeconds(travelDuration);


        // 2. 기차 문을 열고 일정 시간 뒤에 다시 출발
        Wall.SetActive(false);
        foreach (TrainDoor traindoor in trainDoors) traindoor.StartOpen_Close(stopDuration);


        yield return new WaitForSeconds(stopDuration);


        // 3. StationPoint에서 EndPoint로 이동 (서서히 가속하는 효과)
        Wall.SetActive(true);
        if (trainPlayerCheck.bPlayerNearby)
        {
            Floor.SetActive(false);
            GameAssistManager.Instance.PlayerInputLockOn();
        } 
            
           
        transform.DOMove(position_EndPoint.position, travelDuration)
            .SetEase(Ease.InCubic)   // 출발 시 서서히 가속
            .SetUpdate(UpdateType.Fixed, true);

        yield return new WaitForSeconds(travelDuration);


        // 만약 플레이어가 탑승한 것이 확인되지 않았다면 다시 되돌림
        if (!SubWayAssist.Instance.bPlayerTakeTrain) StartCoroutine(StartTrainJourney());
    }



}
