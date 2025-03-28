using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KyungsooClockWork_1 : ClockWork
{
    public AttackGyungsoo gyungsoo;




    public override void GrapClockWorkOn()
    {
        Debug.Log("¿‚«˚¿Ω");

        canInteract = false;
        gyungsoo.CutScencStart();

   
    }   
   
    
}
