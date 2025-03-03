using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GGILICK_ClockWork : InteractableObject
{
    
    

    private void Start()
    {
        type = InteractableType.SingleEvent;
    }


    public override void ActiveEvent()
    {
        canInteract = false;

        InsideAssist.Instance.StartDirect_1();
       
    }


}
