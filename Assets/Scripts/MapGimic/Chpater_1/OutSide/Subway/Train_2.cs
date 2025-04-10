using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Train_2 : MonoBehaviour
{
    // #. 시작 지점, 정거장 지점, 끝 지점 Transform 다 설정 해야 함
    public Transform position_StationPoint_2;

    public float travelDuration;        // 출발점 -> 정거장, 정거장 -> 최종 목적지 이동 시간

    public Transform[] telepotTranforms;
    public TrainDoor[] trainDoors;
    public NPC_Simple[] npcArray;
    public Transform transform_RotationTarget;

    [Header("지하철 바닥 활성화 여부 관리")]


    public GameObject Floor;
    // public GameObject Wall;


    public void StartTrain()
    {
        StartCoroutine(StartTrainJourney());
    }

    // 기차 여정을 시작하는 코루틴
    private IEnumerator StartTrainJourney()
    {
        Rigidbody rigid = GameAssistManager.Instance.player.GetComponent<Rigidbody>();
        GameAssistManager.Instance.player.transform.SetParent(telepotTranforms[SubWayAssist.Instance.iCrowedRanNum]);
        npcArray[SubWayAssist.Instance.iCrowedRanNum].gameObject.SetActive(false);

   
        rigid.isKinematic = true;


        yield return new WaitForSeconds(0.1f);
        


        // 기차가 계속 이동합니다.
        transform.DOMove(position_StationPoint_2.position, travelDuration)
               .SetEase(Ease.OutCubic)
               .SetUpdate(UpdateType.Fixed);

        yield return new WaitForSeconds(travelDuration);

        rigid.isKinematic = false;
        Floor.SetActive(true);

        for (int i = 0; i < npcArray.Length; ++i)
        {
            if (i == SubWayAssist.Instance.iCrowedRanNum) continue;

            npcArray[i].GetNav().enabled = true;
        }


        // 문을 엽니다.
        GameAssistManager.Instance.player.transform.SetParent(null);
        foreach (TrainDoor traindoor in trainDoors)
            traindoor.StartOpen();

        GameAssistManager.Instance.PlayerInputLockOff();

        yield return new WaitForSeconds(3f);


        for (int i = 0; i < npcArray.Length; ++i)
        {
            if (i == SubWayAssist.Instance.iCrowedRanNum) continue;

            npcArray[i].ChangeStateToSubWayTakeOff();
            npcArray[i].machine.SubwayStateTakeOffState.target = transform_RotationTarget;
        }



        // Floor.SetActive(true);
        // Wall.SetActive(false);

    }
}
