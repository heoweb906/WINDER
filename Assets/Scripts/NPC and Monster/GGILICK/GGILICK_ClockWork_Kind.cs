using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GGILICK_ClockWork_Kind : SingleEventObject
{
    public override void Start()
    {
        base.Start();
        singleEventType = SingleEventType.Ggilick;
    }

    public override void ActiveEvent()
    {
        canInteract = false;

        InsideAssist.Instance.StartDirect_2();

    }
}
