using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricAreaBattery : ClockBattery
{

    private Coroutine nowCoroutine;
    
    [SerializeField]
    private GameObject electricArea;

    public override void TurnOnObj()
    {
        base.TurnOnObj();

        RotateObject((int)fCurClockBattery);
        nowCoroutine = StartCoroutine(SetActiveElectricArea());
        electricArea.SetActive(false);
    }
    public override void TurnOffObj()
    {
        if (nowCoroutine != null) StopCoroutine(nowCoroutine);

        base.TurnOffObj();

        electricArea.SetActive(true);
    }

    IEnumerator SetActiveElectricArea()
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
