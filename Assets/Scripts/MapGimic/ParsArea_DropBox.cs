using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParsArea_DropBox : PartsArea
{
    [SerializeField]
    private Event_BeforeStation_Controller eventController;

    public GameObject Wall;

    public int ItemCount = 0;
    public override void InsertParts(GameObject partsObj)
    {
        eventController.ItemPickUp();
    }
    
    
}
