using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrafficLight_2 : MonoBehaviour
{
    [Header("신호등 불빛 관리")]
    public GameObject[] TrafficThreeColors;   // 차량 신호등
    public GameObject[] TrafficTwoClolors;    // 도보 신호등

    public TrafficClockWorkAssist[] trafficClockWorkAssists;




    // #. 신호등 불 교체 함수 
    // 0 = 빨간불, 1 = 노란불, 2 = 초록불
    public void ChangeTrafficColor_(int index)
    {
        if (index < 0 || index >= TrafficThreeColors.Length) return;

        for (int i = 0; i < TrafficThreeColors.Length; i++) TrafficThreeColors[i].SetActive(false);
        for (int i = 0; i < TrafficTwoClolors.Length; i++) TrafficTwoClolors[i].SetActive(false);

        // 차량 신호등 관리
        TrafficThreeColors[index].SetActive(true);


        // 인도 신호등 관리
        if (index == 0) TrafficTwoClolors[1].SetActive(true);
        else TrafficTwoClolors[0].SetActive(true);
    }

    public void SpinClockWork(int spinTime)
    {
        for (int i = 0; i < trafficClockWorkAssists?.Length; i++)
        {
            trafficClockWorkAssists[i].RotateObject(spinTime + 3, i % 2 == 0 ? 1f : -1f);
        }
    }
}
