using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KyungsooClockWork_1 : ClockWork
{
    public AttackGyungsoo gyungsoo;




    public override void GrapClockWorkOn()
    {
        Debug.Log("������");

        canInteract = false;
        gyungsoo.CutScencStart();

   
    }   
   
    
}
