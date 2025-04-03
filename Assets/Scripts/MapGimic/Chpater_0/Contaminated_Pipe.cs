using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Contaminated_Pipe : ClockBattery
{
    public GameObject obj;
    private Coroutine nowCoroutine;
   


    public override void TurnOnObj()
    {
        base.TurnOnObj();

        RotateObject((int)fCurClockBattery + 1);
        nowCoroutine = StartCoroutine(PumpStop());
    }
    public override void TurnOffObj()
    {
        base.TurnOffObj();

        obj.SetActive(true);
        if (nowCoroutine != null) StopCoroutine(nowCoroutine);
    }


    IEnumerator PumpStop()
    {
        yield return new WaitForSeconds(0.4f); // 1초 대기

        obj.SetActive(false);
        while (fCurClockBattery > 0)
        {
            yield return new WaitForSeconds(1.0f); // 1초 대기
            fCurClockBattery -= 1;
        }

 
   
        TurnOffObj();
        yield break;
    }
}
