using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train_2 : MonoBehaviour
{
    // #. 시작 지점, 정거장 지점, 끝 지점 Transform 다 설정 해야 함
    public Transform position_StationPoint_2;

    public float travelDuration;        // 출발점 -> 정거장, 정거장 -> 최종 목적지 이동 시간

    public TrainDoor[] trainDoors;
    public GameObject[] crowds;

    [Header("지하철 바닥 활성화 여부 관리")]
    public GameObject Floor;
    public GameObject Wall;

    public void StartTrain()
    {
        StartCoroutine(StartTrainJourney());
    }

    // 기차 여정을 시작하는 코루틴
    private IEnumerator StartTrainJourney()
    {
        GameAssistManager.Instance.player.transform.SetParent(transform);

        // 열리는 문 중에서 하나의 문에만 탑승할 수 있도록
        for (int i = 0; i < trainDoors.Length; i++)
        {
            crowds[i].SetActive(true);
        }
        crowds[SubWayAssist.Instance.iCrowedRanNum].SetActive(false);

        yield return new WaitForSeconds(0.1f);


        // 기차가 계속 이동합니다.
        transform.DOMove(position_StationPoint_2.position, travelDuration)
                       .SetEase(Ease.OutCubic)
                       .SetUpdate(true); 

        yield return new WaitForSeconds(travelDuration);


        // 문을 엽니다.
        GameAssistManager.Instance.player.transform.SetParent(null);
        foreach (TrainDoor traindoor in trainDoors)
            traindoor.StartOpen();

        GameAssistManager.Instance.PlayerInputLockOff();
        Floor.SetActive(true);
        Wall.SetActive(false);

    }
}
