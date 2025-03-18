using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseStartBattery : ClockBattery
{
    private Coroutine nowCoroutine;
    
    [SerializeField]
    private GGILICK_ChaseManager chaseManager;

    public override void TurnOnObj()
    {
        base.TurnOnObj();

        RotateObject((int)fCurClockBattery);
        nowCoroutine = StartCoroutine(ChaseStart());
        chaseManager.ChaseStart();

    }
    public override void TurnOffObj()
    {
        if (nowCoroutine != null) StopCoroutine(nowCoroutine);

        base.TurnOffObj();

    }

    IEnumerator ChaseStart()
    {
        while (fCurClockBattery > 0)
        {

            yield return new WaitForSecondsRealtime(1.0f);

            fCurClockBattery -= 1;
        }

        yield return new WaitForSecondsRealtime(1.0f);

        TurnOffObj(); // 배터리가 다 되면 종료
    }

}
