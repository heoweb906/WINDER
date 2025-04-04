using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ClockWorkType
{
    Floor,
    Wall
};

public class ClockWork : InteractableObject
{
    public ClockBattery clockBattery;
    [SerializeField] private ClockWorkType clockWorkType;

    public List<ClockWork> plusClockWorksList  = new List<ClockWork>();

    public bool isSingleEvent;

    private void Start()
    {
        type = InteractableType.ClockWork;
        // canInteract = true;
    }


    public void ChargingBattery()
    {
        if (clockBattery.fMaxClockBattery > clockBattery.fCurClockBattery && !clockBattery.bDoing)
        {
            //Debug.Log("태엽 돌리는 중");
            clockBattery.fCurClockBattery += 1;
            //transform.Rotate(Vector3.forward * 80f * Time.deltaTime);
            clockBattery.clockWork = this.gameObject;
            canInteract = false;
        }
        if (clockBattery.fMaxClockBattery <= clockBattery.fCurClockBattery) clockBattery.bBatteryFull = true;
    }

    public void EndCharging_To_BatteryStart()
    {
        if (!canInteract)
        {
            //Debug.Log("태엽 -> 배터리 가동");
            clockBattery.TurnOnObj();
        }
    }


    public bool BoolBatteryFullCharging()
    {
        return clockBattery.bBatteryFull;
    }


    public ClockWorkType GetClockWorkType()
    {
        return clockWorkType;
    }

    public void ClockWorkRotate(float fRotateDirection = 1f, float fRotateSpeed_Wall = 0.3f, float fRotateSpeed_Floor = 0.8f)
    {
        if (clockWorkType == ClockWorkType.Wall)
        {
            gameObject.transform.DORotate(new Vector3(0, 0, 180 * fRotateDirection), 0.3f, RotateMode.LocalAxisAdd)
                .SetEase(Ease.Linear);

            for (int i = 0; i < plusClockWorksList?.Count; i++)  // 수정된 부분
            {
                plusClockWorksList[i].ClockWorkRotate(i % 2 == 0 ? 1f : -1f, 0.3f, 0.3f);
            }
        }
        else if (clockWorkType == ClockWorkType.Floor)
        {
            gameObject.transform.DORotate(new Vector3(0, 0, 180 * fRotateDirection), fRotateSpeed_Floor, RotateMode.LocalAxisAdd)
                .SetEase(Ease.Linear);

            for (int i = 0; i < plusClockWorksList?.Count; i++)  // 수정된 부분
            {
                plusClockWorksList[i].ClockWorkRotate(i % 2 == 0 ? 1f : -1f, 0.8f, 0.8f);
            }
        }
    }



    public void SetClockWorkType(int index)
    {
        if(index == 0)
        {
            clockWorkType = ClockWorkType.Floor;
        }
        else
        {
            clockWorkType = ClockWorkType.Wall;
        }
    }
}


