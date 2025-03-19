using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TestObject_Clock : ClockBattery
{
    Coroutine nowCoroutine;

    public override void TurnOnObj()
    {
        base.TurnOnObj();

        nowCoroutine = StartCoroutine(MoveForwardWithAcceleration());
        RotateObject((int)fCurClockBattery);
    }

    public override void TurnOffObj()
    {
        base.TurnOffObj();
    }


    private IEnumerator MoveForwardWithAcceleration()
    {
      

        while (fCurClockBattery > 0)
        {
            fCurClockBattery -= Time.deltaTime;
            yield return null;
        }

        TurnOffObj();
    }

}
