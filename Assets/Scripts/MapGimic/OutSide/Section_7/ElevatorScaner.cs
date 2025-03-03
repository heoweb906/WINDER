using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorScaner : InteractableObject
{
    public Elevator elevator;

    private void Start()
    {
        canInteract = true;
        type = InteractableType.SingleEvent;
    }

    override public void ActiveEvent()
    {
        elevator.bScan = true;
        canInteract = false;
    }

}
