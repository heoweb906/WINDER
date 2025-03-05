using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_ClockWork : ClockWork
{
    public NPCHeart npcHeart;

    public GameObject NPC;

    

    
    public override void GrapClockWorkOn()
    {
        npcHeart.machine.OnStateChange(npcHeart.machine.GrappedState);

     


    }


    public override void ClockWorkRotate(float fRotateDirection = 1f, float fRotateSpeed_Wall = 0.3f, float fRotateSpeed_Floor = 0.8f)
    {
        base.ClockWorkRotate();

        Debug.Log("ÅÂ¿± È¸Àü!!!");

        npcHeart.GetAnimator().SetTrigger("doClockWorkStop");
    }


  



}
