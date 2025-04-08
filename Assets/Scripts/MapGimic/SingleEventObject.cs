using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleEventObject : InteractableObject
{
    public SingleEventType singleEventType;

    public virtual void Start()
    {
        type = InteractableType.SingleEvent;
    }
}

public enum SingleEventType
{
    WheelChair,
    Ggilick
}


