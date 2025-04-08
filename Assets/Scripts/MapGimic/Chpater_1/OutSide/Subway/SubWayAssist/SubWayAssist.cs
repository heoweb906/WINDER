using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubWayAssist : MonoBehaviour
{
    public static SubWayAssist Instance;

    [Header("플레이어 위치 체크")]
    public bool bPlayerInSubway;   // 지하철 진입 완료
    public bool bPlayerTakeTrain;  // 기차 탑승 완료
    public bool bPlayerTeleport;   // 지하철역 2구역 진입 완료

    public GameObject TrainObj;


    public int iCrowedRanNum = 0;
   

    private void Awake()
    {
        Instance = this; 
    }


    public void LetsStartTrain()
    {
        Debug.Log("어디서 호출하는 거야");

        bPlayerInSubway = true;
        Train train = TrainObj.GetComponent<Train>();
        train.StartTrain();
    }


}
