using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCHeart : ClockBattery
{
    private Animator npcAnim;
    public NPC_Simple_StateMachine machine;

    private Coroutine nowCoroutine;

  

    public override void TurnOnObj()
    {
        base.TurnOnObj();

        nowCoroutine = StartCoroutine(PowerCharge());
    }
    public override void TurnOffObj()
    {
        base.TurnOffObj();


        machine.OnStateChange(machine.ThankState);

        ClockWork _clockWork = clockWork.GetComponent<ClockWork>();
        _clockWork.canInteract = false; 

    }


    private IEnumerator PowerCharge()
    {
        while (fCurClockBattery > 0)
        {
            fCurClockBattery -= 1;
            yield return new WaitForSecondsRealtime(1.0f); // 1√  ¥Î±‚
        }

        TurnOffObj();
        yield break;
    }


    public Animator GetAnimator()
    {
        return npcAnim;
    }
    public void SetAnimator(Animator anim)
    {
        npcAnim = anim;
    }
}
