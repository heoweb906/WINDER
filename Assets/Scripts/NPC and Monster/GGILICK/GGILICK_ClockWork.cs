using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GGILICK_ClockWork : SingleEventObject
{
    public override void Start()
    {
        base.Start();
        singleEventType = SingleEventType.Ggilick;
    }

    //private void Update()
    //{
    //    if(Input.GetKeyDown(KeyCode.Y))
    //    {
    //        InsideAssist.Instance.StartDirect_1();
    //    }
    //}


    public override void ActiveEvent()
    {
        canInteract = false;

        InsideAssist.Instance.StartDirect_1();
       
    }


}
