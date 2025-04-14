using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBlock_Battery : ClockBattery
{
    public bool bIsUp;

    public MoveBlock_Main moveBlock_Main;

    public override void TurnOnObj()
    {
        base.TurnOnObj();

        RotateObject((int)fCurClockBattery);
        moveBlock_Main.SetBatteryMaxCharge((int)fMaxClockBattery - (int)fCurClockBattery);
        StartCoroutine(SetAddMoveBlock());
    }

    public override void TurnOffObj()
    {
        base.TurnOffObj();
        if((int)fMaxClockBattery <= 0)
        {
            clockWork.GetComponent<ClockWork>().canInteract = false;
        }
    }

    IEnumerator SetAddMoveBlock()
    {
        while (fCurClockBattery > 0)
        {

            yield return new WaitForSeconds(1.0f);
            moveBlock_Main.AddInputList(bIsUp ? 0 : 1);

            fCurClockBattery -= 1;
        }

        yield return new WaitForSeconds(1.0f);

        TurnOffObj(); // 배터리가 다 되면 종료
    }
    
    

}
