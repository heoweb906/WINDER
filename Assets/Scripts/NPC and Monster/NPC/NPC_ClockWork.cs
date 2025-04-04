using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_ClockWork : InteractableObject
{
    public NPC_Simple_StateMachine machine;

    private void Start()
    {
        type = InteractableType.SingleEvent;
    }


    public override void ActiveEvent()
    {
        canInteract = false;

        machine.OnStateChange(machine.GrappedState);
    }



}
