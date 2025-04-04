using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ClockBattery : MonoBehaviour
{
    public GameObject clockWork;
    [Header("배터리")]
    public float fMaxClockBattery;
    public float fCurClockBattery;
    public float fLowClockBatteryPoint;

    public bool bDoing;
    public bool bBatteryFull;

    public virtual void TurnOnObj()
    {
        Debug.Log("작동 시작");

        bDoing = true;
        clockWork.GetComponent<ClockWork>().canInteract = false; // 보험 (사실 필요없음 )
    }


    public virtual void TurnOffObj()
    {
        Debug.Log("작동 끝");

        bDoing = false;
        if(clockWork != null) clockWork.GetComponent<ClockWork>().canInteract = true;
        bBatteryFull = false;
        fCurClockBattery = 0f;
    }


    // #. 회전 시간에 따라 태엽을 회전, 오브젝트 별로 회전 시간을 다르게 조절하기 위해 TurnOn() 함수에서 사용하지 않음
    protected void RotateObject(int time)
    {
        float rotationAmount = time * -180f;

        clockWork.transform.DORotate(new Vector3(0, 0, rotationAmount), time , RotateMode.LocalAxisAdd)
                 .SetEase(Ease.OutQuad);
    }



    // #. 태엽 기본 회전 함수 (처음에는 빠르게 회전, 서서히 천천히 회전)
    //protected void TurningClockWork()
    //{
    //    Debug.Log("배터리 작동 중!!!!!!!!!");

    //    float batteryRatio = fCurClockBattery / fMaxClockBattery;

    //    float rotationSpeed = Mathf.Lerp(-100f, -20f, 1f - batteryRatio); // 최소 회전 속도 10f, 최대 속도 200f

    //    clockWork.transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    //}

    // #. 태엽의 일반 회전
    protected void TruningClockWork_Simple(float fRoateSpeed)
    {
        clockWork.transform.Rotate(Vector3.forward * fRoateSpeed * Time.deltaTime);
    }







    // #. 태엽을 흔듬 (오작동 연출)
    protected void TruningClockWork_Shake(float fDuration, float dShakeStength = 20f)
    {
        clockWork.transform.DOPunchRotation(new Vector3(0, 0, 5), fDuration, 20, 1);
    }


}
