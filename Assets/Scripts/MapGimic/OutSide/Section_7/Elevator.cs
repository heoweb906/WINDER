using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Elevator : ClockBattery
{
    public ElevatorDoor elevaDoor;

    public bool bScan;
    public float doorMoveDuration_first;     // 문이 열리는 데 걸리는 시간
    public float doorMoveDuration_second;   // 문이 열리는 데 걸리는 시간
    public float shakeDuration;             // 떨림 애니메이션 지속 시간
    public float shakeStrength;             // 떨림 강도

    public override void TurnOnObj()
    {
        base.TurnOnObj();

        RotateObject((int)fCurClockBattery);
        if (fCurClockBattery < 4f) StartCoroutine(JustCloseDoors());
        else StartCoroutine(OpenAndCloseDoors());
    }


    private IEnumerator JustCloseDoors()
    {
        // 1. 문을 중간 위치로 천천히 이동 (문이 열리는 중간 단계)
        elevaDoor.leftDoor.transform.DOMove(elevaDoor.position_middle_LeftDoor.position, doorMoveDuration_first).SetEase(Ease.InOutCubic);
        elevaDoor.rightDoor.transform.DOMove(elevaDoor.position_middle_RightDoor.position, doorMoveDuration_first).SetEase(Ease.InOutCubic);

        while (fCurClockBattery >= 0f)
        {
            TruningClockWork_Simple(-50f);
            fCurClockBattery -= Time.deltaTime;
            yield return null;
        }

        DOTween.Kill(gameObject);

        elevaDoor.leftDoor.transform.DOMove(elevaDoor.originalPosition_LeftDoor, doorMoveDuration_first).SetEase(Ease.OutCubic);
        elevaDoor.rightDoor.transform.DOMove(elevaDoor.originalPosition_RightDoor, doorMoveDuration_first).SetEase(Ease.OutCubic);

        TurnOffObj();
    }




    private IEnumerator OpenAndCloseDoors()
    {
        float fTime = 0;

        // 1. 문을 중간 위치로 천천히 이동 (문이 열리는 중간 단계)
        elevaDoor.leftDoor.transform.DOMove(elevaDoor.position_middle_LeftDoor.position, doorMoveDuration_first).SetEase(Ease.InOutCubic);
        elevaDoor.rightDoor.transform.DOMove(elevaDoor.position_middle_RightDoor.position, doorMoveDuration_first).SetEase(Ease.InOutCubic);

        while(fTime < doorMoveDuration_first)
        {
            fTime += Time.deltaTime;
            TruningClockWork_Simple(-50f);
            fCurClockBattery -= Time.deltaTime;
            yield return null;
        }

        // 스캔을 찍지 않았을 때
        if (!bScan)
        {
            elevaDoor.leftDoor.transform.DOShakePosition(shakeDuration, new Vector3(shakeStrength, 0, 0), 10, 0, false, true);
            elevaDoor.rightDoor.transform.DOShakePosition(shakeDuration, new Vector3(-shakeStrength, 0, 0), 10, 0, false, true);
            TruningClockWork_Shake(shakeDuration, shakeStrength);

            yield return new WaitForSeconds(shakeDuration);

            elevaDoor.leftDoor.transform.DOMove(elevaDoor.originalPosition_LeftDoor, doorMoveDuration_second).SetEase(Ease.OutCubic);
            elevaDoor.rightDoor.transform.DOMove(elevaDoor.originalPosition_RightDoor, doorMoveDuration_second).SetEase(Ease.OutCubic);

           
        }
        else
        {
            fTime = 0f;
            while (fTime < 1.8f)
            {
                fTime += Time.deltaTime;
                TruningClockWork_Simple(-50f);
                fCurClockBattery -= Time.deltaTime;
                yield return null;
            }

            elevaDoor.leftDoor.transform.DOMove(elevaDoor.position_target_LeftDoor.position, doorMoveDuration_second).SetEase(Ease.OutCubic);
            elevaDoor.rightDoor.transform.DOMove(elevaDoor.position_target_RightDoor.position, doorMoveDuration_second).SetEase(Ease.OutCubic);
        }

        fTime = 0;
        while (fTime < doorMoveDuration_second)
        {
            fTime += Time.deltaTime;
            TruningClockWork_Simple(-400f);
            fCurClockBattery = 0f;
            yield return null;
        }

        TurnOffObj();

    }



    public override void TurnOffObj()
    {
        base.TurnOffObj();
    }




}
