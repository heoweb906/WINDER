using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseStartBattery : ClockBattery
{
    private Coroutine nowCoroutine;

    public CineCameraChager changer_1;
    public CineCameraChager changer_2;

    [SerializeField]
    private GGILICK_ChaseManager chaseManager;

    public override void TurnOnObj()
    {
        base.TurnOnObj();

        RotateObject((int)fCurClockBattery + 2);
        nowCoroutine = StartCoroutine(ChaseStart());
        

    }
    public override void TurnOffObj()
    {
        if (nowCoroutine != null) StopCoroutine(nowCoroutine);


        chaseManager.ChaseStart();
        base.TurnOffObj();

    }

    IEnumerator ChaseStart()
    {
        changer_1.CameraChange();

        while (fCurClockBattery > 0)
        {

            yield return new WaitForSecondsRealtime(1.0f);

            fCurClockBattery -= 1;
        }

        yield return new WaitForSecondsRealtime(1.0f);

        changer_2.CameraChange();

        yield return new WaitForSecondsRealtime(2.0f);

        TurnOffObj(); 
    }

}
