using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_ClockWork : ClockWork
{
    public NPCHeart npcHeart;

    

    
    public override void GrapClockWorkOn()
    {
        npcHeart.machine.OnStateChange(npcHeart.machine.GrappedState);

        npcHeart.GetAnimator().SetTrigger("doClockWorkStop");



    }






}
