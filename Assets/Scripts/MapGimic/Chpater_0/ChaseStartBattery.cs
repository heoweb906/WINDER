using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ChaseStartBattery : ClockBattery
{
    private Coroutine nowCoroutine;

    public CineCameraChager changer_1;
    public CineCameraChager changer_2;

    [Header("������ ����")]
    public HandheldCamera handHeld;
    public CameraEvent cameraEvent;

    [SerializeField]
    private GGILICK_ChaseManager chaseManager;

    public override void TurnOnObj()
    {
        base.TurnOnObj();

        RotateObject((int)fCurClockBattery + 2);
        nowCoroutine = StartCoroutine(ChaseStart_1());
    }
    public override void TurnOffObj()
    {
        if (nowCoroutine != null) StopCoroutine(nowCoroutine);

        GameAssistManager.Instance.PlayerInputLockOff();

        nowCoroutine = StartCoroutine(ChaseStart_2());


        bDoing = false;
        if (clockWork != null) clockWork.GetComponent<ClockWork>().canInteract = false;
        bBatteryFull = false;
        fCurClockBattery = 0f;
    }


    IEnumerator ChaseStart_1()
    {
        changer_1.CameraChange();



        while (fCurClockBattery > 0)
        {

            yield return new WaitForSeconds(1.0f);

            fCurClockBattery -= 1;
        }

        yield return new WaitForSeconds(1.0f);

        changer_2.CameraChange();

        yield return new WaitForSeconds(2.0f);

        TurnOffObj(); 
    }



    IEnumerator ChaseStart_2()
    {
        yield return new WaitForSeconds(2.0f);

        cameraEvent.CameraTriggerStart(4);

        yield return new WaitForSeconds(2.1f);


        chaseManager.ChaseStart();
    }

}
