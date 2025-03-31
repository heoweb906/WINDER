using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelChairObject : SingleEventObject
{
    public override void Start()
    {
        base.Start();
        singleEventType = SingleEventType.WheelChair;
    }

}
