using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Toy_Train : ClockBattery
{
    public float fSpeed;

    private Rigidbody rb;
    private Coroutine nowCoroutine;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void TurnOnObj()
    {
        base.TurnOnObj();

        RotateObject((int)fCurClockBattery);
        nowCoroutine = StartCoroutine(MoveForward());
    }
    public override void TurnOffObj()
    {
        base.TurnOffObj();
    }


    IEnumerator MoveForward()
    {
        float currentSpeed = fSpeed;

        while (fCurClockBattery > 0)
        {
            if (fCurClockBattery < fLowClockBatteryPoint)
            {
                currentSpeed = Mathf.Lerp(0, fSpeed, fCurClockBattery / fLowClockBatteryPoint);
            }

            transform.position += transform.forward * currentSpeed  * Time.deltaTime;
            fCurClockBattery -= Time.deltaTime;

            yield return null;
        }

        TurnOffObj();


    }



}
